using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TraktRater.Logger;
using TraktRater.Settings;
using TraktRater.Sites.API.MovieLens;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.UI;

namespace TraktRater.Sites
{
    public class MovieLens : IRateSite
    {
        #region Variables

        private readonly bool mImportRatings;
        private readonly bool mImportActivities;
        private readonly bool mImportTags;
        private readonly bool mImportWishlist;

        private readonly string mRatingsFilename;
        private readonly string mActivityFilename;
        private readonly string mTagsFilename;
        private readonly string mWishlistFilename;

        private bool mImportCancelled;
        private readonly CsvConfiguration mCsvConfiguration = new CsvConfiguration();

        #endregion
        public MovieLens(string aRatingsFilename, string aActivityFilename, string aTagsFilename, string aWishListFilename)
        {
            mRatingsFilename = aRatingsFilename;
            if (!string.IsNullOrEmpty(mRatingsFilename))
            {
                mImportRatings = File.Exists(mRatingsFilename);
            }

            mActivityFilename = aActivityFilename;
            if (!string.IsNullOrEmpty(mActivityFilename))
            {
                mImportActivities = File.Exists(mActivityFilename);
            }

            mTagsFilename = aTagsFilename;
            if (!string.IsNullOrEmpty(mTagsFilename))
            {
                mImportTags = File.Exists(mTagsFilename);
            }

            mWishlistFilename = aWishListFilename;
            if (!string.IsNullOrEmpty(mWishlistFilename))
            {
                mImportWishlist = File.Exists(mWishlistFilename);
            }

            Enabled = mImportRatings | mImportActivities | mImportTags | mImportWishlist;

            if (Enabled) SetCSVHelperOptions();
        }

        public string Name
        {
            get { return "MovieLens"; }
        }

        public bool Enabled { get; set; }

        public void Cancel()
        {
            mImportCancelled = true;
        }

        public void ImportRatings()
        {
            List<MovieLensRatingItem> lRatings;
            List<MovieLensActivityItem.MovieRatingActivity> lRatingActivities = new List<MovieLensActivityItem.MovieRatingActivity>();

            if (mImportCancelled) return;

            if (mImportActivities)
            {
                // the activity log gives us some more detail when importing 
                // such as the date/time a movie was rated
                var lActivities = ParseMovieActivitiesCsv();
                UIUtils.UpdateStatus($"Found {lActivities.Count} activities in Movie Lens Activity Log CSV file");

                // select 'rating' activities 
                lRatingActivities = lActivities.Where(a => a.ActionType == "rating").Select(r => r.ToRatingActivity()).ToList();
                UIUtils.UpdateStatus($"Found {lRatingActivities.Count} rating activities in Movie Lens Activity Log CSV file");
            }

            if (mImportRatings)
            {
                lRatings = ParseMovieRatingsCsv();
                UIUtils.UpdateStatus($"Found {lRatings.Count} ratings in Movie Lens Rating CSV file");

                var lRatedMovies = lRatings.Where(tdm => tdm.Rating > 0)
                                           .Select(tdm => tdm.ToTraktRatedMovie(lRatingActivities.FirstOrDefault(r => r.MovieId == tdm.MovieId))).ToList();

                if (lRatedMovies.Any())
                {
                    AddMoviesToRatings(lRatedMovies);
                    if (mImportCancelled) return;

                    // mark rated movies as watched
                    if (AppSettings.MarkAsWatched)
                    {
                        var lWatchedMovies = lRatings.Where(tdm => tdm.Rating > 0)
                                                     .Select(tdm => tdm.ToTraktWatchedMovie(lRatingActivities.FirstOrDefault(r => r.MovieId == tdm.MovieId))).ToList();

                        if (lWatchedMovies.Any())
                        {
                            AddMoviesToWatchedHistory(lWatchedMovies);
                            if (mImportCancelled) return;
                        }
                    }
                }
            }
        }

        private void AddMoviesToWatchedHistory(List<TraktMovieWatched> aWatchedMovies)
        {
            // get watched movies from trakt.tv
            UIUtils.UpdateStatus("Requesting watched movies from trakt...");
            var lWatchedTraktMovies = TraktAPI.TraktAPI.GetWatchedMovies();
            if (lWatchedTraktMovies == null)
            {
                UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv, skipping watched movie import", true);
                Thread.Sleep(2000);
            }
            else
            {
                if (mImportCancelled) return;

                UIUtils.UpdateStatus($"Found {lWatchedTraktMovies.Count()} watched movies on trakt");
                UIUtils.UpdateStatus("Filtering out watched movies that are already watched on trakt.tv");

                aWatchedMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId || t.Movie.Ids.TmdbId == w.Ids.TmdbId) != null);

                // mark all rated movies as watched
                UIUtils.UpdateStatus($"Importing {aWatchedMovies.Count} Movie Lens movies as watched...");

                int lPageSize = AppSettings.BatchSize;
                int lPages = (int)Math.Ceiling((double)aWatchedMovies.Count / lPageSize);
                for (int i = 0; i < lPages; i++)
                {
                    UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} Movie Lens movies as watched...");

                    var lMoviesToSync = new TraktMovieWatchedSync()
                    {
                        Movies = aWatchedMovies.Skip(i * lPageSize).Take(lPageSize).ToList()
                    };

                    var lResponse = TraktAPI.TraktAPI.AddMoviesToWatchedHistory(lMoviesToSync);
                    if (lResponse == null)
                    {
                        UIUtils.UpdateStatus("Failed to send watched status for Movie Lens movies to trakt.tv", true);
                        Thread.Sleep(2000);
                    }
                    else if (lResponse.NotFound.Movies.Count > 0)
                    {
                        UIUtils.UpdateStatus($"Unable to sync watched states for {lResponse.NotFound.Movies.Count} movies as they're not found on trakt.tv!");
                        Thread.Sleep(1000);
                    }
                    if (mImportCancelled) return;
                }
            }
        }

        private void AddMoviesToRatings(List<TraktMovieRating> aRatedMovies)
        {
            // filter out movies that are already rated
            // get rated movies from trakt.tv
            UIUtils.UpdateStatus("Requesting rated movies from trakt...");
            var lRatedTraktMovies = TraktAPI.TraktAPI.GetRatedMovies();
            if (lRatedTraktMovies == null)
            {
                UIUtils.UpdateStatus("Failed to get rated movies from trakt.tv, skipping rated movie import", true);
                Thread.Sleep(2000);
                return;
            }
            
            if (mImportCancelled) return;

            UIUtils.UpdateStatus("Found {0} rated movies on trakt", lRatedTraktMovies.Count());
            UIUtils.UpdateStatus("Filtering out rated movies that are already rated on trakt.tv");

            aRatedMovies.RemoveAll(w => lRatedTraktMovies.FirstOrDefault(t => t.Movie.Ids.TmdbId == w.Ids.TmdbId) != null);

            UIUtils.UpdateStatus("Importing {0} Movie Lens ratings...", aRatedMovies.Count);

            if (aRatedMovies.Count() > 0)
            {
                int lPageSize = AppSettings.BatchSize;
                int lPages = (int)System.Math.Ceiling((double)aRatedMovies.Count() / lPageSize);
                for (int i = 0; i < lPages; i++)
                {
                    if (mImportCancelled) return;

                    UIUtils.UpdateStatus("Importing page {0}/{1} Movie Lens ratings...", i + 1, lPages);

                    var lRatingsToSync = new TraktMovieRatingSync()
                    {
                        movies = aRatedMovies.Skip(i * lPageSize).Take(lPageSize).ToList()
                    };

                    var lResponse = TraktAPI.TraktAPI.AddMoviesToRatings(lRatingsToSync);
                    HandleResponse(lResponse);
                }
            }
        }

        private void HandleResponse(TraktSyncResponse aResponse)
        {
            if (aResponse == null)
            {
                UIUtils.UpdateStatus("Error importing Movie Lens list to trakt.tv", true);
                Thread.Sleep(2000);
            }
            else if (aResponse.NotFound.Movies.Count > 0)
            {
                UIUtils.UpdateStatus("Unable to process {0} movies as they're not found on trakt.tv!", aResponse.NotFound.Movies.Count);
                foreach (var movie in aResponse.NotFound.Movies)
                {
                    UIUtils.UpdateStatus("Unable to process movie: Title = {0}, TMDb = {1}", movie.Title, movie.Ids.TmdbId);
                }
                Thread.Sleep(1000);
            }
        }

        private List<MovieLensRatingItem> ParseMovieRatingsCsv()
        {
            mCsvConfiguration.RegisterClassMap<CSVRatingsFileDefinitionMap>();

            UIUtils.UpdateStatus("Parsing Movie Lens Ratings CSV file");
            var textReader = File.OpenText(mRatingsFilename);

            var csv = new CsvReader(textReader, mCsvConfiguration);
            return csv.GetRecords<MovieLensRatingItem>().ToList();
        }

        private List<MovieLensActivityItem> ParseMovieActivitiesCsv()
        {
            mCsvConfiguration.RegisterClassMap<CSVActivityFileDefinitionMap>();

            UIUtils.UpdateStatus("Parsing Movie Lens Activity Log CSV file");
            var textReader = File.OpenText(mActivityFilename);

            var csv = new CsvReader(textReader, mCsvConfiguration);
            return csv.GetRecords<MovieLensActivityItem>().ToList();
        }

        private void SetCSVHelperOptions()
        {
            mCsvConfiguration.IsHeaderCaseSensitive = false;

            // MovieLens use "." for decimal seperator so set culture to cater for this            
            mCsvConfiguration.CultureInfo = new System.Globalization.CultureInfo("en-US");

            // if we're unable parse a row, log the details for analysis
            mCsvConfiguration.IgnoreReadingExceptions = true;
            mCsvConfiguration.ReadingExceptionCallback = (ex, row) =>
            {
                FileLog.Error($"Error reading row '{ex.Data["CsvHelper"]}'");
            };
        }
    }
}

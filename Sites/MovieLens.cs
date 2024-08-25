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
        private readonly CsvConfiguration mCsvConfiguration = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));

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

            // tags are currently ignored
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
            if (mImportCancelled) return;

            List<MovieLensRatingItem> lRatings;
            List<MovieLensWishlistItem> lWishlist;
            
            List<MovieLensActivityItem.ActivityDate> lRatingActivities = new List<MovieLensActivityItem.ActivityDate>();
            List<MovieLensActivityItem.ActivityDate> lUserListActivities = new List<MovieLensActivityItem.ActivityDate>();

            if (mImportActivities)
            {
                // the activity log gives us some more detail when importing 
                // such as the date/time a movie was rated
                var lActivities = ParseMovieActivitiesCsv();
                UIUtils.UpdateStatus($"Found {lActivities.Count} activities in Movie Lens Activity Log CSV file");

                // select 'rating' activities 
                lRatingActivities = lActivities.Where(a => a.ActionType == "rating").Select(r => r.ToRatingActivity()).ToList();
                UIUtils.UpdateStatus($"Found {lRatingActivities.Count} rating activities in Movie Lens Activity Log CSV file");

                // select 'user-list' activities
                // NB: presumably there is future support for other list types, wishlist appears to have listId == 1
                // we may need to update this in the future to filter by list Id
                lUserListActivities = lActivities.Where(a => a.ActionType == "user-list").Select(w => w.ToUserListActivity()).ToList();
                UIUtils.UpdateStatus($"Found {lUserListActivities.Count} user-list activities in Movie Lens Activity Log CSV file");
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

            if (mImportWishlist)
            {
                lWishlist = ParseMovieWishlistCsv();
                UIUtils.UpdateStatus($"Found {lWishlist.Count} wishlist items in Movie Lens Wishlist CSV file");

                var lWishlistedMovies = lWishlist.Select(tdm => tdm.ToTraktWatchlistMovie()).ToList();

                if (lWishlistedMovies.Any())
                {
                    AddMoviesToWatchlist(lWishlistedMovies);
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

        private void AddMoviesToWatchlist(List<TraktMovie> aWatchlistMovies)
        {
            // filter out already watchlisted items
            UIUtils.UpdateStatus("Requesting existing watchlisted movies from trakt...");
            var lWatchlistTraktMovies = TraktAPI.TraktAPI.GetWatchlistMovies();
            if (lWatchlistTraktMovies == null)
            {
                UIUtils.UpdateStatus("Failed to get watchlisted movies from trakt.tv, skipping watchlisted movie import", true);
                Thread.Sleep(2000);
                return;
            }

            UIUtils.UpdateStatus($"Found {lWatchlistTraktMovies.Count()} watchlist movies on trakt");
            UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
            aWatchlistMovies.RemoveAll(w => lWatchlistTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId ||
                                                                                      t.Movie.Ids.TmdbId == w.Ids.TmdbId) != null);

            // further filter out any watched items (optional)
            if (AppSettings.IgnoreWatchedForWatchlist && aWatchlistMovies.Count > 0)
            {
                if (mImportCancelled) return;

                // get watched movies from trakt so we don't import movies into watchlist that are already watched
                // NB: we may have already done this, but let's do it again in case we have added more from ratings                

                UIUtils.UpdateStatus("Requesting watched movies from trakt...");
                var lWatchedTraktMovies = TraktAPI.TraktAPI.GetWatchedMovies();
                if (lWatchedTraktMovies == null)
                {
                    UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv", true);
                    Thread.Sleep(2000);
                }
                else
                {
                    UIUtils.UpdateStatus($"Found {lWatchedTraktMovies.Count()} watched movies on trakt");
                    UIUtils.UpdateStatus("Filtering out watchlist movies that are watched on trakt.tv");

                    // remove movies from sync list which are watched already
                    aWatchlistMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId ||
                                                                                            t.Movie.Ids.TmdbId == w.Ids.TmdbId) != null);
                }
            }

            int lPageSize = AppSettings.BatchSize;
            int lPages = (int)Math.Ceiling((double)aWatchlistMovies.Count / lPageSize);
            for (int i = 0; i < lPages; i++)
            {
                if (mImportCancelled) return;

                UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} Movie Lens movies into watchlist...");                

                var lMoviesToSync = new TraktMovieSync()
                {
                    Movies = aWatchlistMovies.Skip(i * lPageSize).Take(lPageSize).ToList()
                };

                var lResponse = TraktAPI.TraktAPI.AddMoviesToWatchlist(lMoviesToSync);
                if (lResponse == null)
                {
                    UIUtils.UpdateStatus("Failed to send watchlist for Movie Lens movies to trakt.tv", true);
                    Thread.Sleep(2000);
                }
                else if (lResponse.NotFound.Movies.Count > 0)
                {
                    UIUtils.UpdateStatus($"Unable to sync watchlist for {lResponse.NotFound.Movies.Count} movies as they're not found on trakt.tv!");
                    Thread.Sleep(1000);
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
            UIUtils.UpdateStatus("Parsing Movie Lens Ratings CSV file");
            var textReader = File.OpenText(mRatingsFilename);

            var csv = new CsvReader(textReader, mCsvConfiguration);
            csv.Context.RegisterClassMap<CSVRatingsFileDefinitionMap>();
            return csv.GetRecords<MovieLensRatingItem>().ToList();
        }

        private List<MovieLensWishlistItem> ParseMovieWishlistCsv()
        {
            UIUtils.UpdateStatus("Parsing Movie Lens Wishlist CSV file");
            var textReader = File.OpenText(mWishlistFilename);

            var csv = new CsvReader(textReader, mCsvConfiguration);
            csv.Context.RegisterClassMap<CSVWishlistFileDefinitionMap>();
            return csv.GetRecords<MovieLensWishlistItem>().ToList();
        }

        private List<MovieLensActivityItem> ParseMovieActivitiesCsv()
        {
            UIUtils.UpdateStatus("Parsing Movie Lens Activity Log CSV file");
            var textReader = File.OpenText(mActivityFilename);

            var csv = new CsvReader(textReader, mCsvConfiguration);
            csv.Context.RegisterClassMap<CSVActivityFileDefinitionMap>();
            return csv.GetRecords<MovieLensActivityItem>().ToList();
        }

        private void SetCSVHelperOptions()
        {
            // if we're unable parse a row, log the details for analysis
            mCsvConfiguration.ReadingExceptionOccurred = args =>
            {
                FileLog.Error($"Error reading row '{args.Exception}'");
                return true;
            };
        }
    }
}

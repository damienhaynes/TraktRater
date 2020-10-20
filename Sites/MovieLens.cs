using CsvHelper;
using CsvHelper.Configuration;
using global::TraktRater.Settings;
using global::TraktRater.Sites.API.MovieLens;
using global::TraktRater.TraktAPI.DataStructures;
using global::TraktRater.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

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
            List<MovieLensItem> lRatings;

            if (mImportCancelled) return;

            if (mImportRatings)
            {
                lRatings = ParseMovieListRatingsCsv();
                var lRatedMovies = lRatings.Where(tdm => tdm.Rating > 0).Select(tdm => tdm.ToTraktRatedMovie()).ToList();
                if (lRatedMovies.Any())
                {
                    AddMoviesToRatings(lRatedMovies);
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

        private List<MovieLensItem> ParseMovieListRatingsCsv()
        {
            mCsvConfiguration.RegisterClassMap<CSVFileDefinitionMap>();

            UIUtils.UpdateStatus("Parsing Movie Lens Ratings CSV file");
            var textReader = File.OpenText(mRatingsFilename);

            var csv = new CsvReader(textReader, mCsvConfiguration);
            return csv.GetRecords<MovieLensItem>().ToList();
        }
    }
}

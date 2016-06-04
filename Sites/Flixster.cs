namespace TraktRater.Sites
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using global::TraktRater.Extensions;
    using global::TraktRater.Logger;
    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.Flixster;
    using global::TraktRater.TraktAPI;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;

    internal class Flixster : IRateSite
    {
        #region Variables

        bool mImportCancelled = false;
        string mUserId = null;

        #endregion

        public Flixster(string userId)
        {
            mUserId = userId;
            Enabled = !string.IsNullOrEmpty(userId);
        }

        #region IRateSite Members

        public string Name
        {
            get { return "Flixster"; }
        }

        public bool Enabled { get; set; }

        public void ImportRatings()
        {
            mImportCancelled = false;

            var lAllMovieRatings = new List<FlixsterMovieRating>();

            #region Get Rated Movies
            int lPageSize = 50;

            UIUtils.UpdateStatus("Getting page 1 of Flixster rated movies...");
            var lPagedMovieRatings = FlixsterAPI.GetRatedMovies(mUserId.ToString(), 1, lPageSize);
            if (mImportCancelled) return;

            if (lPagedMovieRatings == null)
            {
                UIUtils.UpdateStatus("Failed to get movie ratings from Flixster", true);
                Thread.Sleep(2000);
                return;
            }

            // see if there are any more movie ratings to request
            // we don't get back a number of pages so need to manually work it out
            if (lPagedMovieRatings.Count() >= lPageSize)
            {
                lAllMovieRatings.AddRange(lPagedMovieRatings);

                int lPage = 2;
                bool lRequestMore = true;

                while (lRequestMore)
                {
                    UIUtils.UpdateStatus("Getting page {0} of Flixster rated movies...", lPage);
                    lPagedMovieRatings = FlixsterAPI.GetRatedMovies(mUserId.ToString(), lPage++, lPageSize);
                    if (lPagedMovieRatings == null)
                    {
                        lRequestMore = false;
                        UIUtils.UpdateStatus(string.Format("Failed to get movie ratings on page {0} from Flixster", lPage - 1), true);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        // when we request another page, if there are no more movies, it returns the same ones again
                        // we can just check if the first movie returned already exists in our collection
                        if (lAllMovieRatings.Exists(r => r.Movie.Title == lPagedMovieRatings.First().Movie.Title && r.Movie.Year == lPagedMovieRatings.First().Movie.Year))
                        {
                            lRequestMore = false;
                        }
                        else
                        {
                            lAllMovieRatings.AddRange(lPagedMovieRatings);

                            // check if there is any more pages worth requesting based on size returned in last batch
                            if (lPagedMovieRatings.Count() < lPageSize)
                                lRequestMore = false;
                        }
                    }
                }
            }

            #endregion

            #region Import Rated Movies
            FileLog.Info("Found {0} movie ratings on Flixster", lAllMovieRatings.Count);
            if (lAllMovieRatings.Any())
            {
                var lMovieRatings = new List<FlixsterMovieRating>(lAllMovieRatings);

                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var lCurrentUserMovieRatings = TraktAPI.GetRatedMovies();

                if (lCurrentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user movie ratings on trakt.tv", lCurrentUserMovieRatings.Count());
                    lMovieRatings.RemoveAll(r => lCurrentUserMovieRatings.Any(c => c.Movie.Title.ToLowerInvariant() == r.Movie.Title.ToLowerInvariant() && c.Movie.Year == r.Movie.Year));
                }

                UIUtils.UpdateStatus("Importing {0} new movie ratings to trakt.tv", lMovieRatings.Count());

                if (lMovieRatings.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lMovieRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} Flixster rated movies...", i + 1, pages);

                        TraktSyncResponse response = TraktAPI.AddMoviesToRatings(GetRateMoviesData(lMovieRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Error importing Flixster movie ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Mark Rated Items as Watched
            IEnumerable<TraktMoviePlays> lWatchedTraktMovies = null;

            if (AppSettings.MarkAsWatched && lAllMovieRatings.Count > 0)
            {
                var lMoviesWatched = new List<FlixsterMovieRating>(lAllMovieRatings);

                // get watched movies from trakt.tv
                UIUtils.UpdateStatus("Requesting watched movies from trakt.tv...");
                lWatchedTraktMovies = TraktAPI.GetWatchedMovies();
                if (lWatchedTraktMovies == null)
                {
                    UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv, skipping watched movie import", true);
                    Thread.Sleep(2000);
                }
                else
                {
                    if (mImportCancelled) return;

                    UIUtils.UpdateStatus("Found {0} watched movies on trakt", lWatchedTraktMovies.Count());
                    UIUtils.UpdateStatus("Filtering out watched movies that are already watched on trakt.tv");

                    lMoviesWatched.RemoveAll(w => lWatchedTraktMovies.Any(t => t.Movie.Title.ToLowerInvariant() == w.Movie.Title.ToLowerInvariant() && t.Movie.Year == w.Movie.Year));

                    // mark all rated movies as watched
                    UIUtils.UpdateStatus("Importing {0} Flixster movies as watched...", lMoviesWatched.Count);

                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lMoviesWatched.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb movies as watched...", i + 1, pages);

                        var response = TraktAPI.AddMoviesToWatchedHistory(GetSyncWatchedMoviesData(lMoviesWatched.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send watched status for Flixster movies to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync watched states for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }
                        if (mImportCancelled) return;
                    }
                }
            }
            #endregion
        }

        public void Cancel()
        {
            mImportCancelled = true;
        }

        #endregion

        #region Private Methods

        private TraktMovieRatingSync GetRateMoviesData(List<FlixsterMovieRating> aRatedItems)
        {
            var traktMovies = new List<TraktMovieRating>();

            traktMovies.AddRange(from ratedItem in aRatedItems
                                 select new TraktMovieRating
                                 {
                                     Title = ratedItem.Movie.Title,
                                     Year =  ratedItem.Movie.Year,
                                     Rating = (int)Math.Ceiling(float.Parse(ratedItem.UserScore, CultureInfo.InvariantCulture.NumberFormat) * 2),
                                     RatedAt = GetDateUpdated(ratedItem)
                                 });

            var movieRateData = new TraktMovieRatingSync
            {
                movies = traktMovies
            };

            return movieRateData;
        }

        private TraktMovieWatchedSync GetSyncWatchedMoviesData(List<FlixsterMovieRating> aRatedItems)
        {
            var traktMovies = new List<TraktMovieWatched>();

            traktMovies.AddRange(from ratedItem in aRatedItems
                                 select new TraktMovieWatched
                                 {
                                     Title = ratedItem.Movie.Title,
                                     Year = ratedItem.Movie.Year,
                                     WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : GetDateUpdated(ratedItem)
                                 });

            var movieSyncData = new TraktMovieWatchedSync
            {
                Movies = traktMovies
            };

            return movieSyncData;
        }

        private string GetDateUpdated(FlixsterMovieRating aRatedItem)
        {
            // Flixster stores dates using a string in the form:
            // <value> <unit> ago e.g.
            // 1 minute ago
            // 10 minutes ago
            // 1 day ago
            // 5 days ago
            // 7 years ago

            string lLastUpdated = aRatedItem.LastUpdated.ToLowerInvariant();

            string[] lLastUpdatedParts = lLastUpdated.Split(' ');

            if (lLastUpdated.Contains("minute"))
            {
                return DateTime.UtcNow.Subtract(new TimeSpan(0, int.Parse(lLastUpdatedParts[0]), 0)).ToString().ToISO8601();
            }
            else if (lLastUpdated.Contains("hour"))
            {
                return DateTime.UtcNow.Subtract(new TimeSpan(int.Parse(lLastUpdatedParts[0]), 0, 0)).ToString().ToISO8601();
            }
            else if (lLastUpdated.Contains("day"))
            {
                return DateTime.UtcNow.Subtract(new TimeSpan(int.Parse(lLastUpdatedParts[0]), 0, 0, 0)).ToString().ToISO8601();
            }
            else if (lLastUpdated.Contains("week"))
            {
                return DateTime.UtcNow.Subtract(new TimeSpan(int.Parse(lLastUpdatedParts[0]) * 7, 0, 0, 0)).ToString().ToISO8601();
            }
            else if (lLastUpdated.Contains("month"))
            {
                return DateTime.UtcNow.Subtract(new TimeSpan(int.Parse(lLastUpdatedParts[0]) * 30, 0, 0, 0)).ToString().ToISO8601();
            }
            else if (lLastUpdated.Contains("year"))
            {
                return DateTime.UtcNow.Subtract(new TimeSpan(int.Parse(lLastUpdatedParts[0]) * 365, 0, 0, 0)).ToString().ToISO8601();
            }

            return DateTime.UtcNow.ToString().ToISO8601();
        }

        #endregion

    }
}

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
        bool mSyncWantToSee = false;

        #endregion

        public Flixster(string aUserId, bool aSyncWantToSee)
        {
            mUserId = aUserId;
            mSyncWantToSee = aSyncWantToSee;
            Enabled = !string.IsNullOrEmpty(aUserId);
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

            var lAllMovies = new List<FlixsterMovieRating>();
            var lAllMovieRatings = new List<FlixsterMovieRating>();
            var lAllMoviesWatchlist = new List<FlixsterMovieRating>();            

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
                lAllMovies.AddRange(lPagedMovieRatings);

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
                    else if (lPagedMovieRatings.Count() > 0)
                    {
                        // when we request another page, if there are no more movies, it *sometimes* returns the same ones again
                        // we can just check if the first movie returned already exists in our collection
                        if (lAllMovies.Exists(r => r.Movie.Title == lPagedMovieRatings.First().Movie.Title && r.Movie.Year == lPagedMovieRatings.First().Movie.Year))
                        {
                            lRequestMore = false;
                        }
                        else
                        {
                            lAllMovies.AddRange(lPagedMovieRatings);

                            // check if there is any more pages worth requesting based on size returned in last batch
                            if (lPagedMovieRatings.Count() < lPageSize)
                                lRequestMore = false;
                        }
                    }
                    else
                    {
                        lRequestMore = false;
                    }
                }
            }

            #endregion

            #region Import Rated Movies
            lAllMovieRatings.AddRange(lAllMovies.Where(m => m.UserScore.IsFloat()));

            FileLog.Info("Found {0} movie ratings on Flixster", lAllMovieRatings.Count);
            if (lAllMovieRatings.Any())
            {
                var lMovieRatings = new List<FlixsterMovieRating>(lAllMovieRatings);

                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var lCurrentUserMovieRatings = TraktAPI.GetRatedMovies();

                if (lCurrentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user movie ratings on trakt.tv", lCurrentUserMovieRatings.Count());
                    lMovieRatings.RemoveAll(r => lCurrentUserMovieRatings.Any(c => c.Movie.Title.ToLowerInvariant() == r.Movie.Title.ToLowerInvariant() && c.Movie.Year == r.Movie.Year.ToYear()));
                }

                UIUtils.UpdateStatus("Importing {0} new movie ratings to trakt.tv", lMovieRatings.Count());

                if (lMovieRatings.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lMovieRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} Flixster rated movies...", i + 1, pages);

                        TraktSyncResponse response = TraktAPI.AddMoviesToRatings(GetSyncRateMoviesData(lMovieRatings.Skip(i * pageSize).Take(pageSize).ToList()));
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
                if (mImportCancelled) return;

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

                    lMoviesWatched.RemoveAll(w => lWatchedTraktMovies.Any(t => t.Movie.Title.ToLowerInvariant() == w.Movie.Title.ToLowerInvariant() && t.Movie.Year == w.Movie.Year.ToYear()));

                    // mark all rated movies as watched
                    UIUtils.UpdateStatus("Importing {0} Flixster movies as watched...", lMoviesWatched.Count);

                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lMoviesWatched.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} Flixster movies as watched...", i + 1, pages);

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

            #region Sync Watchlist

            if (mSyncWantToSee)
            {
                if (mImportCancelled) return;

                lAllMoviesWatchlist.AddRange(lAllMovies.Where(m => m.UserScore == "+"));

                if (lAllMoviesWatchlist.Count > 0)
                {
                    UIUtils.UpdateStatus("Requesting existing watchlist movies from trakt...");
                    var lWatchlistTraktMovies = TraktAPI.GetWatchlistMovies();
                    if (lWatchlistTraktMovies != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watchlist movies on trakt", lWatchlistTraktMovies.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                        lAllMoviesWatchlist.RemoveAll(w => lWatchlistTraktMovies.Any(t => t.Movie.Title.ToLowerInvariant() == w.Movie.Title.ToLowerInvariant() && t.Movie.Year == w.Movie.Year.ToYear()));
                    }
                    if (mImportCancelled) return;

                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Requesting watched movies from trakt...");

                        // get watched movies from trakt so we don't import movies into watchlist that are already watched
                        if (lWatchedTraktMovies == null)
                        {
                            lWatchedTraktMovies = TraktAPI.GetWatchedMovies();
                            if (lWatchedTraktMovies != null)
                            {
                                UIUtils.UpdateStatus("Found {0} watched movies on trakt", lWatchedTraktMovies.Count());

                                // remove movies from sync list which are watched already
                                lAllMoviesWatchlist.RemoveAll(w => lWatchedTraktMovies.Any(t => t.Movie.Title.ToLowerInvariant() == w.Movie.Title.ToLowerInvariant() && t.Movie.Year == w.Movie.Year.ToYear()));
                            }
                        }
                        if (mImportCancelled) return;
                    }

                    // add all movies to watchlist
                    UIUtils.UpdateStatus("Importing {0} Flixster wanttosee movies to trakt.tv watchlist...", lAllMoviesWatchlist.Count());

                    if (lAllMoviesWatchlist.Count > 0)
                    {
                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)lAllMoviesWatchlist.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus("Importing page {0}/{1} Flixster wantlist movies to trakt.tv watchlist...", i + 1, pages);

                            var watchlistMoviesResponse = TraktAPI.AddMoviesToWatchlist(GetSyncMoviesData(lAllMoviesWatchlist.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (watchlistMoviesResponse == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watchlist for Flixster movies", true);
                                Thread.Sleep(2000);
                            }

                            if (mImportCancelled) return;
                        }
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

        private TraktMovieRatingSync GetSyncRateMoviesData(List<FlixsterMovieRating> aRatedItems)
        {
            var traktMovies = new List<TraktMovieRating>();

            traktMovies.AddRange(from ratedItem in aRatedItems
                                 select new TraktMovieRating
                                 {
                                     Title = ratedItem.Movie.Title,
                                     Year =  ratedItem.Movie.Year.ToYear(),
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
                                     Year = ratedItem.Movie.Year.ToYear(),
                                     WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : GetDateUpdated(ratedItem)
                                 });

            var movieSyncData = new TraktMovieWatchedSync
            {
                Movies = traktMovies
            };

            return movieSyncData;
        }

        private TraktMovieSync GetSyncMoviesData(List<FlixsterMovieRating> aMovieItems)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movieItem in aMovieItems
                                 select new TraktMovie
                                 {
                                     Title = movieItem.Movie.Title,
                                     Year = movieItem.Movie.Year.ToYear(),
                                 });

            var movieSyncData = new TraktMovieSync
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

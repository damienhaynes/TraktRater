namespace TraktRater.Sites
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.TMDb;
    using global::TraktRater.TraktAPI;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;

    internal class TMDb : IRateSite
    {
        #region Variables

        bool importCancelled = false;

        #endregion

        #region Constructor

        public TMDb(string requestToken, string sessionId)
        {
            Enabled = !string.IsNullOrEmpty(requestToken) || !string.IsNullOrEmpty(sessionId);
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "TMDb"; } }

        public void ImportRatings()
        {
            importCancelled = false;
            List<TMDbMovie> watchedMovies = new List<TMDbMovie>();

            #region Session Id
            // check if we have a session id
            // note: request token if new is only valid for 60mins
            if (string.IsNullOrEmpty(AppSettings.TMDbSessionId))
            {
                UIUtils.UpdateStatus("Getting TMDb Authentication Session Id...");
                var sessionResponse = TMDbAPI.RequestSessionId(AppSettings.TMDbRequestToken);
                if (sessionResponse == null || !sessionResponse.Success)
                {
                    UIUtils.UpdateStatus("Unable to get TMDb Authentication Session Id.", true);
                    Thread.Sleep(2000);
                    return;
                }
                AppSettings.TMDbSessionId = sessionResponse.SessionId;
            }
            if (importCancelled) return;
            #endregion

            #region Account Information
            UIUtils.UpdateStatus("Getting TMDb Account Id...");
            var accountInfo = TMDbAPI.GetAccountId(AppSettings.TMDbSessionId);
            if (accountInfo == null)
            {
                UIUtils.UpdateStatus("Unable to get TMDb Account Id.", true);
                Thread.Sleep(2000);
                return;
            }
            if (importCancelled) return;
            #endregion

            #region Get Rated Movies
            UIUtils.UpdateStatus("Getting first batch of TMDb rated movies...");
            var movieRatings = TMDbAPI.GetRatedMovies(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, 1);
            if (importCancelled) return;
            #endregion

            #region Import Movie Ratings

            if (movieRatings != null && movieRatings.TotalResults > 0)
            {
                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var currentUserMovieRatings = TraktAPI.GetRatedMovies();

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count());
                    UIUtils.UpdateStatus("Filtering out movies already rated on trakt.tv");
                    movieRatings.Movies.RemoveAll(m => currentUserMovieRatings.Any(c => c.Movie.Ids.TmdbId == m.Id));
                }

                UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TMDb Movie Ratings...", movieRatings.Page, movieRatings.TotalPages, movieRatings.Movies.Count);

                if (movieRatings.Movies.Count > 0)
                {
                    var response = TraktAPI.AddMoviesToRatings(GetRateMoviesData(movieRatings.Movies));
                    if (response == null)
                    {
                        UIUtils.UpdateStatus("Failed to send ratings for TMDb movies", true);
                        Thread.Sleep(2000);
                    }
                    else if (response.NotFound.Movies.Count > 0)
                    {
                        UIUtils.UpdateStatus("Unable to sync ratings for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                        Thread.Sleep(1000);
                    }
                    if (importCancelled) return;

                    // add to list of movies to mark as watched
                    watchedMovies.AddRange(movieRatings.Movies);
                }

                // get each page of movies
                for (int i = 2; i <= movieRatings.TotalPages; i++)
                {
                    UIUtils.UpdateStatus("[{0}/{1}] Getting next batch of TMDb Rated Movies...", movieRatings.Page, movieRatings.TotalPages);
                    movieRatings = TMDbAPI.GetRatedMovies(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, i);
                    if (movieRatings == null || movieRatings.TotalResults == 0 || importCancelled) return;

                    if (currentUserMovieRatings != null)
                    {
                        UIUtils.UpdateStatus("Filtering out movies already rated on trakt.tv");
                        movieRatings.Movies.RemoveAll(m => currentUserMovieRatings.Any(c => c.Movie.Ids.TmdbId == m.Id));
                    }

                    UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TMDb Movie Ratings...", movieRatings.Page, movieRatings.TotalPages, movieRatings.Movies.Count);

                    if (movieRatings.Movies.Count > 0)
                    {
                        var response = TraktAPI.AddMoviesToRatings(GetRateMoviesData(movieRatings.Movies));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for TMDb movies", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }
                        if (importCancelled) return;

                        // add to list of movies to mark as watched
                        watchedMovies.AddRange(movieRatings.Movies);
                    }
                }
            }
            if (importCancelled) return;

            #endregion

            #region Mark As Watched
            if (AppSettings.MarkAsWatched && watchedMovies.Count > 0)
            {
                // mark all rated movies as watched
                UIUtils.UpdateStatus("Importing {0} TMDb movies as watched...", watchedMovies.Count);

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)watchedMovies.Count / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    UIUtils.UpdateStatus("Importing page {0}/{1} TMDb movies as watched...", i + 1, pages);

                    var response = TraktAPI.AddMoviesToWatchedHistory(GetSyncWatchedMoviesData(watchedMovies.Skip(i * pageSize).Take(pageSize).ToList()));
                    if (response == null)
                    {
                        UIUtils.UpdateStatus("Failed to send watched status for TMDb movies to trakt.tv", true);
                        Thread.Sleep(2000);
                    }
                    else if (response.NotFound.Movies.Count > 0)
                    {
                        UIUtils.UpdateStatus("Unable to sync watched states for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                        Thread.Sleep(1000);
                    }
                    if (importCancelled) return;
                }
            }
            #endregion

            #region Get Rated Shows
            UIUtils.UpdateStatus("Getting first batch of TMDb rated shows...");
            var showRatings = TMDbAPI.GetRatedShows(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, 1);
            if (importCancelled) return;
            #endregion

            #region Import Show Ratings
            if (showRatings != null && showRatings.TotalResults > 0) return;
            {
                UIUtils.UpdateStatus("Retrieving existing show ratings from trakt.tv");
                var currentUserShowRatings = TraktAPI.GetRatedShows();

                if (currentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user show ratings on trakt.tv", currentUserShowRatings.Count());
                    UIUtils.UpdateStatus("Filtering out shows already rated on trakt.tv");
                    showRatings.Shows.RemoveAll(s => currentUserShowRatings.Any(c => c.Show.Ids.TmdbId == s.Id));
                }

                UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TMDb Show Ratings...", showRatings.Page, showRatings.TotalPages, showRatings.Shows.Count);

                if (showRatings == null || showRatings.Shows.Count > 0)
                {
                    var response = TraktAPI.AddShowsToRatings(GetRateShowsData(showRatings.Shows));
                    if (response == null)
                    {
                        UIUtils.UpdateStatus("Failed to send ratings for TMDb shows", true);
                        Thread.Sleep(2000);
                    }
                    else if (response.NotFound.Shows.Count > 0)
                    {
                        UIUtils.UpdateStatus("Unable to sync ratings for {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                        Thread.Sleep(1000);
                    }

                    if (importCancelled) return;
                }

                // get each page of movies
                for (int i = 2; i <= showRatings.TotalPages; i++)
                {
                    UIUtils.UpdateStatus("[{0}/{1}] Getting next batch of TMDb rated shows...", showRatings.Page, showRatings.TotalPages);
                    showRatings = TMDbAPI.GetRatedShows(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, i);
                    if (importCancelled) return;

                    if (currentUserShowRatings != null)
                    {
                        UIUtils.UpdateStatus("Filtering out shows already rated on trakt.tv");
                        showRatings.Shows.RemoveAll(s => currentUserShowRatings.Any(c => c.Show.Ids.TmdbId == s.Id));
                    }

                    UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TMDb show ratings...", showRatings.Page, showRatings.TotalPages, showRatings.Shows.Count);

                    if (showRatings == null || showRatings.Shows.Count > 0)
                    {
                        var response = TraktAPI.AddShowsToRatings(GetRateShowsData(showRatings.Shows));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for TMDb shows.", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
            }
            if (importCancelled) return;
            #endregion

            #region Import Watchlist

            if (AppSettings.TMDbSyncWatchlist)
            {
                #region Movies
                IEnumerable<TraktMoviePlays> traktWatchedMovies = null;

                UIUtils.UpdateStatus("Getting first batch of TMDb watchlist movies...");
                var moviesInWatchlist = TMDbAPI.GetWatchlistMovies(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, 1);
                if (importCancelled) return;

                if (moviesInWatchlist != null && moviesInWatchlist.TotalResults > 0)
                {
                    UIUtils.UpdateStatus("Requesting existing watchlist movies from trakt...");
                    var traktWatchlistMovies = TraktAPI.GetWatchlistMovies();
                    if (traktWatchlistMovies != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watchlist movies on trakt", traktWatchlistMovies.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                        moviesInWatchlist.Movies.RemoveAll(w => traktWatchlistMovies.FirstOrDefault(t => t.Movie.Ids.TmdbId == w.Id) != null);
                    }
                    if (importCancelled) return;

                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Retrieving existing watched movies from trakt.tv");
                        traktWatchedMovies = TraktAPI.GetWatchedMovies();

                        if (traktWatchedMovies != null)
                        {
                            UIUtils.UpdateStatus("Found {0} watched movies on trakt.tv", traktWatchedMovies.Count());
                            UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watched on trakt.tv");
                            moviesInWatchlist.Movies.RemoveAll(m => traktWatchedMovies.Any(c => c.Movie.Ids.TmdbId == m.Id));
                        }
                    }
                    if (importCancelled) return;

                    UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TMDb watchlist movies...", moviesInWatchlist.Page, moviesInWatchlist.TotalPages, moviesInWatchlist.Movies.Count);

                    if (moviesInWatchlist.Movies.Count > 0)
                    {
                        var response = TraktAPI.AddMoviesToWatchlist(GetSyncMoviesData(moviesInWatchlist.Movies));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send watchlist for TMDb movies", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus(string.Format("Unable to sync watchlist for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count));
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }

                    // get each page of movies
                    for (int i = 2; i <= moviesInWatchlist.TotalPages; i++)
                    {
                        UIUtils.UpdateStatus("[{0}/{1}] Getting next batch of TMDb watchlist movies...", moviesInWatchlist.Page, moviesInWatchlist.TotalPages);
                        moviesInWatchlist = TMDbAPI.GetWatchlistMovies(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, i);
                        if (importCancelled) return;

                        if (moviesInWatchlist != null && moviesInWatchlist.TotalResults > 0)
                        {
                            if (traktWatchlistMovies != null)
                            {
                                UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                                moviesInWatchlist.Movies.RemoveAll(w => traktWatchlistMovies.FirstOrDefault(t => t.Movie.Ids.TmdbId == w.Id) != null);
                            }

                            if (traktWatchedMovies != null)
                            {
                                UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watched on trakt.tv");
                                moviesInWatchlist.Movies.RemoveAll(m => traktWatchedMovies.Any(c => c.Movie.Ids.TmdbId == m.Id));
                            }

                            UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TMDb watchlist movies...", moviesInWatchlist.Page, moviesInWatchlist.TotalPages, moviesInWatchlist.Movies.Count);

                            if (moviesInWatchlist.Movies.Count > 0)
                            {
                                var response = TraktAPI.AddMoviesToWatchlist(GetSyncMoviesData(moviesInWatchlist.Movies));
                                if (response == null)
                                {
                                    UIUtils.UpdateStatus("Failed to send watchlist for TMDb movies", true);
                                    Thread.Sleep(2000);
                                }
                                else if (response.NotFound.Movies.Count > 0)
                                {
                                    UIUtils.UpdateStatus("Unable to sync watchlist for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                                    Thread.Sleep(1000);
                                }
                                if (importCancelled) return;
                            }
                        }
                    }
                }
                if (importCancelled) return;
                #endregion

                #region Shows
                IEnumerable<TraktShowPlays> traktWatchedShows = null;

                UIUtils.UpdateStatus("Getting first batch of TMDb watchlist shows...");
                var showsInWatchlist = TMDbAPI.GetWatchlistShows(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, 1);
                if (importCancelled) return;

                if (showsInWatchlist != null || showsInWatchlist.TotalResults > 0)
                {
                    UIUtils.UpdateStatus("Requesting existing watchlist shows from trakt...");
                    var traktWatchlistShows = TraktAPI.GetWatchlistShows();
                    if (traktWatchlistShows != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watchlist shows on trakt", traktWatchlistShows.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watchlist on trakt.tv");
                        showsInWatchlist.Shows.RemoveAll(w => traktWatchlistShows.FirstOrDefault(t => t.Show.Ids.TmdbId == w.Id) != null);
                    }

                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Retrieving existing watched shows from trakt.tv.");
                        traktWatchedShows = TraktAPI.GetWatchedShows();

                        if (traktWatchedShows != null)
                        {
                            UIUtils.UpdateStatus("Found {0} watched shows on trakt.tv", traktWatchedShows.Count());
                            UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watched on trakt.tv");
                            showsInWatchlist.Shows.RemoveAll(s => traktWatchedShows.Any(c => c.Show.Ids.TmdbId == s.Id));
                        }
                    }

                    UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TMDb watchlist shows...", showsInWatchlist.Page, showsInWatchlist.TotalPages, showsInWatchlist.Shows.Count);

                    if (showsInWatchlist.Shows.Count > 0)
                    {
                        var response = TraktAPI.AddShowsToWatchlist(GetSyncShowsData(showsInWatchlist.Shows));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send watchlist for TMDb shows.", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync watchlist for {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                            Thread.Sleep(1000);
                        }
                        if (importCancelled) return;
                    }

                    // get each page of movies
                    for (int i = 2; i <= showsInWatchlist.TotalPages; i++)
                    {
                        UIUtils.UpdateStatus("[{0}/{1}] Getting next batch of TMDb watchlist shows...", showsInWatchlist.Page, showsInWatchlist.TotalPages);
                        showsInWatchlist = TMDbAPI.GetWatchlistShows(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, i);
                        if (importCancelled) return;

                        if (showsInWatchlist != null || showsInWatchlist.TotalResults > 0)
                        {
                            if (traktWatchlistShows != null)
                            {
                                UIUtils.UpdateStatus("Found {0} watchlist shows on trakt", traktWatchlistShows.Count());
                                UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watchlist on trakt.tv");
                                showsInWatchlist.Shows.RemoveAll(w => traktWatchlistShows.FirstOrDefault(t => t.Show.Ids.TmdbId == w.Id) != null);
                            }

                            if (traktWatchedShows != null)
                            {
                                UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watched on trakt.tv");
                                showsInWatchlist.Shows.RemoveAll(s => traktWatchedShows.Any(c => c.Show.Ids.TmdbId == s.Id));
                            }

                            UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TMDb watchlist shows...", showsInWatchlist.Page, showsInWatchlist.TotalPages, showsInWatchlist.Shows.Count);

                            if (showsInWatchlist.Shows.Count > 0)
                            {
                                var response = TraktAPI.AddShowsToWatchlist(GetSyncShowsData(showsInWatchlist.Shows));
                                if (response == null)
                                {
                                    UIUtils.UpdateStatus("Failed to send watchlist for TMDb shows", true);
                                    Thread.Sleep(2000);
                                }
                                else if (response.NotFound.Shows.Count > 0)
                                {
                                    UIUtils.UpdateStatus("Unable to sync watchlist for {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                                    Thread.Sleep(1000);
                                }
                                if (importCancelled) return;
                            }
                        }
                    }
                }
                if (importCancelled) return;
                #endregion
            }

            #endregion
        }

        public void Cancel()
        {
            // signals to cancel import
            importCancelled = true;
        }

        #endregion

        #region Private Methods

        private TraktMovieWatchedSync GetSyncWatchedMoviesData(List<TMDbMovie> movies)
        {
            var traktMovies = new List<TraktMovieWatched>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieWatched
                                 {
                                     Ids = new TraktMovieId { TmdbId = movie.Id }
                                 });

            var movieSyncData = new TraktMovieWatchedSync
            {
                Movies = traktMovies
            };

            return movieSyncData;
        }

        private TraktMovieSync GetSyncMoviesData(List<TMDbMovie> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     Ids = new TraktMovieId { TmdbId = movie.Id }
                                 });

            var movieSyncData = new TraktMovieSync
            {
                Movies = traktMovies
            };

            return movieSyncData;
        }

        private TraktShowSync GetSyncShowsData(List<TMDbShow> shows)
        {
            var traktShows = new List<TraktShow>();

            traktShows.AddRange(from show in shows
                                select new TraktShow
                                 {
                                     Ids = new TraktShowId { TmdbId = show.Id},
                                     Title = show.Title,
                                     Year = int.Parse(show.ReleaseDate.Substring(0,4))
                                 });

            var showSyncData = new TraktShowSync
            {
                Shows = traktShows
            };

            return showSyncData;
        }

        private TraktMovieRatingSync GetRateMoviesData(List<TMDbRatedMovie> movies)
        {
            var traktMovies = new List<TraktMovieRating>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieRating
                                 {
                                     Ids = new TraktMovieId { TmdbId = movie.Id },
                                     Rating = Convert.ToInt32(Math.Round(movie.Rating, MidpointRounding.AwayFromZero))
                                 });

            var movieRateData = new TraktMovieRatingSync
            {
                movies = traktMovies
            };

            return movieRateData;
        }

        private TraktShowRatingSync GetRateShowsData(List<TMDbRatedShow> shows)
        {
            var traktShows = new List<TraktShowRating>();

            traktShows.AddRange(from show in shows
                                select new TraktShowRating
                                 {
                                     Ids = new TraktShowId { TmdbId = show.Id },
                                     Title = show.Title,
                                     Year = string.IsNullOrEmpty(show.ReleaseDate) ? 0 : int.Parse(show.ReleaseDate.Substring(0, 4)),
                                     Rating = Convert.ToInt32(Math.Round(show.Rating, MidpointRounding.AwayFromZero))
                                 });

            var showRateData = new TraktShowRatingSync
            {
                shows = traktShows
            };

            return showRateData;
        }

        #endregion

        #region Public Static Methods

        public static string RequestToken()
        {
            UIUtils.UpdateStatus("Requesting token from TMDb...");
            TMDbTokenResponse response = TMDbAPI.RequestToken();
            if (response == null || !response.Success)
            {
                UIUtils.UpdateStatus("Failed to get TMDb token", true);
                Thread.Sleep(2000);
                return null;
            }
            return response.RequestToken;
        }

        public static void RequestAuthorization(string requestToken)
        {
            if (string.IsNullOrEmpty(requestToken))
                return;

            UIUtils.UpdateStatus("Launching default browser for Authentication Request...");

            Process.Start(string.Format(TMDbURIs.Authenticate, requestToken));

            UIUtils.UpdateStatus("Click on the 'Allow' button in webbrowser then start import.");
        }

        #endregion
    }
}

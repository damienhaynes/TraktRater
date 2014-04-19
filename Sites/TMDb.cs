using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TraktRater.UI;
using TraktRater.TraktAPI;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.Sites.API.TMDb;
using TraktRater.Settings;

namespace TraktRater.Sites
{
    internal class TMDb : IRateSite
    {
        #region Variables

        bool ImportCancelled = false;

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
            ImportCancelled = false;
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
            if (ImportCancelled) return;
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
            if (ImportCancelled) return;
            #endregion

            #region Get Rated Movies
            UIUtils.UpdateStatus("Getting first batch of TMDb rated movies..");
            var movieRatings = TMDbAPI.GetRatedMovies(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, 1);
            if (ImportCancelled) return;
            #endregion

            #region Import Movie Ratings
            if (movieRatings == null || movieRatings.TotalResults == 0) return;

            UIUtils.UpdateStatus("Retreiving existing movie ratings from trakt.tv.");
            var currentUserMovieRatings = TraktAPI.TraktAPI.GetUserRatedMovies(AppSettings.TraktUsername);

            if (currentUserMovieRatings != null)
            {
                UIUtils.UpdateStatus(string.Format("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count()));
                // Filter out movies to rate from existing ratings online
                movieRatings.Movies.RemoveAll(m => currentUserMovieRatings.Any(c => c.TMDBID == m.Id.ToString()));
            }

            UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} TMDb Movie Ratings...", movieRatings.Page, movieRatings.TotalPages, movieRatings.Movies.Count));

            if (movieRatings == null || movieRatings.Movies.Count > 0)
            {
                var response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(movieRatings.Movies));
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Failed to send ratings for TMDb movies.", true);
                    Thread.Sleep(2000);
                    if (ImportCancelled) return;
                }

                // add to list of movies to mark as watched
                watchedMovies.AddRange(movieRatings.Movies);
            }

            // get each page of movies
            for (int i = 2; i <= movieRatings.TotalPages; i++)
            {
                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Getting next batch of TMDb Rated Movies...", movieRatings.Page, movieRatings.TotalPages));
                movieRatings = TMDbAPI.GetRatedMovies(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, i);
                if (ImportCancelled) return;

                if (currentUserMovieRatings != null)
                {
                    // Filter out movies to rate from existing ratings online
                    movieRatings.Movies.RemoveAll(m => currentUserMovieRatings.Any(c => c.TMDBID == m.Id.ToString()));
                }

                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} TMDb Movie Ratings...", movieRatings.Page, movieRatings.TotalPages, movieRatings.Movies.Count));

                if (movieRatings == null || movieRatings.Movies.Count > 0)
                {
                    var response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(movieRatings.Movies));
                    if (response == null || response.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send ratings for TMDb movies.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }

                    // add to list of movies to mark as watched
                    watchedMovies.AddRange(movieRatings.Movies);
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Mark As Watched
            if (AppSettings.MarkAsWatched && watchedMovies.Count > 0)
            {
                // mark all movies as watched if rated
                UIUtils.UpdateStatus(string.Format("Importing {0} TMDb Movies as Watched...", watchedMovies.Count));
                TraktMovieSyncResponse watchedResponse = TraktAPI.TraktAPI.SyncMovieLibrary(GetWatchedMoviesData(watchedMovies), TraktSyncModes.seen);
                if (watchedResponse == null || watchedResponse.Status != "success")
                {
                    UIUtils.UpdateStatus("Failed to send watched status for TMDb movies.", true);
                    Thread.Sleep(2000);
                    if (ImportCancelled) return;
                }
            }
            #endregion

            #region Get Rated Shows
            UIUtils.UpdateStatus("Getting first batch of TMDb rated shows..");
            var showRatings = TMDbAPI.GetRatedShows(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, 1);
            if (ImportCancelled) return;
            #endregion

            #region Import Show Ratings
            if (showRatings == null || showRatings.TotalResults == 0) return;

            UIUtils.UpdateStatus("Retreiving existing show ratings from trakt.tv.");
            var currentUserShowRatings = TraktAPI.TraktAPI.GetUserRatedShows(AppSettings.TraktUsername);

            if (currentUserShowRatings != null)
            {
                UIUtils.UpdateStatus(string.Format("Found {0} user show ratings on trakt.tv", currentUserShowRatings.Count()));
                // Filter out shows to rate from existing ratings online
                showRatings.Shows.RemoveAll(m => currentUserShowRatings.Any(c => c.Title == m.Title && c.Year.ToString() == m.ReleaseDate.Substring(0,4)));
            }

            UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} TMDb Show Ratings...", showRatings.Page, showRatings.TotalPages, showRatings.Shows.Count));

            if (showRatings == null || showRatings.Shows.Count > 0)
            {
                var response = TraktAPI.TraktAPI.RateShows(GetRateShowsData(showRatings.Shows));
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Failed to send ratings for TMDb shows.", true);
                    Thread.Sleep(2000);
                    if (ImportCancelled) return;
                }
            }

            // get each page of movies
            for (int i = 2; i <= showRatings.TotalPages; i++)
            {
                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Getting next batch of TMDb rated shows...", showRatings.Page, showRatings.TotalPages));
                showRatings = TMDbAPI.GetRatedShows(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, i);
                if (ImportCancelled) return;

                if (currentUserShowRatings != null)
                {
                    // Filter out shows to rate from existing ratings online
                    showRatings.Shows.RemoveAll(m => currentUserShowRatings.Any(c => c.Title == m.Title && c.Year.ToString() == m.ReleaseDate.Substring(0, 4)));
                }

                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} TMDb show ratings...", showRatings.Page, showRatings.TotalPages, showRatings.Shows.Count));

                if (showRatings == null || showRatings.Shows.Count > 0)
                {
                    var response = TraktAPI.TraktAPI.RateShows(GetRateShowsData(showRatings.Shows));
                    if (response == null || response.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send ratings for TMDb shows.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }
                }
            }
            if (ImportCancelled) return;
            #endregion

            return;
        }

        public void Cancel()
        {
            // signals to cancel import
            ImportCancelled = true;
        }

        #endregion

        #region Private Methods

        private TraktMovieSync GetWatchedMoviesData(List<TMDbMovie> movies)
        {
            var traktMovies = new List<TraktMovieSync.Movie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieSync.Movie
                                 {
                                     TMDBID = movie.Id.ToString()
                                 });

            var movieWatchedData = new TraktMovieSync
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                MovieList = traktMovies
            };

            return movieWatchedData;
        }

        private TraktMovies GetRateMoviesData(List<TMDbMovie> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     TMDbId = movie.Id,
                                     Rating = Convert.ToInt32(Math.Round(movie.Rating, MidpointRounding.AwayFromZero))
                                 });

            var movieRateData = new TraktMovies
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Movies = traktMovies
            };

            return movieRateData;
        }

        private TraktShows GetRateShowsData(List<TMDbShow> shows)
        {
            var traktShows = new List<TraktShow>();

            traktShows.AddRange(from show in shows
                                 select new TraktShow
                                 {
                                     Title = show.Title,
                                     Year = int.Parse(show.ReleaseDate.Substring(0,4)),
                                     Rating = Convert.ToInt32(Math.Round(show.Rating, MidpointRounding.AwayFromZero))
                                 });

            var showRateData = new TraktShows
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Shows = traktShows
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

            System.Diagnostics.Process.Start(string.Format(TMDbURIs.Authenticate, requestToken));

            UIUtils.UpdateStatus("Click on the 'Allow' button in webbrowser then start import.");
        }

        #endregion
    }
}

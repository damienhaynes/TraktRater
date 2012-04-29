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
    public class TMDb : IRateSite
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
            UIUtils.UpdateStatus("Getting first batch of TMDb Rated Movies..");
            var ratings = TMDbAPI.GetRatedMovies(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, 1);
            if (ImportCancelled) return;
            #endregion

            #region Import Movie Ratings
            if (ratings.TotalResults == 0) return;
            UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} TMDb Movie Ratings...", ratings.Page, ratings.TotalPages, ratings.Movies.Count));
            var response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(ratings.Movies));
            if (response == null || response.Status != "success")
            {
                UIUtils.UpdateStatus("Failed to send ratings for TMDb movies.", true);
                Thread.Sleep(2000);
                if (ImportCancelled) return;
            }

            // get each page of movies
            for (int i = 2; i <= ratings.TotalPages; i++)
            {
                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Getting next batch of TMDb Rated Movies...", ratings.Page, ratings.TotalPages));
                ratings = TMDbAPI.GetRatedMovies(accountInfo.Id.ToString(), AppSettings.TMDbSessionId, i);
                if (ImportCancelled) return;

                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} TMDb Movie Ratings...", ratings.Page, ratings.TotalPages, ratings.Movies.Count));
                response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(ratings.Movies));
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Failed to send ratings for TMDb movies.", true);
                    Thread.Sleep(2000);
                    if (ImportCancelled) return;
                }                
            } 
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

        private TraktRateMovies GetRateMoviesData(List<TMDbMovie> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     TMDbId = movie.Id,
                                     Rating = Convert.ToInt32(Math.Round(movie.Rating))
                                 });

            var movieRateData = new TraktRateMovies
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Movies = traktMovies
            };

            return movieRateData;
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

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TraktRater.Extensions;
using TraktRater.TraktAPI;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.Settings;
using TraktRater.Sites.API.Critiker;
using TraktRater.UI;

namespace TraktRater.Sites
{
    internal class Critiker : IRateSite
    {
        #region Variables

        bool ImportCancelled = false;
        string CritikerMovieFile = null;

        #endregion
        
        #region Constructor

        public Critiker(string exportMovieFile)
        {
            CritikerMovieFile = exportMovieFile;
            Enabled = File.Exists(CritikerMovieFile);
        }

        #endregion

        #region IRateSite Members

        public string Name
        {
            get { return "Critiker"; }
        }

        public bool Enabled { get; set; }

        public void ImportRatings()
        {
            var critiker = CritikerAPI.ReadCritikerMovieExportFile(CritikerMovieFile);

            // check if everything we need was read okay
            if (critiker == null || critiker.Films == null)
            {
                UI.UIUtils.UpdateStatus("Error reading Critiker movie XML file.", true);
                return;
            }

            UIUtils.UpdateStatus(string.Format("Found {0} movies with ratings.", critiker.Films.Count));
            if (ImportCancelled) return;

            #region Import Ratings

            if (critiker.Films.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retreiving existing movie ratings from trakt.tv.");
                var currentUserMovieRatings = TraktAPI.TraktAPI.GetUserRatedMovies(AppSettings.TraktUsername);
                if (ImportCancelled) return;

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count()));

                    // filter out movies to rate from existing ratings online
                    critiker.Films.RemoveAll(m => currentUserMovieRatings.Any(c => c.Title.ToLowerInvariant() == m.Title.ToLowerInvariant() && c.Year == m.Year.ToString()));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} Critiker movie ratings...", critiker.Films.Count));
                if (critiker.Films.Count > 0)
                {
                    var response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(critiker.Films));
                    if (response == null || response.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send ratings for Critiker movies.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }
                }
            }

            #endregion

            #region Mark As Watched

            if (AppSettings.MarkAsWatched)
            {
                if (ImportCancelled) return;

                // mark all movies as watched if rated
                UIUtils.UpdateStatus(string.Format("Importing {0} Critiker movies as watched...", critiker.Films.Count));
                if (critiker.Films.Count > 0)
                {
                    var watchedResponse = TraktAPI.TraktAPI.SyncMovieLibrary(GetSyncMoviesData(critiker.Films), TraktSyncModes.seen);
                    if (watchedResponse == null || watchedResponse.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send watched status for Critiker movies.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }
                }
            }

            #endregion

            return;
        }

        public void Cancel()
        {
            ImportCancelled = true;
        }

        #endregion

        #region Private Methods

        private TraktMovies GetRateMoviesData(List<CritikerFilmRankings.Film> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     Title = movie.Title,
                                     Year = movie.Year,
                                     Rating = Convert.ToInt32(Math.Round(movie.Score / 10.0, MidpointRounding.AwayFromZero))
                                 });

            var movieRateData = new TraktMovies
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Movies = traktMovies
            };

            return movieRateData;
        }

        private TraktMovieSync GetSyncMoviesData(List<CritikerFilmRankings.Film> movies)
        {
            var traktMovies = new List<TraktMovieSync.Movie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieSync.Movie
                                 {
                                     Title = movie.Title,
                                     Year = movie.Year.ToString(),
                                     LastPlayed = movie.ReviewDate.ToEpoch()
                                 });

            var movieWatchedData = new TraktMovieSync
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                MovieList = traktMovies
            };

            return movieWatchedData;
        }

        #endregion
    }
}

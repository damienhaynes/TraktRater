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
using TraktRater.Sites.API.Criticker;
using TraktRater.UI;

namespace TraktRater.Sites
{
    internal class Criticker : IRateSite
    {
        #region Variables

        bool ImportCancelled = false;
        string CritickerMovieFile = null;

        #endregion
        
        #region Constructor

        public Criticker(string exportMovieFile)
        {
            CritickerMovieFile = exportMovieFile;
            Enabled = File.Exists(CritickerMovieFile);
        }

        #endregion

        #region IRateSite Members

        public string Name
        {
            get { return "Criticker"; }
        }

        public bool Enabled { get; set; }

        public void ImportRatings()
        {
            var criticker = CritickerAPI.ReadCritickerMovieExportFile(CritickerMovieFile);

            // check if everything we need was read okay
            if (criticker == null || criticker.Films == null)
            {
                UI.UIUtils.UpdateStatus("Error reading Criticker movie XML file.", true);
                return;
            }

            UIUtils.UpdateStatus(string.Format("Found {0} movies with ratings.", criticker.Films.Count));
            if (ImportCancelled) return;

            #region Import Ratings

            if (criticker.Films.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retreiving existing movie ratings from trakt.tv.");
                var currentUserMovieRatings = TraktAPI.TraktAPI.GetUserRatedMovies(AppSettings.TraktUsername);
                if (ImportCancelled) return;

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count()));

                    // filter out movies to rate from existing ratings online
                    UIUtils.UpdateStatus("Filtering out movies which are already rated");
                    criticker.Films.RemoveAll(m => currentUserMovieRatings.Any(c => c.Title.ToLowerInvariant() == m.Title.ToLowerInvariant() && c.Year == m.Year.ToString()));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} Criticker movie ratings...", criticker.Films.Count));
                if (criticker.Films.Count > 0)
                {
                    int pages = (int)Math.Ceiling((double)criticker.Films.Count() / 50);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Criticker movie ratings...", i + 1, pages));

                        var movies = GetRateMoviesData(criticker.Films.Skip(i * 50).Take(50).ToList());
                        var response = TraktAPI.TraktAPI.RateMovies(movies);
                        if (response == null || response.Status != "success")
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for Criticker movies.", true);
                            Thread.Sleep(2000);
                            if (ImportCancelled) return;
                        }
                    }
                }
            }

            #endregion

            #region Mark As Watched

            if (AppSettings.MarkAsWatched)
            {
                if (ImportCancelled) return;

                // mark all movies as watched if rated
                UIUtils.UpdateStatus(string.Format("Importing {0} Criticker movies as watched...", criticker.Films.Count));
                if (criticker.Films.Count > 0)
                {
                    int pages = (int)Math.Ceiling((double)criticker.Films.Count() / 50);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Criticker movies as watched...", i + 1, pages));

                        var watchedResponse = TraktAPI.TraktAPI.SyncMovieLibrary(GetSyncMoviesData(criticker.Films.Skip(i * 50).Take(50).ToList()), TraktSyncModes.seen);
                        if (watchedResponse == null || watchedResponse.Status != "success")
                        {
                            UIUtils.UpdateStatus("Failed to send watched status for Criticker movies.", true);
                            Thread.Sleep(2000);
                            if (ImportCancelled) return;
                        }
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

        private TraktMovies GetRateMoviesData(List<CritickerFilmRankings.Film> movies)
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

        private TraktMovieSync GetSyncMoviesData(List<CritickerFilmRankings.Film> movies)
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

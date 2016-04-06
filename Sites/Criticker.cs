namespace TraktRater.Sites
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using global::TraktRater.Extensions;
    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.Criticker;
    using global::TraktRater.TraktAPI;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;

    internal class Criticker : IRateSite
    {
        #region Variables

        bool importCancelled = false;

        readonly string critickerMovieFile = null;

        #endregion
        
        #region Constructor

        public Criticker(string exportMovieFile)
        {
            critickerMovieFile = exportMovieFile;
            Enabled = File.Exists(critickerMovieFile);
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
            var criticker = CritickerAPI.ReadCritickerMovieExportFile(critickerMovieFile);

            // check if everything we need was read okay
            if (criticker == null || criticker.Films == null)
            {
                UIUtils.UpdateStatus("Error reading Criticker movie XML file", true);
                return;
            }

            UIUtils.UpdateStatus("Found {0} movies with ratings", criticker.Films.Count);
            if (importCancelled) return;

            #region Import Ratings

            if (criticker.Films.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var currentUserMovieRatings = TraktAPI.GetRatedMovies();
                if (importCancelled) return;

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count());

                    // filter out movies to rate from existing ratings online
                    UIUtils.UpdateStatus("Filtering out movies which are already rated");
                    criticker.Films.RemoveAll(m => currentUserMovieRatings.Any(c => c.Movie.Title.ToLowerInvariant() == m.Title.ToLowerInvariant() && c.Movie.Year == m.Year));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} Criticker movie ratings...", criticker.Films.Count));
                if (criticker.Films.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)criticker.Films.Count() / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} Criticker movie ratings...", i + 1, pages);

                        var movies = GetRateMoviesData(criticker.Films.Skip(i * pageSize).Take(pageSize).ToList());
                        var response = TraktAPI.AddMoviesToRatings(movies);
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for Criticker movies.", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
            }

            #endregion

            #region Mark As Watched

            if (AppSettings.MarkAsWatched)
            {
                if (importCancelled) return;

                // mark all movies as watched if rated
                UIUtils.UpdateStatus("Importing {0} Criticker movies as watched...", criticker.Films.Count);
                if (criticker.Films.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)criticker.Films.Count() / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} Criticker movies as watched...", i + 1, pages);

                        var watchedResponse = TraktAPI.AddMoviesToWatchedHistory(GetSyncMoviesData(criticker.Films.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (watchedResponse == null)
                        {
                            UIUtils.UpdateStatus("Failed to send watched status for Criticker movies", true);
                            Thread.Sleep(2000);
                        }
                        else if (watchedResponse.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync watched for {0} movies as they're not found on trakt.tv!", watchedResponse.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
            }

            #endregion
        }

        public void Cancel()
        {
            importCancelled = true;
        }

        #endregion

        #region Private Methods

        private TraktMovieRatingSync GetRateMoviesData(List<CritickerFilmRankings.Film> movies)
        {
            var traktMovies = new List<TraktMovieRating>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieRating
                                 {
                                     Title = movie.Title,
                                     Year = movie.Year,
                                     Rating = Convert.ToInt32(Math.Round(movie.Score / 10.0, MidpointRounding.AwayFromZero)),
                                     RatedAt = movie.ReviewDate.ToISO8601()
                                 });

            var movieRateData = new TraktMovieRatingSync
            {
                movies = traktMovies
            };

            return movieRateData;
        }

        private TraktMovieWatchedSync GetSyncMoviesData(List<CritickerFilmRankings.Film> movies)
        {
            var traktMovies = new List<TraktMovieWatched>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieWatched
                                 {
                                     Title = movie.Title,
                                     Year = movie.Year,
                                     WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : movie.ReviewDate.ToISO8601()
                                 });

            var movieWatchedData = new TraktMovieWatchedSync
            {
                Movies = traktMovies
            };

            return movieWatchedData;
        }

        #endregion
    }
}

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
using TraktRater.Sites.API.Listal;
using TraktRater.UI;

namespace TraktRater.Sites
{
    internal class Listal : IRateSite
    {
        #region Variables
  
        bool ImportCancelled = false;
        bool ImportWantlist = false;

        string ListalMovieFile = null;

        #endregion
        
        #region Constructor

        public Listal(string exportMovieFile, bool syncWantList)
        {
            Enabled = File.Exists(exportMovieFile);
            
            ListalMovieFile = exportMovieFile;
            ImportWantlist = syncWantList;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "Listal"; } }

        public void ImportRatings()
        {
            ImportCancelled = false;

            var listal = ListalAPI.ReadListExportFile(ListalMovieFile);

            // check if everything we need was read okay
            if (listal == null || listal.Channel == null || listal.Channel.Items == null)
            {
                UI.UIUtils.UpdateStatus("Error reading Listal Movie XML file.", true);
                return;
            }

            UIUtils.UpdateStatus(string.Format("Found {0} movies in Listal export file", listal.Channel.Items.Count));
            if (ImportCancelled) return;

            #region Ratings

            var listalMovieRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();
            UIUtils.UpdateStatus(string.Format("Found {0} movies with ratings.", listalMovieRatings.Count), true);

            if (listalMovieRatings.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retreiving existing movie ratings from trakt.tv.");
                var currentUserMovieRatings = TraktAPI.TraktAPI.GetUserRatedMovies(AppSettings.TraktUsername);
                if (ImportCancelled) return;

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count()));
                    
                    // filter out movies to rate from existing ratings online
                    listalMovieRatings.RemoveAll(m => currentUserMovieRatings.Any(c => c.IMDBID == "tt" + m.IMDbId.ToString()));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} Listal Movie Ratings...", listalMovieRatings.Count));
                if (listalMovieRatings.Count > 0)
                {
                    var response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(listalMovieRatings));
                    if (response == null || response.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send ratings for Listal movies.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }
                }
            }

            #endregion

            #region Watched
            if (AppSettings.MarkAsWatched)
            {
                if (ImportCancelled) return;

                // mark all movies as watched if rated
                UIUtils.UpdateStatus(string.Format("Importing {0} Listal movies as watched...", listalMovieRatings.Count));
                if (listalMovieRatings.Count > 0)
                {
                    var watchedResponse = TraktAPI.TraktAPI.SyncMovieLibrary(GetSyncMoviesData(listalMovieRatings), TraktSyncModes.seen);
                    if (watchedResponse == null || watchedResponse.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send watched status for Listal movies.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }
                }
            }
            #endregion

            #region Watchlist
            // Convert Listal Wantlist to a Trakt Watchlist
            if (ImportWantlist)
            {                
                if (ImportCancelled) return;

                var wantList = listal.Channel.Items.Where(m => m.ListType == ListType.wanted.ToString()).ToList();

                if (wantList.Count > 0)
                {
                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Requesting watched movies from trakt...");

                        // get watched movies from trakt so we don't import movies into watchlist that are already watched
                        var watchedTraktMovies = TraktAPI.TraktAPI.GetUserWatchedMovies(AppSettings.TraktUsername);
                        if (watchedTraktMovies != null)
                        {
                            UIUtils.UpdateStatus(string.Format("Found {0} watched movies on trakt", watchedTraktMovies.Count()));

                            // remove movies from sync list which are watched already
                            wantList.RemoveAll(w => watchedTraktMovies.Count(t => t.IMDbId == "tt" + w.IMDbId) != 0);
                        }
                        if (ImportCancelled) return;
                    }

                    // add all movies to watchlist
                    UIUtils.UpdateStatus(string.Format("Importing {0} Listal Wantlist movies to trakt.tv Watchlist...", wantList.Count()));
                    var watchlistMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(GetSyncMoviesData(wantList), TraktSyncModes.watchlist);
                    if (watchlistMoviesResponse == null || watchlistMoviesResponse.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send watchlist for Listal movies.", true);
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
            // signals to cancel import
            ImportCancelled = true;
        }

        #endregion

        #region Private Methods

        private TraktMovies GetRateMoviesData(List<ListalExport.RSSChannel.Item> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     Title = movie.Title,
                                     IMDbId = "tt" + movie.IMDbId,
                                     Rating = movie.Rating
                                 });

            var movieRateData = new TraktMovies
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Movies = traktMovies
            };

            return movieRateData;
        }

        private TraktMovieSync GetSyncMoviesData(List<ListalExport.RSSChannel.Item> movies)
        {
            var traktMovies = new List<TraktMovieSync.Movie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieSync.Movie
                                 {
                                     Title = movie.Title,
                                     IMDBID = "tt" + movie.IMDbId,
                                     LastPlayed = movie.PublishedDate.ToEpoch(-5) // bug with Listal's GMT conversion
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

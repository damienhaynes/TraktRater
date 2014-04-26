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

        bool ImportMovies = false;
        bool ImportShows = false;

        string ListalMovieFile = null;
        string ListalShowFile = null;

        #endregion
        
        #region Constructor

        public Listal(string exportMovieFile, string exportShowFile, bool syncWantList)
        {
            ListalMovieFile = exportMovieFile;
            ListalShowFile = exportShowFile;

            ImportMovies = File.Exists(exportMovieFile);
            ImportShows = File.Exists(exportShowFile);

            ImportWantlist = syncWantList;

            Enabled = ImportShows || ImportMovies;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "Listal"; } }

        public void ImportRatings()
        {
            ImportCancelled = false;

            ImportMovieData();
            ImportShowData();

            return;
        }

        public void Cancel()
        {
            // signals to cancel import
            ImportCancelled = true;
        }

        #endregion

        #region Private Methods

        private void ImportMovieData()
        {
            if (!ImportMovies) return;

            var listal = ListalAPI.ReadListalExportFile(ListalMovieFile);

            // check if everything we need was read okay
            if (listal == null || listal.Channel == null || listal.Channel.Items == null)
            {
                UI.UIUtils.UpdateStatus("Error reading Listal movie XML file.", true);
                return;
            }

            UIUtils.UpdateStatus(string.Format("Found {0} movies in Listal export file", listal.Channel.Items.Count));
            if (ImportCancelled) return;

            #region Ratings

            var listalMovieRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();
            UIUtils.UpdateStatus(string.Format("Found {0} movies with ratings.", listalMovieRatings.Count));

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

                UIUtils.UpdateStatus(string.Format("Importing {0} Listal movie ratings...", listalMovieRatings.Count));
                if (listalMovieRatings.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)listalMovieRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Listal rated movies...", i + 1, pages));

                        var response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(listalMovieRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (response == null || response.Status != "success")
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for Listal movies.", true);
                            Thread.Sleep(2000);
                        }

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
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)listalMovieRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Listal movies as watched...", i + 1, pages));

                        var watchedResponse = TraktAPI.TraktAPI.SyncMovieLibrary(GetSyncMoviesData(listalMovieRatings.Skip(i * pageSize).Take(pageSize).ToList()), TraktSyncModes.seen);
                        if (watchedResponse == null || watchedResponse.Status != "success")
                        {
                            UIUtils.UpdateStatus("Failed to send watched status for Listal movies.", true);
                            Thread.Sleep(2000);
                        }

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

                    if (wantList.Count > 0)
                    {
                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)wantList.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Listal wantlist movies to trakt.tv watchlist...", i + 1, pages));

                            var watchlistMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(GetSyncMoviesData(wantList.Skip(i * pageSize).Take(pageSize).ToList()), TraktSyncModes.watchlist);
                            if (watchlistMoviesResponse == null || watchlistMoviesResponse.Status != "success")
                            {
                                UIUtils.UpdateStatus("Failed to send watchlist for Listal movies.", true);
                                Thread.Sleep(2000);
                            }

                            if (ImportCancelled) return;
                        }
                    }
                }
            }

            #endregion
        }

        private void ImportShowData()
        {
            if (!ImportShows) return;

            var listal = ListalAPI.ReadListalExportFile(ListalShowFile);

            // check if everything we need was read okay
            if (listal == null || listal.Channel == null || listal.Channel.Items == null)
            {
                UI.UIUtils.UpdateStatus("Error reading Listal tv show XML file.", true);
                return;
            }

            UIUtils.UpdateStatus(string.Format("Found {0} tv shows in Listal export file", listal.Channel.Items.Count));
            if (ImportCancelled) return;

            #region Ratings

            var listalShowRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();
            UIUtils.UpdateStatus(string.Format("Found {0} tv shows with ratings.", listalShowRatings.Count), true);

            if (listalShowRatings.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retreiving existing tv show ratings from trakt.tv.");
                var currentUserShowRatings = TraktAPI.TraktAPI.GetUserRatedShows(AppSettings.TraktUsername);
                if (ImportCancelled) return;

                if (currentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count()));

                    // filter out shows to rate from existing ratings online
                    listalShowRatings.RemoveAll(m => currentUserShowRatings.Any(c => c.IMDBID == "tt" + m.IMDbId.ToString()));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} Listal tv show ratings...", listalShowRatings.Count));
                if (listalShowRatings.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)listalShowRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Listal show ratings...", i + 1, pages));

                        var response = TraktAPI.TraktAPI.RateShows(GetRateShowsData(listalShowRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (response == null || response.Status != "success")
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for Listal tv shows.", true);
                            Thread.Sleep(2000);
                        }

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
                        UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                        // get watched tv shows from trakt so we don't import shows into watchlist that are already watched
                        // here we're really just checking if at least a single episode is watched on trakt as that's what we get back from the API
                        var watchedTraktShows = TraktAPI.TraktAPI.GetUserWatchedShows(AppSettings.TraktUsername);
                        if (watchedTraktShows != null)
                        {
                            UIUtils.UpdateStatus(string.Format("Found {0} watched tv shows on trakt", watchedTraktShows.Count()));

                            // remove shows from sync list which are watched already
                            wantList.RemoveAll(w => watchedTraktShows.Count(t => t.IMDbId == "tt" + w.IMDbId) != 0);
                        }
                        if (ImportCancelled) return;
                    }

                    // add movies to watchlist
                    UIUtils.UpdateStatus(string.Format("Importing {0} Listal Wantlist tv shows to trakt.tv Watchlist...", wantList.Count()));

                    if (wantList.Count > 0)
                    {
                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)wantList.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Listal wantlist tv shows to trakt.tv watchlist...", i + 1, pages));

                            var watchlistShowsResponse = TraktAPI.TraktAPI.SyncShowLibrary(GetSyncShowsData(wantList.Skip(i * pageSize).Take(pageSize).ToList()), TraktSyncModes.watchlist);
                            if (watchlistShowsResponse == null || watchlistShowsResponse.Status != "success")
                            {
                                UIUtils.UpdateStatus("Failed to send watchlist for Listal tv shows.", true);
                                Thread.Sleep(2000);
                            }

                            if (ImportCancelled) return;
                        }
                    }
                }
            }

            #endregion
        }

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

        private TraktShows GetRateShowsData(List<ListalExport.RSSChannel.Item> shows)
        {
            var traktShows = new List<TraktShow>();

            traktShows.AddRange(from show in shows
                                select new TraktShow
                                {
                                    IMDbId = "tt" + show.IMDbId,
                                    Title = show.Title,
                                    Rating = show.Rating
                                });

            var movieRateData = new TraktShows
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Shows = traktShows
            };

            return movieRateData;
        }

        private TraktShowSync GetSyncShowsData(List<ListalExport.RSSChannel.Item> shows)
        {
            var traktShows = new List<TraktShowSync.Show>();

            traktShows.AddRange(from show in shows
                                select new TraktShowSync.Show
                                 {
                                     Title = show.Title,
                                     IMDbId = "tt" + show.IMDbId                                     
                                 });

            var showWatchedData = new TraktShowSync
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Showlist = traktShows
            };

            return showWatchedData;
        }
        #endregion
    }
}

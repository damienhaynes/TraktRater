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

        IEnumerable<TraktMoviePlays> watchedTraktMovies = null;

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
                UI.UIUtils.UpdateStatus("Error reading Listal movie XML file", true);
                return;
            }

            UIUtils.UpdateStatus(string.Format("Found {0} movies in Listal export file", listal.Channel.Items.Count));
            if (ImportCancelled) return;

            #region Ratings

            var listalMovieRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();
            UIUtils.UpdateStatus(string.Format("Found {0} movies with ratings", listalMovieRatings.Count));

            if (listalMovieRatings.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var currentUserMovieRatings = TraktAPI.TraktAPI.GetRatedMovies();
                if (ImportCancelled) return;

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count()));

                    // filter out movies to rate from existing ratings online
                    listalMovieRatings.RemoveAll(m => currentUserMovieRatings.Any(c => c.Movie.Ids.ImdbId == "tt" + m.IMDbId.ToString()));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} new Listal movie ratings...", listalMovieRatings.Count));
                if (listalMovieRatings.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)listalMovieRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Listal rated movies...", i + 1, pages));

                        var response = TraktAPI.TraktAPI.SyncMoviesRated(GetRateMoviesData(listalMovieRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for Listal movies", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus(string.Format("Unable to sync ratings for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count));
                            Thread.Sleep(1000);
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
                listalMovieRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();

                // get watched movies from trakt.tv
                UIUtils.UpdateStatus("Requesting watched movies from trakt...");
                watchedTraktMovies = TraktAPI.TraktAPI.GetWatchedMovies();
                if (watchedTraktMovies == null)
                {
                    UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv, skipping watched movie import", true);
                    Thread.Sleep(2000);
                }
                else
                {
                    if (ImportCancelled) return;

                    UIUtils.UpdateStatus(string.Format("Found {0} watched movies on trakt", watchedTraktMovies.Count()));
                    UIUtils.UpdateStatus("Filtering out watched movies that are already watched on trakt.tv");
                    listalMovieRatings.RemoveAll(w => watchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == "tt" + w.IMDbId) != null);

                    UIUtils.UpdateStatus(string.Format("Importing {0} Listal movies as watched...", listalMovieRatings.Count));

                    if (listalMovieRatings.Count > 0)
                    {
                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)listalMovieRatings.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Listal movies as watched...", i + 1, pages));

                            var watchedResponse = TraktAPI.TraktAPI.SyncMoviesWatched(GetWatchedMoviesData(listalMovieRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (watchedResponse == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watched status for Listal movies", true);
                                Thread.Sleep(2000);
                            }
                            else if (watchedResponse.NotFound.Movies.Count > 0)
                            {
                                UIUtils.UpdateStatus(string.Format("Unable to sync watched for {0} movies as they're not found on trakt.tv!", watchedResponse.NotFound.Movies.Count));
                                Thread.Sleep(1000);
                            }

                            if (ImportCancelled) return;
                        }
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
                    UIUtils.UpdateStatus("Requesting existing watchlist movies from trakt...");
                    var watchlistTraktMovies = TraktAPI.TraktAPI.GetWatchlistMovies();
                    if (watchlistTraktMovies != null)
                    {
                        UIUtils.UpdateStatus(string.Format("Found {0} watchlist movies on trakt", watchlistTraktMovies.Count()));
                        UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                        wantList.RemoveAll(w => watchlistTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == "tt" + w.IMDbId) != null);
                    }
                    if (ImportCancelled) return;

                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Requesting watched movies from trakt...");

                        // get watched movies from trakt so we don't import movies into watchlist that are already watched
                        if (watchedTraktMovies == null)
                        {
                            watchedTraktMovies = TraktAPI.TraktAPI.GetWatchedMovies();
                            if (watchedTraktMovies != null)
                            {
                                UIUtils.UpdateStatus(string.Format("Found {0} watched movies on trakt", watchedTraktMovies.Count()));

                                // remove movies from sync list which are watched already
                                wantList.RemoveAll(w => watchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == "tt" + w.IMDbId) != null);
                            }
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

                            var watchlistMoviesResponse = TraktAPI.TraktAPI.SyncMovieWatchlist(GetMoviesData(wantList.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (watchlistMoviesResponse == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watchlist for Listal movies", true);
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
                UI.UIUtils.UpdateStatus("Error reading Listal tv show XML file", true);
                return;
            }

            UIUtils.UpdateStatus(string.Format("Found {0} tv shows in Listal export file", listal.Channel.Items.Count));
            if (ImportCancelled) return;

            #region Ratings

            var listalShowRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();
            UIUtils.UpdateStatus(string.Format("Found {0} tv shows with ratings", listalShowRatings.Count), true);

            if (listalShowRatings.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retrieving existing tv show ratings from trakt.tv");
                var currentUserShowRatings = TraktAPI.TraktAPI.GetRatedShows();
                if (ImportCancelled) return;

                if (currentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count()));

                    // filter out shows to rate from existing ratings online
                    listalShowRatings.RemoveAll(m => currentUserShowRatings.Any(c => c.Show.Ids.ImdbId == "tt" + m.IMDbId.ToString()));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} Listal tv show ratings...", listalShowRatings.Count));
                if (listalShowRatings.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)listalShowRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} Listal show ratings...", i + 1, pages));

                        var response = TraktAPI.TraktAPI.SyncShowsRated(GetRateShowsData(listalShowRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for Listal tv shows", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus(string.Format("Unable to sync ratings for {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count));
                            Thread.Sleep(1000);
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
                    UIUtils.UpdateStatus("Requesting existing watchlist shows from trakt...");
                    var watchlistTraktShows = TraktAPI.TraktAPI.GetWatchlistShows();
                    if (watchlistTraktShows != null)
                    {
                        UIUtils.UpdateStatus(string.Format("Found {0} watchlist shows on trakt", watchlistTraktShows.Count()));
                        UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watchlist on trakt.tv");
                        wantList.RemoveAll(w => watchlistTraktShows.FirstOrDefault(t => t.Show.Ids.ImdbId == "tt" + w.IMDbId) != null);
                    }
                    if (ImportCancelled) return;

                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                        // get watched movies from trakt so we don't import shows into watchlist that are already watched
                        var watchedTraktShows = TraktAPI.TraktAPI.GetWatchedEpisodes();
                        if (watchedTraktShows != null)
                        {
                            UIUtils.UpdateStatus(string.Format("Found {0} watched shows on trakt", watchedTraktShows.Count()));
                            UIUtils.UpdateStatus("Filtering out watchlist shows containing watched episodes on trakt.tv.");

                            // remove shows from sync list which are watched already
                            wantList.RemoveAll(w => watchedTraktShows.FirstOrDefault(t => t.Show.Ids.ImdbId == "tt" + w.IMDbId) != null);
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

                            var watchlistShowsResponse = TraktAPI.TraktAPI.SyncShowWatchlist(GetSyncShowsData(wantList.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (watchlistShowsResponse == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watchlist for Listal tv shows", true);
                                Thread.Sleep(2000);
                            }

                            if (ImportCancelled) return;
                        }
                    }
                }
            }

            #endregion
        }

        private TraktMovieRatingSync GetRateMoviesData(List<ListalExport.RSSChannel.Item> movies)
        {
            var traktMovies = new List<TraktMovieRating>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieRating
                                 {
                                     Ids = new TraktMovieId { ImdbId = "tt" + movie.IMDbId },
                                     Title = movie.Title,
                                     Rating = movie.Rating,
                                     RatedAt = movie.PublishedDate.ToISO8601(-5) // bug with Listal's GMT conversion
                                 });

            var movieRateData = new TraktMovieRatingSync
            {
                movies = traktMovies
            };

            return movieRateData;
        }

        private TraktMovieWatchedSync GetWatchedMoviesData(List<ListalExport.RSSChannel.Item> movies)
        {
            var traktMovies = new List<TraktMovieWatched>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieWatched
                                 {
                                     Title = movie.Title,
                                     Ids = new TraktMovieId { ImdbId = "tt" + movie.IMDbId },
                                     WatchedAt = movie.PublishedDate.ToISO8601(-5) // bug with Listal's GMT conversion
                                 });

            var movieWatchedData = new TraktMovieWatchedSync
            {
                Movies = traktMovies
            };

            return movieWatchedData;
        }

        private TraktMovieSync GetMoviesData(List<ListalExport.RSSChannel.Item> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     Title = movie.Title,
                                     Ids = new TraktMovieId { ImdbId = "tt" + movie.IMDbId },
                                 });

            var movieData = new TraktMovieSync
            {
                Movies = traktMovies
            };

            return movieData;
        }

        private TraktShowRatingSync GetRateShowsData(List<ListalExport.RSSChannel.Item> shows)
        {
            var traktShows = new List<TraktShowRating>();

            traktShows.AddRange(from show in shows
                                select new TraktShowRating
                                {
                                    Ids = new TraktShowId { ImdbId = "tt" + show.IMDbId },
                                    Title = show.Title,
                                    Rating = show.Rating,
                                    RatedAt = show.PublishedDate.ToISO8601(-5) // bug with Listal's GMT conversion
                                });

            var showRateData = new TraktShowRatingSync
            {
                shows = traktShows               
            };

            return showRateData;
        }

        private TraktShowSync GetSyncShowsData(List<ListalExport.RSSChannel.Item> shows)
        {
            var traktShows = new List<TraktShow>();

            traktShows.AddRange(from show in shows
                                select new TraktShow
                                 {
                                     Title = show.Title,
                                     Ids = new TraktShowId { ImdbId = "tt" + show.IMDbId }
                                 });

            var showWatchedData = new TraktShowSync
            {
                Shows = traktShows
            };

            return showWatchedData;
        }

        #endregion
    }
}

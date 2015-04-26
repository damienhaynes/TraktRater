namespace TraktRater.Sites
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using global::TraktRater.Extensions;
    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.Listal;
    using global::TraktRater.TraktAPI;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;

    internal class Listal : IRateSite
    {
        #region Variables
  
        bool importCancelled = false;

        readonly bool importWantlist = false;
        readonly bool importMovies = false;
        readonly bool importShows = false;
        readonly string listalMovieFile = null;
        readonly string listalShowFile = null;

        IEnumerable<TraktMoviePlays> watchedTraktMovies = null;

        #endregion
        
        #region Constructor

        public Listal(string exportMovieFile, string exportShowFile, bool syncWantList)
        {
            listalMovieFile = exportMovieFile;
            listalShowFile = exportShowFile;

            importMovies = File.Exists(exportMovieFile);
            importShows = File.Exists(exportShowFile);

            importWantlist = syncWantList;

            Enabled = importShows || importMovies;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "Listal"; } }

        public void ImportRatings()
        {
            importCancelled = false;

            ImportMovieData();
            ImportShowData();
        }

        public void Cancel()
        {
            // signals to cancel import
            importCancelled = true;
        }

        #endregion

        #region Private Methods

        private void ImportMovieData()
        {
            if (!importMovies) return;

            var listal = ListalAPI.ReadListalExportFile(listalMovieFile);

            // check if everything we need was read okay
            if (listal == null || listal.Channel == null || listal.Channel.Items == null)
            {
                UIUtils.UpdateStatus("Error reading Listal movie XML file", true);
                return;
            }

            UIUtils.UpdateStatus("Found {0} movies in Listal export file", listal.Channel.Items.Count);
            if (importCancelled) return;

            #region Ratings

            var listalMovieRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();
            UIUtils.UpdateStatus("Found {0} movies with ratings", listalMovieRatings.Count);

            if (listalMovieRatings.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var currentUserMovieRatings = TraktAPI.GetRatedMovies();
                if (importCancelled) return;

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count());

                    // filter out movies to rate from existing ratings online
                    listalMovieRatings.RemoveAll(m => currentUserMovieRatings.Any(c => c.Movie.Ids.ImdbId == "tt" + m.IMDbId.ToString()));
                }

                UIUtils.UpdateStatus("Importing {0} new Listal movie ratings...", listalMovieRatings.Count);
                if (listalMovieRatings.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)listalMovieRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} Listal rated movies...", i + 1, pages);

                        var response = TraktAPI.AddMoviesToRatings(GetRateMoviesData(listalMovieRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for Listal movies", true);
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

            #region Watched
            
            if (AppSettings.MarkAsWatched)
            {
                if (importCancelled) return;

                // mark all movies as watched if rated
                listalMovieRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();

                // get watched movies from trakt.tv
                UIUtils.UpdateStatus("Requesting watched movies from trakt...");
                watchedTraktMovies = TraktAPI.GetWatchedMovies();
                if (watchedTraktMovies == null)
                {
                    UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv, skipping watched movie import", true);
                    Thread.Sleep(2000);
                }
                else
                {
                    if (importCancelled) return;

                    UIUtils.UpdateStatus("Found {0} watched movies on trakt", watchedTraktMovies.Count());
                    UIUtils.UpdateStatus("Filtering out watched movies that are already watched on trakt.tv");
                    listalMovieRatings.RemoveAll(w => watchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == "tt" + w.IMDbId) != null);

                    UIUtils.UpdateStatus("Importing {0} Listal movies as watched...", listalMovieRatings.Count);

                    if (listalMovieRatings.Count > 0)
                    {
                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)listalMovieRatings.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus("Importing page {0}/{1} Listal movies as watched...", i + 1, pages);

                            var watchedResponse = TraktAPI.AddMoviesToWatchedHistory(GetWatchedMoviesData(listalMovieRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (watchedResponse == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watched status for Listal movies", true);
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
            }
            #endregion

            #region Watchlist

            // Convert Listal Wantlist to a Trakt Watchlist
            if (importWantlist)
            {
                if (importCancelled) return;

                var wantList = listal.Channel.Items.Where(m => m.ListType == ListType.wanted.ToString()).ToList();

                if (wantList.Count > 0)
                {
                    UIUtils.UpdateStatus("Requesting existing watchlist movies from trakt...");
                    var watchlistTraktMovies = TraktAPI.GetWatchlistMovies();
                    if (watchlistTraktMovies != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watchlist movies on trakt", watchlistTraktMovies.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                        wantList.RemoveAll(w => watchlistTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == "tt" + w.IMDbId) != null);
                    }
                    if (importCancelled) return;

                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Requesting watched movies from trakt...");

                        // get watched movies from trakt so we don't import movies into watchlist that are already watched
                        if (watchedTraktMovies == null)
                        {
                            watchedTraktMovies = TraktAPI.GetWatchedMovies();
                            if (watchedTraktMovies != null)
                            {
                                UIUtils.UpdateStatus("Found {0} watched movies on trakt", watchedTraktMovies.Count());

                                // remove movies from sync list which are watched already
                                wantList.RemoveAll(w => watchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == "tt" + w.IMDbId) != null);
                            }
                        }
                        if (importCancelled) return;
                    }

                    // add all movies to watchlist
                    UIUtils.UpdateStatus("Importing {0} Listal Wantlist movies to trakt.tv Watchlist...", wantList.Count());

                    if (wantList.Count > 0)
                    {
                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)wantList.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus("Importing page {0}/{1} Listal wantlist movies to trakt.tv watchlist...", i + 1, pages);

                            var watchlistMoviesResponse = TraktAPI.AddMoviesToWatchlist(GetMoviesData(wantList.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (watchlistMoviesResponse == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watchlist for Listal movies", true);
                                Thread.Sleep(2000);
                            }

                            if (importCancelled) return;
                        }
                    }
                }
            }

            #endregion
        }

        private void ImportShowData()
        {
            if (!importShows) return;

            var listal = ListalAPI.ReadListalExportFile(listalShowFile);

            // check if everything we need was read okay
            if (listal == null || listal.Channel == null || listal.Channel.Items == null)
            {
                UIUtils.UpdateStatus("Error reading Listal tv show XML file", true);
                return;
            }

            UIUtils.UpdateStatus("Found {0} tv shows in Listal export file", listal.Channel.Items.Count);
            if (importCancelled) return;

            #region Ratings

            var listalShowRatings = listal.Channel.Items.Where(m => m.Rating > 0).ToList();
            UIUtils.UpdateStatus(string.Format("Found {0} tv shows with ratings", listalShowRatings.Count), true);

            if (listalShowRatings.Count > 0)
            {
                // get current trakt ratings
                UIUtils.UpdateStatus("Retrieving existing tv show ratings from trakt.tv");
                var currentUserShowRatings = TraktAPI.GetRatedShows();
                if (importCancelled) return;

                if (currentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count());

                    // filter out shows to rate from existing ratings online
                    listalShowRatings.RemoveAll(m => currentUserShowRatings.Any(c => c.Show.Ids.ImdbId == "tt" + m.IMDbId.ToString()));
                }

                UIUtils.UpdateStatus("Importing {0} Listal tv show ratings...", listalShowRatings.Count);
                if (listalShowRatings.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)listalShowRatings.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} Listal show ratings...", i + 1, pages);

                        var response = TraktAPI.AddShowsToRatings(GetRateShowsData(listalShowRatings.Skip(i * pageSize).Take(pageSize).ToList()));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send ratings for Listal tv shows", true);
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

            #endregion

            #region Watchlist
            // Convert Listal Wantlist to a Trakt Watchlist
            if (importWantlist)
            {
                if (importCancelled) return;

                var wantList = listal.Channel.Items.Where(m => m.ListType == ListType.wanted.ToString()).ToList();

                if (wantList.Count > 0)
                {
                    UIUtils.UpdateStatus("Requesting existing watchlist shows from trakt...");
                    var watchlistTraktShows = TraktAPI.GetWatchlistShows();
                    if (watchlistTraktShows != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watchlist shows on trakt", watchlistTraktShows.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watchlist on trakt.tv");
                        wantList.RemoveAll(w => watchlistTraktShows.FirstOrDefault(t => t.Show.Ids.ImdbId == "tt" + w.IMDbId) != null);
                    }
                    if (importCancelled) return;

                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                        // get watched movies from trakt so we don't import shows into watchlist that are already watched
                        var watchedTraktShows = TraktAPI.GetWatchedShows();
                        if (watchedTraktShows != null)
                        {
                            UIUtils.UpdateStatus("Found {0} watched shows on trakt", watchedTraktShows.Count());
                            UIUtils.UpdateStatus("Filtering out watchlist shows containing watched episodes on trakt.tv.");

                            // remove shows from sync list which are watched already
                            wantList.RemoveAll(w => watchedTraktShows.FirstOrDefault(t => t.Show.Ids.ImdbId == "tt" + w.IMDbId) != null);
                        }

                        if (importCancelled) return;
                    }

                    // add movies to watchlist
                    UIUtils.UpdateStatus("Importing {0} Listal Wantlist tv shows to trakt.tv Watchlist...", wantList.Count());

                    if (wantList.Count > 0)
                    {
                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)wantList.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus("Importing page {0}/{1} Listal wantlist tv shows to trakt.tv watchlist...", i + 1, pages);

                            var watchlistShowsResponse = TraktAPI.AddShowsToWatchlist(GetSyncShowsData(wantList.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (watchlistShowsResponse == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watchlist for Listal tv shows", true);
                                Thread.Sleep(2000);
                            }

                            if (importCancelled) return;
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

namespace TraktRater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using global::TraktRater.Settings;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;

    static class Maintenance
    {
        public static bool Cancel { get; set; }

        public static void RemoveEpisodesFromWatchedHistory()
        {
            // get current watched shows
            UIUtils.UpdateStatus("Getting watched shows from trakt.tv");
            var watchedShows = TraktAPI.TraktAPI.GetWatchedShows();
            if (watchedShows != null)
            {
                int i = 0;
                int count = watchedShows.Count();
                UIUtils.UpdateStatus("Found {0} shows with {1} episodes watched ({2} plays) on trakt.tv", count, watchedShows.Sum(w => w.Seasons.Sum(we => we.Episodes.Count())), watchedShows.Sum(s => s.Plays));

                // remove one show at a time
                // there could be many underlying episodes per show
                foreach (var watchedShow in watchedShows)
                {
                    if (Cancel) return;

                    var syncData = new TraktShowSync
                    {
                        Shows = new List<TraktShow>
                        {
                            new TraktShow
                            {
                                Ids = new TraktShowId
                                { 
                                    Trakt = watchedShow.Show.Ids.Trakt
                                }
                            }
                        }
                    };

                    UIUtils.UpdateStatus("[{0}/{1}] Removing all episodes of {2} from trakt.tv watched history", ++i, count, watchedShow.Show.Title);
                    var syncResponse = TraktAPI.TraktAPI.RemoveShowsFromWatchedHistory(syncData);
                    if (syncResponse == null)
                    {
                        UIUtils.UpdateStatus(string.Format("Failed to remove episodes of {0} from trakt.tv watched history", watchedShow.Show.Title), true);
                        Thread.Sleep(2000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watched shows from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void RemoveMoviesFromWatchedHistory()
        {
            // get current watched movies
            UIUtils.UpdateStatus("Getting watched movies from trakt.tv");
            var watchedMovies = TraktAPI.TraktAPI.GetWatchedMovies();
            if (watchedMovies != null)
            {
                UIUtils.UpdateStatus("Found {0} movies watched ({1} plays) on trakt.tv", watchedMovies.Count(), watchedMovies.Sum(s => s.Plays));
                
                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)watchedMovies.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    if (Cancel) return;

                    var syncData = new TraktMovieSync
                    {
                        Movies = watchedMovies.Select(w => w.Movie).Skip(i * pageSize).Take(pageSize).ToList()
                    };

                    UIUtils.UpdateStatus("[{0}/{1}] Removing movies from trakt.tv watched history", i + 1, pages);
                    var syncResponse = TraktAPI.TraktAPI.RemoveMoviesFromWatchedHistory(syncData);
                    if (syncResponse == null)
                    {
                        UIUtils.UpdateStatus(string.Format("[{0}/{1}] Failed to remove movies from trakt.tv watched history", i + 1, pages), true);
                        Thread.Sleep(2000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watched movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void RemoveEpisodesFromCollection()
        {
            // get current watched shows
            UIUtils.UpdateStatus("Getting collected shows from trakt.tv");
            var collectedShows = TraktAPI.TraktAPI.GetCollectedShows();
            if (collectedShows != null)
            {
                int i = 0;
                int count = collectedShows.Count();
                UIUtils.UpdateStatus("Found {0} shows with {1} episodes collected on trakt.tv", count, collectedShows.Sum(c => c.Seasons.Sum(ce => ce.Episodes.Count())));

                // remove one show at a time
                // there could be many underlying episodes per show
                foreach (var collectedShow in collectedShows)
                {
                    if (Cancel) return;

                    var syncData = new TraktShowSync
                    {
                        Shows = new List<TraktShow>
                        {
                            new TraktShow
                            {
                                Ids = new TraktShowId
                                { 
                                    Trakt = collectedShow.Show.Ids.Trakt
                                }
                            }
                        }
                    };

                    UIUtils.UpdateStatus("[{0}/{1}] Removing all episodes of {2} from trakt.tv collection", ++i, count, collectedShow.Show.Title);
                    var syncResponse = TraktAPI.TraktAPI.RemoveShowsFromCollection(syncData);
                    if (syncResponse == null)
                    {
                        UIUtils.UpdateStatus(string.Format("Failed to remove episodes of {0} from trakt.tv collection", collectedShow.Show.Title), true);
                        Thread.Sleep(2000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of collected shows from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void RemoveMoviesFromCollection()
        {
            // get current watched movies
            UIUtils.UpdateStatus("Getting collected movies from trakt.tv");
            var collectedMovies = TraktAPI.TraktAPI.GetCollectedMovies();
            if (collectedMovies != null)
            {
                UIUtils.UpdateStatus("Found {0} movies collected on trakt.tv", collectedMovies.Count());

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)collectedMovies.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    if (Cancel) return;

                    var syncData = new TraktMovieSync
                    {
                        Movies = collectedMovies.Select(c => c.Movie).Skip(i * pageSize).Take(pageSize).ToList()
                    };

                    UIUtils.UpdateStatus("[{0}/{1}] Removing movies from trakt.tv collection", i + 1, pages);
                    var syncResponse = TraktAPI.TraktAPI.RemoveMoviesFromCollection(syncData);
                    if (syncResponse == null)
                    {
                        UIUtils.UpdateStatus(string.Format("[{0}/{1}] Failed to remove movies from trakt.tv collection", i + 1, pages), true);
                        Thread.Sleep(2000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of collected movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void RemoveEpisodesFromRatings()
        {
            // get current rated episodes
            UIUtils.UpdateStatus("Getting rated episodes from trakt.tv");
            var ratedEpisodes = TraktAPI.TraktAPI.GetRatedEpisodes();
            if (ratedEpisodes != null)
            {
                UIUtils.UpdateStatus("Found {0} episodes rated on trakt.tv", ratedEpisodes.Count());

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)ratedEpisodes.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    if (Cancel) return;

                    var syncData = new TraktEpisodeSync
                    {
                        Episodes = ratedEpisodes.Select(r => new TraktEpisode { Ids = r.Episode.Ids })
                                                .Skip(i * pageSize).Take(pageSize).ToList()
                    };

                    UIUtils.UpdateStatus("[{0}/{1}] Removing episodes from trakt.tv ratings", i + 1, pages);
                    var syncResponse = TraktAPI.TraktAPI.RemoveEpisodesFromRatings(syncData);
                    if (syncResponse == null)
                    {
                        UIUtils.UpdateStatus(string.Format("[{0}/{1}] Failed to remove episodes from trakt.tv ratings", i + 1, pages), true);
                        Thread.Sleep(2000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of rated episodes from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void RemoveShowsFromRatings()
        {
            // get current rated episodes
            UIUtils.UpdateStatus("Getting rated shows from trakt.tv");
            var ratedShows = TraktAPI.TraktAPI.GetRatedShows();
            if (ratedShows != null)
            {
                UIUtils.UpdateStatus("Found {0} shows rated on trakt.tv", ratedShows.Count());

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)ratedShows.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    if (Cancel) return;

                    var syncData = new TraktShowSync
                    {
                        Shows = ratedShows.Select(r => r.Show).Skip(i * pageSize).Take(pageSize).ToList()
                    };

                    UIUtils.UpdateStatus("[{0}/{1}] Removing shows from trakt.tv ratings", i + 1, pages);
                    var syncResponse = TraktAPI.TraktAPI.RemoveShowsFromRatings(syncData);
                    if (syncResponse == null)
                    {
                        UIUtils.UpdateStatus(string.Format("[{0}/{1}] Failed to remove shows from trakt.tv ratings", i + 1, pages), true);
                        Thread.Sleep(2000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of rated shows from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }

        public static void RemoveMoviesFromRatings()
        {
            // get current rated episodes
            UIUtils.UpdateStatus("Getting rated movies from trakt.tv");
            var ratedMovies = TraktAPI.TraktAPI.GetRatedMovies();
            if (ratedMovies != null)
            {
                UIUtils.UpdateStatus("Found {0} movies rated on trakt.tv", ratedMovies.Count());

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)ratedMovies.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    if (Cancel) return;

                    var syncData = new TraktMovieSync
                    {
                        Movies = ratedMovies.Select(r => r.Movie).Skip(i * pageSize).Take(pageSize).ToList()
                    };

                    UIUtils.UpdateStatus("[{0}/{1}] Removing movies from trakt.tv ratings", i + 1, pages);
                    var syncResponse = TraktAPI.TraktAPI.RemoveMoviesFromRatings(syncData);
                    if (syncResponse == null)
                    {
                        UIUtils.UpdateStatus(string.Format("[{0}/{1}] Failed to remove movies from trakt.tv ratings", i + 1, pages), true);
                        Thread.Sleep(2000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of rated movies from trakt.tv", true);
                Thread.Sleep(2000);
            }
        }
    }
}

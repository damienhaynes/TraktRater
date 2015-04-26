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
                UIUtils.UpdateStatus("Found {0} shows with {1} episodes watched ({2} plays) on trakt.tv", count, watchedShows.Sum(s => s.Seasons.Sum(se => se.Episodes.Count())), watchedShows.Sum(s => s.Plays));

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
                        Thread.Sleep(1000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watched shows from trakt.tv", true);
                Thread.Sleep(1000);
            }
        }

        public static void RemoveMoviesFromWatchedHistory()
        {
            // get current watched movies
            UIUtils.UpdateStatus("Getting watched movies from trakt.tv");
            var watchedMovies = TraktAPI.TraktAPI.GetWatchedMovies();
            if (watchedMovies != null && watchedMovies.Count() > 0)
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
                        Thread.Sleep(1000);
                        continue;
                    }
                }
            }
            else
            {
                UIUtils.UpdateStatus("Failed to get current list of watched movies from trakt.tv", true);
                Thread.Sleep(1000);
            }
        }
    }
}

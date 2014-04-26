using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraktRater.UI;
using TraktRater.Extensions;
using TraktRater.TraktAPI;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.Sites.API.IMDb;
using TraktRater.Sites.Common.IMDb;
using TraktRater.Logger;
using Microsoft.VisualBasic.FileIO;
using TraktRater.Settings;
using TraktRater.Web;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Threading;

namespace TraktRater.Sites
{
    class IMDbWeb : IRateSite
    {
        bool ImportCancelled = false;
        private string Username;

        public IMDbWeb(string userName, bool enabled)
        {
            Enabled = !string.IsNullOrEmpty(userName) && enabled;
            this.Username = userName;
        }

        #region IRateSite

        public string Name
        {
            get { return "IMDbWeb"; }
        }

        public bool Enabled
        { get; set; }

        public void ImportRatings()
        {
            ImportCancelled = false;

            TraktEpisodes ratedEpisodes = null;
            TraktEpisodes watchlistEpisodes = null;
                        
            List<Dictionary<string, string>> watchlistShows = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> watchlistMovies = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> watchedMovies = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> ratedItems = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> watchlistItems = new List<Dictionary<string, string>>();

            #region Download Data
            UIUtils.UpdateStatus("Reading IMDb ratings from web...");

            WebClient client = new WebClient();
            int count = ratedItems.Count;
            int movieIndex = 1;
            int movieIncrement = 250;

            #region Ratings
            do
            {
                count = ratedItems.Count;
                UI.UIUtils.UpdateStatus(string.Format("Requesting ratings {0} - {1}, Total Results: {2}", movieIndex, (movieIncrement + movieIndex - 1), count));
                string url = "http://www.imdb.com/user/" + Username + "/ratings?start=" + movieIndex + "&view=compact";                
                string response = TraktWeb.Transmit(url, null, false);
                if (response == null) break;
                int begin = 0;

                // only log response when set to trace as it's very verbose in this case
                if (AppSettings.LogSeverityLevel >= AppSettings.LoggingSeverity.Trace)
                    FileLog.Trace("Response: {0}", response); 

                while ((begin = response.IndexOf("<tr data-item-id", begin)) > 0)
                {
                    var rateItem = new Dictionary<string, string>();
                    string sub = response.Substring(begin, response.IndexOf("</tr>", begin) - begin);

                    Regex reg = new Regex("<td class=\"title[^\"]*\"><a href=\"/title/(?<cIMDbID>tt\\d+)/[^\"]*\">(?<cTitle>[^<]+)</a>(?:\\s*<br>\\s*Episode:\\s*<a href=\"/title/(?<cEpisodeID>tt\\d+)/[^\"]*\">(?<cEpisodeTitle>[^<]+)</a>)?</td>");

                    // Get IMDb ID
                    var find = reg.Match(sub);
                    rateItem.Add(IMDbFieldMapping.cIMDbID, find.Groups["cIMDbID"].ToString());
                    
                    // Get Title
                    // If it's a TV Episode then include both show and episode title
                    if (!string.IsNullOrEmpty(find.Groups["cEpisodeTitle"].ToString()))
                        rateItem.Add(IMDbFieldMapping.cTitle, string.Format("{0}: {1}", find.Groups["cTitle"].ToString(), find.Groups["cEpisodeTitle"].ToString()));
                    else
                        rateItem.Add(IMDbFieldMapping.cTitle, find.Groups["cTitle"].ToString());

                    // Get User Rating
                    reg = new Regex("<td class=\"your_ratings\">\\n    <a>([1-9][0-9]{0,1})</a>\\n</td>");
                    find = reg.Match(sub);
                    rateItem.Add(IMDbFieldMapping.cRating, find.Groups[1].ToString());

                    // Get Year
                    reg = new Regex("<td class=\"year\">([1-2][0-9]{3})</td>");
                    find = reg.Match(sub);
                    rateItem.Add(IMDbFieldMapping.cYear, find.Groups[1].ToString());

                    // Get Type
                    reg = new Regex("<td class=\"title_type\"> (.*)</td>");
                    find = reg.Match(sub);
                    if (find.Groups[1].ToString() == string.Empty)
                        rateItem.Add(IMDbFieldMapping.cType, "Feature Film");
                    else
                        rateItem.Add(IMDbFieldMapping.cType, find.Groups[1].ToString());

                    // Set provider to web or csv
                    rateItem.Add(IMDbFieldMapping.cProvider, "web");

                    ratedItems.Add(rateItem);

                    begin += 10;
                }
                // fetch next page
                movieIndex += movieIncrement;
            }
            while (count < ratedItems.Count);
            #endregion

            #region Watchlist
            if (AppSettings.IMDbSyncWatchlist)
            {
                UIUtils.UpdateStatus("Reading IMDb watchlist from web...");
                movieIndex = 1;
                movieIncrement = 100;

                do
                {
                    count = watchlistItems.Count;
                    UI.UIUtils.UpdateStatus(string.Format("Requesting watchlist items {0} - {1}, Total Results: {2}", movieIndex, (movieIncrement + movieIndex - 1), count));
                    string url = "http://www.imdb.com/user/" + Username + "/watchlist?start=" + movieIndex + "&view=compact";
                    string response = TraktWeb.Transmit(url, null, false);
                    if (response == null) break;
                    int begin = 0;

                    // only log response when set to trace as it's very verbose in this case
                    if (AppSettings.LogSeverityLevel >= AppSettings.LoggingSeverity.Trace)
                        FileLog.Trace("Response: {0}", response); 

                    if (response == null) continue;

                    while ((begin = response.IndexOf("<tr data-item-id", begin)) > 0)
                    {
                        var watchListItem = new Dictionary<string, string>();
                        string sub = response.Substring(begin, response.IndexOf("</tr>", begin) - begin);

                        Regex reg = new Regex("<td class=\"title[^\"]*\"><a href=\"/title/(?<cIMDbID>tt\\d+)/[^\"]*\">(?<cTitle>[^<]+)</a>(?:\\s*<br>\\s*Episode:\\s*<a href=\"/title/(?<cEpisodeID>tt\\d+)/[^\"]*\">(?<cEpisodeTitle>[^<]+)</a>)?</td>");

                        // Get IMDb ID
                        var find = reg.Match(sub);
                        watchListItem.Add(IMDbFieldMapping.cIMDbID, find.Groups["cIMDbID"].ToString());

                        // Get Title
                        // If it's a TV Episode then include both show and episode title
                        if (!string.IsNullOrEmpty(find.Groups["cEpisodeTitle"].ToString()))
                            watchListItem.Add(IMDbFieldMapping.cTitle, string.Format("{0}: {1}", find.Groups["cTitle"].ToString(), find.Groups["cEpisodeTitle"].ToString()));
                        else
                            watchListItem.Add(IMDbFieldMapping.cTitle, find.Groups["cTitle"].ToString());

                        // Get Year
                        reg = new Regex("<td class=\"year\">([1-2][0-9]{3})</td>");
                        find = reg.Match(sub);
                        watchListItem.Add(IMDbFieldMapping.cYear, find.Groups[1].ToString());

                        // Get Type
                        reg = new Regex("<td class=\"title_type\"> (.*)</td>");
                        find = reg.Match(sub);
                        if (find.Groups[1].ToString() == string.Empty)
                            watchListItem.Add(IMDbFieldMapping.cType, "Feature Film");
                        else
                            watchListItem.Add(IMDbFieldMapping.cType, find.Groups[1].ToString());

                        // Set provider to web or csv
                        watchListItem.Add(IMDbFieldMapping.cProvider, "web");

                        watchlistItems.Add(watchListItem);
                        
                        begin += 10;
                    }
                    // fetch next page
                    movieIndex += movieIncrement;
                }
                while (count < watchlistItems.Count);
            }
            #endregion
            #endregion

            if (ImportCancelled) return;

            #region Sync Ratings
            #region Movies
            var movies = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie).ToList();
            if (movies.Count() > 0)
            {
                UIUtils.UpdateStatus("Retreiving existing movie ratings from trakt.tv.");
                var currentUserMovieRatings = TraktAPI.TraktAPI.GetUserRatedMovies(AppSettings.TraktUsername);

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count()));
                    // Filter out movies to rate from existing ratings online
                    movies.RemoveAll(m => currentUserMovieRatings.Any(c => c.IMDBID == m[IMDbFieldMapping.cIMDbID]));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} IMDb movie ratings to trakt.tv.", movies.Count()));

                if (movies.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)movies.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} IMDb rated movies...", i + 1, pages));

                        TraktRatingsResponse response = TraktAPI.TraktAPI.RateMovies(Helper.GetRateMoviesData(movies.Skip(i * pageSize).Take(pageSize)));
                        if (response == null || response.Status != "success")
                        {
                            UIUtils.UpdateStatus("Error importing IMDb movie ratings to trakt.tv.", true);
                            Thread.Sleep(2000);
                        }

                        // add to list of movies to mark as watched
                        watchedMovies.AddRange(movies);

                        if (ImportCancelled) return;
                    }
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region TV Shows
            var shows = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show).ToList();
            if (shows.Count() > 0)
            {
                UIUtils.UpdateStatus("Retreiving existing tv show ratings from trakt.tv.");
                var currentUserShowRatings = TraktAPI.TraktAPI.GetUserRatedShows(AppSettings.TraktUsername);

                if (currentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count()));
                    // Filter out shows to rate from existing ratings online
                    shows.RemoveAll(s => currentUserShowRatings.Any(c => (c.IMDBID == s[IMDbFieldMapping.cIMDbID]) || (c.Title == s[IMDbFieldMapping.cTitle] && c.Year.ToString() == s[IMDbFieldMapping.cYear])));
                }

                if (shows.Count > 0)
                {
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb tv show ratings to trakt.tv.", shows.Count()));

                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)shows.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} IMDb rated shows...", i + 1, pages));

                        TraktRatingsResponse response = TraktAPI.TraktAPI.RateShows(Helper.GetRateShowsData(shows.Skip(i * pageSize).Take(pageSize)));
                        if (response == null || response.Status != "success")
                        {
                            UIUtils.UpdateStatus("Error importing IMDb show ratings to trakt.tv.", true);
                            Thread.Sleep(2000);
                        }

                        if (ImportCancelled) return;
                    }
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Episodes
            var episodes = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
            if (episodes.Count() > 0)
            {
                UIUtils.UpdateStatus("Retreiving existing tv episode ratings from trakt.tv.");
                var currentUserEpisodeRatings = TraktAPI.TraktAPI.GetUserRatedEpisodes(AppSettings.TraktUsername);

                if (currentUserEpisodeRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user tv episode ratings on trakt.tv", currentUserEpisodeRatings.Count()));
                    // Filter out shows to rate from existing ratings online
                    episodes.RemoveAll(e => currentUserEpisodeRatings.Any(c => (c.ShowDetails.IMDBID == e[IMDbFieldMapping.cIMDbID] || c.ShowDetails.Title.ToLowerInvariant().StartsWith(Helper.GetShowName(e[IMDbFieldMapping.cTitle]).ToLowerInvariant())) && (c.EpisodeDetails.Title.ToLowerInvariant() == Helper.GetEpisodeName(e[IMDbFieldMapping.cTitle]).ToLowerInvariant())));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} IMDb episode ratings to trakt.tv.", episodes.Count()));

                if (episodes.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)episodes.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        ratedEpisodes = Helper.GetEpisodeData(episodes.Skip(i * pageSize).Take(pageSize));

                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} IMDb rated episodes...", i + 1, pages));

                        TraktRatingsResponse response = TraktAPI.TraktAPI.RateEpisodes(ratedEpisodes);
                        if (response == null || response.Status != "success")
                        {
                            UIUtils.UpdateStatus("Error importing IMDb episodes ratings to trakt.tv.", true);
                            Thread.Sleep(2000);
                        }

                        if (ImportCancelled) return;
                    }
                }
            }
            if (ImportCancelled) return;
            #endregion
            #endregion

            #region Mark as Watched
            if (AppSettings.MarkAsWatched)
            {
                #region Movies
                if (watchedMovies.Count > 0)
                {
                    // mark movies as watched if rated
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb movies as watched...", watchedMovies.Count));

                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)watchedMovies.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} IMDb movies as watched...", i + 1, pages));

                        var watchedMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(Helper.GetSyncMoviesData(watchedMovies.Skip(i * pageSize).Take(pageSize).ToList()), TraktSyncModes.seen);
                        if (watchedMoviesResponse == null || watchedMoviesResponse.Status != "success")
                        {
                            UIUtils.UpdateStatus("Failed to send watched status for IMDb movies.", true);
                            Thread.Sleep(2000);
                        }

                        if (ImportCancelled) return;
                    }
                }
                #endregion

                #region Episodes
                if (ratedEpisodes != null && ratedEpisodes.Episodes.Count() > 0)
                {
                    // mark all episodes as watched if rated
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb episodes as watched...", ratedEpisodes.Episodes.Count));
                    var watchedEpisodes = Helper.GetSyncEpisodeData(ratedEpisodes.Episodes);
                    foreach (var showSyncData in watchedEpisodes)
                    {
                        if (ImportCancelled) return;

                        // send the episodes from each show as watched
                        UIUtils.UpdateStatus(string.Format("Importing {0} episodes of {1} as watched...", showSyncData.EpisodeList.Count(), showSyncData.Title));
                        var watchedEpisodesResponse = TraktAPI.TraktAPI.SyncEpisodeLibrary(showSyncData, TraktSyncModes.seen);
                        if (watchedEpisodesResponse == null || watchedEpisodesResponse.Status != "success")
                        {
                            UIUtils.UpdateStatus(string.Format("Failed to send watched status for IMDb '{0}' episodes.", showSyncData.Title), true);
                            Thread.Sleep(2000);
                            continue;
                        }
                    }
                }
                #endregion
            }
            #endregion
       
            #region Sync Watchlist
            if (AppSettings.IMDbSyncWatchlist)
            {
                #region Movies
                watchlistMovies.AddRange(watchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie));
                if (watchlistMovies.Count() > 0)
                {
                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Requesting watched movies from trakt...");

                        // get watched movies from trakt so we don't import movies into watchlist that are already watched
                        var watchedTraktMovies = TraktAPI.TraktAPI.GetUserWatchedMovies(AppSettings.TraktUsername);
                        if (watchedTraktMovies != null)
                        {
                            UIUtils.UpdateStatus(string.Format("Found {0} watched movies on trakt",watchedTraktMovies.Count()));

                            // remove movies from sync list which are watched already
                            watchlistMovies.RemoveAll(w => watchedTraktMovies.Count(t => t.IMDbId == w[IMDbFieldMapping.cIMDbID]) != 0);
                        }
                    }

                    // add all movies to watchlist
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist movies to trakt.tv ...", watchlistMovies.Count()));

                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)watchlistMovies.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} IMDb movies into watchlist...", i + 1, pages));

                        var watchlistMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(Helper.GetSyncMoviesData(watchlistMovies.Skip(i * pageSize).Take(pageSize).ToList()), TraktSyncModes.watchlist);
                        if (watchlistMoviesResponse == null || watchlistMoviesResponse.Status != "success")
                        {
                            UIUtils.UpdateStatus("Failed to send watchlist for IMDb movies.", true);
                            Thread.Sleep(2000);
                        }

                        if (ImportCancelled) return;
                    }
                }
                #endregion

                #region TV Shows
                IEnumerable<TraktShowWatched> watchedTraktShows = null;
                watchlistShows.AddRange(watchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show));
                if (watchlistShows.Count() > 0)
                {
                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                        // get watched movies from trakt so we don't import shows into watchlist that are already watched
                        watchedTraktShows = TraktAPI.TraktAPI.GetUserWatchedShows(AppSettings.TraktUsername);
                        if (watchedTraktShows != null)
                        {
                            UIUtils.UpdateStatus(string.Format("Found {0} watched shows on trakt", watchedTraktShows.Count()));

                            // remove shows from sync list which are watched already
                            watchlistShows.RemoveAll(w => watchedTraktShows.Count(t => (t.IMDbId == w[IMDbFieldMapping.cIMDbID]) || (t.Title == w[IMDbFieldMapping.cTitle] && t.Year.ToString() == w[IMDbFieldMapping.cYear])) != 0);
                        }
                    }

                    //add all shows to watchlist
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist shows to trakt.tv ...", watchlistShows.Count()));

                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)watchlistShows.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} IMDb shows into watchlist...", i + 1, pages));

                        var watchlistShowsResponse = TraktAPI.TraktAPI.SyncShowLibrary(Helper.GetSyncShowsData(watchlistShows.Skip(i * pageSize).Take(pageSize)), TraktSyncModes.watchlist);
                        if (watchlistShowsResponse == null || watchlistShowsResponse.Status != "success")
                        {
                            UIUtils.UpdateStatus("Failed to send watchlist for IMDb tv shows.", true);
                            Thread.Sleep(2000);
                        }

                        if (ImportCancelled) return;
                    }
                }
                #endregion

                #region Episodes
                episodes = watchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
                if (episodes.Count() > 0)
                {
                    if (AppSettings.IgnoreWatchedForWatchlist)
                    {
                        // we already might have it from the shows sync
                        if (watchedTraktShows == null)
                        {
                            UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                            // get watched movies from trakt so we don't import shows into watchlist that are already watched
                            watchedTraktShows = TraktAPI.TraktAPI.GetUserWatchedShows(AppSettings.TraktUsername);
                        }
                    }

                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist episodes...", episodes.Count()));
                    watchlistEpisodes = Helper.GetEpisodeData(episodes, false);

                    var syncEpisodeData = Helper.GetSyncEpisodeData(watchlistEpisodes.Episodes);

                    foreach (var showSyncData in syncEpisodeData)
                    {
                        if (ImportCancelled) return;

                        // filter out watched episodes
                        if (watchedTraktShows != null)
                        {
                            var traktShow = watchedTraktShows.FirstOrDefault(t => t.IMDbId == showSyncData.IMDBID || (t.Title == showSyncData.Title && t.Year.ToString() == showSyncData.Year));
                            if (traktShow != null)
                            {
                                // check if we have already watched any of the episodes of the show and filter them out from the EpisodeList
                                var unwatchedEpisodes = new List<TraktEpisodeSync.Episode>();
                                foreach(var episode in showSyncData.EpisodeList)
                                {
                                    if (traktShow.Seasons.Count(s => s.Season.ToString() == episode.SeasonIndex && s.Episodes.Contains(int.Parse(episode.EpisodeIndex))) == 0)
                                    {
                                        unwatchedEpisodes.Add(new TraktEpisodeSync.Episode { EpisodeIndex = episode.EpisodeIndex, SeasonIndex = episode.SeasonIndex, LastPlayed = episode.LastPlayed });
                                    }
                                }

                                if (unwatchedEpisodes.Count == 0)
                                {
                                    UIUtils.UpdateStatus(string.Format("No unwatched episodes of {0} found", showSyncData.Title));
                                    continue;
                                }
                                showSyncData.EpisodeList = unwatchedEpisodes;                                
                            }
                        }

                        // send the episodes from each show as watched
                        UIUtils.UpdateStatus(string.Format("Importing {0} episodes of {1} to watchlist...", showSyncData.EpisodeList.Count(), showSyncData.Title));
                        var watchlistEpisodesResponse = TraktAPI.TraktAPI.SyncEpisodeLibrary(showSyncData, TraktSyncModes.watchlist);
                        if (watchlistEpisodesResponse == null || watchlistEpisodesResponse.Status != "success")
                        {
                            UIUtils.UpdateStatus(string.Format("Failed to send watchlist for IMDb '{0}' episodes.", showSyncData.Title), true);
                            Thread.Sleep(2000);
                            continue;
                        }
                    }
                }
                #endregion
            }
            #endregion
        }

        public void Cancel()
        {
            // signals to cancel import
            ImportCancelled = true;
        }

        #endregion
    }
}
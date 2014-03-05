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
using Microsoft.VisualBasic.FileIO;
using TraktRater.Settings;
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

            #region Ratings
            do
            {
                count = ratedItems.Count;
                string s = client.DownloadString("http://www.imdb.com/user/" + Username + "/ratings?start=" + movieIndex + "&view=compact");
                int begin = 0;

                while ((begin = s.IndexOf("<tr data-item-id", begin)) > 0)
                {
                    var rateItem = new Dictionary<string, string>();
                    string sub = s.Substring(begin, s.IndexOf("</tr>", begin) - begin);

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

                    ratedItems.Add(rateItem);

                    begin += 10;
                }
                // fetch next page
                movieIndex += 250;
            }
            while (count < ratedItems.Count);
            #endregion

            #region Watchlist
            if (AppSettings.IMDbSyncWatchlist)
            {
                UIUtils.UpdateStatus("Reading IMDb watchlist from web...");
                movieIndex = 1;

                do
                {
                    count = watchlistItems.Count;
                    string s = client.DownloadString("http://www.imdb.com/user/" + Username + "/watchlist?start=" + movieIndex + "&view=compact");
                    int begin = 0;

                    while ((begin = s.IndexOf("<tr data-item-id", begin)) > 0)
                    {
                        var watchListItem = new Dictionary<string, string>();
                        string sub = s.Substring(begin, s.IndexOf("</tr>", begin) - begin);

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
                        
                        watchlistItems.Add(watchListItem);
                        
                        begin += 10;
                    }
                    // fetch next page
                    movieIndex += 250;
                }
                while (count < watchlistItems.Count);
            }
            #endregion
            #endregion

            if (ImportCancelled) return;

            #region Rate
            #region Movies
            var movies = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie);
            if (movies.Count() > 0)
            {
                UIUtils.UpdateStatus(string.Format("Importing {0} movie ratings to trakt.tv.", movies.Count()));

                TraktRatingsResponse response = TraktAPI.TraktAPI.RateMovies(Helper.GetRateMoviesData(movies));
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Error importing movie ratings to trakt.tv.", true);
                    Thread.Sleep(2000);
                }

                // add to list of movies to mark as watched
                watchedMovies.AddRange(movies);
            }
            if (ImportCancelled) return;
            #endregion

            #region TV Shows
            var shows = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show);
            if (shows.Count() > 0)
            {
                UIUtils.UpdateStatus(string.Format("Importing {0} show ratings to trakt.tv.", shows.Count()));

                TraktRatingsResponse response = TraktAPI.TraktAPI.RateShows(Helper.GetRateShowsData(shows));
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Error importing show ratings to trakt.tv.", true);
                    Thread.Sleep(2000);
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Episodes
            var episodes = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode);
            if (episodes.Count() > 0)
            {
                UIUtils.UpdateStatus(string.Format("Importing {0} episode ratings to trakt.tv.", episodes.Count()));

                ratedEpisodes = Helper.GetEpisodeData(episodes);

                TraktRatingsResponse response = TraktAPI.TraktAPI.RateEpisodes(ratedEpisodes);
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Error importing episodes ratings to trakt.tv.", true);
                    Thread.Sleep(2000);
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
                    // mark all movies as watched if rated
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb Movies as Watched...", watchedMovies.Count));
                    TraktMovieSyncResponse watchedMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(Helper.GetSyncMoviesData(watchedMovies), TraktSyncModes.seen);
                    if (watchedMoviesResponse == null || watchedMoviesResponse.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send watched status for IMDb movies.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }
                }
                #endregion

                #region Episodes
                if (ratedEpisodes.Episodes.Count() > 0)
                {
                    // mark all episodes as watched if rated
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb Episodes as Watched...", ratedEpisodes.Episodes.Count));
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
                    //add all movies to watchlist
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist movies to trakt.tv ...", watchlistMovies.Count()));
                    TraktMovieSyncResponse watchlistMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(Helper.GetSyncMoviesData(watchlistMovies), TraktSyncModes.watchlist);
                    if (watchlistMoviesResponse == null || watchlistMoviesResponse.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send watchlist for IMDb movies.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }
                }
                #endregion

                #region TV Shows
                watchlistShows.AddRange(watchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show));
                if (watchlistShows.Count() > 0)
                {
                    //add all shows to watchlist
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist shows to trakt.tv ...", watchlistShows.Count()));
                    TraktResponse watchlistMoviesResponse = TraktAPI.TraktAPI.SyncShowLibrary(Helper.GetSyncShowsData(watchlistShows), TraktSyncModes.watchlist);
                    if (watchlistMoviesResponse == null || watchlistMoviesResponse.Status != "success")
                    {
                        UIUtils.UpdateStatus("Failed to send watchlist for IMDb shows.", true);
                        Thread.Sleep(2000);
                        if (ImportCancelled) return;
                    }
                }
                #endregion

                #region Episodes
                episodes = watchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode);
                if (episodes.Count() > 0)
                {
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist episodes...", episodes.Count()));
                    watchlistEpisodes = Helper.GetEpisodeData(episodes, false);

                    var syncEpisodeData = Helper.GetSyncEpisodeData(watchlistEpisodes.Episodes);

                    foreach (var showSyncData in syncEpisodeData)
                    {
                        if (ImportCancelled) return;

                        // send the episodes from each show as watched
                        UIUtils.UpdateStatus(string.Format("Importing {0} episodes of {1} to watchlist...", showSyncData.EpisodeList.Count(), showSyncData.Title));
                        var watchedEpisodesResponse = TraktAPI.TraktAPI.SyncEpisodeLibrary(showSyncData, TraktSyncModes.watchlist);
                        if (watchedEpisodesResponse == null || watchedEpisodesResponse.Status != "success")
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
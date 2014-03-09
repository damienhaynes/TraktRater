using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TraktRater.Extensions;
using TraktRater.UI;
using TraktRater.TraktAPI;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.Sites.API.IMDb;
using TraktRater.Sites.Common.IMDb;
using Microsoft.VisualBasic.FileIO;
using TraktRater.Settings;

namespace TraktRater.Sites
{
    public class IMDb : IRateSite
    {
        #region Variables
  
        bool ImportCancelled = false;

        string CSVRatingsFile = null;
        string CSVWatchlistFile = null;

        List<Dictionary<string, string>> RateItems = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> WatchlistItems = new List<Dictionary<string, string>>();

        bool ImportCSVRatings = false;
        bool ImportCSVWatchlist = false;

        #endregion

        #region Constructor

        public IMDb(string ratingsFile, string watchlistFile, bool enabled)
        {
            Enabled = enabled;

            if (!enabled) return;

            // just do a simple null check, we check file existence on parse
            ImportCSVRatings = !string.IsNullOrEmpty(ratingsFile);
            ImportCSVWatchlist = !string.IsNullOrEmpty(watchlistFile);

            CSVRatingsFile = ratingsFile;
            CSVWatchlistFile = watchlistFile;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "IMDb"; } }

        public void ImportRatings()
        {
            ImportCancelled = false;
            List<Dictionary<string, string>> watchedMovies = new List<Dictionary<string,string>>();

            #region Parse Ratings CSV
            UIUtils.UpdateStatus("Reading IMDb ratings export...");
            if (ImportCSVRatings && !ParseCSVFile(CSVRatingsFile, out RateItems))
            {
                UIUtils.UpdateStatus("Failed to parse IMDb ratings file!", true);
                Thread.Sleep(2000);
                return;
            }
            if (ImportCancelled) return;
            #endregion

            #region Parse Watchlist CSV
            UIUtils.UpdateStatus("Reading IMDb watchlist export...");
            if (ImportCSVWatchlist && !ParseCSVFile(CSVWatchlistFile, out WatchlistItems))
            {
                UIUtils.UpdateStatus("Failed to parse IMDb watchlist file!", true);
                Thread.Sleep(2000);
                return;
            }
            if (ImportCancelled) return;
            #endregion

            #region Import Rated Movies
            var movies = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie).ToList();
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

                UIUtils.UpdateStatus(string.Format("Importing {0} movie ratings to trakt.tv.", movies.Count()));

                if (movies.Count > 0)
                {
                    TraktRatingsResponse response = TraktAPI.TraktAPI.RateMovies(Helper.GetRateMoviesData(movies));
                    if (response == null || response.Status != "success")
                    {
                        UIUtils.UpdateStatus("Error importing movie ratings to trakt.tv.", true);
                        Thread.Sleep(2000);
                    }

                    // add to list of movies to mark as watched
                    watchedMovies.AddRange(movies);
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Import Rated TV Shows
            var shows = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show).ToList();
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

                UIUtils.UpdateStatus(string.Format("Importing {0} tv show ratings to trakt.tv.", shows.Count()));

                if (shows.Count > 0)
                {
                    TraktRatingsResponse response = TraktAPI.TraktAPI.RateShows(Helper.GetRateShowsData(shows));
                    if (response == null || response.Status != "success")
                    {
                        UIUtils.UpdateStatus("Error importing tv show ratings to trakt.tv.", true);
                        Thread.Sleep(2000);
                    }
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Import Rated Episodes
            var episodes = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
            TraktEpisodes episodesRated = null;
            if (episodes.Count() > 0)
            {
                UIUtils.UpdateStatus("Retreiving existing tv episode ratings from trakt.tv.");
                var currentUserEpisodeRatings = TraktAPI.TraktAPI.GetUserRatedEpisodes(AppSettings.TraktUsername);

                if (currentUserEpisodeRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user tv episode ratings on trakt.tv", currentUserEpisodeRatings.Count()));
                    // Filter out shows to rate from existing ratings online
                    // IMDb CSV does not contain a IMDb for the show, only episode so we can't use that for matching
                    // For the show name, check the start of the IMDb Title is in the trakt Title (some show names dont match up e.g. Doctor Who -> Doctor Who 2005
                    // We check episode titles as well so the 'starts with' will provide an accurate match.
                    episodes.RemoveAll(e => currentUserEpisodeRatings.Any(c => c.ShowDetails.Title.ToLowerInvariant().StartsWith(Helper.GetShowName(e[IMDbFieldMapping.cTitle]).ToLowerInvariant()) && c.EpisodeDetails.Title.ToLowerInvariant() == Helper.GetEpisodeName(e[IMDbFieldMapping.cTitle]).ToLowerInvariant()));
                }

                UIUtils.UpdateStatus(string.Format("Importing {0} episode ratings to trakt.tv.", episodes.Count()));

                if (episodes.Count > 0)
                {
                    episodesRated = Helper.GetEpisodeData(episodes);

                    TraktRatingsResponse response = TraktAPI.TraktAPI.RateEpisodes(episodesRated);
                    if (response == null || response.Status != "success")
                    {
                        UIUtils.UpdateStatus("Error importing episodes ratings to trakt.tv.", true);
                        Thread.Sleep(2000);
                    }
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Mark Rated Items as Watched
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
                if (episodesRated != null && episodesRated.Episodes.Count() > 0)
                {
                    // mark all episodes as watched if rated
                    UIUtils.UpdateStatus(string.Format("Importing {0} IMDb Episodes as Watched...", episodesRated.Episodes.Count));
                    var watchedEpisodes = Helper.GetSyncEpisodeData(episodesRated.Episodes);
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

            #region Import Watchlist Movies
            movies = WatchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie).ToList();
            if (movies.Count() > 0)
            {
                //add all movies to watchlist
                UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist movies to trakt.tv ...", movies.Count()));
                var watchlistMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(Helper.GetSyncMoviesData(movies), TraktSyncModes.watchlist);
                if (watchlistMoviesResponse == null || watchlistMoviesResponse.Status != "success")
                {
                    UIUtils.UpdateStatus("Failed to send watchlist for IMDb movies.", true);
                    Thread.Sleep(2000);
                    if (ImportCancelled) return;
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Import Watchlist TV Shows
            shows = WatchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show).ToList();
            if (shows.Count() > 0)
            {
                //add all shows to watchlist
                UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist shows to trakt.tv ...", shows.Count()));
                var watchlistShowsResponse = TraktAPI.TraktAPI.SyncShowLibrary(Helper.GetSyncShowsData(shows), TraktSyncModes.watchlist);
                if (watchlistShowsResponse == null || watchlistShowsResponse.Status != "success")
                {
                    UIUtils.UpdateStatus("Failed to send watchlist for IMDb tv shows.", true);
                    Thread.Sleep(2000);
                    if (ImportCancelled) return;
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Import Watchlist Episodes
            episodes = WatchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
            if (episodes.Count() > 0)
            {
                UIUtils.UpdateStatus(string.Format("Importing {0} IMDb watchlist episodes...", episodes.Count()));
                var watchlistEpisodes = Helper.GetEpisodeData(episodes, false);

                var syncEpisodeData = Helper.GetSyncEpisodeData(watchlistEpisodes.Episodes);

                foreach (var showSyncData in syncEpisodeData)
                {
                    if (ImportCancelled) return;

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

            return;
        }

        public void Cancel()
        {
            // signals to cancel import
            ImportCancelled = true;
        }

        #endregion

        #region Private Methods
        
        private bool ParseCSVFile(string filename, out List<Dictionary<string, string>> parsedCSV)
        {
            parsedCSV = new List<Dictionary<string, string>>();

            if (!File.Exists(filename)) return false;

            string[] fieldHeadings = new string[]{};
            
            try
            {
                TextFieldParser parser = new TextFieldParser(filename);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    // processing fields in row
                    string[] fields = parser.ReadFields();

                    // get header fields
                    // line number increments after first read
                    if (parser.LineNumber == 2)
                    {
                        fieldHeadings = fields;
                        continue;
                    }

                    if (fields.Count() != fieldHeadings.Count()) continue;

                    // get each field value
                    int index = 0;
                    var exportItem = new Dictionary<string, string>();

                    foreach (string field in fields)
                    {
                        exportItem.Add(fieldHeadings[index], field);
                        index++;
                    }

                    // Set provider to web or csv
                    exportItem.Add(IMDbFieldMapping.cProvider, "csv");

                    // add to list of items
                    parsedCSV.Add(exportItem);
                }
                parser.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion
    }

}

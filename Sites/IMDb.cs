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
        string CSVFile = null;
        List<Dictionary<string, string>> RateItems = new List<Dictionary<string, string>>();

        #endregion

        #region Constructor

        public IMDb(string filename, bool enabled)
        {
            Enabled = !string.IsNullOrEmpty(filename) && File.Exists(filename) && enabled;
            CSVFile = filename;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "IMDb"; } }

        public void ImportRatings()
        {
            ImportCancelled = false;
            List<Dictionary<string, string>> watchedMovies = new List<Dictionary<string,string>>();            

            UIUtils.UpdateStatus("Reading IMDb ratings export...");
            if (!ParseCSVFile(CSVFile))
            {
                UIUtils.UpdateStatus("Failed to parse IMDb ratings file!", true);
                Thread.Sleep(2000);
                return;
            }
            if (ImportCancelled) return;

            #region Movies
            var movies = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie);
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
            var shows = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show);
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
            var episodes = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode);
            TraktRateEpisodes episodesRated = null;
            if (episodes.Count() > 0)
            {
                UIUtils.UpdateStatus(string.Format("Importing {0} episode ratings to trakt.tv.", episodes.Count()));

                episodesRated = Helper.GetRateEpisodeData(episodes);

                TraktRatingsResponse response = TraktAPI.TraktAPI.RateEpisodes(episodesRated);
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Error importing episodes ratings to trakt.tv.", true);
                    Thread.Sleep(2000);
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region Mark as Watched

            #region Movies

            if (AppSettings.MarkAsWatched && watchedMovies.Count > 0)
            {
                // mark all movies as watched if rated
                UIUtils.UpdateStatus(string.Format("Importing {0} IMDb Movies as Watched...", watchedMovies.Count));
                TraktMovieSyncResponse watchedMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(Helper.GetWatchedMoviesData(watchedMovies), TraktSyncModes.seen);
                if (watchedMoviesResponse == null || watchedMoviesResponse.Status != "success")
                {
                    UIUtils.UpdateStatus("Failed to send watched status for IMDb movies.", true);
                    Thread.Sleep(2000);
                    if (ImportCancelled) return;
                }
            }

            #endregion

            #region Episodes

            if (AppSettings.MarkAsWatched && episodesRated.Episodes.Count() > 0)
            {
                // mark all episodes as watched if rated
                UIUtils.UpdateStatus(string.Format("Importing {0} IMDb Episodes as Watched...", episodesRated.Episodes.Count));
                var watchedEpisodes = Helper.GetWatchedEpisodeData(episodesRated.Episodes);
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
        
        private bool ParseCSVFile(string filename)
        {
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
                    var rateItem = new Dictionary<string, string>();

                    foreach (string field in fields)
                    {
                        rateItem.Add(fieldHeadings[index], field);
                        index++;
                    }

                    // add to list of items
                    RateItems.Add(rateItem);
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

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
        Dictionary<string, TraktShowSummary> ShowSummaries = new Dictionary<string, TraktShowSummary>();

        #endregion

        #region Constructor

        public IMDb(string filename)
        {
            CSVFile = filename;
            Enabled = !string.IsNullOrEmpty(filename) && File.Exists(filename);
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

            // IMDb does not return the season and episode number for TV Episodes
            // so we should filter down to TV Shows and Movies only

            #region Movies
            var movies = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie);
            if (movies.Count() > 0)
            {
                UIUtils.UpdateStatus(string.Format("Importing {0} movie ratings to trakt.tv.", movies.Count()));

                TraktRatingsResponse response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(movies));
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

                TraktRatingsResponse response = TraktAPI.TraktAPI.RateShows(GetRateShowsData(shows));
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

                episodesRated = GetRateEpisodeData(episodes);

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
                TraktMovieSyncResponse watchedMoviesResponse = TraktAPI.TraktAPI.SyncMovieLibrary(GetWatchedMoviesData(watchedMovies), TraktSyncModes.seen);
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
                var watchedEpisodes = GetWatchedEpisodeData(episodesRated.Episodes);
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

        /// <summary>
        /// returns a list of shows with episodes to mark as watched
        /// must send to trakt per show!
        /// </summary>
        private List<TraktEpisodeSync> GetWatchedEpisodeData(List<TraktEpisode> episodes)
        {
            var traktEpisodesSync = new List<TraktEpisodeSync>();

            // seperate episodes list into shows
            foreach (var showId in episodes.Select(e => e.TVDbId).Distinct())
            {
                var episodesInShow = episodes.Where(e => e.TVDbId == showId);
                var episodesWatchedData = new List<TraktEpisodeSync.Episode>();

                if (episodesInShow.Count() == 0) continue;

                episodesWatchedData.AddRange(from episode in episodesInShow
                                             select new TraktEpisodeSync.Episode
                                             {
                                                 EpisodeIndex = episode.Episode.ToString(),
                                                 SeasonIndex = episode.Season.ToString()
                                             });

                if (episodesWatchedData.Count() == 0) continue;

                var episodeSyncData = new TraktEpisodeSync
                {
                    Username = AppSettings.TraktUsername,
                    Password = AppSettings.TraktPassword,
                    EpisodeList = episodesWatchedData,
                    SeriesID = showId.ToString(),
                    Title = episodesInShow.First().Title,
                    Year = episodesInShow.First().Year.ToString()
                };

                traktEpisodesSync.Add(episodeSyncData);
            }

            return traktEpisodesSync;
        }

        private TraktMovieSync GetWatchedMoviesData(List<Dictionary<string, string>> movies)
        {
            var traktMovies = new List<TraktMovieSync.Movie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieSync.Movie
                                 {
                                     IMDBID = movie[IMDbFieldMapping.cIMDbID],
                                     Title = movie[IMDbFieldMapping.cTitle],
                                     Year = int.Parse(movie[IMDbFieldMapping.cYear]).ToString()
                                 });

            var movieWatchedData = new TraktMovieSync
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                MovieList = traktMovies
            };

            return movieWatchedData;
        }

        private TraktRateMovies GetRateMoviesData(IEnumerable<Dictionary<string,string>> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     IMDbId = movie[IMDbFieldMapping.cIMDbID],
                                     Title = movie[IMDbFieldMapping.cTitle],
                                     Year = int.Parse(movie[IMDbFieldMapping.cYear]),
                                     Rating = int.Parse(movie[IMDbFieldMapping.cRating])
                                 });

            var movieRateData = new TraktRateMovies
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Movies = traktMovies
            };

            return movieRateData;
        }

        private TraktRateShows GetRateShowsData(IEnumerable<Dictionary<string, string>> shows)
        {
            var traktShows = new List<TraktShow>();

            traktShows.AddRange(from show in shows
                                 select new TraktShow
                                 {
                                     IMDbId = show[IMDbFieldMapping.cIMDbID],
                                     Title = show[IMDbFieldMapping.cTitle],
                                     Year = int.Parse(show[IMDbFieldMapping.cYear]),
                                     Rating = int.Parse(show[IMDbFieldMapping.cRating])
                                 });

            var movieRateData = new TraktRateShows
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Shows = traktShows
            };

            return movieRateData;
        }

        private TraktRateEpisodes GetRateEpisodeData(IEnumerable<Dictionary<string, string>> episodes)
        {
            var traktEpisodes = new List<TraktEpisode>();

            foreach(var episode in episodes)
            {
                // get the show information
                string showTitle = GetShowName(episode[IMDbFieldMapping.cTitle]);
                if (string.IsNullOrEmpty(showTitle)) continue;

                // get slug of show title
                string slug = showTitle.GenerateSlug();
                if (string.IsNullOrEmpty(slug)) continue;

                TraktShowSummary showSummary = new TraktShowSummary();

                if (!ShowSummaries.TryGetValue(showTitle, out showSummary))
                {
                    // get from online
                    UIUtils.UpdateStatus(string.Format("Retrieving data for {0}", showTitle));
                    showSummary = TraktAPI.TraktAPI.GetShowSummary(slug);
                    if (showSummary == null || showSummary.Seasons == null || showSummary.Seasons.Count == 0)
                    {
                        UIUtils.UpdateStatus(string.Format("Unable to get info for {0}", showTitle), true);
                        continue;
                    }

                    // store show summary
                    ShowSummaries.Add(showTitle, showSummary);
                }
                
                var traktEpisode = GetTraktEpisodeRateData(episode, showSummary);
                if (traktEpisode == null) continue;

                traktEpisodes.Add(traktEpisode);
            }

            var episodeRateData = new TraktRateEpisodes
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Episodes = traktEpisodes
            };

            return episodeRateData;
        }

        /// <summary>
        /// Removes the episode name and returns only the show title
        /// </summary>
        private string GetShowName(string title)
        {
            if (string.IsNullOrEmpty(title)) return null;

            // IMDb populates title field in the form "show: episode"
            // Some shows also include colons so should only remove last one.
            var parts = title.Split(':');
            
            if (parts.Count() == 2)
                return parts[0];

            if (parts.Count() > 2)
                return title.Replace(string.Format(": {0}", parts[parts.Count() - 1].Trim()), string.Empty);

            return title;
        }

        /// <summary>
        /// returns only the episode name part of the title
        /// </summary>
        private string GetEpisodeName(string title)
        {
            if (string.IsNullOrEmpty(title)) return null;

            // IMDb populates title field in the form "show: episode"
            // Some shows also include colons so should only remove last one.
            var parts = title.Split(':');

            if (parts.Count() >= 2)
                return parts[parts.Count() - 1].Trim();

            return null;
        }

        private TraktEpisode GetTraktEpisodeRateData(Dictionary<string,string> episode, TraktShowSummary showSummary)
        {
            if (showSummary == null || showSummary.Seasons == null || showSummary.Seasons.Count == 0)
                return null;

            string episodeTitle = GetEpisodeName(episode[IMDbFieldMapping.cTitle]);
            
            // find episode title in list of episodes from show summary
            if (!string.IsNullOrEmpty(episodeTitle))
            {
                TraktShowSummary.TraktSeason.TraktEpisode match = null;
                foreach (var season in showSummary.Seasons)
                {
                    if (match != null) continue;
                    match = season.Episodes.FirstOrDefault(e => string.Equals(e.Title, episodeTitle, StringComparison.InvariantCultureIgnoreCase));
                }

                if (match != null)
                {
                    return new TraktEpisode
                                {
                                    Episode = match.Episode,
                                    Season = match.Season,
                                    TVDbId = showSummary.TVDbId,
                                    Title = showSummary.Title,
                                    Year = showSummary.Year,
                                    Rating = int.Parse(episode[IMDbFieldMapping.cRating])
                                };
                }
            }

            // we can also lookup by airDate
            string episodeAirDate = episode[IMDbFieldMapping.cReleaseDate];
           
            if (!string.IsNullOrEmpty(episodeAirDate))
            {
                // get epoch date
                long dateTimeEpoch = 0;
                try
                {
                    var splitDate = episodeAirDate.Split('-');
                    // parse date and add 8hours for PST
                    DateTime dateTime = new DateTime(int.Parse(splitDate[0]), int.Parse(splitDate[1]), int.Parse(splitDate[2])).AddHours(8);
                    dateTimeEpoch = dateTime.ToEpoch();
                }
                catch
                {
                    UIUtils.UpdateStatus(string.Format("Unable to get info for {0}", episode[IMDbFieldMapping.cTitle]), true);
                    return null;
                }

                TraktShowSummary.TraktSeason.TraktEpisode match = null;
                foreach (var season in showSummary.Seasons)
                {
                    if (match != null) continue;
                    match = season.Episodes.FirstOrDefault(e => e.FirstAired == dateTimeEpoch);
                }

                if (match != null)
                {
                    return new TraktEpisode
                    {
                        Episode = match.Episode,
                        Season = match.Season,
                        TVDbId = match.TVDbId,
                        Title = showSummary.Title,
                        Year = showSummary.Year,
                        Rating = int.Parse(episode[IMDbFieldMapping.cRating])
                    };
                }
            }

            UIUtils.UpdateStatus(string.Format("Unable to get info for {0}", episode[IMDbFieldMapping.cTitle]), true);
            return null;
        }

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

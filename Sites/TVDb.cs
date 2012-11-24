using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TraktRater;
using TraktRater.TraktAPI;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.Sites.API.TVDb;
using TraktRater.UI;
using TraktRater.Settings;

namespace TraktRater.Sites
{
    public class TVDb : IRateSite
    {
        #region Variables

        string AccountId = string.Empty;
        bool ImportCancelled = false;
    
        #endregion

        #region Constructor

        public TVDb(string accountId)
        {
            AccountId = accountId;
            Enabled = !string.IsNullOrEmpty(accountId) && accountId.Length == 16;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "TVDb"; } }

        public void ImportRatings()
        {
            ImportCancelled = false;

            // get show userratings from theTVDb.com first
            UIUtils.UpdateStatus("Getting show ratings from theTVDb.com.");

            TVDbShowRatings showRatings = TVDbAPI.GetShowRatings(AccountId);

            // if there are no show ratings quit
            if (showRatings == null || showRatings.Shows.Count == 0)
            {
                UIUtils.UpdateStatus("Unable to get list of shows from thetvdb.com.", true);
                return;
            }

            #region Import Show Ratings
            if (ImportCancelled) return;
            UIUtils.UpdateStatus(string.Format("Importing {0} show ratings to trakt.tv.", showRatings.Shows.Count));

            TraktRatingsResponse response = TraktAPI.TraktAPI.RateShows(GetRateShowsData(showRatings));
            if (response == null || response.Status != "success")
            {
                UIUtils.UpdateStatus("Error importing show ratings to trakt.tv.", true);
                Thread.Sleep(2000);
            }
            #endregion

            #region Import Episode Ratings
            int iCounter = 0;
            List<TraktEpisode> episodesRated = new List<TraktEpisode>();

            foreach (var show in showRatings.Shows)
            {
                if (ImportCancelled) return;
                iCounter++;

                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Getting show info for series id {2}", iCounter, showRatings.Shows.Count, show.Id));

                // we need to get the episode/season numbers as trakt api requires this
                // tvdb only returns episode ids, so user series info call to this info
                TVDbShow showInfo = TVDbAPI.GetShowInfo(show.Id.ToString());
                if (showInfo == null)
                {
                    UIUtils.UpdateStatus(string.Format("Unable to show info for series id: {0}", show.Id), true);
                    Thread.Sleep(2000);
                    continue;
                }
                if (ImportCancelled) return;
                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Requesting episode ratings for {2}", iCounter, showRatings.Shows.Count, showInfo.Show.Name));

                // get episode ratings for each show in showratings
                TVDbEpisodeRatings episodeRatings = TVDbAPI.GetEpisodeRatings(AccountId, show.Id.ToString());
                if (episodeRatings == null)
                {
                    UIUtils.UpdateStatus(string.Format("Unable to get episode ratings for {0}", showInfo.Show.Name), true);
                    Thread.Sleep(2000);
                    continue;
                }
                if (ImportCancelled) return;
                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} episode ratings for {3}", iCounter, showRatings.Shows.Count, episodeRatings.Episodes.Count, showInfo.Show.Name));
                if (episodeRatings.Episodes.Count == 0) continue;

                // submit one series at a time
                var episodesToRate = GetRateEpisodeData(episodeRatings, showInfo);
                response = TraktAPI.TraktAPI.RateEpisodes(episodesToRate);
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus(string.Format("Error importing {0} episode ratings to trakt.tv.", showInfo.Show.Name), true);
                    Thread.Sleep(2000);
                    continue;
                }
                episodesRated.AddRange(episodesToRate.Episodes);
            }
            #endregion

            #region Mark As Watched

            if (AppSettings.MarkAsWatched && episodesRated.Count() > 0)
            {
                // mark all episodes as watched if rated
                UIUtils.UpdateStatus(string.Format("Importing {0} TVDb Episodes as Watched...", episodesRated.Count()));
                var watchedEpisodes = GetWatchedEpisodeData(episodesRated);
                foreach (var showSyncData in watchedEpisodes)
                {
                    if (ImportCancelled) return;

                    // send the episodes from each show as watched
                    UIUtils.UpdateStatus(string.Format("Importing {0} episodes of {1} as watched...", showSyncData.EpisodeList.Count(), showSyncData.Title));
                    var watchedEpisodesResponse = TraktAPI.TraktAPI.SyncEpisodeLibrary(showSyncData, TraktSyncModes.seen);
                    if (watchedEpisodesResponse == null || watchedEpisodesResponse.Status != "success")
                    {
                        UIUtils.UpdateStatus(string.Format("Failed to send watched status for TVDb '{0}' episodes.", showSyncData.Title), true);
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
                    UserName = AppSettings.TraktUsername,
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

        private TraktRateShows GetRateShowsData(TVDbShowRatings showRatings)
        {
            List<TraktShow> shows = new List<TraktShow>();

            shows.AddRange(from show in showRatings.Shows
                           select new TraktShow { TVDbId = show.Id, Rating = show.UserRating });

            TraktRateShows showRateData = new TraktRateShows
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Shows = shows
            };

            return showRateData;
        }

        private TraktRateEpisodes GetRateEpisodeData(TVDbEpisodeRatings episodeRatings, TVDbShow showInfo)
        {
            List<TraktEpisode> episodes = new List<TraktEpisode>();

            foreach (var episode in episodeRatings.Episodes)
            {
                // get season/episode info from showInfo object
                var tvdbEpisode = showInfo.Episodes.Find(e => e.Id == episode.Id);
                if (tvdbEpisode == null) continue;

                var traktEpisode = new TraktEpisode
                {
                    Episode = tvdbEpisode.EpisodeNumber,
                    Season = tvdbEpisode.SeasonNumber,
                    TVDbId = episodeRatings.Show.Id,
                    Rating = episode.UserRating,
                    Title = showInfo.Show.Name
                };
                episodes.Add(traktEpisode);
            }

            TraktRateEpisodes episodeRateData = new TraktRateEpisodes
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Episodes = episodes,
            };

            return episodeRateData;
        }

        #endregion

    }
}

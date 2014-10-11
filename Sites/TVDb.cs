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
            UIUtils.UpdateStatus("Getting show ratings from theTVDb.com");

            TVDbShowRatings showRatings = TVDbAPI.GetShowRatings(AccountId);

            // if there are no show ratings quit
            if (showRatings == null || showRatings.Shows.Count == 0)
            {
                UIUtils.UpdateStatus("Unable to get list of shows from thetvdb.com, NOTE: episode ratings can not be retreived from theTVDb.com unless the Show has also been rated!", true);
                return;
            }

            #region Import Show Ratings
            if (ImportCancelled) return;
            UIUtils.UpdateStatus("Retrieving existing tv show ratings from trakt.tv");
            var currentUserShowRatings = TraktAPI.TraktAPI.GetRatedShows();

            var filteredShows = new TVDbShowRatings();
            filteredShows.Shows.AddRange(showRatings.Shows);

            if (currentUserShowRatings != null)
            {
                UIUtils.UpdateStatus(string.Format("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count()));
                UIUtils.UpdateStatus("Filtering out tvdb show ratings that already exist at trakt.tv");

                // Filter out shows to rate from existing ratings online                
                filteredShows.Shows.RemoveAll(s => currentUserShowRatings.Any(c => c.Show.Ids.TvdbId == s.Id));
            }

            UIUtils.UpdateStatus(string.Format("Importing {0} show ratings to trakt.tv", filteredShows.Shows.Count));

            if (filteredShows.Shows.Count > 0)
            {
                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)filteredShows.Shows.Count / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    UIUtils.UpdateStatus(string.Format("Importing page {0}/{1} TVDb rated shows...", i + 1, pages));

                    TraktSyncResponse response = TraktAPI.TraktAPI.SyncShowsRated(GetRateShowsData(filteredShows.Shows.Skip(i * pageSize).Take(pageSize).ToList()));
                    if (response == null)
                    {
                        UIUtils.UpdateStatus("Error importing show ratings to trakt.tv", true);
                        Thread.Sleep(2000);
                    }
                    else if (response.NotFound.Shows.Count > 0)
                    {
                        UIUtils.UpdateStatus(string.Format("Unable to sync ratings of {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count));
                        Thread.Sleep(1000);
                    }
                    if (ImportCancelled) return;
                }
            }
            #endregion

            #region Import Episode Ratings
            int iCounter = 0;
            var episodesRated = new Dictionary<string, List<TraktEpisodeRating>>();

            // get all existing user ratings from trakt.tv
            UIUtils.UpdateStatus("Retrieving existing episode ratings from trakt.tv");
            var currentUserEpisodeRatings = TraktAPI.TraktAPI.GetRatedEpisodes();

            if (currentUserEpisodeRatings != null)
            {
                UIUtils.UpdateStatus(string.Format("Found {0} user tv episode ratings on trakt.tv", currentUserEpisodeRatings.Count()));
            }

            foreach (var show in showRatings.Shows)
            {
                if (ImportCancelled) return;
                iCounter++;

                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Getting show info for tvdb series id {2}", iCounter, showRatings.Shows.Count, show.Id));

                // we need to get the episode/season numbers as trakt api requires this
                // tvdb only returns episode ids, so user series info call to this info
                TVDbShow showInfo = TVDbAPI.GetShowInfo(show.Id.ToString());
                if (showInfo == null)
                {
                    UIUtils.UpdateStatus(string.Format("Unable to get show info for tvdb series id: {0}", show.Id), true);
                    Thread.Sleep(2000);
                    continue;
                }
                if (ImportCancelled) return;
                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Requesting episode ratings for {2} from theTVDb.com", iCounter, showRatings.Shows.Count, showInfo.Show.Name));

                // get episode ratings for each show in showratings
                TVDbEpisodeRatings episodeRatings = TVDbAPI.GetEpisodeRatings(AccountId, show.Id.ToString());
                if (episodeRatings == null)
                {
                    UIUtils.UpdateStatus(string.Format("Unable to get episode ratings for {0} [{1}] from theTVDb.com", showInfo.Show.Name, show.Id), true);
                    Thread.Sleep(2000);
                    continue;
                }
                if (ImportCancelled) return;

                UIUtils.UpdateStatus(string.Format("Found {0} episode ratings for {1} on theTVDb.com", episodeRatings.Episodes.Count, showInfo.Show.Name));

                if (currentUserEpisodeRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Filtering out {0} tvdb episode ratings that already exist at trakt.tv", showInfo.Show.Name));

                    // Filter out episodes to rate from existing ratings online, using tvdb episode id's
                    episodeRatings.Episodes.RemoveAll(e => currentUserEpisodeRatings.Any(c => ((c.Episode.Ids.TvdbId == e.Id))));
                }

                UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} episode ratings for {3}", iCounter, showRatings.Shows.Count, episodeRatings.Episodes.Count, showInfo.Show.Name));
                if (episodeRatings.Episodes.Count == 0) continue;

                // submit one series at a time
                var episodesToRate = GetRateEpisodeData(episodeRatings);
                var response = TraktAPI.TraktAPI.SyncEpisodesRated(episodesToRate);
                if (response == null)
                {
                    UIUtils.UpdateStatus(string.Format("Error importing {0} episode ratings to trakt.tv", showInfo.Show.Name), true);
                    Thread.Sleep(2000);
                    continue;
                }
                else if (response.NotFound.Episodes.Count > 0)
                {
                    UIUtils.UpdateStatus(string.Format("[{0}/{1}] Unable to sync ratings for {2} episodes of {3} as they're not found on trakt.tv!", iCounter, showRatings.Shows.Count, response.NotFound.Episodes.Count, showInfo.Show.Name));
                    Thread.Sleep(1000);
                }
                episodesRated.Add(showInfo.Show.Name, episodesToRate.Episodes);
            }
            #endregion

            #region Mark As Watched

            if (AppSettings.MarkAsWatched && episodesRated.Count() > 0)
            {
                int i = 0;
                foreach (var show in episodesRated)
                {
                    if (ImportCancelled) return;

                    // mark all episodes as watched if rated                
                    UIUtils.UpdateStatus(string.Format("[{0}/{1}] Importing {2} TVDb episodes of {3} as watched to trakt.tv...", i++, episodesRated.Count , show.Value.Count, show.Key));
                    var watchedEpisodes = GetWatchedEpisodeData(show.Value);                 
                    var response = TraktAPI.TraktAPI.SyncEpisodesWatched(watchedEpisodes);
                    if (response == null)
                    {
                        UIUtils.UpdateStatus(string.Format("Failed to send watched status for TVDb '{0}' episodes", show.Key), true);
                        Thread.Sleep(2000);
                        continue;
                    }
                    else if (response.NotFound.Episodes.Count > 0)
                    {
                        UIUtils.UpdateStatus(string.Format("[{0}/{1}] Unable to sync {2} TVDb episodes of {3} as watched as they're not found on trakt.tv!", i, episodesRated.Count, response.NotFound.Episodes.Count, show.Key));
                        Thread.Sleep(1000);
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

        private TraktShowRatingSync GetRateShowsData(List<TVDbShowRatings.Series> shows)
        {
            var tvshows = new List<TraktShowRating>();

            tvshows.AddRange(from show in shows
                             select new TraktShowRating
                             {
                                 Ids = new TraktShowId { TvdbId = show.Id },
                                 Rating = show.UserRating
                             });

            var showRateData = new TraktShowRatingSync
            {
                shows = tvshows
            };

            return showRateData;
        }

        /// <summary>
        /// returns a list of shows with episodes to rate
        /// </summary>
        //private TraktEpisodeRatingSyncEx GetRateEpisodeDataEx(TVDbEpisodeRatings episodeRatings, TVDbShow showInfo)
        //{
        //    // we're only syncing one series of episodes at a time
        //    var showData = new TraktEpisodeRatingSyncEx.TraktShowSeasonsRating
        //    {
        //        Ids = new TraktShowId { TvdbId = episodeRatings.Show.Id },
        //        Title = showInfo.Show.Name,
        //        Seasons = new List<TraktEpisodeRatingSyncEx.TraktShowSeasonsRating.TraktSeasonEpisodesRating>()
        //    };            

        //    foreach (var episode in episodeRatings.Episodes)
        //    {
        //        // get season/episode info from showInfo object
        //        var tvdbEpisode = showInfo.Episodes.Find(e => e.Id == episode.Id);
        //        if (tvdbEpisode == null) continue;
                
        //        // check if we already have the season object
        //        if (!showData.Seasons.Exists(s => s.Number == tvdbEpisode.SeasonNumber))
        //        {
        //            showData.Seasons.Add(new TraktEpisodeRatingSyncEx.TraktShowSeasonsRating.TraktSeasonEpisodesRating { Number = tvdbEpisode.SeasonNumber, Episodes = new List<TraktEpisodeRatingEx>() });
        //        };

        //        // add the episode to the episodes list
        //        var season = showData.Seasons.Find(s => s.Number == tvdbEpisode.SeasonNumber);
        //        if (season == null) continue;

        //        season.Episodes.Add(new TraktEpisodeRatingEx { Number = tvdbEpisode.EpisodeNumber, Rating = episode.UserRating });
        //    }

        //    var episodeRateData = new TraktEpisodeRatingSyncEx
        //    {
        //        // we're only syncing one series of episodes at a time
        //        // i.e. new up a list but only add one series worth of episodes
        //        Shows = new List<TraktEpisodeRatingSyncEx.TraktShowSeasonsRating> { showData }
        //    };

        //    return episodeRateData;
        //}

        private TraktEpisodeRatingSync GetRateEpisodeData(TVDbEpisodeRatings episodeRatings)
        {
            var episodeRateData = new TraktEpisodeRatingSync { Episodes = new List<TraktEpisodeRating>() };

            foreach (var episode in episodeRatings.Episodes)
            {
                episodeRateData.Episodes.Add(new TraktEpisodeRating { Rating = episode.UserRating, Ids = new TraktEpisodeId { TvdbId = episode.Id } });
            }

            return episodeRateData;
        }


        /// <summary>
        /// returns a list of shows with episodes to mark as watched
        /// </summary>
        //private TraktEpisodeWatchedSyncEx GetWatchedEpisodeDataEx(TraktEpisodeRatingSyncEx episodes)
        //{
        //    var show = episodes.Shows.First();

        //    // we're only syncing one series of episodes at a time
        //    var showData = new TraktEpisodeWatchedSyncEx.TraktShowSeasonsWatched
        //    {
        //        Ids = new TraktShowId { TvdbId = show.Ids.TvdbId },
        //        Title = show.Title,
        //        seasons = new List<TraktEpisodeWatchedSyncEx.TraktShowSeasonsWatched.TraktSeasonEpisodesWatched>()
        //    };

        //    foreach (var season in show.Seasons)
        //    {
        //        showData.seasons.Add(new TraktEpisodeWatchedSyncEx.TraktShowSeasonsWatched.TraktSeasonEpisodesWatched { Number = season.Number, Episodes = new List<TraktEpisodeWatchedEx>() });

        //        // add the episodes to the season
        //        var currSeason = showData.seasons.Find(s => s.Number == season.Number);
        //        if (currSeason == null) continue;

        //        foreach (var episode in season.Episodes)
        //        {
        //            currSeason.Episodes.Add(new TraktEpisodeWatchedEx { Number = episode.Number });
        //        }
        //    }

        //    var episodeWatchedData = new TraktEpisodeWatchedSyncEx
        //    {
        //        // we're only syncing one series of episodes at a time
        //        // i.e. new up a list but only add one series worth of episodes
        //        shows = new List<TraktEpisodeWatchedSyncEx.TraktShowSeasonsWatched> { showData }
        //    };

        //    return episodeWatchedData;
        //}

        private TraktEpisodeWatchedSync GetWatchedEpisodeData(List<TraktEpisodeRating> ratedEpisodes)
        {
            var episodeWatchData = new TraktEpisodeWatchedSync { Episodes = new List<TraktEpisodeWatched>() };

            foreach (var episode in ratedEpisodes)
            {
                episodeWatchData.Episodes.Add(new TraktEpisodeWatched { Ids = new TraktEpisodeId { TvdbId = episode.Ids.TvdbId } });
            }

            return episodeWatchData;
        }

        #endregion

    }
}

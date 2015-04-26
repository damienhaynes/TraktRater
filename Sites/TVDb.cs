namespace TraktRater.Sites
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.TVDb;
    using global::TraktRater.TraktAPI;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;

    public class TVDb : IRateSite
    {
        #region Variables

        readonly string accountId = string.Empty;
        bool importCancelled = false;
    
        #endregion

        #region Constructor

        public TVDb(string accountId)
        {
            this.accountId = accountId;
            Enabled = !string.IsNullOrEmpty(accountId) && accountId.Length == 16;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "TVDb"; } }

        public void ImportRatings()
        {
            importCancelled = false;

            // get show userratings from theTVDb.com first
            UIUtils.UpdateStatus("Getting show ratings from theTVDb.com");

            TVDbShowRatings showRatings = TVDbAPI.GetShowRatings(accountId);

            // if there are no show ratings quit
            if (showRatings == null || showRatings.Shows.Count == 0)
            {
                UIUtils.UpdateStatus("Unable to get list of shows from thetvdb.com, NOTE: episode ratings can not be retreived from theTVDb.com unless the Show has also been rated!", true);
                return;
            }

            #region Import Show Ratings
            if (importCancelled) return;
            UIUtils.UpdateStatus("Retrieving existing tv show ratings from trakt.tv");
            var currentUserShowRatings = TraktAPI.GetRatedShows();

            var filteredShows = new TVDbShowRatings();
            filteredShows.Shows.AddRange(showRatings.Shows);

            if (currentUserShowRatings != null)
            {
                UIUtils.UpdateStatus("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count());
                UIUtils.UpdateStatus("Filtering out tvdb show ratings that already exist at trakt.tv");

                // Filter out shows to rate from existing ratings online                
                filteredShows.Shows.RemoveAll(s => currentUserShowRatings.Any(c => c.Show.Ids.TvdbId == s.Id));
            }

            UIUtils.UpdateStatus("Importing {0} show ratings to trakt.tv", filteredShows.Shows.Count);

            if (filteredShows.Shows.Count > 0)
            {
                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)filteredShows.Shows.Count / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    UIUtils.UpdateStatus("Importing page {0}/{1} TVDb rated shows...", i + 1, pages);

                    TraktSyncResponse response = TraktAPI.AddShowsToRatings(GetRateShowsData(filteredShows.Shows.Skip(i * pageSize).Take(pageSize).ToList()));
                    if (response == null)
                    {
                        UIUtils.UpdateStatus("Error importing show ratings to trakt.tv", true);
                        Thread.Sleep(2000);
                    }
                    else if (response.NotFound.Shows.Count > 0)
                    {
                        UIUtils.UpdateStatus("Unable to sync ratings of {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                        Thread.Sleep(1000);
                    }
                    if (importCancelled) return;
                }
            }
            #endregion

            #region Import Episode Ratings
            int iCounter = 0;
            var episodesRated = new Dictionary<string, List<TraktEpisodeRating>>();

            // get all existing user ratings from trakt.tv
            UIUtils.UpdateStatus("Retrieving existing episode ratings from trakt.tv");
            var currentUserEpisodeRatings = TraktAPI.GetRatedEpisodes();

            if (currentUserEpisodeRatings != null)
            {
                UIUtils.UpdateStatus("Found {0} user tv episode ratings on trakt.tv", currentUserEpisodeRatings.Count());
            }

            foreach (var show in showRatings.Shows)
            {
                if (importCancelled) return;
                iCounter++;

                UIUtils.UpdateStatus("[{0}/{1}] Getting show info for tvdb series id {2}", iCounter, showRatings.Shows.Count, show.Id);

                // we need to get the episode/season numbers as trakt api requires this
                // tvdb only returns episode ids, so user series info call to this info
                TVDbShow showInfo = TVDbAPI.GetShowInfo(show.Id.ToString());
                if (showInfo == null)
                {
                    UIUtils.UpdateStatus(string.Format("Unable to get show info for tvdb series id: {0}", show.Id), true);
                    Thread.Sleep(2000);
                    continue;
                }
                if (importCancelled) return;
                UIUtils.UpdateStatus("[{0}/{1}] Requesting episode ratings for {2} from theTVDb.com", iCounter, showRatings.Shows.Count, showInfo.Show.Name);

                // get episode ratings for each show in showratings
                TVDbEpisodeRatings episodeRatings = TVDbAPI.GetEpisodeRatings(accountId, show.Id.ToString());
                if (episodeRatings == null)
                {
                    UIUtils.UpdateStatus(string.Format("Unable to get episode ratings for {0} [{1}] from theTVDb.com", showInfo.Show.Name, show.Id), true);
                    Thread.Sleep(2000);
                    continue;
                }
                if (importCancelled) return;

                UIUtils.UpdateStatus("Found {0} episode ratings for {1} on theTVDb.com", episodeRatings.Episodes.Count, showInfo.Show.Name);

                if (currentUserEpisodeRatings != null)
                {
                    UIUtils.UpdateStatus("Filtering out {0} tvdb episode ratings that already exist at trakt.tv", showInfo.Show.Name);

                    // Filter out episodes to rate from existing ratings online, using tvdb episode id's
                    episodeRatings.Episodes.RemoveAll(e => currentUserEpisodeRatings.Any(c => ((c.Episode.Ids.TvdbId == e.Id))));
                }

                UIUtils.UpdateStatus("[{0}/{1}] Importing {2} episode ratings for {3}", iCounter, showRatings.Shows.Count, episodeRatings.Episodes.Count, showInfo.Show.Name);
                if (episodeRatings.Episodes.Count == 0) continue;

                // submit one series at a time
                var episodesToRate = GetRateEpisodeData(episodeRatings);
                var response = TraktAPI.AddsEpisodesToRatings(episodesToRate);
                if (response == null)
                {
                    UIUtils.UpdateStatus(string.Format("Error importing {0} episode ratings to trakt.tv", showInfo.Show.Name), true);
                    Thread.Sleep(2000);
                    continue;
                }
                else if (response.NotFound.Episodes.Count > 0)
                {
                    UIUtils.UpdateStatus("[{0}/{1}] Unable to sync ratings for {2} episodes of {3} as they're not found on trakt.tv!", iCounter, showRatings.Shows.Count, response.NotFound.Episodes.Count, showInfo.Show.Name);
                    Thread.Sleep(1000);
                }
                episodesRated.Add(showInfo.Show.Name, episodesToRate.Episodes);
            }
            #endregion

            #region Mark As Watched

            if (AppSettings.MarkAsWatched && episodesRated.Any())
            {
                int i = 0;
                foreach (var show in episodesRated)
                {
                    if (importCancelled) return;

                    // mark all episodes as watched if rated                
                    UIUtils.UpdateStatus("[{0}/{1}] Importing {2} TVDb episodes of {3} as watched to trakt.tv...", ++i, episodesRated.Count, show.Value.Count, show.Key);
                    var watchedEpisodes = GetWatchedEpisodeData(show.Value);                 
                    var response = TraktAPI.AddEpisodesToWatchedHistory(watchedEpisodes);
                    if (response == null)
                    {
                        UIUtils.UpdateStatus(string.Format("Failed to send watched status for TVDb '{0}' episodes", show.Key), true);
                        Thread.Sleep(2000);
                    }
                    else if (response.NotFound.Episodes.Count > 0)
                    {
                        UIUtils.UpdateStatus("[{0}/{1}] Unable to sync {2} TVDb episodes of {3} as watched as they're not found on trakt.tv!", i, episodesRated.Count, response.NotFound.Episodes.Count, show.Key);
                        Thread.Sleep(1000);
                    }
                }
            }

            #endregion
        }

        public void Cancel()
        {
            // signals to cancel import
            importCancelled = true;
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
        
        private TraktEpisodeRatingSync GetRateEpisodeData(TVDbEpisodeRatings episodeRatings)
        {
            var episodeRateData = new TraktEpisodeRatingSync { Episodes = new List<TraktEpisodeRating>() };

            foreach (var episode in episodeRatings.Episodes)
            {
                episodeRateData.Episodes.Add(new TraktEpisodeRating { Rating = episode.UserRating, Ids = new TraktEpisodeId { TvdbId = episode.Id } });
            }

            return episodeRateData;
        }

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

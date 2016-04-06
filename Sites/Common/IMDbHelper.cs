namespace TraktRater.Sites.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using global::TraktRater.Extensions;
    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.IMDb;
    using global::TraktRater.Sites.API.TVDb;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;

    public class Helper
    {
        public static TraktMovieSync GetSyncMoviesData(List<Dictionary<string, string>> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     Ids = new TraktMovieId { ImdbId = movie[IMDbFieldMapping.cIMDbID] },
                                     Title = movie[IMDbFieldMapping.cTitle],
                                     Year = movie[IMDbFieldMapping.cYear].ToYear()
                                 });

            var movieData = new TraktMovieSync
            {
                Movies = traktMovies
            };

            return movieData;
        }

        public static TraktMovieWatchedSync GetSyncWatchedMoviesData(List<Dictionary<string, string>> movies)
        {
            var traktMovies = new List<TraktMovieWatched>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieWatched
                                 {
                                     Ids = new TraktMovieId { ImdbId = movie[IMDbFieldMapping.cIMDbID] },
                                     Title = movie[IMDbFieldMapping.cTitle],
                                     Year = movie[IMDbFieldMapping.cYear].ToYear(),
                                     WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : GetLastCreatedDate(movie)
                                 });

            var movieData = new TraktMovieWatchedSync
            {
                Movies = traktMovies
            };

            return movieData;
        }

        public static TraktShowSync GetSyncShowsData(IEnumerable<Dictionary<string, string>> shows)
        {
            var traktShows = new List<TraktShow>();

            traktShows.AddRange(from show in shows
                                select new TraktShow
                                {
                                    Ids = new TraktShowId { ImdbId = show[IMDbFieldMapping.cIMDbID] },
                                    Title = show[IMDbFieldMapping.cTitle],
                                    Year = show[IMDbFieldMapping.cYear].ToYear()
                                });

            var traktShowsData = new TraktShowSync
            {
                Shows = traktShows
            };

            return traktShowsData;
        }

        public static TraktMovieRatingSync GetRateMoviesData(IEnumerable<Dictionary<string, string>> movies)
        {
            var traktMovies = new List<TraktMovieRating>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovieRating
                                 {
                                     Ids = new TraktMovieId { ImdbId = movie[IMDbFieldMapping.cIMDbID] },
                                     Title = movie[IMDbFieldMapping.cTitle],
                                     Year = movie[IMDbFieldMapping.cYear].ToYear(),
                                     Rating = int.Parse(movie[IMDbFieldMapping.cRating]),
                                     RatedAt = GetLastCreatedDate(movie)
                                 });

            var movieRateData = new TraktMovieRatingSync
            {
                movies = traktMovies
            };

            return movieRateData;
        }

        public static TraktShowRatingSync GetRateShowsData(IEnumerable<Dictionary<string, string>> shows)
        {
            var traktShows = new List<TraktShowRating>();

            traktShows.AddRange(from show in shows
                                select new TraktShowRating
                                {
                                    Ids = new TraktShowId { ImdbId = show[IMDbFieldMapping.cIMDbID] },
                                    Title = show[IMDbFieldMapping.cTitle],
                                    Year = show[IMDbFieldMapping.cYear].ToYear(),
                                    Rating = int.Parse(show[IMDbFieldMapping.cRating]),
                                    RatedAt = GetLastCreatedDate(show)
                                });

            var showRateData = new TraktShowRatingSync
            {
                shows = traktShows
            };

            return showRateData;
        }

        public static TraktEpisodeRatingSync GetTraktEpisodeRateData(IEnumerable<IMDbEpisode> episodes)
        {
            if (episodes == null)
                return null;

            var traktEpisodes = new List<TraktEpisodeRating>();

            traktEpisodes.AddRange(from episode in episodes
                                   select new TraktEpisodeRating
                                   {
                                       Ids = new TraktEpisodeId { TvdbId = episode.TvdbId, ImdbId = episode.ImdbId },
                                       Rating = episode.Rating,
                                       RatedAt = GetLastCreatedDate(episode.Created)
                                   });

            var episodesToRate = new TraktEpisodeRatingSync
            {
                Episodes = traktEpisodes
            };

            return episodesToRate;

        }

        public static TraktEpisodeWatchedSync GetTraktEpisodeWatchedData(IEnumerable<IMDbEpisode> episodes)
        {
            if (episodes == null)
                return null;

            var traktEpisodes = new List<TraktEpisodeWatched>();

            traktEpisodes.AddRange(from episode in episodes
                                   select new TraktEpisodeWatched
                                   {
                                       Ids = new TraktEpisodeId { TvdbId = episode.TvdbId, ImdbId = episode.ImdbId },
                                       WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : GetLastCreatedDate(episode.Created)
                                   });

            var episodesWatched = new TraktEpisodeWatchedSync
            {
                Episodes = traktEpisodes
            };

            return episodesWatched;
        }

        public static TraktEpisodeSync GetTraktEpisodeData(IEnumerable<IMDbEpisode> episodes)
        {
            if (episodes == null)
                return null;

            var traktEpisodes = new List<TraktEpisode>();

            traktEpisodes.AddRange(from episode in episodes
                                   select new TraktEpisode
                                   {
                                       Ids = new TraktEpisodeId { TvdbId = episode.TvdbId, ImdbId = episode.ImdbId }
                                   });

            var episodeSync = new TraktEpisodeSync
            {
                Episodes = traktEpisodes
            };

            return episodeSync;
        }
        
        /// <summary>
        /// Removes the episode name and returns only the show title
        /// </summary>
        private static string GetShowName(string title)
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
        private static string GetEpisodeName(string title)
        {
            if (string.IsNullOrEmpty(title)) return null;

            // IMDb populates title field in the form "show: episode"
            // Some shows also include colons so should only remove last one.
            var parts = title.Split(':');

            if (parts.Count() >= 2)
                return parts[parts.Count() - 1].Trim();

            return null;
        }

        public static IMDbEpisode GetIMDbEpisodeFromTVDb(Dictionary<string, string> episode)
        {
            try
            {
                string tvEpisodeName = GetEpisodeName(episode[IMDbFieldMapping.cTitle]);
                string tvShowName = GetShowName(episode[IMDbFieldMapping.cTitle]);
                string tvShowYear = episode[IMDbFieldMapping.cYear];
                string tvShowImdbId = episode[IMDbFieldMapping.cIMDbID];

                // search for the show
                UIUtils.UpdateStatus("Searching for tv show {0} on thetvdb.com", tvShowName);
                var searchResults = TVDbAPI.SearchShow(tvShowName);
                if (searchResults == null)
                {
                    UIUtils.UpdateStatus(string.Format("Failed to search for tv show {0} from thetvdb.com", tvShowName), true);
                    Thread.Sleep(2000);
                    return null;
                }

                // get the first match that contains the same 'year'
                // only if we're using a csv export file 
                var tvdbShowSearchResult = new TVDbShowSearch.Series();
                if (episode[IMDbFieldMapping.cProvider].IsCSVExport())
                {
                    tvdbShowSearchResult = searchResults.Shows.Find(s => s.FirstAired != null && s.FirstAired.Contains(tvShowYear));
                    if (tvdbShowSearchResult == null)
                    {
                        UIUtils.UpdateStatus(string.Format("Failed to search for tv show {0} ({1}) from thetvdb.com", tvShowName, tvShowYear), true);
                        Thread.Sleep(2000);
                        return null;
                    }
                }
                else
                {
                    // the website populates the 'year' with the episode year (not show)
                    // so we can't use that for a show match.
                    // However, the website populates the IMDb using the IMDb ID of the show (not episode).
                    tvdbShowSearchResult = searchResults.Shows.Find(s => s.ImdbId != null && s.ImdbId == tvShowImdbId);
                    if (tvdbShowSearchResult == null)
                    {
                        UIUtils.UpdateStatus(string.Format("Failed to search for tv show {0} (imdb_id:{1}) from thetvdb.com", tvShowName, tvShowImdbId ?? "<empty>"), true);
                        Thread.Sleep(2000);
                        return null;
                    }
                }

                // get the show info for the given show
                UIUtils.UpdateStatus(string.Format("Getting tv show info for {0} [tvdb_id:{1}] on thetvdb.com", tvShowName, tvdbShowSearchResult.Id));
                var tvdbShowInfo = TVDbAPI.GetShowInfo(tvdbShowSearchResult.Id.ToString());
                if (tvdbShowInfo == null)
                {
                    UIUtils.UpdateStatus(string.Format("Failed to get show info for tv show {0} [tvdb_id:{1}] from thetvdb.com", tvShowName, tvdbShowSearchResult.Id), true);
                    Thread.Sleep(2000);
                    return null;
                }

                // we now have a list of episodes from thetvdb.com, we can use the IMDb Episode Title to lookup a tvdb ID
                var tvdbEpisodeInfo = tvdbShowInfo.Episodes.Find(e => e.Name.ToLowerInvariant() == tvEpisodeName.ToLowerInvariant());
                if (tvdbEpisodeInfo == null)
                {
                    // we can also lookup by airDate if using a csv export file
                    if (episode[IMDbFieldMapping.cProvider].IsCSVExport())
                    {
                        string episodeAirDate = null;
                        episode.TryGetValue(IMDbFieldMapping.cReleaseDate, out episodeAirDate);
                        if (!string.IsNullOrEmpty(episodeAirDate))
                        {
                            tvdbEpisodeInfo = tvdbShowInfo.Episodes.Find(e => e.AirDate == episodeAirDate);
                        }

                        // still no luck?
                        if (tvdbEpisodeInfo == null)
                        {
                            UIUtils.UpdateStatus(string.Format("Failed to get episode info for tv show {0} [tvdb_id:{1}] - {2} [AirDate:{3}] from thetvdb.com", tvShowName, tvdbShowSearchResult.Id, tvEpisodeName, episodeAirDate ?? "<empty>"), true);
                            Thread.Sleep(2000);
                            return null;
                        }
                    }
                    else
                    {
                        if (tvdbEpisodeInfo == null)
                        {
                            UIUtils.UpdateStatus(string.Format("Failed to get episode info for tv show {0} [tvdb_id:{1}] - {2} from thetvdb.com", tvShowName, tvdbShowSearchResult.Id, tvEpisodeName), true);
                            Thread.Sleep(2000);
                            return null;
                        }
                    }
                }

                // Note: Web Parsing does not use the IMDb ID for the episode, only the show.
                //       we're also not setting the created date from the webrequest.
                var imdbEpisode = new IMDbEpisode
                {
                    Created = episode[IMDbFieldMapping.cProvider].IsCSVExport() ? episode[IMDbFieldMapping.cCreated] : null,
                    EpisodeName = tvEpisodeName,
                    EpisodeNumber = tvdbEpisodeInfo.EpisodeNumber,
                    ImdbId = episode[IMDbFieldMapping.cProvider].IsCSVExport() ? episode[IMDbFieldMapping.cIMDbID] : null,
                    SeasonNumber = tvdbEpisodeInfo.SeasonNumber,
                    ShowName = tvShowName,
                    TvdbId = tvdbEpisodeInfo.Id
                };

                if (episode.ContainsKey(IMDbFieldMapping.cRating))
                    imdbEpisode.Rating = string.IsNullOrEmpty(episode[IMDbFieldMapping.cRating]) ? 0 : int.Parse(episode[IMDbFieldMapping.cRating]);

                // return the episode
                return imdbEpisode;
            }
            catch (Exception e)
            {
                UIUtils.UpdateStatus(string.Format("Failed to get episode info for '{0}' from thetvdb.com, Reason: '{1}'", episode[IMDbFieldMapping.cTitle], e.Message), true);
                Thread.Sleep(2000);
                return null;
            }
        }

        static string GetLastCreatedDate(Dictionary<string, string> item)
        {
            string createdDate = DateTime.UtcNow.ToString().ToISO8601();

            // check if we have the 'created' field
            if (item.ContainsKey(IMDbFieldMapping.cCreated))
            {
                createdDate = GetLastCreatedDate(item[IMDbFieldMapping.cCreated]);
            }

            return createdDate;
        }

        static string GetLastCreatedDate(string imdbDateTime)
        {
            string createdDate = DateTime.Now.ToString().ToISO8601();

            // check if we have the 'created' field
            if (!string.IsNullOrEmpty(imdbDateTime))
            {
                // date is in the form:
                // Tue Mar  4 00:00:00 2014
                string[] splits = imdbDateTime.Split(new[] { " ", "  " }, StringSplitOptions.RemoveEmptyEntries);
                if (splits.Count() == 5)
                {
                    // make date in form DD MMM YYYY
                    DateTime result;
                    if (DateTime.TryParse(string.Format("{0} {1} {2}", splits[2], splits[1], splits[4]), out result))
                    {
                        createdDate = result.ToString().ToISO8601();
                    }
                }
            }

            return createdDate;
        }
    }
}

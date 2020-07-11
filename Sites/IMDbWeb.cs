namespace TraktRater.Sites
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using CsvHelper;
    using CsvHelper.Configuration;
    using global::TraktRater.Logger;
    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.IMDb;
    using global::TraktRater.Sites.Common;
    using global::TraktRater.TraktAPI;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;
    using global::TraktRater.Web;

    class IMDbWeb : IRateSite
    {
        bool importCancelled = false;
        private readonly string username;

        public IMDbWeb(string userName, bool enabled)
        {
            Enabled = !string.IsNullOrEmpty(userName) && enabled;
            username = userName;
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
            importCancelled = false;

            List<Dictionary<string, string>> watchlistMovies = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> watchlistShows = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> ratedItems = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> watchlistItems = new List<Dictionary<string, string>>();
            List<IMDbListItem> lWatchlistItems = new List<IMDbListItem>();

            #region Download Data
            UIUtils.UpdateStatus("Requesting ratings from IMDb.com...");

            int movieIndex = 0;
            int movieIncrement = 100;
            string paginationKey = string.Empty;
            Regex reg;
            Match find;

            #region Ratings
            do
            {
                UIUtils.UpdateStatus("Requesting ratings {0} - {1}, Total Results: {2}", movieIndex + 1, (movieIncrement + movieIndex), ratedItems.Count);
                string url = "http://www.imdb.com/user/" + username + "/ratings?sort=date_added%2Cdesc&mode=detail&lastPosition=" + movieIndex
                    + (string.IsNullOrEmpty(paginationKey) ? string.Empty : "&paginationKey=" + paginationKey);

                string response = TraktWeb.Transmit(url, null, false);
                if (response == null) break;

                int begin = 0;

                // only log response when set to trace as it's very verbose in this case
                if (AppSettings.LogSeverityLevel >= AppSettings.LoggingSeverity.Trace)
                    FileLog.Trace("Response: {0}", response);

                while ((begin = response.IndexOf("lister-item-content", begin)) > 0)
                {
                    var rateItem = new Dictionary<string, string>();
                    string sub = response.Substring(begin, response.IndexOf("class=\"clear\"", begin) - begin);

                    reg = new Regex("<a href=\"/title/(?<cIMDbID>tt\\d+)/[^\"]*\"\n>(?<cTitle>[^<]*)</a>(?:[.\\s\\S]*Episode:</small>\\s*<a href=\"/title/(?<cEpisodeID>tt\\d+)/[^\"]*\"\n*>(?<cEpisodeTitle>[^<]+)</a>){0,1}");

                    // Get IMDb ID
                    find = reg.Match(sub);
                    rateItem.Add(IMDbFieldMapping.cIMDbID, find.Groups["cIMDbID"].ToString());

                    // Get Title
                    // If it's a TV Episode then include both show and episode title
                    if (!string.IsNullOrEmpty(find.Groups["cEpisodeTitle"].ToString()))
                        rateItem.Add(IMDbFieldMapping.cTitle, string.Format("{0}: {1}", find.Groups["cTitle"], find.Groups["cEpisodeTitle"]));
                    else
                        rateItem.Add(IMDbFieldMapping.cTitle, find.Groups["cTitle"].ToString());

                    // Get User Rating
                    reg = new Regex("<span class=\"ipl-rating-star__rating\">([1-9][0-9]{0,1})</span>");
                    find = reg.Match(sub);
                    rateItem.Add(IMDbFieldMapping.cRating, find.Groups[1].ToString());

                    // Get Year
                    reg = new Regex("<span class=\"lister-item-year text-muted unbold\">\\(([1-2][0-9]{3}).*\\)</span>");
                    find = reg.Match(sub);
                    rateItem.Add(IMDbFieldMapping.cYear, find.Groups[1].ToString());

                    // Get Type
                    reg = new Regex("<span class=\"lister-item-year text-muted unbold\">\\(([1-2][0-9]{3}–).*\\)</span>");
                    find = reg.Match(sub);
                    if (find.Groups[1].ToString() == string.Empty)
                        rateItem.Add(IMDbFieldMapping.cType, "Feature Film");
                    else
                        rateItem.Add(IMDbFieldMapping.cType, "tvseries");

                    // Set provider to web or csv
                    rateItem.Add(IMDbFieldMapping.cProvider, "web");

                    ratedItems.Add(rateItem);

                    begin += 10;
                }
                // fetch next page
                movieIndex += movieIncrement;

                reg = new Regex("<a class=\"flat-button lister-page-next next-page\" href=.*&paginationKey=(?<cPaginationKey>.*)&");
                find = reg.Match(response);
                paginationKey = find.Groups["cPaginationKey"].ToString();
            }
            while (movieIndex == ratedItems.Count);
            #endregion

            #region Watchlist
            if (AppSettings.IMDbSyncWatchlist)
            {
                UIUtils.UpdateStatus("Reading IMDb watchlist from web...");

                string url = "http://www.imdb.com/user/" + username + "/watchlist";
                string response = TraktWeb.Transmit(url, null, false);
                if (response != null)
                {
                    var sub = response.Substring(0, response.IndexOf("</head>", 0));
                    reg = new Regex("ls\\d+");

                    // Get IMDb Watchlist ID
                    find = reg.Match(sub);

                    url = "http://www.imdb.com/list/" + find.Value + "/export";

                    using (StreamReader reader = TraktWeb.GetCsvStream(url))
                    {
                        if (reader == null)
                        {
                            UIUtils.UpdateStatus("Failed to download watchlist from IMDb.", true);
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            lWatchlistItems = ParseCsvFile<IMDbListItem>(reader);
                            if (lWatchlistItems == null)
                            {
                                UIUtils.UpdateStatus("Failed to parse IMDb watchlist file!", true);
                                Thread.Sleep(2000);
                            }
                        }
                    }
                }

                // only log response when set to trace as it's very verbose in this case
                if (AppSettings.LogSeverityLevel >= AppSettings.LoggingSeverity.Trace)
                    FileLog.Trace("Response: {0}", response);
            }
            #endregion
            #endregion

            if (importCancelled) return;

            #region Sync Ratings
            #region Movies
            var movies = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie && !string.IsNullOrEmpty(r[IMDbFieldMapping.cRating])).ToList();
            if (movies.Any())
            {
                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var currentUserMovieRatings = TraktAPI.GetRatedMovies();

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus(string.Format("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count()));
                    // Filter out movies to rate from existing ratings online
                    movies.RemoveAll(m => currentUserMovieRatings.Any(c => c.Movie.Ids.ImdbId == m[IMDbFieldMapping.cIMDbID]));
                }

                UIUtils.UpdateStatus("Importing {0} new IMDb movie ratings to trakt.tv", movies.Count());

                if (movies.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)movies.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb rated movies...", i + 1, pages);

                        TraktSyncResponse response = TraktAPI.AddMoviesToRatings(Helper.GetRateMoviesData(movies.Skip(i * pageSize).Take(pageSize)));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb movie ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} IMDb movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
            }
            if (importCancelled) return;
            #endregion

            #region TV Shows
            var shows = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show && !string.IsNullOrEmpty(r[IMDbFieldMapping.cRating])).ToList();
            if (shows.Any())
            {
                UIUtils.UpdateStatus("Retrieving existing tv show ratings from trakt.tv");
                var currentUserShowRatings = TraktAPI.GetRatedShows();

                if (currentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count());
                    // Filter out shows to rate from existing ratings online
                    shows.RemoveAll(s => currentUserShowRatings.Any(c => (c.Show.Ids.ImdbId == s[IMDbFieldMapping.cIMDbID]) || (c.Show.Title == s[IMDbFieldMapping.cTitle] && c.Show.Year.ToString() == s[IMDbFieldMapping.cYear])));
                }

                if (shows.Count > 0)
                {
                    UIUtils.UpdateStatus("Importing {0} new IMDb tv show ratings to trakt.tv", shows.Count());

                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)shows.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb rated shows...", i + 1, pages);

                        TraktSyncResponse response = TraktAPI.AddShowsToRatings(Helper.GetRateShowsData(shows.Skip(i * pageSize).Take(pageSize)));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb show ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} IMDb shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
            }
            if (importCancelled) return;
            #endregion

            #region Episodes
            var imdbEpisodes = new List<IMDbEpisode>();
            var episodes = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
            if (episodes.Any())
            {
                // we can't rely on the imdb id as trakt most likely wont have the info for episodes

                // search and cache all series info needed for syncing
                // use the tvdb API to first search for each unique series name
                // then GetSeries by TVDb ID to get a list of all episodes
                // each episode will have TVDb ID which we can use for syncing.

                imdbEpisodes.AddRange(episodes.Select(Helper.GetIMDbEpisodeFromTrakt).Where(imdbEpisode => imdbEpisode != null));

                UIUtils.UpdateStatus("Retrieving existing tv episode ratings from trakt.tv");
                var currentUserEpisodeRatings = TraktAPI.GetRatedEpisodes();

                if (currentUserEpisodeRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user tv episode ratings on trakt.tv", currentUserEpisodeRatings.Count());

                    // Filter out episodes to rate from existing ratings online
                    imdbEpisodes.RemoveAll(e => currentUserEpisodeRatings.Any(c => c.Episode.Ids.Trakt == e.TraktId));
                }

                UIUtils.UpdateStatus("Importing {0} episode ratings to trakt.tv", imdbEpisodes.Count());

                if (imdbEpisodes.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)imdbEpisodes.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        var episodesRated = Helper.GetTraktEpisodeRateData(imdbEpisodes.Skip(i * pageSize).Take(pageSize));

                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb rated episodes...", i + 1, pages);

                        var response = TraktAPI.AddsEpisodesToRatings(episodesRated);
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb episodes ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Episodes.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} IMDb episodes as they're not found on trakt.tv!", response.NotFound.Episodes.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
            }
            if (importCancelled) return;
            #endregion
            #endregion

            #region Mark as Watched
            IEnumerable<TraktMoviePlays> watchedTraktMovies = null;

            if (AppSettings.MarkAsWatched)
            {
                #region Movies
                // compare all movies rated against what's not watched on trakt
                movies = ratedItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie && !string.IsNullOrEmpty(r[IMDbFieldMapping.cRating])).ToList();
                if (movies.Count > 0)
                {
                    // get watched movies from trakt.tv
                    UIUtils.UpdateStatus("Requesting watched movies from trakt...");
                    watchedTraktMovies = TraktAPI.GetWatchedMovies();
                    if (watchedTraktMovies == null)
                    {
                        UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv, skipping watched movie import", true);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        if (importCancelled) return;

                        UIUtils.UpdateStatus("Found {0} watched movies on trakt", watchedTraktMovies.Count());
                        UIUtils.UpdateStatus("Filtering out watched movies that are already watched on trakt.tv");

                        movies.RemoveAll(w => watchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID] || (t.Movie.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle] && t.Movie.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);

                        // mark all rated movies as watched
                        UIUtils.UpdateStatus("Importing {0} IMDb movies as watched...", movies.Count);

                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)movies.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus("Importing page {0}/{1} IMDb movies as watched...", i + 1, pages);

                            var response = TraktAPI.AddMoviesToWatchedHistory(Helper.GetSyncWatchedMoviesData(movies.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (response == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watched status for IMDb movies", true);
                                Thread.Sleep(2000);
                            }
                            else if (response.NotFound.Movies.Count > 0)
                            {
                                UIUtils.UpdateStatus("Unable to sync watched state for {0} IMDb movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                                Thread.Sleep(1000);
                            }

                            if (importCancelled) return;
                        }
                    }
                }
                #endregion

                #region Episodes

                if (imdbEpisodes != null && imdbEpisodes.Any())
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)imdbEpisodes.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        var episodesWatched = Helper.GetTraktEpisodeWatchedData(imdbEpisodes.Skip(i * pageSize).Take(pageSize));

                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb watched episodes...", i + 1, pages);

                        var response = TraktAPI.AddEpisodesToWatchedHistory(episodesWatched);
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb episodes as watched to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Episodes.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync {0} IMDb episodes as watched, as they're not found on trakt.tv!", response.NotFound.Episodes.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
                #endregion
            }
            #endregion

            #region Sync Watchlist
            if (AppSettings.IMDbSyncWatchlist)
            {
                #region Movies
                var lWatchlistedMovies = lWatchlistItems.Where(r => r.Type.ItemType() == IMDbType.Movie)
                                                              .Select(s => s.ToTraktMovie()).ToList();
                if (lWatchlistedMovies.Any())
                {
                    UIUtils.UpdateStatus("Requesting existing watchlist movies from trakt...");
                    var lWatchlistTraktMovies = TraktAPI.GetWatchlistMovies();
                    if (lWatchlistTraktMovies != null)
                    {
                        UIUtils.UpdateStatus($"Found {0} watchlist movies on trakt", lWatchlistTraktMovies.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                        lWatchlistedMovies.RemoveAll(w => lWatchlistTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId ||
                                                                                                      (t.Movie.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Movie.Year == w.Year)) != null);
                    }

                    if (AppSettings.IgnoreWatchedForWatchlist && lWatchlistedMovies.Count > 0)
                    {
                        // get watched movies from trakt so we don't import movies into watchlist that are already watched
                        // we may already have this if we imported rated items as watched
                        if (watchedTraktMovies == null)
                        {
                            UIUtils.UpdateStatus("Requesting watched movies from trakt...");
                            watchedTraktMovies = TraktAPI.GetWatchedMovies();
                            if (watchedTraktMovies == null)
                            {
                                UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv", true);
                                Thread.Sleep(2000);
                            }
                        }

                        if (watchedTraktMovies != null)
                        {
                            UIUtils.UpdateStatus($"Found {0} watched movies on trakt", watchedTraktMovies.Count());
                            UIUtils.UpdateStatus("Filtering out watchlist movies that are watched on trakt.tv");

                            // remove movies from sync list which are watched already
                            lWatchlistedMovies.RemoveAll(w => watchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId ||
                                                                                                        (t.Movie.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Movie.Year == w.Year)) != null);
                        }
                    }

                    // add movies to watchlist
                    UIUtils.UpdateStatus($"Importing {0} IMDb watchlist movies to trakt.tv...", lWatchlistedMovies.Count());

                    int lPageSize = AppSettings.BatchSize;
                    int lPages = (int)Math.Ceiling((double)lWatchlistedMovies.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {
                        UIUtils.UpdateStatus($"Importing page {0}/{1} IMDb watchlist movies...", i + 1, lPages);

                        var lWatchlistedToSync = new TraktMovieSync()
                        {
                            Movies = lWatchlistedMovies.Skip(i * lPageSize).Take(lPageSize).ToList()
                        };

                        var lResponse = TraktAPI.AddMoviesToWatchlist(lWatchlistedToSync);
                        if (lResponse == null)
                        {
                            UIUtils.UpdateStatus("Failed to send watchlist for IMDb movies", true);
                            Thread.Sleep(2000);
                        }
                        else if (lResponse.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus($"Unable to sync watchlist for {0} movies as they're not found on trakt.tv!", lResponse.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
                if (importCancelled) return;
                #endregion

                #region TV Shows
                IEnumerable<TraktShowPlays> lWatchedTraktShows = null;
                var lWatchlistedShows = lWatchlistItems.Where(r => r.Type.ItemType() == IMDbType.Show)
                                                             .Select(s => s.ToTraktShow()).ToList();
                if (lWatchlistedShows.Any())
                {
                    UIUtils.UpdateStatus("Requesting existing watchlisted shows from trakt...");
                    var lWatchlistTraktShows = TraktAPI.GetWatchlistShows();
                    if (lWatchlistTraktShows != null)
                    {
                        UIUtils.UpdateStatus($"Found {0} watchlist shows on trakt", lWatchlistTraktShows.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watchlist on trakt.tv");
                        lWatchlistedShows.RemoveAll(w => lWatchlistTraktShows.FirstOrDefault(t => t.Show.Ids.ImdbId == w.Ids.ImdbId ||
                                                                                                    (t.Show.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Show.Year == w.Year)) != null);
                    }

                    if (AppSettings.IgnoreWatchedForWatchlist && lWatchlistedShows.Count > 0)
                    {
                        UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                        // get watched movies from trakt so we don't import shows into watchlist that are already watched
                        lWatchedTraktShows = TraktAPI.GetWatchedShows();
                        if (lWatchedTraktShows != null)
                        {
                            UIUtils.UpdateStatus($"Found {0} watched shows on trakt", lWatchedTraktShows.Count());
                            UIUtils.UpdateStatus("Filtering out watchlist shows containing watched episodes on trakt.tv.");

                            // remove shows from sync list which are watched already
                            lWatchlistedShows.RemoveAll(w => lWatchedTraktShows.FirstOrDefault(t => (t.Show.Ids.ImdbId == w.Ids.ImdbId) ||
                                                                                                       (t.Show.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Show.Year == w.Year)) != null);
                        }
                    }

                    // add shows to watchlist
                    UIUtils.UpdateStatus($"Importing {0} IMDb watchlist shows to trakt.tv...", lWatchlistedShows.Count());

                    int lPageSize = AppSettings.BatchSize;
                    int lPages = (int)Math.Ceiling((double)lWatchlistedShows.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {
                        UIUtils.UpdateStatus($"Importing page {0}/{1} IMDb watchlist shows...", i + 1, lPages);

                        var lWatchlistShowsToSync = new TraktShowSync
                        {
                            Shows = lWatchlistedShows.Skip(i * lPageSize).Take(lPageSize).ToList()
                        };

                        var lResponse = TraktAPI.AddShowsToWatchlist(lWatchlistShowsToSync);
                        if (lResponse == null)
                        {
                            UIUtils.UpdateStatus("Failed to send watchlist for IMDb tv shows", true);
                            Thread.Sleep(2000);
                        }
                        else if (lResponse.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus($"Unable to sync watchlist for {0} shows as they're not found on trakt.tv!", lResponse.NotFound.Shows.Count);
                            Thread.Sleep(1000);
                        }

                        if (importCancelled) return;
                    }
                }
                if (importCancelled) return;
                #endregion

                #region Episodes
                var lImdbEpisodes = new List<IMDbEpisode>();
                var lImdbWatchlistedEpisodes = lWatchlistItems.Where(r => r.Type.ItemType() == IMDbType.Episode).ToList();

                if (lImdbWatchlistedEpisodes.Any())
                {
                    UIUtils.UpdateStatus($"Found {0} IMDb watchlisted episodes", lImdbWatchlistedEpisodes.Count());

                    lImdbEpisodes.AddRange(lImdbWatchlistedEpisodes.Select(Helper.GetIMDbEpisodeFromTrakt).Where(imdbEpisode => imdbEpisode != null));
                    if (importCancelled) return;

                    // filter out existing watchlist episodes
                    UIUtils.UpdateStatus("Requesting existing watchlist episodes from trakt...");
                    var lWatchlistedTraktEpisodes = TraktAPI.GetWatchlistEpisodes();
                    if (lWatchlistedTraktEpisodes != null)
                    {
                        UIUtils.UpdateStatus($"Found {0} watchlist episodes on trakt", lWatchlistedTraktEpisodes.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist episodes that are already in watchlist on trakt.tv");
                        lImdbEpisodes.RemoveAll(e => lWatchlistedTraktEpisodes.FirstOrDefault(w => w.Episode.Ids.Trakt == e.TraktId) != null);
                    }

                    if (AppSettings.IgnoreWatchedForWatchlist && lImdbEpisodes.Count > 0)
                    {
                        // we already might have it from the shows sync
                        if (lWatchedTraktShows == null)
                        {
                            UIUtils.UpdateStatus("Requesting watched episodes from trakt...");

                            // get watched episodes from trakt so we don't import episodes into watchlist that are already watched
                            lWatchedTraktShows = TraktAPI.GetWatchedShows();
                        }

                        if (lWatchedTraktShows != null)
                        {
                            UIUtils.UpdateStatus("Filtering out watchlist episodes containing watched episodes on trakt.tv");

                            // this wont work atm due to show IMDb ID not being set in the IMDbEpisode object
                            lImdbEpisodes.RemoveAll(e => lWatchedTraktShows.Where(s => s.Show.Ids.ImdbId == e.ShowImdbId)
                                                                                                  .Any(s => s.Seasons.Exists(se => se.Number == e.SeasonNumber &&
                                                                                                                                   se.Episodes.Exists(ep => ep.Number == e.EpisodeNumber))));
                        }
                    }

                    UIUtils.UpdateStatus($"Importing {0} episodes in watchlist to trakt.tv", lImdbEpisodes.Count());

                    if (lImdbEpisodes.Count > 0)
                    {
                        int lPageSize = AppSettings.BatchSize;
                        int lPages = (int)Math.Ceiling((double)lImdbEpisodes.Count / lPageSize);
                        for (int i = 0; i < lPages; i++)
                        {
                            UIUtils.UpdateStatus($"Importing page {0}/{1} IMDb watchlisted episodes...", i + 1, lPages);

                            var lResponse = TraktAPI.AddEpisodesToWatchlist(Helper.GetTraktEpisodeData(lImdbEpisodes.Skip(i * lPageSize).Take(lPageSize)));
                            if (lResponse == null)
                            {
                                UIUtils.UpdateStatus("Error importing IMDb episode watchlist to trakt.tv", true);
                                Thread.Sleep(2000);
                            }
                            else if (lResponse.NotFound.Episodes.Count > 0)
                            {
                                UIUtils.UpdateStatus("Unable to sync watchlist for {0} IMDb episodes as they're not found on trakt.tv!", lResponse.NotFound.Episodes.Count);
                                Thread.Sleep(1000);
                            }

                            if (importCancelled) return;
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
            importCancelled = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns records of a IMDb CSV rating or list file.
        /// </summary>
        /// <typeparam name="T">IMDbListItem or IMDbRateItem</typeparam>
        /// <param name="aFileStreamReader">CSV file stream reader</param>
        /// <returns>Records of type T</returns>
        private List<T> ParseCsvFile<T>(StreamReader aFileStreamReader)
        {
            CsvConfiguration csvConfiguration = new CsvConfiguration
            {
                IsHeaderCaseSensitive = false,

                // IMDb use "." for decimal seperator so set culture to cater for this            
                CultureInfo = new System.Globalization.CultureInfo("en-US"),

                // if we're unable parse a row, log the details for analysis
                IgnoreReadingExceptions = true,
                ReadingExceptionCallback = (ex, row) =>
                {
                    FileLog.Error($"Error reading row '{ex.Data["CsvHelper"]}'");
                }
            };
            csvConfiguration.RegisterClassMap<IMDbListCsvMap>();

            using (var csv = new CsvReader(aFileStreamReader, csvConfiguration))
            {
                List<T> records = csv.GetRecords<T>().ToList();
                aFileStreamReader.Close();

                return records;
            }
        }

        #endregion
    }
}
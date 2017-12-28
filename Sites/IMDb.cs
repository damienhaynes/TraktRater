namespace TraktRater.Sites
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Microsoft.VisualBasic.FileIO;

    using global::TraktRater.Logger;
    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.IMDb;
    using global::TraktRater.Sites.Common;
    using global::TraktRater.TraktAPI;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;
    
    public class IMDb : IRateSite
    {
        #region Variables
  
        bool mImportCancelled = false;

        readonly string mRatingsFileCsv = null;
        readonly string mWatchlistFileCsv = null;
        readonly List<string> mCustomListsCsvs = null;

        readonly bool mImportCsvRatings = false;
        readonly bool mImportCsvWatchlist = false;
        readonly bool mImportCsvCustomLists = false;

        #endregion

        #region Constructor

        public IMDb(string aRatingsFile, string aWatchlistFile, List<string> aImdbCustomLists, bool aEnabled)
        {
            Enabled = aEnabled;

            if (!aEnabled) return;

            // just do a simple null check, we check file existence on parse
            mImportCsvRatings = !string.IsNullOrEmpty(aRatingsFile);
            mImportCsvWatchlist = !string.IsNullOrEmpty(aWatchlistFile);
            mImportCsvCustomLists = aImdbCustomLists.Count > 0;

            mRatingsFileCsv = aRatingsFile;
            mWatchlistFileCsv = aWatchlistFile;
            mCustomListsCsvs = aImdbCustomLists;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "IMDb"; } }

        public void ImportRatings()
        {
            mImportCancelled = false;

            var lRateItems = new List<Dictionary<string, string>>();
            var lWatchlistItems = new List<Dictionary<string, string>>();
            var lCustomLists = new Dictionary<string, List<Dictionary<string, string>>>();

            #region Parse Ratings CSV
            UIUtils.UpdateStatus("Reading IMDb ratings export...");
            if (mImportCsvRatings && !ParseCSVFile(mRatingsFileCsv, out lRateItems))
            {
                UIUtils.UpdateStatus("Failed to parse IMDb ratings file!", true);
                Thread.Sleep(2000);
                return;
            }
            if (mImportCancelled) return;
            #endregion

            #region Parse Watchlist CSV
            UIUtils.UpdateStatus("Reading IMDb watchlist export...");
            if (mImportCsvWatchlist && !ParseCSVFile(mWatchlistFileCsv, out lWatchlistItems))
            {
                UIUtils.UpdateStatus("Failed to parse IMDb watchlist file!", true);
                Thread.Sleep(2000);
                return;
            }
            if (mImportCancelled) return;
            #endregion

            #region Parse Custom List CSVs
            UIUtils.UpdateStatus("Reading IMDb custom lists export...");
            if (mImportCsvCustomLists)
            {
                foreach(var list in mCustomListsCsvs)
                {
                    UIUtils.UpdateStatus("Reading IMDb custom list '{0}' export...", list);
                    var lCustomList = new List<Dictionary<string, string>>();

                    if (!ParseCSVFile(list, out lCustomList))
                    {
                        UIUtils.UpdateStatus("Failed to parse IMDb custom list file!", true);
                        Thread.Sleep(2000);
                        return;
                    }

                    lCustomLists.Add(list, lCustomList);
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Rated Movies
            var lMovies = lRateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie && !string.IsNullOrEmpty(r[IMDbFieldMapping.cRating])).ToList();
            FileLog.Info("Found {0} movie ratings in CSV file", lMovies.Count);
            if (lMovies.Any())
            {
                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var lCurrentUserMovieRatings = TraktAPI.GetRatedMovies();

                if (lCurrentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user movie ratings on trakt.tv", lCurrentUserMovieRatings.Count());
                    // Filter out movies to rate from existing ratings online
                    lMovies.RemoveAll(m => lCurrentUserMovieRatings.Any(c => c.Movie.Ids.ImdbId == m[IMDbFieldMapping.cIMDbID]));
                }

                UIUtils.UpdateStatus("Importing {0} new movie ratings to trakt.tv", lMovies.Count());

                if (lMovies.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lMovies.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb rated movies...", i + 1, pages);

                        TraktSyncResponse response = TraktAPI.AddMoviesToRatings(Helper.GetRateMoviesData(lMovies.Skip(i * pageSize).Take(pageSize)));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb movie ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Rated TV Shows
            var lShows = lRateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show && !string.IsNullOrEmpty(r[IMDbFieldMapping.cRating])).ToList();
            FileLog.Info("Found {0} tv show ratings in CSV file", lShows.Count);
            if (lShows.Any())
            {
                UIUtils.UpdateStatus("Retrieving existing tv show ratings from trakt.tv");
                var currentUserShowRatings = TraktAPI.GetRatedShows();

                if (currentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count());
                    // Filter out shows to rate from existing ratings online
                    lShows.RemoveAll(s => currentUserShowRatings.Any(c => (c.Show.Ids.ImdbId == s[IMDbFieldMapping.cIMDbID]) || (c.Show.Title == s[IMDbFieldMapping.cTitle] && c.Show.Year.ToString() == s[IMDbFieldMapping.cYear])));
                }

                UIUtils.UpdateStatus("Importing {0} tv show ratings to trakt.tv", lShows.Count());

                if (lShows.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lShows.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb rated shows...", i + 1, pages);

                        TraktSyncResponse response = TraktAPI.AddShowsToRatings(Helper.GetRateShowsData(lShows.Skip(i * pageSize).Take(pageSize)));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb tv show ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync ratings for {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Rated Episodes
            var lImdbEpisodes = new List<IMDbEpisode>();
            var lImdbCsvEpisodes = lRateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
            FileLog.Info("Found {0} tv episode ratings in CSV file", lImdbCsvEpisodes.Count);
            if (lImdbCsvEpisodes.Any())
            {
                // we can't rely on the imdb id as trakt most likely wont have the info for episodes

                // search and cache all series info needed for syncing
                // use the tvdb API to first search for each unique series name
                // then GetSeries by TVDb ID to get a list of all episodes
                // each episode will have TVDb ID which we can use for syncing.

                lImdbEpisodes.AddRange(lImdbCsvEpisodes.Select(Helper.GetIMDbEpisodeFromTVDb).Where(imdbEpisode => imdbEpisode != null));

                UIUtils.UpdateStatus("Retrieving existing tv episode ratings from trakt.tv");
                var currentUserEpisodeRatings = TraktAPI.GetRatedEpisodes();

                if (currentUserEpisodeRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user tv episode ratings on trakt.tv", currentUserEpisodeRatings.Count());

                    // Filter out episodes to rate from existing ratings online
                    lImdbEpisodes.RemoveAll(e => currentUserEpisodeRatings.Any(c => c.Episode.Ids.TvdbId == e.TvdbId || c.Episode.Ids.ImdbId == e.ImdbId));
                }

                UIUtils.UpdateStatus("Importing {0} episode ratings to trakt.tv", lImdbEpisodes.Count());

                if (lImdbEpisodes.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lImdbEpisodes.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        var episodesRated = Helper.GetTraktEpisodeRateData(lImdbEpisodes.Skip(i * pageSize).Take(pageSize));

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

                        if (mImportCancelled) return;
                    }
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Mark Rated Items as Watched
            IEnumerable<TraktMoviePlays> lWatchedTraktMovies = null;

            if (AppSettings.MarkAsWatched)
            {
                #region Movies
                // compare all movies rated against what's not watched on trakt
                lMovies = lRateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie).ToList();
                FileLog.Info("Found {0} movies in CSV file", lMovies.Count);
                if (lMovies.Count > 0)
                {
                    // get watched movies from trakt.tv
                    UIUtils.UpdateStatus("Requesting watched movies from trakt...");
                    lWatchedTraktMovies = TraktAPI.GetWatchedMovies();
                    if (lWatchedTraktMovies == null)
                    {
                        UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv, skipping watched movie import", true);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        if (mImportCancelled) return;

                        UIUtils.UpdateStatus("Found {0} watched movies on trakt", lWatchedTraktMovies.Count());
                        UIUtils.UpdateStatus("Filtering out watched movies that are already watched on trakt.tv");

                        lMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID] || (t.Movie.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle] && t.Movie.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);

                        // mark all rated movies as watched
                        UIUtils.UpdateStatus("Importing {0} IMDb movies as watched...", lMovies.Count);

                        int pageSize = AppSettings.BatchSize;
                        int pages = (int)Math.Ceiling((double)lMovies.Count / pageSize);
                        for (int i = 0; i < pages; i++)
                        {
                            UIUtils.UpdateStatus("Importing page {0}/{1} IMDb movies as watched...", i + 1, pages);

                            var response = TraktAPI.AddMoviesToWatchedHistory(Helper.GetSyncWatchedMoviesData(lMovies.Skip(i * pageSize).Take(pageSize).ToList()));
                            if (response == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watched status for IMDb movies to trakt.tv", true);
                                Thread.Sleep(2000);
                            }
                            else if (response.NotFound.Movies.Count > 0)
                            {
                                UIUtils.UpdateStatus("Unable to sync watched states for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                                Thread.Sleep(1000);
                            }
                            if (mImportCancelled) return;
                        }
                    }
                }
                #endregion

                #region Episodes
                if (lImdbEpisodes != null && lImdbEpisodes.Any())
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lImdbEpisodes.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        var episodesWatched = Helper.GetTraktEpisodeWatchedData(lImdbEpisodes.Skip(i * pageSize).Take(pageSize));

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

                        if (mImportCancelled) return;
                    }
                }
                #endregion
            }
            #endregion

            #region Import Watchlist Movies
            lMovies = lWatchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie).ToList();
            FileLog.Info("Found {0} movies watchlisted in CSV file", lMovies.Count);
            if (lMovies.Any())
            {
                UIUtils.UpdateStatus("Requesting existing watchlist movies from trakt...");
                var watchlistTraktMovies = TraktAPI.GetWatchlistMovies();
                if (watchlistTraktMovies != null)
                {
                    UIUtils.UpdateStatus("Found {0} watchlist movies on trakt", watchlistTraktMovies.Count());
                    UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                    lMovies.RemoveAll(w => watchlistTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID] || (t.Movie.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle] && t.Movie.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && lMovies.Count > 0)
                {
                    UIUtils.UpdateStatus("Requesting watched movies from trakt...");

                    // get watched movies from trakt so we don't import movies into watchlist that are already watched
                    if (lWatchedTraktMovies != null)
                    {
                        lWatchedTraktMovies = TraktAPI.GetWatchedMovies();
                        if (lWatchedTraktMovies == null)
                        {
                            UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                    }

                    if (lWatchedTraktMovies != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watched movies on trakt", lWatchedTraktMovies.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist movies that are watched on trakt.tv");

                        // remove movies from sync list which are watched already
                        lMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID] || (t.Movie.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle] && t.Movie.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);
                    }
                }

                // add movies to watchlist
                UIUtils.UpdateStatus("Importing {0} IMDb watchlist movies to trakt.tv...", lMovies.Count());

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)lMovies.Count / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    UIUtils.UpdateStatus("Importing page {0}/{1} IMDb watchlist movies...", i + 1, pages);

                    var response = TraktAPI.AddMoviesToWatchlist(Helper.GetSyncMoviesData(lMovies.Skip(i * pageSize).Take(pageSize).ToList()));
                    if (response == null)
                    {
                        UIUtils.UpdateStatus("Failed to send watchlist for IMDb movies", true);
                        Thread.Sleep(2000);
                    }
                    else if (response.NotFound.Movies.Count > 0)
                    {
                        UIUtils.UpdateStatus("Unable to sync watchlist for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                        Thread.Sleep(1000);
                    }

                    if (mImportCancelled) return;
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Watchlist TV Shows
            IEnumerable<TraktShowPlays> watchedTraktShows = null;
            lShows = lWatchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show).ToList();
            FileLog.Info("Found {0} tv shows watchlisted in CSV file", lShows.Count);
            if (lShows.Any())
            {
                UIUtils.UpdateStatus("Requesting existing watchlist shows from trakt...");
                var watchlistTraktShows = TraktAPI.GetWatchlistShows();
                if (watchlistTraktShows != null)
                {
                    UIUtils.UpdateStatus("Found {0} watchlist shows on trakt", watchlistTraktShows.Count());
                    UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watchlist on trakt.tv");
                    lShows.RemoveAll(w => watchlistTraktShows.FirstOrDefault(t => t.Show.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID] || (t.Show.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle].ToLowerInvariant() && t.Show.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && lShows.Count > 0)
                {
                    UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                    // get watched movies from trakt so we don't import shows into watchlist that are already watched
                    watchedTraktShows = TraktAPI.GetWatchedShows();
                    if (watchedTraktShows != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watched shows on trakt", watchedTraktShows.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist shows containing watched episodes on trakt.tv.");

                        // remove shows from sync list which are watched already
                        lShows.RemoveAll(w => watchedTraktShows.FirstOrDefault(t => (t.Show.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID]) || (t.Show.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle].ToLowerInvariant() && t.Show.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);
                    }
                }

                // add shows to watchlist
                UIUtils.UpdateStatus("Importing {0} IMDb watchlist shows to trakt.tv...", lShows.Count());

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)lShows.Count / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    UIUtils.UpdateStatus("Importing page {0}/{1} IMDb watchlist shows...", i + 1, pages);

                    var response = TraktAPI.AddShowsToWatchlist(Helper.GetSyncShowsData(lShows.Skip(i * pageSize).Take(pageSize)));
                    if (response == null)
                    {
                        UIUtils.UpdateStatus("Failed to send watchlist for IMDb tv shows", true);
                        Thread.Sleep(2000);
                    }
                    else if (response.NotFound.Shows.Count > 0)
                    {
                        UIUtils.UpdateStatus("Unable to sync watchlist for {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                        Thread.Sleep(1000);
                    }

                    if (mImportCancelled) return;
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Watchlist Episodes
            lImdbEpisodes.Clear();
            lImdbCsvEpisodes = lWatchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
            FileLog.Info("Found {0} tv episodes watchlisted in CSV file", lImdbCsvEpisodes.Count);
            if (lImdbCsvEpisodes.Any())
            {
                UIUtils.UpdateStatus("Found {0} IMDb watchlist episodes", lImdbCsvEpisodes.Count());

                lImdbEpisodes.AddRange(lImdbCsvEpisodes.Select(Helper.GetIMDbEpisodeFromTVDb).Where(imdbEpisode => imdbEpisode != null));

                // filter out existing watchlist episodes
                UIUtils.UpdateStatus("Requesting existing watchlist episodes from trakt...");
                var watchlistTraktEpisodes = TraktAPI.GetWatchlistEpisodes();
                if (watchlistTraktEpisodes != null)
                {
                    UIUtils.UpdateStatus("Found {0} watchlist episodes on trakt", watchlistTraktEpisodes.Count());
                    UIUtils.UpdateStatus("Filtering out watchlist episodes that are already in watchlist on trakt.tv");
                    lImdbEpisodes.RemoveAll(e => watchlistTraktEpisodes.FirstOrDefault(w => w.Episode.Ids.ImdbId == e.ImdbId || w.Episode.Ids.TvdbId == e.TvdbId) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && lImdbEpisodes.Count > 0)
                {
                    // we already might have it from the shows sync
                    if (watchedTraktShows == null)
                    {
                        UIUtils.UpdateStatus("Requesting watched episodes from trakt...");

                        // get watched episodes from trakt so we don't import episodes into watchlist that are already watched
                        watchedTraktShows = TraktAPI.GetWatchedShows();
                    }

                    if (watchedTraktShows != null)
                    {
                        UIUtils.UpdateStatus("Filtering out watchlist episodes containing watched episodes on trakt.tv");

                        lImdbEpisodes.RemoveAll(e => watchedTraktShows.Where(s => s.Show.Ids.ImdbId == e.ImdbId)
                                                                         .Any(s => s.Seasons.Exists(se => se.Number == e.SeasonNumber && se.Episodes.Exists(ep => ep.Number == e.EpisodeNumber))));
                    }
                }

                UIUtils.UpdateStatus("Importing {0} episodes in watchlist to trakt.tv", lImdbEpisodes.Count());

                if (lImdbEpisodes.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lImdbEpisodes.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb watchlist episodes...", i + 1, pages);

                        var response = TraktAPI.AddEpisodesToWatchlist(Helper.GetTraktEpisodeData(lImdbEpisodes.Skip(i * pageSize).Take(pageSize)));
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb episode watchlist to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Episodes.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync watchlist for {0} IMDb episodes as they're not found on trakt.tv!", response.NotFound.Episodes.Count);
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                }
            }
            #endregion

            #region Import Custom Lists
            if (lCustomLists.Count > 0)
            {
                UIUtils.UpdateStatus("Requesting custom lists from trakt...");
                var lTraktCustomLists = TraktAPI.GetCustomLists();
                if (lTraktCustomLists == null)
                {
                    UIUtils.UpdateStatus("Error requesting custom lists from trakt.tv", true);
                    Thread.Sleep(2000);
                    return;
                }

                UIUtils.UpdateStatus("Found {0} custom lists on trakt.tv", lTraktCustomLists.Count());

                foreach (var list in lCustomLists)
                {
                    bool lListCreated = false;
                    string lListName = Path.GetFileNameWithoutExtension(list.Key);

                    // create the list if we don't have it
                    TraktListDetail lTraktCustomList = lTraktCustomLists.FirstOrDefault(l => l.Name == lListName);

                    if (lTraktCustomList == null)
                    {
                        UIUtils.UpdateStatus("Creating new custom list '{0}' on trakt.tv", lListName);
                        var lTraktList = new TraktList
                        {
                            Name = lListName,
                            DisplayNumbers = true,
                        };

                        lTraktCustomList = TraktAPI.CreateCustomList(lTraktList);
                        if (lTraktCustomList == null)
                        {
                            UIUtils.UpdateStatus("Error creating custom list on trakt.tv, skipping list creation", true);
                            Thread.Sleep(2000);
                            continue;
                        }

                        lListCreated = true;
                    }

                    // get the CSV list items parsed
                    var lIMDbListItems = list.Value;
                    
                    var lImdbListMovies = lIMDbListItems.Where(l => l.ItemType() == IMDbType.Movie).ToList();
                    var lImdbListShows = lIMDbListItems.Where(l => l.ItemType() == IMDbType.Show).ToList();

                    // if the list already exists, get current items for list 
                    if (!lListCreated)
                    {
                        lTraktCustomList = lTraktCustomLists.FirstOrDefault(l => l.Name == lListName);
                        
                        UIUtils.UpdateStatus("Requesting existing custom list '{0}' items from trakt...", lListName);
                        var lTraktListItems = TraktAPI.GetCustomListItems(lTraktCustomList.Ids.Trakt.ToString());
                        if (lTraktListItems == null)
                        {
                            UIUtils.UpdateStatus("Error requesting custom list items on trakt.tv, skipping list creation", true);
                            Thread.Sleep(2000);
                            continue;
                        }

                        // filter out existing items from CSV so we don't send again
                        FileLog.Info("Filtering out existing items from IMDb list '{0}' so we don't send again to trakt.tv", lListName);
                        lImdbListMovies.RemoveAll(d => d.ItemType() == IMDbType.Movie && lTraktListItems.FirstOrDefault(l => l.Movie.Ids.ImdbId == d[IMDbFieldMapping.cIMDbID]) != null);
                        lImdbListShows.RemoveAll(d => d.ItemType() == IMDbType.Show && lTraktListItems.FirstOrDefault(l => l.Show.Ids.ImdbId == d[IMDbFieldMapping.cIMDbID]) != null);
                    }

                    #region Movies

                    UIUtils.UpdateStatus("Importing {0} movies into {1} custom list...", lImdbListMovies.Count(), lListName);

                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)lImdbListMovies.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb custom list movies...", i + 1, pages);

                        // create list sync object to hold list items
                        var lTraktMovieSync = new TraktSyncAll
                        {
                            Movies = Helper.GetSyncMoviesData(lImdbListMovies.Skip(i * pageSize).Take(pageSize).ToList()).Movies
                        };

                        var response = TraktAPI.AddItemsToList(lTraktCustomList.Ids.Trakt.ToString(), lTraktMovieSync);
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send custom list items for IMDb movies", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync custom list items for {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                    #endregion

                    #region Shows

                    UIUtils.UpdateStatus("Importing {0} shows into {1} custom list...", lImdbListShows.Count(), lListName);

                    pageSize = AppSettings.BatchSize;
                    pages = (int)Math.Ceiling((double)lImdbListShows.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb custom list shows...", i + 1, pages);

                        // create list sync object to hold list items
                        var lTraktShowSync = new TraktSyncAll
                        {
                            Shows = Helper.GetSyncShowsData(lImdbListShows.Skip(i * pageSize).Take(pageSize).ToList()).Shows
                        };

                        var response = TraktAPI.AddItemsToList(lTraktCustomList.Ids.Trakt.ToString(), lTraktShowSync);
                        if (response == null)
                        {
                            UIUtils.UpdateStatus("Failed to send custom list items for IMDb shows", true);
                            Thread.Sleep(2000);
                        }
                        else if (response.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync custom list items for {0} shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }

                    #endregion
                }
            }
            #endregion
        }

        public void Cancel()
        {
            // signals to cancel import
            mImportCancelled = true;
        }

        #endregion

        #region Private Methods
        
        private bool ParseCSVFile(string aFilename, out List<Dictionary<string, string>> aParsedCSV)
        {
            aParsedCSV = new List<Dictionary<string, string>>();

            if (!File.Exists(aFilename)) return false;

            string[] lFieldHeadings = new string[]{};
            int lRecordNumber = 0;
            
            try
            {
                var lParser = new TextFieldParser(aFilename) { TextFieldType = FieldType.Delimited };
                lParser.SetDelimiters(",");
                while (!lParser.EndOfData)
                {
                    lRecordNumber++;
                    // processing fields in row
                    string[] lFields = lParser.ReadFields();

                    // get header fields
                    // line number increments after first read
                    if (lRecordNumber == 1)
                    {
                        lFieldHeadings = lFields;
                        FileLog.Info($"Found headers: {string.Join(", ", lFieldHeadings)}");

                        continue;
                    }

                    // ensure the CSV row has the same number of fields as the header
                    if (lFields.Count() != lFieldHeadings.Count()) continue;

                    // get each field value
                    int lIndex = 0;
                    var lExportItem = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

                    foreach (string field in lFields)
                    {
                        lExportItem.Add(lFieldHeadings[lIndex], field);
                        lIndex++;
                    }

                    // Set provider to web or csv
                    lExportItem.Add(IMDbFieldMapping.cProvider, "csv");

                    // add to list of items
                    aParsedCSV.Add(lExportItem);
                }
                lParser.Close();
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

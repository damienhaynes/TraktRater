namespace TraktRater.Sites
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using global::TraktRater.Logger;
    using global::TraktRater.Settings;
    using global::TraktRater.Sites.API.IMDb;
    using global::TraktRater.Sites.Common;
    using global::TraktRater.TraktAPI;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.UI;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    public class IMDb : IRateSite
    {
        #region Variables
  
        internal static bool mImportCancelled = false;

        readonly CsvConfiguration mCsvConfiguration = new CsvConfiguration();

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
            
            mImportCsvRatings = !string.IsNullOrEmpty(aRatingsFile);
            mImportCsvWatchlist = !string.IsNullOrEmpty(aWatchlistFile);
            mImportCsvCustomLists = aImdbCustomLists.Count > 0;

            mRatingsFileCsv = aRatingsFile;
            mWatchlistFileCsv = aWatchlistFile;
            mCustomListsCsvs = aImdbCustomLists;

            SetCSVHelperOptions();
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "IMDb"; } }

        public void ImportRatings()
        {
            mImportCancelled = false;

            var lRatedCsvItems = new List<IMDbRateItem>();
            var lWatchlistCsvItems = new List<IMDbListItem>();
            var lCustomLists = new Dictionary<string, List<IMDbListItem>>();
            
            #region Parse Ratings CSV
            UIUtils.UpdateStatus("Reading IMDb ratings export...");
            if (mImportCsvRatings)
            {
                mCsvConfiguration.RegisterClassMap<IMDbRatingCsvMap>();
                
                lRatedCsvItems = ParseCsvFile<IMDbRateItem>(mRatingsFileCsv);
                if (lRatedCsvItems == null)
                {
                    UIUtils.UpdateStatus("Failed to parse IMDb ratings file!", true);
                    Thread.Sleep(2000);
                    return;
                }
                mCsvConfiguration.UnregisterClassMap<IMDbRatingCsvMap>();
            }
            if (mImportCancelled) return;
            #endregion

            #region Parse Watchlist CSV
            UIUtils.UpdateStatus("Reading IMDb watchlist export...");
            if (mImportCsvWatchlist)
            {
                mCsvConfiguration.RegisterClassMap<IMDbListCsvMap>();

                lWatchlistCsvItems = ParseCsvFile<IMDbListItem>(mWatchlistFileCsv);
                if (lWatchlistCsvItems == null)
                {
                    UIUtils.UpdateStatus("Failed to parse IMDb watchlist file!", true);
                    Thread.Sleep(2000);
                    return;
                }
                mCsvConfiguration.UnregisterClassMap<IMDbListCsvMap>();
            }
            if (mImportCancelled) return;
            #endregion

            #region Parse Custom List CSVs
            UIUtils.UpdateStatus("Reading IMDb custom lists export...");
            if (mImportCsvCustomLists)
            {
                mCsvConfiguration.RegisterClassMap<IMDbListCsvMap>();

                foreach (var list in mCustomListsCsvs)
                {
                    UIUtils.UpdateStatus($"Reading IMDb custom list '{list}'");

                    var lListCsvItems = ParseCsvFile<IMDbListItem>(list);
                    if (lListCsvItems == null)
                    {
                        UIUtils.UpdateStatus("Failed to parse IMDb custom list file!", true);
                        Thread.Sleep(2000);
                        continue;
                    }
                    lCustomLists.Add(list, lListCsvItems);
                }
                mCsvConfiguration.UnregisterClassMap<IMDbListCsvMap>();
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Rated Movies
            var lRatedCsvMovies = lRatedCsvItems.Where(r => r.Type.ItemType() == IMDbType.Movie && r.MyRating != null)
                                                .Select(s => s.ToTraktRatedMovie()).ToList();

            FileLog.Info($"Found {lRatedCsvMovies.Count} movie ratings in CSV file");
            if (lRatedCsvMovies.Any())
            {
                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var lCurrentUserMovieRatings = TraktAPI.GetRatedMovies();

                if (lCurrentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus($"Found {lCurrentUserMovieRatings.Count()} user movie ratings on trakt.tv");
                    // Filter out movies to rate from existing ratings online
                    lRatedCsvMovies.RemoveAll(m => lCurrentUserMovieRatings.Any(c => c.Movie.Ids.ImdbId == m.Ids.ImdbId));
                }

                UIUtils.UpdateStatus($"Importing {lRatedCsvMovies.Count()} new movie ratings to trakt.tv");

                if (lRatedCsvMovies.Count > 0)
                {
                    int lPageSize = AppSettings.BatchSize;
                    int lPages = (int)Math.Ceiling((double)lRatedCsvMovies.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {   
                        UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb rated movies...");

                        var lRatingsToSync = new TraktMovieRatingSync()
                        {
                            movies = lRatedCsvMovies.Skip(i * lPageSize).Take(lPageSize).ToList()
                        };
                        
                        TraktSyncResponse lResponse = TraktAPI.AddMoviesToRatings(lRatingsToSync);
                        if (lResponse == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb movie ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (lResponse.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus($"Unable to sync ratings for {lResponse.NotFound.Movies.Count} movies as they're not found on trakt.tv!");
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Rated TV Shows
            var lRatedCsvShows = lRatedCsvItems.Where(r => r.Type.ItemType() == IMDbType.Show && r.MyRating != null)
                                               .Select(s => s.ToTraktRatedShow()).ToList();

            FileLog.Info($"Found {lRatedCsvShows.Count} tv show ratings in CSV file");
            if (lRatedCsvShows.Any())
            {
                UIUtils.UpdateStatus("Retrieving existing tv show ratings from trakt.tv");
                var lCurrentUserShowRatings = TraktAPI.GetRatedShows();

                if (lCurrentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus($"Found {lCurrentUserShowRatings.Count()} user tv show ratings on trakt.tv");
                    // Filter out shows to rate from existing ratings online
                    lRatedCsvShows.RemoveAll(s => lCurrentUserShowRatings.Any(c => (c.Show.Ids.ImdbId == s.Ids.ImdbId) || 
                                                                                   (c.Show.Title.ToLowerInvariant() == s.Title.ToLowerInvariant() && c.Show.Year == s.Year)));
                }

                UIUtils.UpdateStatus($"Importing {lRatedCsvShows.Count()} tv show ratings to trakt.tv");

                if (lRatedCsvShows.Count > 0)
                {
                    int lPageSize = AppSettings.BatchSize;
                    int lPages = (int)Math.Ceiling((double)lRatedCsvShows.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {
                        UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb rated shows...");

                        var lRatingsToSync = new TraktShowRatingSync()
                        {
                            shows = lRatedCsvShows.Skip(i * lPageSize).Take(lPageSize).ToList()
                        };

                        TraktSyncResponse lResponse = TraktAPI.AddShowsToRatings(lRatingsToSync);
                        if (lResponse == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb tv show ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (lResponse.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus($"Unable to sync ratings for {lResponse.NotFound.Shows.Count} shows as they're not found on trakt.tv!");
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Rated Episodes
            var lImdbRatedEpisodes = new List<IMDbEpisode>();
            var lImdbRatedCsvEpisodes = lRatedCsvItems.Where(r => r.Type.ItemType() == IMDbType.Episode).ToList();

            FileLog.Info($"Found {lImdbRatedCsvEpisodes.Count} tv episode ratings in CSV file");
            if (lImdbRatedCsvEpisodes.Any())
            {
                // we can't rely on the episode imdb id as trakt most likely wont have that info at the episode level
                lImdbRatedEpisodes.AddRange(lImdbRatedCsvEpisodes.Select(Helper.GetIMDbEpisodeFromTrakt).Where(imdbEpisode => imdbEpisode != null));
                if (mImportCancelled) return;

                UIUtils.UpdateStatus("Retrieving existing tv episode ratings from trakt.tv");
                var lCurrentUserEpisodeRatings = TraktAPI.GetRatedEpisodes();

                if (lCurrentUserEpisodeRatings != null)
                {
                    UIUtils.UpdateStatus($"Found {lCurrentUserEpisodeRatings.Count()} user tv episode ratings on trakt.tv");

                    // Filter out episodes to rate from existing ratings online
                    lImdbRatedEpisodes.RemoveAll(e => lCurrentUserEpisodeRatings.Any(c => c.Episode.Ids.Trakt == e.TraktId));
                }

                UIUtils.UpdateStatus($"Importing {lImdbRatedEpisodes.Count()} episode ratings to trakt.tv");

                if (lImdbRatedEpisodes.Count > 0)
                {
                    int lPageSize = AppSettings.BatchSize;
                    int lPages = (int)Math.Ceiling((double)lImdbRatedEpisodes.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {
                        var lEpisodesRated = Helper.GetTraktEpisodeRateData(lImdbRatedEpisodes.Skip(i * lPageSize).Take(lPageSize));

                        UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb rated episodes...");

                        var lResponse = TraktAPI.AddsEpisodesToRatings(lEpisodesRated);
                        if (lResponse == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb episodes ratings to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (lResponse.NotFound.Episodes.Count > 0)
                        {
                            UIUtils.UpdateStatus($"Unable to sync ratings for {lResponse.NotFound.Episodes.Count} IMDb episodes as they're not found on trakt.tv!");
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
                var lWatchedCsvMovies = lRatedCsvItems.Where(r => r.Type.ItemType() == IMDbType.Movie)
                                                      .Select(s => s.ToTraktWatchedMovie()).ToList();

                FileLog.Info("Found {0} movies in CSV file", lWatchedCsvMovies.Count);

                // compare all movies rated against what's not watched on trakt
                if (lWatchedCsvMovies.Count > 0)
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

                        UIUtils.UpdateStatus($"Found {lWatchedTraktMovies.Count()} watched movies on trakt");
                        UIUtils.UpdateStatus("Filtering out watched movies that are already watched on trakt.tv");

                        lWatchedCsvMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId || 
                                                                                                (t.Movie.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Movie.Year == w.Year)) != null);

                        // mark all rated movies as watched
                        UIUtils.UpdateStatus($"Importing {lWatchedCsvMovies.Count} IMDb movies as watched...");

                        int lPageSize = AppSettings.BatchSize;
                        int lPages = (int)Math.Ceiling((double)lRatedCsvMovies.Count / lPageSize);
                        for (int i = 0; i < lPages; i++)
                        {
                            UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb movies as watched...");

                            var lMoviesToSync = new TraktMovieWatchedSync()
                            {                                
                                Movies = lWatchedCsvMovies.Skip(i * lPageSize).Take(lPageSize).ToList()
                            };

                            var lResponse = TraktAPI.AddMoviesToWatchedHistory(lMoviesToSync);
                            if (lResponse == null)
                            {
                                UIUtils.UpdateStatus("Failed to send watched status for IMDb movies to trakt.tv", true);
                                Thread.Sleep(2000);
                            }
                            else if (lResponse.NotFound.Movies.Count > 0)
                            {
                                UIUtils.UpdateStatus($"Unable to sync watched states for {lResponse.NotFound.Movies.Count} movies as they're not found on trakt.tv!");
                                Thread.Sleep(1000);
                            }
                            if (mImportCancelled) return;
                        }
                    }
                }
                #endregion

                #region Episodes
                if (lImdbRatedEpisodes != null && lImdbRatedEpisodes.Any())
                {
                    int lPageSize = AppSettings.BatchSize;
                    int lPages = (int)Math.Ceiling((double)lImdbRatedEpisodes.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {
                        var lEpisodesWatched = Helper.GetTraktEpisodeWatchedData(lImdbRatedEpisodes.Skip(i * lPageSize).Take(lPageSize));

                        UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb watched episodes...");

                        var lResponse = TraktAPI.AddEpisodesToWatchedHistory(lEpisodesWatched);
                        if (lResponse == null)
                        {
                            UIUtils.UpdateStatus("Error importing IMDb episodes as watched to trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                        else if (lResponse.NotFound.Episodes.Count > 0)
                        {
                            UIUtils.UpdateStatus($"Unable to sync {lResponse.NotFound.Episodes.Count} IMDb episodes as watched, as they're not found on trakt.tv!");
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                }
                #endregion
            }
            #endregion

            #region Import Watchlist Movies
            var lWatchlistedCsvMovies = lWatchlistCsvItems.Where(r => r.Type.ItemType() == IMDbType.Movie)
                                                          .Select(s => s.ToTraktMovie()).ToList();

            FileLog.Info($"Found {lWatchlistedCsvMovies.Count} movies watchlisted in CSV file");
            if (lWatchlistedCsvMovies.Any())
            {
                UIUtils.UpdateStatus("Requesting existing watchlisted movies from trakt...");
                var lWatchlistTraktMovies = TraktAPI.GetWatchlistMovies();
                if (lWatchlistTraktMovies != null)
                {
                    UIUtils.UpdateStatus($"Found {lWatchlistTraktMovies.Count()} watchlist movies on trakt");
                    UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                    lWatchlistedCsvMovies.RemoveAll(w => lWatchlistTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId || 
                                                                                                  (t.Movie.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Movie.Year == w.Year)) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && lWatchlistedCsvMovies.Count > 0)
                {
                    // get watched movies from trakt so we don't import movies into watchlist that are already watched
                    // we may already have this if we imported rated items as watched
                    if (lWatchedTraktMovies != null)
                    {
                        UIUtils.UpdateStatus("Requesting watched movies from trakt...");
                        lWatchedTraktMovies = TraktAPI.GetWatchedMovies();
                        if (lWatchedTraktMovies == null)
                        {
                            UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                    }

                    if (lWatchedTraktMovies != null)
                    {
                        UIUtils.UpdateStatus($"Found {lWatchedTraktMovies.Count()} watched movies on trakt");
                        UIUtils.UpdateStatus("Filtering out watchlist movies that are watched on trakt.tv");

                        // remove movies from sync list which are watched already
                        lWatchlistedCsvMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId || 
                                                                                                    (t.Movie.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Movie.Year == w.Year)) != null);
                    }
                }

                // add movies to watchlist
                UIUtils.UpdateStatus($"Importing {lWatchlistedCsvMovies.Count()} IMDb watchlist movies to trakt.tv...");

                int lPageSize = AppSettings.BatchSize;
                int lPages = (int)Math.Ceiling((double)lWatchlistedCsvMovies.Count / lPageSize);
                for (int i = 0; i < lPages; i++)
                {
                    UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb watchlist movies...");

                    var lWatchlistedToSync = new TraktMovieSync()
                    {
                        Movies = lWatchlistedCsvMovies.Skip(i * lPageSize).Take(lPageSize).ToList()
                    };

                    var lResponse = TraktAPI.AddMoviesToWatchlist(lWatchlistedToSync);
                    if (lResponse == null)
                    {
                        UIUtils.UpdateStatus("Failed to send watchlist for IMDb movies", true);
                        Thread.Sleep(2000);
                    }
                    else if (lResponse.NotFound.Movies.Count > 0)
                    {
                        UIUtils.UpdateStatus($"Unable to sync watchlist for {lResponse.NotFound.Movies.Count} movies as they're not found on trakt.tv!");
                        Thread.Sleep(1000);
                    }

                    if (mImportCancelled) return;
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Watchlist TV Shows
            IEnumerable<TraktShowPlays> lWatchedTraktShows = null;
            var lWatchlistedCsvShows = lWatchlistCsvItems.Where(r => r.Type.ItemType() == IMDbType.Show)
                                                         .Select(s => s.ToTraktShow()).ToList();

            FileLog.Info($"Found {lWatchlistedCsvShows.Count} tv shows watchlisted in CSV file");
            if (lWatchlistedCsvShows.Any())
            {
                UIUtils.UpdateStatus("Requesting existing watchlisted shows from trakt...");
                var lWatchlistTraktShows = TraktAPI.GetWatchlistShows();
                if (lWatchlistTraktShows != null)
                {
                    UIUtils.UpdateStatus($"Found {lWatchlistTraktShows.Count()} watchlist shows on trakt");
                    UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watchlist on trakt.tv");
                    lWatchlistedCsvShows.RemoveAll(w => lWatchlistTraktShows.FirstOrDefault(t => t.Show.Ids.ImdbId == w.Ids.ImdbId || 
                                                                                                (t.Show.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Show.Year == w.Year)) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && lWatchlistedCsvShows.Count > 0)
                {
                    UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                    // get watched movies from trakt so we don't import shows into watchlist that are already watched
                    lWatchedTraktShows = TraktAPI.GetWatchedShows();
                    if (lWatchedTraktShows != null)
                    {
                        UIUtils.UpdateStatus($"Found {lWatchedTraktShows.Count()} watched shows on trakt");
                        UIUtils.UpdateStatus("Filtering out watchlist shows containing watched episodes on trakt.tv.");

                        // remove shows from sync list which are watched already
                        lWatchlistedCsvShows.RemoveAll(w => lWatchedTraktShows.FirstOrDefault(t => (t.Show.Ids.ImdbId == w.Ids.ImdbId) || 
                                                                                                   (t.Show.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Show.Year == w.Year)) != null);
                    }
                }

                // add shows to watchlist
                UIUtils.UpdateStatus($"Importing {lWatchlistedCsvShows.Count()} IMDb watchlist shows to trakt.tv...");

                int lPageSize = AppSettings.BatchSize;
                int lPages = (int)Math.Ceiling((double)lWatchlistedCsvShows.Count / lPageSize);
                for (int i = 0; i < lPages; i++)
                {
                    UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb watchlist shows...");

                    var lWatchlistShowsToSync = new TraktShowSync
                    {
                        Shows = lWatchlistedCsvShows.Skip(i * lPageSize).Take(lPageSize).ToList()
                    };

                    var lResponse = TraktAPI.AddShowsToWatchlist(lWatchlistShowsToSync);
                    if (lResponse == null)
                    {
                        UIUtils.UpdateStatus("Failed to send watchlist for IMDb tv shows", true);
                        Thread.Sleep(2000);
                    }
                    else if (lResponse.NotFound.Shows.Count > 0)
                    {
                        UIUtils.UpdateStatus($"Unable to sync watchlist for {lResponse.NotFound.Shows.Count} shows as they're not found on trakt.tv!");
                        Thread.Sleep(1000);
                    }

                    if (mImportCancelled) return;
                }
            }
            if (mImportCancelled) return;
            #endregion

            #region Import Watchlist Episodes
            var lImdbWatchlistedEpisodes = new List<IMDbEpisode>();
            var lImdbWatchlistedCsvEpisodes = lWatchlistCsvItems.Where(r => r.Type.ItemType() == IMDbType.Episode).ToList();

            FileLog.Info($"Found {lImdbWatchlistedCsvEpisodes.Count} tv episodes watchlisted in CSV file");

            if (lImdbWatchlistedCsvEpisodes.Any())
            {
                UIUtils.UpdateStatus($"Found {lImdbWatchlistedCsvEpisodes.Count()} IMDb watchlisted episodes");

                lImdbWatchlistedEpisodes.AddRange(lImdbWatchlistedCsvEpisodes.Select(Helper.GetIMDbEpisodeFromTrakt).Where(imdbEpisode => imdbEpisode != null));
                if (mImportCancelled) return;

                // filter out existing watchlist episodes
                UIUtils.UpdateStatus("Requesting existing watchlist episodes from trakt...");
                var lWatchlistedTraktEpisodes = TraktAPI.GetWatchlistEpisodes();
                if (lWatchlistedTraktEpisodes != null)
                {
                    UIUtils.UpdateStatus($"Found {lWatchlistedTraktEpisodes.Count()} watchlist episodes on trakt");
                    UIUtils.UpdateStatus("Filtering out watchlist episodes that are already in watchlist on trakt.tv");
                    lImdbWatchlistedEpisodes.RemoveAll(e => lWatchlistedTraktEpisodes.FirstOrDefault(w => w.Episode.Ids.Trakt == e.TraktId) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && lImdbWatchlistedEpisodes.Count > 0)
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
                        lImdbWatchlistedEpisodes.RemoveAll(e => lWatchedTraktShows.Where(s => s.Show.Ids.ImdbId == e.ShowImdbId)
                                                                                              .Any(s => s.Seasons.Exists(se => se.Number == e.SeasonNumber && 
                                                                                                                               se.Episodes.Exists(ep => ep.Number == e.EpisodeNumber))));
                    }
                }

                UIUtils.UpdateStatus($"Importing {lImdbWatchlistedEpisodes.Count()} episodes in watchlist to trakt.tv");

                if (lImdbWatchlistedEpisodes.Count > 0)
                {
                    int lPageSize = AppSettings.BatchSize;
                    int lPages = (int)Math.Ceiling((double)lImdbWatchlistedEpisodes.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {
                        UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb watchlisted episodes...");

                        var lResponse = TraktAPI.AddEpisodesToWatchlist(Helper.GetTraktEpisodeData(lImdbWatchlistedEpisodes.Skip(i * lPageSize).Take(lPageSize)));
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

                UIUtils.UpdateStatus($"Found {lTraktCustomLists.Count()} custom lists on trakt.tv");

                foreach (var list in lCustomLists)
                {
                    bool lListCreated = false;
                    string lListName = Path.GetFileNameWithoutExtension(list.Key);

                    // create the list if we don't have it
                    TraktListDetail lTraktCustomList = lTraktCustomLists.FirstOrDefault(l => l.Name == lListName);

                    if (lTraktCustomList == null)
                    {
                        UIUtils.UpdateStatus($"Creating new custom list '{lListName}' on trakt.tv");
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
                    var lIMDbCsvListItems = list.Value;
                    
                    var lImdbCsvListMovies = lIMDbCsvListItems.Where(l => l.Type.ItemType() == IMDbType.Movie).Select(m => m.ToTraktMovie()).ToList();
                    var lImdbCsvListShows = lIMDbCsvListItems.Where(l => l.Type.ItemType() == IMDbType.Show).Select(m => m.ToTraktShow()).ToList();

                    FileLog.Info($"Found {lIMDbCsvListItems.Count} movies and {lImdbCsvListShows.Count} shows in IMDb {lListName} list", lListName);

                    // if the list already exists, get current items for list 
                    if (!lListCreated)
                    {
                        lTraktCustomList = lTraktCustomLists.FirstOrDefault(l => l.Name == lListName);
                        
                        UIUtils.UpdateStatus($"Requesting existing custom list '{lListName}' items from trakt...");
                        var lTraktListItems = TraktAPI.GetCustomListItems(lTraktCustomList.Ids.Trakt.ToString());
                        if (lTraktListItems == null)
                        {
                            UIUtils.UpdateStatus("Error requesting custom list items from trakt.tv, skipping list creation", true);
                            Thread.Sleep(2000);
                            continue;
                        }

                        // filter out existing items from CSV so we don't send again
                        FileLog.Info($"Filtering out existing items from IMDb list '{lListName}' so we don't send again to trakt.tv");
                        lImdbCsvListMovies.RemoveAll(i => lTraktListItems.FirstOrDefault(l => l.Movie.Ids.ImdbId == i.Ids.ImdbId) != null);
                        lImdbCsvListShows.RemoveAll(i => lTraktListItems.FirstOrDefault(l => l.Show.Ids.ImdbId == i.Ids.ImdbId) != null);
                    }

                    #region Movies

                    UIUtils.UpdateStatus($"Importing {lImdbCsvListMovies.Count} new movies into {lListName} custom list...");

                    int lPageSize = AppSettings.BatchSize;
                    int lPages = (int)Math.Ceiling((double)lImdbCsvListMovies.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {
                        UIUtils.UpdateStatus($"Importing page {i + 1}/{lPages} IMDb custom list movies...");

                        // create list sync object to hold list items
                        var lTraktMovieSync = new TraktSyncAll
                        {
                            Movies = lImdbCsvListMovies.Skip(i * lPageSize).Take(lPageSize).ToList()
                        };

                        var lResponse = TraktAPI.AddItemsToList(lTraktCustomList.Ids.Trakt.ToString(), lTraktMovieSync);
                        if (lResponse == null)
                        {
                            UIUtils.UpdateStatus("Failed to send custom list items for IMDb movies", true);
                            Thread.Sleep(2000);
                        }
                        else if (lResponse.NotFound.Movies.Count > 0)
                        {
                            UIUtils.UpdateStatus($"Unable to sync custom list items for {lResponse.NotFound.Movies.Count} movies as they're not found on trakt.tv!");
                            Thread.Sleep(1000);
                        }

                        if (mImportCancelled) return;
                    }
                    #endregion

                    #region Shows

                    UIUtils.UpdateStatus("Importing {0} new shows into {1} custom list...", lImdbCsvListShows.Count(), lListName);

                    lPageSize = AppSettings.BatchSize;
                    lPages = (int)Math.Ceiling((double)lImdbCsvListShows.Count / lPageSize);
                    for (int i = 0; i < lPages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb custom list shows...", i + 1, lPages);

                        // create list sync object to hold list items
                        var lTraktShowSync = new TraktSyncAll
                        {
                            Shows = lImdbCsvListShows.Skip(i * lPageSize).Take(lPageSize).ToList()
                        };

                        var lResponse = TraktAPI.AddItemsToList(lTraktCustomList.Ids.Trakt.ToString(), lTraktShowSync);
                        if (lResponse == null)
                        {
                            UIUtils.UpdateStatus("Failed to send custom list items for IMDb shows", true);
                            Thread.Sleep(2000);
                        }
                        else if (lResponse.NotFound.Shows.Count > 0)
                        {
                            UIUtils.UpdateStatus("Unable to sync custom list items for {0} shows as they're not found on trakt.tv!", lResponse.NotFound.Shows.Count);
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
        
        private void SetCSVHelperOptions()
        {
            mCsvConfiguration.IsHeaderCaseSensitive = false;

            // IMDb use "." for decimal seperator so set culture to cater for this            
            mCsvConfiguration.CultureInfo = new System.Globalization.CultureInfo("en-US");

            // if we're unable parse a row, log the details for analysis
            mCsvConfiguration.IgnoreReadingExceptions = true;
            mCsvConfiguration.ReadingExceptionCallback = (ex, row) =>
            {
                FileLog.Error($"Error reading row '{ex.Data["CsvHelper"]}'");
            };
        }

        private List<T> ParseCsvFile<T>(string aFilename)
        {
            using (var reader = new StreamReader(aFilename))
            using (var csv = new CsvReader(reader, mCsvConfiguration))
            {
                return csv.GetRecords<T>().ToList();
            }
        }
        
        #endregion
    }

}

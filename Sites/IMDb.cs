using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualBasic.FileIO;
using TraktRater.Logger;
using TraktRater.Settings;
using TraktRater.Sites.API.IMDb;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.UI;

namespace TraktRater.Sites
{
    using global::TraktRater.Sites.Common;

    public class IMDb : IRateSite
    {
        #region Variables
  
        bool importCancelled = false;

        readonly string csvRatingsFile = null;
        readonly string csvWatchlistFile = null;

        List<Dictionary<string, string>> rateItems = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> watchlistItems = new List<Dictionary<string, string>>();

        readonly bool importCsvRatings = false;
        readonly bool importCsvWatchlist = false;

        #endregion

        #region Constructor

        public IMDb(string ratingsFile, string watchlistFile, bool enabled)
        {
            Enabled = enabled;

            if (!enabled) return;

            // just do a simple null check, we check file existence on parse
            importCsvRatings = !string.IsNullOrEmpty(ratingsFile);
            importCsvWatchlist = !string.IsNullOrEmpty(watchlistFile);

            csvRatingsFile = ratingsFile;
            csvWatchlistFile = watchlistFile;
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "IMDb"; } }

        public void ImportRatings()
        {
            importCancelled = false;

            #region Parse Ratings CSV
            UIUtils.UpdateStatus("Reading IMDb ratings export...");
            if (importCsvRatings && !ParseCSVFile(csvRatingsFile, out rateItems))
            {
                UIUtils.UpdateStatus("Failed to parse IMDb ratings file!", true);
                Thread.Sleep(2000);
                return;
            }
            if (importCancelled) return;
            #endregion

            #region Parse Watchlist CSV
            UIUtils.UpdateStatus("Reading IMDb watchlist export...");
            if (importCsvWatchlist && !ParseCSVFile(csvWatchlistFile, out watchlistItems))
            {
                UIUtils.UpdateStatus("Failed to parse IMDb watchlist file!", true);
                Thread.Sleep(2000);
                return;
            }
            if (importCancelled) return;
            #endregion

            #region Import Rated Movies
            var movies = rateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie && !string.IsNullOrEmpty(r[IMDbFieldMapping.cRating])).ToList();
            if (movies.Any())
            {
                FileLog.Info("Found {0} movie ratings in CSV file", movies.Count);
                UIUtils.UpdateStatus("Retrieving existing movie ratings from trakt.tv");
                var currentUserMovieRatings = TraktAPI.TraktAPI.GetRatedMovies();

                if (currentUserMovieRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user movie ratings on trakt.tv", currentUserMovieRatings.Count());
                    // Filter out movies to rate from existing ratings online
                    movies.RemoveAll(m => currentUserMovieRatings.Any(c => c.Movie.Ids.ImdbId == m[IMDbFieldMapping.cIMDbID]));
                }

                UIUtils.UpdateStatus("Importing {0} new movie ratings to trakt.tv", movies.Count());

                if (movies.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)movies.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb rated movies...", i + 1, pages);

                        TraktSyncResponse response = TraktAPI.TraktAPI.AddMoviesToRatings(Helper.GetRateMoviesData(movies.Skip(i * pageSize).Take(pageSize)));
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

                        if (importCancelled) return;
                    }
                }
            }
            if (importCancelled) return;
            #endregion

            #region Import Rated TV Shows
            var shows = rateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show && !string.IsNullOrEmpty(r[IMDbFieldMapping.cRating])).ToList();
            if (shows.Any())
            {
                FileLog.Info("Found {0} tv show ratings in CSV file", shows.Count);
                UIUtils.UpdateStatus("Retrieving existing tv show ratings from trakt.tv");
                var currentUserShowRatings = TraktAPI.TraktAPI.GetRatedShows();

                if (currentUserShowRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user tv show ratings on trakt.tv", currentUserShowRatings.Count());
                    // Filter out shows to rate from existing ratings online
                    shows.RemoveAll(s => currentUserShowRatings.Any(c => (c.Show.Ids.ImdbId == s[IMDbFieldMapping.cIMDbID]) || (c.Show.Title == s[IMDbFieldMapping.cTitle] && c.Show.Year.ToString() == s[IMDbFieldMapping.cYear])));
                }

                UIUtils.UpdateStatus("Importing {0} tv show ratings to trakt.tv", shows.Count());

                if (shows.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)shows.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb rated shows...", i + 1, pages);

                        TraktSyncResponse response = TraktAPI.TraktAPI.AddShowsToRatings(Helper.GetRateShowsData(shows.Skip(i * pageSize).Take(pageSize)));
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

                        if (importCancelled) return;
                    }
                }
            }
            if (importCancelled) return;
            #endregion

            #region Import Rated Episodes
            var imdbEpisodes = new List<IMDbEpisode>();
            var imdbCSVEpisodes = rateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
            if (imdbCSVEpisodes.Any())
            {
                FileLog.Info("Found {0} tv episode ratings in CSV file", imdbCSVEpisodes.Count);

                // we can't rely on the imdb id as trakt most likely wont have the info for episodes

                // search and cache all series info needed for syncing
                // use the tvdb API to first search for each unique series name
                // then GetSeries by TVDb ID to get a list of all episodes
                // each episode will have TVDb ID which we can use for syncing.

                imdbEpisodes.AddRange(imdbCSVEpisodes.Select(Helper.GetIMDbEpisodeFromTVDb).Where(imdbEpisode => imdbEpisode != null));

                UIUtils.UpdateStatus("Retrieving existing tv episode ratings from trakt.tv");
                var currentUserEpisodeRatings = TraktAPI.TraktAPI.GetRatedEpisodes();

                if (currentUserEpisodeRatings != null)
                {
                    UIUtils.UpdateStatus("Found {0} user tv episode ratings on trakt.tv", currentUserEpisodeRatings.Count());

                    // Filter out episodes to rate from existing ratings online
                    imdbEpisodes.RemoveAll(e => currentUserEpisodeRatings.Any(c => c.Episode.Ids.TvdbId == e.TvdbId || c.Episode.Ids.ImdbId == e.ImdbId));
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

                        var response = TraktAPI.TraktAPI.AddsEpisodesToRatings(episodesRated);
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

            #region Mark Rated Items as Watched
            IEnumerable<TraktMoviePlays> watchedTraktMovies = null;

            if (AppSettings.MarkAsWatched)
            {
                #region Movies
                // compare all movies rated against what's not watched on trakt
                movies = rateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie && !string.IsNullOrEmpty(r[IMDbFieldMapping.cRating])).ToList();
                if (movies.Count > 0)
                {
                    // get watched movies from trakt.tv
                    UIUtils.UpdateStatus("Requesting watched movies from trakt...");
                    watchedTraktMovies = TraktAPI.TraktAPI.GetWatchedMovies();
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

                            var response = TraktAPI.TraktAPI.AddMoviesToWatchedHistory(Helper.GetSyncWatchedMoviesData(movies.Skip(i * pageSize).Take(pageSize).ToList()));
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

                        var response = TraktAPI.TraktAPI.AddEpisodesToWatchedHistory(episodesWatched);
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

            #region Import Watchlist Movies
            movies = watchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie).ToList();
            if (movies.Any())
            {
                FileLog.Info("Found {0} movies watchlisted in CSV file", movies.Count);
                UIUtils.UpdateStatus("Requesting existing watchlist movies from trakt...");
                var watchlistTraktMovies = TraktAPI.TraktAPI.GetWatchlistMovies();
                if (watchlistTraktMovies != null)
                {
                    UIUtils.UpdateStatus("Found {0} watchlist movies on trakt", watchlistTraktMovies.Count());
                    UIUtils.UpdateStatus("Filtering out watchlist movies that are already in watchlist on trakt.tv");
                    movies.RemoveAll(w => watchlistTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID] || (t.Movie.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle] && t.Movie.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && movies.Count > 0)
                {
                    UIUtils.UpdateStatus("Requesting watched movies from trakt...");

                    // get watched movies from trakt so we don't import movies into watchlist that are already watched
                    if (watchedTraktMovies != null)
                    {
                        watchedTraktMovies = TraktAPI.TraktAPI.GetWatchedMovies();
                        if (watchedTraktMovies == null)
                        {
                            UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv", true);
                            Thread.Sleep(2000);
                        }
                    }

                    if (watchedTraktMovies != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watched movies on trakt", watchedTraktMovies.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist movies that are watched on trakt.tv");

                        // remove movies from sync list which are watched already
                        movies.RemoveAll(w => watchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID] || (t.Movie.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle] && t.Movie.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);
                    }
                }

                // add movies to watchlist
                UIUtils.UpdateStatus("Importing {0} IMDb watchlist movies to trakt.tv...", movies.Count());

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)movies.Count / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    UIUtils.UpdateStatus("Importing page {0}/{1} IMDb watchlist movies...", i + 1, pages);

                    var response = TraktAPI.TraktAPI.AddMoviesToWatchlist(Helper.GetSyncMoviesData(movies.Skip(i * pageSize).Take(pageSize).ToList()));
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

                    if (importCancelled) return;
                }
            }
            if (importCancelled) return;
            #endregion

            #region Import Watchlist TV Shows
            IEnumerable<TraktShowPlays> watchedTraktShows = null;
            shows = watchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show).ToList();
            if (shows.Any())
            {
                FileLog.Info("Found {0} tv shows watchlisted in CSV file", shows.Count);
                UIUtils.UpdateStatus("Requesting existing watchlist shows from trakt...");
                var watchlistTraktShows = TraktAPI.TraktAPI.GetWatchlistShows();
                if (watchlistTraktShows != null)
                {
                    UIUtils.UpdateStatus("Found {0} watchlist shows on trakt", watchlistTraktShows.Count());
                    UIUtils.UpdateStatus("Filtering out watchlist shows that are already in watchlist on trakt.tv");
                    shows.RemoveAll(w => watchlistTraktShows.FirstOrDefault(t => t.Show.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID] || (t.Show.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle].ToLowerInvariant() && t.Show.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && shows.Count > 0)
                {
                    UIUtils.UpdateStatus("Requesting watched shows from trakt...");

                    // get watched movies from trakt so we don't import shows into watchlist that are already watched
                    watchedTraktShows = TraktAPI.TraktAPI.GetWatchedShows();
                    if (watchedTraktShows != null)
                    {
                        UIUtils.UpdateStatus("Found {0} watched shows on trakt", watchedTraktShows.Count());
                        UIUtils.UpdateStatus("Filtering out watchlist shows containing watched episodes on trakt.tv.");

                        // remove shows from sync list which are watched already
                        shows.RemoveAll(w => watchedTraktShows.FirstOrDefault(t => (t.Show.Ids.ImdbId == w[IMDbFieldMapping.cIMDbID]) || (t.Show.Title.ToLowerInvariant() == w[IMDbFieldMapping.cTitle].ToLowerInvariant() && t.Show.Year.ToString() == w[IMDbFieldMapping.cYear])) != null);
                    }
                }

                // add shows to watchlist
                UIUtils.UpdateStatus("Importing {0} IMDb watchlist shows to trakt.tv...", shows.Count());

                int pageSize = AppSettings.BatchSize;
                int pages = (int)Math.Ceiling((double)shows.Count / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    UIUtils.UpdateStatus("Importing page {0}/{1} IMDb watchlist shows...", i + 1, pages);

                    var response = TraktAPI.TraktAPI.AddShowsToWatchlist(Helper.GetSyncShowsData(shows.Skip(i * pageSize).Take(pageSize)));
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

                    if (importCancelled) return;
                }
            }
            if (importCancelled) return;
            #endregion

            #region Import Watchlist Episodes
            imdbEpisodes.Clear();
            imdbCSVEpisodes = watchlistItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Episode).ToList();
            if (imdbCSVEpisodes.Any())
            {
                FileLog.Info("Found {0} tv episodes watchlisted in CSV file", imdbCSVEpisodes.Count);
                UIUtils.UpdateStatus("Found {0} IMDb watchlist episodes", imdbCSVEpisodes.Count());

                imdbEpisodes.AddRange(imdbCSVEpisodes.Select(Helper.GetIMDbEpisodeFromTVDb).Where(imdbEpisode => imdbEpisode != null));

                // filter out existing watchlist episodes
                UIUtils.UpdateStatus("Requesting existing watchlist episodes from trakt...");
                var watchlistTraktEpisodes = TraktAPI.TraktAPI.GetWatchlistEpisodes();
                if (watchlistTraktEpisodes != null)
                {
                    UIUtils.UpdateStatus("Found {0} watchlist episodes on trakt", watchlistTraktEpisodes.Count());
                    UIUtils.UpdateStatus("Filtering out watchlist episodes that are already in watchlist on trakt.tv");
                    imdbEpisodes.RemoveAll(e => watchlistTraktEpisodes.FirstOrDefault(w => w.Episode.Ids.ImdbId == e.ImdbId || w.Episode.Ids.TvdbId == e.TvdbId) != null);
                }

                if (AppSettings.IgnoreWatchedForWatchlist && imdbEpisodes.Count > 0)
                {
                    // we already might have it from the shows sync
                    if (watchedTraktShows == null)
                    {
                        UIUtils.UpdateStatus("Requesting watched episodes from trakt...");

                        // get watched episodes from trakt so we don't import episodes into watchlist that are already watched
                        watchedTraktShows = TraktAPI.TraktAPI.GetWatchedShows();
                    }

                    if (watchedTraktShows != null)
                    {
                        UIUtils.UpdateStatus("Filtering out watchlist episodes containing watched episodes on trakt.tv");

                        imdbEpisodes.RemoveAll(e => watchedTraktShows.Where(s => s.Show.Ids.ImdbId == e.ImdbId)
                                                                         .Any(s => s.Seasons.Exists(se => se.Number == e.SeasonNumber && se.Episodes.Exists(ep => ep.Number == e.EpisodeNumber))));
                    }
                }

                UIUtils.UpdateStatus("Importing {0} episodes in watchlist to trakt.tv", imdbEpisodes.Count());

                if (imdbEpisodes.Count > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)Math.Ceiling((double)imdbEpisodes.Count / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        UIUtils.UpdateStatus("Importing page {0}/{1} IMDb watchlist episodes...", i + 1, pages);

                        var response = TraktAPI.TraktAPI.AddEpisodesToWatchlist(Helper.GetTraktEpisodeData(imdbEpisodes.Skip(i * pageSize).Take(pageSize)));
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

                        if (importCancelled) return;
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
        
        private bool ParseCSVFile(string filename, out List<Dictionary<string, string>> parsedCSV)
        {
            parsedCSV = new List<Dictionary<string, string>>();

            if (!File.Exists(filename)) return false;

            string[] fieldHeadings = new string[]{};
            
            try
            {
                TextFieldParser parser = new TextFieldParser(filename) { TextFieldType = FieldType.Delimited };
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

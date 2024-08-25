using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using CsvHelper.Configuration;
using TraktRater.Settings;
using TraktRater.Sites.API.iCheckMovies;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.UI;

namespace TraktRater.Sites
{
    internal class CheckMovies : IRateSite
    {
        private bool ImportCancelled;
        private readonly string CheckMoviesFilename;
        private readonly int DelimiterOption;
        
        private readonly CsvConfiguration csvConfiguration = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        public CheckMovies(string checkMoviesFilename, int delimiter)
        {
            CheckMoviesFilename = checkMoviesFilename;
            DelimiterOption = delimiter;

            Enabled = File.Exists(checkMoviesFilename);
        }

        public string Name => "iCheckMovies";

        public bool Enabled { get; set; }

        public void Cancel()
        {
            ImportCancelled = true;
        }

        public void ImportRatings()
        {
            // iCheckMovies does not have a compatible ratings system with trakt.tv
            // We can sync collection, watched and watchlist data.

            if (ImportCancelled) return;            

            var cmMovieList = ParseCheckMoviesCsv();
            
            // Add movies to watchlist
            var watchListMovies = AppSettings.CheckMoviesAddWatchedMoviesToWatchlist ? cmMovieList.Where(cm => cm.IsChecked) : cmMovieList.Where(cm => cm.InWatchlist).ToList();
            if (watchListMovies.Any())
            {
                AddMoviesToWatchlist(watchListMovies);
                if (ImportCancelled) return;
            }
            
            // add movies to watched history
            if (AppSettings.CheckMoviesUpdateWatchedHistory)
            {
                var watchedMovies = cmMovieList.Where(cm => cm.IsChecked).Select(cm => cm.ToTraktMovieWatched()).ToList();
                if (watchedMovies.Any())
                {
                    AddMoviesToWatchedHistory(watchedMovies);
                }
            }

            // add movies to collection
            if (AppSettings.CheckMoviesAddToCollection)
            {
                // does 'Owned' return a date or 'yes' when owned, if so then we can use the collected_at field?
                var collectedMovies = cmMovieList.Where(cm => cm.IsOwned).ToList();
                if (collectedMovies.Any())
                {
                    AddMoviesToCollection(collectedMovies);
                }
            }
        }

        private void AddMoviesToWatchedHistory(List<TraktMovieWatched> watchedMovies)
        {
            // filter out movies that are already added to watched history
            // get watched movies from trakt.tv
            UIUtils.UpdateStatus("Requesting watched movies from trakt...");
            var lWatchedTraktMovies = TraktAPI.TraktAPI.GetWatchedMovies();
            if (lWatchedTraktMovies == null)
            {
                UIUtils.UpdateStatus("Failed to get watched movies from trakt.tv, skipping watched movie import", true);
                Thread.Sleep(2000);
            }
            else
            {
                if (ImportCancelled) return;

                UIUtils.UpdateStatus("Found {0} watched movies on trakt", lWatchedTraktMovies.Count());
                UIUtils.UpdateStatus("Filtering out watched movies that are already watched on trakt.tv");

                watchedMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId || (t.Movie.Title.ToLowerInvariant() == w.Title.ToLowerInvariant() && t.Movie.Year == w.Year)) != null);

                UIUtils.UpdateStatus("Importing {0} iCheckMovies movies as watched...", watchedMovies.Count);

                if (watchedMovies.Count() > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)System.Math.Ceiling((double)watchedMovies.Count() / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        if (ImportCancelled) return;

                        UIUtils.UpdateStatus("Importing page {0}/{1} iCheckMovies watched history...", i + 1, pages);

                        var watchedToSync = new TraktMovieWatchedSync()
                        {
                            Movies = watchedMovies.Skip(i * pageSize).Take(pageSize).ToList()
                        };

                        var addToWatchedHistoryResponse = TraktAPI.TraktAPI.AddMoviesToWatchedHistory(watchedToSync);
                        HandleResponse(addToWatchedHistoryResponse);
                    }
                }
            }
        }

        private void AddMoviesToWatchlist(IEnumerable<CheckMoviesListItem> watchListMovies)
        {
            UIUtils.UpdateStatus("Updating Trakt watchlist with {0} movies from iCheckMovies.", watchListMovies.Count());

            if (watchListMovies.Count() > 0)
            {
                int pageSize = AppSettings.BatchSize;
                int pages = (int)System.Math.Ceiling((double)watchListMovies.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    if (ImportCancelled) return;

                    UIUtils.UpdateStatus("Importing page {0}/{1} iCheckMovies watchlist...", i + 1, pages);

                    var watchlistToSync = new TraktMovieSync()
                    {
                        Movies = watchListMovies.Skip(i * pageSize).Take(pageSize).Select(icm => icm.ToTraktMovie()).ToList()
                    };

                    var addToWatchlistResponse = TraktAPI.TraktAPI.AddMoviesToWatchlist(watchlistToSync);
                    HandleResponse(addToWatchlistResponse);
                }
            }
        }

        private void AddMoviesToCollection(IEnumerable<CheckMoviesListItem> collectedMovies)
        {
            UIUtils.UpdateStatus("Updating Trakt collection with {0} movies from iCheckMovies.", collectedMovies.Count());

            if (collectedMovies.Count() > 0)
            {
                int pageSize = AppSettings.BatchSize;
                int pages = (int)System.Math.Ceiling((double)collectedMovies.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
                    if (ImportCancelled) return;

                    UIUtils.UpdateStatus("Importing page {0}/{1} iCheckMovies collection...", i + 1, pages);

                    var collectedToSync = new TraktMovieSync()
                    {
                        Movies = collectedMovies.Skip(i * pageSize).Take(pageSize).Select(icm => icm.ToTraktMovie()).ToList()
                    };

                    var addToCollectionResponse = TraktAPI.TraktAPI.AddMoviesToCollection(collectedToSync);
                    HandleResponse(addToCollectionResponse);
                }
            }
        }

        private void HandleResponse(TraktSyncResponse response)
        {
            if (response == null)
            {
                UIUtils.UpdateStatus("Error importing iCheckMovies list to trakt.tv", true);
                Thread.Sleep(2000);
            }
            else if (response.NotFound.Movies.Count > 0)
            {
                UIUtils.UpdateStatus("Unable to process {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                foreach (var movie in response.NotFound.Movies)
                {
                    UIUtils.UpdateStatus("Unable to process movie: Title = {0}, Year = {1}, IMDb = {2}", movie.Title, movie.Year, movie.Ids.ImdbId);
                }
                Thread.Sleep(1000);
            }
        }

        private List<CheckMoviesListItem> ParseCheckMoviesCsv()
        {
            // Delimiter options are '0: Comma' and '1: Semicolon'
            csvConfiguration.Delimiter = DelimiterOption == 0 ? "," : ";";

            UIUtils.UpdateStatus("Parsing iCheckMovies CSV file");
            var textReader = File.OpenText(CheckMoviesFilename);

            var csv = new CsvReader(textReader, csvConfiguration);
            return csv.GetRecords<CheckMoviesListItem>().ToList();
        }
    }

}
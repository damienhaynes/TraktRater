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
        private string CheckMoviesFilename;
        private int DelimiterOption;
        private bool ImportCancelled;
        private readonly CsvConfiguration csvConfiguration = new CsvConfiguration()
        {
            HasHeaderRecord = true,
            IsHeaderCaseSensitive = false
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
            if (ImportCancelled)
            {
                return;
            }

            var cmMovieList = ParseCheckMoviesCsv();
            
            // Add all movies or only movies that are not already watched based on setting.
            var watchListMovies = AppSettings.CheckMoviesAddWatchedMoviesToWatchlist ? cmMovieList : cmMovieList.Where(cm => !cm.IsChecked).ToList();
            if (watchListMovies.Any())
            {
                AddMoviesToWatchlist(watchListMovies);
            }

            if (ImportCancelled || !AppSettings.CheckMoviesUpdateWatchedHistory)
            {
                return;
            }

            var watchedMovies = cmMovieList.Where(icm => icm.IsChecked).Select(cm => cm.ToTraktMovieWatched()).ToList();
            if (watchedMovies.Any())
            {
                UpdateWatchedHistory(watchedMovies);
            }
        }

        private static void UpdateWatchedHistory(List<TraktMovieWatched> watchedMovies)
        {
            UIUtils.UpdateStatus("Updating Trakt watched history with movies from iCheckMovies.");

            if (watchedMovies.Count() > 0)
            {
                int pageSize = AppSettings.BatchSize;
                int pages = (int)System.Math.Ceiling((double)watchedMovies.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
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

        private void AddMoviesToWatchlist(IEnumerable<CheckMoviesListItem> watchListMovies)
        {
            UIUtils.UpdateStatus("Updating Trakt watchlist with movies from iCheckMovies.");

            if (watchListMovies.Count() > 0)
            {
                int pageSize = AppSettings.BatchSize;
                int pages = (int)System.Math.Ceiling((double)watchListMovies.Count() / pageSize);
                for (int i = 0; i < pages; i++)
                {
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

        private static void HandleResponse(TraktSyncResponse addToWatchlistResponse)
        {
            if (addToWatchlistResponse == null)
            {
                UIUtils.UpdateStatus("Error importing iCheckMovies list to trakt.tv", true);
                Thread.Sleep(2000);
            }
            else if (addToWatchlistResponse.NotFound.Movies.Count > 0)
            {
                UIUtils.UpdateStatus("Unable to process {0} movies as they're not found on trakt.tv!",
                    addToWatchlistResponse.NotFound.Movies.Count);
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
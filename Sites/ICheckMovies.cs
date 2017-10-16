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
    // ReSharper disable once InconsistentNaming
    internal class ICheckMovies : IRateSite
    {
        private string iCheckMoviesFilename;
        private bool importCancelled;
        private readonly CsvConfiguration csvConfiguration = new CsvConfiguration()
        {
            HasHeaderRecord = true,
            IsHeaderCaseSensitive = false,
            Delimiter = ";"
        };

        public ICheckMovies(string iCheckMoviesFilename)
        {
            this.iCheckMoviesFilename = iCheckMoviesFilename;
            Enabled = File.Exists(iCheckMoviesFilename);
        }

        public string Name => "ICheckMovies";

        public bool Enabled { get; set; }

        public void Cancel()
        {
            importCancelled = true;
        }

        public void ImportRatings()
        {
            if (importCancelled)
            {
                return;
            }
            var icmMovieList = ParseIcheckMoviesCsv();
            
            // Add all movies or only movies that are not already watched based on setting.
            var watchListMovies = AppSettings.ICheckMoviesAddWatchedMoviesToWatchlist ? icmMovieList : icmMovieList.Where(icm => !icm.IsChecked).ToList();
            if (watchListMovies.Any())
            {
                AddMoviesToWatchlist(watchListMovies);
            }

            if (importCancelled || !AppSettings.ICheckMoviesUpdateWatchedStatus)
            {
                return;
            }

            var watchedMovies = icmMovieList.Where(icm => icm.IsChecked).Select(icm => icm.ToTraktMovieWatched()).ToList();
            if (watchedMovies.Any())
            {
                UpdateWatchedHistory(watchedMovies);
            }
        }

        private static void UpdateWatchedHistory(List<TraktMovieWatched> watchedMovies)
        {
            var watchedToSync = new TraktMovieWatchedSync()
            {
                Movies = watchedMovies
            };

            var addToWatchedHistoryResponse = TraktAPI.TraktAPI.AddMoviesToWatchedHistory(watchedToSync);
            HandleResponse(addToWatchedHistoryResponse);
        }

        private void AddMoviesToWatchlist(IEnumerable<ICheckMoviesListItem> watchListMovies)
        {
            var watchlistToSync = new TraktMovieSync()
            {
                Movies = watchListMovies.Select(icm => icm.ToTraktMovie()).ToList()
            };

            var addToWatchlistResponse = TraktAPI.TraktAPI.AddMoviesToWatchlist(watchlistToSync);
            HandleResponse(addToWatchlistResponse);
        }

        private static void HandleResponse(TraktSyncResponse addToWatchlistResponse)
        {
            if (addToWatchlistResponse == null)
            {
                UIUtils.UpdateStatus("Error importing ICheckMovies list to trakt.tv", true);
                Thread.Sleep(2000);
            }
            else if (addToWatchlistResponse.NotFound.Movies.Count > 0)
            {
                UIUtils.UpdateStatus("Unable to process {0} movies as they're not found on trakt.tv!",
                    addToWatchlistResponse.NotFound.Movies.Count);
                Thread.Sleep(1000);
            }
        }

        private List<ICheckMoviesListItem> ParseIcheckMoviesCsv()
        {
            var textReader = File.OpenText(iCheckMoviesFilename);

            var csv = new CsvReader(textReader, csvConfiguration);
            return csv.GetRecords<ICheckMoviesListItem>().ToList();
        }
    }

}
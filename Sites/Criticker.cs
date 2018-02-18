using CsvHelper;
using CsvHelper.Configuration;
using global::TraktRater.Settings;
using global::TraktRater.Sites.API.Criticker;
using global::TraktRater.TraktAPI.DataStructures;
using global::TraktRater.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace TraktRater.Sites
{
    public class Criticker : IRateSite
    {
        #region Variables

        private string CritickerFilename;
        private bool ImportCancelled;
        private readonly CsvConfiguration csvConfiguration = new CsvConfiguration();

        #endregion

        #region Constructor

        public Criticker(string exportFile)
        {
            CritickerFilename = exportFile;
            Enabled = File.Exists(exportFile);
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name
        {
            get { return "Criticker"; }
        }

        public void Cancel()
        {
            ImportCancelled = true;
        }

        public void ImportRatings()
        {
            if (ImportCancelled) return;

            var csvList = ParseCritickerCsv();

            // Add movies to ratings
            var ratedMovies = csvList.Where(cm => cm.Rating > 0 && !cm.IsTvShow).Select(cm => cm.ToTraktRatedMovie()).ToList();
            if (ratedMovies.Any())
            {
                AddMoviesToRatings(ratedMovies);
                if (ImportCancelled) return;
            }

            // add movies to watched history
            var watchedMovies = new List<TraktMovieWatched>();

            if (AppSettings.MarkAsWatched)
            {
                watchedMovies = csvList.Where(cm => cm.Rating > 0 && !cm.IsTvShow).Select(cm => cm.ToTraktWatchedMovie()).ToList();
                if (watchedMovies.Any())
                {
                    AddMoviesToWatchedHistory(watchedMovies);
                }
            }

            // Add shows to ratings
            var ratedShows = csvList.Where(cm => cm.Rating > 0 && cm.IsTvShow).Select(cm => cm.ToTraktRatedShow()).ToList();
            if (ratedMovies.Any())
            {
                AddShowsToRatings(ratedShows);
                if (ImportCancelled) return;
            }
        }

        #endregion

        #region Private Methods

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

                watchedMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId) != null);

                UIUtils.UpdateStatus("Importing {0} Criticker as watched...", watchedMovies.Count);

                if (watchedMovies.Count() > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)System.Math.Ceiling((double)watchedMovies.Count() / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        if (ImportCancelled) return;

                        UIUtils.UpdateStatus("Importing page {0}/{1} Criticker watched history...", i + 1, pages);

                        var watchedToSync = new TraktMovieWatchedSync()
                        {
                            Movies = watchedMovies.Skip(i * pageSize).Take(pageSize).ToList()
                        };

                        var addToWatchedHistoryResponse = TraktAPI.TraktAPI.AddMoviesToWatchedHistory(watchedToSync);
                        HandleMovieResponse(addToWatchedHistoryResponse);
                    }
                }
            }
        }

        private void AddMoviesToRatings(List<TraktMovieRating> ratedMovies)
        {
            // filter out movies that are already rated
            // get rated movies from trakt.tv
            UIUtils.UpdateStatus("Requesting rated movies from trakt...");
            var lRatedTraktMovies = TraktAPI.TraktAPI.GetRatedMovies();
            if (lRatedTraktMovies == null)
            {
                UIUtils.UpdateStatus("Failed to get rated movies from trakt.tv, skipping rated movie import", true);
                Thread.Sleep(2000);
            }
            else
            {
                if (ImportCancelled) return;

                UIUtils.UpdateStatus("Found {0} rated movies on trakt", lRatedTraktMovies.Count());
                UIUtils.UpdateStatus("Filtering out rated movies that are already rated on trakt.tv");

                ratedMovies.RemoveAll(w => lRatedTraktMovies.FirstOrDefault(t => t.Movie.Ids.ImdbId == w.Ids.ImdbId) != null);

                UIUtils.UpdateStatus("Importing {0} Criticker movie ratings...", ratedMovies.Count);

                if (ratedMovies.Count() > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)System.Math.Ceiling((double)ratedMovies.Count() / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        if (ImportCancelled) return;

                        UIUtils.UpdateStatus("Importing page {0}/{1} Criticker movie ratings...", i + 1, pages);

                        var ratingsToSync = new TraktMovieRatingSync()
                        {
                            movies = ratedMovies.Skip(i * pageSize).Take(pageSize).ToList()
                        };

                        var addToRatingsResponse = TraktAPI.TraktAPI.AddMoviesToRatings(ratingsToSync);
                        HandleMovieResponse(addToRatingsResponse);
                    }
                }
            }
        }

        private void AddShowsToRatings(List<TraktShowRating> ratedShows)
        {
            // filter out movies that are already rated
            // get rated movies from trakt.tv
            UIUtils.UpdateStatus("Requesting rated shows from trakt...");
            var lRatedTraktShows = TraktAPI.TraktAPI.GetRatedShows();
            if (lRatedTraktShows == null)
            {
                UIUtils.UpdateStatus("Failed to get rated shows from trakt.tv, skipping rated show import", true);
                Thread.Sleep(2000);
            }
            else
            {
                if (ImportCancelled) return;

                UIUtils.UpdateStatus("Found {0} rated shows on trakt", lRatedTraktShows.Count());
                UIUtils.UpdateStatus("Filtering out rated shows that are already rated on trakt.tv");

                ratedShows.RemoveAll(w => lRatedTraktShows.FirstOrDefault(t => t.Show.Ids.TmdbId == w.Ids.TmdbId) != null);

                UIUtils.UpdateStatus("Importing {0} Criticker tv show ratings...", ratedShows.Count);

                if (ratedShows.Count() > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)System.Math.Ceiling((double)ratedShows.Count() / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        if (ImportCancelled) return;

                        UIUtils.UpdateStatus("Importing page {0}/{1} Criticker tv show ratings...", i + 1, pages);

                        var ratingsToSync = new TraktShowRatingSync()
                        {
                            shows = ratedShows.Skip(i * pageSize).Take(pageSize).ToList()
                        };

                        var addToRatingsResponse = TraktAPI.TraktAPI.AddShowsToRatings(ratingsToSync);
                        HandleShowResponse(addToRatingsResponse);
                    }
                }
            }
        }

        private void HandleMovieResponse(TraktSyncResponse response)
        {
            if (response == null)
            {
                UIUtils.UpdateStatus("Error importing Criticker movie list to trakt.tv", true);
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

        private void HandleShowResponse(TraktSyncResponse response)
        {
            if (response == null)
            {
                UIUtils.UpdateStatus("Error importing Criticker tv show list to trakt.tv", true);
                Thread.Sleep(2000);
            }
            else if (response.NotFound.Shows.Count > 0)
            {
                UIUtils.UpdateStatus("Unable to process {0} tv shows as they're not found on trakt.tv!", response.NotFound.Shows.Count);
                foreach (var show in response.NotFound.Shows)
                {
                    UIUtils.UpdateStatus("Unable to process tv show: Title = {0}, Year = {1}, IMDb = {2}", show.Title, show.Year, show.Ids.ImdbId);
                }
                Thread.Sleep(1000);
            }
        }

        private List<CritickerItem> ParseCritickerCsv()
        {
            csvConfiguration.RegisterClassMap<CSVFileDefinitionMap>();

            UIUtils.UpdateStatus("Parsing Criticker CSV file");
            var textReader = File.OpenText(CritickerFilename);

            var csv = new CsvReader(textReader, csvConfiguration);
            return csv.GetRecords<CritickerItem>().ToList();
        }

        #endregion
    }
}

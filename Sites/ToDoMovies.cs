using CsvHelper;
using CsvHelper.Configuration;
using global::TraktRater.Settings;
using global::TraktRater.Sites.API.ToDoMovies;
using global::TraktRater.TraktAPI.DataStructures;
using global::TraktRater.UI;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace TraktRater.Sites
{
    public class ToDoMovies : IRateSite
    {
        #region Variables

        private readonly string ToDoMoviesFilename;
        private bool ImportCancelled;
        private readonly CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

        #endregion

        #region Constructor

        public ToDoMovies(string exportMovieFile)
        {
            ToDoMoviesFilename = exportMovieFile;
            Enabled = File.Exists(exportMovieFile);
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name
        {
            get { return "ToDoMovies"; }
        }

        public void Cancel()
        {
            ImportCancelled = true;
        }

        public void ImportRatings()
        {
            if (ImportCancelled) return;

            var tdMovieList = ParseToDoMoviesCsv();

            // Add movies to ratings
            var ratedMovies = tdMovieList.Where(tdm => tdm.Rating > 0).Select(tdm => tdm.ToTraktRatedMovie()).ToList();
            if (ratedMovies.Any())
            {
                AddMoviesToRatings(ratedMovies);
                if (ImportCancelled) return;
            }

            // add movies to watched history
            var watchedMovies = new List<TraktMovieWatched>();

            if (AppSettings.MarkAsWatched)
            {
                watchedMovies = tdMovieList.Where(tdm => tdm.Rating > 0 || tdm.Watched).Select(tdm => tdm.ToTraktWatchedMovie()).ToList();
            }
            else
            {
                watchedMovies = tdMovieList.Where(tdm => tdm.Watched).Select(tdm => tdm.ToTraktWatchedMovie()).ToList();
            }

            if (watchedMovies.Any())
            {
                AddMoviesToWatchedHistory(watchedMovies);
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

                watchedMovies.RemoveAll(w => lWatchedTraktMovies.FirstOrDefault(t => t.Movie.Ids.TmdbId == w.Ids.TmdbId) != null);

                UIUtils.UpdateStatus("Importing {0} ToDoMovies as watched...", watchedMovies.Count);

                if (watchedMovies.Count() > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)System.Math.Ceiling((double)watchedMovies.Count() / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        if (ImportCancelled) return;

                        UIUtils.UpdateStatus("Importing page {0}/{1} ToDoMovies watched history...", i + 1, pages);

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

                ratedMovies.RemoveAll(w => lRatedTraktMovies.FirstOrDefault(t => t.Movie.Ids.TmdbId == w.Ids.TmdbId) != null);

                UIUtils.UpdateStatus("Importing {0} ToDoMovies ratings...", ratedMovies.Count);

                if (ratedMovies.Count() > 0)
                {
                    int pageSize = AppSettings.BatchSize;
                    int pages = (int)System.Math.Ceiling((double)ratedMovies.Count() / pageSize);
                    for (int i = 0; i < pages; i++)
                    {
                        if (ImportCancelled) return;

                        UIUtils.UpdateStatus("Importing page {0}/{1} ToDoMovies ratings...", i + 1, pages);

                        var ratingsToSync = new TraktMovieRatingSync()
                        {
                            movies = ratedMovies.Skip(i * pageSize).Take(pageSize).ToList()
                        };

                        var addToWatchlistResponse = TraktAPI.TraktAPI.AddMoviesToRatings(ratingsToSync);
                        HandleResponse(addToWatchlistResponse);
                    }
                }
            }
        }
        
        private void HandleResponse(TraktSyncResponse response)
        {
            if (response == null)
            {
                UIUtils.UpdateStatus("Error importing ToDoMovies list to trakt.tv", true);
                Thread.Sleep(2000);
            }
            else if (response.NotFound.Movies.Count > 0)
            {
                UIUtils.UpdateStatus("Unable to process {0} movies as they're not found on trakt.tv!", response.NotFound.Movies.Count);
                foreach (var movie in response.NotFound.Movies)
                {
                    UIUtils.UpdateStatus("Unable to process movie: Title = {0}, TMDb = {1}", movie.Title, movie.Ids.TmdbId);
                }
                Thread.Sleep(1000);
            }
        }

        private List<ToDoMoviesListItem> ParseToDoMoviesCsv()
        {
            UIUtils.UpdateStatus("Parsing ToDoMovies CSV file");
            var textReader = File.OpenText(ToDoMoviesFilename);

            var csv = new CsvReader(textReader, csvConfiguration);
            csv.Context.RegisterClassMap<CSVFileDefinitionMap>();
            return csv.GetRecords<ToDoMoviesListItem>().ToList();
        }

        #endregion
    }
}

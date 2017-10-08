using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using TraktRater.Sites.API.iCheckMovies;
using TraktRater.TraktAPI.DataStructures;

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
            var traktMovies = icmMovieList.Select(icm => icm.ToTraktMovie()).ToList();
            var moviesToSync = new TraktMovieSync()
            {
                Movies = traktMovies
            };
            if (importCancelled)
            {
                return;
            }
            var response = TraktAPI.TraktAPI.AddMoviesToWatchlist(moviesToSync);

        }

        private List<ICheckMoviesListItem> ParseIcheckMoviesCsv()
        {
            var textReader = File.OpenText(iCheckMoviesFilename);

            var csv = new CsvReader(textReader, csvConfiguration);
            return csv.GetRecords<ICheckMoviesListItem>().ToList();
        }
    }

}
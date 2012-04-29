using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TraktRater.UI;
using TraktRater.TraktAPI;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.Sites.API.IMDb;
using Microsoft.VisualBasic.FileIO;
using TraktRater.Settings;

namespace TraktRater.Sites
{
    public class IMDb : IRateSite
    {
        #region Variables
  
        bool ImportCancelled = false;
        string CSVFile = null;
        List<Dictionary<string, string>> RateItems = new List<Dictionary<string, string>>();

        #endregion

        #region Constructor

        public IMDb(string filename)
        {
            CSVFile = filename;
            Enabled = !string.IsNullOrEmpty(filename) && File.Exists(filename);
        }

        #endregion

        #region IRateSite Members

        public bool Enabled { get; set; }

        public string Name { get { return "IMDb"; } }

        public void ImportRatings()
        {
            ImportCancelled = false;

            UIUtils.UpdateStatus("Reading IMDb ratings export...");
            if (!ParseCSVFile(CSVFile))
            {
                UIUtils.UpdateStatus("Failed to parse IMDb ratings file!", true);
                Thread.Sleep(2000);
                return;
            }
            if (ImportCancelled) return;

            // IMDb does not return the season and episode number for TV Episodes
            // so we should filter down to TV Shows and Movies only

            #region Movies
            var movies = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Movie);
            if (movies.Count() > 0)
            {
                UIUtils.UpdateStatus(string.Format("Importing {0} movie ratings to trakt.tv.", movies.Count()));

                TraktRatingsResponse response = TraktAPI.TraktAPI.RateMovies(GetRateMoviesData(movies));
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Error importing movie ratings to trakt.tv.", true);
                    Thread.Sleep(2000);
                }
            }
            if (ImportCancelled) return;
            #endregion

            #region TV Shows
            var shows = RateItems.Where(r => r[IMDbFieldMapping.cType].ItemType() == IMDbType.Show);
            if (shows.Count() > 0)
            {
                UIUtils.UpdateStatus(string.Format("Importing {0} show ratings to trakt.tv.", shows.Count()));

                TraktRatingsResponse response = TraktAPI.TraktAPI.RateShows(GetRateShowsData(shows));
                if (response == null || response.Status != "success")
                {
                    UIUtils.UpdateStatus("Error importing show ratings to trakt.tv.", true);
                    Thread.Sleep(2000);
                }
            }
            if (ImportCancelled) return;
            #endregion

            return;
        }

        public void Cancel()
        {
            // signals to cancel import
            ImportCancelled = true;
        }

        #endregion

        #region Private Methods

        private TraktRateMovies GetRateMoviesData(IEnumerable<Dictionary<string,string>> movies)
        {
            var traktMovies = new List<TraktMovie>();

            traktMovies.AddRange(from movie in movies
                                 select new TraktMovie
                                 {
                                     IMDbId = movie[IMDbFieldMapping.cIMDbID],
                                     Title = movie[IMDbFieldMapping.cTitle],
                                     Year = int.Parse(movie[IMDbFieldMapping.cYear]),
                                     Rating = int.Parse(movie[IMDbFieldMapping.cRating])
                                 });

            var movieRateData = new TraktRateMovies
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Movies = traktMovies
            };

            return movieRateData;
        }

        private TraktRateShows GetRateShowsData(IEnumerable<Dictionary<string, string>> shows)
        {
            var traktShows = new List<TraktShow>();

            traktShows.AddRange(from show in shows
                                 select new TraktShow
                                 {
                                     IMDbId = show[IMDbFieldMapping.cIMDbID],
                                     Title = show[IMDbFieldMapping.cTitle],
                                     Year = int.Parse(show[IMDbFieldMapping.cYear]),
                                     Rating = int.Parse(show[IMDbFieldMapping.cRating])
                                 });

            var movieRateData = new TraktRateShows
            {
                Username = AppSettings.TraktUsername,
                Password = AppSettings.TraktPassword,
                Shows = traktShows
            };

            return movieRateData;
        }

        private bool ParseCSVFile(string filename)
        {
            if (!File.Exists(filename)) return false;

            string[] fieldHeadings = new string[]{};
            
            try
            {
                TextFieldParser parser = new TextFieldParser(filename);
                parser.TextFieldType = FieldType.Delimited;
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
                    var rateItem = new Dictionary<string, string>();

                    foreach (string field in fields)
                    {
                        rateItem.Add(fieldHeadings[index], field);
                        index++;
                    }

                    // add to list of items
                    RateItems.Add(rateItem);
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

using CsvHelper.Configuration;
using global::TraktRater.Extensions;
using global::TraktRater.Settings;
using global::TraktRater.TraktAPI.DataStructures;
using System;

namespace TraktRater.Sites.API.ToDoMovies
{
    sealed class CSVFileDefinitionMap : CsvClassMap<ToDoMoviesListItem>
    {
        public CSVFileDefinitionMap()
        {
            Map(m => m.Title).Name("Title");
            Map(m => m.Genre).Name("Genre");
            Map(m => m.ReleaseDate).Name("Release Date");
            Map(m => m.Watched).Name("Watched");
            Map(m => m.Rating).Name("Your Rating");
            Map(m => m.ToDoMovieUrl).Name("TodoMovies URL");
            Map(m => m.TheMovieDatabaseID).Name("The Movie Database ID");
        }
    }

    class ToDoMoviesListItem
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ReleaseDate { get; set; }
        public bool Watched { get; set; }
        public int? Rating { get; set; }
        public string ToDoMovieUrl { get; set; }
        public int TheMovieDatabaseID { get; set; }
        
        public TraktMovieRating ToTraktRatedMovie()
        {
            return new TraktMovieRating()
            {
                Ids = new TraktMovieId() { TmdbId = TheMovieDatabaseID },
                Title = Title,
                Rating = Convert.ToUInt16(Rating) * 2
            };
        }

        public TraktMovieWatched ToTraktWatchedMovie()
        {
            return new TraktMovieWatched()
            {
                Ids = new TraktMovieId() { TmdbId = TheMovieDatabaseID },
                Title = Title,
                WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : DateTime.UtcNow.ToString().ToISO8601()
            };
        }
    }
}

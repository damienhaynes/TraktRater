using CsvHelper.Configuration;
using global::TraktRater.Extensions;
using global::TraktRater.Settings;
using global::TraktRater.TraktAPI.DataStructures;
using System;

namespace TraktRater.Sites.API.MovieLens
{
    sealed class CSVFileDefinitionMap : CsvClassMap<MovieLensItem>
    {
        public CSVFileDefinitionMap()
        {
            Map(m => m.MovieId).Name("movie_id");
            Map(m => m.ImdbId).Name("imdb_id");
            Map(m => m.TmdbId).Name("tmdb_id");
            Map(m => m.Rating).Name("rating");
            Map(m => m.AverageRating).Name("average_rating");
            Map(m => m.Title).Name("title");
        }
    }

    class MovieLensItem
    {
        public string Title { get; set; }        
        public float Rating { get; set; }
        public float AverageRating { get; set; }
        public int MovieId { get; set; }
        public int ImdbId { get; set; }
        public int TmdbId { get; set; }

        public TraktMovieRating ToTraktRatedMovie()
        {
            return new TraktMovieRating()
            {
                Ids = new TraktMovieId()
                { 
                    TmdbId = TmdbId,
                    ImdbId = "tt" + ImdbId.ToString().PadLeft(7, '0')
                },
                // rating is a decimal between 0 and 5
                Rating = (int)(Rating * 2)
            };
        }

        public TraktMovieWatched ToTraktWatchedMovie()
        {
            return new TraktMovieWatched()
            {
                Ids = new TraktMovieId()
                {
                    TmdbId = TmdbId,
                    ImdbId = "tt" + ImdbId.ToString().PadLeft(7, '0')
                },
                WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : DateTime.UtcNow.ToString().ToISO8601()
            };
        }
    }
}

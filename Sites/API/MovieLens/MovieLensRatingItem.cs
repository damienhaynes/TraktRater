using CsvHelper.Configuration;
using global::TraktRater.Extensions;
using global::TraktRater.Settings;
using global::TraktRater.TraktAPI.DataStructures;
using System;

namespace TraktRater.Sites.API.MovieLens
{
    sealed class CSVRatingsFileDefinitionMap : CsvClassMap<MovieLensRatingItem>
    {
        public CSVRatingsFileDefinitionMap()
        {
            Map(m => m.MovieId).Name("movie_id");
            Map(m => m.ImdbId).Name("imdb_id");
            Map(m => m.TmdbId).Name("tmdb_id");
            Map(m => m.Rating).Name("rating");
            Map(m => m.AverageRating).Name("average_rating");
            Map(m => m.Title).Name("title");
        }
    }

    class MovieLensRatingItem
    {
        public string Title { get; set; }        
        public float Rating { get; set; }
        public float AverageRating { get; set; }
        public int MovieId { get; set; }
        public int ImdbId { get; set; }
        public int TmdbId { get; set; }

        public TraktMovieRating ToTraktRatedMovie(MovieLensActivityItem.MovieRatingActivity aRatingActivity)
        {
            return new TraktMovieRating()
            {
                Ids = new TraktMovieId()
                { 
                    TmdbId = TmdbId,
                    ImdbId = "tt" + ImdbId.ToString().PadLeft(7, '0')
                },
                // rating is a decimal between 0 and 5
                Rating = (int)(Rating * 2),
                RatedAt = GetRatingDate(aRatingActivity)
            };
        }

        public TraktMovieWatched ToTraktWatchedMovie(MovieLensActivityItem.MovieRatingActivity aRatingActivity)
        {
            return new TraktMovieWatched()
            {
                Ids = new TraktMovieId()
                {
                    TmdbId = TmdbId,
                    ImdbId = "tt" + ImdbId.ToString().PadLeft(7, '0')
                },
                WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : GetRatingDate(aRatingActivity)
            };
        }

        private string GetRatingDate(MovieLensActivityItem.MovieRatingActivity aRatingActivity)
        {
            if (aRatingActivity == null)
                return null;

            if (!DateTime.TryParse(aRatingActivity.Date, out DateTime lResult))
                return null;

            return lResult.ToString().ToISO8601();
        }

    }
}

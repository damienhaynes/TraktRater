using CsvHelper.Configuration;
using TraktRater.TraktAPI.DataStructures;

namespace TraktRater.Sites.API.MovieLens
{
    sealed class CSVWishlistFileDefinitionMap : ClassMap<MovieLensWishlistItem>
    {
        public CSVWishlistFileDefinitionMap()
        {
            Map(m => m.MovieId).Name("movie_id");
            Map(m => m.ImdbId).Name("imdb_id");
            Map(m => m.TmdbId).Name("tmdb_id");
            Map(m => m.AverageRating).Name("average_rating");
            Map(m => m.Title).Name("title");
        }
    }

    class MovieLensWishlistItem
    {
        public string Title { get; set; }
        public float AverageRating { get; set; }
        public int MovieId { get; set; }
        public int ImdbId { get; set; }
        public int TmdbId { get; set; }

        public TraktMovie ToTraktWatchlistMovie()
        {
            return new TraktMovie()
            {
                Ids = new TraktMovieId()
                {
                    TmdbId = TmdbId,
                    ImdbId = "tt" + ImdbId.ToString().PadLeft(7, '0')
                }
            };
        }
    }
}

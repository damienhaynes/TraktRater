using CsvHelper.Configuration;
using global::TraktRater.Extensions;
using global::TraktRater.Settings;
using global::TraktRater.TraktAPI.DataStructures;
using System;

namespace TraktRater.Sites.API.Criticker
{
    sealed class CSVFileDefinitionMap : CsvClassMap<CritickerItem>
    {
        public CSVFileDefinitionMap()
        {
            Map(m => m.Rating).Name("Score");
            Map(m => m.Title).Name(" Film Name");
            Map(m => m.Year).Name(" Year");
            Map(m => m.MiniReview).Name(" Mini Review");
            Map(m => m.Url).Name(" URL");
            Map(m => m.IMDbId).Name(" IMDB ID");
        }
    }

    class CritickerItem
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int? Year { get; set; }        
        public int Rating { get; set; }
        public string Url { get; set; }
        public string MiniReview { get; set; }
        public string IMDbId { get; set; }

        public bool IsTvShow
        {
            get
            {
                return (Url ?? string.Empty).ToLowerInvariant().Contains("/tv/");
            }
        }

        public TraktMovieRating ToTraktRatedMovie()
        {
            return new TraktMovieRating()
            {
                Ids = new TraktMovieId() { ImdbId = IMDbId },
                Title = Title,
                Year = Year,
                Rating = Convert.ToInt32(Math.Round(Rating / 10.0, MidpointRounding.AwayFromZero)),
            };
        }

        public TraktShowRating ToTraktRatedShow()
        {
            return new TraktShowRating()
            {
                Ids = new TraktShowId() { ImdbId = IMDbId },
                Title = Title,
                Year = Year,
                Rating = Convert.ToInt32(Math.Round(Rating / 10.0, MidpointRounding.AwayFromZero)),
            };
        }

        public TraktMovieWatched ToTraktWatchedMovie()
        {
            return new TraktMovieWatched()
            {
                Ids = new TraktMovieId() { ImdbId = IMDbId },
                Title = Title,
                Year = Year,
                WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : DateTime.UtcNow.ToString().ToISO8601()
            };
        }
    }
}

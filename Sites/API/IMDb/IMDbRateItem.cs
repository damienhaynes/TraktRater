namespace TraktRater.Sites.API.IMDb
{
    using CsvHelper.Configuration;
    using global::TraktRater.Extensions;
    using global::TraktRater.Settings;
    using global::TraktRater.TraktAPI.DataStructures;
    using System;

    sealed class IMDbRatingCsvMap : ClassMap<IMDbRateItem>
    {
        public IMDbRatingCsvMap()
        {
            Map(m => m.Id).Name("Const");
            Map(m => m.MyRating).Name("Your Rating");
            Map(m => m.RatedDate).Name("Date Rated");
            Map(m => m.Title).Name("Title");
            Map(m => m.Url).Name("URL");
            Map(m => m.Type).Name("Title Type");
            Map(m => m.SiteRating).Name("IMDb Rating");
            Map(m => m.Runtime).Name("Runtime (mins)");
            Map(m => m.Year).Name("Year");
            Map(m => m.Genres).Name("Genres");
            Map(m => m.Votes).Name("Num Votes");
            Map(m => m.ReleaseDate).Name("Release Date");
            Map(m => m.Directors).Name("Directors");
        }
    }

    public class IMDbRateItem
    {
        /// <summary>
        /// Example Header as of 17th Aug 2019
        /// Const,Your Rating,Date Rated,Title,URL,Title Type,IMDb Rating,Runtime (mins),Year,Genres,Num Votes,Release Date,Directors
        /// </summary>
        public string Id { get; set; }

        public int? MyRating { get; set; }

        public string RatedDate { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Type { get; set; }

        public float? SiteRating { get; set; }
        
        public int? Runtime { get; set; }

        public int? Year { get; set; }

        public string Genres { get; set; }

        public int? Votes { get; set; }

        public string ReleaseDate { get; set; }

        public string Directors { get; set; }

        public TraktMovieRating ToTraktRatedMovie()
        {
            return new TraktMovieRating()
            {
                Ids = new TraktMovieId() { ImdbId = Id },
                Title = Title,
                Year = Year,
                Rating = MyRating.GetValueOrDefault(0),
                RatedAt = GetFormattedDate(RatedDate)
            };
        }

        public TraktShowRating ToTraktRatedShow()
        {
            return new TraktShowRating()
            {
                Ids = new TraktShowId() { ImdbId = Id },
                Title = Title,
                Year = Year,
                Rating = MyRating.GetValueOrDefault(0),
                RatedAt = GetFormattedDate(RatedDate)
            };
        }

        public TraktMovieWatched ToTraktWatchedMovie()
        {
            return new TraktMovieWatched()
            {
                Ids = new TraktMovieId() { ImdbId = Id },
                Title = Title,
                Year = Year,
                WatchedAt = AppSettings.WatchedOnReleaseDay ? "released" : GetFormattedDate(RatedDate)
            };
        }
        
        private string GetFormattedDate(string imdbDateTime)
        {
            string createdDate = DateTime.Now.ToString().ToISO8601();

            if (!string.IsNullOrEmpty(imdbDateTime))
            {
                // date is in the form 'YYYY-MM-DD' (but no guarantee for other locales)
                if (DateTime.TryParse(imdbDateTime, out DateTime result))
                {
                    createdDate = result.ToString().ToISO8601();
                }
            }

            return createdDate;
        }
    }
}

namespace TraktRater.Sites.API.IMDb
{
    using CsvHelper.Configuration;
    using global::TraktRater.TraktAPI.DataStructures;

    sealed class IMDbListCsvMap : CsvClassMap<IMDbListItem>
    {
        public IMDbListCsvMap()
        {
            Map(m => m.Position).Name("Position");
            Map(m => m.Id).Name("Const");
            Map(m => m.CreatedDate).Name("Created");
            Map(m => m.ModifiedDate).Name("Modified");
            Map(m => m.Description).Name("Description");
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

    class IMDbListItem
    {
        /// <summary>
        /// Example Header as of 17th Aug 2019 (same for watchlist and custom lists)
        /// Position,Const,Created,Modified,Description,Title,URL,Title Type,IMDb Rating,Runtime (mins),Year,Genres,Num Votes,Release Date,Directors
        /// </summary>

        public int? Position { get; set; }

        public string Id { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }

        public string Description { get; set; }

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
        
        public TraktMovie ToTraktMovie()
        {
            return new TraktMovie()
            {
                Ids = new TraktMovieId() { ImdbId = Id },
                Title = Title,
                Year = Year
            };
        }

        public TraktShow ToTraktShow()
        {
            return new TraktShow()
            {
                Ids = new TraktShowId() { ImdbId = Id },
                Title = Title,
                Year = Year
            };
        }
    }
}

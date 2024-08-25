namespace TraktRater.Sites.API.Letterboxd
{
    using CsvHelper.Configuration;
    using global::TraktRater.TraktAPI.DataStructures;

    sealed class LetterboxdListCsvMap : ClassMap<LetterboxdListItem>
    {
        public LetterboxdListCsvMap()
        {
            Map( m => m.Rank ).Name( "Position" );
            Map( m => m.Title ).Name( "Name" );
            Map( m => m.Url ).Name( "URL" );
            Map( m => m.Year ).Name( "Year" );
            Map( m => m.Description ).Name( "Description" );
        }
    }

    public class LetterboxdListItem
    {
        /// Example Header for movie list as of 2nd Jan 2020
        /// Position,Name,Year,URL,Description
        /// 
        /// Example Header for list details as of 2nd Jan 2020
        /// Date,Name,Tags,URL,Description
        
        public int Rank { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
        
        public int? Year { get; set; }
        
        public string Description { get; set; }

        public TraktMovie ToTraktMovie()
        {
            return new TraktMovie()
            {
                Title = Title,
                Year = Year
            };
        }
    }
}

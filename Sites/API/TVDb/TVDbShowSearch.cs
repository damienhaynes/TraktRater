namespace TraktRater.Sites.API.TVDb
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("Data")]
    public class TVDbShowSearch
    {
        [XmlElement("Series")]
        public List<Series> Shows { get; set; }

        public class Series
        {
            [XmlElement("SeriesName")]
            public string Name { get; set; }

            [XmlElement("id")]
            public int Id { get; set; }

            [XmlElement("FirstAired")]
            public string FirstAired { get; set; }

            [XmlElement("IMDB_ID")]
            public string ImdbId { get; set; }
        }
    }
}

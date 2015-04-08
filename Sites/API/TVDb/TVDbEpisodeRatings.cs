namespace TraktRater.Sites.API.TVDb
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("Data")]
    public class TVDbEpisodeRatings
    {
        public TVDbEpisodeRatings() { Episodes = new List<Episode>(); }

        [XmlElement("Episode")]
        public List<Episode> Episodes { get; set; }

        [XmlElement("Series")]
        public Series Show { get; set; }

        public class Series
        {
            [XmlElement("seriesid")]
            public Int32 Id { get; set; }

            [XmlElement("UserRating")]
            public Int32 UserRating { get; set; }

            [XmlElement("CommunityRating")]
            public double CommunityRating { get; set; }
        }

        public class Episode
        {
            [XmlElement("id")]
            public Int32 Id { get; set; }

            [XmlElement("UserRating")]
            public Int32 UserRating { get; set; }

            [XmlElement("CommunityRating")]
            public double CommunityRating { get; set; }
        }
    }

    
}

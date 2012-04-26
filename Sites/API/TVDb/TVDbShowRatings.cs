using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TraktRater.Sites.API.TVDb
{
    [XmlRoot("Data")]
    public class TVDbShowRatings
    {
        public TVDbShowRatings() { Shows = new List<Series>(); }

        [XmlElement("Series")]
        public List<Series> Shows { get; set; }

        public class Series
        {
            [XmlElement("seriesid")]
            public Int32 Id { get; set; }

            [XmlElement("UserRating")]
            public Int32 UserRating { get; set; }

            [XmlElement("CommunityRating")]
            public double CommunityRating { get; set; }
        }
    }

    
}

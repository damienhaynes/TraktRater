using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TraktRater.Sites.API.TVDb
{
    [XmlRoot("Data")]
    public class TVDbShow
    {
        public TVDbShow() { Episodes = new List<Episode>(); }

        [XmlElement("Series")]
        public Series Show { get; set; }

        [XmlElement("Episode")]
        public List<Episode> Episodes { get; set; }

        public class Series
        {
            [XmlElement("SeriesName")]
            public string Name { get; set; }
        }

        public class Episode
        {
            [XmlElement("id")]
            public Int32 Id { get; set; }

            [XmlElement("SeasonNumber")]
            public Int32 SeasonNumber { get; set; }

            [XmlElement("EpisodeNumber")]
            public Int32 EpisodeNumber { get; set; }

            [XmlElement("EpisodeName")]
            public string Name { get; set; }

            [XmlElement("FirstAired")]
            public string AirDate { get; set; }
        }
    }


}

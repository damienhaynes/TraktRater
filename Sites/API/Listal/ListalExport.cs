using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TraktRater.Sites.API.Listal
{
    [XmlRoot("rss")]
    public class ListalExport
    {
        [XmlElement("channel")]
        public RSSChannel Channel { get; set; }

        public class RSSChannel
        {
            [XmlElement("title")]
            public string Title { get; set; }

            [XmlElement("link")]
            public string Link { get; set; }

            [XmlElement("description")]
            public string Description { get; set; }

            [XmlElement("language")]
            public string Language { get; set; }

            [XmlElement("pubDate")]
            public string PublishedDate { get; set; }

            [XmlElement("lastBuildDate")]
            public string LastBuildDate { get; set; }

            [XmlElement("generator")]
            public string Generator { get; set; }

            [XmlElement("item")]
            public List<Item> Items { get; set; }

            public class Item
            {
                [XmlElement("title")]
                public string Title { get; set; }

                [XmlElement("link")]
                public string Link { get; set; }

                [XmlElement("guid")]
                public string Guid { get; set; }

                [XmlElement("description")]
                public string Description { get; set; }

                [XmlElement("pubDate")]
                public string PublishedDate { get; set; }

                [XmlElement("rating", Namespace = "http://www.listal.com/rsshelp/")]
                public Int16 Rating { get; set; }

                [XmlElement("imdb", Namespace = "http://www.listal.com/rsshelp/")]
                public string IMDbId { get; set; }

                [XmlElement("condition", Namespace = "http://www.listal.com/rsshelp/")]
                public string Condition { get; set; }

                [XmlElement("notes", Namespace = "http://www.listal.com/rsshelp/")]
                public string Notes { get; set; }

                [XmlElement("loanedtype", Namespace = "http://www.listal.com/rsshelp/")]
                public string LoanedType { get; set; }

                [XmlElement("listtype", Namespace = "http://www.listal.com/rsshelp/")]
                public string ListType { get; set; }
            }
        }
    }
}

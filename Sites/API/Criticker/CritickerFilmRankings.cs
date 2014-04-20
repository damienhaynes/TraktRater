using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TraktRater.Sites.API.Criticker
{
    [XmlRoot("recentrankings")]
    public class CritickerFilmRankings
    {
        [XmlElement("film")]
        public List<Film> Films { get; set; }

        public class Film
        {
            [XmlElement("filmid")]
            public int FilmId { get; set; }

            [XmlElement("filmname")]
            public string FilmName { get; set; }

            [XmlElement("filmlink")]
            public string FilmLink { get; set; }

            [XmlElement("img")]
            public string Image { get; set; }

            [XmlElement("score")]
            public int Score { get; set; }

            [XmlElement("quote")]
            public string Quote { get; set; }

            [XmlElement("reviewdate")]
            public string ReviewDate { get; set; }

            [XmlElement("tier")]
            public string Tier { get; set; }

            /// <summary>
            /// Gets title from filmname
            /// </summary>
            public string Title
            {
                get
                {
                    var regMatch = Regex.Match(this.FilmName, @"^(?<title>.+?)(?:\s*[\(\[](?<year>\d{4})[\]\)])?$");
                    return regMatch.Groups["title"].Value;
                }
            }

            /// <summary>
            /// Gets year from filmname
            /// </summary>
            public int Year
            {
                get
                {
                    int iYear = 0;
                    var regMatch = Regex.Match(this.FilmName, @"^(?<title>.+?)(?:\s*[\(\[](?<year>\d{4})[\]\)])?$");
                    string year = regMatch.Groups["year"].Value;
                    
                    int.TryParse(year, out iYear);                    
                    return iYear;
                }
            }
        }
    }
}

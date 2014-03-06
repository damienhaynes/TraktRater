using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktUserEpisodeRating
    {
        [DataMember(Name = "inserted")]
        public long Inserted { get; set; }

        [DataMember(Name = "rating")]
        public string Rating { get; set; }

        [DataMember(Name = "rating_advanced")]
        public int RatingAdvanced { get; set; }

        [DataMember(Name = "show")]
        public Show ShowDetails { get; set; }

        [DataMember(Name = "episode")]
        public Episode EpisodeDetails { get; set; }

        [DataContract]
        public class Show
        {
            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "year")]
            public int Year { get; set; }

            [DataMember(Name = "imdb_id")]
            public string IMDBID { get; set; }

            [DataMember(Name = "tvdb_id")]
            public string TVDBID { get; set; }

            [DataMember(Name = "tvrage_id")]
            public string TVRageID { get; set; }
        }

        [DataContract]
        public class Episode
        {
            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "season")]
            public int SeasonNumber { get; set; }

            [DataMember(Name = "number")]
            public int EpisodeNumber { get; set; }

            [DataMember(Name = "tvdb_id")]
            public string TVDBID { get; set; }
        }
    }
}

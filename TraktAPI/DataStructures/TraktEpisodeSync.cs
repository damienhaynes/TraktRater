using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktEpisodeSync : TraktAuthentication
    {
        [DataMember(Name = "imdb_id")]
        public string IMDBID { get; set; }

        [DataMember(Name = "tvdb_id")]
        public string SeriesID { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "year")]
        public string Year { get; set; }

        [DataMember(Name = "episodes")]
        public List<Episode> EpisodeList { get; set; }

        [DataContract]
        public class Episode
        {
            [DataMember(Name = "season")]
            public string SeasonIndex { get; set; }

            [DataMember(Name = "episode")]
            public string EpisodeIndex { get; set; }
        }

    }
}

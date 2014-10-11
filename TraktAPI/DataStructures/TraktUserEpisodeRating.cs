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
        [DataMember(Name = "rated_at")]
        public string RatedAt { get; set; }

        [DataMember(Name = "rating")]
        public int Rating { get; set; }

        [DataMember(Name = "show")]
        public TraktShow Show { get; set; }

        [DataMember(Name = "episode")]
        public TraktEpisodeSyncSummary Episode { get; set; }

        [DataContract]
        public class TraktEpisodeSyncSummary
        {
            [DataMember(Name = "ids")]
            public TraktEpisodeId Ids { get; set; }

            [DataMember(Name = "number")]
            public int Number { get; set; }

            [DataMember(Name = "season")]
            public int Season { get; set; }

            [DataMember(Name = "title")]
            public string Title { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktShowSummary : TraktShow
    {
        [DataMember(Name = "seasons")]
        public List<TraktSeason> Seasons { get; set; }

        [DataContract]
        public class TraktSeason
        {
            [DataMember(Name = "episodes")]
            public List<TraktSeason.TraktEpisode> Episodes { get; set; }

            [DataMember(Name = "season")]
            public int Season { get; set; }

            [DataContract]
            public class TraktEpisode
            {
                [DataMember(Name = "tvdb_id")]
                public int TVDbId { get; set; }

                [DataMember(Name = "title")]
                public string Title { get; set; }

                [DataMember(Name = "first_aired")]
                public long FirstAired { get; set; }

                [DataMember(Name = "season")]
                public int Season { get; set; }

                [DataMember(Name = "episode")]
                public int Episode { get; set; }
            }
        }

    }
}

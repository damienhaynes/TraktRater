using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktEpisodeWatchedSyncEx
    {
        [DataMember(Name = "shows")]
        public List<TraktShowSeasonsWatched> shows { get; set; }

        [DataContract]
        public class TraktShowSeasonsWatched : TraktShow
        {
            [DataMember(Name = "seasons")]
            public List<TraktSeasonEpisodesWatched> seasons { get; set; }

            [DataContract]
            public class TraktSeasonEpisodesWatched
            {
                [DataMember(Name = "number")]
                public int Number { get; set; }

                [DataMember(Name = "episodes")]
                public List<TraktEpisodeWatchedEx> Episodes { get; set; }
            }
        }
    }

    [DataContract]
    public class TraktEpisodeWatchedSync
    {
        [DataMember(Name = "episodes")]
        public List<TraktEpisodeWatched> Episodes { get; set; }
    }
}

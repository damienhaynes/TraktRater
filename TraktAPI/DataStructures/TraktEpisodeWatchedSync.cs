namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktEpisodeWatchedSyncEx
    {
        [DataMember(Name = "shows")]
        public List<TraktShowSeasonsWatched> Shows { get; set; }

        [DataContract]
        public class TraktShowSeasonsWatched : TraktShow
        {
            [DataMember(Name = "seasons")]
            public List<TraktSeasonEpisodesWatched> Seasons { get; set; }

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

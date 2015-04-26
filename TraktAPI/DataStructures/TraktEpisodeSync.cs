namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktEpisodeSyncEx
    {
        [DataMember(Name = "shows")]
        public List<TraktShowSeasonEpisodes> Shows { get; set; }

        [DataContract]
        public class TraktShowSeasonEpisodes : TraktShow
        {
            [DataMember(Name = "seasons")]
            public List<TraktSeasonEpisodes> Seasons { get; set; }

            [DataContract]
            public class TraktSeasonEpisodes
            {
                [DataMember(Name = "number")]
                public int Number { get; set; }

                [DataMember(Name = "episodes")]
                public List<TraktEpisodeEx> Episodes { get; set; }
            }
        }
    }

    [DataContract]
    public class TraktEpisodeSync
    {
        [DataMember(Name = "episodes")]
        public List<TraktEpisode> Episodes { get; set; }
    }
}

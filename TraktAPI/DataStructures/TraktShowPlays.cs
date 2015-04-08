namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktShowPlays
    {
        [DataMember(Name = "plays")]
        public int Plays { get; set; }

        [DataMember(Name = "show")]
        public TraktShowItem Show { get; set; }

        [DataContract]
        public class TraktShowItem : TraktShow
        {
            [DataMember(Name = "seasons")]
            public List<TraktSeasonItem> Seasons { get; set; }

            [DataContract]
            public class TraktSeasonItem : TraktSeason
            {
                [DataMember(Name = "episodes")]
                public List<TraktEpisodeWatchedEx> Episodes { get; set; }
            }
        }
    }
}

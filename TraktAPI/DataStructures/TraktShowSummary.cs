namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktShowSummary : TraktShow
    {
        [DataMember(Name = "seasons")]
        public List<TraktSeason> Seasons { get; set; }

        [DataContract]
        public class TraktSeason
        {
            [DataMember(Name = "number")]
            public int Number { get; set; }

            [DataMember(Name = "episodes")]
            public List<TraktEpisode> Episodes { get; set; }
        }
    }
}

namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktSeasonSync
    {
        [DataMember(Name = "shows")]
        public List<TraktShowSeason> Shows { get; set; }

        [DataContract]
        public class TraktShowSeason : TraktShow
        {
            [DataMember(Name = "seasons")]
            public List<TraktSeason> Seasons { get; set; }
        }
    }
}

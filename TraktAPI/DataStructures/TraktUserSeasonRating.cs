namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktUserSeasonRating
    {
        [DataMember(Name = "rated_at")]
        public string RatedAt { get; set; }

        [DataMember(Name = "rating")]
        public int Rating { get; set; }

        [DataMember(Name = "show")]
        public TraktShow Show { get; set; }

        [DataMember(Name = "season")]
        public TraktSeasonEx Season { get; set; }

        [DataContract]
        public class TraktSeasonEx : TraktSeason
        {
            [DataMember(Name = "ids")]
            public TraktSeasonId Ids { get; set; }
        }
    }
}

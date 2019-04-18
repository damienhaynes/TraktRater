namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktSeasonWatchlist
    {
        [DataMember(Name = "listed_at")]
        public string InsertedAt { get; set; }

        [DataMember(Name = "rank")]
        public int Rank { get; set; }

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

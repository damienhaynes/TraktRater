namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktEpisodeWatchlist
    {
        [DataMember(Name = "episode")]
        public TraktEpisodeSummary Episode { get; set; }

        [DataMember(Name = "show")]
        public TraktShow Show { get; set; }

        [DataMember(Name = "listed_at")]
        public string InsertedAt { get; set; }

        [DataMember(Name = "rank")]
        public int Rank { get; set; }
    }
}

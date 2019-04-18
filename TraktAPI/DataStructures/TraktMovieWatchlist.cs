namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktMovieWatchlist
    {
        [DataMember(Name = "movie")]
        public TraktMovie Movie { get; set; }

        [DataMember(Name = "listed_at")]
        public string InsertedAt { get; set; }

        [DataMember(Name = "rank")]
        public int Rank { get; set; }
    }
}

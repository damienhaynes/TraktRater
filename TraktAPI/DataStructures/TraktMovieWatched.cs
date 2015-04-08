namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktMovieWatched : TraktMovie
    {
        [DataMember(Name = "watched_at")]
        public string WatchedAt { get; set; }
    }

    [DataContract]
    public class TraktMoviePlays
    {
        [DataMember(Name = "movie")]
        public TraktMovie Movie { get; set; }

        [DataMember(Name = "plays")]
        public int Plays { get; set; }

    }
}

namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktMovieCollected
    {
        [DataMember(Name = "movie")]
        public TraktMovie Movie { get; set; }

        [DataMember(Name = "collected_at")]
        public string CollectedAt { get; set; }
    }

    public class TraktMovieCollection : TraktMovie
    {
        [DataMember(Name = "collected_at")]
        public string CollectedAt { get; set; }
    }
}

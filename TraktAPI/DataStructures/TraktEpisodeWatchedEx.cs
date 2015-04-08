namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktEpisodeWatchedEx : TraktEpisodeEx
    {
        [DataMember(Name = "watched_at")]
        public string WatchedAt { get; set; }

        [DataMember(Name = "plays")]
        public int Plays { get; set; }
    }
}

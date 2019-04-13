using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktSyncPaused
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }

        [DataMember(Name = "progress")]
        public float Progress { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "paused_at")]
        public string PausedAt { get; set; }
    }

    [DataContract]
    public class TraktSyncPausedMovie : TraktSyncPaused
    {
        [DataMember(Name = "movie")]
        public TraktMovie Movie { get; set; }
    }

    [DataContract]
    public class TraktSyncPausedEpisode : TraktSyncPaused
    {
        [DataMember(Name = "show")]
        public TraktShow Show { get; set; }

        [DataMember(Name = "episode")]
        public TraktEpisodeSummary Episode { get; set; }
    }
}

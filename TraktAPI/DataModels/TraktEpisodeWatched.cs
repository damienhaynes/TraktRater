namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktEpisodeWatched : TraktEpisode
  {
    [DataMember( Name = "watched_at" )]
    public string WatchedAt { get; set; }
  }
}

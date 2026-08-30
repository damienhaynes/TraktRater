namespace TraktRater.TraktAPI.DataStructures
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktUserStats
  {
    [DataMember( Name = "rating" )]
    public uint Rating { get; set; }

    [DataMember( Name = "play_count" )]
    public uint PlayCount { get; set; }

    [DataMember( Name = "completed_count" )]
    public uint CompletedCount { get; set; }
  }
}

namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktEpisode
  {
    [DataMember( Name = "ids" )]
    public TraktEpisodeId Ids { get; set; }
  }
}

namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktEpisodeEx
  {
    [DataMember( Name = "number" )]
    public int Number { get; set; }
  }
}

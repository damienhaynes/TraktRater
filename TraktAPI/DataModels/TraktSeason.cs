namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktSeason
  {
    [DataMember( Name = "number" )]
    public int Number { get; set; }
  }
}

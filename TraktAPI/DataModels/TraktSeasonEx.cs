using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataModels
{
  [DataContract]
  public class TraktSeasonEx : TraktSeason
  {
    [DataMember( Name = "ids" )]
    public TraktSeasonId Ids { get; set; }
  }
}

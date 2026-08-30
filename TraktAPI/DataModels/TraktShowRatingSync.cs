namespace TraktRater.TraktAPI.DataModels
{
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  public class TraktShowRatingSync
  {
    [DataMember( Name = "shows" )]
    public List<TraktShowRating> shows { get; set; }
  }
}

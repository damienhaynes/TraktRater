namespace TraktRater.TraktAPI.DataModels
{
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktShowSync
  {
    [DataMember( Name = "shows" )]
    public List<TraktShow> Shows { get; set; }
  }
}

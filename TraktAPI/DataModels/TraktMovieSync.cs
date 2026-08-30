namespace TraktRater.TraktAPI.DataModels
{
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktMovieSync
  {
    [DataMember( Name = "movies" )]
    public List<TraktMovie> Movies { get; set; }
  }
}

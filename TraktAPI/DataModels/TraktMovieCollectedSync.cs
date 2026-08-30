namespace TraktRater.TraktAPI.DataModels
{
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktMovieCollectedSync
  {
    [DataMember( Name = "movies" )]
    public List<TraktMovieCollection> Movies { get; set; }
  }
}

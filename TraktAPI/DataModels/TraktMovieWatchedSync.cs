namespace TraktRater.TraktAPI.DataModels
{
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktMovieWatchedSync
  {
    [DataMember( Name = "movies" )]
    public List<TraktMovieWatched> Movies { get; set; }
  }
}

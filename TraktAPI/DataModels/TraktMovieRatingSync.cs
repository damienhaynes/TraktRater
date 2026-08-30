namespace TraktRater.TraktAPI.DataModels
{
  using System.Collections.Generic;
  using System.Runtime.Serialization;

  public class TraktMovieRatingSync
  {
    [DataMember( Name = "movies" )]
    public List<TraktMovieRating> movies { get; set; }
  }
}

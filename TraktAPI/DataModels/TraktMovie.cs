namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktMovie
  {
    [DataMember( Name = "title" )]
    public string Title { get; set; }

    [DataMember( Name = "year" )]
    public int? Year { get; set; }

    [DataMember( Name = "ids" )]
    public TraktMovieId Ids { get; set; }
  }
}

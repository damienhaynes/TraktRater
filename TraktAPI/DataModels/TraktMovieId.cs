namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktMovieId : TraktId
  {
    [DataMember( Name = "imdb", EmitDefaultValue = false )]
    public string ImdbId { get; set; }

    [DataMember( Name = "tmdb", EmitDefaultValue = false )]
    public int? TmdbId { get; set; }
  }
}

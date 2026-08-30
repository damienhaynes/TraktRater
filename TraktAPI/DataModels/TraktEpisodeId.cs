namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktEpisodeId : TraktId
  {
    [DataMember( Name = "imdb" )]
    public string ImdbId { get; set; }

    [DataMember( Name = "tmdb" )]
    public int? TmdbId { get; set; }

    [DataMember( Name = "tvdb" )]
    public int? TvdbId { get; set; }
  }
}

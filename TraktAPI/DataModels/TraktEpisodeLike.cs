namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktEpisodeLike
  {
    [DataMember( Name = "number" )]
    public int Number { get; set; }

    [DataMember( Name = "season" )]
    public int Season { get; set; }

    [DataMember( Name = "title" )]
    public string Title { get; set; }

    [DataMember( Name = "ids" )]
    public TraktEpisodeId Ids { get; set; }
  }
}

namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktId
  {
    [DataMember( Name = "trakt", EmitDefaultValue = false )]
    public int? Trakt { get; set; }

    [DataMember( Name = "slug", EmitDefaultValue = false )]
    public string Slug { get; set; }
  }
}

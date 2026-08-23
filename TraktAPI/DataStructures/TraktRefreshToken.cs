namespace TraktRater.TraktAPI.DataStructures
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktRefreshToken
  {
    [DataMember( Name = "refresh_token" )]
    public string RefreshToken { get; set; }

    [DataMember( Name = "client_id" )]
    public string ClientId { get; set; }

    [DataMember( Name = "client_secret" )]
    public string ClientSecret { get; set; }

    [DataMember( Name = "redirect_uri" )]
    public string RedirectUrl { get; set; }

    [DataMember( Name = "grant_type" )]
    public string GrantType { get; set; }
  }
}

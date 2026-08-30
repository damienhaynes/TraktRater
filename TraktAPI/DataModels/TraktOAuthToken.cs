namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktOAuthToken
  {
    [DataMember( Name = "access_token" )]
    public string AccessToken { get; set; }

    [DataMember( Name = "token_type" )]
    public string TokenType { get; set; }

    [DataMember( Name = "expires_in" )]
    public uint ExpiresIn { get; set; }

    [DataMember( Name = "refresh_token" )]
    public string RefreshToken { get; set; }

    [DataMember( Name = "scope" )]
    public string Scope { get; set; }

    [DataMember( Name = "created_at" )]
    public ulong CreatedAt { get; set; }
  }
}

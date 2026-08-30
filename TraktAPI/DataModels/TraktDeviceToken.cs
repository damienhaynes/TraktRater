namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktDeviceToken
  {
    [DataMember( Name = "code" )]
    public string Code { get; set; }

    [DataMember( Name = "client_id" )]
    public string ClientId { get; set; }

    [DataMember( Name = "client_secret" )]
    public string ClientSecret { get; set; }
  }
}

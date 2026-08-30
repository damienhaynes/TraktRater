namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktUserToken
  {
    [DataMember( Name = "token" )]
    public string Token { get; set; }
  }
}

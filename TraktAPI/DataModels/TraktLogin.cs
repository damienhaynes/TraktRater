namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktLogin
  {
    [DataMember( Name = "login" )]
    public string Login { get; set; }

    [DataMember( Name = "password" )]
    public string Password { get; set; }
  }
}

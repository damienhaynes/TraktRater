namespace TraktRater.TraktAPI.DataModels
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktOAuthError
  {
    [DataMember( Name = "error" )]
    public string Error { get; set; }

    [DataMember( Name = "error_description" )]
    public string ErrorDescription { get; set; }
  }
}

namespace TraktRater.TraktAPI.DataStructures
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktClientId
  {
    [DataMember( Name = "client_id" )]
    public string ClientId { get; set; }
  }
}

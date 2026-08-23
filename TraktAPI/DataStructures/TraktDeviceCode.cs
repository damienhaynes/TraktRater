namespace TraktRater.TraktAPI.DataStructures
{
  using System.Runtime.Serialization;

  [DataContract]
  public class TraktDeviceCode
  {
    [DataMember( Name = "device_code" )]
    public string DeviceCode { get; set; }

    [DataMember( Name = "user_code" )]
    public string UserCode { get; set; }

    [DataMember( Name = "verification_url" )]
    public string VerificationUrl { get; set; }

    [DataMember( Name = "expires_in" )]
    public ushort ExpiresIn { get; set; }

    [DataMember( Name = "interval" )]
    public ushort Interval { get; set; }
  }
}

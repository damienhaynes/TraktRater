namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktOAuthLogin
    {
        [DataMember(Name = "code", EmitDefaultValue=false)]
        public string Code { get; set; }

        [DataMember(Name = "refresh_token", EmitDefaultValue = false)]
        public string RefreshToken { get; set; }

        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }

        [DataMember(Name = "client_secret")]
        public string ClientSecret { get; set; }

        [DataMember(Name = "redirect_uri")]
        public string RedirectUri { get; set; }

        [DataMember(Name = "grant_type")]
        public string GrantType { get; set; }
    }
}

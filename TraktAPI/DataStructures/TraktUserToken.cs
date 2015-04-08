namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktUserToken
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}

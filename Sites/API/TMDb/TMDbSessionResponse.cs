namespace TraktRater.Sites.API.TMDb
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TMDbSessionResponse
    {
        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }
}

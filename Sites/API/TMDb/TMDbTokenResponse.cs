using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.Sites.API.TMDb
{
    [DataContract]
    public class TMDbTokenResponse
    {
        [DataMember(Name = "expires_at")]
        public string ExpiresAt { get; set; }

        [DataMember(Name = "request_token")]
        public string RequestToken { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }
}

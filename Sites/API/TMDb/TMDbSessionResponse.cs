using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.Sites.API.TMDb
{
    [DataContract]
    public class TMDbSessionResponse
    {
        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }
}

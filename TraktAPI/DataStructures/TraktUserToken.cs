using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktUserToken
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}

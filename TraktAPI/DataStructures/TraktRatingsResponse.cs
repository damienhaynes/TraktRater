using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktRatingsResponse : TraktResponse
    {
        [DataMember(Name = "rated")]
        public int Rated { get; set; }

        [DataMember(Name = "unrated")]
        public int UnRated { get; set; }

        [DataMember(Name = "skipped")]
        public int Skipped { get; set; }
    }
}

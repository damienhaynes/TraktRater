using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktId
    {
        [DataMember(Name = "trakt")]
        public int? Id { get; set; }

        [DataMember(Name = "slug")]
        public string TraktSlug { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktEpisodeEx
    {
        [DataMember(Name = "number")]
        public int Number { get; set; }
    }
}

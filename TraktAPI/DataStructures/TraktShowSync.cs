using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktShowSync
    {
        [DataMember(Name = "shows")]
        public List<TraktShow> Shows { get; set; }
    }
}

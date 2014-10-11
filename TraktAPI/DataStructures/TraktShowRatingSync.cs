using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    public class TraktShowRatingSync
    {
        [DataMember(Name = "shows")]
        public List<TraktShowRating> shows { get; set; }
    }
}

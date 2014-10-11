using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    public class TraktMovieRatingSync
    {
        [DataMember(Name = "movies")]
        public List<TraktMovieRating> movies { get; set; }
    }
}

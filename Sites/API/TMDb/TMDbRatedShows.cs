using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.Sites.API.TMDb
{
    [DataContract]
    public class TMDbRatedShows : TMDbPage
    {
        [DataMember(Name = "results")]
        public List<TMDbShow> Shows { get; set; }
    }
}

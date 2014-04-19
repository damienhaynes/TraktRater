using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.Sites.API.TMDb
{
    [DataContract]
    public class TMDbRatedMovies : TMDbPage
    {
        [DataMember(Name = "results")]
        public List<TMDbRatedMovie> Movies { get; set; }
    }
}

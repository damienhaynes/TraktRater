using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.Sites.API.TMDb
{
    [DataContract]
    public class TMDbWatchlistMovies : TMDbPage
    {
        [DataMember(Name = "results")]
        public List<TMDbMovie> Movies { get; set; }
    }
}

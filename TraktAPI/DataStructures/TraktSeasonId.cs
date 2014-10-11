using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktSeasonId
    {
        [DataMember(Name = "tmdb")]
        public int TmdbId { get; set; }

        [DataMember(Name = "tvdb")]
        public int TvdbId { get; set; }

        [DataMember(Name = "tvrage")]
        public int? TvRageId { get; set; }
    }
}

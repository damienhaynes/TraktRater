using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktMovie
    {
        [DataMember(Name = "imdb_id")]
        public int IMDbId { get; set; }

        [DataMember(Name = "tmdb_id")]
        public int TMDbId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "year")]
        public int Year { get; set; }

        [DataMember(Name = "rating")]
        public int Rating { get; set; }
    }
}

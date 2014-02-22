using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktShowSync : TraktAuthentication
    {
        [DataMember(Name = "shows")]
        public List<Show> Showlist { get; set; }

        [DataContract]
        public class Show
        {
            [DataMember(Name = "imdb_id")]
            public string IMDbId { get; set; }

            [DataMember(Name = "tmdb_id")]
            public string TMDBID { get; set; }

            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "year")]
            public string Year { get; set; }
        }
    }
}

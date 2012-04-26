using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktEpisode
    {
        [DataMember(Name = "tvdb_id")]
        public int TVDbId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "year")]
        public int Year { get; set; }

        [DataMember(Name = "season")]
        public int Season { get; set; }

        [DataMember(Name = "episode")]
        public int Episode { get; set; }

        [DataMember(Name = "rating")]
        public int Rating { get; set; }
    }
}

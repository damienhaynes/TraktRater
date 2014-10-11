using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktEpisodeWatched : TraktEpisode
    {
        [DataMember(Name = "watched_at")]
        public string WatchedAt { get; set; }
    }
}

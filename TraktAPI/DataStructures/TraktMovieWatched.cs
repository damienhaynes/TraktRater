using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktMovieWatched : TraktMovie
    {
        [DataMember(Name = "watched_at")]
        public string WatchedAt { get; set; }
    }

    [DataContract]
    public class TraktMoviePlays
    {
        [DataMember(Name = "movie")]
        public TraktMovie Movie { get; set; }

        [DataMember(Name = "plays")]
        public int Plays { get; set; }

    }
}

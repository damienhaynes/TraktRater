using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktMovieSyncResponse : TraktResponse
    {
        [DataMember(Name = "inserted")]
        public string Inserted { get; set; }

        [DataMember(Name = "already_exist")]
        public string AlreadyExist { get; set; }

        [DataMember(Name = "skipped")]
        public string Skipped { get; set; }

        [DataMember(Name = "skipped_movies")]
        public List<TraktMovieSync.Movie> SkippedMovies { get; set; }

        [DataMember(Name = "already_exist_movies")]
        public List<TraktMovieSync.Movie> AlreadyExistMovies { get; set; }
    }
}

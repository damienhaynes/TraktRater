using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    /// <summary>
    /// Data structure for Syncing library to Trakt
    /// </summary>
    [DataContract]
    public class TraktMovieSync : TraktAuthentication
    {
        [DataMember(Name = "movies")]
        public List<Movie> MovieList { get; set; }

        [DataContract]
        public class Movie
        {
            [DataMember(Name = "imdb_id")]
            public string IMDBID { get; set; }

            [DataMember(Name = "tmdb_id")]
            public string TMDBID { get; set; }

            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "year")]
            public string Year { get; set; }

            [DataMember(Name = "last_played")]
            public long LastPlayed { get; set; }
        }
    }
}

namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktMovieWatchedSync
    {
        [DataMember(Name = "movies")]
        public List<TraktMovieWatched> Movies { get; set; }
    }
}

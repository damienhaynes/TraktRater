namespace TraktRater.Sites.API.TMDb
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TMDbWatchlistMovies : TMDbPage
    {
        [DataMember(Name = "results")]
        public List<TMDbMovie> Movies { get; set; }
    }
}

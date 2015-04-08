namespace TraktRater.Sites.API.TMDb
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TMDbRatedMovies : TMDbPage
    {
        [DataMember(Name = "results")]
        public List<TMDbRatedMovie> Movies { get; set; }
    }
}

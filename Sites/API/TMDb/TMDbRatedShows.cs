namespace TraktRater.Sites.API.TMDb
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TMDbRatedShows : TMDbPage
    {
        [DataMember(Name = "results")]
        public List<TMDbRatedShow> Shows { get; set; }
    }
}

namespace TraktRater.Sites.API.TMDb
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TMDbWatchlistShows : TMDbPage
    {
        [DataMember(Name = "results")]
        public List<TMDbShow> Shows { get; set; }
    }
}

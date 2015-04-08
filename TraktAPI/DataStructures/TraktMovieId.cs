namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktMovieId : TraktId
    {
        [DataMember(Name = "imdb")]
        public string ImdbId { get; set; }

        [DataMember(Name = "tmdb")]
        public int? TmdbId { get; set; }
    }
}

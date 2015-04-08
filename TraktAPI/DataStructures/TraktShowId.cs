namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktShowId : TraktId
    {
        [DataMember(Name = "imdb")]
        public string ImdbId { get; set; }

        [DataMember(Name = "tmdb")]
        public int? TmdbId { get; set; }

        [DataMember(Name = "tvdb")]
        public int? TvdbId { get; set; }

        [DataMember(Name = "tvrage")]
        public int? TvRageId { get; set; }
    }
}

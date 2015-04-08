namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktSeasonId
    {
        [DataMember(Name = "tmdb")]
        public int TmdbId { get; set; }

        [DataMember(Name = "tvdb")]
        public int TvdbId { get; set; }

        [DataMember(Name = "tvrage")]
        public int? TvRageId { get; set; }
    }
}

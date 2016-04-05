using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktPersonId : TraktId
    {
        [DataMember(Name = "imdb")]
        public string ImdbId { get; set; }

        [DataMember(Name = "tmdb")]
        public int? TmdbId { get; set; }

        [DataMember(Name = "tvrage")]
        public int? TvRageId { get; set; }
    }
}

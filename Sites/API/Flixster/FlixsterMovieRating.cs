namespace TraktRater.Sites.API.Flixster
{
    using System.Runtime.Serialization;

    [DataContract]
    public class FlixsterMovieRating
    {
        [DataMember(Name = "user")]
        public FlixsterUser User { get; set; }

        [DataMember(Name = "movie")]
        public FlixsterMovie Movie { get; set; }

        [DataMember(Name = "movieId")]
        public string MovieId { get; set; }

        [DataMember(Name = "movieUrl")]
        public string MovieUrl { get; set; }

        [DataMember(Name = "score")]
        public string UserScore { get; set; }

        [DataMember(Name = "review")]
        public string Review { get; set; }

        [DataMember(Name = "lastUpdated")]
        public string LastUpdated { get; set; }

        [DataMember(Name = "ratingSource")]
        public string RatingSource { get; set; }

        [DataMember(Name = "fbshare")]
        public bool FacebookShare { get; set; }
    }
}

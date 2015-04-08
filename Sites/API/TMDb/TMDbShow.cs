namespace TraktRater.Sites.API.TMDb
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TMDbShow
    {
        [DataMember(Name = "backdrop_path")]
        public string BackdropPath { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "original_name")]
        public string OriginalTitle { get; set; }

        [DataMember(Name = "poster_path")]
        public string PosterPath { get; set; }

        [DataMember(Name = "popularity")]
        public double Popularity { get; set; }

        [DataMember(Name = "first_air_date")]
        public string ReleaseDate { get; set; }

        [DataMember(Name = "name")]
        public string Title { get; set; }

        [DataMember(Name = "vote_average")]
        public double VoteAverage { get; set; }

        [DataMember(Name = "vote_count")]
        public int VoteCount { get; set; }
    }

    [DataContract]
    public class TMDbRatedShow : TMDbShow
    {
        [DataMember(Name = "rating")]
        public double Rating { get; set; }
    }
}

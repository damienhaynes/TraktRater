namespace TraktRater.Sites.API.Flixster
{
    using System.Runtime.Serialization;

    [DataContract]
    public class FlixsterMovie
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "year")]
        public string Year { get; set; }

        [DataMember(Name = "synopsis")]
        public string Synopsis { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "vanity")]
        public string Vanity { get; set; }

        [DataMember(Name = "movietype")]
        public string Movietype { get; set; }

        [DataMember(Name = "dvdReleaseDate")]
        public string DvdReleaseDate { get; set; }

        [DataMember(Name = "tomatometer")]
        public int TomatoMeter { get; set; }

        [DataMember(Name = "audienceScore")]
        public int AudienceScore { get; set; }

        [DataMember(Name = "mpaa")]
        public string Certification { get; set; }

        [DataMember(Name = "runningTime")]
        public string RunningTime { get; set; }
    }
}

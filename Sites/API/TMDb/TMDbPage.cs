namespace TraktRater.Sites.API.TMDb
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TMDbPage
    {
        [DataMember(Name = "page")]
        public int Page { get; set; }

        [DataMember(Name = "total_pages")]
        public int TotalPages { get; set; }

        [DataMember(Name = "total_results")]
        public int TotalResults { get; set; }
    }
}

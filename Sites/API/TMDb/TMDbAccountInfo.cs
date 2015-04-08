namespace TraktRater.Sites.API.TMDb
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TMDbAccountInfo
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "include_adult")]
        public bool IncludeAdult { get; set; }

        [DataMember(Name = "iso_3166_1")]
        public string CountryCode { get; set; }

        [DataMember(Name = "iso_639_1")]
        public string LanguageCode { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }
    }
}

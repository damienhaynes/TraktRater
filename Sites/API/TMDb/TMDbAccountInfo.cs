using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.Sites.API.TMDb
{
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

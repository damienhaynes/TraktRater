namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktId
    {
        [DataMember(Name = "trakt")]
        public int? Trakt { get; set; }

        [DataMember(Name = "slug")]
        public string Slug { get; set; }
    }
}

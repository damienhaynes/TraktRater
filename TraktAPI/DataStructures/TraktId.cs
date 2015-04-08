namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktId
    {
        [DataMember(Name = "trakt")]
        public int? Id { get; set; }

        [DataMember(Name = "slug")]
        public string TraktSlug { get; set; }
    }
}

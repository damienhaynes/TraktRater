namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktListDetail : TraktList
    {
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public string UpdatedAt { get; set; }

        [DataMember(Name = "item_count")]
        public int ItemCount { get; set; }

        [DataMember(Name = "comment_count")]
        public int Comments { get; set; }

        [DataMember(Name = "likes")]
        public int Likes { get; set; }

        [DataMember(Name = "ids")]
        public TraktId Ids { get; set; }

        [DataMember(Name = "user")]
        public TraktUser User { get; set; }
    }
}

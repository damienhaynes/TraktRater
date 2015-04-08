namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktEpisodeRatingEx : TraktEpisodeEx
    {
        [DataMember(Name = "rated_at")]
        public string RatedAt { get; set; }

        [DataMember(Name = "rating")]
        public int Rating { get; set; }
    }
}

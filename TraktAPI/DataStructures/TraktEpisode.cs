namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktEpisode
    {
        [DataMember(Name = "ids")]
        public TraktEpisodeId Ids { get; set; }
    }
}

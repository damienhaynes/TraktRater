namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktEpisodeEx
    {
        [DataMember(Name = "number")]
        public int Number { get; set; }
    }
}

namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktEpisodeSummary : TraktEpisode
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "season")]
        public uint Season { get; set; }

        [DataMember(Name = "number")]
        public uint Number { get; set; }
    }
}

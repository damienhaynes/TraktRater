namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktShow
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "year")]
        public int? Year { get; set; }

        [DataMember(Name = "ids")]
        public TraktShowId Ids { get; set; }
    }
}

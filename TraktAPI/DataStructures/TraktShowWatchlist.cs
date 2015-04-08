namespace TraktRater.TraktAPI.DataStructures
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktShowWatchlist
    {
        [DataMember(Name = "show")]
        public TraktShow Show { get; set; }

        [DataMember(Name = "inserted_at")]
        public string InsertedAt { get; set; }
    }
}

using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{    
    [DataContract]
    public class TraktSeasonEx : TraktSeason
    {
        [DataMember(Name = "ids")]
        public TraktSeasonId Ids { get; set; }
    }
}

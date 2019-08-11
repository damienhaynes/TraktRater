using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktSeasonSummary : TraktSeason
    {
        [DataMember(Name = "episodes")]
        public IEnumerable<TraktEpisodeSummary> Episodes { get; set; }
    }
}

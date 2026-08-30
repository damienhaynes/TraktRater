using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataModels
{
  [DataContract]
  public class TraktSeasonSummary : TraktSeason
  {
    [DataMember( Name = "episodes" )]
    public IEnumerable<TraktEpisodeSummary> Episodes { get; set; }
  }
}

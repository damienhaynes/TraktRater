using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktShowSummary : TraktShow
    {
        [DataMember(Name = "seasons")]
        public List<TraktSeason> Seasons { get; set; }

        [DataContract]
        public class TraktSeason
        {
            [DataMember(Name = "number")]
            public int Number { get; set; }

            [DataMember(Name = "episodes")]
            public List<TraktEpisode> Episodes { get; set; }
        }
    }
}

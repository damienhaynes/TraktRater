using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktShowWatched : TraktShow
    {
        [DataMember(Name = "seasons")]
        public List<ShowSeason> Seasons { get; set; }

        [DataContract]
        public class ShowSeason
        {
            [DataMember(Name = "season")]
            public int Season { get; set; }

            [DataMember(Name = "episodes")]
            public List<int> Episodes { get; set; }
        }
    }
}

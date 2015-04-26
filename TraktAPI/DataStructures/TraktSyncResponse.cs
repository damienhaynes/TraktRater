namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktSyncResponse
    {
        [DataMember(Name = "added")]
        public MediaItems Added { get; set; }

        [DataMember(Name = "deleted")]
        public MediaItems Deleted { get; set; }

        [DataContract]
        public class MediaItems
        {
            [DataMember(Name = "movies")]
            public int Movies { get; set; }

            [DataMember(Name = "shows")]
            public int Shows { get; set; }

            [DataMember(Name = "seasons")]
            public int Seasons { get; set; }

            [DataMember(Name = "episodes")]
            public int Episodes { get; set; }
        }

        [DataMember(Name = "not_found")]
        public NotFoundItems NotFound { get; set; }

        [DataContract]
        public class NotFoundItems
        {
            [DataMember(Name = "movies")]
            public List<TraktMovie> Movies { get; set; }

            [DataMember(Name = "shows")]
            public List<TraktShow> Shows { get; set; }

            [DataMember(Name = "episodes")]
            public List<TraktEpisode> Episodes { get; set; }
        }
    }
}

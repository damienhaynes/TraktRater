namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktShowSync
    {
        [DataMember(Name = "shows")]
        public List<TraktShow> Shows { get; set; }
    }
}

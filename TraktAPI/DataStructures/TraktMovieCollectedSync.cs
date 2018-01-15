namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TraktMovieCollectedSync
    {
        [DataMember(Name = "movies")]
        public List<TraktMovieCollection> Movies { get; set; }
    }
}

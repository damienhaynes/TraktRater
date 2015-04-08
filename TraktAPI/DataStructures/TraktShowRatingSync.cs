namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class TraktShowRatingSync
    {
        [DataMember(Name = "shows")]
        public List<TraktShowRating> shows { get; set; }
    }
}

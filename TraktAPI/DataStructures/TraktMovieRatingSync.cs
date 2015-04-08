namespace TraktRater.TraktAPI.DataStructures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class TraktMovieRatingSync
    {
        [DataMember(Name = "movies")]
        public List<TraktMovieRating> movies { get; set; }
    }
}

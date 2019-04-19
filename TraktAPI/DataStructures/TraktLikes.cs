using System.Collections.Generic;

namespace TraktRater.TraktAPI.DataStructures
{
    public class TraktLikes : TraktPagination
    {
        public IEnumerable<TraktLike> Likes { get; set; }
    }
}

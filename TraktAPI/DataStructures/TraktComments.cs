using System.Collections.Generic;

namespace TraktRater.TraktAPI.DataStructures
{
    public class TraktComments : TraktPagination
    {
        public IEnumerable<TraktCommentItem> Comments { get; set; }
    }
}

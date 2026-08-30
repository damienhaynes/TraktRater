using System.Collections.Generic;

namespace TraktRater.TraktAPI.DataModels
{
  public class TraktComments : TraktPagination
  {
    public IEnumerable<TraktCommentItem> Comments { get; set; }
  }
}

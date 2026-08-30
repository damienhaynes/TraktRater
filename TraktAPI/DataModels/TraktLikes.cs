using System.Collections.Generic;

namespace TraktRater.TraktAPI.DataModels
{
  public class TraktLikes : TraktPagination
  {
    public IEnumerable<TraktLike> Likes { get; set; }
  }
}

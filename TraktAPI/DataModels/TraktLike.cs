using System;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataModels
{
  [DataContract]
  public class TraktLike : IEquatable<TraktLike>
  {
    [DataMember( Name = "liked_at" )]
    public string LikedAt { get; set; }

    [DataMember( Name = "type" )]
    public string Type { get; set; }

    [DataMember( Name = "comment_type", EmitDefaultValue = false )]
    public string CommentType { get; set; }

    [DataMember( Name = "list", EmitDefaultValue = false )]
    public TraktListDetail List { get; set; }

    [DataMember( Name = "comment", EmitDefaultValue = false )]
    public TraktComment Comment { get; set; }

    [DataMember( Name = "show", EmitDefaultValue = false )]
    public TraktShow Show { get; set; }

    [DataMember( Name = "movie", EmitDefaultValue = false )]
    public TraktMovie Movie { get; set; }

    [DataMember( Name = "episode", EmitDefaultValue = false )]
    public TraktEpisodeLike Episode { get; set; }

    [DataMember( Name = "season", EmitDefaultValue = false )]
    public TraktSeason Season { get; set; }

    #region IEquatable
    public bool Equals( TraktLike other )
    {
      if ( other == null || ( other.Comment == null && other.Type == "comment" ) || ( other.List == null && other.Type == "list" ) )
        return false;

      if ( this.Type == "list" )
      {
        if ( this.List.Ids == null || other.List.Ids == null )
          return false;

        return ( this.Type == other.Type && this.List.Ids.Trakt == other.List.Ids.Trakt );
      }
      else
      {
        return ( this.Type == other.Type && this.Comment.Id == other.Comment.Id );
      }
    }

    public override int GetHashCode()
    {
      if ( this.Type == "list" )
      {
        return ( this.List.Ids.Trakt.ToString() + "_" + this.Type ).GetHashCode();
      }
      else
      {
        return ( this.Comment.Id.ToString() + "_" + this.Type ).GetHashCode();
      }
    }
    #endregion
  }
}

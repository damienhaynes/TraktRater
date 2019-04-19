using System;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktLike : IEquatable<TraktLike>
    {
        [DataMember(Name = "liked_at")]
        public string LikedAt { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "list")]
        public TraktListDetail List { get; set; }

        [DataMember(Name = "comment")]
        public TraktComment Comment { get; set; }

        #region IEquatable
        public bool Equals(TraktLike other)
        {
            if (other == null || (other.Comment == null && other.Type == "comment") || (other.List == null && other.Type == "list"))
                return false;

            if (this.Type == "list")
            {
                if (this.List.Ids == null || other.List.Ids == null)
                    return false;

                return (this.Type == other.Type && this.List.Ids.Trakt == other.List.Ids.Trakt);
            }
            else
            {
                return (this.Type == other.Type && this.Comment.Id == other.Comment.Id);
            }
        }

        public override int GetHashCode()
        {
            if (this.Type == "list")
            {
                return (this.List.Ids.Trakt.ToString() + "_" + this.Type).GetHashCode();
            }
            else
            {
                return (this.Comment.Id.ToString() + "_" + this.Type).GetHashCode();
            }
        }
        #endregion
    }
}

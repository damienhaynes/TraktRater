using System;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktCommentItem : IEquatable<TraktCommentItem>
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "movie")]
        public TraktMovie Movie { get; set; }

        [DataMember(Name = "show")]
        public TraktShow Show { get; set; }

        [DataMember(Name = "season")]
        public TraktSeasonEx Season { get; set; }

        [DataMember(Name = "episode")]
        public TraktEpisodeSummary Episode { get; set; }

        [DataMember(Name = "list")]
        public TraktListDetail List { get; set; }

        [DataMember(Name = "comment")]
        public TraktComment Comment { get; set; }

        #region IEquatable
        public bool Equals(TraktCommentItem other)
        {
            if (other == null || other.Comment == null)
                return false;

            return (this.Comment.Id == other.Comment.Id && this.Type == other.Type);
        }

        public override int GetHashCode()
        {
            return (this.Comment.Id.ToString() + "_" + this.Type).GetHashCode();
        }
        #endregion
    }
}

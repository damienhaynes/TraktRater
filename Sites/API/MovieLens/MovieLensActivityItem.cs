using CsvHelper.Configuration;
using System.Runtime.Serialization;
using TraktRater.Extensions;

namespace TraktRater.Sites.API.MovieLens
{
    sealed class CSVActivityFileDefinitionMap : CsvClassMap<MovieLensActivityItem>
    {
        public CSVActivityFileDefinitionMap()
        {
            Map(m => m.DateTime).Name("datetime");
            Map(m => m.LoginId).Name("login_id");
            Map(m => m.ActionType).Name("action_type");
            Map(m => m.JsonLog).Name("log_json");
        }
    }

    class MovieLensActivityItem
    {
        /// <summary>
        /// Date in format yyyy-MM-dd HH:mm:ss.0
        /// </summary>
        public string DateTime { get; set; }
        public string LoginId { get; set; }
        public string ActionType { get; set; }
        public string JsonLog { get; set; }

        public ActivityDate ToRatingActivity()
        {
            var lActivity = JsonLog.FromJSON<RatingActivity>();

            return new ActivityDate()
            {
                MovieId = lActivity?.MovieId,
                Date = DateTime
            };
        }

        public ActivityDate ToUserListActivity()
        {
            var lActivity = JsonLog.FromJSON<UserListActivity>();

            return new ActivityDate()
            {
                MovieId = lActivity?.MovieId,
                Date = DateTime
            };
        }

        [DataContract]
        private class Activity
        {
            [DataMember(Name = "movieId")]
            public int MovieId { get; set; }

            [DataMember(Name = "action")]
            public string Action { get; set; }
        }

        [DataContract]
        private class RatingActivity : Activity
        {
            [DataMember(Name= "rating")]
            public float Rating { get; set; }

            [DataMember(Name = "pred")]
            public float Pred { get; set; }
        }

        [DataContract]
        private class UserListActivity : Activity
        {
            [DataMember(Name = "listId")]
            public int ListId { get; set; }
        }

        public class ActivityDate
        {
            public int? MovieId { get; set; }

            public string Date { get; set; }
        }
    }
}

using CsvHelper.Configuration;
using global::TraktRater.Extensions;
using global::TraktRater.Settings;
using global::TraktRater.TraktAPI.DataStructures;
using System;
using System.Runtime.Serialization;

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
        public string DateTime { get; set; }
        public string LoginId { get; set; }
        public string ActionType { get; set; }
        public string JsonLog { get; set; }

        public MovieRatingActivity ToRatingActivity()
        {
            var lActivity = JsonLog.FromJSON<RatingActivity>();

            return new MovieRatingActivity()
            {
                MovieId = lActivity?.MovieId,
                Date = DateTime
            };
        }

        [DataContract]
        private class RatingActivity
        {
            [DataMember(Name="movieId")]
            public int MovieId { get; set; }

            [DataMember(Name="action")]
            public string Action { get; set; }

            [DataMember(Name= "rating")]
            public float Rating { get; set; }

            [DataMember(Name = "pred")]
            public float Pred { get; set; }
        }
        public class MovieRatingActivity
        {
            public int? MovieId { get; set; }

            public string Date { get; set; }
        }
    }
}

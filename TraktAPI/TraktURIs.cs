using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraktRater.TraktAPI
{
    /// <summary>
    /// List of URIs for the Trakt API
    /// </summary>
    public static class TraktURIs
    {
        const string apiKey = "5a3cf09bdce2e48c78f94f11f41b68ba";

        public const string RateEpisodes = @"http://api.trakt.tv/rate/episodes/" + apiKey;
        public const string RateMovies = @"http://api.trakt.tv/rate/movies/" + apiKey;
        public const string RateShows = @"http://api.trakt.tv/rate/shows/" + apiKey;
        public const string TestAccount = @"http://api.trakt.tv/account/test/" + apiKey;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraktRater.Web;
using TraktRater.Extensions;
using TraktRater.Sites.API.TMDb;

namespace TraktRater.Sites.API.TMDb
{
    /// <summary>
    /// Object that communicates with the TMDb API
    /// </summary>
    public static class TMDbAPI
    {
        /// <summary>
        /// This method is used to generate a valid request token for user based authentication. 
        /// A request token is required in order to request a session id.
        /// </summary>
        public static TMDbTokenResponse RequestToken()
        {
            string response = TraktWeb.TransmitExtended(TMDbURIs.RequestToken);
            return response.FromJSON<TMDbTokenResponse>();
        }

        public static TMDbSessionResponse RequestSessionId(string requestToken)
        {
            string response = TraktWeb.TransmitExtended(string.Format(TMDbURIs.RequestSessionId, requestToken));
            return response.FromJSON<TMDbSessionResponse>();
        }

        public static TMDbAccountInfo GetAccountId(string sessionId)
        {
            string response = TraktWeb.TransmitExtended(string.Format(TMDbURIs.AccountInfo, sessionId));
            return response.FromJSON<TMDbAccountInfo>();
        }

        public static TMDbRatedMovies GetRatedMovies(string accountId, string sessionId, int page)
        {
            string response = TraktWeb.TransmitExtended(string.Format(TMDbURIs.UserRatings, accountId, sessionId, page.ToString()));
            return response.FromJSON<TMDbRatedMovies>();
        }
    }
}

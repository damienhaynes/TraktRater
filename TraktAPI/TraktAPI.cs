using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TraktRater;
using TraktRater.Extensions;
using TraktRater.TraktAPI;
using TraktRater.TraktAPI.DataStructures;
using TraktRater.Web;

namespace TraktRater.TraktAPI
{
    /// <summary>
    /// List of Sync Modes
    /// </summary>
    public enum TraktSyncModes
    {
        library,
        seen,
        unlibrary,
        unseen,
        watchlist,
        unwatchlist
    }

    /// <summary>
    /// Object that communicates with the Trakt API
    /// </summary>
    public static class TraktAPI
    {
        public static string Username { get; set; }
        public static string Password { get; set; }

        /// <summary>
        /// Tests account details can login to trakt.tv
        /// </summary>
        /// <param name="data">Object containing username/password</param>
        /// <returns>The response from trakt</returns>
        public static TraktResponse TestAccount(TraktAuthentication data)
        {
            string response = TraktWeb.Transmit(TraktURIs.TestAccount, data.ToJSON());
            return response.FromJSON<TraktResponse>();
        }

        /// <summary>
        /// Rates a list of episodes on trakt
        /// </summary>
        /// <param name="data">The object containing the list of episodes to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktRatingsResponse RateEpisodes(TraktEpisodes data)
        {
            // check that we have everything we need
            if (data == null || data.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.Transmit(TraktURIs.RateEpisodes, data.ToJSON());
      
            // return success or failure
            return response.FromJSON<TraktRatingsResponse>();
        }

        /// <summary>
        /// Rates a list of shows on trakt
        /// </summary>
        /// <param name="data">The object containing the list of shows to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktRatingsResponse RateShows(TraktShows data)
        {
            // check that we have everything we need
            if (data == null || data.Shows.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.Transmit(TraktURIs.RateShows, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktRatingsResponse>();
        }
        
        /// <summary>
        /// Rates a list of movies on trakt
        /// </summary>
        /// <param name="data">The object containing the list of movies to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktRatingsResponse RateMovies(TraktMovies data)
        {
            // check that we have everything we need
            if (data == null || data.Movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.Transmit(TraktURIs.RateMovies, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktRatingsResponse>();
        }

        public static TraktShowSummary GetShowSummary(string slug)
        {
            // check that we have everything we need
            if (string.IsNullOrEmpty(slug))
                return null;
            
            string fileCache = string.Format(TraktCache.cShowInfoFileCache, slug);
            string response = TraktCache.GetFromCache(fileCache, 1);
            if (string.IsNullOrEmpty(response))
            {
                // serialize data to JSON and send to server
                response = TraktWeb.Transmit(string.Format(TraktURIs.ShowSummary, slug), string.Empty);
                TraktCache.CacheResponse(response, fileCache);
                if (response.FromJSON<TraktShowSummary>() == null)
                {
                    TraktCache.DeleteFromCache(fileCache);
                    return null;
                }
            }
            // return success or failure
            return response.FromJSON<TraktShowSummary>();
        }

        /// <summary>
        /// Sends movie sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <param name="mode">The sync mode to use</param>
        /// <returns>The response from trakt</returns>
        public static TraktMovieSyncResponse SyncMovieLibrary(TraktMovieSync syncData, TraktSyncModes mode)
        {
            // check that we have everything we need
            if (syncData == null || syncData.MovieList.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.Transmit(string.Format(TraktURIs.SyncMovieLibrary, mode.ToString()), syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktMovieSyncResponse>();
        }

        /// <summary>
        /// Sends episode sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <param name="mode">The sync mode to use</param>
        public static TraktResponse SyncEpisodeLibrary(TraktEpisodeSync syncData, TraktSyncModes mode)
        {
            // check that we have everything we need
            if (syncData == null || string.IsNullOrEmpty(syncData.SeriesID))
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.Transmit(string.Format(TraktURIs.SyncEpisodeLibrary, mode.ToString()), syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktResponse>();
        }

        public static TraktResponse SyncShowLibrary(TraktShowSync syncData, TraktSyncModes mode)
        {
            // check that we have everything we need
            if (syncData == null)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.Transmit(string.Format(TraktURIs.SyncShowLibrary, mode.ToString()), syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktResponse>();
        }

        /// <summary>
        /// Returns the users Rated Movies
        /// </summary>
        /// <param name="user">username of person</param>
        public static IEnumerable<TraktUserMovieRating> GetUserRatedMovies(string user)
        {
            string ratedMovies = TraktWeb.Transmit(string.Format(TraktURIs.UserRatedMoviesList, user), GetUserAuthentication());

            // if we timeout we will return an error response
            TraktResponse response = ratedMovies.FromJSON<TraktResponse>();
            if (response == null || response.Error != null) return null;

            return ratedMovies.FromJSONArray<TraktUserMovieRating>();
        }

        /// <summary>
        /// Returns the users Rated Shows
        /// </summary>
        /// <param name="user">username of person</param>
        public static IEnumerable<TraktUserShowRating> GetUserRatedShows(string user)
        {
            string ratedShows = TraktWeb.Transmit(string.Format(TraktURIs.UserRatedShowsList, user), GetUserAuthentication());

            // if we timeout we will return an error response
            TraktResponse response = ratedShows.FromJSON<TraktResponse>();
            if (response == null || response.Error != null) return null;

            return ratedShows.FromJSONArray<TraktUserShowRating>();
        }

        /// <summary>
        /// Returns the users Rated Episodes
        /// </summary>
        /// <param name="user">username of person</param>
        public static IEnumerable<TraktUserEpisodeRating> GetUserRatedEpisodes(string user)
        {
            string ratedEpisodes = TraktWeb.Transmit(string.Format(TraktURIs.UserRatedEpisodesList, user), GetUserAuthentication());

            // if we timeout we will return an error response
            TraktResponse response = ratedEpisodes.FromJSON<TraktResponse>();
            if (response == null || response.Error != null) return null;

            return ratedEpisodes.FromJSONArray<TraktUserEpisodeRating>();
        }

        /// <summary>
        /// Gets a User Authentication object
        /// </summary>       
        /// <returns>The User Authentication json string</returns>
        private static string GetUserAuthentication()
        {
            return new TraktAuthentication { Username = TraktAPI.Username, Password = TraktAPI.Password }.ToJSON();
        }

    }
}
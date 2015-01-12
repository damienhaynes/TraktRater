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
    /// Object that communicates with the Trakt API
    /// </summary>
    public class TraktAPI
    {
        const string ApplicationId = "4feebb4e3791029816a401952c09fa5b446ed4a81b01d600031e422f0d3ae86d";

        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string UserAgent { get; set; }

        /// <summary>
        /// Login to trakt and to request a user token for all subsequent requests
        /// </summary>
        /// <returns></returns>
        public static TraktUserToken GetUserToken()
        {
            // set our required headers now
            TraktWeb.CustomRequestHeaders.Clear();

            TraktWeb.CustomRequestHeaders.Add("trakt-api-key", ApplicationId);
            TraktWeb.CustomRequestHeaders.Add("trakt-api-version", "2");
            TraktWeb.CustomRequestHeaders.Add("trakt-user-login", Username);

            string response = TraktWeb.PostToTrakt(TraktURIs.Login, GetUserLogin(), false);
            var loginResponse = response.FromJSON<TraktUserToken>();
            
            if (loginResponse == null)
                return loginResponse;

            // add the token for authenticated methods
            TraktWeb.CustomRequestHeaders.Add("trakt-user-token", loginResponse.Token);

            return loginResponse;
        }

        /// <summary>
        /// Gets a User Login object
        /// </summary>       
        /// <returns>The User Login json string</returns>
        private static string GetUserLogin()
        {
            return new TraktLogin { Login = TraktAPI.Username, Password = TraktAPI.Password }.ToJSON();
        }

        #region Sync to Trakt

        #region Watchlist

        /// <summary>
        /// Sends movie sync data to Trakt Watchlist
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse SyncMovieWatchlist(TraktMovieSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Movies == null || syncData.Movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlist, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends show sync data to Trakt Watchlist
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse SyncShowWatchlist(TraktShowSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Shows == null || syncData.Shows.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlist, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends episode sync data to Trakt Watchlist
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse SyncEpisodeWatchlistEx(TraktEpisodeSyncEx syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Shows == null || syncData.Shows.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlist, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends episode sync data to Trakt Watchlist
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse SyncEpisodeWatchlist(TraktEpisodeSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Episodes == null || syncData.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlist, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Watched

        /// <summary>
        /// Sends episode watched sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        public static TraktSyncResponse SyncEpisodesWatchedEx(TraktEpisodeWatchedSyncEx syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.shows == null || syncData.shows.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatched, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends episode watched sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        public static TraktSyncResponse SyncEpisodesWatched(TraktEpisodeWatchedSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Episodes == null || syncData.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatched, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends movies watched sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        public static TraktSyncResponse SyncMoviesWatched(TraktMovieWatchedSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Movies == null || syncData.Movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatched, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Rated

        /// <summary>
        /// Rates a list of movies on trakt
        /// </summary>
        /// <param name="data">The object containing the list of movies to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse SyncMoviesRated(TraktMovieRatingSync data)
        {
            // check that we have everything we need
            if (data == null || data.movies == null || data.movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncRatings, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Rates a list of shows on trakt
        /// </summary>
        /// <param name="data">The object containing the list of shows to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse SyncShowsRated(TraktShowRatingSync data)
        {
            // check that we have everything we need
            if (data == null || data.shows == null || data.shows.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncRatings, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }
        
        /// <summary>
        /// Rates a list of episodes on trakt
        /// </summary>
        /// <param name="data">The object containing the list of episodes to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse SyncEpisodesRatedEx(TraktEpisodeRatingSyncEx data)
        {
            // check that we have everything we need
            if (data == null || data.Shows == null || data.Shows.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncRatings, data.ToJSON());
      
            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Rates a list of episodes on trakt
        /// </summary>
        /// <param name="data">The object containing the list of episodes to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse SyncEpisodesRated(TraktEpisodeRatingSync data)
        {
            // check that we have everything we need
            if (data == null || data.Episodes == null || data.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncRatings, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #endregion

        #region Get Current User Data

        #region Ratings

        /// <summary>
        /// Returns the current users Rated Movies
        /// </summary>
        /// <param name="user">username of person</param>
        public static IEnumerable<TraktUserMovieRating> GetRatedMovies()
        {
            string ratedMovies = TraktWeb.GetFromTrakt(TraktURIs.RatedMoviesList);
            return ratedMovies.FromJSONArray<TraktUserMovieRating>();
        }

        /// <summary>
        /// Returns the current users Rated Shows
        /// </summary>
        /// <param name="user">username of person</param>
        public static IEnumerable<TraktUserShowRating> GetRatedShows()
        {
            string ratedShows = TraktWeb.GetFromTrakt(TraktURIs.RatedShowsList);
            return ratedShows.FromJSONArray<TraktUserShowRating>();
        }

        /// <summary>
        /// Returns the current users Rated Episodes
        /// </summary>
        /// <param name="user">username of person</param>
        public static IEnumerable<TraktUserEpisodeRating> GetRatedEpisodes()
        {
            string ratedEpisodes = TraktWeb.GetFromTrakt(TraktURIs.RatedEpisodesList);
            return ratedEpisodes.FromJSONArray<TraktUserEpisodeRating>();
        }

        #endregion

        #region Watched

        /// <summary>
        /// Returns the current users watched movies and play counts
        /// </summary>
        public static IEnumerable<TraktMoviePlays> GetWatchedMovies()
        {
            string watchedMovies = TraktWeb.GetFromTrakt(TraktURIs.WatchedMoviesList);
            return watchedMovies.FromJSONArray<TraktMoviePlays>();
        }

        /// <summary>
        /// Returns the current users watched episodes and play counts
        /// </summary>
        public static IEnumerable<TraktShowPlays> GetWatchedEpisodes()
        {
            string watchedShows = TraktWeb.GetFromTrakt(TraktURIs.WatchedEpisodesList);
            return watchedShows.FromJSONArray<TraktShowPlays>();
        }

        #endregion

        #region Watchlist

        /// <summary>
        /// Returns the current users watchlist movies
        /// </summary>
        public static IEnumerable<TraktMovieWatchlist> GetWatchlistMovies()
        {
            string watchedMovies = TraktWeb.GetFromTrakt(TraktURIs.WatchlistMoviesList);
            return watchedMovies.FromJSONArray<TraktMovieWatchlist>();
        }

        /// <summary>
        /// Returns the current users watchlist shows
        /// </summary>
        public static IEnumerable<TraktShowWatchlist> GetWatchlistShows()
        {
            string watchedMovies = TraktWeb.GetFromTrakt(TraktURIs.WatchlistShowsList);
            return watchedMovies.FromJSONArray<TraktShowWatchlist>();
        }

        /// <summary>
        /// Returns the current users watchlist episodes
        /// </summary>
        public static IEnumerable<TraktEpisodeWatchlist> GetWatchlistEpisodes()
        {
            string watchedMovies = TraktWeb.GetFromTrakt(TraktURIs.WatchlistEpisodesList);
            return watchedMovies.FromJSONArray<TraktEpisodeWatchlist>();
        }

        #endregion

        #endregion

        #region Summary Data

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
                response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.ShowSummary, slug));
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

        #endregion

    }
}
namespace TraktRater.TraktAPI
{
    using global::TraktRater.Extensions;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Object that communicates with the Trakt API
    /// </summary>
    public static partial class TraktAPI
    {
        private enum TraktDeviceAuthStatus
        {
          Pending,
          Authorised,
          Denied,
          Expired,
          Invalid,
          AlreadyUsed,
          SlowDown
        }

        public static TraktDeviceCode GenerateDeviceCode()
        {
          TraktWeb.CustomRequestHeaders.Clear();

          string lResponse = TraktWeb.PostToTrakt(
            address: TraktURIs.DeviceCode,
            postData: new TraktClientId { ClientId = AppClientId }.ToJSON() );

          if ( string.IsNullOrEmpty( lResponse ) )
            return null;

          return lResponse.FromJSON<TraktDeviceCode>();
        }

        public static async Task<TraktOAuthToken> WaitForDeviceAuthorisation( TraktDeviceCode aDeviceCode )
        {
          if ( aDeviceCode == null || string.IsNullOrEmpty( aDeviceCode.DeviceCode ) )
            return null;

          var lExpiryTime = DateTime.UtcNow.AddSeconds( aDeviceCode.ExpiresIn );

          while ( DateTime.UtcNow < lExpiryTime )
          {
            await Task.Delay( aDeviceCode.Interval * 1_000 );

            TraktDeviceAuthStatus lStatus = PollForDeviceToken( aDeviceCode.DeviceCode, out TraktOAuthToken lToken );

            switch ( lStatus )
            {
              case TraktDeviceAuthStatus.Authorised:
                // add authentication headers for future requests
                SetAuthenticationHeaders( lToken.AccessToken );
                return lToken;

              case TraktDeviceAuthStatus.Pending:
                continue;

              case TraktDeviceAuthStatus.Denied:
              case TraktDeviceAuthStatus.Expired:
              case TraktDeviceAuthStatus.Invalid:
              case TraktDeviceAuthStatus.AlreadyUsed:
                return null;
            }
          }

          return null;
        }

        private static TraktDeviceAuthStatus PollForDeviceToken( string aDeviceCode, out TraktOAuthToken aToken )
        {
          aToken = new TraktOAuthToken();

          var lDeviceToken = new TraktDeviceToken
          {
            Code = aDeviceCode,
            ClientId = AppClientId,
            ClientSecret = AppClientSecret
          };

          string lTokenResponse = TraktWeb.PostToTraktWithStatus(
            aAddress: TraktURIs.DeviceToken,
            aPostData: lDeviceToken.ToJSON(),
            aStatusCode: out HttpStatusCode lStatusCode );

          switch ( lStatusCode )
          {
            // 200 (Success)
            case HttpStatusCode.OK:
              aToken = lTokenResponse.FromJSON<TraktOAuthToken>();
              return TraktDeviceAuthStatus.Authorised;

            // 400 (Pending)
            case HttpStatusCode.BadRequest:
              return TraktDeviceAuthStatus.Pending;

            // 409 (Already Used)
            case HttpStatusCode.Conflict:
              return TraktDeviceAuthStatus.AlreadyUsed;

            // 410 (Expired)
            case HttpStatusCode.Gone:
              return TraktDeviceAuthStatus.Expired;

            // 418 (Denied)
            case (HttpStatusCode)418:
              return TraktDeviceAuthStatus.Denied;

            // 429 (Slow Down) Handled in PostToTraktWithStatus

            default:
              return TraktDeviceAuthStatus.Invalid;
          }
        }

        public static TraktOAuthToken RefreshToken(string aRefreshToken)
        {
            TraktWeb.CustomRequestHeaders.Clear();

            var lRefreshTokenData = new TraktRefreshToken
            {
              RefreshToken = aRefreshToken,
              ClientId = AppClientId,
              ClientSecret = AppClientSecret,
              RedirectUrl = AppirectUri,
              GrantType = "refresh_token"
            };

            string lResponse = TraktWeb.PostToTrakt(TraktURIs.LoginOAuth, lRefreshTokenData.ToJSON());
            if ( lResponse == null )
              return null;

            var lLoginResponse = lResponse.FromJSON<TraktOAuthToken>();

            if (lLoginResponse == null || lLoginResponse.AccessToken == null)
                return null;

            // add authentication headers for future requests
            SetAuthenticationHeaders( lLoginResponse.AccessToken );

            return lLoginResponse;
        }

        public static void SetAuthenticationHeaders( string aAccessToken )
        {
          TraktWeb.CustomRequestHeaders.Clear();

          TraktWeb.CustomRequestHeaders.Add( "Authorization", $"Bearer {aAccessToken}" );
          TraktWeb.CustomRequestHeaders.Add( "trakt-api-version", "2" );
          TraktWeb.CustomRequestHeaders.Add( "trakt-api-key", AppClientId );
        }

        #region Sync to Trakt

        #region Watchlist

        /// <summary>
        /// Sends movie sync data to Trakt Watchlist
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddMoviesToWatchlist(TraktMovieSync syncData)
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
        public static TraktSyncResponse AddShowsToWatchlist(TraktShowSync syncData)
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
        public static TraktSyncResponse AddEpisodesToWatchlist(TraktEpisodeSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Episodes == null || syncData.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlist, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all episodes from watchlist from trakt
        /// </summary>
        /// <param name="syncData">list of episodes</param>
        public static TraktSyncResponse RemoveEpisodesFromWatchlist(TraktEpisodeSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlistRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all shows from watchlist from trakt
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveShowsFromWatchlist(TraktShowSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlistRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all seasons from watchlist from trakt
        /// </summary>
        /// <param name="syncData">list of shows with seasons</param>
        public static TraktSyncResponse RemoveSeasonsFromWatchlist(TraktSeasonSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlistRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all movies from watchlist from trakt
        /// </summary>
        /// <param name="syncData">list of movies</param>
        public static TraktSyncResponse RemoveMoviesFromWatchlist(TraktMovieSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlistRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Watched

        /// <summary>
        /// Sends episode watched sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        public static TraktSyncResponse AddEpisodesToWatchedHistory(TraktEpisodeWatchedSync syncData)
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
        /// Removes all episodes for each show in users watched history
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveShowsFromWatchedHistory(TraktShowSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchedRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends movies watched sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        public static TraktSyncResponse AddMoviesToWatchedHistory(TraktMovieWatchedSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Movies == null || syncData.Movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatched, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes movies from users watched history
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveMoviesFromWatchedHistory(TraktMovieSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchedRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Rated

        /// <summary>
        /// Rates a list of movies on trakt
        /// </summary>
        /// <param name="data">The object containing the list of movies to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddMoviesToRatings(TraktMovieRatingSync data)
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
        public static TraktSyncResponse AddShowsToRatings(TraktShowRatingSync data)
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
        public static TraktSyncResponse AddsEpisodesToRatings(TraktEpisodeRatingSync data)
        {
            // check that we have everything we need
            if (data == null || data.Episodes == null || data.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncRatings, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all episode ratings from trakt
        /// </summary>
        /// <param name="syncData">list of episodes</param>
        public static TraktSyncResponse RemoveEpisodesFromRatings(TraktEpisodeSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncRatingsRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all show ratings from trakt
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveShowsFromRatings(TraktShowSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncRatingsRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        // <summary>
        /// Removes all season ratings from trakt
        /// </summary>
        /// <param name="syncData">list of shows with seasons</param>
        public static TraktSyncResponse RemoveSeasonsFromRatings(TraktSeasonSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncRatingsRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all movie ratings from trakt
        /// </summary>
        /// <param name="syncData">list of movies</param>
        public static TraktSyncResponse RemoveMoviesFromRatings(TraktMovieSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncRatingsRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Collection

        /// <summary>
        /// Sends movie sync data to Trakt Collection
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddMoviesToCollection(TraktMovieSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Movies == null || syncData.Movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncCollection, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all episodes for each show in users collection
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveShowsFromCollection(TraktShowSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncCollectionRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes movies from users collection
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveMoviesFromCollection(TraktMovieSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncCollectionRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Paused State

        public static bool RemovePausedState(uint id)
        {
            return TraktWeb.DeleteFromTrakt(string.Format(TraktURIs.SyncPausedRemove, id));
        }

        #endregion

        #endregion

        #region Get Current User Data

        #region Ratings

        /// <summary>
        /// Returns the current users Rated Movies
        /// </summary>
        public static IEnumerable<TraktUserMovieRating> GetRatedMovies()
        {
          const int limit = 250;
          var allRatings = new List<TraktUserMovieRating>();

          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string ratedMovies = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.RatedMovies, page, limit ),
                out WebHeaderCollection headers );

            var result = ratedMovies.FromJSONArray<TraktUserMovieRating>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allRatings.AddRange(
                result.Where( r => r.Movie?.Title != null && r.Movie.Ids != null ) );

            // Only need to get this from the first response
            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allRatings;
        }

        /// <summary>
        /// Returns the current users Rated Shows
        /// </summary>
        public static IEnumerable<TraktUserShowRating> GetRatedShows()
        {
          const int limit = 250;
          var allRatings = new List<TraktUserShowRating>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string ratedShows = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.RatedShows, page, limit ),
                out WebHeaderCollection headers );

            var result = ratedShows.FromJSONArray<TraktUserShowRating>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allRatings.AddRange(
                result.Where( r => r.Show?.Title != null && r.Show.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allRatings;
        }

        /// <summary>
        /// Returns the current users Rated Episodes
        /// </summary>
        public static IEnumerable<TraktUserEpisodeRating> GetRatedEpisodes()
        {
          const int limit = 250;
          var allRatings = new List<TraktUserEpisodeRating>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string ratedEpisodes = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.RatedEpisodes, page, limit ),
                out WebHeaderCollection headers );

            var result = ratedEpisodes.FromJSONArray<TraktUserEpisodeRating>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allRatings.AddRange(
                result.Where( r => r.Show?.Title != null && r.Show.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allRatings;
        }

        /// <summary>
        /// Returns the current users Rated Seasons
        /// </summary>
        public static IEnumerable<TraktUserSeasonRating> GetRatedSeasons()
        {
          const int limit = 250;
          var allRatings = new List<TraktUserSeasonRating>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string ratedSeasons = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.RatedSeasons, page, limit ),
                out WebHeaderCollection headers );

            var result = ratedSeasons.FromJSONArray<TraktUserSeasonRating>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allRatings.AddRange(
                result.Where( r => r.Show?.Title != null && r.Show.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allRatings;
        }

        #endregion

        #region Watched

        /// <summary>
        /// Returns the current users watched movies and play counts
        /// </summary>
        public static IEnumerable<TraktMoviePlays> GetWatchedMovies()
        {
          const int limit = 250;
          var allMovies = new List<TraktMoviePlays>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string watchedMovies = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.WatchedMovies, page, limit ),
                out WebHeaderCollection headers );

            var result = watchedMovies.FromJSONArray<TraktMoviePlays>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allMovies.AddRange(
                result.Where( r => r.Movie?.Title != null && r.Movie.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allMovies;
        }

        /// <summary>
        /// Returns the current users watched episodes and play counts
        /// </summary>
        public static IEnumerable<TraktShowPlays> GetWatchedShows()
        {
          const int limit = 250;
          var allShows = new List<TraktShowPlays>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string watchedShows = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.WatchedShows, page, limit ),
                out WebHeaderCollection headers );

            var result = watchedShows.FromJSONArray<TraktShowPlays>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allShows.AddRange(
                result.Where( r => r.Show?.Title != null && r.Show.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allShows;
        }

        #endregion

        #region Watchlist

        /// <summary>
        /// Returns the current users watchlist movies
        /// </summary>
        public static IEnumerable<TraktMovieWatchlist> GetWatchlistMovies()
        {
          const int limit = 100;
          var allMovies = new List<TraktMovieWatchlist>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string watchlistMovies = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.WatchlistMovies, page, limit ),
                out WebHeaderCollection headers );

            var result = watchlistMovies.FromJSONArray<TraktMovieWatchlist>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allMovies.AddRange(
                result.Where( r => r.Movie?.Title != null && r.Movie.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allMovies;
        }

        /// <summary>
        /// Returns the current users watchlist shows
        /// </summary>
        public static IEnumerable<TraktShowWatchlist> GetWatchlistShows()
        {
          const int limit = 100;
          var allShows = new List<TraktShowWatchlist>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string watchlistShows = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.WatchlistShows, page, limit ),
                out WebHeaderCollection headers );

            var result = watchlistShows.FromJSONArray<TraktShowWatchlist>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allShows.AddRange(
                result.Where( r => r.Show?.Title != null && r.Show.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allShows;
        }

        /// <summary>
        /// Returns the current users watchlist episodes
        /// </summary>
        public static IEnumerable<TraktEpisodeWatchlist> GetWatchlistEpisodes()
        {
          const int limit = 100;
          var allEpisodes = new List<TraktEpisodeWatchlist>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string watchlistEpisodes = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.WatchlistEpisodes, page, limit ),
                out WebHeaderCollection headers );

            var result = watchlistEpisodes.FromJSONArray<TraktEpisodeWatchlist>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allEpisodes.AddRange(
                result.Where( r => r.Show?.Title != null && r.Show.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allEpisodes;
        }

        /// <summary>
        /// Returns the current users watchlist seasons
        /// </summary>
        public static IEnumerable<TraktSeasonWatchlist> GetWatchlistSeasons()
        {
          const int limit = 100;
          var allSeasons = new List<TraktSeasonWatchlist>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string watchlistSeasons = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.WatchlistSeasons, page, limit ),
                out WebHeaderCollection headers );

            var result = watchlistSeasons.FromJSONArray<TraktSeasonWatchlist>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allSeasons.AddRange(
                result.Where( r => r.Show?.Title != null && r.Show.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allSeasons;
        }

        #endregion

        #region Collection

        /// <summary>
        /// Returns the current users collected movies
        /// </summary>
        public static IEnumerable<TraktMovieCollected> GetCollectedMovies()
        {
          const int limit = 100;
          var allMovies = new List<TraktMovieCollected>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string collectedMovies = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.CollectedMovies, page, limit ),
                out WebHeaderCollection headers );

            var result = collectedMovies.FromJSONArray<TraktMovieCollected>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allMovies.AddRange(
                result.Where( r => r.Movie?.Title != null && r.Movie.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allMovies;
        }

        /// <summary>
        /// Returns the current users collected shows
        /// </summary>
        public static IEnumerable<TraktShowCollected> GetCollectedShows()
        {
          const int limit = 100;
          var allShows = new List<TraktShowCollected>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string collectedShows = TraktWeb.GetFromTrakt(
                string.Format( TraktURIs.CollectedShows, page, limit ),
                out WebHeaderCollection headers );

            var result = collectedShows.FromJSONArray<TraktShowCollected>();

            if ( result == null )
              break;

            // Filter out anything invalid
            allShows.AddRange(
                result.Where( r => r.Show?.Title != null && r.Show.Ids != null ) );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allShows;
        }

        #endregion

        #region Custom Lists

        /// <summary>
        /// Returns all custom lists for a user
        /// </summary>
        /// <param name="username">Username of person's list</param>
        public static IEnumerable<TraktListDetail> GetCustomLists(string username = "me")
        {
            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.UserLists, username));
            return response.FromJSONArray<TraktListDetail>();
        }

        public static bool DeleteCustomList(string listId, string username ="me")
        {
            return TraktWeb.DeleteFromTrakt(string.Format(TraktURIs.UserListDelete, username, listId));
        }

        public static TraktListDetail CreateCustomList(TraktList list, string username = "me")
        {
            var response = TraktWeb.PostToTrakt(string.Format(TraktURIs.UserListAdd, username), list.ToJSON());
            return response.FromJSON<TraktListDetail>();
        }

        public static IEnumerable<TraktListItem> GetCustomListItems(
            string listId,
            string username = "me",
            string extendedInfoParams = "min" )
        {
          const int limit = 100;
          var allItems = new List<TraktListItem>();
          int pageCount = 1;

          for ( int page = 1; page <= pageCount; page++ )
          {
            string response = TraktWeb.GetFromTrakt(
                string.Format(
                    TraktURIs.UserListItems,
                    username,
                    listId,
                    extendedInfoParams,
                    page,
                    limit ),
                out WebHeaderCollection headers );

            var result = response.FromJSONArray<TraktListItem>();

            if ( result == null )
              break;

            allItems.AddRange( result );

            if ( page == 1 &&
                int.TryParse( headers[ "X-Pagination-Page-Count" ], out int parsedPageCount ) )
            {
              pageCount = parsedPageCount;
            }
          }

          return allItems;
        }

        public static TraktSyncResponse AddItemsToList(string id, TraktSyncAll items, string username = "me")
        {
            var response = TraktWeb.PostToTrakt(string.Format(TraktURIs.UserListItemsAdd, username, id), items.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Paused State

        public static IEnumerable<TraktSyncPausedMovie> GetPausedMovies()
        {
            var response = TraktWeb.GetFromTrakt(TraktURIs.SyncPausedMovies);
            return response.FromJSONArray<TraktSyncPausedMovie>();
        }

        public static IEnumerable<TraktSyncPausedEpisode> GetPausedEpisodes()
        {
            var response = TraktWeb.GetFromTrakt(TraktURIs.SyncPausedEpisodes);
            return response.FromJSONArray<TraktSyncPausedEpisode>();
        }

        #endregion

        #region Comments
        /// <summary>
        /// Get comments for user sorted by most recent
        /// </summary>
        /// <param name="username">Username of person that made comment</param>
        /// <param name="commentType">all, reviews, shouts</param>
        /// <param name="type"> all, movies, shows, seasons, episodes, lists</param>
        /// <param name="extendedInfoParams">Extended Info: min, full, images (comma separated)</param>
        public static TraktComments GetUsersComments(string username = "me", string commentType = "all", string type = "all", string extendedInfoParams = "min", int page = 1, int maxItems = 50)
        {
            var headers = new WebHeaderCollection();

            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.UserComments, username, commentType, type, extendedInfoParams, page, maxItems), out headers);
            if (response == null)
                return null;

            try
            {
                return new TraktComments
                {
                    CurrentPage = page,
                    TotalItemsPerPage = maxItems,
                    TotalPages = int.Parse(headers["X-Pagination-Page-Count"]),
                    TotalItems = int.Parse(headers["X-Pagination-Item-Count"]),
                    Comments = response.FromJSONArray<TraktCommentItem>()
                };
            }
            catch
            {
                // most likely bad header response
                return null;
            }
        }
        #endregion

        #region Likes
        /// <summary>
        /// Gets the current users liked items (comments and/or lists)
        /// </summary>
        /// <param name="type">The type of liked item: all (default), lists or comments</param>
        /// <param name="extendedInfoParams">Extended Info: min, full, images (comma separated)</param>
        /// <param name="page">Page Number</param>
        /// <param name="maxItems">Maximum number of items to request per page (this should be consistent per page request)</param>
        public static TraktLikes GetLikedItems(string type = "all", string extendedInfoParams = "min", int page = 1, int maxItems = 100)
        {
            var headers = new WebHeaderCollection();

            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.UserLikedItems, type, extendedInfoParams, page, maxItems), out headers);
            if (response == null)
                return null;

            try
            {
                return new TraktLikes
                {
                    CurrentPage = page,
                    TotalItemsPerPage = maxItems,
                    TotalPages = int.Parse(headers["X-Pagination-Page-Count"]),
                    TotalItems = int.Parse(headers["X-Pagination-Item-Count"]),
                    Likes = response.FromJSONArray<TraktLike>()
                };
            }
            catch
            {
                // most likely bad header response
                return null;
            }
        }
        #endregion

        #endregion

        #region Search Methods

        /// <summary>
        /// Gets the seasons for a show
        /// </summary>
        /// <param name="id">the id of the tv show. Trakt ID, Trakt slug, or IMDb ID Example: game-of-thrones.</param>
        public static IEnumerable<TraktSeasonSummary> GetShowSeasons(string id, string extendedParameter = "full")
        {
            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.SeasonSummary, id));
            return response.FromJSONArray<TraktSeasonSummary>();
        }

        #endregion
    }
}
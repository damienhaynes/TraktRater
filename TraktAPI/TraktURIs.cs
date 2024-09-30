    namespace TraktRater.TraktAPI
{
    /// <summary>
    /// List of URIs for the Trakt API
    /// </summary>
    public static class TraktURIs
    {
        public const string PinUrl = @"https://trakt.tv/pin/{0}";
        public const string Login = @"https://api.trakt.tv/auth/login";
        public const string LoginOAuth = @"https://api.trakt.tv/oauth/token";

        public const string RatedMovies = @"https://api.trakt.tv/sync/ratings/movies";
        public const string RatedShows = @"https://api.trakt.tv/sync/ratings/shows";
        public const string RatedEpisodes = @"https://api.trakt.tv/sync/ratings/episodes";
        public const string RatedSeasons = @"https://api.trakt.tv/sync/ratings/seasons";

        public const string WatchedMovies = @"https://api.trakt.tv/sync/watched/movies";
        public const string WatchedShows = @"https://api.trakt.tv/sync/watched/shows";

        public const string CollectedMovies = @"https://api.trakt.tv/sync/collection/movies";
        public const string CollectedShows = @"https://api.trakt.tv/sync/collection/shows";

        public const string WatchlistMovies = @"https://api.trakt.tv/sync/watchlist/movies";
        public const string WatchlistShows = @"https://api.trakt.tv/sync/watchlist/shows";
        public const string WatchlistEpisodes = @"https://api.trakt.tv/sync/watchlist/episodes";
        public const string WatchlistSeasons = @"https://api.trakt.tv/sync/watchlist/seasons";

        public const string SyncRatings = @"https://api.trakt.tv/sync/ratings";
        public const string SyncWatchlist = @"https://api.trakt.tv/sync/watchlist";
        public const string SyncWatched = @"https://api.trakt.tv/sync/history";
        public const string SyncCollection = @"https://api.trakt.tv/sync/collection";
        public const string SyncWatchedRemove = @"https://api.trakt.tv/sync/history/remove";
        public const string SyncCollectionRemove = @"https://api.trakt.tv/sync/collection/remove";
        public const string SyncRatingsRemove = @"https://api.trakt.tv/sync/ratings/remove";
        public const string SyncWatchlistRemove = @"https://api.trakt.tv/sync/watchlist/remove";

        public const string ShowSummary = @"https://api.trakt.tv/shows/{0}?extended=full";

        public const string UserLists = @"https://api.trakt.tv/users/{0}/lists";
        public const string UserListAdd = @"https://api.trakt.tv/users/{0}/lists";
        public const string UserListDelete = @"https://api.trakt.tv/users/{0}/lists/{1}";
        
        public const string UserListItems = @"https://api.trakt.tv/users/{0}/lists/{1}/items?extended={2}";
        public const string UserListItemsAdd = @"https://api.trakt.tv/users/{0}/lists/{1}/items";
        public const string UserListItemsRemove = @"https://api.trakt.tv/users/{0}/lists/{1}/items/remove";

        public const string SyncPausedMovies = @"https://api.trakt.tv/sync/playback/movies";
        public const string SyncPausedEpisodes = @"https://api.trakt.tv/sync/playback/episodes";
        public const string SyncPausedRemove = @"https://api.trakt.tv/sync/playback/{0}";

        public const string UserLikedItems = @"https://api.trakt.tv/users/likes/{0}?extended={1}&page={2}&limit={3}";
        public const string UserComments = @"https://api.trakt.tv/users/{0}/comments/{1}/{2}?extended={3}&page={4}&limit={5}&include_replies=false";

        public const string SeasonSummary = @"https://api.trakt.tv/shows/{0}/seasons?extended=episodes";
    }
}

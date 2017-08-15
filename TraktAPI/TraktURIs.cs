    namespace TraktRater.TraktAPI
{
    /// <summary>
    /// List of URIs for the Trakt API
    /// </summary>
    public static class TraktURIs
    {
        public const string PinUrl = @"http://trakt.tv/pin/{0}";
        public const string Login = @"http://api.trakt.tv/auth/login";
        public const string LoginOAuth = @"http://api.trakt.tv/oauth/token";

        public const string RatedMovies = @"http://api.trakt.tv/sync/ratings/movies";
        public const string RatedShows = @"http://api.trakt.tv/sync/ratings/shows";
        public const string RatedEpisodes = @"http://api.trakt.tv/sync/ratings/episodes";
        public const string RatedSeasons = @"http://api.trakt.tv/sync/ratings/seasons";

        public const string WatchedMovies = @"http://api.trakt.tv/sync/watched/movies";
        public const string WatchedShows = @"http://api.trakt.tv/sync/watched/shows";

        public const string CollectedMovies = @"http://api.trakt.tv/sync/collection/movies";
        public const string CollectedShows = @"http://api.trakt.tv/sync/collection/shows";

        public const string WatchlistMovies = @"http://api.trakt.tv/sync/watchlist/movies";
        public const string WatchlistShows = @"http://api.trakt.tv/sync/watchlist/shows";
        public const string WatchlistEpisodes = @"http://api.trakt.tv/sync/watchlist/episodes";
        public const string WatchlistSeasons = @"http://api.trakt.tv/sync/watchlist/seasons";

        public const string SyncRatings = @"http://api.trakt.tv/sync/ratings";
        public const string SyncWatchlist = @"http://api.trakt.tv/sync/watchlist";
        public const string SyncWatched = @"http://api.trakt.tv/sync/history";
        public const string SyncWatchedRemove = "https://api.trakt.tv/sync/history/remove";
        public const string SyncCollectionRemove = "https://api.trakt.tv/sync/collection/remove";
        public const string SyncRatingsRemove = "https://api.trakt.tv/sync/ratings/remove";
        public const string SyncWatchlistRemove = "https://api.trakt.tv/sync/watchlist/remove";

        public const string ShowSummary = @"http://api.trakt.tv/shows/{0}?extended=full";

        public const string UserLists = "https://api.trakt.tv/users/{0}/lists";
        public const string UserListAdd = "https://api.trakt.tv/users/{0}/lists";
        public const string UserListDelete = "https://api.trakt.tv/users/{0}/lists/{1}";
        
        public const string UserListItems = "https://api.trakt.tv/users/{0}/lists/{1}/items?extended={2}";
        public const string UserListItemsAdd = "https://api.trakt.tv/users/{0}/lists/{1}/items";
        public const string UserListItemsRemove = "https://api.trakt.tv/users/{0}/lists/{1}/items/remove";
    }
}

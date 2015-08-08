namespace TraktRater.TraktAPI
{
    /// <summary>
    /// List of URIs for the Trakt API
    /// </summary>
    public static class TraktURIs
    {
        public const string PinUrl = @"http://trakt.tv/pin/{0}";
        public const string Login = @"http://api-v2launch.trakt.tv/auth/login";
        public const string LoginOAuth = @"http://api-v2launch.trakt.tv/oauth/token";

        public const string RatedMovies = @"http://api-v2launch.trakt.tv/sync/ratings/movies";
        public const string RatedShows = @"http://api-v2launch.trakt.tv/sync/ratings/shows";
        public const string RatedEpisodes = @"http://api-v2launch.trakt.tv/sync/ratings/episodes";
        public const string RatedSeasons = @"http://api-v2launch.trakt.tv/sync/ratings/seasons";

        public const string WatchedMovies = @"http://api-v2launch.trakt.tv/sync/watched/movies";
        public const string WatchedShows = @"http://api-v2launch.trakt.tv/sync/watched/shows";

        public const string CollectedMovies = @"http://api-v2launch.trakt.tv/sync/collection/movies";
        public const string CollectedShows = @"http://api-v2launch.trakt.tv/sync/collection/shows";

        public const string WatchlistMovies = @"http://api-v2launch.trakt.tv/sync/watchlist/movies";
        public const string WatchlistShows = @"http://api-v2launch.trakt.tv/sync/watchlist/shows";
        public const string WatchlistEpisodes = @"http://api-v2launch.trakt.tv/sync/watchlist/episodes";
        public const string WatchlistSeasons = @"http://api-v2launch.trakt.tv/sync/watchlist/seasons";

        public const string SyncRatings = @"http://api-v2launch.trakt.tv/sync/ratings";
        public const string SyncWatchlist = @"http://api-v2launch.trakt.tv/sync/watchlist";
        public const string SyncWatched = @"http://api-v2launch.trakt.tv/sync/history";
        public const string SyncWatchedRemove = "https://api-v2launch.trakt.tv/sync/history/remove";
        public const string SyncCollectionRemove = "https://api-v2launch.trakt.tv/sync/collection/remove";
        public const string SyncRatingsRemove = "https://api-v2launch.trakt.tv/sync/ratings/remove";
        public const string SyncWatchlistRemove = "https://api-v2launch.trakt.tv/sync/watchlist/remove";

        public const string ShowSummary = @"http://api-v2launch.trakt.tv/shows/{0}?extended=full";
    }
}

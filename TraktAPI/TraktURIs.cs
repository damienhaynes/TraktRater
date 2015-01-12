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
        public const string Login = @"http://api.trakt.tv/auth/login";

        public const string RatedMoviesList = @"http://api.trakt.tv/sync/ratings/movies";
        public const string RatedShowsList = @"http://api.trakt.tv/sync/ratings/shows";
        public const string RatedEpisodesList = @"http://api.trakt.tv/sync/ratings/episodes";

        public const string WatchedMoviesList = @"http://api.trakt.tv/sync/watched/movies";
        public const string WatchedEpisodesList = @"http://api.trakt.tv/sync/watched/shows";

        public const string WatchlistMoviesList = @"http://api.trakt.tv/sync/watchlist/movies";
        public const string WatchlistShowsList = @"http://api.trakt.tv/sync/watchlist/shows";
        public const string WatchlistEpisodesList = @"http://api.trakt.tv/sync/watchlist/episodes";

        public const string SyncRatings = @"http://api.trakt.tv/sync/ratings";
        public const string SyncWatchlist = @"http://api.trakt.tv/sync/watchlist";
        public const string SyncWatched = @"http://api.trakt.tv/sync/history";

        public const string ShowSummary = @"http://api.trakt.tv/shows/{0}?extended=full";
    }
}

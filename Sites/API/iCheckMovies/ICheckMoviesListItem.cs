using System;
using System.Text.RegularExpressions;
using TraktRater.TraktAPI.DataStructures;

namespace TraktRater.Sites.API.iCheckMovies
{
    class ICheckMoviesListItem
    {
        public int Position { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int CheckedCount { get; set; }
        public int FavouriteCount { get; set; }
        public int UserToplistCount { get; set; }
        public int OfficialToplistCount { get; set; }
        public string AkaTitle { get; set; }
        public string ImdbUrl { get; set; }
        public string Checked { get; set; }
        public bool IsChecked => Checked == "yes";
        public string Favorite { get; set; }
        public bool IsFavorite => Favorite == "yes";
        public string Disliked { get; set; }
        public bool IsDisliked => Disliked == "yes";
        public string Watchlist { get; set; }
        public bool InWatchlist => Watchlist == "yes";
        public string Owned { get; set; }
        public bool IsOwned => Owned == "yes";

        public string ImdbId
        {
            get
            {
                string pattern = @"(tt\d{7})";

                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                Match m = r.Match(ImdbUrl);
                if (!m.Success)
                {
                    throw new Exception("Unable to parse imdb id from imdb url");
                }
                return m.Value;
            }
        }

        public TraktMovie ToTraktMovie()
        {
            return ToTraktMovieWatched();
        }

        public TraktMovieWatched ToTraktMovieWatched()
        {
            var traktMovie = new TraktMovieWatched()
            {
                Ids = new TraktMovieId() { ImdbId = ImdbId },
                Title = Title,
                Year = Year,
            };
            if (IsChecked)
            {
                traktMovie.WatchedAt = Checked;
            }
            return traktMovie;
        }
    }
}

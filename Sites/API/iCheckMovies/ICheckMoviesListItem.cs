using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraktRater.Sites.API.iCheckMovies
{
    class ICheckMoviesListItem
    {
        public int Position { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int CheckedCount { get; set; }
        public int FavouriteCount { get; set; }
        // officialtoplistcount	usertoplistcount	akatitle	imdburl	checked	favorite	disliked	watchlist	owned
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
    }
}

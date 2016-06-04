namespace TraktRater.Sites.API.Flixster
{
    using System.Collections.Generic;

    using global::TraktRater.Extensions;
    using global::TraktRater.Web;

    public static class FlixsterAPI
    {
        public static IEnumerable<FlixsterMovieRating> GetRatedMovies(string userId, int page, int itemsPerPage)
        {
            string response = TraktWeb.TransmitExtended(string.Format(FlixsterURIs.UserRatingsMovies, userId, page, itemsPerPage));
            return response.FromJSONArray<FlixsterMovieRating>();
        }
    }
}

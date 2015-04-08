namespace TraktRater.Sites.API.TVDb
{
    using System.Net;

    using global::TraktRater.Extensions;
    using global::TraktRater.Web;

    public static class TVDbAPI
    {
        public static TVDbShowRatings GetShowRatings(string accountId)
        {
            string fileCache = TVDbCache.cShowRatingsFileCache;
            string response = TVDbCache.GetFromCache(fileCache);
            if (string.IsNullOrEmpty(response))
            {
                response = TraktWeb.Transmit(string.Format(TVDbURIs.UserShowRatings, accountId), string.Empty);
                TVDbCache.CacheResponse(response, fileCache);
                if (response.FromXML<TVDbShowRatings>() == null)
                {
                    TVDbCache.DeleteFromCache(fileCache);
                    return null;
                }
            }
            return response.FromXML<TVDbShowRatings>();
        }

        public static TVDbEpisodeRatings GetEpisodeRatings(string accountId, string seriesId)
        {
            string fileCache = string.Format(TVDbCache.cEpisodeRatingsFileCache, seriesId);
            string response = TVDbCache.GetFromCache(fileCache);
            if (string.IsNullOrEmpty(response))
            {
                response = TraktWeb.Transmit(string.Format(TVDbURIs.UserEpisodeRatings, accountId, seriesId), string.Empty);
                TVDbCache.CacheResponse(response, fileCache);
                if (response.FromXML<TVDbEpisodeRatings>() == null)
                {
                    TVDbCache.DeleteFromCache(fileCache);
                    return null;
                }
            }
            return response.FromXML<TVDbEpisodeRatings>();
        }

        public static TVDbShow GetShowInfo(string seriesId)
        {
            string fileCache = string.Format(TVDbCache.cShowInfoFileCache, seriesId);
            string response = TVDbCache.GetFromCache(fileCache, 7);
            if (string.IsNullOrEmpty(response))
            {
                response = TraktWeb.Transmit(string.Format(TVDbURIs.SeriesInfo, seriesId), string.Empty);
                TVDbCache.CacheResponse(response, fileCache);
                if (response.FromXML<TVDbShow>() == null)
                {
                    TVDbCache.DeleteFromCache(fileCache);
                    return null;
                }
            }
            return response.FromXML<TVDbShow>();
        }

        public static TVDbShowSearch SearchShow(string showName)
        {
            string fileCache = string.Format(TVDbCache.cShowSearchFileCache, showName.ReplaceInvalidFileChars());
            string response = TVDbCache.GetFromCache(fileCache, 7);
            if (string.IsNullOrEmpty(response))
            {
                response = TraktWeb.Transmit(string.Format(TVDbURIs.SeriesSearch, WebUtility.HtmlEncode(showName)), string.Empty);
                TVDbCache.CacheResponse(response, fileCache);
                if (response.FromXML<TVDbShowSearch>() == null)
                {
                    TVDbCache.DeleteFromCache(fileCache);
                    return null;
                }
            }
            return response.FromXML<TVDbShowSearch>();
        }
    }
}

namespace TraktRater.Sites.API.TVDb
{
    public static class TVDbURIs
    {
        const string ApiKey = "0023593574450A60";

        public const string UserShowRatings = @"http://thetvdb.com/api/GetRatingsForUser.php?apikey=" + ApiKey + "&accountid={0}";
        public const string UserEpisodeRatings = @"http://thetvdb.com/api/GetRatingsForUser.php?apikey=" + ApiKey + "&accountid={0}&seriesid={1}";
        public const string SeriesInfo = @"http://thetvdb.com/api/" + ApiKey + "/series/{0}/all/en.xml";
        public const string SeriesSearch = @"http://thetvdb.com/api/GetSeries.php?seriesname={0}";

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraktRater.Sites.API.TVDb
{
    public static class TVDbURIs
    {
        const string apiKey = "0023593574450A60";

        public const string UserShowRatings = @"http://thetvdb.com/api/GetRatingsForUser.php?apikey=" + apiKey + "&accountid={0}";
        public const string UserEpisodeRatings = @"http://thetvdb.com/api/GetRatingsForUser.php?apikey=" + apiKey + "&accountid={0}&seriesid={1}";
        public const string SeriesInfo = @"http://thetvdb.com/api/" + apiKey + "/series/{0}/all/en.xml";

    }
}

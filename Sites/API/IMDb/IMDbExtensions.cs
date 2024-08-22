namespace TraktRater.Sites.API.IMDb
{
    using System.Collections.Generic;

    public static class IMDbExtensions
    {
        public static IMDbType ItemType(this Dictionary<string, string> item)
        {
            return item[IMDbFieldMapping.cType].ItemType();
        }

        public static IMDbType ItemType(this string itemType)
        {
            IMDbType retValue = IMDbType.Unknown;

            switch (itemType.Replace(" ", "").ToLower())
            {
                case "video":
                case "documentary":
                case "tvmovie":
                case "tvshort":
                case "featurefilm":
                case "unknownwork":
                case "movie":
                    retValue = IMDbType.Movie;
                    break;

                case "tvspecial":
                case "tvminiseries":
                case "tvseries":
                    retValue = IMDbType.Show;
                    break;

                case "tvepisode":
                    retValue = IMDbType.Episode;
                    break;
                
                default:
                    // most likely a movie
                    retValue = IMDbType.Movie;
                    break;
            }
            return retValue;
        }

        public static bool IsCSVExport(this string provider)
        {
            return provider != "web";
        }
    }
}

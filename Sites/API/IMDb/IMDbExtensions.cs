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

            switch (itemType.ToLower())
            {
                case "video":
                case "documentary":
                case "tv movie":
                case "short film":
                case "feature film":
                case "unknown work":
                case "movie":
                    retValue = IMDbType.Movie;
                    break;

                case "tv special":
                case "mini-series":
                case "tv series":
                    retValue = IMDbType.Show;
                    break;

                case "tv episode":
                    retValue = IMDbType.Episode;
                    break;
                
                default:
                    retValue = IMDbType.Unknown;
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

namespace TraktRater.Sites.API.IMDb
{
    public static class IMDbExtensions
    {
        public static IMDbType ItemType(this string itemType)
        {
            IMDbType retValue = IMDbType.Unknown;

            switch (itemType)
            {
                case "Video":
                case "Documentary":
                case "TV Movie":
                case "Short Film":
                case "Feature Film":
                case "Unknown Work":
                    retValue = IMDbType.Movie;
                    break;

                case "TV Special":
                case "Mini-Series":
                case "TV Series":
                    retValue = IMDbType.Show;
                    break;

                case "TV Episode":
                    retValue = IMDbType.Episode;
                    break;
                
                default:
                    retValue = IMDbType.Unknown;
                    break;
            }
            return retValue;
        }

        public static int? ToYear(this string year)
        {
            int result = 0;
            if (int.TryParse(year, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static bool IsCSVExport(this string provider)
        {
            return provider != "web";
        }
    }
}

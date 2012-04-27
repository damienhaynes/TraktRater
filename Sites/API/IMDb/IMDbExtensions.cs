using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}

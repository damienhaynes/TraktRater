using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraktRater.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Date Time extension method to return a unix epoch
        /// time as a long
        /// </summary>
        /// <returns> A long representing the Date Time as the number
        /// of seconds since 1/1/1970</returns>
        public static long ToEpoch(this DateTime dt)
        {
            return (long)(dt - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Date Time extension method to return a unix epoch
        /// time as a long
        /// </summary>
        /// <returns> A long representing the Date Time as the number
        /// of seconds since 1/1/1970</returns>
        public static long ToEpoch(this string dt, double hourShift = 0)
        {
            DateTime date;
            if (DateTime.TryParse(dt, out date))
            {
                return date.AddHours(hourShift).ToEpoch();
            }

            return 0;
        }
        /// <summary>
        /// Long extension method to convert a Unix epoch
        /// time to a standard C# DateTime object.
        /// </summary>
        /// <returns>A DateTime object representing the unix
        /// time as seconds since 1/1/1970</returns>
        public static DateTime FromEpoch(this long unixTime)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTime);
        }

        /// <summary>
        /// Converts string DateTime to ISO8601 format
        /// 2014-09-01T09:10:11.000Z
        /// </summary>
        /// <param name="dt">DateTime as string</param>
        /// <param name="hourShift">Number of hours to shift original time</param>
        /// <returns>ISO8601 Timestamp</returns>
        public static string ToISO8601(this string dt, double hourShift = 0)
        {
            DateTime date;
            if (DateTime.TryParse(dt, out date))
            {
                return date.AddHours(hourShift).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }

            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}

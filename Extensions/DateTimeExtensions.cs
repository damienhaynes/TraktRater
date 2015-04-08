namespace TraktRater.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
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

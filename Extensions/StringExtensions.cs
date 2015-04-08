namespace TraktRater.Extensions
{
    using System.IO;
    using System.Linq;

    public static class StringExtensions
    {
        public static string ReplaceInvalidFileChars(this string filename)
        {
            return Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c, '_'));
        }
    }
}

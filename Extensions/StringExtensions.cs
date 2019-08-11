namespace TraktRater.Extensions
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string ReplaceInvalidFileChars(this string filename)
        {
            return Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c, '_'));
        }

        public static string ToSlug(this string phrase)
        {
            if (phrase == null) return string.Empty;
            if (phrase.IsNumber()) return phrase;

            var s = phrase.RemoveDiacritics().ToLower();
            s = Regex.Replace(s, @"[^a-z0-9\s-]", string.Empty);        // remove invalid characters
            s = Regex.Replace(s, @"\s+", " ").Trim();                   // single space
            s = s.Substring(0, s.Length <= 45 ? s.Length : 45).Trim();  // cut and trim
            s = Regex.Replace(s, @"\s", "-");                           // insert hyphens
            return s.ToLower();
        }

        public static string RemoveDiacritics(this string text)
        {
            if (text == null) return string.Empty;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool IsNumber(this string number)
        {
            double retValue;
            return double.TryParse(number, out retValue);
        }
    }
}

namespace TraktRater.Extensions
{
    public static class NumberExtensions
    {
        public static bool IsFloat(this string value)
        {
            float fvalue;
            return float.TryParse(value, out fvalue);
        }
    }
}
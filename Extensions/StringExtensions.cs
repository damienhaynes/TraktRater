using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TraktRater.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return null;

            string str = phrase.RemoveAccent().ToLower();

            // replace ampersand char with 'and'
            str = str.Replace("&", "and");

            // invalid chars           
            str = str.Replace("(", string.Empty);
            str = str.Replace(")", string.Empty);
            str = str.Replace("[", string.Empty);
            str = str.Replace("]", string.Empty);
            str = str.Replace(".", string.Empty);
            str = str.Replace("-", string.Empty);
            str = str.Replace("+", string.Empty);
            str = str.Replace("$", string.Empty);
            str = str.Replace("=", string.Empty);
            str = str.Replace("!", string.Empty);
            str = str.Replace(":", string.Empty);
            str = str.Replace(";", string.Empty);
            str = str.Replace("?", string.Empty);
            str = str.Replace("*", string.Empty);
            str = str.Replace("#", string.Empty);
            str = str.Replace("@", string.Empty);
            str = str.Replace("/", string.Empty);
            str = str.Replace("\\", string.Empty);

            // replace space with hyphen
            str = str.Replace(" ", "-");
            
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string ReplaceInvalidFileChars(this string filename)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }

            return filename;
        }
    }
}

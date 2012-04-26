using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TraktRater.Sites.API.TVDb
{
    public static class TVDbCache
    {
        static string cAppDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string cEpisodeRatingsFileCache = cAppDir + @"\TraktRater\Ratings\{0}.xml";
        public static string cShowRatingsFileCache = cAppDir + @"\TraktRater\Ratings\series.xml";
        public static string cShowInfoFileCache = cAppDir + @"\TraktRater\Series\{0}.xml";

        public static string GetFromCache(string filename)
        {
            return GetFromCache(filename, true);
        }
        public static string GetFromCache(string filename, bool expires)
        {
            try
            {
                if (!File.Exists(filename)) return null;

                // if cache is older than 1 day disregard
                if (expires && File.GetLastWriteTime(filename) <= DateTime.Now.Subtract(TimeSpan.FromDays(1)))
                    return null;

                return File.ReadAllText(filename);
            }
            catch { return null; }
        }

        public static void CacheResponse(string response, string filename)
        {
            if (response == null) return;

            try
            {
                string dir = Path.GetDirectoryName(filename);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(filename, response);
            }
            catch { return; }
        }

        public static void DeleteFromCache(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch { return; }
        }
    }
}

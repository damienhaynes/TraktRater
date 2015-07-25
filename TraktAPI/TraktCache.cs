namespace TraktRater.TraktAPI
{
    using System;
    using System.IO;

    public static class TraktCache
    {
        static string cAppDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        
        public static string cShowInfoFileCache = Path.Combine(cAppDir, @"TraktRater", @"Series", @"{0}.json");

        public static string GetFromCache(string filename, int expiresInDays)
        {
            try
            {
                if (!File.Exists(filename)) return null;

                // if cache is older than X days disregard
                if (File.GetLastWriteTime(filename) <= DateTime.Now.Subtract(TimeSpan.FromDays(expiresInDays)))
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
            catch (Exception)
            {
            }
        }

        public static void DeleteFromCache(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch (Exception)
            {
            }
        }
    }
}

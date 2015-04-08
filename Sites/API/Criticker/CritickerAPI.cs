namespace TraktRater.Sites.API.Criticker
{
    using System.IO;

    using global::TraktRater.Extensions;

    internal static class CritickerAPI
    {
        public static CritickerFilmRankings ReadCritickerMovieExportFile(string exportFile)
        {
            if (!File.Exists(exportFile)) return null;

            string xml = File.ReadAllText(exportFile);
            return xml.FromXML<CritickerFilmRankings>();
        }
    }
}

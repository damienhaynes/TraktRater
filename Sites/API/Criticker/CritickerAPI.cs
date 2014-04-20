using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TraktRater.Web;
using TraktRater.Extensions;

namespace TraktRater.Sites.API.Criticker
{
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

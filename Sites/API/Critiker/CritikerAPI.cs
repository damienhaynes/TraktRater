using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TraktRater.Web;
using TraktRater.Extensions;

namespace TraktRater.Sites.API.Critiker
{
    internal static class CritikerAPI
    {
        public static CritikerFilmRankings ReadCritikerMovieExportFile(string exportFile)
        {
            if (!File.Exists(exportFile)) return null;

            string xml = File.ReadAllText(exportFile);
            return xml.FromXML<CritikerFilmRankings>();
        }
    }
}

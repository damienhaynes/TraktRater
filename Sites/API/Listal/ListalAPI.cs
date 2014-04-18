using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraktRater.Web;
using TraktRater.Extensions;

namespace TraktRater.Sites.API.Listal
{
    public enum ListType
    {
        owned,
        wanted
    }

    public static class ListalAPI
    {
        public static ListalExport ReadListExportFile(string exportFile)
        {
            if (!File.Exists(exportFile)) return null;

            string xml = File.ReadAllText(exportFile);
            return xml.FromXML<ListalExport>();
        }
    }
}

namespace TraktRater.Sites.API.Listal
{
    using System.IO;

    using global::TraktRater.Extensions;

    public enum ListType
    {
        owned,
        wanted
    }

    public static class ListalAPI
    {
        public static ListalExport ReadListalExportFile(string exportFile)
        {
            if (!File.Exists(exportFile)) return null;

            string xml = File.ReadAllText(exportFile);
            return xml.FromXML<ListalExport>();
        }
    }
}

namespace TraktRater.Settings.XML
{
    using System;
    using System.IO;
    using System.Xml;

    class XmlReader
    {
        XmlDocument document = new XmlDocument();

        #region Settings Helper
        public bool GetSettingValueAsBool(string name, bool defaultvalue)
        {
            if (document == null) return defaultvalue;

            XmlNode node = null;
            node = document.DocumentElement.SelectSingleNode(string.Format("/settings//setting[@name='{0}']", name));
            if (node == null) return defaultvalue;

            try
            {
                bool result;
                if (bool.TryParse(node.Attributes["value"].Value, out result))
                    return result;
                else
                    return defaultvalue;
            }
            catch
            {
                return defaultvalue;
            }
        }

        public string GetSettingValueAsString(string name, string defaultvalue)
        {
            if (document == null) return defaultvalue;

            XmlNode node = null;
            node = document.DocumentElement.SelectSingleNode(string.Format("/settings//setting[@name='{0}']", name));
            if (node == null) return defaultvalue;

            try
            {
                return node.Attributes["value"].Value;
            }
            catch
            {
                return defaultvalue;
            }
        }

        public int GetSettingValueAsInt(string name, int defaultvalue)
        {
            if (document == null) return defaultvalue;

            XmlNode node = null;
            node = document.DocumentElement.SelectSingleNode(string.Format("/settings//setting[@name='{0}']", name));
            if (node == null) return defaultvalue;

            try
            {
                return Convert.ToInt32(node.Attributes["value"].Value);
            }
            catch
            {
                return defaultvalue;
            }
        }
        #endregion

        public bool Load(string file)
        {
            if (!File.Exists(file))
            {
                document = null;
                return false;
            }

            try
            {
                document.Load(file);
            }
            catch (Exception)
            {
                document = null;
                return false;
            }
            return true;
        }
    }
}

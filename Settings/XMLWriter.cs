namespace TraktRater.Settings.XML
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class XmlWriter
    {
        XmlDocument document = new XmlDocument();

        public void CreateXmlSettings(string file)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(file)))
                    Directory.CreateDirectory(Path.GetDirectoryName(file));

                XmlTextWriter textWriter = new XmlTextWriter(file, Encoding.UTF8);

                textWriter.WriteStartDocument();
                textWriter.WriteStartElement("settings");
                textWriter.WriteEndElement();
                textWriter.WriteEndDocument();

                textWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        public bool Load(string file)
        {
            if (!File.Exists(file)) return false;

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

        public bool Save(string file)
        {
            if (!File.Exists(file)) return false;

            try
            {
                document.Save(file);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #region Settings Helper
        public bool WriteSetting(string name, string value)
        {
            if (document == null) return false;

            try
            {
                value = value == null ? string.Empty : value.Trim();

                XmlNode node = null;
                node = document.SelectSingleNode(string.Format("/settings//setting[@name='{0}']", name));
                if (node == null)
                {
                    // select root node
                    node = document.SelectSingleNode("/settings");

                    // create new section node
                    XmlNode newNode = document.CreateElement("setting");

                    // create name attribute
                    XmlAttribute newAttribute = document.CreateAttribute("name");
                    newAttribute.Value = name;
                    newNode.Attributes.Append(newAttribute);
                    node.AppendChild(newNode);

                    // create value attribute
                    newAttribute = document.CreateAttribute("value");
                    newAttribute.Value = value;
                    newNode.Attributes.Append(newAttribute);
                    node.AppendChild(newNode);

                    return true;
                }
                else
                {
                    node.Attributes["value"].Value = value;
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
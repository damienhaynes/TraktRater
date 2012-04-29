using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace TraktRater.Settings.XML
{
    public class XmlWriter
    {
        XmlDocument Document = new XmlDocument();

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
                return;
            }
        }

        public bool Load(string file)
        {
            if (!File.Exists(file)) return false;

            try
            {
                Document.Load(file);
            }
            catch (XmlException)
            {
                Document = null;
                return false;
            }
            return true;
        }

        public bool Save(string file)
        {
            if (!File.Exists(file)) return false;

            try
            {
                Document.Save(file);
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
            if (Document == null) return false;

            try
            {
                value = value == null ? string.Empty : value.Trim();

                XmlNode node = null;
                node = Document.SelectSingleNode(string.Format("/settings//setting[@name='{0}']", name));
                if (node == null)
                {
                    // select root node
                    node = Document.SelectSingleNode("/settings");

                    // create new section node
                    XmlNode newNode = Document.CreateElement("setting");

                    // create name attribute
                    XmlAttribute newAttribute = Document.CreateAttribute("name");
                    newAttribute.Value = name;
                    newNode.Attributes.Append(newAttribute);
                    node.AppendChild(newNode);

                    // create value attribute
                    newAttribute = Document.CreateAttribute("value");
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
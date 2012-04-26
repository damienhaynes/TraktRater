using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TraktRater.Extensions
{
    public static class XmlExtensions
    {
        /// <summary>
        /// Creates an object from XML
        /// </summary>
        public static T FromXML<T>(this string xml)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (StringReader reader = new StringReader(xml))
                {
                    T ser = (T)serializer.Deserialize(reader);
                    reader.Close();
                    return ser;
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}

namespace TraktRater.Extensions
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

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

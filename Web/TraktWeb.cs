using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TraktRater.Settings;

namespace TraktRater.Web
{
    public class ExtendedWebClient : WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request != null)
                request.Timeout = Timeout;
            return request;
        }

        public ExtendedWebClient()
        {
            Timeout = 100000; // the standard HTTP Request Timeout default
        }
    }

    public static class TraktWeb
    {
        /// <summary>
        /// Communicates to and from a Server
        /// </summary>
        /// <param name="address">The URI to use</param>
        /// <param name="data">The Data to send</param>
        /// <returns>The response from Server</returns>
        public static string Transmit(string address, string data)
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                ExtendedWebClient client = new ExtendedWebClient();
                client.Timeout = 90000;
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("user-agent", AppSettings.UserAgent);
                if (string.IsNullOrEmpty(data))
                    return client.DownloadString(address);
                else
                    return client.UploadString(address, data);
            }
            catch (WebException e)
            {
                string ret = null;

                // something bad happened
                var response = e.Response as HttpWebResponse;
                try
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            ret = reader.ReadToEnd();
                        }
                    }
                }
                catch { }
                return ret;
            }
         }

        public static string TransmitExtended(string address)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                request.UserAgent = AppSettings.UserAgent;
                request.Accept = "application/json";

                WebResponse response = (HttpWebResponse)request.GetResponse();
                if (response == null) return null;

                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string strResponse = reader.ReadToEnd();

                stream.Close();
                reader.Close();
                response.Close();

                return strResponse;
            }
            catch
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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
                client.Timeout = 30000;
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("user-agent", Settings.UserAgent);
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
    }
}

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
        #region Events
        internal delegate void OnDataSendDelegate(string url, string postData);
        internal delegate void OnDataReceivedDelegate(string response);
        internal delegate void OnDataErrorReceivedDelegate(string error);

        internal static event OnDataSendDelegate OnDataSend;
        internal static event OnDataReceivedDelegate OnDataReceived;
        internal static event OnDataErrorReceivedDelegate OnDataErrorReceived;
        #endregion

        /// <summary>
        /// Communicates to and from a Server
        /// </summary>
        /// <param name="address">The URI to use</param>
        /// <param name="data">The Data to send</param>
        /// <returns>The response from Server</returns>
        public static string Transmit(string address, string data, bool logResponse = true)
        {
            if (OnDataSend != null)
                OnDataSend(address, data);

            try
            {
                ServicePointManager.Expect100Continue = false;
                ExtendedWebClient client = new ExtendedWebClient();
                client.Timeout = 90000;
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("user-agent", AppSettings.UserAgent);
                
                string response = string.Empty;

                if (string.IsNullOrEmpty(data))
                    response = client.DownloadString(address);
                else
                    response = client.UploadString(address, data);

                if (logResponse && OnDataReceived != null)
                    OnDataReceived(response);

                return response;
            }
            catch (WebException we)
            {
                string ret = null;

                // something bad happened
                var response = we.Response as HttpWebResponse;
                try
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            ret = reader.ReadToEnd();

                            if (OnDataErrorReceived != null)
                                OnDataErrorReceived(ret);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (OnDataErrorReceived != null)
                        OnDataErrorReceived(e.Message);
                }
                return ret;
            }
         }

        public static string TransmitExtended(string address)
        {
            if (OnDataSend != null)
                OnDataSend(address, null);

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

                if (OnDataReceived != null)
                    OnDataReceived(strResponse);

                stream.Close();
                reader.Close();
                response.Close();

                return strResponse;
            }
            catch (Exception e)
            {
                if (OnDataErrorReceived != null)
                    OnDataErrorReceived(e.Message);

                return null;
            }
        }
    }
}

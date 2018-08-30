using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils
{
    public class HtmlManager
    {
        private static string UserAgent = JavINIClass.IniReadValue("Html", "UserAgent");

        public static HtmlResponse GetHtmlContentViaUrl(string url, string end = "utf-8",  bool isJav = false, CookieContainer cc = null)
        {
            HtmlResponse res = new HtmlResponse
            {
                Success = false
            };

            try
            {
                GC.Collect();
                StringBuilder sb = new StringBuilder();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 90000;
                request.UserAgent = UserAgent;
                request.Method = "GET";

                if (isJav)
                {
                    request.CookieContainer = cc;
                }

                request.KeepAlive = true;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding(end));
                while (!reader.EndOfStream)
                {
                    sb.AppendLine(reader.ReadLine());
                }
                res.Content = sb.ToString();
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                res.Success = false;
            }

            res.Success = true;
            return res;
        }
    }

    public class HtmlResponse
    {
        public bool Success { get; set; }
        public string Content { get; set; }
    }
}

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
        public static HtmlResponse GetHtmlContentViaUrl(string url, string end = "utf-8", bool isJav = false)
        {
            HtmlResponse res = new HtmlResponse();
            res.Success = false;

            try
            {
                GC.Collect();
                StringBuilder sb = new StringBuilder();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 90000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36";//设置User-Agent，伪装成Google Chrome浏览器
                request.Method = "GET";
                if (isJav)
                {
                    CookieContainer cookie = new CookieContainer();
                    cookie.Add(new Cookie("__cfuid", "d68af188bd94d83e83ede105bb0e689791532670718", "/", "www.javlibrary.com"));
                    cookie.Add(new Cookie("timezone", "-480", "/", "www.javlibrary.com"));
                    cookie.Add(new Cookie("over18", "18", "/", "www.javlibrary.com"));
                    cookie.Add(new Cookie("cf_clearance", "b7c1fa02f0ee1825907214fb26da30a53f6895f0-1532671763-3600", "/", "www.javlibrary.com"));
                    cookie.Add(new Cookie("__qca", "P0-1301848006-1532671765632", "/", "www.javlibrary.com"));
                    request.CookieContainer = cookie;
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

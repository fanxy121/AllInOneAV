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
        public static HtmlResponse GetHtmlContentViaUrl(string url, string end = "utf-8")
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
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                request.Method = "GET";
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

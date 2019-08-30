using Microsoft.Win32;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static string Error = JavINIClass.IniReadValue("Jav", "error");

        public static string GetChromeVersion()
        {
            object path;
            path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
            if (path != null)
            {
                return FileVersionInfo.GetVersionInfo(path.ToString()).FileVersion;
            }
            else
            {
                return "";
            }
        }

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
                request.UserAgent = string.Format(UserAgent, GetChromeVersion());
                request.Method = "GET";

                if (isJav)
                {
                    request.CookieContainer = cc;
                }

                request.KeepAlive = true;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding(end));
                //while (!reader.EndOfStream)
                //{
                //    sb.AppendLine(reader.ReadLine());
                //}
                res.Content = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                res.Success = false;

                if (e.Message == Error)
                {
                    Console.WriteLine("123");
                }

            }

            res.Success = true;
            return res;
        }

        public static NeedToUpdate NeedToUpdateCookie(string url, string end = "utf-8", bool isJav = false, CookieContainer cc = null)
        {
            NeedToUpdate ret = new NeedToUpdate();
            ret.Content = new Model.JavModels.HtmlResponse();

            try
            {
                GC.Collect();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 90000;
                request.UserAgent = string.Format(UserAgent, GetChromeVersion());
                request.Method = "GET";

                if (isJav)
                {
                    request.CookieContainer = cc;
                }

                request.KeepAlive = true;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding(end));

                ret.Content.Content = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                if (e.Message == Error)
                {
                    ret.Content.Success = false;
                    ret.Need = true;
                    return ret;
                }
            }

            ret.Content.Success = true;
            ret.Need = false;
            return ret;
        }
    }

    public class HtmlResponse
    {
        public bool Success { get; set; }
        public string Content { get; set; }
    }
}

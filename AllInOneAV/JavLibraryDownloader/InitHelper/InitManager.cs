using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.LogHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace JavLibraryDownloader.InitHelper
{
    public class InitManager
    {
        private static JavLibraryLog _logger = JavLoggerManager.GetInitLogger();
        private static string categoryURL = JavINIClass.IniReadValue("Jav", "category");
        private static string prefix = JavINIClass.IniReadValue("Jav", "prefix");
        private static string postfix = JavINIClass.IniReadValue("Jav", "postfix");
        private static string categoryPattern = JavINIClass.IniReadValue("Jav", "categoryPattern");
        private static string categoryPrefix = JavINIClass.IniReadValue("Jav", "categoryPrefix");

        public static CookieContainer GetCookie()
        {
            CookieContainer cc = new CookieContainer();
            var process = Broswer.OpenBrowserUrl("http://www.javlibrary.com/cn/");
            Thread.Sleep(10000);
            Broswer.Refresh_click(process);
            Thread.Sleep(30000);
            Broswer.CloseBroswer(process);

            var data = new ChromeCookieReader().ReadCookies("jav");

            foreach (var item in data.Distinct())
            {
                Cookie c = new Cookie(item.Name, item.Value, "/", "www.javlibrary.com");

                cc.Add(c);
            }

            cc.Add(new Cookie("over18", "18", "/", "www.javlibrary.com"));
            return cc;
        }

        public static bool InitCategory(CookieContainer cc)
        {
            try
            {
                return GetCategory(cc);
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog("", string.Format("Init failed. {0}", e.ToString()));
            }

            return false;
        }

        private static bool GetCategory(CookieContainer cc)
        {
            Console.WriteLine("Start to init catrgories...");

            var res = HtmlManager.GetHtmlContentViaUrl(categoryURL, "utf-8", true, cc);

            if (res.Success)
            {
                return ProcessCategory(res.Content);
            }
            else
            {
                _logger.WriteExceptionLog("", string.Format("No category found"));          
            }

            return false;
        }

        private static bool ProcessCategory(string content)
        {
            try
            {
                JavDataBaseManager.DeleteCategory();

                var m = Regex.Matches(content, categoryPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                foreach (Match item in m)
                {
                    Category c = new Category
                    {
                        Url = prefix + categoryPrefix + item.Groups[1].Value + postfix,
                        Name = item.Groups[2].Value
                    };

                    Console.WriteLine(string.Format("Get category {0}, URL {1}", c.Name, c.Url));
                    JavDataBaseManager.InsertCategory(c);
                }
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog("", string.Format("Process category error {0}", e.ToString()));
                return false;
            }

            return true;
        }
    }
}

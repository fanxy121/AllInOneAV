using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace JavLibraryDownloader.Process
{
    public class Process
    {
        private static string UpdateURL = JavINIClass.IniReadValue("Jav", "update");

        public static void Start(RunType type)
        {
            var cc = InitHelper.InitManager.GetCookie();
            var res = InitHelper.InitManager.InitCategory(cc);

            if (res)
            {
                if (type == RunType.Both || type == RunType.Scan)
                {
                    DoScan(new List<Category>(), cc);
                    SecondTry(type, cc);
                }

                if (type == RunType.Both || type == RunType.Download)
                {
                    DoDownload(cc);
                    SecondTry(type, cc);
                }

                if (type == RunType.SecondTry)
                {
                    SecondTry(RunType.Both, cc);
                }

                if (type == RunType.Update)
                {
                    DoScan(new List<Category>()
                    {
                        new Category() {
                            Name = "Update",
                            Url = UpdateURL
                        }
                    },cc);
                }
            }
            else
            {
                Console.WriteLine("Nothing to do");
            }
        }

        public static void DoScan(string url)
        {
            var cc = InitHelper.InitManager.GetCookie();
            ScanHelper.ScanManager.Scan(url, url, 1, 1, cc);
        }

        public static void DoDownload(string url)
        {
            var cc = InitHelper.InitManager.GetCookie();
            DownloadHelper.DownloadManager.Download(url, 1, 1, cc);
        }

        public static void DoScan(List<Category> categories, CookieContainer cc)
        {
            bool isUpdate = false;

            if (categories.Count == 0)
            {
                categories = JavDataBaseManager.GetCategories();
            }
            else
            {
                isUpdate = true;
            }

            int currentCategory = 1;

            DateTime now = DateTime.Now;

            foreach (var category in categories)
            {
                if ((DateTime.Now - now).TotalMinutes >= 25)
                {
                    cc = InitHelper.InitManager.GetCookie();
                    now = DateTime.Now;
                }

                ScanHelper.ScanManager.Scan(category.Url, category.Name, currentCategory, categories.Count, cc, isUpdate);
                currentCategory++;
            }
        }

        public static void DoDownload(CookieContainer cc)
        {
            var noDownloads = JavDataBaseManager.GetScanURL().Where(x => x.IsDownload == false);
            int currentItems = 1;

            DateTime now = DateTime.Now;

            foreach (var item in noDownloads)
            {
                if ((DateTime.Now - now).TotalMinutes >= 25)
                {
                    cc = InitHelper.InitManager.GetCookie();
                    now = DateTime.Now;
                }

                DownloadHelper.DownloadManager.Download(item.URL, currentItems, noDownloads.Count(), cc);
            }
        }

        public static void DoSecondScan(List<SecondTry> second, CookieContainer cc)
        {
            int currentCategory = 1;

            DateTime now = DateTime.Now;

            foreach (var item in second)
            {
                if ((DateTime.Now - now).TotalMinutes >= 25)
                {
                    cc = InitHelper.InitManager.GetCookie();
                    now = DateTime.Now;
                }

                ScanHelper.ScanManager.Scan(item.Url, "SecondTry", currentCategory, second.Count, cc);
                currentCategory++;
            }
        }

        public static void DoSecondDownload(List<SecondTry> second, CookieContainer cc)
        {
            int currentItems = 1;

            DateTime now = DateTime.Now;

            foreach (var item in second)
            {
                if ((DateTime.Now - now).TotalMinutes >= 25)
                {
                    cc = InitHelper.InitManager.GetCookie();
                    now = DateTime.Now;
                }

                DownloadHelper.DownloadManager.Download(item.Url, currentItems, second.Count(), cc);
                currentItems++;
            }
        }

        public static void SecondTry(RunType type, CookieContainer cc)
        {
            var flag = true;

            if (type == RunType.Both)
            {
                while(flag)
                {
                    var target = JavDataBaseManager.GetSecondTry();
                    var scanTarget = target.Where(x => x.Logger.Trim() == "Scan").ToList();
                    var downTarget = target.Where(x => x.Logger.Trim() == "Download").ToList();
                    JavDataBaseManager.DeleteSecondTry();

                    DoSecondScan(scanTarget, cc);
                    DoSecondDownload(downTarget, cc);

                    if (target == null || target.Count == 0)
                    {
                        flag = false;
                    }
                }
            }

            if (type == RunType.Scan)
            {
                while (flag)
                {
                    var target = JavDataBaseManager.GetSecondTry();
                    var scanTarget = target.Where(x => x.Logger == "Scan").ToList();
                    JavDataBaseManager.DeleteSecondTry();

                    DoSecondScan(scanTarget, cc);

                    if (target == null || target.Count == 0)
                    {
                        flag = false;
                    }
                }
            }

            if (type == RunType.Download)
            {
                while (flag)
                {
                    var target = JavDataBaseManager.GetSecondTry();
                    var downTarget = target.Where(x => x.Logger == "Download").ToList();
                    JavDataBaseManager.DeleteSecondTry();

                    DoSecondDownload(downTarget, cc);

                    if (target == null || target.Count == 0)
                    {
                        flag = false;
                    }
                }
            }
        }
    }
}

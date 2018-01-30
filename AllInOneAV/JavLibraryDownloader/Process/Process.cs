using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var res = InitHelper.InitManager.InitCategory();

            if (res)
            {
                if (type == RunType.Both || type == RunType.Scan)
                {
                    DoScan(new List<Category>());
                    SecondTry(type);
                }

                if (type == RunType.Both || type == RunType.Download)
                {
                    DoDownload();
                    SecondTry(type);
                }

                if (type == RunType.SecondTry)
                {
                    SecondTry(RunType.Both);
                }

                if (type == RunType.Update)
                {
                    DoScan(new List<Category>()
                    {
                        new Category() {
                            Name = "Update",
                            Url = UpdateURL
                        }
                    });
                }
            }
            else
            {
                Console.WriteLine("Nothing to do");
            }
        }

        public static void DoScan(string url)
        {
            ScanHelper.ScanManager.Scan(url, url, 1, 1);
        }

        public static void DoDownload(string url)
        {
            DownloadHelper.DownloadManager.Download(url, 1, 1);
        }

        public static void DoScan(List<Category> categories)
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

            foreach (var category in categories)
            {
                ScanHelper.ScanManager.Scan(category.Url, category.Name, currentCategory, categories.Count, isUpdate);
                currentCategory++;
            }
        }

        public static void DoDownload()
        {
            var noDownloads = JavDataBaseManager.GetScanURL().Where(x => x.IsDownload == false);
            int currentItems = 1;

            foreach (var item in noDownloads)
            {
                DownloadHelper.DownloadManager.Download(item.URL, currentItems, noDownloads.Count());
            }
        }

        public static void DoSecondScan(List<SecondTry> second)
        {
            int currentCategory = 1;

            foreach (var item in second)
            {
                ScanHelper.ScanManager.Scan(item.Url, "SecondTry", currentCategory, second.Count);
                currentCategory++;
            }
        }

        public static void DoSecondDownload(List<SecondTry> second)
        {
            int currentItems = 1;

            foreach (var item in second)
            {
                DownloadHelper.DownloadManager.Download(item.Url, currentItems, second.Count());
            }
        }

        public static void SecondTry(RunType type)
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

                    DoSecondScan(scanTarget);
                    DoSecondDownload(downTarget);

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

                    DoSecondScan(scanTarget);

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

                    DoSecondDownload(downTarget);

                    if (target == null || target.Count == 0)
                    {
                        flag = false;
                    }
                }
            }
        }
    }
}

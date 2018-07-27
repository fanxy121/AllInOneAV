using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.LogHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace JavLibraryDownloader.DownloadHelper
{
    public class DownloadManager
    {
        private static JavLibraryLog _logger = JavLoggerManager.GetDownloadLogger();
        private static string prefix = JavINIClass.IniReadValue("Jav", "prefix");
        private static string imgFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private static string directorPrefix = JavINIClass.IniReadValue("Jav", "directorPrefix");
        private static string companyPrefix = JavINIClass.IniReadValue("Jav", "companyPrefix");
        private static string publisherPrefix = JavINIClass.IniReadValue("Jav", "publisherPrefix");
        private static string actressPrefix = JavINIClass.IniReadValue("Jav", "actressPrefix");

        private static string detailTitlePattern = JavINIClass.IniReadValue("Jav", "detailTitle");
        private static string detailImgPattern = JavINIClass.IniReadValue("Jav", "detailImg");
        private static string detailIDPattern = JavINIClass.IniReadValue("Jav", "detailID");
        private static string detailDatePattern = JavINIClass.IniReadValue("Jav", "detailDate");
        private static string detailLengthPattern = JavINIClass.IniReadValue("Jav", "detailLength");
        private static string detailDirectorPattern = JavINIClass.IniReadValue("Jav", "detailDirector");
        private static string detailCompanyPattern = JavINIClass.IniReadValue("Jav", "detailCompany");
        private static string detailPublisherPattern = JavINIClass.IniReadValue("Jav", "detailPublisher");
        private static string detailCategoryPattern = JavINIClass.IniReadValue("Jav", "detailCategory");
        private static string detailActressPattern = JavINIClass.IniReadValue("Jav", "detailActress");
        private static string detailCommentPattern = JavINIClass.IniReadValue("Jav", "detailComment");

        public static void Download(string url, int currentItem, int totalItem)
        {
            if (!Directory.Exists(imgFolder))
            {
                Directory.CreateDirectory(imgFolder);
            }

            try
            {
                StartDownload(prefix + url, url, currentItem, totalItem);
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog(url, string.Format("Download failed"));
            }
        }

        public static void StartDownload(string url, string oriURL, int currentItem, int totalItem)
        {
            var res = HtmlManager.GetHtmlContentViaUrl(url, "utf-8", true);

            try
            {
                if (res.Success)
                {
                    _logger.WriteLog(url, string.IsNullOrWhiteSpace(res.Content) ? "空" : res.Content);

                    Console.WriteLine(string.Format("Start to download {0}, {1}/{2}", oriURL, currentItem, totalItem));

                    AV av = new AV();

                    var m = Regex.Matches(res.Content, detailIDPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var data = item.Groups[1].Value;
                        Console.WriteLine(string.Format("Get AV {0}, ID -> {1}", url, data));
                        av.ID = data;
                    }

                    m = Regex.Matches(res.Content, detailTitlePattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var data = item.Groups[2].Value.Replace(av.ID + " ", "");
                        Console.WriteLine(string.Format("Get AV {0}, Title -> {1}", url, data));
                        av.Name = FileUtility.ReplaceInvalidChar(data);
                    }

                    m = Regex.Matches(res.Content, detailImgPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var data = item.Groups[1].Value.StartsWith("http") ? item.Groups[1].Value : "http:" + item.Groups[1].Value;
                        Console.WriteLine(string.Format("Get AV {0}, IMG -> {1}", url, data));
                        av.PictureURL = data;
                    }

                    m = Regex.Matches(res.Content, detailDatePattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var data = item.Groups[1].Value;
                        Console.WriteLine(string.Format("Get AV {0}, Date -> {1}", url, data));
                        av.ReleaseDate = DateTime.Parse(data);
                    }

                    m = Regex.Matches(res.Content, detailLengthPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var data = item.Groups[1].Value;
                        Console.WriteLine(string.Format("Get AV {0}, Length -> {1}", url, data));
                        av.AvLength = int.Parse(data);
                    }

                    m = Regex.Matches(res.Content, detailDirectorPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var u = item.Groups[1].Value;
                        var data = item.Groups[2].Value;
                        Console.WriteLine(string.Format("Get AV {0}, Director -> {1}", url, data));
                        av.Directory += data + ",";
                        Director d = new Director
                        {
                            CreateTime = DateTime.Now,
                            Name = data,
                            URL = prefix + directorPrefix + u
                        };
                        JavDataBaseManager.InsertDirector(d);
                    }

                    m = Regex.Matches(res.Content, detailCompanyPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var u = item.Groups[1].Value;
                        var data = item.Groups[2].Value;
                        Console.WriteLine(string.Format("Get AV {0}, Company -> {1}", url, data));
                        av.Company += data + ",";
                        Company c = new Company
                        {
                            CreateTime = DateTime.Now,
                            Name = data,
                            URL = prefix + companyPrefix + u
                        };
                        JavDataBaseManager.InsertCompany(c);
                    }

                    m = Regex.Matches(res.Content, detailPublisherPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var u = item.Groups[1].Value;
                        var data = item.Groups[2].Value;
                        Console.WriteLine(string.Format("Get AV {0}, Publisher -> {1}", url, data));
                        av.Publisher += data + ",";
                        Publisher p = new Publisher
                        {
                            CreateTime = DateTime.Now,
                            Name = data,
                            URL = prefix + publisherPrefix + u
                        };
                        JavDataBaseManager.InsertPublisher(p);
                    }

                    m = Regex.Matches(res.Content, detailCategoryPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var data = item.Groups[2].Value;
                        Console.WriteLine(string.Format("Get AV {0}, Category -> {1}", url, data));
                        av.Category += data + ",";
                    }

                    m = Regex.Matches(res.Content, detailActressPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var u = item.Groups[1].Value;
                        var data = item.Groups[2].Value;
                        Console.WriteLine(string.Format("Get AV {0}, Actress -> {1}", url, data));
                        av.Actress += data + ",";
                        Actress a = new Actress
                        {
                            CreateTime = DateTime.Now,
                            Name = data,
                            URL = prefix + actressPrefix + u
                        };
                        JavDataBaseManager.InsertActress(a);
                    }

                    m = Regex.Matches(res.Content, detailCommentPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (Match item in m)
                    {
                        var data = item.Groups[1].Value;
                        Console.WriteLine(string.Format("Get AV {0}, Comments -> {1}", url, data));
                        Comments c = new Comments
                        {
                            Comment = data,
                            AvID = av.ID,
                            AvTitle = av.Name,
                            CreateTime = DateTime.Now
                        };
                        if (!JavDataBaseManager.HasComment(c))
                        {
                            JavDataBaseManager.InsertComment(c);
                        }
                    }
                    av.URL = url;

                    JavDataBaseManager.InsertAV(av);
                    var result = Utils.DownloadHelper.DownloadFile(av.PictureURL, imgFolder + av.ID + av.Name + ".jpg");
                    JavDataBaseManager.UpdateScanURL(oriURL);

                    if (!string.IsNullOrEmpty(result))
                    {
                        _logger.WriteExceptionLog(url, string.Format("Download picture failed"));
                    }
                }
                else
                {
                    _logger.WriteExceptionLog(url, string.Format("Download failed"));
                }
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog(url, string.Format("Download failed {0}", e.ToString()));
            }
        }
    }
}

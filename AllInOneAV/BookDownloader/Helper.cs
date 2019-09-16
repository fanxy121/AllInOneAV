using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookDownloader
{
    public class Helper
    {
        public static BookInfo GetAllChapters(string index)
        {
            Console.WriteLine("获取漫画详情");
            BookInfo ret = new BookInfo();
            ret.Chapters = new List<Chapter>();

            try
            {
                var cc = new CookieContainer();
                Console.WriteLine("获取Cookie");
                var cookies = Utils.HtmlManager.GetCookies(index);

                ret.Cookie = cc;

                for (int i = 0; i < cookies.Count; i++)
                {
                    cc.Add(cookies[i]);
                }

                Console.WriteLine("获取详情");
                var content = Utils.HtmlManager.GetHtmlContentViaUrl(index);
                if (content.Success)
                {
                    Console.WriteLine("获取详情成功");
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(content.Content);

                    var titlePath = "//div[@class=\"info\"]/h1";
                    var titleNode = doc.DocumentNode.SelectSingleNode(titlePath);

                    Console.WriteLine("解析标题");
                    if (titleNode != null)
                    {                     
                        var title = titleNode.InnerText;
                        Console.WriteLine("标题 -> " + title);
                        ret.BookName = title;

                        Console.WriteLine("解析章节");
                        var chaptersPath = "//div[@id=\"chapterlistload\"]/ul/li";
                        var chaptersNodes = doc.DocumentNode.SelectNodes(chaptersPath);

                        foreach (var c in chaptersNodes)
                        {
                            Chapter chapter = new Chapter
                            {
                                ChapterName = c.InnerText.Trim(),
                                ChapterUrl = c.ChildNodes["a"].Attributes["href"].Value
                            };

                            Console.WriteLine("章节 -> " + chapter.ChapterName);
                            ret.Chapters.Add(chapter);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("获取详情失败");
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }

            return ret;
        }

        public static List<PicToDownload> DownloadChapter(string host, string root, Chapter chapter, CookieContainer cookie)
        {
            Console.WriteLine("处理章节 -> " + chapter.ChapterName);

            List<PicToDownload> ret = new List<PicToDownload>();
            int index = 1;

            var subRoot = root + "/" + chapter.ChapterName + "/";

            Console.WriteLine("获取详情");
            var content = Utils.HtmlManager.GetHtmlContentViaUrl(host + chapter.ChapterUrl);
            if (content.Success)
            {
                Console.WriteLine("获取详情成功");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content.Content);

                Console.WriteLine("解析图片");
                var picPath = "//img[@class=\"lazy\"]";
                var picNodes = doc.DocumentNode.SelectNodes(picPath);

                foreach (var pic in picNodes)
                {
                    ret.Add(new PicToDownload {
                        PicUrl = pic.Attributes["data-original"].Value,
                        FilePath = subRoot + (index) + ".jpg",
                        FolderPath = subRoot
                    });
                    Console.WriteLine("解析图片 -> " + pic.Attributes["data-original"].Value);
                    index++;
                }
            }

            return ret;
        }

        public static string GenerateJsonFile(List<PicToDownload> total, BookInfo bookInfo, string root)
        {
            Console.WriteLine("处理Json文件");

           var json = JsonConvert.SerializeObject(total);
            var jsonFile = root + "/" + bookInfo.BookName + ".json";

            if (File.Exists(jsonFile))
            {
                Console.WriteLine("删除Json文件");
                File.Delete(jsonFile);
            }
            else
            {
                Console.WriteLine("写入Json文件");
                StreamWriter sw = new StreamWriter(jsonFile);
                sw.WriteLine(json);
                sw.Close();
            }

            return jsonFile;
        }

        public static bool Download(string jsonFile)
        {
            bool ret = false;
            Console.WriteLine("下载图片");
            StreamReader sr = new StreamReader(jsonFile);
            var json = sr.ReadToEnd();
            sr.Close();

            List<PicToDownload> pics = JsonConvert.DeserializeObject<List<PicToDownload>>(json);

            foreach (var pic in pics)
            {
                if (!Directory.Exists(pic.FolderPath))
                {
                    Console.WriteLine("创建文件夹 -> " + pic.FolderPath);
                    Directory.CreateDirectory(pic.FolderPath);
                }

                if (!File.Exists(pic.FilePath))
                {
                    Console.WriteLine("下载图片 -> 从 " + pic.PicUrl + " 到 " + pic.FilePath);
                    Utils.DownloadHelper.DownloadFile(pic.PicUrl, pic.FilePath);
                    ret = true;
                }
                else
                {
                    Console.WriteLine("文件已存在 -> " + pic.FilePath);
                }
            }

            return ret;
        }
    }
}

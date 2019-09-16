using HtmlAgilityPack;
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

        public static void DownloadChapter(string host, string root, Chapter chapter, CookieContainer cookie)
        {
            Console.WriteLine("下载章节");

            List<string> pics = new List<string>();
            var subRoot = root + "/" + chapter.ChapterName + "/";

            if (Directory.Exists(subRoot))
            {
                Console.WriteLine("删除文件夹 -> " + subRoot);
                Directory.Delete(subRoot,true);
            }
            Console.WriteLine("创建文件夹 -> " + subRoot);
            Directory.CreateDirectory(subRoot);

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
                    pics.Add(pic.Attributes["data-original"].Value);
                    Console.WriteLine("解析图片 -> " + pic.Attributes["src"].Value);
                }
            }

            for (int i = 0; i < pics.Count; i++)
            {
                Console.WriteLine("下载图片 -> 从" + pics[i] + " 到" + subRoot + (i + 1) + ".jpg");
                Utils.DownloadHelper.DownloadFile(pics[i], subRoot + (i + 1) + ".jpg");
            }
        }
    }
}

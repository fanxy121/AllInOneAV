using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                var index = args[0];
                var host = args[1];
                var root = "c:/setting/comic";
                List<BookInfo> allBooks = new List<BookInfo>();
                List<string> list = new List<string>();
                List<PicToDownload> total = new List<PicToDownload>();

                var totalPage = Helper.GetTotalPages(args[0]);

                for (int i = 0; i < totalPage; i++)
                {
                    list.AddRange(Helper.GetAllBookIndex(index, host, (i + 1)));
                }

                foreach (var book in list)
                {
                    var bookInfo = Helper.GetAllChapters(book);

                    if (!string.IsNullOrEmpty(bookInfo.BookName) && bookInfo.Chapters.Count > 0)
                    {
                        if (!Directory.Exists(root))
                        {
                            Directory.CreateDirectory(root);
                        }

                        foreach (var c in bookInfo.Chapters)
                        {
                            total.AddRange(Helper.DownloadChapter(host, root, bookInfo.BookName, c));
                        }

                        var jsonFile = Helper.GenerateJsonFile(total, bookInfo, root);

                        bool isContinue = true;

                        while (isContinue)
                        {
                            isContinue = Helper.Download(jsonFile);
                        }
                    }

                    total = new List<PicToDownload>();
                }
            }

            if (args != null && args.Length == 3)
            {
                var index = args[0];
                var root = args[1];
                var host = args[2];
                List<PicToDownload> total = new List<PicToDownload>();

                Console.WriteLine(string.Format("首页 -> {0}, 根目录 -> {1}, 主机 -> {2}", index, root, host));

                var bookInfo = Helper.GetAllChapters(index);

                if (!string.IsNullOrEmpty(bookInfo.BookName) && bookInfo.Chapters.Count > 0)
                {
                    if(!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }

                    foreach (var c in bookInfo.Chapters)
                    {
                        total.AddRange(Helper.DownloadChapter(host, root, bookInfo.BookName, c));
                    }

                    var jsonFile = Helper.GenerateJsonFile(total, bookInfo, root);

                    bool isContinue = true;

                    while(isContinue)
                    {
                        isContinue = Helper.Download(jsonFile);
                    }
                }
            }
        }
    }
}

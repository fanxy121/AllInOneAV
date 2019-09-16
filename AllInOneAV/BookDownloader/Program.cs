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
            if (args != null && args.Length == 3)
            {
                var index = args[0];
                var root = args[1];
                var host = args[2];

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
                        Helper.DownloadChapter(host, root, c, bookInfo.Cookie);
                    }
                }
            }
        }
    }
}

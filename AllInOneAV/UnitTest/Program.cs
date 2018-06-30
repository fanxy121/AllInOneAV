using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.SisDataBaseHelper;
using Model.Common;
using Model.ScanModels;
using Model.SisModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace UnitTest
{
    class Program
    {
        #region SisDownloadParam
        private static string AsiaUncensoredAuthorshipSeed = JavINIClass.IniReadValue("Sis", "AsiaUncensoredAuthorshipSeed");
        private static string AsiaUncensoredSection = JavINIClass.IniReadValue("Sis", "AsiaUncensoredSection");
        private static string WesternUncensoredAuthorshipSeed = JavINIClass.IniReadValue("Sis", "WesternUncensoredAuthorshipSeed");
        private static string WesternUncensored = JavINIClass.IniReadValue("Sis", "WesternUncensored");
        private static string AsiaCensoredAuthorshipSeed = JavINIClass.IniReadValue("Sis", "AsiaCensoredAuthorshipSeed");
        private static string AsiaCensoredSection = JavINIClass.IniReadValue("Sis", "AsiaCensoredSection");
        private static string RootFolder = JavINIClass.IniReadValue("Sis", "root");
        private static string ListPattern = JavINIClass.IniReadValue("Sis", "ListPattern");
        private static string ListDatePattern = JavINIClass.IniReadValue("Sis", "ListDatePattern");
        private static string Prefix = JavINIClass.IniReadValue("Sis", "Prefix");
        private static readonly Dictionary<string, string> ChannelMapping = new Dictionary<string, string> { { AsiaCensoredAuthorshipSeed, "亚洲有码原创" }, { AsiaCensoredSection, "亚洲有码转帖" }, { WesternUncensoredAuthorshipSeed, "欧美无码原创" }, { WesternUncensored, "欧美无码转帖" }, { AsiaUncensoredAuthorshipSeed, "亚洲无码原创" }, { AsiaUncensoredSection, "亚洲无码转帖" } };
        #endregion

        static void Main(string[] args)
        {
            //StartTestSisDownload();
            StartTestRegMatch();

            var list = GetPreFix();
        }

        #region SisDownloadMethod
        private static void StartTestSisDownload()
        {
            StringBuilder sb = new StringBuilder();
            var url = "http://sis001.com/bbs/forum-58-3.html";
            var res = HtmlManager.GetHtmlContentViaUrl(url, "gbk");
            if (res.Success)
            {
                ProcessHtml(res.Content, "", DateTime.Today.AddDays(-1), url, sb);
            }

            Console.WriteLine(sb.ToString());
            Console.ReadKey();
        }

        private static bool ProcessHtml(string content, string channel, DateTime lastDate, string oriUrl, StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var res = false;
                List<ScanThread> temp = new List<ScanThread>();

                var m = Regex.Matches(content, ListPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                foreach (System.Text.RegularExpressions.Match item in m)
                {
                    ScanThread tempItem = new ScanThread
                    {
                        Channel = channel,
                        IsDownloaded = 0,
                        Name = FileUtility.ReplaceInvalidChar(item.Groups[4].Value),
                        Url = Prefix + "thread-" + item.Groups[2].Value + ".html"
                    };

                    //Console.WriteLine(string.Format("Add thread {0} of channel {1} url --> {2} Date {3}", tempItem.Name, tempItem.Channel, tempItem.Url, tempItem.ScannedDate));
                    sb.AppendLine(string.Format("    Add thread {0} url --> {1}", tempItem.Name, tempItem.Url));
                    temp.Add(tempItem);
                }

                var threadCount = m.Count;

                m = Regex.Matches(content, ListDatePattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                var dateCount = m.Count;
                var skip = dateCount - threadCount;
                int index = 0;
                int tempIndex = 0;

                sb.AppendLine(string.Format("    临时列表数量: {0}", temp.Count));
                int removeCount = 0;

                foreach (System.Text.RegularExpressions.Match item in m)
                {
                    if (index >= skip)
                    {
                        DateTime tempDate = new DateTime(int.Parse(item.Groups[1].Value), int.Parse(item.Groups[2].Value), int.Parse(item.Groups[3].Value));

                        if (tempDate.Date <= lastDate.Date)
                        {
                            var tempItem = temp[tempIndex];
                            //Console.WriteLine(string.Format("Remove thread {0} of channel {1} url --> {2} Date {3}", tempItem.Name, tempItem.Channel, tempItem.Url, tempItem.ScannedDate));
                            temp.RemoveAt(tempIndex);
                            sb.AppendLine(string.Format("    从列表中移除 -> '{0}' 因为日期{1}小于最后一次扫描日期{2}", tempItem.Name, tempDate.ToString("yyyy-MM-dd"), lastDate.ToString("yyyy-MM-dd")));
                            removeCount++;
                        }
                        else
                        {
                            temp[tempIndex].ScannedDate = DateTime.Today.Date;
                            tempIndex++;
                        }
                    }

                    index++;
                }

                sb.AppendLine(string.Format("    删除: {0}, 剩余: {1}", removeCount, temp.Count));

                foreach (var item in temp)
                {
                    if (!SisDataBaseManager.IsExistScanThread(item))
                    {
                        SisDataBaseManager.InsertScanThread(item);

                        //Console.WriteLine(string.Format("Insert thread {0} of channel {1} url --> {2} Date {3}", item.Name, item.Channel, item.Url, item.ScannedDate));
                        sb.AppendLine(string.Format("    插入帖子 {0} of channel {1} url --> {2} 日期 {3}", item.Name, item.Channel, item.Url, item.ScannedDate));
                        SisDataBaseManager.InsertScanThread(item);
                    }
                    else
                    {
                        sb.AppendLine(string.Format("    已有此贴{0}，不再插入", item.Url));
                    }
                }

                res = temp.Count > 0 ? true : false;
                return res;
            }

            return false;
        }
        #endregion

        #region RegMatch
        private static void StartTestRegMatch()
        {
            var fileList = GetFileNames("C:\\allav.txt");

            var matchList = GetMatch(fileList);

            Console.WriteLine(string.Format("Only one match -> {0}", matchList.Item.Where(x=>x.PossibleId.Count == 1).Count()));
            Console.WriteLine(string.Format("Multiple match -> {0}", matchList.Item.Where(x => x.PossibleId.Count > 1).Count()));

            foreach (var item in matchList.Item.Where(x => x.PossibleId.Count > 1))
            {
                Console.WriteLine(item.Fi);
                item.PossibleId.ToList().ForEach(x => Console.WriteLine(x));
                Console.WriteLine("---------------------------------------");
            }

            Console.WriteLine(string.Format("Total {0}, Subtotal {1}", fileList.Count, matchList.Item.Count));
        }

        private static List<string> GetFileNames(string path)
        {
            List<string> ret = new List<string>();

            using (StreamReader sw = new StreamReader(path))
            {
                while (!sw.EndOfStream)
                {
                    var content = sw.ReadLine();

                    if (content.Contains(" "))
                    {
                        var fullPath = content.Substring(content.IndexOf(" ") + 1);
                        ret.Add(fullPath.Substring(fullPath.LastIndexOf("\\") + 1));
                    }
                }
            }

            return ret;
        }

        private static MatchPair GetMatch(List<string> list)
        {
            MatchPair ret = new MatchPair();
            List<MatchPairItem> retList = new List<MatchPairItem>();
            string regx = @"([a-zA-Z]{1,8}\s*-?\s*\d{1,8})";

            foreach (var item in list)
            {
                FileInfo fi = new FileInfo(item);              
                HashSet<string> hs = new HashSet<string>();
                MatchPairItem mpi = new MatchPairItem();

                var m = Regex.Matches(fi.Name.Replace(fi.Extension, ""), regx, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                foreach (System.Text.RegularExpressions.Match match in m)
                {
                    var firstTime = true;
                    var charArray = match.Value.Replace("-", "").ToCharArray();
                    StringBuilder sb = new StringBuilder();
                    
                    foreach (var c in charArray)
                    {
                        if (firstTime && Char.IsDigit(c))
                        {
                            sb.Append("-");
                            sb.Append(c);
                            firstTime = false;
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                    hs.Add(sb.ToString().ToUpper().Trim());
                }

                mpi.Fi = fi;
                mpi.PossibleId = hs;
                retList.Add(mpi);
            }

            ret.Item = retList;
            return ret;
        }

        private static List<string> GetPreFix()
        {
            return FileUtility.GetPrefix(JavDataBaseManager.GetAllAV());
        }
        #endregion
    }
}

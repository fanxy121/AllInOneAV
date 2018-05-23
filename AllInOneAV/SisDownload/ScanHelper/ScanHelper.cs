using DataBaseManager.SisDataBaseHelper;
using Model.SisModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace SisDownload.ScanHelper
{
    public class ScanHelper
    {
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

        public static void Init(StringBuilder sb)
        {
            if (!Directory.Exists(RootFolder))
            {
                sb.AppendLine(string.Format("没有找到{0},创建文件夹", RootFolder));
                Directory.CreateDirectory(RootFolder);
            }

            var lastOperationEndDate = SisDataBaseManager.GetLastOperationEndDate();
            sb.AppendLine(string.Format("上次执行时间: {0}", lastOperationEndDate.ToString("yyyy-MM-dd")));

            List<string> listChannel = new List<string>();
            listChannel.Add(AsiaUncensoredAuthorshipSeed);
            sb.AppendLine(string.Format("添加频道: {0}", "AsiaUncensoredAuthorshipSeed"));
            listChannel.Add(AsiaUncensoredSection);
            sb.AppendLine(string.Format("添加频道: {0}", "AsiaUncensoredSection"));
            listChannel.Add(WesternUncensoredAuthorshipSeed);
            sb.AppendLine(string.Format("添加频道: {0}", "WesternUncensoredAuthorshipSeed"));
            listChannel.Add(WesternUncensored);
            sb.AppendLine(string.Format("添加频道: {0}", "WesternUncensored"));
            listChannel.Add(AsiaCensoredAuthorshipSeed);
            sb.AppendLine(string.Format("添加频道: {0}", "AsiaCensoredAuthorshipSeed"));
            listChannel.Add(AsiaCensoredSection);
            sb.AppendLine(string.Format("添加频道: {0}", "AsiaCensoredSection"));

            foreach (var channel in listChannel)
            {
                var needContinue = true;
                var page = 1;

                while (needContinue)
                {
                    var url = string.Format(channel, page);
                    Console.WriteLine("Get content from " + string.Format(channel, page));
                    sb.AppendLine(string.Format("正在处理URL: {0}, 页码: {1}", url, page));
                    var res = HtmlManager.GetHtmlContentViaUrl(url, "gbk");

                    if (res.Success)
                    {
                        sb.AppendLine("    URL内容获取成功");
                        needContinue = GetTargetThread(res.Content, ChannelMapping[channel], lastOperationEndDate, string.Format(channel, page), sb);
                    }
                    else
                    {
                        sb.AppendLine("    URL内容获取失败");
                    }

                    sb.AppendLine("*******************************************************************************");

                    page++;
                }
            }
        }

        private static bool GetTargetThread(string content, string channel, DateTime lastDate, string url, StringBuilder sb)
        {
            var ret = ProcessHtml(content, channel, lastDate, url, sb);

            return ret;
        }

        private static bool ProcessHtml(string content, string channel, DateTime lastDate, string oriUrl, StringBuilder sb)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var res = false;
                List<ScanThread> temp = new List<ScanThread>();

                var m = Regex.Matches(content, ListPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                foreach (Match item in m)
                {
                    ScanThread tempItem = new ScanThread
                    {
                        Channel = channel,
                        IsDownloaded = 0,
                        Name = FileUtility.ReplaceInvalidChar(item.Groups[4].Value),
                        Url = Prefix + "thread-" + item.Groups[2].Value + ".html"
                    };

                    Console.WriteLine(string.Format("Add thread {0} of channel {1} url --> {2} Date {3}", tempItem.Name, tempItem.Channel, tempItem.Url, tempItem.ScannedDate));
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

                foreach (Match item in m)
                {
                    if (index >= skip)
                    {
                        DateTime tempDate = new DateTime(int.Parse(item.Groups[1].Value), int.Parse(item.Groups[2].Value), int.Parse(item.Groups[3].Value));

                        if (tempDate.Date <= lastDate.Date)
                        {
                            var tempItem = temp[tempIndex];
                            Console.WriteLine(string.Format("Remove thread {0} of channel {1} url --> {2} Date {3}", tempItem.Name, tempItem.Channel, tempItem.Url, tempItem.ScannedDate));
                            temp.RemoveAt(tempIndex);
                            sb.AppendLine(string.Format("    从列表中移除{0}因为日期{1}小于最后一次扫描日期{2}", tempItem.Url, tempItem.ScannedDate.ToString("yyyy-MM-dd"), lastDate.ToString("yyyy-MM-dd")));
                            res = false;
                            removeCount++;
                        }
                        else
                        {
                            temp[tempIndex].ScannedDate = DateTime.Today.Date;
                            tempIndex++;
                            res = true;
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

                        Console.WriteLine(string.Format("Insert thread {0} of channel {1} url --> {2} Date {3}", item.Name, item.Channel, item.Url, item.ScannedDate));
                        sb.AppendLine(string.Format("    插入帖子 {0} of channel {1} url --> {2} 日期 {3}", item.Name, item.Channel, item.Url, item.ScannedDate));
                        SisDataBaseManager.InsertScanThread(item);
                    }
                    else
                    {
                        sb.AppendLine(string.Format("    已有此贴{0}，不再插入", item.Url));
                    }
                }

                return res;
            }

            return false;
        }
    }
}

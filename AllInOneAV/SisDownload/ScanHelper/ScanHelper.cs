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

        public static void Init()
        {
            if (!Directory.Exists(RootFolder))
            {
                Directory.CreateDirectory(RootFolder);
            }

            var lastOperationEndDate = SisDataBaseManager.GetLastOperationEndDate();

            List<string> listChannel = new List<string>();
            listChannel.Add(AsiaUncensoredAuthorshipSeed);
            listChannel.Add(AsiaUncensoredSection);
            listChannel.Add(WesternUncensoredAuthorshipSeed);
            listChannel.Add(WesternUncensored);
            listChannel.Add(AsiaCensoredAuthorshipSeed);
            listChannel.Add(AsiaCensoredSection);

            foreach (var channel in listChannel)
            {
                var needContinue = true;
                var page = 1;

                while (needContinue)
                {
                    var url = string.Format(channel, page);
                    Console.WriteLine("Get content from " + string.Format(channel, page));
                    var res = HtmlManager.GetHtmlContentViaUrl(url, "gbk");

                    if (res.Success)
                    {
                        needContinue = GetTargetThread(res.Content, ChannelMapping[channel], lastOperationEndDate, string.Format(channel, page));
                    }

                    page++;
                }
            }
        }

        private static bool GetTargetThread(string content, string channel, DateTime lastDate, string url)
        {
            var ret = ProcessHtml(content, channel, lastDate, url);

            return ret;
        }

        private static bool ProcessHtml(string content, string channel, DateTime lastDate, string oriUrl)
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
                    temp.Add(tempItem);
                }

                var threadCount = m.Count;

                m = Regex.Matches(content, ListDatePattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                var dateCount = m.Count;
                var skip = dateCount - threadCount;
                int index = 0;
                int tempIndex = 0;

                foreach (Match item in m)
                {
                    if (index >= skip)
                    {
                        DateTime tempDate = new DateTime(int.Parse(item.Groups[1].Value), int.Parse(item.Groups[2].Value), int.Parse(item.Groups[3].Value));

                        if (tempDate <= lastDate)
                        {
                            var tempItem = temp[tempIndex];
                            Console.WriteLine(string.Format("Remove thread {0} of channel {1} url --> {2} Date {3}", tempItem.Name, tempItem.Channel, tempItem.Url, tempItem.ScannedDate));
                            temp.RemoveAt(tempIndex);
                            res = false;
                        }
                        else
                        {
                            temp[tempIndex].ScannedDate = tempDate;
                            tempIndex++;
                            res = true;
                        }
                    }

                    index++;
                }

                foreach (var item in temp)
                {
                    Console.WriteLine(string.Format("Insert thread {0} of channel {1} url --> {2} Date {3}", item.Name, item.Channel, item.Url, item.ScannedDate));
                    if (!SisDataBaseManager.IsExistScanThread(item))
                    {
                        SisDataBaseManager.InsertScanThread(item);
                    }
                }

                return res;
            }

            return false;
        }
    }
}

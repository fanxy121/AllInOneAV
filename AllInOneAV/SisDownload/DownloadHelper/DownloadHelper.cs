using DataBaseManager.SisDataBaseHelper;
using Model.SisModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace SisDownload.DownloadHelper
{
    public class DownloadHelper
    {
        private static string DetailImg = JavINIClass.IniReadValue("Sis", "DetailImg");
        private static string DetailAttach = JavINIClass.IniReadValue("Sis", "DetailAttach");
        private static string DetailAttachPrefix = JavINIClass.IniReadValue("Sis", "DetailAttachPrefix");
        private static string RootFolder = JavINIClass.IniReadValue("Sis", "root");
        private static string Prefix = JavINIClass.IniReadValue("Sis", "Prefix");

        public static DateTime Start()
        {
            var targets = SisDataBaseManager.GetScanThread().Where(x => x.IsDownloaded != 1).OrderBy(x => x.Channel);
            DateTime res = DateTime.Today;

            foreach(var item in targets)
            {
                res = DoDownload(item);
            }

            return res;
        }

        public static DateTime DoDownload(ScanThread st)
        {
            return DoParse(st);
        }

        public static DateTime DoParse(ScanThread st)
        {
            var url = st.Url;
            var res = HtmlManager.GetHtmlContentViaUrl(url, "gbk");
            DateTime today = DateTime.Today;

            if (res.Success)
            {
                string subFolder = today.ToString("yyyy年MM月dd日") + "/" + st.Channel + "/";

                if (!string.IsNullOrEmpty(res.Content))
                {
                    var m = Regex.Matches(res.Content, DetailAttach, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    var attachFolder = "";
                    var attachName = "";
                    var innerSubFolder = "";

                    foreach (Match item in m)
                    {
                        attachFolder = FileUtility.ReplaceInvalidChar(st.Name);
                        attachName = FileUtility.ReplaceInvalidChar(st.Name) + ".torrent";

                        innerSubFolder = subFolder + attachFolder + "/";

                        if (!Directory.Exists(RootFolder + innerSubFolder))
                        {
                            Directory.CreateDirectory(RootFolder + innerSubFolder);
                        }

                        var attach = Prefix + DetailAttachPrefix + item.Groups[1].Value;
                        var path = RootFolder + subFolder + attachName;

                        Console.WriteLine(string.Format("Download {0} to {1} and create folder {2} for picture", attach, path, innerSubFolder));
                        Utils.DownloadHelper.DownloadFile(attach, path);

                        var ps = Regex.Matches(res.Content, DetailImg, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        int index = 1;

                        foreach (Match p in ps)
                        {
                            if (p.Groups[1].Value.ToLower().StartsWith("http"))
                            {
                                var pic = p.Groups[1].Value;
                                var picPath = RootFolder + innerSubFolder + index + ".jpg";
                                Console.WriteLine(string.Format("Download Picture {0} to {1}", pic, picPath));
                                Utils.DownloadHelper.DownloadFile(pic, picPath);
                                index++;
                            }
                        }

                        SisDataBaseManager.UpdateDownload(url);
                    }
                }
            }

            return today;
        }
    }
}

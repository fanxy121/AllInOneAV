using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.SisDataBaseHelper;
using Microsoft.Win32;
using Model.Common;
using Model.ScanModels;
using Model.SisModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace UnitTest
{
    public class Program
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
            //StartTestRegMatch();

            //var list = GetPreFix();

            //ChromeCookie("jav");
            //OpenBrowserUrl("www.javlibrary.com/cn");

            //var res = GetHtmlContentViaUrl("http://www.javlibrary.com/cn/genres.php", "utf-8", true, null);
        }

        public static void OpenBrowserUrl(string url)
        {
             try
             {
                 // 64位注册表路径
                 var openKey = @"SOFTWARE\Wow6432Node\Google\Chrome";
                 if (IntPtr.Size == 4)
                 {
                     // 32位注册表路径
                     openKey = @"SOFTWARE\Google\Chrome";
                 }
                 RegistryKey appPath = Registry.LocalMachine.OpenSubKey(openKey);
                 // 谷歌浏览器就用谷歌打开，没找到就用系统默认的浏览器
                 // 谷歌卸载了，注册表还没有清空，程序会返回一个"系统找不到指定的文件。"的bug
                 if (appPath != null)
                 {
                     var result = Process.Start("chrome.exe", url);  
                 }
                 else
                 {
                     var result = Process.Start("chrome.exe", url);
                 }
             }
             catch
             {
                 
             }
         }

        public static void ChromeCookie(string hostName)
        {
            if (hostName == null) throw new ArgumentNullException("hostName");

            var dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cookies";
            if (!System.IO.File.Exists(dbPath)) throw new System.IO.FileNotFoundException("Cant find cookie store", dbPath); // race condition, but i'll risk it

            var connectionString = "Data Source=" + dbPath + ";pooling=false";

            using (var conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                var prm = cmd.CreateParameter();
                prm.ParameterName = "hostName";
                prm.Value = hostName;
                cmd.Parameters.Add(prm);

                cmd.CommandText = "SELECT name,encrypted_value FROM cookies WHERE host_key like '%" + hostName + "%'";

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader[0];
                        var encryptedData = (byte[])reader[1];
                        var decodedData = System.Security.Cryptography.ProtectedData.Unprotect(encryptedData, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
                        var plainText = Encoding.ASCII.GetString(decodedData); // Looks like ASCII
                    }
                }
                conn.Close();
            }
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

        private static HtmlResponse GetHtmlContentViaUrl(string url, string end = "utf-8", bool isJav = false, CookieContainer cc = null)
        {
            HtmlResponse res = new HtmlResponse();
            res.Success = false;

            try
            {
                GC.Collect();
                StringBuilder sb = new StringBuilder();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 90000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";//设置User-Agent，伪装成Google Chrome浏览器
                request.Method = "GET";

                if (isJav)
                {
                    request.CookieContainer = cc;
                }

                request.KeepAlive = true;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding(end));
                while (!reader.EndOfStream)
                {
                    sb.AppendLine(reader.ReadLine());
                }
                res.Content = sb.ToString();
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                res.Success = false;
            }

            res.Success = true;
            return res;
        }

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

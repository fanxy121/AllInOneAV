using DataBaseManager.FindDataBaseHelper;
using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.SisDataBaseHelper;
using Microsoft.Win32;
using Model.Common;
using Model.FindModels;
using Model.JavModels;
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
            var ffmpeg = "C:\\Setting\\ffmpeg.exe";
            var file = "D:\\Fin\\ABG-001-金粉奴隷スナイパー 森沢かな.mp4";

            FileUtility.GetThumbnails(file, ffmpeg, "c:/setting/thumnail/", "1", 5, false);

            //Console.ReadKey();
        }

        

        private static void RemoveDuplicate()
        {
            List<AV> needToFixIds = new List<AV>();
            int invalidCount = 0;
            int tureInvalidCount = 0;
            var invalids = FileUtility.GetInvalidChar();
            var avs = JavDataBaseManager.GetAllAV();

            foreach (var av in avs)
            {
                foreach (var invalid in invalids)
                {
                    if (av.Name.Contains(invalid))
                    {
                        needToFixIds.Add(av);
                    }
                }
            }

            foreach (var id in needToFixIds)
            {
                invalidCount++;
                var effectedRows = avs.Where(x => x.ID == id.ID && x.Company == id.Company && x.Directory == id.Directory).ToList();

                if (effectedRows.Count == 1)
                {
                    tureInvalidCount++;
                    var target = effectedRows.FirstOrDefault();
                    target.Name = FileUtility.ReplaceInvalidChar(target.Name);

                    JavDataBaseManager.UpdateInvalid(target);
                }
                else if (effectedRows.Count > 1)
                {
                    tureInvalidCount++;
                    int needToFixCount = 0;
                    List<AV> toBeDeleted = new List<AV>();

                    foreach (var item in effectedRows)
                    {
                        foreach (var invalid in invalids)
                        {
                            if (item.Name.Contains(invalid))
                            {
                                needToFixCount++;
                                toBeDeleted.Add(item);
                            }
                        }
                    }

                    if (needToFixCount == effectedRows.Count)
                    {
                        var orderedList = toBeDeleted.OrderBy(x => x.AvId).ToList();
                        var keep = orderedList.FirstOrDefault();
                        keep.Name = FileUtility.ReplaceInvalidChar(keep.Name);

                        JavDataBaseManager.UpdateInvalid(keep);

                        foreach (var delete in toBeDeleted.Skip(1))
                        {
                            JavDataBaseManager.DeleteInvalid(delete);
                        }
                    }
                    else
                    {
                        foreach (var delete in toBeDeleted)
                        {
                            JavDataBaseManager.DeleteInvalid(delete);
                        }
                    }
                }
            }

            Console.WriteLine("Has --> " + invalidCount + " invalid.... trueInvalidCount " + tureInvalidCount);
            Console.ReadKey();
        }

        private static void DownloadFile(string file, string desc)
        {
            Utils.DownloadHelper.DownloadFile(file, desc);
        }

        private static void GetAVFromIdAndName()
        {
            var folder = "E:\\New folder\\Fin";

            var files = Directory.GetFiles(folder);

            int total = files.Length;
            int match = 0;
            List<string> unmatched = new List<string>();

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                var split = fi.Name.Split('-');

                if (split.Length >= 3)
                {
                    var id = split[0] + "-" + split[1];
                    var nameAfter = fi.Name.Replace(id, "").Split('-');
                    string name = "";

                    if (nameAfter.Length == 0)
                    {
                        name = fi.Name.Replace(id, "").Replace(fi.Extension, "");
                    }
                    else
                    {
                        name = nameAfter[0];

                        foreach (var slice in nameAfter)
                        {
                            if (slice.Length > 1)
                            {
                                name += "-" + slice;
                            }
                        }
                    }

                    var avs = JavDataBaseManager.GetAllAV(id, name);

                    if (avs != null && avs.Count == 1)
                    {
                        match++;
                    }
                    else
                    {
                        unmatched.Add(fi.FullName);
                    }
                }
            }

            foreach (var unmatch in unmatched)
            {
                Console.WriteLine(unmatch);
            }

            Console.WriteLine("Total: " + total + "  Matched: " + match);
        }

        private static void CheckDuplicate()
        {
            var folder = "E:/New folder/Fin";
            var files = Directory.GetFiles(folder);
            List<FileInfo> fis = new List<FileInfo>();

            foreach (var f in files)
            {
                fis.Add(new FileInfo(f));
            }

            foreach (var fi in fis)
            {

            }
        }
    }
}
using DataBaseManager.FindDataBaseHelper;
using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.SisDataBaseHelper;
using Microsoft.Win32;
using Model.Common;
using Model.FindModels;
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
            var fileFolder = "D:/New folder/";
            var allAvName = JavDataBaseManager.GetAllAV().Select(x => x.ID).ToList();
            List<string> allPrefix = new List<string>();
            var files = Directory.GetFiles(fileFolder);
            List<TempReuslt> list = new List<TempReuslt>();
            Dictionary<string, List<TempReuslt>> dic = new Dictionary<string, List<TempReuslt>>();
            FileInfo file = null;
            TempReuslt temp = null;

            foreach (var name in allAvName)
            {
                if (!allPrefix.Contains(name.Split('-')[0]))
                {
                    allPrefix.Add(name.Split('-')[0]);
                }
            }

            allPrefix = allPrefix.OrderByDescending(x => x.Length).ToList();

            foreach (var f in files.OrderBy(x=>x.Length).Take(50))
            {
                file = new FileInfo(f);
                var fName = file.Name.Replace(file.Extension, "");

                foreach (var p in allPrefix)
                {
                    if (fName.Contains(p))
                    {
                        var pattern = p + "{1}-?\\d{1,5}";
                        var match = Regex.Match(fName, pattern);
                    }
                }
            }

            dic = list.GroupBy(x => x.Current).ToDictionary(x => x.Key, x => x.OrderBy(y=>y.Similarity).Take(3).ToList());

            foreach (var d in dic)
            {
                foreach (var s in dic.Values)
                {
                    foreach (var ss in s)
                    {
                        Console.WriteLine("与" + d.Key + " 最接近的为 " + ss.Target);
                    }
                }
            }

            Console.ReadKey();
        }
    }

    public class TempReuslt
    {
        public string Current { get; set; }
        public string Target { get; set; }
        public decimal Similarity { get; set; }
    }
}

﻿using Model.Common;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenerateReport
{
    public class ReportHelper
    {
        public static Dictionary<string, List<FileInfo>> GenerateExistingAVs()
        {
            string folder = "fin\\";
            var drivers = Environment.GetLogicalDrives();
            Dictionary<string, List<FileInfo>> fileContainer = new Dictionary<string, List<FileInfo>>();

            foreach (var driver in drivers)
            {
                string targetFolder = driver + folder;

                if (Directory.Exists(targetFolder))
                {
                    var files = Directory.GetFiles(targetFolder);

                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        var fileSplitName = fi.Name.Split('-');

                        if (fileSplitName.Length >= 3)
                        {
                            var key = fi.Name.Replace("-1", "").Replace("-2", "").Replace("-3", "").Replace("-4", "").Replace("-5", "").Replace("-6", "").Replace("-c", "").Replace("-C", "").Replace(fi.Extension, "");

                            if (fileContainer.ContainsKey(key))
                            {
                                fileContainer[key].Add(fi);
                            }
                            else
                            {
                                fileContainer.Add(key, new List<FileInfo>() { fi });
                            }
                        }
                    }
                }
            }

            return fileContainer;
        }

        public static void GenerateBasicReport(Dictionary<string, List<FileInfo>> input, List<AV> avs, Dictionary<string, AV> match, ReportModel report)
        {
            int totalAV = 0;
            int totalFile = 0;
            int totalMatch = 0;
            long totalSize = 0;
            Dictionary<string, int> MatchedPrefix = new Dictionary<string, int>();
            List<string> prefixs = new List<string>();
            List<string> notMatchAnything = new List<string>();

            foreach (var av in avs)
            {
                if (av.ID.Contains("-"))
                {
                    var pre = av.ID.Split('-')[0];

                    if (!prefixs.Contains(pre))
                    {
                        prefixs.Add(pre);
                    }
                }
            }

            foreach (var av in input)
            {
                totalAV++;

                foreach (var subItem in av.Value)
                {
                    totalFile++;
                    totalSize += subItem.Length;
                }
            }

            foreach (var av in avs)
            {
                var key = (av.ID + "-" + av.Name).Replace("-1", "").Replace("-2", "").Replace("-3", "").Replace("-4", "").Replace("-5", "").Replace("-6", "").Replace("-c", "");
                var pre = av.ID.Split('-')[0];

                if (input.ContainsKey(key))
                {
                    totalMatch++;

                    if (!MatchedPrefix.ContainsKey(pre))
                    {
                        MatchedPrefix.Add(pre, 1);
                    }
                    else
                    {
                        MatchedPrefix[pre]++;
                    }

                    match.Add(key, av);
                }

                if (!prefixs.Contains(pre))
                {
                    prefixs.Add(pre);
                }
            }

            foreach (var pre in prefixs)
            {
                if (!MatchedPrefix.ContainsKey(pre))
                {
                    notMatchAnything.Add(pre);
                }
            }

            report.TotalRecord = avs.Count;
            report.TotalAV = totalAV;
            report.TotalFiles = totalFile;
            report.TotalSizeStr = FileSize.GetAutoSizeString(totalSize, 2);
            report.TotalSizeLong = totalSize;
            report.TotalMatch = totalMatch;
            report.TotalUnMatch = totalAV - totalMatch;
            report.NotMatchAnyThing = notMatchAnything;
        }

        public static void GenerateMatchReport(Dictionary<string, List<FileInfo>> input, List<AV> avs, Dictionary<string, AV> match, ReportModel report)
        {
            List<AV> matchedAV = new List<AV>();
            Dictionary<string, int> matchPrefix = new Dictionary<string, int>();
            Dictionary<string, int> matchActress = new Dictionary<string, int>();
            Dictionary<string, int> matchDirector = new Dictionary<string, int>();
            Dictionary<string, int> matchCategory = new Dictionary<string, int>();
            Dictionary<string, int> matchCompany = new Dictionary<string, int>();
            Dictionary<string, int> matchDate = new Dictionary<string, int>();

            foreach (var item in match)
            {
                var av = item.Value;

                if (!string.IsNullOrEmpty(av.ID) && av.ID.Contains("-"))
                {
                    var prefix = av.ID.Split('-')[0];

                    if (matchPrefix.ContainsKey(prefix))
                    {
                        matchPrefix[prefix]++;
                    }
                    else
                    {
                        matchPrefix.Add(prefix, 1);
                    }
                }

                if (!string.IsNullOrEmpty(av.Actress))
                {
                    var actress = av.Actress.Split(',');

                    foreach (var act in actress.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        if (matchActress.ContainsKey(act))
                        {
                            matchActress[act]++;
                        }
                        else
                        {
                            matchActress.Add(act, 1);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(av.Director))
                {
                    var dirs = av.Director.Split(',');

                    foreach (var dir in dirs.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        if (matchDirector.ContainsKey(dir))
                        {
                            matchDirector[dir]++;
                        }
                        else
                        {
                            matchDirector.Add(dir, 1);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(av.Category))
                {
                    var cats = av.Category.Split(',');

                    foreach (var cat in cats.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        if (matchCategory.ContainsKey(cat))
                        {
                            matchCategory[cat]++;
                        }
                        else
                        {
                            matchCategory.Add(cat, 1);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(av.Company))
                {
                    var coms = av.Company.Split(',');

                    foreach (var com in coms.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        if (matchCompany.ContainsKey(com))
                        {
                            matchCompany[com]++;
                        }
                        else
                        {
                            matchCompany.Add(com, 1);
                        }
                    }
                }

                if (av.ReleaseDate != DateTime.MinValue)
                {
                    var date = av.ReleaseDate.Year + "";

                    if (matchDate.ContainsKey(date))
                    {
                        matchDate[date]++;
                    }
                    else
                    {
                        matchDate.Add(date, 1);
                    }
                }
            }

            report.TopPrefix = matchPrefix.OrderByDescending(s => s.Value).Take(report.Top).ToDictionary(x => x.Key, x => x.Value);
            report.TopActress = matchActress.OrderByDescending(s => s.Value).Take(report.Top).ToDictionary(x => x.Key, x => x.Value);
            report.TopDirctor = matchDirector.OrderByDescending(s => s.Value).Take(report.Top).ToDictionary(x => x.Key, x => x.Value);
            report.TopCategory = matchCategory.OrderByDescending(s => s.Value).Take(report.Top).ToDictionary(x => x.Key, x => x.Value);
            report.TopCompany = matchCompany.OrderByDescending(s => s.Value).Take(report.Top).ToDictionary(x => x.Key, x => x.Value);
            report.TopDate = matchDate.OrderByDescending(s => s.Value).Take(report.Top).ToDictionary(x => x.Key, x => x.Value);
        }

        public static void GenerateOtherReport(Dictionary<string, List<FileInfo>> input, List<AV> avs, Dictionary<string, AV> match, ReportModel report)
        {
            int avHasFileMoreThan1 = 0;
            int fileLargeThan1G = 0;
            int fileLargeThan2G = 0;
            int fileLargeThan4G = 0;
            Dictionary<string, int> formats = new Dictionary<string, int>();


            foreach (var item in input)
            {
                if (item.Value.Count > 1)
                {
                    avHasFileMoreThan1++;
                }

                foreach (var subItem in item.Value)
                {
                    var ext = subItem.Extension.ToLower();

                    if (subItem.Length >= (1L * 1000 * 1000 * 1000))
                    {
                        fileLargeThan1G++;
                    }

                    if (subItem.Length >= (2L * 1000 * 1000 * 1000))
                    {
                        fileLargeThan2G++;
                    }

                    if (subItem.Length >= (4L * 1000 * 1000 * 1000))
                    {
                        fileLargeThan4G++;
                    }

                    if (formats.ContainsKey(ext))
                    {
                        formats[ext]++;
                    }
                    else
                    {
                        formats.Add(ext, 1);
                    }
                }
            }

            report.AvHasMoreThan1File = avHasFileMoreThan1;
            report.FileLargeThan1G = fileLargeThan1G;
            report.FileLargeThan2G = fileLargeThan2G;
            report.FileLargeThan4G = fileLargeThan4G;
            report.Formats = formats;
        }

        public static string GenerateActuralReport(ReportModel report)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("总共收集 {0} 条AV信息, 本地共有 {1} 部AV, 占总信息量的 {2}%, 共有文件 {3} 个, 总大小 {4}", report.TotalRecord, report.TotalAV, GetPercentage(report.TotalFiles, report.TotalRecord), report.TotalFiles, report.TotalSizeStr));
            sb.AppendLine(string.Format("其中一共成功匹配了 {0} 部, 没有成功匹配 {1} 部, 共有 {2} 部AV有不止一张CD", report.TotalMatch, report.TotalUnMatch, report.AvHasMoreThan1File));
            sb.AppendLine(string.Format("有 {0} 个系列在本地没有任何AV文件存在, 文件一共涉及了 {1} 种格式, 分别为 {2}", report.NotMatchAnyThing.Count, report.Formats.Count, string.Join(",", report.Formats.Keys)));
            sb.AppendLine(string.Format("有 {0} 个文件大于1GB占比 {1}%, 有 {2} 个文件大于2GB占比 {3}%, 有 {4} 个文件大于4GB占比 {5}%", report.FileLargeThan1G, GetPercentage(report.FileLargeThan1G, report.TotalFiles), report.FileLargeThan2G, GetPercentage(report.FileLargeThan2G, report.TotalFiles), report.FileLargeThan4G, GetPercentage(report.FileLargeThan4G, report.TotalFiles)));

            sb.AppendLine(string.Format("系列拥有数前{0} -> ", report.Top));
            foreach (var item in report.TopPrefix)
            {
                sb.AppendLine(string.Format("\t{0} -> {1}次", item.Key, item.Value));
            }

            sb.AppendLine(string.Format("女优拥有数前{0} -> ", report.Top));
            foreach (var item in report.TopActress)
            {
                sb.AppendLine(string.Format("\t{0} -> {1}次", item.Key, item.Value));
            }

            sb.AppendLine(string.Format("导演拥有数前{0} -> ", report.Top));
            foreach (var item in report.TopDirctor)
            {
                sb.AppendLine(string.Format("\t{0} -> {1}次", item.Key, item.Value));
            }
 
            sb.AppendLine(string.Format("类型拥有数前{0} -> ", report.Top));
            foreach (var item in report.TopCategory)
            {
                sb.AppendLine(string.Format("\t{0} -> {1}次", item.Key, item.Value));
            }

            sb.AppendLine(string.Format("制作公司拥有数前{0} -> ", report.Top));
            foreach (var item in report.TopCompany)
            {
                sb.AppendLine(string.Format("\t{0} -> {1}次", item.Key, item.Value));
            }

            sb.AppendLine(string.Format("发行日期拥有数前{0} -> ", report.Top));
            foreach (var item in report.TopDate)
            {
                sb.AppendLine(string.Format("\t{0} -> {1}次", item.Key, item.Value));
            }

            return sb.ToString();
        }

        public static void Test(Dictionary<string, List<FileInfo>> input, List<AV> avs, Dictionary<string, AV> match, ReportModel report)
        {
            foreach (var item in input)
            {
                if (avs.Find(x => (x.ID + "-" + x.Name).Replace("-1", "").Replace("-2", "").Replace("-3", "").Replace("-4", "").Replace("-5", "").Replace("-6", "").Replace("-c", "").Equals(item.Key, StringComparison.OrdinalIgnoreCase)) != null)
                {

                }
                else
                {
                    Console.WriteLine(item.Key);
                }
            }
        }

        public static string GetPercentage(int divied, int divide, int round = 1)
        {
            if (divide == 0)
            {
                return "0";
            }
            else
            {
                var percentage = Math.Round(((decimal)divied / (decimal)divide) * 100, round);

                return percentage + "";
            }
        }
    }
}

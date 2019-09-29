using Model.JavModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class FileUtility
    {
        public static string GetFilesRecursive(string folder, List<string> formats, List<string> excludes, List<FileInfo> res, int minSize = 0)
        {
            try
            {
                var files = Directory.GetFiles(folder);
                var dirs = Directory.GetDirectories(folder);

                foreach (var file in files)
                {
                    var f = new FileInfo(file);

                    if (formats.Contains(f.Extension.ToLower()))
                    {
                        if (minSize > 0)
                        {
                            if (f.Length >= minSize * 1024 * 1024)
                            {
                                res.Add(f);
                            }
                        }
                        else
                        {
                            res.Add(f);
                        }
                    }
                }

                foreach (var dir in dirs)
                {
                    if (!excludes.Contains(dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)))
                    {
                        GetFilesRecursive(dir, formats, excludes, res, minSize);
                    }
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            return "";
        }

        public static string ReadFile(string location)
        {
            StringBuilder sb = new StringBuilder();

            if (File.Exists(location))
            {
                StreamReader sr = new StreamReader(location);

                while (!sr.EndOfStream)
                {
                    sb.Append(sr.ReadLine());
                }

                sr.Close();
            }

            return sb.ToString();
        }

        public static void WriteFile(string content, string location)
        {
            if (!File.Exists(location))
            {
                File.Create(location).Close();
            }

            StreamWriter sw = new StreamWriter(location);

            sw.WriteLine(content);

            sw.Close();
        }

        public static List<string> GetPrefix(List<AV> avs)
        {
            List<string> res = new List<string>();

            foreach (var av in avs)
            {
                var temp = av.ID.Substring(0, av.ID.IndexOf("-"));

                if (!res.Contains(temp))
                {
                    res.Add(temp);
                }
            }

            return res;
        }

        public static string GetSuggestName(string ori, string pre)
        {
            int index = ori.IndexOf(pre) + pre.Length;
            var subStr = ori.Substring(index);

            if (subStr.Length > 0)
            {
                bool hasDash = subStr[0] == '-' ? true : false;
                int newIndex = 0;
                StringBuilder sb = new StringBuilder();

                if (hasDash)
                {
                    newIndex = index + 1;
                }
                else
                {
                    newIndex = index;
                }

                for (int i = newIndex; i < ori.Length; i++)
                {
                    if (ori[i] >= '0' && ori[i] <= '9')
                    {
                        sb.Append(ori[i].ToString());
                    }
                    else
                    {
                        break;
                    }
                }
                return pre + "-" + sb.ToString();
            }

            return pre + "-";
        }

        public static List<string> GetPossibleID(Scan scan, List<string> prefix)
        {
            List<AV> res = new List<AV>();
            List<string> possibleID = new List<string>();

            foreach (var pre in prefix.OrderByDescending(x=>x.Length))
            {
                var fileName = scan.FileName.Substring(0, scan.FileName.LastIndexOf("."));

                if (fileName.Contains(pre))
                {
                    var possible = GetSuggestName(fileName, pre);

                    if (!string.IsNullOrEmpty(possible.Split('-')[1]))
                    {
                        possibleID.Add(possible);
                    }
                }
            }

            return possibleID;
        }

        public static void GetSupportFiles(string folder, List<string> excludes, List<string> formats, List<FileInfo> res)
        {
            var files = Directory.GetFiles(folder);
            var dirs = Directory.GetDirectories(folder);

            foreach (var file in files)
            {
                var f = new FileInfo(file);

                if (formats.Contains(f.Extension.ToLower()) && f.Length >= 1000 * 1000 * 100 * 3)
                {
                    res.Add(f);
                }
            }

            foreach (var dir in dirs)
            {
                if (!excludes.Contains(dir.Substring(dir.LastIndexOf(Path.DirectorySeparatorChar) + 1)))
                {
                    GetSupportFiles(dir, excludes, formats, res);
                }
            }
        }

        public static string ReplaceInvalidChar(string str)
        {
            return str.Replace("'","''").Replace("?","").Replace(":","").Replace("*","").Replace("|","").Replace("\\","").Replace("/", "").Replace("<", "").Replace(">", "");
        }

        public static string GetImageFile(string folder, AV av)
        {
            var file = folder + av.ID + av.Name + ".jpg";

            if (File.Exists(file))
            {
                return file;
            }
            else
            {
                return "";
            }
        }

        public static decimal GetCloseRation(string current, string target)
        {
            int len1 = current.Length;
            var len2 = target.Length;

            int[,] dif = new int[len1 + 1, len2 + 1];

            for (int i = 0; i < len1; i++)
            {
                dif[i, 0] = i;
            }

            for (int i = 0; i < len2; i++)
            {
                dif[0, i] = i;
            }

            int temp;

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (current[i - 1] == target[j - 1])
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }

                    dif[i, j] = min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1, dif[i - 1, j] + 1);
                }
            }

            var diff = (decimal)dif[len1, len2] / Math.Max(current.Length, target.Length);
            var similarity = (decimal)1 - diff;

            return similarity;
        }

        private static int min(int a, int b, int c)
        {
            return Math.Min(Math.Min(a, b), c);
        }
    }
}

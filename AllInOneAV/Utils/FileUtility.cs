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

            foreach (var pre in prefix)
            {
                var fileName = scan.FileName.ToUpper().Substring(0, scan.FileName.LastIndexOf("."));

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
            return str.Replace("'","''").Replace("?","").Replace(":","").Replace("*","").Replace("|","");
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
    }
}

using Model.JavModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static string SecondToHour(double time)
        {
            string str = "";
            int hour = 0;
            int minute = 0;
            int second = 0;
            second = Convert.ToInt32(time);

            if (second > 60)
            {
                minute = second / 60;
                second = second % 60;
            }
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            return (hour + "小时" + minute + "分钟"
                + second + "秒");
        }

            public static List<string> GetInvalidChar()
        {
            List<string> ret = new List<string>();

            ret.Add("'");
            ret.Add("?");
            ret.Add(":");
            ret.Add("*");
            ret.Add("|");
            ret.Add("\\");
            ret.Add("/");
            ret.Add("<");
            ret.Add(">");

            return ret;
        }

        public static void ExcuteProcess(string exe, string arg, DataReceivedEventHandler output)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = arg;

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += output;
                p.ErrorDataReceived += output;

                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();
            }
        }

        public static string GetFileName(string fullName, bool extension)
        {
            FileInfo f = new FileInfo(fullName);
            if (extension)
            {
                return f.Name;
            }
            else
            {
                return f.Name.Replace(f.Extension, "").ToLower();
            }
        }

        public static string GetDuration(string fName, string ffmpegLocation)
        {
            string duration = "";

            try
            {
                string fullName = fName;
                string fileName = GetFileName(fName, false);
                string command_line = " -i \"" + fullName + "\"";

                ExcuteProcess(ffmpegLocation, command_line, (s, t) => duration += (t.Data));

                duration = duration.Substring(duration.IndexOf("Duration") + 10);
                duration = duration.Substring(0, duration.IndexOf(","));

                var hour = int.Parse(duration.Split(':')[0]);
                var min = int.Parse(duration.Split(':')[1]);
                var sec = int.Parse(duration.Split(':')[2].Substring(0, duration.Split(':')[2].IndexOf(".")));

                
            }
            catch (Exception)
            {
                return "-";
            }
            return duration;
        }

        public static int GetThumbnails(string fName, string ffmpegLocation, string whereToSave, string subFolder, int howManyPictures, bool size, int width = 320, int height = 240)
        {
            int result = 0;

            try
            {
                string fullName = fName;
                string fileName = GetFileName(fName, false);
                string command_line = " -i \"" + fullName + "\"";
                string duration = "";

                if (!Directory.Exists(whereToSave))
                {
                    Directory.CreateDirectory(whereToSave);
                }

                ExcuteProcess(ffmpegLocation, command_line, (s, t) => duration += (t.Data));

                duration = duration.Substring(duration.IndexOf("Duration") + 10);
                duration = duration.Substring(0, duration.IndexOf(","));

                var hour = int.Parse(duration.Split(':')[0]);
                var min = int.Parse(duration.Split(':')[1]);
                var sec = int.Parse(duration.Split(':')[2].Substring(0, duration.Split(':')[2].IndexOf(".")));

                var totalSec = hour * 3600 + min * 60 + sec;

                var diff = totalSec / (howManyPictures + 1);

                List<int> frame = new List<int>();

                for (int i = 1; i <= howManyPictures; i++)
                {
                    frame.Add(diff * i);
                }

                int currentIndex = 1;

                foreach (var item in frame)
                {
                    if (File.Exists(whereToSave + subFolder + "-" + currentIndex + ".jpg"))
                    {
                        File.Delete(whereToSave + subFolder + "-" + currentIndex + ".jpg");
                    }

                    string screenLine = "";

                    if (size)
                    {
                        screenLine = "-ss " + item + " -i \"" + fullName + "\" -s " + width + "x" + height + " -vframes 1 \"" + whereToSave + subFolder + "-" + currentIndex + ".jpg\"";
                    }
                    else
                    {
                        screenLine = "-ss " + item + " -i \"" + fullName + "\" -vframes 1 \"" + whereToSave + subFolder + "-" + currentIndex + ".jpg\"";
                    }

                    result++;
                    currentIndex++;

                    ExcuteProcess(ffmpegLocation, screenLine, (s, t) => duration = (t.Data));
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return result;
        }

        public static Dictionary<string, List<FileInfo>> GetCheckDuplicatedData(string folder)
        {
            Dictionary<string, List<FileInfo>> res = new Dictionary<string, List<FileInfo>>();

            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder);

                foreach (var file in files)
                {
                    var split = file.Split('-');
                    if (split.Length >= 3)
                    {
                        var key = split[0] + "-" + split[1];

                        if (res.ContainsKey(key))
                        {
                            res[key].Add(new FileInfo(file));
                        }
                        else
                        {
                            res.Add(key, new List<FileInfo> { new FileInfo(file) });
                        }
                    }
                }
            }
            return res;
        }

        public static double GetVideoDuration(string file)
        {
            MediaPlayer.MediaPlayer media = new MediaPlayer.MediaPlayer();
            media.Open(file);

            Thread.Sleep(600);

            var duration = media.Duration;

            return duration;
        }

        public static void PlayVideo(string file)
        {
            System.Diagnostics.Process.Start(@"" + file);
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

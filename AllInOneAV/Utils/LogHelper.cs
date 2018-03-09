using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class LogHelper
    {
        public static string root = "C:/AvLog/";

        public static void WriteLog(string logFile, string content)
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            if (!File.Exists(logFile))
            {
                File.Create(logFile);
            }

            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine(content);
            sw.Close();
        }
    }
}

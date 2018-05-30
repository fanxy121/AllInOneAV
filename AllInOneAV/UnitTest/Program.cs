using Model.Common;
using Model.ScanModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime d1 = DateTime.Now.AddSeconds(-2);
            DateTime d2 = DateTime.Now.AddMinutes(-2);
            DateTime d3 = DateTime.Now.AddHours(-2);
            DateTime d4 = DateTime.Now.AddMonths(-2);
            DateTime d5 = DateTime.Now.AddYears(-2);

            Console.WriteLine(GetStr(d1));
            Console.WriteLine(GetStr(d2));
            Console.WriteLine(GetStr(d3));
            Console.WriteLine(GetStr(d4));
            Console.WriteLine(GetStr(d5));

            Console.ReadKey();
        }

        public static string GetStr(DateTime item)
        {
            var now = DateTime.Now;
            var dateDiff = (now - item);

            if (dateDiff.TotalSeconds <= 60)
            {
                return("刚刚");
            }
            else if (dateDiff.TotalMinutes <= 60)
            {
                return (string.Format("{0}分钟前", (int)dateDiff.TotalMinutes));
            }
            else if (dateDiff.TotalHours <= 24)
            {
                return (string.Format("{0}小时前", (int)dateDiff.TotalHours));
            }
            else if (item.Year == now.Year)
            {
                return (string.Format("{0}", item.ToString("MM-dd hh:mm")));
            }
            else
            {
                return (string.Format("{0}", item.ToString("yyyy-MM-dd hh:mm")));
            }
        }
    }
}

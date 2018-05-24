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
            StreamReader sr = new StreamReader("c:\\AvLogScanJson2018-05-24-10-31-22.json", Encoding.UTF8);
            var str = sr.ReadLine();
            sr.Close();

            Console.WriteLine(str);
            Console.ReadKey();

            var list = JsonConvert.DeserializeObject<List<DuplicateItem>>(str);

            Console.ReadKey();
        }
    }
}

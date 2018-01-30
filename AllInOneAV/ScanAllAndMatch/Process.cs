using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Model.JavModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ScanAllAndMatch
{
    public class Process
    {
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();

        public static void Start()
        {
            ScanDataBaseManager.ClearMatch();
            ScanDataBaseManager.DeleteFinish();
            var drivers = Environment.GetLogicalDrives().Skip(1).ToList();
            List<FileInfo> fi = new List<FileInfo>();

            foreach(var driver in drivers)
            {
                FileUtility.GetFilesRecursive(driver, formats, excludes, fi, 100);
            }

            var avs = JavDataBaseManager.GetAllAV();
            var prefix = FileUtility.GetPrefix(avs);

            foreach (var file in fi)
            {
                var scan = new Scan
                {
                    FileName = file.Name,
                    Location = file.FullName,
                    Size = FileSize.GetAutoSizeString(file.Length, 2)
                };

                var possibleIDs = FileUtility.GetPossibleID(scan, prefix);

                MatchAndSave(scan, possibleIDs);
            }

            ScanDataBaseManager.InsertFinish();
        }

        private static void MatchAndSave(Scan scan, List<string> possibleIDs)
        {
            foreach (var id in possibleIDs)
            {
                var avs = JavDataBaseManager.GetAllAV(id);

                foreach(var av in avs)
                { 
                    ScanDataBaseManager.SaveMatch(scan, av);
                }
            }
        }
    }
}

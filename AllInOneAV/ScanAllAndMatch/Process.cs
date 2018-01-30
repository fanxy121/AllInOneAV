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
            var drivers = Environment.GetLogicalDrives().Skip(1).ToList();
            List<FileInfo> fi = new List<FileInfo>();
            List<Match> temp = new List<Match>();

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

                AddTemp(scan, possibleIDs, temp);
            }

            var currentMatchs = ScanDataBaseManager.GetAllMatch();

            foreach (var m in currentMatchs)
            {
                m.AvID = m.AvID.Trim();
                m.Location = m.Location.Trim();
                m.Name = m.Name.Trim();
            }

            var shouldDelete = currentMatchs.Except(temp, new MatchComparer());
            var shouldAdd = temp.Except(currentMatchs, new MatchComparer());

            var cd = shouldDelete.Count();
            var ca = shouldAdd.Count();


            foreach (var m in shouldDelete)
            {
                ScanDataBaseManager.DeleteMatch(m.AvID);
            }

            foreach (var m in shouldAdd)
            {
                ScanDataBaseManager.SaveMatch(m);
            }

            ScanDataBaseManager.InsertFinish();
        }

        private static void AddTemp(Scan scan, List<string> possibleIDs, List<Match> temp)
        {
            foreach (var id in possibleIDs)
            {
                var avs = JavDataBaseManager.GetAllAV(id);

                foreach(var av in avs)
                {
                    temp.Add(new Match
                    {
                        AvID = av.ID,
                        Location = scan.Location,
                        Name = scan.FileName
                    });
                }
            }
        }
    }
}

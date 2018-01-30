using DataBaseManager.SisDataBaseHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisDownload.Process
{
    public class Process
    {
        public static void Start()
        {
            ScanHelper.ScanHelper.Init();
            SisDataBaseManager.InsertLastOperationEndDate(DownloadHelper.DownloadHelper.Start());
        }
    }
}

using DataBaseManager.SisDataBaseHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SisDownload.Process
{
    public class Process
    {
        public static void Start()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                ScanHelper.ScanHelper.Init(sb);
                SisDataBaseManager.InsertLastOperationEndDate(DownloadHelper.DownloadHelper.Start());
            }
            catch (Exception e)
            {
                sb.AppendLine(e.ToString());
            }
            finally
            {
                var file = "SisDownload" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "-log.txt";
                LogHelper.WriteLog(file, sb.ToString());

                EmailHelper.SendEmail("SisDownloadLog", "详情见附件", new[] { "cainqs@outlook.com" }, new[] { file });
            }
        }
    }
}

using DataBaseManager.Common;
using Model.FindModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace DataBaseManager.FindDataBaseHelper
{
    public class FindDataBaseManager
    {
        private static string con;
        private static SqlConnection mycon;

        static FindDataBaseManager()
        {
            con = string.Format("Server={0};Database={1};Trusted_Connection=SSPI", JavINIClass.IniReadValue("Scan", "server"), JavINIClass.IniReadValue("Scan", "db"));
            mycon = new SqlConnection(con);
        }

        public static List<Match> GetAllMovies()
        {
            var sql = @"SELECT * FROM Match";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<Match>();
        }
    }
}

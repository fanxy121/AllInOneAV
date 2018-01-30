using DataBaseManager.Common;
using Model.JavModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataBaseManager.ScanDataBaseHelper
{
    public class ScanDataBaseManager
    {
        private static string con;
        private static SqlConnection mycon;

        static ScanDataBaseManager()
        {
            con = string.Format("Server={0};Database={1};Trusted_Connection=SSPI", JavINIClass.IniReadValue("Scan", "server"), JavINIClass.IniReadValue("Scan", "db"));
            mycon = new SqlConnection(con);
        }

        public static int ClearMatch()
        {
            var sql = @"DELETE FROM Match";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int DeleteMatch(string location)
        {
            var sql = @"DELETE FROM Match WHERE Location = '" + location + "'";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int SaveMatch(Scan scan, AV av)
        {
            var sql = @"INSERT INTO Match (AvID, Name, Location, CreateTime) VALUES (@avID, @name, @location, @createTime)";

            SqlParameter[] paras = {
                new SqlParameter("@avID",SqlDbType.NVarChar,100),
                new SqlParameter("@name",SqlDbType.NVarChar,500),
                new SqlParameter("@location",SqlDbType.NVarChar,1000),
                new SqlParameter("@createTime",SqlDbType.DateTime),
            };

            paras[0].Value = av.ID;
            paras[1].Value = scan.FileName;
            paras[2].Value = scan.Location;
            paras[3].Value = DateTime.Now;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql, paras);
        }

        public static int DeleteFinish()
        {
            var sql = @"DELETE FROM Finish";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertFinish()
        {
            var sql = @"INSERT INTO Finish (IsFinish) VALUES (1)";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static bool IsFinish()
        {
            var sql = @"SELECT COUNT(1) FROM Finish";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<Finish>().IsFinish == 0 ? false : true;
        }

        public static int InsertViewHistory(string file)
        {
            var sql = string.Format("INSERT INTO ViewHistory (FileName) VALUES ('{0}')", file);

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static bool ViewedFile(string file)
        {
            var sql = string.Format("SELECT * FROM ViewHistory WHERE FileName = '{0}'", file);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<ViewHistory>() == null ? false : true;
        }
    }
}

using DataBaseManager.Common;
using Model.FindModels;
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
            con = string.Format("Server={0};Database={1};User=sa;password=19880118Qs123!", JavINIClass.IniReadValue("Scan", "server"), JavINIClass.IniReadValue("Scan", "db"));
            mycon = new SqlConnection(con);
        }

        public static int ClearMatch()
        {
            var sql = @"TRUNCATE TABLE Match";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int DeleteMatch(string location, string name)
        {
            name = FileUtility.ReplaceInvalidChar(name);
            location = FileUtility.ReplaceInvalidChar(location);

            var sql = @"DELETE FROM Match WHERE Location = '" + location + "' and Name = '" + name + "'";

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int DeleteMatch(int matchid)
        {
            var sql = @"DELETE FROM Match WHERE matchid = " + matchid;

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<Match> GetAllMatch()
        {
            var sql = @"SELECT * FROM Match";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<Match>();
        }

        public static bool HasMatch(string id)
        {
            var sql = string.Format("SELECT * FROM Match WHERE AvID = '{0}'", id);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<Match>().Count > 0 ? true : false;
        }

        public static int SaveMatch(Match match)
        {
            var sql = string.Format(@"INSERT INTO Match (AvID, Name, Location, CreateTime, AvName) VALUES ('{0}', N'{1}', N'{2}', GETDATE(), N'{3}')", match.AvID, FileUtility.ReplaceInvalidChar(match.Name), match.Location, FileUtility.ReplaceInvalidChar(match.AvName));

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
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
            var sql = @"SELECT * FROM Finish";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<Finish>().IsFinish == 0 ? false : true;
        }

        public static int InsertViewHistory(string file)
        {
            var sql = string.Format("INSERT INTO ViewHistory (FileName) VALUES ('{0}')", FileUtility.ReplaceInvalidChar(file));

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static int InsertSearchHistory(string content)
        {
            var sql = string.Format("IF NOT EXISTS (SELECT 1 FROM SearchHistory WHERE Content = '{0}') INSERT INTO SearchHistory(Content) VALUES('{0}')", FileUtility.ReplaceInvalidChar(content));

            return SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
        }

        public static List<SearchHistory> GetSearchHistory()
        {
            var sql = "SELECT Content From SearchHistory";

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToList<SearchHistory>();
        }

        public static bool ViewedFile(string file)
        {
            var sql = string.Format("SELECT * FROM ViewHistory WHERE FileName = '{0}'", file);

            return SqlHelper.ExecuteDataTable(con, CommandType.Text, sql).ToModel<ViewHistory>() == null ? false : true;
        }
    }
}

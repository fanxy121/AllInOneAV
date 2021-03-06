﻿using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Text;

public class ChromeCookieReader
{
    public List<CookieItem> ReadCookies(string hostName)
    {
        if (hostName == null) throw new ArgumentNullException("hostName");

        List<CookieItem> ret = new List<CookieItem>();

        var dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cookies";
        if (!System.IO.File.Exists(dbPath)) throw new System.IO.FileNotFoundException("Cant find cookie store", dbPath); // race condition, but i'll risk it

        var connectionString = "Data Source=" + dbPath + ";pooling=false";

        using (var conn = new System.Data.SQLite.SQLiteConnection(connectionString))
        using (var cmd = conn.CreateCommand())
        {
            var prm = cmd.CreateParameter();
            prm.ParameterName = "hostName";
            prm.Value = hostName;
            cmd.Parameters.Add(prm);

            cmd.CommandText = "SELECT name,encrypted_value FROM cookies WHERE host_key like '%" + hostName + "%'";

            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var name = reader[0].ToString();
                    var encryptedData = (byte[])reader[1];
                    var decodedData = System.Security.Cryptography.ProtectedData.Unprotect(encryptedData, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
                    var plainText = Encoding.ASCII.GetString(decodedData); // Looks like ASCII

                    if (ret.Find(x => x.Name == name) == null)
                    {
                        ret.Add(new CookieItem
                        {
                            Name = name,
                            Value = plainText
                        });
                    }
                }
            }
            conn.Close();
        }

        return ret;
    }
}

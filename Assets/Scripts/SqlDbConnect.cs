using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DataBase
{
    public class SqlDbConnect
    {
        protected SqliteConnection _splConn;
        public SqlDbConnect(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                CreateDbSqlite(dbPath);
            }
            ConnectDbSqlite(dbPath);
        }

        private bool CreateDbSqlite(string dbPath)
        {
            try
            {
                var dirName= new FileInfo(dbPath).Directory.FullName;
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
                SqliteConnection.CreateFile(dbPath);
                
                return true;
            }
            catch(Exception e)
            {
                Debug.LogError($"Fail to find the DataBase, {e.Message}");
                return false;
            }
        }

        private bool ConnectDbSqlite(string dbPath)
        {
            try
            {
                _splConn = new SqliteConnection(new SqliteConnectionStringBuilder() { DataSource = dbPath }.ToString());
                _splConn.Open();
                return true;
            }
            catch(Exception e)
            {
                Debug.LogError($"Fail to connect to the DataBase, {e.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _splConn.Dispose();
        }
    }

}

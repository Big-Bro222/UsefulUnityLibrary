using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DataBase
{
    public class SqlDbCommand : SqlDbConnect
    {
        private SqliteCommand _sqlComm;
        public SqlDbCommand(string dbPath) : base(dbPath)
        {
            _sqlComm = new SqliteCommand(_splConn);
        }

        private string tableName="";
        #region Table management
        public int CreateTable<T>(string TableName="") where T:class
        {
            
            var type = typeof(T);
            tableName = type.Name;
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            if (IsTableCreated<T>(TableName))
            {
                Debug.Log($"The table {tableName}is already created");
                return -1;
            }

            var stringbuilder = new StringBuilder();
            stringbuilder.Append($"create table {tableName} (");
            var fieldInfos = type.GetFields();
            
            foreach(var f in fieldInfos)
            {
                var attribute = f.GetCustomAttribute<ModelHelp>();
                if (attribute.IsCreated)
                {
                    stringbuilder.Append($"{attribute.FieldName} {attribute.Type} ");
                    if (attribute.IsPrimaryKey)
                    {
                        stringbuilder.Append(" primary key ");
                    }
                    if (attribute.IsCanbeNull)
                    {
                        stringbuilder.Append(" null ");
                    }
                    else
                    {
                        stringbuilder.Append(" not null ");
                    }
                    stringbuilder.Append(",");

                }
            }
            stringbuilder.Remove(stringbuilder.Length - 1, 1);
            stringbuilder.Append(")");

            _sqlComm.CommandText = stringbuilder.ToString();
            Debug.Log($"Create table {tableName}");
            Debug.Log(stringbuilder.ToString());
            return _sqlComm.ExecuteNonQuery();
        }

        public int DeleteTable<T>(string TableName="")
        {
            tableName = typeof(T).Name;
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            var sql = $"DROP TABLE {tableName}";
            Debug.Log(sql);
            _sqlComm.CommandText = sql.ToString();
            Debug.Log($"Delete table {tableName}");
            tableName = "";
            return _sqlComm.ExecuteNonQuery();
        }
        
        public int ClearTable<T>(string TableName = "")
        {
            tableName = typeof(T).Name;
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            var sql = $"Delete from {tableName}";
            _sqlComm.CommandText = sql.ToString();
            Debug.Log($"Clear table {tableName}");
            tableName = "";
            return _sqlComm.ExecuteNonQuery();
        }

        //query table names
        public List<string> QueryAllTable()
        {
            List<string> TableList = new List<string>();
            var sql = $"SELECT name FROM sqlite_master where type='table' order by name";
            _sqlComm.CommandText = sql;
            var dr = _sqlComm.ExecuteReader();
            if (dr != null)
            {
                while (dr.Read())
                {
                    TableList.Add(dr.GetValue(0).ToString());
                }
                dr.Close();
                return TableList;
            }
            dr.Close();
            return null;

            
        }

        public bool IsTableCreated<T>(string TableName="") where T : class
        {
            var tableName = typeof(T).Name;
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            var sql = $"SELECT count(*) FROM sqlite_master WHERE type='table' AND name ='{tableName}'";
            _sqlComm.CommandText = sql;
            var dr = _sqlComm.ExecuteReader();
            if (dr != null && dr.Read())
            {
                bool isCreated = Convert.ToInt32(dr[dr.GetName(0)]) == 1;
                dr.Close();
                return isCreated;
            }
            dr.Close();
            return false;
        }
        #endregion

        #region Insert
        public int Insert<T>(T t,string TableName="")where T : class
        {
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            if (t == default(T))
            {
                Debug.LogError("Insert()��������");
                return -1;
            }
            var type = typeof(T);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT OR REPLACE INTO {tableName} (");

            var fieldInfos = type.GetFields();
            foreach(var f in fieldInfos)
            {
                if (f.GetCustomAttribute<ModelHelp>().IsCreated)
                {
                    stringBuilder.Append(f.GetCustomAttribute<ModelHelp>().FieldName);
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") VALUES (");
            foreach (var p in fieldInfos)
            {
                if (p.GetCustomAttribute<ModelHelp>().IsCreated)
                {
                    if (p.GetCustomAttribute<ModelHelp>().Type == "string")
                    {
                        stringBuilder.Append($"'{p.GetValue(t)}'");
                    }
                    else
                    {
                        stringBuilder.Append(p.GetValue(t));
                    }
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(")");
            _sqlComm.CommandText = stringBuilder.ToString();
            return _sqlComm.ExecuteNonQuery();
        }

        public int Insert<T>(List<T> tList,string TableName) where T : class
        {

            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            if (tList==null||tList.Count==0)
            {
                Debug.LogError("Insert()��������");
                return -1;
            }
            var type = typeof(T);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT OR REPLACE INTO {tableName} (");

            var fieldInfos = type.GetFields();
            foreach (var f in fieldInfos)
            {
                if (f.GetCustomAttribute<ModelHelp>().IsCreated)
                {
                    stringBuilder.Append(f.GetCustomAttribute<ModelHelp>().FieldName);
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") VALUES ");

            foreach(var t in tList)
            {
                stringBuilder.Append("(");
                foreach (var f in fieldInfos)
                {
                    if (f.GetCustomAttribute<ModelHelp>().IsCreated)
                    {
                        if (f.Name.Equals("className"))
                        {
                            string[] array = f.GetValue(t) as string[];
                            if (array == null)
                            {
                                stringBuilder.Append($" null ");
                            }
                            else
                            {
                                stringBuilder.Append($"'{string.Join(",", array)}'");
                            }
                        }
                        else if (f.GetCustomAttribute<ModelHelp>().Type == "string")
                        {
                            stringBuilder.Append($"'{f.GetValue(t)}'");
                        }
                        else
                        {
                            stringBuilder.Append(f.GetValue(t));
                        }
                        stringBuilder.Append(",");

                    }
                }
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                stringBuilder.Append("),");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            _sqlComm.CommandText = stringBuilder.ToString();
            return _sqlComm.ExecuteNonQuery();
        }
        #endregion

        #region Delete
        public int DeleteById<T>(int id)
        {
            var sql = $"DELETE FROM{tableName} where Uuid ={id}";
            _sqlComm.CommandText = sql;
            return _sqlComm.ExecuteNonQuery();
        }

        public int DeleteById<T>(List<int> ids)
        {
            var count = 0;
            foreach(var id in ids)
            {
                count += DeleteById<T>(id);
            }
            return count;
        }

        public int DeleteBySql<T>(string sql)
        {
            _sqlComm.CommandText = $"DELETE FROM{tableName} where {sql}";
            return _sqlComm.ExecuteNonQuery();
        }
        #endregion

        #region Update
        public int Update<T>(T t)where T : class
        {
            string id = "";
            if (t == default(T))
            {
                Debug.LogError("Update()��������");
                return -1;
            }

            var type = typeof(T);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"UPDATE {tableName} set ");
            var fieldInfos = type.GetFields();
            foreach (var f in fieldInfos)
            {
                if (f.GetCustomAttribute<ModelHelp>().IsCreated)
                {
                    if(f.GetCustomAttribute<ModelHelp>().FieldName== "Uuid")
                    {
                        id = (string)f.GetValue(t);
                    }
                    stringBuilder.Append($"{f.GetCustomAttribute<ModelHelp>().FieldName} = ");
                    if (f.GetCustomAttribute<ModelHelp>().Type == "string")
                    {
                        stringBuilder.Append($"'{f.GetValue(t)}'");
                    }
                    else
                    {
                        stringBuilder.Append(f.GetValue(t));
                    }
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append($" where Uuid= '{id}'");
            _sqlComm.CommandText = stringBuilder.ToString();
            return _sqlComm.ExecuteNonQuery();
        }

        public int Update<T>(List<T>tList)where T:class{
            if (tList == null || tList.Count == 0)
            {
                Debug.LogError("Update(list)��������");
                return -1;
            }

            int count = 0;
            foreach(var t in tList)
            {
                count += Update(t);
            }
            return count;
        }
        #endregion

        #region Select

        //ͨ��uuidѰ�Ҷ�Ӧ��
        public T SelectedById<T>(string id, string TableName="")where T : class
        {
            var type = typeof(T);
            tableName = typeof(T).Name;
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            //Todo Uuid�ĳ�����
            var sql = $"SELECT * FROM {tableName} where Uuid='{id}'";
            _sqlComm.CommandText = sql;
            var dr = _sqlComm.ExecuteReader();
            if (dr != null && dr.Read())
            {
                T instance = DataReaderToData<T>(dr);
                dr.Close();
                return instance;
            }
            return default (T);
        }

        //ͨ��colum����ѡ���Ӧ��
        public T SelectedByColumName<T>(string ColumName,string value, string TableName = "") where T : class
        {
            var type = typeof(T);
            tableName = typeof(T).Name;
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            var sql = $"SELECT * FROM {tableName} where {ColumName}='{value}'";
            Debug.Log(sql);
            _sqlComm.CommandText = sql;
            var dr = _sqlComm.ExecuteReader();
            if (dr != null && dr.Read())
            {
                T instance = DataReaderToData<T>(dr);
                dr.Close();
                return instance;
            }
            return null;
        }

        public List<T> SelectAll<T>(string TableName) where T : class
        {
            var ret = new List<T>();

            var type = typeof(T);
            var sql = $"SELECT * FROM {TableName}";
            _sqlComm.CommandText = sql;
            var dr = _sqlComm.ExecuteReader();
            if (dr != null )
            {
                while (dr.Read())
                {
                    ret.Add(DataReaderToData<T>(dr));
                }
            }
            return ret;
        }

        //use the sqlcommand
        public List<T> SelectBySql<T>(string sqlWhere,string TableName="") where T : class
        {
            var ret = new List<T>();
            var type = typeof(T);
            var sql = "";
            tableName = typeof(T).Name;
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            if (string.IsNullOrEmpty(sqlWhere))
            {
                sql = $"SELECT * FROM {tableName}";
            }
            else
            {
                sql = $"SELECT * FROM {tableName} where {sqlWhere}";

            }

            _sqlComm.CommandText = sql;
            //Debug.Log(sql);
            var dr = _sqlComm.ExecuteReader();
            
            if (dr != null)
            {
                while (dr.Read())
                {
                    ret.Add(DataReaderToData<T>(dr));
                }
            }
            dr.Close();
            return ret;
        }
        
        //select a specific column with sqlcommand
        public List<T> SelectColumnBySql<T>(string columnName, string sqlWhere, string TableName = "") where T : class
        {
            var ret = new List<T>();
            var type = typeof(T);
            var sql = "";
            tableName = typeof(T).Name;
            if (!TableName.Equals(""))
            {
                tableName = TableName;
            }
            if (string.IsNullOrEmpty(sqlWhere))
            {
                sql = $"SELECT {columnName} FROM {tableName}";
            }
            else
            {
                sql = $"SELECT {columnName} FROM {tableName} where {sqlWhere}";

            }

            _sqlComm.CommandText = sql;
            //Debug.Log(sql);
            var dr = _sqlComm.ExecuteReader();

            if (dr != null)
            {
                Debug.Log(dr.FieldCount);
                while (dr.Read())
                {

                    ret.Add(dr[columnName] as T);
                }
            }
            dr.Close();
            return ret;
        }

        public List<string> SelectColumnByJOINSql(string columnName, string sqlWhere, string TableName1="",string TableName2="", string Method = "JOIN") 
        {
            var ret = new List<string>();
            var type = typeof(string);
            var sql = "";
            if (string.IsNullOrEmpty(sqlWhere))
            {
                Debug.Log("������sqlite����");
            }
            else
            {
                //Select Uuid From Entities LEFT JOIN EntitiesTemps ON Entities.Uuid = EntitiesTemps.Uuid

                sql = $"SELECT {columnName} FROM {TableName1} {Method} {TableName2} ON {sqlWhere}";

            }

            _sqlComm.CommandText = sql;
            Debug.Log(sql);
            var dr = _sqlComm.ExecuteReader();
            
            if (dr != null)
            {
                while (dr.Read())
                {
                    ret.Add(dr[columnName] as string);
                }
            }
            dr.Close();
            return ret;
        }
        private T DataReaderToData<T>(SqliteDataReader dr)where T : class
        {
            try
            {
                List<string> fieldNames = new List<string>();
                for(int i = 0; i < dr.FieldCount; i++)
                {
                    fieldNames.Add(dr.GetName(i));
                }
                var type = typeof(T);
                T data = Activator.CreateInstance<T>();
                var fieldInfos = type.GetFields();
                foreach(var f in fieldInfos)
                {
                    if (f.IsPrivate)
                    {
                        continue;
                    }
                    var fieldName = f.GetCustomAttribute<ModelHelp>().FieldName;
                    if (fieldNames.Contains(fieldName) && f.GetCustomAttribute<ModelHelp>().IsCreated)
                    {
                        if (f.Name.Equals("className"))
                        {
                            string liststr = dr[fieldName] as string;
                            if (liststr == null)
                            {
                                f.SetValue(data, null);
                            }
                            else
                            {
                                f.SetValue(data, liststr.Split(','));
                            }

                        }
                        else if (f.Name.Equals("components"))
                        {
                            f.SetValue(data, dr[fieldName]);
                        }
                        else
                        {
                            f.SetValue(data, dr[fieldName]);
                        }
                    }
                }
                //dr.Close();
                return data;

            }
            catch (Exception e)
            {
                Debug.LogError($"DataReaderToData()ת��������{typeof(T).Name},{e.Message}");
                return null;
            }
        }


        #endregion

        #region sqlite special selection

        public List<string> SelectColumnbySqlStr(string ColumnName,string sqlCommand)
        {
            var ret = new List<string>();
            _sqlComm.CommandText = sqlCommand;
            var dr = _sqlComm.ExecuteReader();
            if (dr != null)
            {
                while (dr.Read())
                {
                    ret.Add(dr[ColumnName] as string);
                }
            }
            return ret;
        }


        public List<T> SelectListBySqlStr<T>(string sqlCommand) where T : class
        {
            var ret = new List<T>();
            _sqlComm.CommandText = sqlCommand;
            //Debug.Log(sql);
            var dr = _sqlComm.ExecuteReader();

            if (dr != null)
            {
                while (dr.Read())
                {
                    ret.Add(DataReaderToData<T>(dr));
                }
            }
            dr.Close();
            return ret;
        }

        #endregion
    }
}


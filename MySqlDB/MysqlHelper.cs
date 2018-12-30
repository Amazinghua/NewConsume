using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MySqlDB
{
    public class MysqlHelper
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库. 
        public static string connectionString = ConfigurationManager.AppSettings["MySQL"];
        public MysqlHelper() { }
        #region ExecuteNonQuery 
        //执行SQL语句，返回影响的记录数 
        /// <summary> 
        /// 执行SQL语句，返回影响的记录数 
        /// </summary> 
        /// <param name="SQLString">SQL语句</param> 
        /// <returns>影响的记录数</returns> 
        public static int ExecuteNonQuery(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        /// <summary> 
        /// 执行SQL语句，返回影响的记录数 
        /// </summary> 
        /// <param name="SQLString">SQL语句</param> 
        /// <returns>影响的记录数</returns> 
        public static int ExecuteNonQuery(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
        //执行多条SQL语句，实现数据库事务。 
        /// <summary> 
        /// 执行多条SQL语句，实现数据库事务。 
        /// </summary> 
        /// <param name="SQLStringList">多条SQL语句</param> 
        public static bool ExecuteNoQueryTran(List<String> SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                MySqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            PrepareCommand(cmd, conn, tx, strsql, null);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    cmd.ExecuteNonQuery();
                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    return false;
                }
            }
        }
        #endregion
        #region ExecuteScalar 
        /// <summary> 
        /// 执行一条计算查询结果语句，返回查询结果（object）。 
        /// </summary> 
        /// <param name="SQLString">计算查询结果语句</param> 
        /// <returns>查询结果（object）</returns> 
        public static object ExecuteScalar(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        /// <summary> 
        /// 执行一条计算查询结果语句，返回查询结果（object）。 
        /// </summary> 
        /// <param name="SQLString">计算查询结果语句</param> 
        /// <returns>查询结果（object）</returns> 
        public static object ExecuteScalar(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
        #endregion
        #region ExecuteReader 
        /// <summary> 
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close ) 
        /// </summary> 
        /// <param name="strSQL">查询语句</param> 
        /// <returns>MySqlDataReader</returns> 
        public static MySqlDataReader ExecuteReader(string strSQL)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                throw e;
            }
        }
        /// <summary> 
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close ) 
        /// </summary> 
        /// <param name="strSQL">查询语句</param> 
        /// <returns>MySqlDataReader</returns> 
        public static MySqlDataReader ExecuteReader(string SQLString, params MySqlParameter[] cmdParms)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                throw e;
            }
            // finally 
            // { 
            // cmd.Dispose(); 
            // connection.Close(); 
            // } 
        }
        #endregion
        #region ExecuteDataTable 
        /// <summary> 
        /// 执行查询语句，返回DataTable 
        /// </summary> 
        /// <param name="SQLString">查询语句</param> 
        /// <returns>DataTable</returns> 
        public static DataTable ExecuteDataTable(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }
        /// <summary> 
        /// 执行查询语句，返回DataSet 
        /// </summary> 
        /// <param name="SQLString">查询语句</param> 
        /// <returns>DataTable</returns> 
        public static DataTable ExecuteDataTable(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds.Tables[0];
                }
            }
        }
        //获取起始页码和结束页码 
        public static DataTable ExecuteDataTable(string cmdText, int startResord, int maxRecord)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(cmdText, connection);
                    command.Fill(ds, startResord, maxRecord, "ds");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }
        public static DataTable ToDataTablePager(out int recordCount,string whereStr,int pageIdex,int pageSize)
        {
            int rows = 0;
            string sqlCount = whereStr;
            DataTable dt0 = new DataTable();
            dt0 = ExecuteDataTable(whereStr);
            rows = dt0.Rows.Count;

            DataTable dt = new DataTable();
            dt = ExecuteDataTable(whereStr, (pageIdex - 1) * pageSize, pageSize);
            recordCount = rows;
            return dt;
        }
        public static DataTable GetStrTable(string tableName,string whereStr,string orderExpression)
        {
            DataTable dt = new DataTable();
            string sqlStr = "";

                sqlStr = string.Format("select * from {0} where 1=1 {1}", tableName, whereStr);
            if (!string.IsNullOrEmpty(orderExpression)) { sqlStr += string.Format(" Order by {0}", orderExpression); }
            dt = ExecuteDataTable(sqlStr);
                
            return dt;
        }
        #endregion
        /// <summary> 
        /// 获取分页数据 在不用存储过程情况下 
        /// </summary> 
        /// <param name="recordCount">总记录条数</param> 
        /// <param name="selectList">选择的列逗号隔开,支持top num</param> 
        /// <param name="tableName">表名字</param> 
        /// <param name="whereStr">条件字符 必须前加 and</param> 
        /// <param name="orderExpression">排序 例如 ID</param> 
        /// <param name="pageIdex">当前索引页</param> 
        /// <param name="pageSize">每页记录数</param> 
        /// <returns></returns> 
        public static DataTable getPager(out int recordCount, string selectList, string tableName, string whereStr, string orderExpression, int pageIdex, int pageSize)
        {

            int rows = 0;
            DataTable dt = new DataTable();
            MatchCollection matchs = Regex.Matches(selectList, @"top\s+\d{1,}", RegexOptions.IgnoreCase);//含有top 
            string sqlStr = "";
            if (selectList != "")
            {
                sqlStr = string.Format("select {0} from {1} where 1=1 {2}", selectList, tableName, whereStr);
            }
            else
            {
                sqlStr = string.Format("select * from {0} where 1=1 {1}", tableName, whereStr);
            }
            if (!string.IsNullOrEmpty(orderExpression)) { sqlStr += string.Format(" Order by {0}", orderExpression); }
            if (matchs.Count > 0) //含有top的时候 
            {
                DataTable dtTemp = ExecuteDataTable(sqlStr);
                rows = dtTemp.Rows.Count;
            }
            else //不含有top的时候 
            {
                string sqlCount = string.Format("select count(*) from {0} where 1=1 {1} ", tableName, whereStr);
                //获取行数 
                object obj = ExecuteScalar(sqlCount);
                if (obj != null)
                {
                    rows = Convert.ToInt32(obj);
                }
            }
            dt = ExecuteDataTable(sqlStr, (pageIdex - 1) * pageSize, pageSize);
            recordCount = rows;
            return dt;
        }
        #region 创建command 
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType; 
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion
        //批量插入员工
        public static int BulkInsert(string connectionString, DataTable table, string TableName)
        {
            //if (string.IsNullOrEmpty(table.TableName)) throw new Exception(TableName);
            if (table.Rows.Count == 0) return 0;
            int insertCount = 0;
            string tmpPath = Path.GetTempFileName();
            string csv = DataTableToCsv(table);
            File.WriteAllText(tmpPath, csv);
            // MySqlTransaction tran = null;  

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                try
                {
                    //var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                    conn.Open();
                    //tran = conn.BeginTransaction();  
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        NumberOfLinesToSkip = 0,
                        TableName = TableName,
                        Columns = {"Company","usr_no","usr_name", "dept_ID","card_no","phone_no","memo","FeeResource","FeePlace"}

                    };
                    //bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToArray());
                    insertCount = bulk.Load();
                    //tran.Commit();
                }
                catch (MySqlException ex)
                {
                    // if (tran != null) tran.Rollback();  
                    throw ex;
                }
            }
            File.Delete(tmpPath);
            return insertCount;
        }
        //批量插入部门
        public static int DeptInsert(string connectionString,DataTable table,string TableName)
        {
            //if (string.IsNullOrEmpty(table.TableName)) throw new Exception(TableName);
            if (table.Rows.Count == 0) return 0;
            int insertCount = 0;
            string tmpPath = Path.GetTempFileName();
            string csv = DataTableToCsv(table);
            File.WriteAllText(tmpPath, csv);
            // MySqlTransaction tran = null;  

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                try
                {
                    //var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                    conn.Open();
                    //tran = conn.BeginTransaction();  
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        NumberOfLinesToSkip = 0,
                        TableName = TableName,
                        Columns = { "Company","Dept_Name","Dept_Level","Dept_up","dept_state","FeeResource","PlaceType" }

                    };
                    //bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToArray());
                    insertCount = bulk.Load();
                    //tran.Commit();
                }
                catch (MySqlException ex)
                {
                    // if (tran != null) tran.Rollback();  
                    throw ex;
                }
            }
            File.Delete(tmpPath);
            return insertCount;
        }

        public static int QrInsert(string connectionString, DataTable table, string TableName)
        {
            //if (string.IsNullOrEmpty(table.TableName)) throw new Exception(TableName);
            if (table.Rows.Count == 0) return 0;
            int insertCount = 0;
            string tmpPath = Path.GetTempFileName();
            string csv = DataTableToCsv(table);
            File.WriteAllText(tmpPath, csv);
            // MySqlTransaction tran = null;  

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {

                try
                {
                    //var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
                    conn.Open();
                    //tran = conn.BeginTransaction();  
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        NumberOfLinesToSkip = 0,
                        TableName = TableName,
                        Columns = { "Company_owner", "Staff_name", "Staff_phone", "Profession", "Expiry_time", "Create_time", "UUID" }

                    };
                    //bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToArray());
                    insertCount = bulk.Load();
                    //tran.Commit();
                }
                catch (MySqlException ex)
                {
                    // if (tran != null) tran.Rollback();  
                    throw ex;
                }
            }
            File.Delete(tmpPath);
            return insertCount;
        }
        public static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。  
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }


            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public static void LoginIn(string user, string password)
        {
            MySqlDataReader dr = MySqlHelper.ExecuteReader(connectionString, "select * from tab_account where the_user='" + user + "'");
            if (!dr.Read())
            {

            } else if(dr["the_password"].ToString().Trim() == password)
            {

            }
            else
            {

            }
        }

    }
}

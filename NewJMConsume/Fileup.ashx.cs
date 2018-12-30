using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using MySqlDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewJMConsume
{
    /// <summary>
    /// Fileup 的摘要说明
    /// </summary>
    public class Fileup : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string Result = "";
            JObject resjobj = new JObject();

            string savePath = "";
            string timeRemark = DateTime.Now.ToString("yyyy-MM-dd");
            string action = System.Web.HttpContext.Current.Request.Params["type"];
            switch (action)
            {
                #region 批量上传文件
                case "fileToSql":
                    
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                        string dir = HttpContext.Current.Request.PhysicalApplicationPath;

                        savePath = dir + "uploadStaff\\";
                        if (Directory.Exists(savePath) == false) //工程目录下 Log目录 '目录是否存在,为true则没有此目录
                        {
                            Directory.CreateDirectory(savePath); //建立目录　Directory为目录对象
                        }
                        savePath += timeRemark + Path.GetFileName(postedFile.FileName);
                        if (File.Exists(savePath))
                        {
                            File.Delete(savePath);
                        }
                        postedFile.SaveAs(savePath);
                        //读取ecxcel表
                        DataTable fakeTable = ReadExcelToDataTable(savePath);
                        //批量插入数据库
                        int rows = MySqlDB.MysqlHelper.BulkInsert(System.Configuration.ConfigurationManager.AppSettings["MySQL"], fakeTable, "tab_user_info");
                        resjobj.Add("result", JToken.FromObject("ok"));
                    }
                        break;
                case "fileToDept":
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                        string dir = HttpContext.Current.Request.PhysicalApplicationPath;

                        savePath = dir + "uploadDept\\";
                        if (Directory.Exists(savePath) == false) //工程目录下 Log目录 '目录是否存在,为true则没有此目录
                        {
                            Directory.CreateDirectory(savePath); //建立目录　Directory为目录对象
                        }
                        savePath += timeRemark + Path.GetFileName(postedFile.FileName);
                        if (File.Exists(savePath))
                        {
                            File.Delete(savePath);
                        }
                        postedFile.SaveAs(savePath);
                        //读取ecxcel表
                        DataTable fakeTable = ReadExcelToDataTable(savePath);
                        //批量插入数据库
                        int rows = MySqlDB.MysqlHelper.DeptInsert(System.Configuration.ConfigurationManager.AppSettings["MySQL"], fakeTable, "tbdeptinfo");
                        resjobj.Add("result", JToken.FromObject("ok"));
                    }
                        break;
                case "fileToadd":
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                        string dir = HttpContext.Current.Request.PhysicalApplicationPath;

                        savePath = dir + "uploadAdd\\";
                        if (Directory.Exists(savePath) == false) //工程目录下 Log目录 '目录是否存在,为true则没有此目录
                        {
                            Directory.CreateDirectory(savePath); //建立目录　Directory为目录对象
                        }
                        savePath += timeRemark + Path.GetFileName(postedFile.FileName);
                        if (File.Exists(savePath))
                        {
                            File.Delete(savePath);
                        }
                        postedFile.SaveAs(savePath);
                        //读取ecxcel表
                        DataTable fakeTable = ReadExcelToDataTable(savePath);
                        for(int i=0; i<fakeTable.Rows.Count; i++)
                        {
                            string sql = "insert into tab_add_money (card_no,add_year,add_month,add_money,minus_money,acc_money,create_date,usr_no,method,creator,ReMark)values('" + fakeTable.Rows[i][0].ToString() + "', year(CURRENT_DATE), month(CURRENT_DATE), '" + fakeTable.Rows[i][2].ToString() + "', '0', '" + fakeTable.Rows[i][2].ToString() + "', NOW(), '" + fakeTable.Rows[i][1].ToString() + "', '" + "团体充值" + "', '" + HttpContext.Current.Session["user"] + "', '" + fakeTable.Rows[i][3].ToString() + "')";
                            int execNum = MysqlHelper.ExecuteNonQuery(sql);
                        }
                        resjobj.Add("result", JToken.FromObject("ok"));
                    }
                    break;
                case "fileToQR":
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                        string dir = HttpContext.Current.Request.PhysicalApplicationPath;

                        savePath = dir + "uploadQR\\";
                        if (Directory.Exists(savePath) == false) //工程目录下 Log目录 '目录是否存在,为true则没有此目录
                        {
                            Directory.CreateDirectory(savePath); //建立目录　Directory为目录对象
                        }
                        savePath += timeRemark + Path.GetFileName(postedFile.FileName);
                        if (File.Exists(savePath))
                        {
                            File.Delete(savePath);
                        }
                        postedFile.SaveAs(savePath);
                        //读取ecxcel表
                        DataTable fakeTable = ReadExcelToDataTable(savePath);
                        fakeTable.Columns.Add("Create_time", typeof(DateTime));
                        fakeTable.Columns.Add("UUID", typeof(string));
                        for (int i = 0; i < fakeTable.Rows.Count; i++)
                        {
                            fakeTable.Rows[i]["Create_time"] = timeRemark;
                            fakeTable.Rows[i]["UUID"] = System.Guid.NewGuid().ToString("N");
                        }
                        int rows = MySqlDB.MysqlHelper.QrInsert(System.Configuration.ConfigurationManager.AppSettings["MySQL"], fakeTable, "tab_qr_setting");
                        resjobj.Add("result", JToken.FromObject("ok"));
                    }
                        break;
                #endregion
                case "load_role":
                    DataTable dt = MysqlHelper.ExecuteDataTable("select RoId,RoName from tbrole");
                    dt.Dispose();
                    resjobj.Add("result", JToken.FromObject("ok"));
                    resjobj.Add("dt", JToken.FromObject(dt));
                    break;
                case "load_deptID":
                    DataTable dt0 = MysqlHelper.ExecuteDataTable("select Dept_Id,Dept_Name from tbdeptinfo");
                    dt0.Dispose();
                    resjobj.Add("result", JToken.FromObject("ok"));
                    resjobj.Add("dt", JToken.FromObject(dt0));

                    break;
                case "load_company":
                    DataTable dt1 = MysqlHelper.ExecuteDataTable("select id,company_name from tab_company_all");
                    resjobj.Add("result", JToken.FromObject("ok"));
                    resjobj.Add("dt", JToken.FromObject(dt1));
                    break;
                case "load_feeSource":
                    DataTable dt2 = MysqlHelper.ExecuteDataTable("select ID,ResourceID,ResourceName from tab_resource_info");
                    resjobj.Add("result", JToken.FromObject("ok"));
                    resjobj.Add("dt", JToken.FromObject(dt2));
                    break;
                case "load_feeplace":
                    DataTable dt3 = MysqlHelper.ExecuteDataTable("select * from tab_feeplace_list where CanUse ='1'");
                    resjobj.Add("result", JToken.FromObject("ok"));
                    resjobj.Add("dt", JToken.FromObject(dt3));
                    break;
                case "load_dept_up":
                    DataTable dt4 = MysqlHelper.ExecuteDataTable("select Dept_Id,Dept_Name from tbdeptinfo where Dept_up =1");
                    resjobj.Add("result", JToken.FromObject("ok"));
                    resjobj.Add("dt", JToken.FromObject(dt4));
                    break;


            }
            Result = JsonConvert.SerializeObject(resjobj);
            context.Response.Write(Result);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        } 
        /// <summary>
        /// 将excel文件内容读取到DataTable数据表中
        /// </summary>
        /// <param name="fileName">文件完整路径名</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            NPOI.SS.UserModel.ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    return null;
                }
                //根据指定路径读取文件
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //根据文件流创建excel数据结构
                NPOI.SS.UserModel.IWorkbook workbook = NPOI.SS.UserModel.WorkbookFactory.Create(fs);
                //IWorkbook workbook = new HSSFWorkbook(fs);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    NPOI.SS.UserModel.IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            NPOI.SS.UserModel.ICell cell = firstRow.GetCell(i);
                            cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        NPOI.SS.UserModel.IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 批量插入SqlBulkCopy
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName">表名</param>
        public static void BatchInsertBySqlBulkCopy(DataTable dt, string tableName)
        {
            using (SqlBulkCopy sbc = new SqlBulkCopy(System.Configuration.ConfigurationManager.AppSettings["MySQL"]))
            {
                sbc.BatchSize = dt.Rows.Count;
                sbc.BulkCopyTimeout = 60;
                sbc.DestinationTableName = tableName;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sbc.ColumnMappings.Add(dt.Columns[i].ColumnName, i);
                }
                //全部写入数据库
                sbc.WriteToServer(dt);
            }
        }
    }
}
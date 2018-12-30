using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MySqlDB
{
    public class DateExcel
    {
        public static string DataTableToExcel_Count(string filePath, DataTable dataTable,string title)
        {
            string result = "error";
            try
            {
                //创建工作薄  
                IWorkbook wb;
                string extension = System.IO.Path.GetExtension(filePath);
                //根据指定的文件格式创建对应的类
                if (extension.Equals(".xls"))
                {
                    wb = new HSSFWorkbook();
                }
                else
                {
                    wb = new XSSFWorkbook();
                }
                ICellStyle style1 = wb.CreateCellStyle();//样式
                style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                style1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
                IFont font1 = wb.CreateFont();
                font1.Color = HSSFColor.Red.Index;
                style1.SetFont(font1);
                //设置边框
                //style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style1.WrapText = true;//自动换行

                ICellStyle style2 = wb.CreateCellStyle();//样式
                IFont font2 = wb.CreateFont();//字体
                font2.FontName = "楷体";
                font2.Color = HSSFColor.Red.Index;//字体颜色
                font2.Boldweight = (short)FontBoldWeight.Normal;//字体加粗样式
                style2.SetFont(font2);//样式里的字体设置具体的字体样式
                //设置背景色
                style2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                style2.FillPattern = FillPattern.SolidForeground;
                style2.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                style2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式

                //设置单元格日期格式
                ICellStyle dateStyle = wb.CreateCellStyle();//样式
                dateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                dateStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式


                IDataFormat dataFormatCustom = wb.CreateDataFormat();//设置数据显示格式
                dateStyle.DataFormat = dataFormatCustom.GetFormat("yyyy-MM-dd HH:mm:ss");
                //设置第一行字体颜色
                ICellStyle datessStyle = wb.CreateCellStyle();//样式
                IFont fontsse = wb.CreateFont();//字体

                fontsse.Color = HSSFColor.Red.Index;//字体颜色
                fontsse.Boldweight = (short)FontBoldWeight.Normal;//字体加粗样式
                datessStyle.SetFont(fontsse);//样式里的字体设置具体的字体样式


                //设置保留小数后四位单元格数值格式
                ICellStyle styledata4 = wb.CreateCellStyle();//样式
                styledata4.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                styledata4.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
                styledata4.SetFont(font1);
                //                IDataFormat dataFormatNumber = wb.CreateDataFormat();
                styledata4.DataFormat = 43;

                //设置整数单元格数值格式
                ICellStyle styledata0 = wb.CreateCellStyle();//样式
                styledata0.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                styledata0.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
                styledata0.SetFont(font1);
                //                IDataFormat dataFormatNumber2 = wb.CreateDataFormat();
                styledata0.DataFormat = 41;

                //创建一个表单
                ISheet sheet = wb.CreateSheet("Sheet0");
                //设置列宽

                IRow row;
                ICell cell;

                int y = 0;
                string titleName = title;
                string[] titleNamestr = titleName.Split('|');
                row = sheet.CreateRow(0);
                for (int i = 0; i < titleNamestr.Count(); i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = datessStyle;
                    SetCellValue(cell, titleNamestr[i].ToString());
                }

                //for (int i = 0; i < dt.Rows.Count; i++)
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    y = y + 1;
                    row = sheet.CreateRow(y);//创建第y行
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);//创建第j列
                        SetCellValue(cell, dataTable.Rows[i][j]);
                        if (dataTable.Rows[i][j] is DateTime)
                        {
                            cell.CellStyle = dateStyle;
                        }
                    }
                }
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                FileStream fs = File.OpenWrite(filePath);
                wb.Write(fs);//向打开的这个Excel文件中写入表单并保存。  
                fs.Close();
                result = "OK";
            }
            catch (Exception ex)
            {
                return "error";
            }
            return result;
        }

        public static void SetCellValue(ICell cell, object obj)
        {
            if (obj is int)
            {
                cell.SetCellValue((int)obj);
            }
            else if (obj is double)
            {
                cell.SetCellValue((double)obj);
            }
            else if (obj is IRichTextString)
            {
                cell.SetCellValue((IRichTextString)obj);
            }
            else if (obj is string)
            {
                cell.SetCellValue(obj.ToString());
            }
            else if (obj is DateTime)
            {
                cell.SetCellValue((DateTime)obj);
            }
            else if (obj is bool)
            {
                cell.SetCellValue((bool)obj);
            }
            else if (obj is decimal)
            {
                cell.SetCellValue(Convert.ToDouble(obj));
            }
            else
            {
                cell.SetCellValue(obj.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

namespace ESC.Core
{
    public class NopiExcelManager : IExcelMapanger
    {
        /// <summary>
        /// 现在只支持记录小于65535条记录
        /// 多个sheet页暂不支持
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public MemoryStream DataTableToExcel(DataTable table, string fileName)
        {
            if (fileName.EndsWith("xls"))
            {
                return TableToExcelForXLS(table);
            }
            else
            {
                return TableToExcelForXLSX(table);
            }
        }

        #region Excel2003
        /// <summary>
        /// 将Excel文件中的数据读出到DataTable中(xls)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataTable ExcelToTableForXLS(string file)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                ISheet sheet = hssfworkbook.GetSheetAt(0);

                //表头
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                    if (obj == null || obj.ToString() == string.Empty)
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    }
                    else
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    columns.Add(i);
                }
                //数据
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 将DataTable数据导出到Excel文件中(xls)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public static MemoryStream TableToExcelForXLS(DataTable dt)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("sheet1");

            //加工 只导出Caption不为空的数据
            List<DataColumn> cols = new List<DataColumn>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Columns[i].Caption))
                {
                    cols.Add(dt.Columns[i]);
                }
            }

            //表头
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < cols.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(cols[i].Caption);
                ICellStyle style = hssfworkbook.CreateCellStyle();//创建样式对象
                IFont font = hssfworkbook.CreateFont(); //创建一个字体样式对象
                font.Color = new HSSFColor.White().Indexed;
                font.FontHeightInPoints = 13;//字体大小
                style.FillBackgroundColor = HSSFColor.RoyalBlue.Index;
                style.FillForegroundColor =HSSFColor.RoyalBlue.Index;
                style.FillPattern = FillPattern.SolidForeground;
                style.SetFont(font); //将字体样式赋给样式对象
                cell.CellStyle = style; //把样式赋给单元格格
            }

            //数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < cols.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][cols[j]].ToString());
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                hssfworkbook.Write(ms);
                ms.Flush();
                return ms;
            }
        }

        /// <summary>
        /// 获取单元格类型(xls)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueTypeForXLS(HSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }
        #endregion

        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public DataTable ExcelToDataTable(string fileName)
        {
            if (fileName.EndsWith("xls"))
            {
                return ExcelToTableForXLS(fileName);
            }
            else
            {
                return ExcelToTableForXLSX(fileName);
            }
        }

        #region Excel2007
        /// <summary>
        /// 将Excel文件中的数据读出到DataTable中(xlsx)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataTable ExcelToTableForXLSX(string file)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                ISheet sheet = xssfworkbook.GetSheetAt(0);

                //表头
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
                    if (obj == null || obj.ToString() == string.Empty)
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    }
                    else
                    {
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    }
                    columns.Add(i);
                }
                //数据
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 将DataTable数据导出到Excel文件中(xlsx)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public static MemoryStream TableToExcelForXLSX(DataTable dt)
        {
            XSSFWorkbook xssfworkbook = new XSSFWorkbook();
            ISheet sheet = xssfworkbook.CreateSheet("sheet1");

            //加工 只导出Caption不为空的数据
            List<DataColumn> cols = new List<DataColumn>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Columns[i].Caption))
                {
                    cols.Add(dt.Columns[i]);
                }
            }

            //表头
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < cols.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(cols[i].Caption);
                ICellStyle style = xssfworkbook.CreateCellStyle();//创建样式对象
                IFont font = xssfworkbook.CreateFont(); //创建一个字体样式对象
                font.Color = new HSSFColor.White().Indexed;              
                font.FontHeightInPoints = 13;//字体大小
                style.FillBackgroundColor = HSSFColor.RoyalBlue.Index;
                style.FillForegroundColor = HSSFColor.RoyalBlue.Index;
                style.FillPattern = FillPattern.SolidForeground;
                style.SetFont(font); //将字体样式赋给样式对象
                cell.CellStyle = style; //把样式赋给单元格
            }

            //数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < cols.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][cols[j]].ToString());
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                xssfworkbook.Write(ms);
                ms.Flush();
                return ms;
            }
        }

        /// <summary>
        /// 获取单元格类型(xlsx)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueTypeForXLSX(XSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }

        #endregion
    }
}

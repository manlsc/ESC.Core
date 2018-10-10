using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public interface IExcelMapanger
    {
        /// <summary>
        /// 将DataTable转换成内存流
        /// <remarks>一般用户数据下载和传输</remarks>
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        MemoryStream DataTableToExcel(DataTable table, string fileName);

        /// <summary>
        /// 将文件读取到DataTable
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        DataTable ExcelToDataTable(string fileName);
    }
}

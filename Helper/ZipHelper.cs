using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core.Helper
{
    /// <summary>
    /// Zip压缩
    /// </summary>
    public class ZipHelper
    {
        /// <summary>  
        /// 压缩文件
        /// </summary>  
        /// <param name="filePaths">文件列表</param>  
        /// <param name="zipFilePath">目标路径</param>  
        /// <returns></returns>  
        public static void CreateZipFile(List<string> filePaths, string zipFilePath)
        {          
            using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
            {
                s.SetLevel(9); // 压缩级别 0-9
                byte[] buffer = new byte[4096]; //缓冲区大小
                foreach (string file in filePaths)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(file))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Close();
            }
        }
    }
}

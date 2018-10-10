using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ESC.Core
{
    /// <summary> 
    /// 文件操作类 
    /// </summary> 
    public class ESCFile
    {
        #region  路径

        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 检测指定目录是否为空 
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string directoryPath)
        {

            //判断是否存在文件 
            string[] fileNames = GetFileNames(directoryPath);
            if (fileNames.Length > 0)
            {
                return false;
            }

            //判断是否存在文件夹 
            string[] directoryNames = GetDirectories(directoryPath);
            if (directoryNames.Length > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录 
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件 
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }

                //删除目录中所有的子目录 
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }

        /// <summary>
        /// 删除指定的目录
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <returns></returns>
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表 
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符,例如:Log*.xml</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 文件

        /// <summary> 
        /// 检测指定文件是否存在,如果存在则返回true。 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>     
        /// <returns></returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 创建指定的文件,如果已经存在则忽略
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件 
                if (!IsExistFile(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        fs.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                //Log.Exception(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 创建一个文件,并将字节流写入文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件 
                if (!IsExistFile(filePath))
                {
                    using (FileStream fs = File.Create(filePath, buffer.Length))
                    {
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Exception(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 删除指定的文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 清空文件内容
        /// 本质是先删除在重建
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            //删除文件 
            File.Delete(filePath);

            //重新创建该文件 
            CreateFile(filePath);
        }

        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <returns></returns>
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常 
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表 
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符,例如：Log*.xml</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常 
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取文本文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public string[] ReadTextFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        /// <summary>
        /// 向文本文件中写入内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void WriteText(string filePath, string content)
        {
            //向文件写入内容 
            File.WriteAllText(filePath, content);
        }

        /// <summary> 
        /// 向文本文件的尾部追加内容 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        /// <param name="content">写入的内容</param> 
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        /// <summary> 
        /// 将源文件的内容复制到目标文件中 
        /// </summary> 
        /// <param name="sourceFilePath">源文件的绝对路径</param> 
        /// <param name="destFilePath">目标文件的绝对路径</param> 
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        /// <summary>
        /// 将文件移动到指定目录
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称 
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                descDirectoryPath = AddSlash(descDirectoryPath);

                //如果目标中存在同名文件,则删除 
                if (IsExistFile(descDirectoryPath + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + sourceFileName);
                }
                //将文件移动到指定目录 
                File.Move(sourceFilePath, descDirectoryPath + sourceFileName);
            }
        }

        /// <summary> 
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 ) 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称 
            string fileName = GetFileName(filePath);
            return fileName.Split('.')[0];
        }

        /// <summary> 
        /// 从文件的绝对路径中获取扩展名 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static string GetExtension(string filePath)
        {
            //获取文件的名称 
            return Path.GetExtension(filePath);
        }

        /// <summary> 
        /// 从文件的绝对路径中获取文件名( 包含扩展名 ) 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static string GetFileName(string filePath)
        {
            //获取文件的名称 
            return Path.GetFileName(filePath);
        }

        public static byte[] ReadFile(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, Convert.ToInt32(fs.Length));
                    return buffer;
                }
            }
            catch (Exception ex)
            {
                //Log.Exception(ex);
                throw ex;
            }

        }

        /// <summary> 
        /// 将文件读取到字符串中 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.UTF8);
        }

        /// <summary> 
        /// 将文件读取到字符串中 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        /// <param name="encoding">字符编码</param> 
        public static string FileToString(string filePath, Encoding encoding)
        {
            //创建流读取器 
            StreamReader reader = new StreamReader(filePath, encoding);
            try
            {
                //读取流 
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                //Log.Exception(ex);
                throw ex;
            }
            finally
            {
                //关闭流读取器 
                reader.Close();
            }
        }

        /// <summary> 
        /// 将流读取到缓冲区中 
        /// </summary> 
        /// <param name="stream">原始流</param> 
        public static byte[] StreamToBytes(Stream stream)
        {
            try
            {
                //创建缓冲区 
                byte[] buffer = new byte[stream.Length];

                //读取流 
                stream.Read(buffer, 0, Convert.ToInt32(stream.Length));

                //返回流 
                return buffer;
            }
            catch (Exception ex)
            {
                //Log.Exception(ex);
                throw ex;
            }
            finally
            {
                //关闭流 
                stream.Close();
            }
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 为路径添加斜线\\
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AddSlash(string path)
        {
            if (path.EndsWith("\\"))
                return path;
            else
                return path + "\\";
        }

        #endregion

    }

}

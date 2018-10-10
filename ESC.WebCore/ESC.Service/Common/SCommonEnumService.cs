using ESC.Core;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Service
{
    public class SCommonEnumService
    {
        protected SCommonEnumRepository ceRepository;

        public SCommonEnumService()
        {
            ceRepository = new SCommonEnumRepository();
        }

        /// <summary>
        /// 根据字段查询配置
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public List<SCommonEnum> GetCommonEnumByField(string field)
        {
            return ceRepository.GetCommonEnumByField(field);
        }

        public List<SCommonEnum> GetCommonEnum(string field, string enumType)
        {
            return ceRepository.GetCommonEnum(field, enumType);
        }

        public List<SCommonEnum> GetCommonEnumByType(string enumType)
        {
            return ceRepository.GetCommonEnumByType(enumType);
        }

        /// <summary>
        /// 添加枚举
        /// </summary>
        /// <param name="commonEnum"></param>
        /// <returns></returns>
        public int AddCommonEnum(SCommonEnum commonEnum)
        {
            object id = ceRepository.Insert(commonEnum);
            return Convert.ToInt32(id);
        }

        /// <summary>
        /// 添加枚举
        /// </summary>
        /// <param name="commonEnum"></param>
        /// <returns></returns>
        public bool RemoveCommonEnum(int ID)
        {
            return ceRepository.Delete(ID);
        }

        /// <summary>
        /// 添加枚举
        /// </summary>
        /// <param name="commonEnum"></param>
        /// <returns></returns>
        public bool UpdateCommonEnum(SCommonEnum commonEnum)
        {
            return ceRepository.Update(commonEnum);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<SCommonEnum> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = ceRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<SCommonEnum> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return ceRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 创建枚举类
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string CreateEnumClass(string filePath)
        {
            if (ESCFile.IsExistFile(filePath))
            {
                ESCFile.DeleteFile(filePath);
            }
            else
            {
                ESCFile.CreateFile(filePath);
            }

            IEnumerable<IGrouping<string, SCommonEnum>> gruops = ceRepository.QueryAll().GroupBy(t => t.EnumType);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("");
            sb.AppendLine("namespace ESC.Infrastructure.Enums");
            sb.AppendLine("{");
            foreach (IGrouping<string, SCommonEnum> grp in gruops)
            {
                sb.AppendLine("    public class " + grp.Key + "Enum");
                sb.AppendLine("    {");

                foreach (SCommonEnum item in grp)
                {
                    sb.AppendLine("        /// <summary>");
                    sb.AppendLine("        /// " + item.EnumDesc);
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine("        public const int " + item.EnumName + " = " + item.EnumField + ";");
                    sb.AppendLine("");
                }

                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 数据库枚举名称");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        public const string EnumName = \"" + grp.Key + "\";");
                sb.AppendLine("");

                sb.AppendLine("    }");
                sb.AppendLine("");
            }
            sb.AppendLine("}");
            ESCFile.AppendText(filePath, sb.ToString());
            return filePath;
        }
    }
}

using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using Dapper.Extensions;
using System.Linq;
using System.Text;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 单号规则 +
    /// </summary>
    public class SNumberRuleRepository : BaseRepository<SNumberRule>
    {
        #region 构造

        public SNumberRuleRepository() : base() { }

        public SNumberRuleRepository(string connectionString) : base(connectionString) { }

        public SNumberRuleRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句
        /// <summary>
        /// 获取查询的视图
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = @"SELECT R.* ,
                                        U.UserName CreateByUserName ,
                                        U1.UserName UpdateByUserName
                                FROM    SNumberRule R WITH ( NOLOCK )
                                        LEFT JOIN SUser U WITH ( NOLOCK ) ON R.CreateBy = U.ID
                                        LEFT JOIN SUser U1 WITH ( NOLOCK ) ON R.UpdateBy = U1.ID";
            return searchSql;
        }

        /// <summary>
        /// 获取查询的视图
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (WhereItem item in whereItems)
            {
                item.field = "R." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + " ORDER BY R.BusinessType ASC";
        }
        #endregion

        #region 操作

        /// <summary>
        /// 添加序列
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int AddSNumberRule(SNumberRule num)
        {
            //添加序列
            string sql = "CREATE SEQUENCE [Sequence-" + num.BusinessType + "] ";
            sql += " AS [bigint]";
            sql += " START WITH " + num.StartSeq;
            sql += " INCREMENT BY 1";
            sql += " MINVALUE 1";
            sql += " MAXVALUE 9223372036854775807";
            sql += " CACHE";
            //添加单号规则
            num.SeqName = "Sequence-" + num.BusinessType;
            Execute(sql);
            Insert(num);

            return num.ID;
        }

        /// <summary>
        /// 更新序列
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool UpdateSNumberRule(SNumberRule num)
        {
            SNumberRule oldNum = SingleOrDefault(num.ID);

            //删除序列
            string delSeq = "DROP SEQUENCE [Sequence-" + oldNum.BusinessType + "] ";
            //添加序列
            string sql = "CREATE SEQUENCE [Sequence-" + num.BusinessType + "] ";
            sql += " AS [bigint]";
            sql += " START WITH " + num.StartSeq;
            sql += " INCREMENT BY 1";
            sql += " MINVALUE 1";
            sql += " MAXVALUE 9223372036854775807";
            sql += " CACHE";
            //添加单号规则
            num.SeqName = "Sequence-" + num.BusinessType;
            Execute(delSeq);
            Execute(sql);
            return Update(num);
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="num"></param>
        public bool DeleteSNumberRule(SNumberRule num)
        {
            //删除序列
            string delSeq = "DROP SEQUENCE [Sequence-" + num.BusinessType + "] ";
            Execute(delSeq);
            return Delete(num);
        }

        /// <summary>
        /// 自动产生单号
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <returns></returns>
        public string GetNextNumber(string businessType)
        {
            //获取单号
            string sql = "SELECT * FROM SNumberRule WITH(NOLOCK) WHERE BusinessType='" + businessType + "'";
            SNumberRule bill = FirstOrDefault(sql);
            string sqlSeq = "SELECT NEXT VALUE FOR [Sequence-" + businessType + "]";
            string seq = ExecuteScalar<string>(sqlSeq);
            StringBuilder sb = new StringBuilder();
            sb.Append(bill.Prefix);
            for (int i = 0; i < bill.RuleLength - seq.Length; i++)
            {
                sb.Append("0");
            }

            sb.Append(seq);
            return sb.ToString();
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(SNumberRule num, bool isAdd)
        {
            string sql = "SELECT BusinessType FROM SNumberRule WITH (NOLOCK) WHERE BusinessType='" + num.BusinessType + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + num.ID;
            }
            string rest = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rest);
        }

        /// <summary>
        /// 获取当前值
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public int GetCurrentNumber(string businessType)
        {
            string sql = "SELECT current_value FROM sys.sequences WHERE name='Sequence-" + businessType + "'";
            return ExecuteScalar<int>(sql);
        }
        #endregion
    }
}

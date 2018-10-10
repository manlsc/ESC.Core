using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESC.Infrastructure.DomainObjects;
using System.Data;
using ESC.Infrastructure.Repository;
using ESC.Core;
using Dapper;
using Dapper.Extensions;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 资源 +
    /// </summary>
    public class SResourceRepository : BaseRepository<SResource>
    {
        #region 构造

        public SResourceRepository()
            : base()
        {
        }

        public SResourceRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SResourceRepository(DatabaseContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询的视图
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = @"SELECT RS.* ,
                                        P.ResourceDesc ParentName
                                 FROM   SResource RS WITH ( NOLOCK )
                                        LEFT JOIN SResource P WITH ( NOLOCK ) ON RS.ParentID = P.ID";
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
                if (item.field == "ParentName")
                {
                    item.field = "P." + item.field;
                }
                else
                {
                    item.field = "RS." + item.field;
                }
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + " ORDER BY RS.OrderIndex ASC";
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <returns></returns>
        protected string GetOpeartorSearchSql()
        {
            string searchSql = "SELECT O.* FROM SOperator O WITH(NOLOCK)";
            return searchSql;
        }

        #endregion

        #region 操作

        /// <summary>
        /// 根据资源获取按钮
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public List<SOperator> GetOperatorByResource(int resourceId)
        {
            string sql = "SELECT O.* FROM SOperator O WITH(NOLOCK) WHERE O.ResourceID=" + resourceId + " ORDER BY O.OrderIndex";
            return dbContext.Connection.Query<SOperator>(sql).ToList();
        }

        /// <summary>
        /// 获取子资源
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<SResource> GetChildResource(int parentId)
        {
            return Query(GetSearchSql() + " WHERE RS.ParentID=" + parentId);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="SResource"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(SResource SResource, bool isAdd)
        {
            string sql = "SELECT ResourceName FROM SResource WITH(NOLOCK) WHERE ResourceName='" + SResource.ResourceName + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + SResource.ID;
            }
            string rest = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rest);
        }

        /// <summary>
        /// 删除资源的操作
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int RemoveOperatorByID(int operatorId)
        {
            return Execute("DELETE FROM SOperator WHERE ID=" + operatorId);
        }

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        public SResource GetResourcByID(int operatorId)
        {
            string sql = GetSearchSql() + " WHERE RS.ID=" + operatorId;
            return FirstOrDefault(sql);
        }

        /// <summary>
        /// 删除资源操作
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int RemoveOperatorByResourceID(int operatorId)
        {
            return Execute("DELETE FROM SOperator WHERE ResourceID=" + operatorId);
        }


        #endregion
    }
}

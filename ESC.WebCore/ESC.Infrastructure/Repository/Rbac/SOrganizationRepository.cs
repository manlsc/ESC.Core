using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESC.Infrastructure.DomainObjects;
using ESC.Core;
using ESC.Infrastructure.Repository;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 组织机构 +
    /// </summary>
    public class SOrganizationRepository : BaseRepository<SOrganization>
    {
        #region 构造

        public SOrganizationRepository()
            : base()
        {
        }

        public SOrganizationRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SOrganizationRepository(DatabaseContext dbContext)
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
            string searchSql = @"SELECT O.* ,
                                        U.UserName ,
                                        P.OrgName ParentName,
                                        CE.EnumDesc AS InUseName
                                FROM    SOrganization O WITH(NOLOCK)
                                        LEFT JOIN SUser U WITH(NOLOCK) ON O.UserID = U.ID
                                        LEFT JOIN SOrganization P WITH(NOLOCK) ON O.ParentID = P.ID
                                        LEFT JOIN SCommonEnum CE WITH(NOLOCK) ON O.InUse = CE.EnumField AND CE.EnumType = 'Bool'";
            return searchSql;
        }

        /// <summary>
        /// 构造条件
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (WhereItem item in whereItems)
            {
                item.field = "O." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + " ORDER BY O.OrgCode DESC";
        }

        #endregion

        #region 操作

        /// <summary>
        /// 获取结构组织
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SOrganization GetOrganizationById(int Id)
        {
            return FirstOrDefault(GetSearchSql() + " WHERE O.ID=" + Id);
        }

        /// <summary>
        /// 获取子结构组织
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<SOrganization> GetChildOrganization(int Id)
        {
            return Query(GetSearchSql() + " WHERE O.ParentID=" + Id);
        }

        /// <summary>
        /// 获取结构组织
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<SOrganization> GetOrganizationByName(string OrgName)
        {
            return Query(GetSearchSql() + " WHERE O.OrgName like '%" + OrgName + "%'");
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="org"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(SOrganization org, bool isAdd)
        {
            string sql = "SELECT ID FROM SOrganization WITH(NOLOCK) WHERE OrgCode='" + org.OrgCode + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + org.ID;
            }
            string rst = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rst);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<SOrganization> GetOrganizationCodeName(string codeName)
        {
            string sqlSelect = GetSearchSql() + string.Format(" WHERE O.OrgCode LIKE '%{0}%' OR O.OrgName LIKE '%{0}%'", codeName);
            return Query(sqlSelect);
        }
        #endregion
    }
}

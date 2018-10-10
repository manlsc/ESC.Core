using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 物料组
    /// </summary>
    public class BMaterialGroupRepository : BaseRepository<BMaterialGroup>
    {

        #region 构造

        public BMaterialGroupRepository() : base() { }

        public BMaterialGroupRepository(string connectionString) : base(connectionString) { }

        public BMaterialGroupRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = "SELECT MG.*,CE.EnumDesc AS InUseName,O.OrgName,CU.UserName CreateByUserName,UU.UserName UpdateByUserName,PT.GroupName AS ParentName FROM BMaterialGroup MG";
            searchSql += " LEFT JOIN SCommonEnum CE ON MG.InUse=CE.EnumField AND CE.EnumType='InUse'";
            searchSql += " LEFT JOIN SOrganization O ON MG.OrgID=O.ID";
            searchSql += " LEFT JOIN BMaterialGroup PT ON MG.ParentID=PT.ID";
            searchSql += " LEFT JOIN SUser CU ON MG.CreateBy=CU.ID";
            searchSql += " LEFT JOIN SUser UU ON MG.UpdateBy=UU.ID";
            return searchSql;
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (WhereItem item in whereItems)
            {
                item.field = "MG." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        public string GetOrderBy()
        {
            return " ORDER BY MG.GroupCode ASC";
        }
        #endregion

        #region 操作

        public BMaterialGroup GetMaterialGroupById(int groupId)
        {
            string sql = GetSearchSql() + " WHERE MG.ID=" + groupId;
            return SingleOrDefault(sql);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="location"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(BMaterialGroup group, bool isAdd)
        {
            string sql = "SELECT ID FROM BMaterialGroup WHERE GroupCode='" + group.GroupCode + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + group.ID;
            }
            string rst = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rst);
        }

        /// <summary>
        /// 获取子组
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<BMaterialGroup> GetChildMaterialGroups(int parentId)
        {
            string sql = GetSearchSql() + " WHERE MG.ParentID=" + parentId;
            return Query(sql);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BMaterialGroup> GetGroupCodeName(string codeName)
        {
            string sqlSelect = GetSearchSql() + string.Format(" WHERE MG.GroupCode LIKE '%{0}%' OR MG.GroupName LIKE '%{0}%'", codeName);
            return Query(sqlSelect);
        }
        #endregion
    }
}

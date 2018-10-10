using System;
using System.Collections.Generic;
using System.Linq;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 物料
    /// </summary>
    public class BMaterialRepository : BaseRepository<BMaterial>
    {

        #region 构造

        public BMaterialRepository() : base() { }

        public BMaterialRepository(string connectionString) : base(connectionString) { }

        public BMaterialRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = "SELECT M.*,CE.EnumDesc AS InUseName,O.OrgName,BU.UnitName,CU.UserName CreateByUserName,UU.UserName UpdateByUserName,L.LocationDesc,MG.GroupName AS MaterialGroupName FROM BMaterial M WITH (NOLOCK)";
            searchSql += " LEFT JOIN SCommonEnum CE WITH (NOLOCK) ON M.InUse=CE.EnumField AND CE.EnumType='InUse'";
            searchSql += " LEFT JOIN SOrganization O WITH (NOLOCK) ON M.OrgID=O.ID";
            searchSql += " LEFT JOIN BUnit BU WITH (NOLOCK) ON M.UnitID=BU.ID";
            searchSql += " LEFT JOIN BMaterialGroup MG WITH (NOLOCK) ON M.MaterialGroupID=MG.ID";
            searchSql += " LEFT JOIN BLocation L WITH (NOLOCK) ON M.LoctionID=L.ID";
            searchSql += " LEFT JOIN SUser CU WITH (NOLOCK) ON M.CreateBy=CU.ID";
            searchSql += " LEFT JOIN SUser UU WITH (NOLOCK) ON M.UpdateBy=UU.ID";
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
                item.field = "M." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        public string GetOrderBy()
        {
            return " ORDER BY M.MaterialCode ASC";
        }
        #endregion

        #region 操作

        public BMaterial GetMaterialById(int bpId)
        {
            string sqlSelect = GetSearchSql() + " WHERE M.ID=" + bpId;
            return FirstOrDefault(sqlSelect);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BMaterial> GetMaterialCodeName(string codeName)
        {
            string sqlSelect = GetSearchSql() + string.Format(" WHERE M.MaterialCode LIKE '%{0}%' OR M.MaterialName LIKE '%{0}%'", codeName);
            return Query(sqlSelect);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="material"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(BMaterial material, bool isAdd)
        {
            string sql = "SELECT MaterialCode FROM BMaterial WITH (NOLOCK) WHERE MaterialCode='" + material.MaterialCode + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + material.ID;
            }
            string rest = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rest);
        }

        /// <summary>
        /// 根据物料编码查询物料
        /// </summary>
        /// <param name="matCode"></param>
        /// <returns></returns>
        public BMaterial GetMaterialByCode(string matCode)
        {
            string sql = "SELECT * FROM BMaterial WITH (NOLOCK) WHERE MaterialCode='" + matCode + "'";
            return FirstOrDefault(sql);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESC.Infrastructure.DomainObjects;
using ESC.Core;


namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 基本计量单位 +
    /// </summary>
    public class BUnitRepository : BaseRepository<BUnit>
    {
        #region 构造

        public BUnitRepository()
            : base()
        {
        }

        public BUnitRepository(string connectionString)
            : base(connectionString)
        {
        }

        public BUnitRepository(DatabaseContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = "SELECT U.*,O.OrgName,CU.UserName CreateByUserName,UU.UserName UpdateByUserName,CE.EnumDesc as UnitTypeName,CE2.EnumDesc as InUseName";
            searchSql += " FROM BUnit U";
            searchSql += " LEFT JOIN SOrganization O ON U.OrgID=O.ID";
            searchSql += " LEFT JOIN SUser CU ON U.CreateBy=CU.ID";
            searchSql += " LEFT JOIN SUser UU ON U.UpdateBy=UU.ID";
            searchSql += " LEFT JOIN SCommonEnum CE ON U.UnitType=CE.EnumField AND CE.EnumType='UnitType'";
            searchSql += " LEFT JOIN SCommonEnum CE2 ON U.InUse=CE2.EnumField AND CE2.EnumType='InUse'";
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
                item.field = "U." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        protected string GetOrderBy()
        {
            return " ORDER BY U.UnitCode";
        }

        #endregion

        #region 操作

        /// <summary>
        ///  根据编码获取计量单位
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<BUnit> GetBaseUnitByCode(string unitCode)
        {
            return Query(GetSearchSql() + " WHERE U.UnitCode = '" + unitCode + "'" + GetOrderBy());
        }

        /// <summary>
        ///  根据名称获取计量单位
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<BUnit> GetBaseUnitByName(string unitName)
        {
            return Query(GetSearchSql() + " WHERE U.UnitName = '" + unitName + "'" + GetOrderBy());
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(BUnit model, bool isAdd)
        {
            string sql = string.Format("SELECT UnitCode FROM BUnit WHERE UnitCode='{0}'", model.UnitCode);
            if (!isAdd)
            {
                sql += " AND ID!=" + model.ID;
            }
            string rest = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rest);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BUnit> GetUnitCodeName(string codeName)
        {
            string sqlSelect = GetSearchSql() + string.Format(" WHERE U.UnitCode LIKE '%{0}%' OR U.UnitName LIKE '%{0}%'", codeName);
            return Query(sqlSelect);
        }
        #endregion

    }
}

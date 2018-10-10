using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using System.Linq;
using System.Data;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 存储单元
    /// </summary>
    public class BLocationRepository : BaseRepository<BLocation>
    {

        #region 构造

        public BLocationRepository() : base() { }

        public BLocationRepository(string connectionString) : base(connectionString) { }

        public BLocationRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = @"SELECT  L.* ,
                                        U.UserName AS CreateByUserName ,
                                        U1.UserName AS UpdateByUserName ,
                                        PL.LocationCode AS ParentCode ,
                                        PL.LocationDesc AS ParentName ,
                                        O.OrgName AS OrgName ,
                                        CE.EnumDesc AS LocationTypeName ,
                                        CE1.EnumDesc AS InUseName ,
                                        CE2.EnumDesc AS LocationClassName
                                FROM    BLocation L WITH ( NOLOCK )
                                        LEFT JOIN SUser U WITH ( NOLOCK ) ON L.CreateBy = U.ID
                                        LEFT JOIN SUser U1 WITH ( NOLOCK ) ON L.UpdateBy = U1.ID
                                        LEFT JOIN BLocation PL WITH ( NOLOCK ) ON L.ParentID = PL.ID
                                        LEFT JOIN SOrganization O WITH ( NOLOCK ) ON L.OrgID = O.ID
                                        LEFT JOIN SCommonEnum CE WITH ( NOLOCK ) ON L.LocationType = CE.EnumField AND CE.EnumType = 'LocationType'
                                        LEFT JOIN SCommonEnum CE2 WITH ( NOLOCK ) ON L.LocationClass = CE2.EnumField AND CE2.EnumType = 'LocationClass'
                                        LEFT JOIN SCommonEnum CE1 WITH ( NOLOCK ) ON L.InUse = CE.EnumField AND CE1.EnumField = 'Bool'";
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
                item.field = "L." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }


        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        protected string GetOrderBy()
        {
            return " ORDER BY L.ID";
        }

        #endregion

        #region 操作

        /// <summary>
        /// 根据ID 查询存储单元
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public BLocation GetLocationById(int locationId)
        {
            string sql = GetSearchSql() + " WHERE L.ID=" + locationId;
            return SingleOrDefault(sql);
        }


        /// <summary>
        /// 根据编码获取存储单元
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationCodeName(string codeName)
        {
            string sql = GetSearchSql() + string.Format(" WHERE L.LocationCode LIKE '%{0}%' OR L.LocationDesc LIKE '%{0}%'", codeName);
            return Query(sql);
        }

        /// <summary>
        /// 根据编码获取存储单元
        /// </summary>
        /// <param name="codeName">货位编码或名称</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        public List<BLocation> GetLocationCodeNameWithWh(string codeName, int warehouseId)
        {
            string sql = GetSearchSql() + string.Format(" WHERE (L.LocationCode LIKE '%{0}%' OR L.LocationDesc LIKE '%{0}%') AND L.TopLocationID={1} AND L.LocationType>1", codeName, warehouseId);
            return Query(sql);
        }

        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationCodeNameWh(string codeName)
        {
            string sql = GetSearchSql() + string.Format(" WHERE (L.LocationCode LIKE '%{0}%' OR L.LocationDesc LIKE '%{0}%') AND L.LocationType=1", codeName);
            return Query(sql);
        }

        /// <summary>
        /// 获取非仓库存储单元
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationCodeNameNotWh(string codeName)
        {
            string sql = GetSearchSql() + string.Format(" WHERE (L.LocationCode LIKE '%{0}%' OR L.LocationDesc LIKE '%{0}%') AND L.LocationType>1", codeName);
            return Query(sql);
        }

        /// <summary>
        /// 获取所有仓库
        /// </summary>
        /// <returns></returns>
        public List<BLocation> GetTopLocations()
        {
            return Query("SELECT * FROM BLocation WITH (NOLOCK) WHERE LocationType=1");
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="location"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(BLocation location, bool isAdd)
        {
            string sql = "SELECT ID FROM BLocation WITH (NOLOCK) WHERE LocationCode='" + location.LocationCode + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + location.ID;
            }
            string rst = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rst);
        }

        /// <summary>
        /// 获取默认货区
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public BLocation GetDefaultLocation(int parentId, int locationClass)
        {
            string sql = GetSearchSql() + " WHERE L.IsDefault=1 AND L.ParentID=" + parentId + " AND L.LocationClass=" + locationClass;
            return SingleOrDefault(sql);
        }

        /// <summary>
        /// 根据id
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationByParentID(int locationId)
        {
            string sql = GetSearchSql() + " WHERE L.ParentID=" + locationId;
            return Query(sql);
        }

        /// <summary>
        /// 根据物料组查询存储单元
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<BLocation> GetLoctionsByGroup(int groupId)
        {
            string sql = GetSearchSql();
            sql += " LEFT JOIN BMatGrpLocRel MGLR WITH (NOLOCK) ON L.ID=MGLR.LocationID WHERE MGLR.MatGroupID=" + groupId + GetOrderBy();
            return Query(sql);
        }

        /// <summary>
        /// 根据库管员获取负责仓库
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public DataTable GetLocationsUserRel(string userCode)
        {
            string sql = "SELECT L.ID,L.LocationCode FROM SUser U  WITH (NOLOCK)";
            sql += " INNER JOIN BLocationUserRel LUR WITH (NOLOCK) ON U.ID=LUR.UserID";
            sql += " INNER JOIN BLocation L WITH (NOLOCK) ON LUR.LocationID=L.ID AND L.LocationType=1";
            sql += " WHERE U.UserCode='" + userCode + "'";
            return QueryTable(sql);
        }

        /// <summary>
        /// 根据用户获取仓库权限
        /// <remarks>当前组织机构及子组织机构的仓库权限</remarks>
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public DataTable GetLocationsUser(string userCode)
        {
            //获取用户所属的组织机构
            string sql = "SELECT U.OrgID FROM SUser U WITH (NOLOCK) WHERE U.UserCode='" + userCode + "'";
            string orgId = ExecuteScalar<string>(sql);
            if (string.IsNullOrWhiteSpace(orgId) || orgId == "0")
            {
                return null;
            }

            List<string> orgList = new List<string>();
            //获取用户组织机构下面的所有子机构
            sql = "SELECT ID,ParentID FROM SOrganization WITH (NOLOCK) WHERE ID>" + orgId;
            DataTable dt = QueryTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["ParentID"].ToString().Equals(orgId))
                    {
                        orgList.Add(row["ID"].ToString());
                        GetChildOrg(dt, row["ID"].ToString(), orgList);
                    }
                }
            }
            if (orgList.Count < 1)
            {
                return null;
            }
            //根据机构列表获取仓库列表
            sql = "SELECT L.ID,L.LocationCode FROM BLocation L WHERE L.LocationType=1 AND L.OrgID IN (" + string.Join(",", orgList) + ")";
            return QueryTable(sql);
        }

        /// <summary>
        /// 获取组织机构集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private void GetChildOrg(DataTable dt, string orgId, List<string> orgList)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["ParentID"].ToString().Equals(orgId))
                {
                    orgList.Add(row["ID"].ToString());
                    GetChildOrg(dt, row["ID"].ToString(), orgList);
                }
            }
        }

        /// <summary>
        /// 根据物料组获取默认存储单元
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<BLocation> GetLocationsByMaterialGroup(int groupId)
        {
            string sql = GetSearchSql() + " LEFT JOIN BMaterialGroupLocationRel MGR WITH (NOLOCK) ON MGR.LocationID=L.ID WHERE MaterialGroupID=" + groupId;
            return Query(sql);
        }
        #endregion
    }
}

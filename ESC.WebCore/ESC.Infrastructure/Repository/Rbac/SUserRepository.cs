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
    /// 用户 +
    /// </summary>
    public class SUserRepository : BaseRepository<SUser>
    {
        #region 构造

        public SUserRepository()
            : base()
        {
        }

        public SUserRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SUserRepository(DatabaseContext dbContext)
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
            string searchSql = @"SELECT  U.* ,
                                        O.OrgName ,
                                        CE.EnumDesc SuperUserName ,
                                        CU.UserName CreateByUserName ,
                                        UU.UserName UpdateByUserName ,
                                        LU.UserName AS LeaderName
                                FROM    SUser U WITH ( NOLOCK )
                                        LEFT JOIN SOrganization O WITH ( NOLOCK ) ON U.OrgID = O.ID
                                        LEFT JOIN SCommonEnum CE WITH ( NOLOCK ) ON U.SuperUser = CE.EnumField AND CE.EnumType = 'Bool'
                                        LEFT JOIN SUser CU WITH ( NOLOCK ) ON U.CreateBy = CU.ID
                                        LEFT JOIN SUser UU WITH ( NOLOCK ) ON U.UpdateBy = UU.ID
                                        LEFT JOIN SUser LU WITH ( NOLOCK ) ON U.LeaderID = LU.ID";
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
        /// 获取角色下面的所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetUsersByRoleIdSql(int roleId)
        {
            return GetSearchSql() + " LEFT JOIN SUserRole UR WITH (NOLOCK) ON U.ID=UR.UserID WHERE UR.RoleID=" + roleId + GetOrderBy();

        }

        /// <summary>
        /// 根据组织结构查询用户
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public string GetUsersByOrgIdSql(int orgId)
        {
            return GetSearchSql() + " WHERE U.OrgID=" + orgId + GetOrderBy();
        }

        /// <summary>
        /// 根据仓库查询库管
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public string GetUsersByLoctionId(int locationId)
        {
            return GetSearchSql() + " LEFT JOIN BLocationUserRel UR WITH (NOLOCK) ON U.ID=UR.UserID WHERE UR.LocationID=" + locationId + GetOrderBy();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public string GetOrderBy()
        {
            return " ORDER BY U.UserCode";
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public int RemoveOrg(int orgId)
        {
            string sql = "UPDATE SUser SET OrgID=0 WHERE OrgID=" + orgId;
            return Execute(sql);
        }

        #endregion

        #region 操作

        /// <summary>
        ///  根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<SUser> GetUserByName(string userName)
        {
            return Query(GetSearchSql() + " WHERE U.UserName = '" + userName + "'" + GetOrderBy());
        }

        /// <summary>
        /// 根据用户编码获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<SUser> GetUserByCode(string userCode)
        {
            return Query(GetSearchSql() + " WHERE U.UserCode = '" + userCode + "'" + GetOrderBy());
        }

        /// <summary>
        /// 获取当前组织机构的所有职员
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public List<SUser> GetUserByOrganizationId(string organizationId)
        {
            string sql = GetSearchSql() + " WHERE U.OrgID=" + organizationId + GetOrderBy();
            return Query(sql);
        }

        /// <summary>
        /// 获取角色下面的所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<SUser> GetUsersByRoleId(int roleId)
        {
            return Query(GetUsersByRoleIdSql(roleId));
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(SUser user, bool isAdd)
        {
            string sql = "SELECT UserCode FROM SUser WITH (NOLOCK) WHERE UserCode='" + user.UserCode + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + user.ID;
            }
            string rest = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rest);
        }

        /// <summary>
        /// 删除用户和角色关系
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int RemoveUserRoleByUser(int userId)
        {
            return Execute("DELETE FROM SUserRole WHERE UserID=" + userId);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ResetPassword(string pwd, int userId)
        {
            string sql = "UPDATE SUser SET Pwd='" + pwd + "' WHERE ID=" + userId;
            return Execute(sql);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<SUser> GetUserCodeName(string codeName)
        {
            string sqlSelect = GetSearchSql() + string.Format(" WHERE U.UserCode LIKE '%{0}%' OR U.UserName LIKE '%{0}%'", codeName);
            return Query(sqlSelect);
        }
        #endregion
    }
}

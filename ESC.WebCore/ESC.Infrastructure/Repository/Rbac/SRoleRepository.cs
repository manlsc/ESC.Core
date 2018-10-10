using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESC.Infrastructure.DomainObjects;
using ESC.Core;
using Dapper;
using Dapper.Extensions;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 角色
    /// </summary>
    public class SRoleRepository : BaseRepository<SRole>
    {
        #region 构造

        public SRoleRepository()
            : base()
        {
        }

        public SRoleRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SRoleRepository(DatabaseContext dbContext)
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
            string searchSql = @"SELECT  R.* ,
                                        U.UserName AS CreateByUserName
                                FROM    SRole R WITH ( NOLOCK )
                                        LEFT JOIN SUser U WITH ( NOLOCK ) ON R.CreateBy = U.ID";
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
            return GetSearchSql() + BulidWhereSql(whereItems) + " ORDER BY R.RoleName ASC";
        }

        #endregion

        #region 操作

        /// <summary>
        /// 根据角色ID获取角色用户关系
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<SUserRole> GetUserRoleByRole(int roleId)
        {
            return dbContext.Connection.Query<SUserRole>("SELECT * FROM SUserRole WITH(NOLOCK) WHERE RoleID=" + roleId).ToList();
        }

        /// <summary>
        /// 添加用户角色关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int AddUserRole(int roleId, int userId)
        {
            SUserRole ur = new SUserRole()
            {
                RoleID = roleId,
                UserID = userId
            };
            dbContext.Connection.Insert(ur);
            return ur.ID;
        }

        /// <summary>
        /// 根据用户ID获取角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SRole> GetRoleByUserId(int userId)
        {
            return Query("SELECT R.* FROM SRole R WITH(NOLOCK) LEFT JOIN SUserRole WITH(NOLOCK) UR ON R.ID=UR.RoleID WHERE R.UserID=" + userId);
        }

        /// <summary>
        /// 删除用户和角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int RemoveUserRole(int userId, int roleId)
        {
            return Execute("DELETE FROM SUserRole WHERE UserID=" + userId + " AND RoleID=" + roleId);
        }

        /// <summary>
        /// 删除用户和角色关系
        /// </summary>
        /// <param name="roleId"></param>
        public int RemoveUserRoleByRole(int roleId)
        {
            return Execute("DELETE FROM SUserRole WHERE RoleID=" + roleId);
        }

        /// <summary>
        /// 判断角色是否存在
        /// </summary>
        /// <param name="role"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(SRole role, bool isAdd)
        {
            string sql = "SELECT RoleName FROM SRole WITH(NOLOCK) WHERE RoleName='" + role.RoleName + "'";
            if (!isAdd)
            {
                sql += " AND ID!=" + role.ID;
            }
            string rest = ExecuteScalar<string>(sql);
            return string.IsNullOrWhiteSpace(rest);
        }

        #endregion
    }
}

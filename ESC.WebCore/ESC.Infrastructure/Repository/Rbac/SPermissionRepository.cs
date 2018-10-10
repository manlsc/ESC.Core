using Dapper;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.Repository
{
    public class SPermissionRepository : BaseRepository<SOPermission>
    {
        #region 构造

        public SPermissionRepository()
            : base()
        {
        }

        public SPermissionRepository(string connectionString)
            : base(connectionString)
        {
        }

        public SPermissionRepository(DatabaseContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        #region 操作

        /// <summary>
        /// 根据用户获取菜单权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public List<SResource> GetRPermissionByUser(int userId, bool isAdmin)
        {
            //如果不是管理员,则只能查询自己的权限
            string strSql = "";
            if (isAdmin)
            {
                strSql = "SELECT RS.* FROM SResource RS WITH(NOLOCK) ORDER BY RS.OrderIndex ASC";
            }
            else
            {
                strSql = "SELECT DISTINCT RS.* FROM SUser U WITH(NOLOCK) ";
                strSql += " INNER JOIN SUserRole UR WITH(NOLOCK) ON U.ID=UR.UserID";
                strSql += " INNER JOIN SRole R WITH(NOLOCK) ON UR.RoleID=R.ID";
                strSql += " INNER JOIN SRolePermission RP WITH(NOLOCK) ON R.ID=RP.RoleID";
                strSql += " INNER JOIN SResource RS WITH(NOLOCK) ON RP.ResourceID=RS.ID";
                strSql += " WHERE U.ID=" + userId + " ORDER BY RS.OrderIndex ASC";
            }
            return dbContext.Connection.Query<SResource>(strSql).ToList();
        }

        /// <summary>
        /// 根据角色ID获取菜单权限
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<SResource> GetRPermissionByRole(int roleId)
        {
            string strSql = "SELECT RS.* FROM SResource RS WITH(NOLOCK)";
            strSql += " LEFT JOIN SRolePermission RP WITH(NOLOCK) ON RP.ResourceID=RS.ID";
            strSql += " WHERE RP.RoleID=" + roleId + " ORDER BY RS.OrderIndex ASC";
            return dbContext.Connection.Query<SResource>(strSql).ToList();
        }

        /// <summary>
        /// 根据用户获取按钮权限
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="controllerName">控制器名称</param>
        /// <returns></returns>
        public List<SOperator> GetOPermissionByUserAndResource(SUser user, string resourceName)
        {
            //如果不是管理员,则只能查询自己的权限
            string strSql = "";
            if (user.SuperUser == BoolEnum.Yes)
            {
                strSql = "  SELECT O.* FROM SOperator O WITH(NOLOCK) LEFT JOIN SResource RS WITH(NOLOCK) ON O.ResourceID=RS.ID";
                strSql += " WHERE RS.ResourceName='" + resourceName + "'";
                strSql += " ORDER BY O.OrderIndex ASC";
            }
            else
            {
                strSql = "  SELECT DISTINCT O.* FROM SOperator O WITH(NOLOCK)";
                strSql += " LEFT JOIN SOPermission OP WITH(NOLOCK) ON OP.OperatorID= O.ID";
                strSql += " LEFT JOIN SRolePermission RP WITH(NOLOCK) ON RP.ResourceID=OP.ResourceID";
                strSql += " LEFT JOIN SRole R WITH(NOLOCK) ON RP.RoleID=R.ID";
                strSql += " LEFT JOIN SUserRole UR WITH(NOLOCK) ON R.ID=UR.RoleID";
                strSql += " LEFT JOIN SUser U WITH(NOLOCK) ON U.ID=UR.UserID";
                strSql += " LEFT JOIN SResource RS WITH(NOLOCK) ON RP.ResourceID=RS.ID";
                strSql += " WHERE RS.ResourceName='" + resourceName + "' AND U.ID=" + user.ID;
                strSql += " ORDER BY O.OrderIndex ASC";

            }
            return dbContext.Connection.Query<SOperator>(strSql).ToList();
        }

        /// <summary>
        /// 根据角色ID获取按钮权限
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="controllerName">控制器名称</param>
        /// <returns></returns>
        public List<SOperator> GetOPermissionByRoleAndResource(int roleId, string resourceName)
        {
            //如果不是管理员,则只能查询自己的权限
            string strSql = "";
            strSql += " SELECT O.* FROM SOperator O WITH(NOLOCK)";
            strSql += " LEFT JOIN SOPermission OP WITH(NOLOCK) ON OP.OperatorID= O.ID";
            strSql += " LEFT JOIN SRolePermission WITH(NOLOCK) RP ON RP.ResourceID=OP.ResourceID ";
            strSql += " LEFT JOIN SRole R WITH(NOLOCK) ON RP.RoleID=R.ID";
            strSql += " LEFT JOIN SResource RS WITH(NOLOCK) ON RP.ResourceID=RS.ID";
            strSql += " WHERE RS.ResourceName='" + resourceName + "' AND R.ID=" + roleId;
            strSql += " ORDER BY O.OrderIndex ASC";

            return dbContext.Connection.Query<SOperator>(strSql).ToList();
        }

        /// <summary>
        /// 根据用户获取列权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tableName"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public List<SCPermission> GetCPermissionByUserAndResource(SUser user, string tableName, string resourceName)
        {
            //如果不是管理员,则只能查询自己的权限
            string strSql = "";
            List<SCPermission> cPerms = new List<SCPermission>();
            if (user.SuperUser == BoolEnum.No)
            {
                strSql = "  SELECT CP.*,CE1.EnumDesc AS VisibleName,CE2.EnumName AS DisabledName,CE3.EnumName AS RequiredName FROM SCPermission CP WITH(NOLOCK)";
                strSql += " LEFT JOIN SRole R WITH(NOLOCK) ON CP.RoleID=R.ID";
                strSql += " LEFT JOIN SUserRole UR WITH(NOLOCK) ON R.ID=UR.RoleID";
                strSql += " LEFT JOIN SUser U WITH(NOLOCK) ON U.ID=UR.UserID";
                strSql += " LEFT JOIN SCommonEnum CE1 WITH(NOLOCK) ON CP.Visible=CE1.EnumField AND CE1.EnumType='Bool'";
                strSql += " LEFT JOIN SCommonEnum CE2 WITH(NOLOCK) ON CP.Disabled=CE2.EnumField AND CE2.EnumType='Bool'";
                strSql += " LEFT JOIN SCommonEnum CE3 WITH(NOLOCK) ON CP.Required=CE3.EnumField AND CE3.EnumType='Bool'";
                strSql += " WHERE CP.TableName='" + tableName + "' AND U.ID=" + user.ID ;
                strSql += " ORDER BY CP.OrderIndex ASC";
                cPerms = dbContext.Connection.Query<SCPermission>(strSql).ToList();
            }
            if (cPerms.Count < 1)
            {
                strSql = "  SELECT C.*,CE1.EnumDesc AS VisibleName,CE2.EnumName AS DisabledName,CE3.EnumName AS RequiredName FROM SColumn C WITH(NOLOCK)";
                strSql += " LEFT JOIN SCommonEnum CE1 WITH(NOLOCK) ON C.Visible=CE1.EnumField AND CE1.EnumType='Bool'";
                strSql += " LEFT JOIN SCommonEnum CE2 WITH(NOLOCK) ON C.Disabled=CE2.EnumField AND CE2.EnumType='Bool'";
                strSql += " LEFT JOIN SCommonEnum CE3 WITH(NOLOCK) ON C.Required=CE3.EnumField AND CE3.EnumType='Bool'";
                strSql += " WHERE C.TableName='" + tableName + "'";
                strSql += " ORDER BY C.OrderIndex ASC";
                cPerms = dbContext.Connection.Query<SCPermission>(strSql).ToList();
            }
            return cPerms;
        }

        /// <summary>
        /// 根据角色获取列权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<SCPermission> GetCPermissionByRoleAndResource(int roleId, string tableName, string resourceName)
        {
            List<SCPermission> cPerms = new List<SCPermission>();
            string strSql = "SELECT CP.*,CE1.Name AS VisibleName,CE2.Name AS DisabledName,CE3.Name AS RequiredName FROM SCPermission CP WITH(NOLOCK)";
            strSql += " LEFT JOIN SRole R WITH(NOLOCK) ON CP.RoleID=R.ID";
            strSql += " LEFT JOIN SCommonEnum CE1 WITH(NOLOCK) ON CP.Visible=CE1.Field AND CE1.EnumType='Bool'";
            strSql += " LEFT JOIN SCommonEnum CE2 WITH(NOLOCK) ON CP.Disabled=CE2.Field AND CE2.EnumType='Bool'";
            strSql += " LEFT JOIN SCommonEnum CE3 WITH(NOLOCK) ON CP.Required=CE3.Field AND CE3.EnumType='Bool'";
            strSql += " WHERE CP.TableName='" + tableName + "' AND R.ID=" + roleId + " AND CP.ResourceName='" + resourceName + "'";
            strSql += " ORDER BY CP.OrderIndex ASC";
            cPerms = dbContext.Connection.Query<SCPermission>(strSql).ToList();
            if (cPerms.Count < 1)
            {
                strSql = "  SELECT C.*,CE1.EnumDesc AS VisibleName,CE2.EnumName AS DisabledName,CE3.EnumName AS RequiredName FROM SColumn C WITH(NOLOCK)";
                strSql += " LEFT JOIN SCommonEnum CE1 WITH(NOLOCK) ON C.Visible=CE1.EnumField AND CE1.EnumType='Bool'";
                strSql += " LEFT JOIN SCommonEnum CE2 WITH(NOLOCK) ON C.Disabled=CE2.EnumField AND CE2.EnumType='Bool'";
                strSql += " LEFT JOIN SCommonEnum CE3 WITH(NOLOCK) ON C.Required=CE3.EnumField AND CE3.EnumType='Bool'";
                strSql += " WHERE C.TableName='" + tableName + "'";
                strSql += " ORDER BY C.OrderIndex ASC";
                cPerms = dbContext.Connection.Query<SCPermission>(strSql).ToList();
            }
            return cPerms;
        }

        /// <summary>
        /// 根据角色删除角色权限关系
        /// </summary>
        /// <param name="roleId"></param>
        public void RemoveResPermissionByRole(int roleId)
        {
            Execute("DELETE FROM SRolePermission WHERE RoleID=" + roleId);
        }

        /// <summary>
        /// 根据角色删除权限  
        /// </summary>
        /// <param name="roleId"></param>
        public int RemovePermissionByRole(int roleId)
        {

            int result = Execute("DELETE FROM SOPermission WHERE RoleID = " + roleId);
            Execute("DELETE FROM SRolePermission WHERE RoleID = " + roleId);
            return result;
        }

        /// <summary>
        /// 根据角色获取
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<string> GetOPermissionByRole(int roleId)
        {
            string strSql = "SELECT DISTINCT 'btn'+CONVERT(NVARCHAR(10),OperatorID) AS RID FROM SOPermission WITH(NOLOCK) WHERE RoleID=" + roleId;
            strSql += " UNION ALL";
            strSql += " SELECT DISTINCT CONVERT(NVARCHAR(10),ResourceID) AS RID FROM SRolePermission WITH(NOLOCK) WHERE RoleID=" + roleId;
            return dbContext.Connection.Query<string>(strSql).ToList();
        }

        /// <summary>
        /// 根据资源、表和角色删除列权限
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int RemoveCPermissionByTableAndRole(string tableName, int roleId)
        {
            return Execute("DELETE FROM SCPermission WHERE TableName='" + tableName + "' AND RoleID=" + roleId);
        }

        /// <summary>
        /// 初始化角色菜单
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public int InitMenu(int roleId)
        {
            //先删除
            string delSql = "DELETE FROM SCPermission WHERE RoleID=" + roleId;
            return Execute(delSql);
        }

        #endregion
    }
}

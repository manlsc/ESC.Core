using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Service
{
    /// <summary>
    /// 资源服务
    /// </summary>
   public class SPermissionService
    {
       protected SPermissionRepository pRepository;

       public SPermissionService() {
           pRepository = new SPermissionRepository();
       }

       /// <summary>
       /// 根据用户获取菜单权限
       /// </summary>
       /// <param name="userId"></param>
       /// <param name="isAdmin"></param>
       /// <returns></returns>
       public List<SResource> GetRPermissionByUser(int userId, bool isAdmin)
       {
           return pRepository.GetRPermissionByUser(userId, isAdmin);
       }

       /// <summary>
       /// 根据角色ID获取菜单权限
       /// </summary>
       /// <param name="ID"></param>
       /// <returns></returns>
       public List<SResource> GetRPermissionByRole(int roleId)
       {
           return pRepository.GetRPermissionByRole(roleId);
       }

       /// <summary>
       /// 根据用户获取按钮权限
       /// </summary>
       /// <param name="user">用户</param>
       /// <param name="controllerName">控制器名称</param>
       /// <returns></returns>
       public List<SOperator> GetOPermissionByUserAndResource(SUser user, string resourceName)
       {
           return pRepository.GetOPermissionByUserAndResource(user,resourceName);
       }

       /// <summary>
       /// 根据角色ID获取按钮权限
       /// </summary>
       /// <param name="user">用户</param>
       /// <param name="controllerName">控制器名称</param>
       /// <returns></returns>
       public List<SOperator> GetOPermissionByRoleAndResource(int roleId, string resourceName)
       {
           return pRepository.GetOPermissionByRoleAndResource(roleId, resourceName);
       }

       /// <summary>
       /// 根据用户获取列权限
       /// </summary>
       /// <param name="user"></param>
       /// <param name="tableName"></param>
       /// <returns></returns>
       public List<SCPermission> GetCPermissionByUserAndResource(SUser user, string tableName,string resourceName)
       {
           return pRepository.GetCPermissionByUserAndResource(user, tableName, resourceName);
       }

       /// <summary>
       /// 根据角色获取列权限
       /// </summary>
       /// <param name="user"></param>
       /// <param name="tableName"></param>
       /// <returns></returns>
       public List<SCPermission> GetCPermissionByRoleAndResource(int roleId, string tableName,string rescourceName)
       {
           return pRepository.GetCPermissionByRoleAndResource(roleId, tableName, rescourceName);
       }

       ///// <summary>
       ///// 根据资源查表名
       ///// </summary>
       ///// <param name="resourceName"></param>
       ///// <returns></returns>
       //public List<SResTableRel> GetResTableResByResource(string resourceName)
       //{
       //    return rtrRespository.GetResTableResByResource(resourceName); ;
       //}
    }
}

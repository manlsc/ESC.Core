using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure;
using ESC.Infrastructure.Repository;
using ESC.Core;
using Dapper.Extensions;

namespace ESC.Service
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public class SRoleService
    {
        protected SRoleRepository rRepository;
        protected SResourceRepository resRepository;
        protected SPermissionRepository pRepository;
        protected SUserRepository uRepository;
        protected SRolePermissionRepository rpRepository;
        protected SOPermissionRepository opRepository;
        protected SCPermissionRepository cpRepository;

        public SRoleService()
        {
            rRepository = new SRoleRepository();
            resRepository = new SResourceRepository(rRepository.DbCondext);
            pRepository = new SPermissionRepository(rRepository.DbCondext);
            uRepository = new SUserRepository(rRepository.DbCondext);
            rpRepository = new SRolePermissionRepository(rRepository.DbCondext);
            opRepository = new SOPermissionRepository(rRepository.DbCondext);
            cpRepository = new SCPermissionRepository(rRepository.DbCondext);
        }

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public List<SRole> GetAllRoles()
        {
            string sql = rRepository.GetSearchSql();
            return rRepository.Query(sql);

        }

        /// <summary>
        /// 根据ID查询角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public SRole GetRoleById(int roleId)
        {
            return rRepository.SingleOrDefault(roleId);
        }

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            return rRepository.GetSearchSql(whereItems);
        }

        /// <summary>
        /// 根据角色ID列表删除角色
        /// </summary>
        /// <param name="roleIDs"></param>
        public void RemoveRole(SRole role)
        {
            rRepository.Delete(role);
            RemovePermissionByRole(role.ID);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        public void AddRole(SRole role)
        {
            rRepository.Insert(role);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"></param>
        public void UpdateRole(SRole role)
        {
            rRepository.Update(role);
        }

        /// <summary>
        /// 获取库存组织树
        /// </summary>
        /// <returns></returns>
        public List<TreeNode> GetRoleTree(List<WhereItem> whereItems)
        {
            List<TreeNode> tns = new List<TreeNode>();
            string sql = rRepository.GetSearchSql(whereItems);
            List<SRole> roles = rRepository.Query(sql);
            foreach (SRole role in roles)
            {
                TreeNode tn = new TreeNode();
                tn.id = role.ID.ToString();
                tn.text = role.RoleName;
                tn.name = role.ID.ToString();
                tns.Add(tn);
            }
            return tns;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="role"></param>
        public string CheckRole(SRole role)
        {
            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                return "角色名称不能为空.";
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断角色下面是否存在用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool CheckHasUsers(int roleId)
        {
            return uRepository.GetUsersByRoleId(roleId).Count > 0;
        }

        /// <summary>
        /// 删除用户和角色关系
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int RemoveUserRoleRelation(int userID, int roleId)
        {
            return rRepository.RemoveUserRole(userID, roleId);
        }

        /// <summary>
        /// 根据角色ID获取角色用户关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<SUserRole> GetUserRoleByRole(int roleId)
        {
            return rRepository.GetUserRoleByRole(roleId);
        }

        /// <summary>
        /// 添加用户角色关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int AddUserRole(int roleId, int userId)
        {
            return rRepository.AddUserRole(roleId, userId);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="role"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(SRole role, bool isAdd)
        {
            return rRepository.IsNotExits(role, isAdd);
        }

        /// <summary>
        /// 根据角色获取
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<string> GetOPermissionByRole(int roleId)
        {
            return pRepository.GetOPermissionByRole(roleId);
        }

        /// <summary>
        /// 根据角色获取列权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<SCPermission> GetCPermissionByRole(int roleId, string tableName, string resourceName)
        {
            return pRepository.GetCPermissionByRoleAndResource(roleId, tableName, resourceName);
        }

        /// <summary>
        /// 删除权限树
        /// </summary>
        /// <param name="ID"></param>
        public void RemovePermissionByRole(int roleId)
        {
            //删除权限
            pRepository.RemovePermissionByRole(roleId);
            //删除权限角色关系
            pRepository.RemoveResPermissionByRole(roleId);
        }

        /// <summary>
        /// 保存资源权限
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public int SaveOpermissions(List<SOPermission> pers, int roleId)
        {
            RemovePermissionByRole(roleId);
            foreach (SOPermission p in pers)
            {
                if (p.OperatorID > 0)
                {
                    opRepository.Insert(p);
                }
                else
                {
                    SRolePermission rp = new SRolePermission();
                    rp.RoleID = roleId;
                    rp.ResourceID = p.ResourceID;
                    rpRepository.Insert(rp);
                }
            }
            return pers.Count;
        }

        /// <summary>
        /// 保存列权限
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public int SaveCpermissions(List<SCPermission> pers)
        {
            //删除原来的权限
            if (pers.Count > 0)
            {
                pRepository.RemoveCPermissionByTableAndRole(pers[0].TableName, pers[0].RoleID);
            }

            //新增权限
            foreach (SCPermission item in pers)
            {
                cpRepository.Insert(item);
            }
            return pers.Count;
        }

        /// <summary>
        /// 初始化角色菜单
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public int InitMenu(int roleId)
        {
            return pRepository.InitMenu(roleId);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<SRole> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = rRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<SRole> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return rRepository.Pages(pageIndex, pageSize, sql, args);
        }
    }
}

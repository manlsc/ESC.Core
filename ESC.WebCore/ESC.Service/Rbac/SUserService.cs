using ESC.Core;
using ESC.Core.Helper;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESC.Service
{
    /// <summary>
    /// 用户服务 +
    /// </summary>
    public class SUserService
    {
        protected SUserRepository uRepository;

        public SUserService()
        {
            uRepository = new SUserRepository();
        }

        /// <summary>
        /// 获取角色下面的所有用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<SUser> GetUsersByRoleId(int roleId)
        {
            string sql = uRepository.GetUsersByRoleIdSql(roleId);
            return uRepository.Query(sql);

        }

        /// <summary>
        /// 根据组织机构Id获取用户列表
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public List<SUser> GetUsersByOrgId(int orgID)
        {
            string sql = uRepository.GetUsersByOrgIdSql(orgID);
            return uRepository.Query(sql);
        }

        /// <summary>
        /// 检查非空验证
        /// </summary>
        /// <param name="user"></param>
        public string CheckUser(SUser user)
        {
            string msg = string.Empty;
            if (user.UserCode.Equals("admin"))
            {
                msg = "管理员用户不能删除.";
                return msg;
            }
            if (string.IsNullOrWhiteSpace(user.UserCode))
            {
                msg = "用户编码不能为空.";
                return msg;
            }
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                msg = "用户名不能为空.";
                return msg;
            }
            if (!string.IsNullOrWhiteSpace(user.EMail))
            {
                if (!ESCRegex.IsEmail(user.EMail))
                {
                    msg = "邮箱格式错误.";
                    return msg;
                }
            }
            if (!string.IsNullOrWhiteSpace(user.Mobile))
            {
                if (!ESCRegex.IsTel(user.Mobile))
                {
                    msg = "电话格式错误.";
                    return msg;
                }
            }
            if (!string.IsNullOrWhiteSpace(user.Phone))
            {
                if (!ESCRegex.IsTel(user.Phone))
                {
                    msg = "手机号码格式错误.";
                    return msg;
                }
            }
            return msg;
        }

        /// <summary>
        /// 判断是否系统管理员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsSuperUser(SUser user)
        {
            if (user.UserCode.Equals("admin") && user.SuperUser == BoolEnum.Yes)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Id"></param>
        public void RemoveUserById(int Id)
        {
            uRepository.Delete(Id);
        }

        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isAdd"></param>
        public bool IsNotExits(SUser user, bool isAdd)
        {
            return uRepository.IsNotExits(user, isAdd);
        }

        /// <summary>
        /// 根据用户编码获取用户
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public List<SUser> GetUserByCode(string userCode)
        {
            return uRepository.GetUserByCode(userCode);
        }

        /// <summary>
        /// 根据仓库查询库管
        /// </summary>
        /// <param name="locationId"></param>
        public List<SUser> GetUsersByLoctionId(int locationId)
        {
            string sql = uRepository.GetUsersByLoctionId(locationId);
            return uRepository.Query(sql);
        }

        /// <summary>
        /// 插入新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int AddUser(SUser user)
        {
            object result = uRepository.Insert(user);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public int RemoveUsers(List<SUser> users)
        {
            DatabaseContext db = uRepository.DbCondext;
            try
            {
                db.BeginTransaction();
                foreach (SUser u in users)
                {
                    //系统管理员,不能删除
                    if (!IsSuperUser(u))
                    {
                        RemoveUser(u);
                    }
                }
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
            return users.Count;

        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool RemoveUser(SUser user)
        {
            //删除用户角色关系
            uRepository.RemoveUserRoleByUser(user.ID);
            //删除用户
            return uRepository.Delete(user);
        }

        /// <summary>
        /// 根据Id删除用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool RemoveUser(int Id)
        {
            return uRepository.Delete(Id);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUser(SUser user)
        {
            return uRepository.Update(user);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ResetPassword(string pwd, int userId)
        {
            return uRepository.ResetPassword(pwd, userId);
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public int RemoveOrg(int orgId)
        {
            return uRepository.RemoveOrg(orgId);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<SUser> GetUserCodeName(string codeName)
        {
            return uRepository.GetUserCodeName(codeName);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<SUser> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = uRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<SUser> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return uRepository.Pages(pageIndex, pageSize, sql, args);
        }
    }
}

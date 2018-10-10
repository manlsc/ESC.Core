using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESC.Infrastructure;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;

namespace ESC.Service
{
    /// <summary>
    /// 组织结构
    /// </summary>
    public class SOrganizationService
    {
        protected SOrganizationRepository oRepository;
        protected SUserRepository uRepository;

        public SOrganizationService()
        {
            oRepository = new SOrganizationRepository();
            uRepository = new SUserRepository(oRepository.DbCondext);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="users"></param>
        /// <param name="orgID"></param>
        public int AddOrganization(SOrganization org)
        {
            //插入组织
            object orgID = oRepository.Insert(org);
            return Convert.ToInt32(orgID);
        }

        /// <summary>
        /// 通过ID删除库存组织
        /// </summary>
        /// <param name="orgId"></param>
        public bool RemoveOrganization(int orgId)
        {
            SOrganization org = oRepository.SingleOrDefault(orgId);
            if (org == null)
            {
                return false;
            }
            uRepository.RemoveOrg(orgId);
            return oRepository.Delete(org);
        }

        /// <summary>
        /// 判断是否存在子组织结构
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public bool HasChildrenOrg(int orgId)
        {
            List<SOrganization> children = oRepository.GetChildOrganization(orgId);
            return children.Count > 0;
        }


        /// <summary>
        /// 获取库存组织树
        /// </summary>
        /// <returns></returns>
        public List<SOrganization> GetOrganizationTree()
        {
            List<SOrganization> orgs = oRepository.QueryAll();
            List<SOrganization> parents = orgs.Select(t => t).Where(o => o.ParentID == 0).ToList();
            foreach (SOrganization parent in parents)
            {
                GetOrganizationChild(parent, orgs);
            }
            return parents;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="orgs"></param>
        /// <returns></returns>
        private void GetOrganizationChild(SOrganization parent, List<SOrganization> orgs)
        {
            List<SOrganization> children = orgs.Select(t => t).Where(o => o.ParentID == parent.ID).ToList();
            if (children.Count > 0)
            {
                parent.Children.AddRange(children);
                foreach (SOrganization child in children)
                {
                    GetOrganizationChild(child, orgs);
                }
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="org"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(SOrganization org, bool isAdd)
        {
            if (org == null) return true;
            if (string.IsNullOrWhiteSpace(org.OrgCode)) return true;
            return oRepository.IsNotExits(org, isAdd);
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public string CheckOrganization(SOrganization org)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(org.OrgCode))
            {
                return "组织编码不能为空.";
            }
            if (string.IsNullOrWhiteSpace(org.OrgName))
            {
                return "组织名称不能为空.";
            }

            if (org.ID > 0)
            {
                if (org.ID == org.ParentID)
                {
                    return "上级组织不能选择自身.";
                }
            }

            return msg;
        }

        /// <summary>
        /// 更新组织
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public bool UpdateOrganization(SOrganization org)
        {
            return oRepository.Update(org);
        }

        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public SOrganization GetOrganizationById(int Id)
        {
            return oRepository.GetOrganizationById(Id);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<SOrganization> GetOrganizationCodeName(string codeName)
        {
            return oRepository.GetOrganizationCodeName(codeName);
        }
    }
}

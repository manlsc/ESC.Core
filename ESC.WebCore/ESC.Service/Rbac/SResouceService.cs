using Dapper.Extensions;
using ESC.Core;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Service
{
    /// <summary>
    /// 资源服务
    /// </summary>
    public class SResourceService
    {
        protected SResourceRepository rRepository;
        protected SOperatorRepository oRepository;

        public SResourceService()
        {
            rRepository = new SResourceRepository();
            oRepository = new SOperatorRepository(rRepository.DbCondext);
        }

        /// <summary>
        /// 根据ID集合删除资源
        /// </summary>
        /// <param name="resourceIDs"></param>
        /// <returns></returns>
        public bool RemoveResource(int resId)
        {
            bool result = false;
            DatabaseContext dbContext = rRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                rRepository.RemoveOperatorByResourceID(resId);
                result = rRepository.Delete(resId);

                dbContext.CompleteTransaction();
            }
            catch (Exception ex)
            {
                dbContext.AbortTransaction();
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public object AddResource(SResource resource)
        {
            DatabaseContext dbContext = rRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                //添加资源  添加按钮
                object id = rRepository.Insert(resource);
                foreach (SOperator o in resource.Operators)
                {
                    o.ResourceID = Convert.ToInt32(id);
                    o.OperatorDesc = o.OperatorName;
                    oRepository.Insert(o);
                }

                dbContext.CompleteTransaction();
                return id;
            }
            catch (Exception ex)
            {
                dbContext.AbortTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="resource"></param>
        public void UpdateResource(SResource resource)
        {
            DatabaseContext dbContext = rRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                rRepository.Update(resource);
                foreach (SOperator o in resource.Operators)
                {
                    switch (o.CURD)
                    {
                        case "add":
                            o.ResourceID = resource.ID;
                            o.OperatorDesc = o.OperatorName;
                           oRepository.Insert(o);
                            break;
                        case "delete":
                            oRepository.Delete(o);
                            break;
                        default:
                            o.OperatorDesc = o.OperatorName;
                            oRepository.Update(o);
                            break;
                    }
                }

                dbContext.CompleteTransaction();
            }
            catch (Exception ex)
            {
                dbContext.AbortTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 根据资源获取操作
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public List<SOperator> GetOperatorByResource(int resourceId)
        {
            return rRepository.GetOperatorByResource(resourceId);
        }


        /// <summary>
        /// 查询所有子资源（包括本身）
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="resources"></param>
        public void GetResourceWithChildren(SResource parent, List<SResource> resources)
        {
            if (parent == null)
                return;
            resources.Add(parent);

            List<SResource> children = GetChildResource(parent.ID);
            foreach (SResource child in children)
            {
                GetResourceWithChildren(child, resources);
            }
        }

        /// <summary>
        /// 获取子资源
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<SResource> GetChildResource(int parentId)
        {
            return rRepository.GetChildResource(parentId);
        }

        /// <summary>
        /// 获取资源树
        /// </summary>
        /// <returns></returns>
        public List<SResource> GetResouceTree()
        {
            string sql = rRepository.GetSearchSql(new List<WhereItem>());
            List<SResource> resources = rRepository.Query(sql);
            List<SResource> parents = resources.Select(t => t).Where(o => o.ParentID == 0).ToList();
            foreach (SResource parent in parents)
            {
                GetChildTree(parent, resources);
            }
            return parents;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="orgs"></param>
        /// <returns></returns>
        private void GetChildTree(SResource parent, List<SResource> resources)
        {
            List<SResource> children = resources.Select(t => t).Where(o => o.ParentID == parent.ID).ToList();
            foreach (SResource child in children)
            {
                parent.Children.Add(child);
                GetChildTree(child, resources);
            }
        }

        /// <summary>
        /// 删除资源的操作
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public int RemoveOperator(int operatorId)
        {
            return rRepository.RemoveOperatorByID(operatorId);
        }

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public SResource GetResourcByID(int resourceId)
        {
            return rRepository.GetResourcByID(resourceId);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public bool IsNotExits(SResource resource, bool isAdd)
        {
            return rRepository.IsNotExits(resource, isAdd);
        }

        /// <summary>
        /// 判断父类组织是否等于当前组织
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public bool IsParentEqSelf(SResource resource)
        {
            if (resource.ParentID == 0)
                return false;
            return resource.ParentID == resource.ID;
        }

        /// <summary>
        /// 获取菜单角色树
        /// </summary>
        /// <returns></returns>
        public List<SResource> GetResourceBtnTree()
        {
            List<SResource> orgs = rRepository.QueryAll();
            List<SResource> parents = orgs.Select(t => t).Where(o => o.ParentID == 0).ToList();
            foreach (SResource parent in parents)
            {
                GetResouceTree(parent, orgs);
            }
            return parents;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="resources"></param>
        /// <returns></returns>
        private void GetResouceTree(SResource parent, List<SResource> resources)
        {
            List<SResource> children = resources.Select(t => t).Where(o => o.ParentID == parent.ID).ToList();
            if (children.Count > 0)
            {
                foreach (SResource child in children)
                {
                    parent.Children.Add(child);
                    GetResouceTree(child, resources);
                }
            }
            else if (!string.IsNullOrWhiteSpace(parent.ResourceURL))
            {
                List<SOperator> ops = rRepository.GetOperatorByResource(parent.ID);
                foreach (SOperator child in ops)
                {
                    parent.Operators.Add(child);
                }
            }
        }
    }
}

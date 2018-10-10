using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ESC.Web.Controllers
{
    /// <summary>
    /// 权限配置 +
    /// </summary>
    public class SRolePermissionController : BaseController
    {
        SRoleService rService = new SRoleService();
        SUserService uService = new SUserService();
        SResourceService resService = new SResourceService();

        // GET: SPermission
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult GetRoles()
        {
            List<SRole> roles = rService.GetAllRoles();
            return ReturnResult(roles);
        }

        /// <summary>
        /// 查询角色下面的用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult ForeignSearch()
        {
            int roleId = GetParam<int>("foreign");
            List<SUser> users = uService.GetUsersByRoleId(roleId);
            return ReturnResult(users);
        }

        /// <summary>
        /// 添加按钮权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Add()
        {
            List<SOPermission> pers = GetAdd<List<SOPermission>>();
            if (pers.Count > 0)
            {
                rService.SaveOpermissions(pers, pers[0].RoleID);
            }
            ResultData<string> rt = new ResultData<string>();
            return ReturnResult(rt);
        }

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult GetPermissionTree()
        {
            string strRoleId = GetParam("roleId");
            List<string> perms = rService.GetOPermissionByRole(Convert.ToInt32(strRoleId));
            return ReturnResult(perms);
        }

        #region 获取资源树

        /// <summary>
        /// 获取菜单和按钮
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult GetResourceAndOper()
        {
            List<SResource> resourceTree = resService.GetResouceTree();

            return ReturnResult(BulidTree(resourceTree));
        }

        /// <summary>
        /// 构建树
        /// </summary>
        /// <param name="orgTree"></param>
        /// <returns></returns>
        public List<TreeNode> BulidTree(List<SResource> resourceTree)
        {
            List<TreeNode> tns = new List<TreeNode>();
            foreach (SResource res in resourceTree)
            {
                TreeNode tn = new TreeNode();
                tn.id = res.ID.ToString();
                tn.text = res.ResourceDesc;
                tn.name = "res";
                if (res.Children.Count > 0)
                {
                    tn.children = new List<TreeNode>(res.Children.Count);
                    BuildChildTree(res, tn);
                }
                tns.Add(tn);
            }
            return tns;
        }

        /// <summary>
        /// 构建子节点
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="parentTn"></param>
        public void BuildChildTree(SResource resource, TreeNode parentTn)
        {
            foreach (SResource res in resource.Children)
            {
                TreeNode tn = new TreeNode();
                tn.id = res.ID.ToString();
                tn.text = res.ResourceDesc;
                tn.name = "res";
                if (res.Children.Count > 0)
                {
                    BuildChildTree(res, tn);
                }
                GetOperatiors(tn, res);
                parentTn.children.Add(tn);
            }
        }

        /// <summary>
        /// 根据菜单获取按钮列表
        /// </summary>
        /// <param name="parentTn"></param>
        /// <param name="resource"></param>
        public void GetOperatiors(TreeNode parentTn, SResource resource)
        {
            List<SOperator> opts = resService.GetOperatorByResource(resource.ID);
            foreach (SOperator opt in opts)
            {
                TreeNode tn = new TreeNode();
                tn.id = "btn" + opt.ID.ToString();  //防止按钮id和菜单id冲突,所以添加btn前缀
                tn.text = opt.OperatorDesc;
                tn.name = "btn";
                parentTn.children.Add(tn);
            }
        }

        #endregion

        #region 分配\删除角色

        [HttpPost]
        public ContentResult AddUser()
        {
            ResultData<string> rt = new ResultData<string>();

            List<int> userIds = GetAdd<List<int>>();
            string strRoleId = GetParam("roleId");
            int roleId = int.Parse(strRoleId);

            //查询角色角色关系
            List<SUserRole> userRoles = rService.GetUserRoleByRole(roleId);
            foreach (int userId in userIds)
            {
                if (!userRoles.Exists(ur => ur.UserID == userId))
                {
                    rService.AddUserRole(roleId, userId);
                }
            }

            return ReturnResult(rt);
        }


        [HttpPost]
        public ContentResult RmoveUser()
        {
            ResultData<string> rt = new ResultData<string>();

            List<int> userIds = GetDelete<List<int>>();
            string strRoleId = GetParam("roleId");
            int roleId = int.Parse(strRoleId);

            //查询角色角色关系
            foreach (int userId in userIds)
            {
                rService.RemoveUserRoleRelation(userId, roleId);
            }

            return ReturnResult(rt);
        }
        #endregion
    }
}
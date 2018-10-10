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
    /// 组织机构 +
    /// </summary>
    public class SOrganizationController : BaseController
    {
        SOrganizationService orgService = new SOrganizationService();
        SUserService uService = new SUserService();

        // GET: SOrganization
        public ActionResult Index()
        {
            return View();
        }

        #region 弹层选择

        [HttpGet]
        public ActionResult SearchOrgView()
        {
            return View();
        }

        #endregion

        #region 初始化

        [HttpGet]
        public ContentResult Init()
        {
            InitData idata = BaseInit(typeof(SOrganization), true);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {
                    case "ParentID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "id",
                            foreignValue = "text",
                            table = "Organization"
                        };
                        break;
                }
            }          
            return ReturnResult(idata);
        }

        [HttpGet]
        public ContentResult ForeignInit()
        {
            InitData idata = BaseInit(typeof(SUser), false);
            return ReturnResult(idata);
        }
        #endregion

        #region 获取组织机构树

        [HttpGet]
        public ContentResult GetTree()
        {
            List<SOrganization> orgTree = orgService.GetOrganizationTree();

            return ReturnResult(BulidTree(orgTree));
        }

        /// <summary>
        /// 构建树
        /// </summary>
        /// <param name="orgTree"></param>
        /// <returns></returns>
        public List<TreeNode> BulidTree(List<SOrganization> orgTree)
        {
            List<TreeNode> tns = new List<TreeNode>();
            foreach (SOrganization org in orgTree)
            {
                TreeNode tn = new TreeNode();
                tn.id = org.ID.ToString();
                tn.text = org.OrgName;
                tn.name = org.OrgCode;
                if (org.Children.Count > 0)
                {
                    tn.children = new List<TreeNode>(org.Children.Count);
                    BuildChildTree(org, tn);
                }
                tns.Add(tn);
            }
            return tns;
        }

        /// <summary>
        /// 构建子节点
        /// </summary>
        /// <param name="org"></param>
        /// <param name="parentTn"></param>
        public void BuildChildTree(SOrganization org, TreeNode parentTn)
        {
            foreach (SOrganization chd in org.Children)
            {
                TreeNode tn = new TreeNode();
                tn.id = chd.ID.ToString();
                tn.text = chd.OrgName;
                tn.name = chd.OrgCode;
                if (chd.Children.Count > 0)
                {
                    BuildChildTree(chd, tn);
                }
                parentTn.children.Add(tn);
            }
        }

        #endregion

        #region 增删改

        [HttpPost]
        public ContentResult Delete()
        {
            ResultData<string> rt = new ResultData<string>();

            //获取前台传会的删除ID
            int orgId = GetDelete<int>();

            if (orgService.HasChildrenOrg(orgId))
            {
                rt.status = -1;
                rt.message = "当前组织存在子组织不能删除.";
            }
            else
            {
                orgService.RemoveOrganization(orgId);
                rt.message = "删除成功.";
            }

            return ReturnResult(rt);
        }

        [HttpPost]
        public ContentResult Add()
        {
            ResultData<string> rt = new ResultData<string>();

            SOrganization org = GetAdd<SOrganization>();

            //非空验证和属性格式验证
            string msg = orgService.CheckOrganization(org);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //添加级别
            if (org.ParentID > 0)
            {
                SOrganization pOrg = orgService.GetOrganizationById(org.ParentID);
                org.OrgLevel = pOrg.OrgLevel + 1;
            }
            else
            {
                org.OrgLevel = 1;
            }

            //验证用户是否存在
            if (orgService.IsNotExits(org, true))
            {
                orgService.AddOrganization(org);
            }
            else
            {
                rt.status = -1;
                rt.message = "组织编码已经存在.";
            }
            return ReturnResult(rt);
        }

        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            SOrganization org = GetUpdate<SOrganization>();

            //非空验证和属性格式验证
            string msg = orgService.CheckOrganization(org);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //添加级别
            if (org.ParentID > 0)
            {
                SOrganization pOrg = orgService.GetOrganizationById(org.ParentID);
                org.OrgLevel = pOrg.OrgLevel + 1;
            }
            else
            {
                org.OrgLevel = 1;
            }

            //验证用户是否存在
            if (orgService.IsNotExits(org, false))
            {
                orgService.UpdateOrganization(org);
            }
            else
            {
                rt.status = -1;
                rt.message = "组织编码已经存在.";
            }
            return ReturnResult(rt);
        }

        #endregion

        #region 查询

        [HttpGet]
        public ContentResult SearchDetail()
        {
            string orgId = GetParam("parentId");
            List<SUser> users = uService.GetUsersByOrgId(Convert.ToInt32(orgId));
            return ReturnResult(users);
        }

        [HttpGet]
        public ContentResult SearchSingle()
        {
            string orgId = GetParam("Id");
            SOrganization org = orgService.GetOrganizationById(Convert.ToInt32(orgId));
            return ReturnResult(org);
        }

        /// <summary>
        /// 根据编码模糊查询
        /// </summary>
        /// <returns></returns>
        public ContentResult GetOrganizationCodeName()
        {
            string codeName = GetParam("words");
            ResultData<List<SOrganization>> rt = new ResultData<List<SOrganization>>();
            rt.result = orgService.GetOrganizationCodeName(codeName);
            return ReturnResult(rt);
        }

        #endregion
    }
}
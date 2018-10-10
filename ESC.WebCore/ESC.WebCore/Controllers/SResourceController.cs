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
    /// 菜单管理 +
    /// </summary>
    public class SResourceController : BaseController
    {
        SResourceService rService = new SResourceService();

        // GET: SResouce
        public ActionResult Index()
        {
            return View();
        }

        #region 初始化

        [HttpGet]
        public ContentResult Init()
        {
            InitData idata = BaseInit(typeof(SResource), true);           
            return ReturnResult(idata);
        }

        [HttpGet]
        public ContentResult ForeignInit()
        {
            InitData idata = BaseInit(typeof(SOperator), false);
            return ReturnResult(idata);
        }
        #endregion

        #region 获取资源树

        [HttpGet]
        public ContentResult GetTree()
        {
            List<SResource> resourceTree = rService.GetResouceTree();

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
                tn.name = res.ResourceName;
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
                tn.name = res.ResourceName;
                if (res.Children.Count > 0)
                {
                    BuildChildTree(res, tn);
                }
                parentTn.children.Add(tn);
            }
        }

        #endregion

        #region 增删改查

        [HttpPost]
        public ContentResult Delete()
        {
            ResultData<string> rt = new ResultData<string>();

            int resId = GetDelete<int>();
            rService.RemoveResource(resId);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Add()
        {
            ResultData<string> rt = new ResultData<string>();
            SResource resource = GetAdd<SResource>();
            //验证
            if (rService.IsNotExits(resource, true))
            {
                if (!rService.IsParentEqSelf(resource))
                {
                    rService.AddResource(resource);
                }
                else
                {
                    rt.status = -1;
                    rt.message = "父类资源不能是资源本身.";
                }
            }
            else
            {
                rt.status = -1;
                rt.message = "资源名称已经存在.";
            }

            return ReturnResult(rt);
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddView()
        {
            List<SResource> res = rService.GetChildResource(0);
            return View(res);
        }

        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();
            SResource resource = GetUpdate<SResource>();
            //验证
            if (rService.IsNotExits(resource, false))
            {
                if (!rService.IsParentEqSelf(resource))
                {
                    rService.UpdateResource(resource);
                }
                else
                {
                    rt.status = -1;
                    rt.message = "父类资源不能是资源本身.";
                }
            }
            else
            {
                rt.status = -1;
                rt.message = "资源名称已经存在.";
            }

            return ReturnResult(rt);
        }

        public ActionResult UpdateView()
        {
            int resId = GetParam<int>("id");
            ViewBag.Resource = rService.GetResourcByID(resId);

            List<SResource> res = rService.GetChildResource(0);
            return View(res);
        }

        #endregion

        #region 查询

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult SearchDetail()
        {
            string resId = GetParam("parentId");
            List<SOperator> oprs = rService.GetOperatorByResource(Convert.ToInt32(resId));
            return ReturnResult(oprs);
        }

        [HttpGet]
        public ContentResult SearchSingle(int Id)
        {
            SResource res = rService.GetResourcByID(Id);
            return ReturnResult(res);
        }
        #endregion
    }
}
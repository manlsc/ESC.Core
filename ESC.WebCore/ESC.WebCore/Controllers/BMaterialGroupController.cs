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
    /// 物料组管理 +
    /// </summary>
    public class BMaterialGroupController : BaseController
    {
        BLocationService lService = new BLocationService();
        BMaterialGroupService mgService = new BMaterialGroupService();
        // GET: BMaterialGroup
        public ActionResult Index()
        {
            return View();
        }

        #region 弹层选择

        public ActionResult SearchMaterialGroupView()
        {
            return View();
        }

        #endregion

        #region 初始化

        [HttpGet]
        public ContentResult Init()
        {
            InitData idata = BaseInit(typeof(BMaterialGroup), true);
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
                            table = "BMaterialGroup"
                        };
                        break;
                    case "OrgID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "id",
                            foreignValue = "text",
                            table = "Organization"
                        };
                        break;
                    case "CreateBy":
                    case "UpdateBy":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "UserName",
                            table = "SUser"
                        };
                        break;                 
                }
            }           
            return ReturnResult(idata);
        }

        [HttpGet]
        public ContentResult ForeignInit()
        {
            InitData idata = BaseInit(typeof(BLocation), false);
            return ReturnResult(idata);
        }

        #endregion

        #region 获取资源树

        [HttpGet]
        public ContentResult GetTree()
        {
            List<BMaterialGroup> matgroupTree = mgService.GetMaterialGroupTree();

            return ReturnResult(BulidTree(matgroupTree));
        }

        /// <summary>
        /// 构建树
        /// </summary>
        /// <param name="orgTree"></param>
        /// <returns></returns>
        public List<TreeNode> BulidTree(List<BMaterialGroup> matgroupTree)
        {
            List<TreeNode> tns = new List<TreeNode>();
            foreach (BMaterialGroup mg in matgroupTree)
            {
                TreeNode tn = new TreeNode();
                tn.id = mg.ID.ToString();
                tn.text = mg.GroupName;
                tn.name = mg.GroupName;
                if (mg.Children.Count > 0)
                {
                    tn.children = new List<TreeNode>(mg.Children.Count);
                    BuildChildTree(mg, tn);
                }
                tns.Add(tn);
            }
            return tns;
        }

        /// <summary>
        /// 构建子节点
        /// </summary>
        /// <param name="mgation"></param>
        /// <param name="parentTn"></param>
        public void BuildChildTree(BMaterialGroup mgation, TreeNode parentTn)
        {
            foreach (BMaterialGroup mg in mgation.Children)
            {
                TreeNode tn = new TreeNode();
                tn.id = mg.ID.ToString();
                tn.text = mg.GroupName;
                tn.name = mg.GroupName;
                if (mg.Children.Count > 0)
                {
                    BuildChildTree(mg, tn);
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

            //获取前台传会的删除ID
            int mgId = GetDelete<int>();

            //判断物料组是否存在物料

            //删除
            mgService.RemoveMaterialGroup(mgId);
            rt.message = "删除成功.";

            return ReturnResult(rt);
        }

        [HttpPost]
        public ContentResult Add()
        {
            ResultData<string> rt = new ResultData<string>();

            BMaterialGroup location = GetAdd<BMaterialGroup>();

            //非空验证和属性格式验证
            string msg = mgService.CheckMaterialGroup(location);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //判断是否子存储单元,添加顶级存储单元
            if (location.ParentID > 0)
            {
                BMaterialGroup parent = mgService.GetMaterialGroupById(location.ParentID);
                location.TopGroupID = parent.TopGroupID;
                location.GroupLevel = parent.GroupLevel + 1;              
            }
            else
            {
                location.GroupLevel = 1;
            }

            //是否存在
            if (mgService.IsNotExits(location, false))
            {
                location.CreateBy = this.CurrentUser.ID;
                location.CreateDate = DateTime.Now;
                location.UpdateDate = DateTime.Now;
                int mgId = mgService.AddMaterialGroup(location);

                //如果添加为仓库,则跟新顶级存储单元ID
                if (location.ParentID < 1)
                {
                    location.ID = mgId;
                    location.TopGroupID = mgId;
                    mgService.UpdateMaterialGroup(location);
                }
            }
            else
            {
                rt.status = -1;
                rt.message = "存储单元已经存在.";
            }


            return ReturnResult(rt);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult SearchDetail()
        {
            int locationId = GetParentID();
            List<BLocation> loctions = lService.GetLocationsByMaterialGroup(locationId);
            return ReturnResult(loctions);
        }

        /// <summary>
        /// 查询明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult SearchSingle()
        {
            string locationId = GetParam("ID");
            BMaterialGroup group = mgService.GetMaterialGroupById(Convert.ToInt32(locationId));
            return ReturnResult(group);
        }

        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            BMaterialGroup group = GetUpdate<BMaterialGroup>();

            //非空验证和属性格式验证
            string msg = mgService.CheckMaterialGroup(group);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }
            if (mgService.IsParentEqSelf(group))
            {
                rt.message = "父物料组不能选择自身";
                rt.status = -1;
                return ReturnResult(rt);
            }

            group.UpdateDate = DateTime.Now;
            group.UpdateBy = this.CurrentUser.ID;

            //是否存在
            if (mgService.IsNotExits(group, false))
            {
                mgService.UpdateMaterialGroup(group);
            }
            else
            {
                rt.status = -1;
                rt.message = "存储单元已经存在.";
            }
            return ReturnResult(rt);
        }

        /// <summary>
        /// 根据编码模糊查询
        /// </summary>
        /// <returns></returns>
        public ContentResult GetGroupCodeName()
        {
            string codeName = GetParam("words");
            ResultData<List<BMaterialGroup>> rt = new ResultData<List<BMaterialGroup>>();
            rt.result = mgService.GetGroupCodeName(codeName);
            return ReturnResult(rt);
        }

        #endregion

        #region 分配\删除默认仓库

        [HttpPost]
        public ContentResult AddLocation()
        {
            ResultData<string> rt = new ResultData<string>();

            List<int> locationIds = GetAdd<List<int>>();
            int groupId = GetParam<int>("groupId");
            mgService.AddMaterialGroupLocation(locationIds, groupId);

            return ReturnResult(rt);
        }


        [HttpPost]
        public ContentResult RmoveLocation()
        {
            ResultData<string> rt = new ResultData<string>();

            List<int> locationIds = GetDelete<List<int>>();
            int groupId = GetParam<int>("groupId");

            mgService.RemoveMaterialGroupLocation(locationIds, groupId);

            return ReturnResult(rt);
        }
        #endregion
    }
}
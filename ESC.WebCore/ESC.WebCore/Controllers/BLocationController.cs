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
    public class BLocationController : BaseController
    {
        BLocationService lService = new BLocationService();
        SUserService uService = new SUserService();

        // GET: BLocation
        public ActionResult Index()
        {
            return View();
        }

        #region 弹层选择

        /// <summary>
        /// 树形选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchLocationTreeView()
        {
            return View();
        }

        /// <summary>
        /// 表格选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchLocationGridView()
        {
            List<ComboxDataItem> items = GetCombox("LocationType").items;
            return View(items);
        }

        /// <summary>
        /// 根据仓库获取货位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchLocationByWhView()
        {
            ViewBag.TopLocationID = GetParam("TopLocationID");
            return View();
        }

        /// <summary>
        /// 选择仓库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchLocationWhView()
        {
            return View();
        }
       
        /// <summary>
        /// 获取非仓库存储单元
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchLocationNotWhView()
        {
            return View();
        }
        #endregion

        #region 初始化

        [HttpGet]
        public ContentResult Init()
        {
            InitData idata = BaseInit(typeof(BLocation), true);
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
                            table = "BLocation"
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
                    case "LocationType":
                    case "InUse":
                    case "LocationClass":
                    case "IsDefault":
                        item.combox = GetCombox(item.dataIndex).items;
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

        #region 获取资源树

        [HttpGet]
        public ContentResult GetTree()
        {
            List<BLocation> locationTree = lService.GetLocationTree();

            return ReturnResult(BulidTree(locationTree));
        }

        /// <summary>
        /// 构建树
        /// </summary>
        /// <param name="orgTree"></param>
        /// <returns></returns>
        public List<TreeNode> BulidTree(List<BLocation> locationTree)
        {
            List<TreeNode> tns = new List<TreeNode>();
            foreach (BLocation loc in locationTree)
            {
                TreeNode tn = new TreeNode();
                tn.id = loc.ID.ToString();
                tn.text = loc.LocationDesc;
                tn.name = loc.LocationCode;
                if (loc.Children.Count > 0)
                {
                    tn.children = new List<TreeNode>(loc.Children.Count);
                    BuildChildTree(loc, tn);
                }
                tns.Add(tn);
            }
            return tns;
        }

        /// <summary>
        /// 构建子节点
        /// </summary>
        /// <param name="location"></param>
        /// <param name="parentTn"></param>
        public void BuildChildTree(BLocation location, TreeNode parentTn)
        {
            foreach (BLocation loc in location.Children)
            {
                TreeNode tn = new TreeNode();
                tn.id = loc.ID.ToString();
                tn.text = loc.LocationDesc;
                tn.name = loc.LocationCode;
                if (loc.Children.Count > 0)
                {
                    BuildChildTree(loc, tn);
                }
                parentTn.children.Add(tn);
            }
        }

        #endregion

        #region 增删改查

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult Search()
        {
            List<WhereItem> whereItems = GetWhereItems();
            long pageIndex = GetPageIndex();
            long pageSize = GetPageSize();
            var page = lService.PageSearch(pageIndex, pageSize, whereItems);
            return ReturnResult(page);
        }

        /// <summary>
        /// 根据编码模糊查询
        /// </summary>
        /// <returns></returns>
        public ContentResult GetLocationCodeName()
        {
            string locCode = GetParam("words");
            ResultData<List<BLocation>> rt = new ResultData<List<BLocation>>();
            rt.result = lService.GetLocationCodeName(locCode);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 获取特定仓库下面的存储单元
        /// </summary>
        /// <returns></returns>
        public ContentResult GetLocationCodeNameWithWh()
        {
            string locCode = GetParam("words");
            int wharehouseId = GetParam<int>("TopLocationID");
            ResultData<List<BLocation>> rt = new ResultData<List<BLocation>>();
            rt.result = lService.GetLocationCodeNameWithWh(locCode, wharehouseId);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <returns></returns>
        public ContentResult GetLocationCodeNameWh()
        {
            string locCode = GetParam("words");
            ResultData<List<BLocation>> rt = new ResultData<List<BLocation>>();
            rt.result = lService.GetLocationCodeNameWh(locCode);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 获取非仓库存储单元
        /// </summary>
        /// <returns></returns>
        public ContentResult GetLocationCodeNameNotWh()
        {
            string locCode = GetParam("words");
            ResultData<List<BLocation>> rt = new ResultData<List<BLocation>>();
            rt.result = lService.GetLocationCodeNameNotWh(locCode);
            return ReturnResult(rt);
        }

        [HttpPost]
        public ContentResult Delete()
        {
            ResultData<string> rt = new ResultData<string>();

            //获取前台传会的删除ID
            int locId = GetDelete<int>();

            //判断仓库是否存在库存

            //删除
            lService.RmoveLocation(locId);
            rt.message = "删除成功.";

            return ReturnResult(rt);
        }

        [HttpPost]
        public ContentResult Add()
        {
            ResultData<string> rt = new ResultData<string>();

            BLocation location = GetAdd<BLocation>();

            //非空验证和属性格式验证
            string msg = lService.CheckLocation(location);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //判断是否子存储单元,添加顶级存储单元
            if (location.ParentID > 0)
            {
                BLocation parent = lService.GetLocationById(location.ParentID);
                location.TopLocationID = parent.TopLocationID;
                location.LocationLevel = parent.LocationLevel + 1;
                ////如果是货位,默认货区的类型
                //if (location.LocationLevel == 3)
                //{
                //    location.LocationClass = parent.LocationClass;
                //}
            }
            else
            {
                location.LocationLevel = 1;
            }


            //判断是否存在其他货位也是默认货位
            //if (location.LocationLevel == 2)
            //{
            //    if (location.IsDefault > 0)
            //    {
            //        BLocation dftLocation = lService.GetDefaultLocation(location.ParentID, location.LocationClass);
            //        if (dftLocation != null)
            //        {
            //            rt.status = -1;
            //            rt.message = "当前仓库已经存在默认货区(" + dftLocation.LocationCode + ")";
            //            return ReturnResult(rt);
            //        }
            //    }
            //}

            //是否存在
            if (lService.IsNotExits(location, false))
            {
                location.CreateBy = this.CurrentUser.ID;
                location.CreateDate = DateTime.Now;
                location.UpdateDate = DateTime.Now;
                int locId = lService.AddLocation(location);

                //如果添加为仓库,则跟新顶级存储单元ID
                if (location.ParentID < 1)
                {
                    location.ID = locId;
                    location.TopLocationID = locId;
                    lService.UpdateLocation(location);
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
            List<SUser> users = uService.GetUsersByLoctionId(locationId);
            return ReturnResult(users);
        }

        /// <summary>
        /// 查询明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult SearchSingle()
        {
            string locationId = GetParam("ID");
            BLocation loc = lService.GetLocationById(Convert.ToInt32(locationId));
            return ReturnResult(loc);
        }

        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            BLocation locaiton = GetUpdate<BLocation>();

            //非空验证和属性格式验证
            string msg = lService.CheckLocation(locaiton);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            locaiton.UpdateDate = DateTime.Now;
            locaiton.UpdateBy = this.CurrentUser.ID;

            //更新顶级仓库
            BLocation pLocation= lService.GetLocationById(locaiton.ParentID);
            if (pLocation != null)
            {
                locaiton.TopLocationID = pLocation.TopLocationID;
            }

            //是否存在
            if (lService.IsNotExits(locaiton, false))
            {
                lService.UpdateLocation(locaiton);
            }
            else
            {
                rt.status = -1;
                rt.message = "存储单元已经存在.";
            }
            return ReturnResult(rt);
        }

        #endregion

        #region 分配\删除库管

        [HttpPost]
        public ContentResult AddUser()
        {
            ResultData<string> rt = new ResultData<string>();

            List<int> userIds = GetAdd<List<int>>();
            int locationId = GetParam<int>("locationId");
            lService.AddLocationUserRels(userIds, locationId);

            return ReturnResult(rt);
        }


        [HttpPost]
        public ContentResult RmoveUser()
        {
            ResultData<string> rt = new ResultData<string>();

            List<int> userIds = GetDelete<List<int>>();
            int locationId = GetParam<int>("locationId");

            lService.RemoveLocationUserRels(userIds, locationId);

            return ReturnResult(rt);
        }
        #endregion        
        
    }
}
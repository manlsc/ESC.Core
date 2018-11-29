using ESC.Core;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ESC.Web.Controllers
{
    /// <summary>
    /// 物料管理 +
    /// </summary>
    public class BMaterialController : BaseController
    {
        BMaterialService mService = new BMaterialService();
        SColumnService scService = new SColumnService();
        //
        // GET: /BMaterial/

        public ActionResult Index()
        {
            return View();
        }

        #region 弹层选择

        /// <summary>
        /// 查询所有物料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchMaterialView()
        {
            return View();
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Init()
        {
            InitData idata = BaseInit(typeof(BMaterial), true);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {
                    case "UnitID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "UnitName",
                            table = "BUnit"                            
                        };
                        break;
                    case "MaterialGroupID":
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
                            table = "BMaterial"
                        };
                        break;
                }
            }

            return ReturnResult(idata);
        }

        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        private ComboxData GetCombox()
        {
            ComboxData cdata = new ComboxData();
            cdata.items.Add(new ComboxDataItem()
            {
                value = 0,
                text = "否"
            });
            cdata.items.Add(new ComboxDataItem()
            {
                value = 1,
                text = "是"
            });
            return cdata;
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
            var page = mService.PageSearch(pageIndex, pageSize, whereItems);
            return ReturnResult(page);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Delete()
        {
            ResultData<string> rt = new ResultData<string>();

            //获取前台传会的删除ID
            BMaterial user = GetDelete<BMaterial>();
            mService.RemoveMaterial(user);
            rt.message = "删除成功.";

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

            BMaterial u = GetAdd<BMaterial>();

            //非空验证和属性格式验证
            string msg = mService.CheckMaterial(u);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证用户是否存在
            if (mService.IsNotExits(u, true))
            {
                u.CreateBy = this.CurrentUser.ID;
                u.CreateDate = DateTime.Now;
                u.UpdateDate = DateTime.Now;

                mService.AddMaterial(u);
            }
            else
            {
                rt.status = -1;
                rt.message = "物料编码已经存在.";
            }
            return ReturnResult(rt);
        }

        /// <summary>
        ///更新 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            BMaterial u = GetUpdate<BMaterial>();

            //非空验证和属性格式验证
            string msg = mService.CheckMaterial(u);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证用户是否存在
            if (mService.IsNotExits(u, false))
            {
                u.UpdateDate = DateTime.Now;
                u.UpdateBy = this.CurrentUser.ID;
                mService.UpdateMaterial(u);
            }
            else
            {
                rt.status = -1;
                rt.message = "用户编码已经存在.";
            }
            return ReturnResult(rt);
        }

        /// <summary>
        /// 根据编码模糊查询
        /// </summary>
        /// <returns></returns>
        public ContentResult GetMagerialCodeName()
        {
            string codeName = GetParam("words");
            ResultData<List<BMaterial>> rt = new ResultData<List<BMaterial>>();
            rt.result = mService.GetMaterialCodeName(codeName);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult FileUpload()
        {
            //for (int i = 0; i < Request..Count; i++)
            //{
            //    //读取数据
            //    string fileFullName = FileSaveAS(Request.Files[i]);
            //    DataTable dt = new NopiExcelManager().ExcelToDataTable(fileFullName);
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        BMaterial mat = new BMaterial();
            //        mat.CreateBy = CurrentUser.ID;
            //        //mat.MaterialCode=
            //    }
            //}
            return ReturnResult(new ResultData<string>());
        }
        #endregion
    }
}
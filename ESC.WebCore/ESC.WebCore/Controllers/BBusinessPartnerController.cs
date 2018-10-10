using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;
using ESC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ESC.Web.Controllers
{
    /// <summary>
    /// 业务合作伙伴
    /// </summary>
    public class BBusinessPartnerController : BaseController
    {
        BBusinessPartnerService bpService = new BBusinessPartnerService();
        //
        // GET: BBusinessPartner

        public ActionResult Index()
        {
            List<ComboxDataItem> items = GetCombox("BusinessPartnerType").items;
            return View(items);
        }

        #region 弹层选择

        /// <summary>
        /// 查询业务伙伴
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchBusinessPartnerView()
        {
            List<ComboxDataItem> items = GetCombox("BusinessPartnerType").items;
            return View(items);
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
            InitData idata = BaseInit(typeof(BBusinessPartner), true);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {                  
                    case "OrgID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex= item.dataIndex,
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
                    case "BusinessPartnerType":
                        item.combox = GetCombox("BusinessPartnerType").items;
                        break;
                }
            }

            return ReturnResult(idata);
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
            var page = bpService.PageSearch(pageIndex, pageSize, whereItems);
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
            BBusinessPartner bp = GetDelete<BBusinessPartner>();

            bpService.RemoveBusinessPartner(bp);
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

            BBusinessPartner bp = GetAdd<BBusinessPartner>();

            //非空验证和属性格式验证
            string msg = bpService.CheckBusinessPartner(bp);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证用户是否存在
            if (bpService.IsNotExits(bp, true))
            {
                bp.CreateBy = this.CurrentUser.ID;
                bp.CreateDate = DateTime.Now;
                bp.UpdateDate = DateTime.Now;
              
                bpService.AddBusinessPartner(bp);
            }
            else
            {
                rt.status = -1;
                rt.message = "业务伙伴编码已经存在.";
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

            BBusinessPartner bp = GetUpdate<BBusinessPartner>();

            //非空验证和属性格式验证
            string msg = bpService.CheckBusinessPartner(bp);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证用户是否存在
            if (bpService.IsNotExits(bp, false))
            {
                bp.UpdateDate = DateTime.Now;
                bp.UpdateBy = this.CurrentUser.ID;
                bpService.UpdateBusinessPartner(bp);
            }
            else
            {
                rt.status = -1;
                rt.message = "业务伙伴编码已经存在.";
            }
            return ReturnResult(rt);
        }
      
        /// <summary>
        /// 根据名称或编码查询
        /// </summary>
        /// <returns></returns>
        public ContentResult GetCodeName()
        {
            string words = GetParam("words");
            ResultData<List<BBusinessPartner>> rt = new ResultData<List<BBusinessPartner>>();
            rt.result = bpService.GetBusinessPartnerByCodeName(words);
            return ReturnResult(rt);
        }
        #endregion
    }
}
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
    /// 角色管理 +
    /// </summary>
    public class SRoleController : BaseController
    {
        //
        // GET: /Role/

        SRoleService rService = new SRoleService();
        SUserService uService = new SUserService();

        public ActionResult Index()
        {
            return View();
        }

        #region 初始化

        [HttpGet]
        public ActionResult Init()
        {
            InitData idata = BaseInit(typeof(SRole), true);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {
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

        #endregion

        #region 增删改查

        [HttpGet]
        public ContentResult Search()
        {
            List<WhereItem> whereItems = GetWhereItems();
            long pageIndex = GetPageIndex();
            long pageSize = GetPageSize();
            var page = rService.PageSearch(pageIndex, pageSize, whereItems);
            return ReturnResult(page);
        }

        [HttpPost]
        public ContentResult Delete()
        {
            ResultData<string> rt = new ResultData<string>();

            //获取前台传会的删除ID
            SRole role = GetDelete<SRole>();
            if (rService.CheckHasUsers(role.ID))
            {
                rt.message = "当前角色下面存在用户,不能删除.";
                rt.status = -1;
            }
            else
            {
                rService.RemoveRole(role);
                rt.message = "删除成功.";
            }

            return ReturnResult(rt);
        }

        [HttpPost]
        public ContentResult Add()
        {
            ResultData<string> rt = new ResultData<string>();

            SRole r = GetAdd<SRole>();

            //非空验证和属性格式验证
            string msg = rService.CheckRole(r);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证角色是否存在
            if (rService.IsNotExits(r, true))
            {
                r.CreateBy = this.CurrentUser.ID;
                r.CreateDate = DateTime.Now;
                rService.AddRole(r);
            }
            else
            {
                rt.status = -1;
                rt.message = "角色名称已经存在.";
            }
            return ReturnResult(rt);
        }

        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            SRole r = GetUpdate<SRole>();

            //非空验证和属性格式验证
            string msg = rService.CheckRole(r);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证角色是否存在
            if (rService.IsNotExits(r, false))
            {
                rService.UpdateRole(r);
            }
            else
            {
                rt.status = -1;
                rt.message = "角色名称已经存在.";
            }
            return ReturnResult(rt);
        }
        #endregion
    }
}
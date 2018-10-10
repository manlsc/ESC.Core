using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ESC.Core;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;

namespace ESC.Web.Controllers
{
    /// <summary>
    /// 用户管理 +
    /// </summary>
    public class SUserController : BaseController
    {
        SUserService uService = new SUserService();
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        #region 弹层选择

        /// <summary>
        /// 查询所有人员
        /// 没有任何条件限制  单选
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchUserView()
        {
            return View();
        }

        /// <summary>
        /// 查询所有人员
        /// 没有任何限制条件  多选
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchUsersView()
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
            InitData idata = BaseInit(typeof(SUser), true);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {
                    case "SuperUser":
                        item.combox = GetCombox().items;
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
            var page = uService.PageSearch(pageIndex, pageSize, whereItems);
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
            SUser user = GetDelete<SUser>();
            if (user.UserCode.Equals("admin"))
            {
                rt.status = -1;
                rt.message = "系统管理员不能删除.";
            }
            else
            {
                uService.RemoveUser(user);
                rt.message = "删除成功.";
            }
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

            SUser u = GetAdd<SUser>();

            //非空验证和属性格式验证
            string msg = uService.CheckUser(u);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证用户是否存在
            if (uService.IsNotExits(u, true))
            {
                u.CreateBy = this.CurrentUser.ID;
                u.CreateDate = DateTime.Now;
                u.UpdateDate = DateTime.Now;
                //获取默认密码
                //string dftPassord = new ConfigurationManagerWrapper().AppSettings["DefaultPwd"];
               // u.Pwd = dftPassord;

                uService.AddUser(u);
            }
            else
            {
                rt.status = -1;
                rt.message = "用户编码已经存在.";
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

            SUser u = GetUpdate<SUser>();

            //非空验证和属性格式验证
            string msg = uService.CheckUser(u);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证用户是否存在
            if (uService.IsNotExits(u, false))
            {
                u.UpdateDate = DateTime.Now;
                u.UpdateBy = this.CurrentUser.ID;
                uService.UpdateUser(u);
            }
            else
            {
                rt.status = -1;
                rt.message = "用户编码已经存在.";
            }
            return ReturnResult(rt);
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult ResetPassword()
        {
            ResultData<string> rt = new ResultData<string>();

            int userId = GetParam<int>("userId");

           // string dftPassord = new ConfigurationManagerWrapper().AppSettings["DefaultPwd"];
            //uService.ResetPassword(dftPassord, userId);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public ContentResult GetUserCodeName()
        {
            string codeName = GetParam("words");
            ResultData<List<SUser>> rt = new ResultData<List<SUser>>();
            rt.result = uService.GetUserCodeName(codeName);
            return ReturnResult(rt);
        }
        #endregion
    }
}
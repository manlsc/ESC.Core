using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure;
using Newtonsoft.Json;
using ESC.Service;
using System.Threading;
using System.Data;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace ESC.Web.Controllers
{
    public class BaseController : Controller
    {
        //当前用户
        protected SUser CurrentUser { set; get; }
        //日志服务
        protected LogService logService = new LogService();
        protected SCommonEnumService ceService = new SCommonEnumService();

        #region 重写基类方法
        /// <summary>
        /// 方法执行完毕,添加用户行为
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            CurrentUser = new SUserService().GetUserByCode("admin").FirstOrDefault();
            CurrentUser.Remark = string.Format("欢迎：{0} {1}", CurrentUser.UserName, DateTime.Now.ToString("yyyy年HH月dd日"));
            ViewBag.CurrentUser = CurrentUser;

            //记录行为日志  增删改
            if (context.HttpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                SBehaviorLog log = new SBehaviorLog()
                {
                    HttpMethod = context.HttpContext.Request.Method,
                    ReqDate = DateTime.Now,
                    ReqIP = context.HttpContext.Request.Host.Host,
                    ReqUrl = context.HttpContext.Request.Path.Value,
                    UserId = CurrentUser.ID,
                    ReqParams = GetFormData(),
                    UserName = CurrentUser.UserName
                };
                logService.AddBehaviorLog(log);
            }
        }
        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="inBtn">是否包括按钮权限</param>
        /// <returns></returns>
        protected InitData BaseInit(string tableName, bool inBtn)
        {
            InitData idata = new InitData();
            SPermissionService pService = new SPermissionService();
            if (inBtn)
            {
                //按钮权限
                List<SOperator> operators = pService.GetOPermissionByUserAndResource(this.CurrentUser, this.ToString());
                foreach (SOperator item in operators)
                {
                    BUICommand cmd = new BUICommand() { CommandName = item.OperatorName, Description = item.OperatorDesc, OnClick = item.OnClick, IconClass = item.IconClass };
                    idata.Commands.Add(cmd);
                }
            }

            //列权限
            List<SCPermission> itemumns = pService.GetCPermissionByUserAndResource(this.CurrentUser, tableName, this.ToString());
            foreach (SCPermission item in itemumns)
            {
                BUIColumn col = new BUIColumn();
                col.visible = item.Visible > 0;
                col.required = item.Required > 0;
                col.title = item.Title;
                col.datatype = item.ColumnType;
                col.dataIndex = item.ColumnName;
                col.disabled = item.Disabled > 0;
                col.displayfield = item.DisplayColumn;
                col.width = item.Width;
                idata.Columns.Add(col);
            }

            return idata;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tableName">实体类型</param>
        /// <param name="inBtn">是否包括按钮权限</param>
        /// <returns></returns>
        protected InitData BaseInit(Type type, bool inBtn)
        {
            InitData idata = BaseInit(type.Name, inBtn);
            ////如果没有初始化则自动初始化
            //if (idata.Columns.Count < 1)
            //{
            //    DatabaseContext dbContext = new DatabaseContext();
            //    IEnumerable<PocoColumn> columns = dbContext.GetPocoColumns(type, null);
            //    List<SColumn> sColumns = new List<SColumn>(columns.Count());
            //    foreach (PocoColumn kvp in columns)
            //    {
            //        idata.Columns.Add(new BUIColumn()
            //        {
            //            visible = kvp.Visible,
            //            required = kvp.Required,
            //            title = kvp.Title,
            //            datatype = kvp.ColumnType.ToLower(),
            //            dataIndex = kvp.ColumnName,
            //            disabled = kvp.Disabled,
            //            displayfield = kvp.DisplayColumn,
            //            width = kvp.Width
            //        });
            //        sColumns.Add(new SColumn()
            //        {
            //            ColumnName = kvp.ColumnName,
            //            Title = kvp.Title,
            //            ColumnType = kvp.ColumnType.ToLower(),
            //            Disabled = kvp.Disabled ? 1 : 0,
            //            DisplayColumn = kvp.DisplayColumn,
            //            Visible = kvp.Visible ? 1 : 0,
            //            Required = kvp.Required ? 1 : 0,
            //            TableDesc = type.Name,
            //            TableName = type.Name,
            //            Width = kvp.Width
            //        });
            //    }
            //    dbContext.BatchInsert(sColumns);
            //}
            return idata;
        }

        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected ComboxData GetCombox(string field)
        {
            ComboxData cd = new ComboxData();
            List<SCommonEnum> comms = ceService.GetCommonEnumByType(field);
            foreach (SCommonEnum item in comms)
            {
                cd.items.Add(new ComboxDataItem()
                {
                    text = item.EnumDesc,
                    value = Convert.ToInt32(item.EnumField)
                });
            }
            return cd;
        }

        #endregion

        #region 文件下载/上传

        /// <summary>
        /// 根据文件名返回相应的文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [NonAction]
        protected FileResult BaseDownloadFile(string fileName, DataTable dt)
        {
            NopiExcelManager manager = new NopiExcelManager();
            MemoryStream stream = manager.DataTableToExcel(dt, fileName);
            byte[] fileContents = stream.ToArray();
            return BaseDownloadFile(fileName, fileContents);
        }

        /// <summary>
        /// 根据文件名返回相应的文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [NonAction]
        protected FileResult BaseDownloadFile(string fileName, byte[] fileContents)
        {
            //文件后缀
            string fileExt = fileName.Substring(fileName.LastIndexOf('.'));
            string contentType = "";
            switch (fileExt.ToLower())
            {
                case ".xls":
                case ".xlsx":
                    contentType = "application/x-excel";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/msword";
                    break;
                case "gif":
                case "jpeg":
                case "png":
                case "tiff":
                    contentType = "image/" + fileExt.ToLower();
                    break;
                case ".pdf":
                case ".zip":
                case ".xml":
                    contentType = "application/" + fileExt.ToLower();
                    break;
                default:
                    contentType = "text/plain";
                    break;
            }
            FileContentResult fcr = new FileContentResult(fileContents, contentType);
            fcr.FileDownloadName = fileName;
            return fcr;
        }

        ///// <summary>
        ///// 保存
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //protected string FileSaveAS(HttpPostedFileBase file)
        //{
        //    string target = Server.MapPath("~/Uploads");  //文件路径
        //    string filename = FileHelper.GetFileName(file.FileName);//取得文件名字
        //    string path = FileHelper.AddSlash(target) + filename;//获取存储的目标地址
        //    file.SaveAs(path);
        //    return path;
        //}

        #endregion

        #region 系列化/反序列化

        /// <summary>
        /// 将对象转换成JSON
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string SerializeObject(object value)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(value, Formatting.Indented, timeFormat);
        }

        /// <summary>
        /// 将JSON字符串转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        #endregion

        #region 重写返回

        /// <summary>
        /// 重写JsonResult
        /// 由于自带的JsonResult不支持DataTable等对象的序列化
        /// 所以手动构建JsonResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected ContentResult ReturnResult(object result)
        {
            ContentResult cr = new ContentResult();
            cr.ContentType = "application/json";
            cr.Content = SerializeObject(result);
            return cr;
        }

        #endregion

        #region Request参数

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        //public string BaseUploadFile(string fileParam)
        //{
        //    HttpPostedFileBase file = Request.Files[fileParam];
        //    if (file != null)
        //    {
        //        string filePath = Path.Combine(HttpContext.Server.MapPath("/Uploads"), file.FileName);
        //        file.SaveAs(filePath);
        //        return filePath;
        //    }
        //    return string.Empty;
        //}

        /// <summary>
        /// 获取回传参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetParam(string name)
        {
            StringValues value = StringValues.Empty;
            if (Request.Method=="POST")
            {
                value = Request.Form[name];
            }
            if (value == StringValues.Empty)
            {
                value = Request.Query[name];
            }
            if (value == StringValues.Empty)
            {
                return "";
            }
            return value.FirstOrDefault();
        }

        /// <summary>
        /// 获取指定类型参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T GetParam<T>(string name)
        {
            var param = GetParam(name);
            if (string.IsNullOrEmpty(param))
            {
                return default(T);
            }
            return DeserializeObject<T>(param);
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        protected List<WhereItem> GetWhereItems()
        {
            var wItems = GetParam("whereItems");
            if (string.IsNullOrEmpty(wItems))
            {
                return new List<WhereItem>();
            }
            return DeserializeObject<List<WhereItem>>(wItems);
        }

        /// <summary>
        /// 获取删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetDelete<T>()
        {
            var dels = GetParam("delete");
            if (string.IsNullOrEmpty(dels))
            {
                return default(T);
            }
            return DeserializeObject<T>(dels);
        }

        /// <summary>
        /// 获取添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetAdd<T>()
        {
            var add = GetParam("add");
            if (string.IsNullOrEmpty(add))
            {
                return default(T);
            }
            return DeserializeObject<T>(add);
        }

        /// <summary>
        /// 获取更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetUpdate<T>()
        {
            var upd = GetParam("update");
            if (string.IsNullOrEmpty(upd))
            {
                return default(T);
            }
            return DeserializeObject<T>(upd);
        }

        /// <summary>
        /// 获取页码
        /// </summary>
        /// <returns></returns>
        protected long GetPageIndex()
        {
            string page = GetParam("page");
            long p = 1;
            if (!long.TryParse(page, out p))
            {
                p = 1;
            }
            return p;
        }

        /// <summary>
        /// 获取页码
        /// </summary>
        /// <returns></returns>
        protected long GetPageSize()
        {
            string page = GetParam("rows");
            long p = 20;
            if (!long.TryParse(page, out p))
            {
                p = 20;
            }
            return p;
        }

        /// <summary>
        /// 获取父ID
        /// <remarks>一般用于SearchDetail方法</remarks>
        /// </summary>
        /// <returns></returns>
        protected int GetParentID()
        {
            string parentID = GetParam("parentID");
            return string.IsNullOrEmpty(parentID) ? 0 : int.Parse(parentID);
        }

        /// <summary>
        /// 获取外建表
        /// <remarks>一般用于ForeignInit方法</remarks>
        /// </summary>
        /// <returns></returns>
        protected string GetTable()
        {
            return GetParam("table");
        }

        /// <summary>
        /// 获取外键参数
        /// <remarks>一般用于ForeignSearch方法</remarks>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetForeign<T>()
        {
            string foreign = GetParam("foreign");
            if (string.IsNullOrEmpty(foreign))
            {
                return default(T);
            }
            return DeserializeObject<T>(foreign);
        }

        /// <summary>
        /// 获取前台传递Form
        /// </summary>
        /// <returns></returns>
        protected string GetFormData()
        {
            string sb = "";
            foreach (string key in Request.Form.Keys)
            {
                sb = sb + key + ":" + Request.Form[key] + ",";
            }
            if (sb.Length > 4000)
            {
                return sb.Substring(0, 3999);
            }
            return sb;
        }

        #endregion
    }
}

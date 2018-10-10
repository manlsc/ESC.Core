using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.WebCore.ViewComponents
{
    /// <summary>
    /// 左侧菜单
    /// </summary>
    public class SideMenu : ViewComponent
    {
        /// <summary>
        /// 获取左侧菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="superUser"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int userId, int superUser, string url)
        {
            StringBuilder sb = new StringBuilder();

            List<TreeNode> tns = GetResources(userId, superUser, url);
            foreach (TreeNode item in tns)
            {
                sb.AppendLine(GetItemTpl(item));
            }
            return new HtmlContentViewComponentResult(new HtmlString(sb.ToString()));
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="superUser"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static List<TreeNode> GetResources(int userId, int superUser, string url)
        {
            SPermissionService pService = new SPermissionService();
            //查询当前用户的所有页面权限
            List<SResource> resouses = pService.GetRPermissionByUser(userId, superUser > 0);
            List<SResource> parents = resouses.Select(r => r).Where(t => t.ParentID == 0).ToList();

            List<TreeNode> tns = new List<TreeNode>();
            foreach (SResource parent in parents)
            {
                TreeNode tn = new TreeNode();
                tn.id = "tree" + parent.ID;
                tn.text = string.IsNullOrEmpty(parent.ResourceDesc) ? "菜单" : parent.ResourceDesc;
                tn.href = string.IsNullOrEmpty(parent.ResourceURL) ? "javascript:###;" : parent.ResourceURL;
                tn.name = parent.ResourceName;
                tn.chdCls = "treeview-menu";
                tn.level = 1;
                tn.children.AddRange(GetTree(parent, resouses, tn, url));
                tn.iconCls = "iconfont icon-folder";
                tns.Add(tn);
            }
            return tns;
        }

        /// <summary>
        /// 递归遍历
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="resouses"></param>
        /// <param name="tn"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static List<TreeNode> GetTree(SResource parent, List<SResource> resouses, TreeNode tp, string url)
        {
            List<TreeNode> tns = new List<TreeNode>();
            string curUrl = url.Substring(1, url.LastIndexOf("/"));

            List<SResource> children = resouses.Select(r => r).Where(t => t.ParentID == parent.ID).ToList();
            foreach (SResource child in children)
            {
                TreeNode tn = new TreeNode();
                tn.id = "tree" + child.ID;
                tn.text = string.IsNullOrEmpty(child.ResourceDesc) ? "菜单" : child.ResourceDesc;
                tn.href = string.IsNullOrEmpty(child.ResourceURL) ? "javascript:###;" : child.ResourceURL;
                tn.name = child.ResourceName;
                tn.level = 2;
                tn.children = GetTree(child, resouses, tn, url);
                if (tn.children.Count > 0)
                {
                    tn.iconCls = "iconfont icon-folder";
                }
                else
                {
                    tn.iconCls = "iconfont icon-file";
                }

                if (tn.href.LastIndexOf(curUrl) > -1)
                {
                    tn.state = "active";
                    tp.state = "active";
                }
                tns.Add(tn);
            }
            return tns;
        }

        /// <summary>
        /// 拼接html字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string GetItemTpl(TreeNode item)
        {
            StringBuilder tpl = new StringBuilder();
            if (item.level < 2)
            {
                tpl.AppendLine("<li class=\"treeview " + item.state + "\"><a href=\"javascript:###;\">");
            }
            else
            {
                tpl.AppendLine("<li class=\"" + item.state + "\"><a href=\"" + item.href + "\">");
            }
            tpl.Append("<i class=\"" + item.iconCls + "\"></i>");
            if (item.level < 2)
            {
                tpl.Append("<span>" + item.text + "</span>");
            }
            else
            {
                tpl.Append(item.text);
            }
            if (item.children.Count > 0)
            {
                if (item.state == "active")
                {
                    item.state = " menu-open";
                }
                tpl.AppendLine("<span class=\"pull-right-container\"><i class=\"iconfont icon-left1 pull-right\"></i></span>");
                tpl.AppendLine("<ul class=\"" + item.chdCls + item.state + "\">");
                foreach (TreeNode chd in item.children)
                {
                    tpl.AppendLine(GetItemTpl(chd));
                }
                tpl.AppendLine("</ul>");
            }
            tpl.Append("</a></li>");
            return tpl.ToString();
        }
    }
}

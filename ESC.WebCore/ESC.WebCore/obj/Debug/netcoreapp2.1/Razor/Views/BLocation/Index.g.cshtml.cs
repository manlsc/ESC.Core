#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1167231a881ecdda534a8520e27cb0382c29c070"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BLocation_Index), @"mvc.1.0.view", @"/Views/BLocation/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/BLocation/Index.cshtml", typeof(AspNetCore.Views_BLocation_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\_ViewImports.cshtml"
using ESC.WebCore;

#line default
#line hidden
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\_ViewImports.cshtml"
using ESC.WebCore.Models;

#line default
#line hidden
#line 3 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\_ViewImports.cshtml"
using ESC.Infrastructure.DomainObjects;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1167231a881ecdda534a8520e27cb0382c29c070", @"/Views/BLocation/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_BLocation_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(86, 13902, true);
            WriteLiteral(@"<div id=""location-layout"" class=""bui-view-port bui-border-layout"">
    <div class=""bui-layout-border"">
        <div class=""bui-border-middle"">
            <div id=""location-layout-west"" style=""width:200px;border-right:2px solid #ddd"" class=""bui-border-west bui-layout-item-border"">
            </div>
            <div id=""location-layout-center"" style=""overflow-y:auto;"" class=""bui-border-center bui-layout-item-border"">
                <div id=""BLocationBtns"" class=""SearchBtns"" style=""width:100%;border:1px solid #ddd;"">
                </div>
                <div id=""BLocationForm"" class=""bui-clear"">
                    <table style=""border-collapse:separate;"" width=""100%"">
                        <tr>
                            <td align=""right"" style=""padding:5px;"">
                                编码
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""LocationCode"" style=""wi");
            WriteLiteral(@"dth: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                名称
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""LocationDesc"" style=""width: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                类型
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""LocationTypeName"" style=""width: 180px;"" />
                            </td>
                        </tr>
                        <tr>
                            <td align=""right"" style=""padding:5px;"">
                                组织机构
                            </td>
                            <td style=""padding:5px;"">");
            WriteLiteral(@"
                                <input class=""form-control"" readonly=""readonly"" name=""OrgName"" style=""width: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                备注
                            </td>
                            <td colspan=""2"" style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""Remark"" style=""width: 360px;"" />
                            </td>
                            <td >
                                <input name=""ID"" type=""hidden"" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id=""UserGrid"">
                </div>
            </div>
        </div>
    </div>
</div>
<script type=""text/javascript"">
    var common = new CommonBUI({ controller: ""BLocation"", DataGrid: ""#UserGrid"", Btns: ""#BLocationBtns"" });
    var fcommon = new C");
            WriteLiteral(@"ommonBUI({ controller: ""BLocation"", DataGrid: ""#UserGrid"", Btns: ""#BLocationBtns"" });
    var store = new BUI.Data.TreeStore(), tree, oGrid, locData;

    //初始化布局
    function initLayout() {
        var layoutControl = new BUI.Layout.BuiLayout({
            srcNode: ""#location-layout"",
            render: ""parent"",
            children: [{
                srcNode: ""#location-layout-west"",
                xclass: 'controller',
                region: 'west',
                width: 200
            }, {
                srcNode: ""#location-layout-center"",
                xclass: 'controller',
                region: 'center'
            }]
        });
        layoutControl.render();
    }

    //初始化结构树
    function initTree() {
        tree = new BUI.Tree.TreeList({
            render: '#location-layout-west',
            store: store
        });
        tree.render();

        tree.on('itemclick', function (ev) {
            var item = ev.item;
            loadGrid(item.id);
    ");
            WriteLiteral(@"        loadLocation(item.id);
        });

        common.search();
    }

    //初始化grid
    function initGrid(initData) {
        oGrid = new BUIGrid({
            render: fcommon.DataGrid,
            common: fcommon,
            checkbox: true,
            pager: false
        });
        oGrid.init();
    }

    //初始化列显示隐藏
    function initUserColumns(iData) {
        for (var i = 0; i < iData.Columns.length; i++) {
            switch (iData.Columns[i].dataIndex) {
                case ""UserCode"":
                case ""UserName"":
                case ""SuperUser"":
                case ""OrgID"":
                    iData.Columns[i].visible = true;
                    break;
                default:
                    iData.Columns[i].visible = false;
            }
        }
    }

    //查询按钮
    function loadGrid(locId) {
        common.SelectDetail(""SUser"", locId, 1, 20, function (result) {
            oGrid.setResult(result);
        });
    }

    //查询资源
    func");
            WriteLiteral(@"tion loadLocation(locId) {
        common.get(common.controller, ""SearchSingle"", ""Id="" + locId, function (data) {
            locData = data;
            $(""#BLocationForm .form-control"").each(function () {
                $(this).val(data[this.name]);
            });
        });
    }

    //添加
    common.add = function () {
        var form = new BuiForm({ common: common, data: {}, type: ""add"" });

        form.save = function (data) {
            common.Insert(data, function () {
                common.search();
                form.close();
            });
        }

        //外键查询
        form.fSearch = function (opts) {
            if (BUI.isNullOrEmpty(opts.text) || opts.evttype == ""click"") {
                if (opts.param.table == ""Organization"") {
                    searchOrg(form, opts);
                }else {
                    searchLoc(form, opts);
                }
            } else {
                if (opts.param.table == ""Organization"") {
                    ");
            WriteLiteral(@"//查询业务伙伴
                    common.get(""SOrganization"", ""GetOrganizationCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.length == 1) {
                            var data = rdata.result[0];
                            var target = opts.target;
                            target.setSearchValue(data.ID || 0);
                            target.setSearchText(data.OrgName || """");
                        } else {
                            searchOrg(form, opts);
                        }
                    });
                }  else {
                    searchLoc(form, opts);
                }
            }          
        }

        form.show(800, 300);
    }

    //删除
    common.remove = function () {
        var deleteRow = tree.getSelected();
        if (deleteRow == null) {
            CommonBUI.alert(""提示"", ""请选择存储单元."");
            return false;
        }
        if (deleteRow.children.length > 0) {
         ");
            WriteLiteral(@"   CommonBUI.alert(""提示"", ""当前节点存在子存储单元,不能删除."");
            return false;
        }
        common.Delete(deleteRow.id, function () {
            common.search();
        });
    }

    //查询
    common.search = function () {
        common.get(common.controller, ""GetTree"", """", function (treedata) {
            store.setResult(treedata);
        });
    }

    //编辑
    common.edit = function () {
        var deleteRow = tree.getSelected();
        if (deleteRow) {
            var form = new BuiForm({ data: locData, common: common, type: ""edit"" });

            //外键查询
            form.fSearch = function (opts) {
                if (BUI.isNullOrEmpty(opts.text) || opts.evttype == ""click"") {
                    if (opts.param.table == ""Organization"") {
                        searchOrg(form, opts);
                    } else {
                        searchLoc(form, opts);
                    }
                } else {
                    if (opts.param.table == ""Organization"") {
   ");
            WriteLiteral(@"                     //查询业务伙伴
                        common.get(""SOrganization"", ""GetOrganizationCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                            if (rdata.result.length == 1) {
                                var data = rdata.result[0];
                                var target = opts.target;
                                target.setSearchValue(data.ID || 0);
                                target.setSearchText(data.OrgName || """");
                            } else {
                                searchOrg(form, opts);
                            }
                        });
                    } else {
                        searchLoc(form, opts);
                    }
                }
            }

            form.save = function (data) {
                common.Update(data, function () {
                    common.search();
                    form.close();
                });
            }
            form.show(800, 300)");
            WriteLiteral(@";
        } else {
            CommonBUI.alert(""提示"", ""请选择编辑的节点."");
        }
    }

    //初始化列
    function initColumns(columns) {
        $.each(columns,
          function (index, item) {
              columns[index] = new BUI.Grid.Column(item);
          });
    }

    //选择存储单元机构
    function searchOrg(frm, opts) {
        var dialog = new BUI.Overlay.Dialog({
            title: ""选择组织机构"",
            width: 300,
            height: 500,
            mask: true,
            closeAction: ""destroy"",
            bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../SOrganization/SearchOrgView"" style=""width:100%;height:100%;""></iframe>',
            success: function () {
                var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                if (data) {
                    frm.setReturnValue(opts, data);
                    dialog.close();
                } else {
                    frm.setReturnValue(opts, {});
                 ");
            WriteLiteral(@"   dialog.close();
                }
            }
        });
        dialog.show();
    }

    //选择存储单元
    function searchLoc(frm, opts) {
        var dialog = new BUI.Overlay.Dialog({
            title: ""选择存储单元"",
            width: 300,
            height: 500,
            mask: true,
            closeAction: ""destroy"",
            bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BLocation/SearchLocationTreeView"" style=""width:100%;height:100%;""></iframe>',
            success: function () {
                var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                if (data) {
                    frm.setReturnValue(opts, data);
                    dialog.close();
                } else {
                    frm.setReturnValue(opts, {});
                    dialog.close();
                }
            }
        });
        dialog.show();
    }

    //选择用户
    common.searchUser=function() {
        var deleteRow = tree.get");
            WriteLiteral(@"Selected();
        if (deleteRow==null) {
            CommonBUI.alert(""提示"", ""请选择存储单元."");
            return false;
        }
        var dialog = new BUI.Overlay.Dialog({
            title: ""选择用户"",
            width: 800,
            height: 500,
            mask: true,
            closeAction: ""destroy"",
            bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../SUser/SearchUsersView"" style=""width:100%;height:100%;""></iframe>',
            success: function () {
                var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                var userIds = [];
                for (var i = 0; i < data.length; i++) {
                    userIds.push(data[i].ID);
                }
                var addItems = ""add="" + BUI.JSON.stringify(userIds) + ""&locationId="" + deleteRow.id;
                common.post(common.controller, ""AddUser"", addItems, function () {
                    loadGrid(deleteRow.id);
                    dialog.close();
 ");
            WriteLiteral(@"               });
            }
        });
        dialog.show();
    }

    //删除用户
    common.removeUser=function() {
        var data = oGrid.getSelected();
        if (data.length < 1) {
            CommonBUI.alert(""提示"", ""请选择要删除的用户"");
            return false;
        }
        var deleteRow = tree.getSelected();
        if (deleteRow == null) {
            CommonBUI.alert(""提示"", ""请选择存储单元."");
            return false;
        }

        var userIds = [];
        for (var i = 0; i < data.length; i++) {
            userIds.push(data[i].ID);
        }
        var deleteItems = ""delete="" + BUI.JSON.stringify(userIds) + ""&locationId="" + deleteRow.id;
        common.post(common.controller, ""RmoveUser"", deleteItems, function () {
            CommonBUI.alert(""提示"", ""删除成功."");
            loadGrid(deleteRow.id);
        });
    }

    $(document).ready(function () {
        //初始化布局
        initLayout();

        //初始化
        common.Init(function (iData) {
            //按钮初始化
   ");
            WriteLiteral(@"         common.InitCommonds(common, $(common.Btns), iData.Commands);
            initColumns(common.initData.Columns);
        });

        //外键初始化
        common.ForeignInit(""SUser"", function (fdata) {
            initUserColumns(fdata);
            fcommon.initData = fdata;
            initGrid();
        });

        //初始化存储单元结构树
        initTree();

        //绑定回车查询
        $(common.SearchForm).find(""input:text"").keyup(function (e) {
            if (e.which == BUI.KeyCode.ENTER) {
                common.search();
            }
        });
    });
</script>
");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591

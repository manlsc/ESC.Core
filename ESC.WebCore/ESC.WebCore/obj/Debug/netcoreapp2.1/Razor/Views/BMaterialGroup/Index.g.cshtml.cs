#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BMaterialGroup\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ac220a19f8149637af5a12cf73cb52507d918f17"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BMaterialGroup_Index), @"mvc.1.0.view", @"/Views/BMaterialGroup/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/BMaterialGroup/Index.cshtml", typeof(AspNetCore.Views_BMaterialGroup_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ac220a19f8149637af5a12cf73cb52507d918f17", @"/Views/BMaterialGroup/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_BMaterialGroup_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BMaterialGroup\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(86, 2911, true);
            WriteLiteral(@"<div id=""location-layout"" class=""bui-view-port bui-border-layout"">
    <div class=""bui-layout-border"">
        <div class=""bui-border-middle"">
            <div id=""location-layout-west"" style=""width:200px;border-right:2px solid #ddd"" class=""bui-border-west bui-layout-item-border"">
            </div>
            <div id=""location-layout-center"" style=""overflow-y:auto;"" class=""bui-border-center bui-layout-item-border"">
                <div id=""BMaterialGroupBtns"" class=""SearchBtns"" style=""width:100%;border:1px solid #ddd;"">
                </div>
                <div id=""BMaterialGroupForm"" class=""bui-clear"">
                    <table style=""border-collapse:separate;"" width=""100%"">
                        <tr>
                            <td align=""right"" style=""padding:5px;"">
                                编码
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""GroupCode"" st");
            WriteLiteral(@"yle=""width: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                名称
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""GroupName"" style=""width: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                父物料组
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""ParentName"" style=""width: 180px;"" />
                            </td>
                        </tr>
                        <tr>
                            <td align=""right"" style=""padding:5px;"">
                                物料组机构
                            </td>
                            <td style=""padding:5px;""");
            WriteLiteral(@">
                                <input class=""form-control"" readonly=""readonly"" name=""OrgName"" style=""width: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                备注
                            </td>
                            <td colspan=""2"" style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""Remark"" style=""width: 360px;"" />
                            </td>
                            <td>
                                <input name=""ID"" type=""hidden"" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id=""LocationGrid"">
                </div>
            </div>
        </div>
    </div>
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(3014, 12380, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""BMaterialGroup"", DataGrid: ""#LocationGrid"", Btns: ""#BMaterialGroupBtns"" });
        var fcommon = new CommonBUI({ controller: ""BMaterialGroup"", DataGrid: ""#LocationGrid"", Btns: ""#BMaterialGroupBtns"" });
        var store = new BUI.Data.TreeStore(), tree, oGrid, grpData;

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
      ");
                WriteLiteral(@"  function initTree() {
            tree = new BUI.Tree.TreeList({
                render: '#location-layout-west',
                store: store
            });
            tree.render();

            tree.on('itemclick', function (ev) {
                var item = ev.item;
                loadGrid(item.id);
                loadMaterialGroup(item.id);
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

        //查询按钮
        function loadGrid(groupId) {
            common.SelectDetail(""BLocation"", groupId, 1, 20, function (result) {
                oGrid.setResult(result);
            });
        }

        //查询资源
        function loadMaterialGroup(groupId) {
            common.get(common.control");
                WriteLiteral(@"ler, ""SearchSingle"", ""Id="" + groupId, function (data) {
                grpData = data;
                $(""#BMaterialGroupForm .form-control"").each(function () {
                    $(this).val(data[this.name]);
                });
            });
        }

        //添加
        common.add = function () {
            var data = {};
            if (grpData) {
                data.ParentID = grpData.ID;
                data.ParentName = grpData.GroupName;
            }

            var form = new BuiForm({ common: common, data: data, type: ""add"" });

            form.save = function (data) {
                common.Insert(data, function () {
                    common.search();
                    form.close();
                });
            }

            //外键查询
            form.fSearch = function (opts) {
                if (opts.param.table == ""Organization"") {
                    //查询业务伙伴
                    common.get(""SOrganization"", ""GetOrganizationCodeName"", ""words="" + Commo");
                WriteLiteral(@"nBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.length == 1) {
                            var data = rdata.result[0];
                            var target = opts.target;
                            target.setSearchValue(data.ID || 0);
                            target.setSearchText(data.OrgName || """");
                        } else {
                            //查询物料组
                            common.get(""BMaterialGroup"", ""GetGroupCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                                if (rdata.result.length == 1) {
                                    var data = rdata.result[0];
                                    var target = opts.target;
                                    target.setSearchValue(data.ID || 0);
                                    target.setSearchText(data.GroupName || """");
                                } else {
                                    searchGoup(form, opts);
         ");
                WriteLiteral(@"                       }
                            });
                        }
                    });
                } else {
                    searchGoup(form, opts);
                }
            }

            form.show(900, 300);
        }

        //删除
        common.remove = function () {
            var deleteRow = tree.getSelected();
            if (deleteRow == null) {
                CommonBUI.alert(""提示"", ""请选择物料组."");
                return false;
            }

            if (deleteRow.children.length > 0) {
                CommonBUI.alert(""提示"", ""当前节点存在子物料组,不能删除."");
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
        common.edit = fu");
                WriteLiteral(@"nction () {
            var deleteRow = tree.getSelected();
            if (deleteRow) {
                var form = new BuiForm({ data: grpData, common: common, type: ""edit"" });

                //外键查询
                form.fSearch = function (opts) {
                    if (opts.param.table == ""Organization"") {
                        //查询业务伙伴
                        common.get(""SOrganization"", ""GetOrganizationCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                            if (rdata.result.length == 1) {
                                var data = rdata.result[0];
                                var target = opts.target;
                                target.setSearchValue(data.ID || 0);
                                target.setSearchText(data.OrgName || """");
                            } else {
                                //查询物料组
                                common.get(""BMaterialGroup"", ""GetGroupCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text)");
                WriteLiteral(@", function (rdata) {
                                    if (rdata.result.length == 1) {
                                        var data = rdata.result[0];
                                        var target = opts.target;
                                        target.setSearchValue(data.ID || 0);
                                        target.setSearchText(data.GroupName || """");
                                    } else {
                                        searchGoup(form, opts);
                                    }
                                });
                            }
                        });
                    } else {
                        searchGoup(form, opts);
                    }
                }

                form.save = function (data) {
                    common.Update(data, function () {
                        common.search();
                        form.close();
                    });
                }
                form.show(900, 300);");
                WriteLiteral(@"
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

        //选择物料组机构
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
                        dialog.c");
                WriteLiteral(@"lose();
                    } else {
                        frm.setReturnValue(opts, {});
                        dialog.close();
                    }
                }
            });
            dialog.show();
        }

        //选择存储单元
        function searchGoup(frm, opts) {
            var dialog = new BUI.Overlay.Dialog({
                title: ""选择物料组"",
                width: 300,
                height: 500,
                mask: true,
                closeAction: ""destroy"",
                bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BMaterialGroup/SearchMaterialGroupView"" style=""width:100%;height:100%;""></iframe>',
                success: function () {
                    var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                    if (data) {
                        frm.setReturnValue(opts, data);
                        dialog.close();
                    } else {
                        frm.setReturnValue(opts");
                WriteLiteral(@", {});
                        dialog.close();
                    }
                }
            });
            dialog.show();
        }

        //选择存储单元
        common.searchLocation = function () {
            var deleteRow = tree.getSelected();
            if (deleteRow == null) {
                CommonBUI.alert(""提示"", ""请选择物料组."");
                return false;
            }
            var dialog = new BUI.Overlay.Dialog({
                title: ""选择存储单元"",
                width: 800,
                height: 500,
                mask: true,
                closeAction: ""destroy"",
                bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BLocation/SearchLocationWhView"" style=""width:100%;height:100%;""></iframe>',
                success: function () {
                    var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                    var locationIds = [];
                    if (data) {
                        locationIds.");
                WriteLiteral(@"push(data.ID);
                    }
                    var addItems = ""add="" + BUI.JSON.stringify(locationIds) + ""&groupId="" + deleteRow.id;
                    common.post(common.controller, ""AddLocation"", addItems, function () {
                        loadGrid(deleteRow.id);
                        dialog.close();
                    });
                }
            });
            dialog.show();
        }

        //删除存储单元
        common.removeLocation = function () {
            var data = oGrid.getSelected();
            if (data.length < 1) {
                CommonBUI.alert(""提示"", ""请选择要删除的存储单元"");
                return false;
            }
            var deleteRow = tree.getSelected();
            if (deleteRow == null) {
                CommonBUI.alert(""提示"", ""请选择存物料组."");
                return false;
            }

            var locationIds = [];
            for (var i = 0; i < data.length; i++) {
                locationIds.push(data[i].ID);
            }
           ");
                WriteLiteral(@" var deleteItems = ""delete="" + BUI.JSON.stringify(locationIds) + ""&groupId="" + deleteRow.id;
            common.post(common.controller, ""RmoveLocation"", deleteItems, function () {
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
                common.InitCommonds(common, $(common.Btns), iData.Commands);
                initColumns(common.initData.Columns);
            });

            //外键初始化
            common.ForeignInit(""BLocation"", function (fdata) {
                fcommon.initData = fdata;
                initGrid();
            });

            //初始化物料组结构树
            initTree();

            //绑定回车查询
            $(common.SearchForm).find(""input:text"").keyup(function (e) {
                if (e.which == BUI.KeyCode.ENTER) {
              ");
                WriteLiteral("      common.search();\r\n                }\r\n            });\r\n        });\r\n    </script>\r\n    ");
                EndContext();
            }
            );
            BeginContext(15397, 4, true);
            WriteLiteral("\r\n\r\n");
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

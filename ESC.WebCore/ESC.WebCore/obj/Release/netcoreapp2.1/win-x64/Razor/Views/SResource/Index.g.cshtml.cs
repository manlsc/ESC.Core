#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d9e01b6ec48d9657e243e43670fc036acea09909"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SResource_Index), @"mvc.1.0.view", @"/Views/SResource/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/SResource/Index.cshtml", typeof(AspNetCore.Views_SResource_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d9e01b6ec48d9657e243e43670fc036acea09909", @"/Views/SResource/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_SResource_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(84, 2913, true);
            WriteLiteral(@"<div id=""resource-layout"" class=""bui-view-port bui-border-layout"">
    <div class=""bui-layout-border"">
        <div class=""bui-border-middle"">
            <div id=""resource-layout-west"" style=""width:200px;border-right:2px solid #ddd"" class=""bui-border-west bui-layout-item-border"">
            </div>
            <div id=""resource-layout-center"" style=""overflow-y:auto;"" class=""bui-border-center bui-layout-item-border"">
                <div id=""SResourceBtns"" class=""SearchBtns"" style=""width:100%;border:1px solid #ddd;"">
                </div>
                <div id=""SResourceForm"" class=""bui-clear"">
                    <table style=""border-collapse:separate;"" width=""100%"">
                        <tr>
                            <td align=""right"" style=""padding:5px;"">
                                名称
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""ResourceName"" style=""wi");
            WriteLiteral(@"dth: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                资源路径
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""ResourceURL"" style=""width: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                描述
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""ResourceDesc"" style=""width: 180px;"" />
                            </td>
                        </tr>
                        <tr>
                            <td align=""right"" style=""padding:5px;"">
                                父资源
                            </td>
                            <td style=""padding:5px;"">
  ");
            WriteLiteral(@"                              <input class=""form-control"" readonly=""readonly"" name=""ParentName"" style=""width: 180px;"" />
                            </td>
                            <td align=""right"" style=""padding:5px;"">
                                排序
                            </td>
                            <td style=""padding:5px;"">
                                <input class=""form-control"" readonly=""readonly"" name=""OrderIndex"" style=""width: 180px;"" />
                            </td>
                            <td colspan=""2"">
                                <input name=""ID"" type=""hidden"" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id=""OperatorGrid"">
                </div>
            </div>
        </div>
    </div>
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(3014, 5508, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""SResource"", DataGrid: ""#OperatorGrid"", Btns: ""#SResourceBtns"" });
        var fcommon = new CommonBUI({ controller: ""SResource"", DataGrid: ""#OperatorGrid"", Btns: ""#SResourceBtns"" });
        var store = new BUI.Data.TreeStore(), tree, oGrid;

        //初始化布局
        function initLayout() {
            var layoutControl = new BUI.Layout.BuiLayout({
                srcNode: ""#resource-layout"",
                render: ""parent"",
                children: [{
                    srcNode: ""#resource-layout-west"",
                    xclass: 'controller',
                    region: 'west',
                    width: 200
                }, {
                    srcNode: ""#resource-layout-center"",
                    xclass: 'controller',
                    region: 'center'
                }]
            });
            layoutControl.render();
        }

        //初始化结构树
        function initTree() {
    ");
                WriteLiteral(@"        tree = new BUI.Tree.TreeList({
                render: '#resource-layout-west',
                store: store
            });
            tree.render();

            tree.on('itemclick', function (ev) {
                var item = ev.item;
                loadGrid(item.id);
                loadResource(item.id);
            });

            common.search();
        }

        //初始化grid
        function initGrid(initData) {
            oGrid = new BUIGrid({
                render: fcommon.DataGrid,
                common: fcommon,
                pager: false
            });
            oGrid.init();
        }

        //查询按钮
        function loadGrid(resId) {
            common.SelectDetail(""SOperator"", resId, 1, 20, function (result) {
                oGrid.setResult(result);
            });
        }

        //查询资源
        function loadResource(resId) {
            common.get(common.controller, ""SearchSingle"", ""Id="" + resId, function (data) {
                $(""#SRe");
                WriteLiteral(@"sourceForm .form-control"").each(function () {
                    $(this).val(data[this.name]);
                });
            });
        }

        //添加
        common.add = function () {
            var dialog = new BUI.Overlay.Dialog({
                title: ""添加"",
                width: 800,
                height: 550,
                mask: true,
                closeAction: ""destroy"",
                bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../SResource/AddView"" style=""width:100%;height:100%;""></iframe>',
                success: function () {
                    var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                    if (data) {
                        common.Insert(data, function () {
                            dialog.close();
                            common.search();
                        });
                    }
                }
            });
            dialog.show();
        }

        //删除
  ");
                WriteLiteral(@"      common.remove = function () {
            var deleteRow = tree.getSelected();
            if (deleteRow) {
                if (deleteRow.children.length > 0) {
                    CommonBUI.alert(""提示"", ""节点存在子节点,不能删除."");
                    return false;
                } else {
                    common.Delete(deleteRow.id, function () {
                        common.search();
                    });
                }
            } else {
                CommonBUI.alert(""提示"", ""请选择删除菜单."");
                return false;
            }
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
                var dialog = new BUI.Overlay.Dialog({
                    title: ""添加"",
                    widt");
                WriteLiteral(@"h: 800,
                    height: 550,
                    mask: true,
                    closeAction: ""destroy"",
                    bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../SResource/UpdateView?id=' + deleteRow.id + '"" style=""width:100%;height:100%;""></iframe>',
                    success: function () {
                        var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                        if (data) {
                            common.Update(data, function () {
                                dialog.close();
                                common.search();
                            });
                        }
                    }
                });
                dialog.show();
            } else {
                CommonBUI.alert(""提示"", ""请选择编辑菜单."");
            }
        }

        $(document).ready(function () {
            //初始化布局
            initLayout();

            //初始化
            common.Init(functi");
                WriteLiteral(@"on (iData) {
                //按钮初始化
                common.InitCommonds(common, $(common.Btns), iData.Commands);
            });

            //外键初始化
            common.ForeignInit(""OPerator"", function (fdata) {
                fcommon.initData = fdata;
                initGrid();
            });

            //初始化组织结构树
            initTree();
        });
    </script>
");
                EndContext();
            }
            );
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

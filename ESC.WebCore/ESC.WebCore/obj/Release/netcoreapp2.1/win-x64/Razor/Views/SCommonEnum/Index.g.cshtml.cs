#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SCommonEnum\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f697568aa05ce3a7ca34fe571060f4af56180225"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SCommonEnum_Index), @"mvc.1.0.view", @"/Views/SCommonEnum/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/SCommonEnum/Index.cshtml", typeof(AspNetCore.Views_SCommonEnum_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f697568aa05ce3a7ca34fe571060f4af56180225", @"/Views/SCommonEnum/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_SCommonEnum_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SCommonEnum\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(86, 967, true);
            WriteLiteral(@"<div id=""CommonEnumForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>枚举类型：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""EnumType"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>字段名称：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""EnumName"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>枚举值：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""EnumField"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <span id=""CommonEnumBtns"" class=""SearchBtns"">
            </span>
        </li>
    </ul>
</div>
<div id=""CommonEnumGrid"">
</div>
<div style=""height:0;visibility:hidden"">
    <iframe style=""height:0;"" id=""download""></iframe>
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1070, 2933, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""SCommonEnum"", SearchForm: ""#CommonEnumForm"", DataGrid: ""#CommonEnumGrid"" });
        var bGrid = null;

        //查询
        common.search = function () {
            var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
            common.Select(whereItems, 0, 0, function (result) {
                bGrid.setResult(result);
            });
        }

        //删除
        common.remove = function () {
            var deleteRow = bGrid.getSelected();
            common.Delete(deleteRow, function () {
                common.search();
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

");
                WriteLiteral(@"            form.show(900, 200);
        }

        //编辑
        common.edit = function () {
            var editRow = bGrid.getSelected();
            if (editRow == null) {
                CommonBUI.alert(""提示"", ""请选择编辑的记录."", ""warning"");
                return false;
            }
            var form = new BuiForm({ data: editRow, common: common, type: ""edit"" });

            form.save = function (data) {
                common.Update(data, function () {
                    common.search();
                    form.close();
                });
            }
            form.show(900, 200);
        }

        //生成实体
        common.createCls = function () {
            $(""#download"").attr(""src"", ""../SCommonEnum/GetEnumClass"");
        }

        //初始化grid
        function initGrid(iData) {
            bGrid = new BUIGrid({
                render: common.DataGrid,
                common: common
            });

            //分页查询
            bGrid.OnPageSearch = function (pageI");
                WriteLiteral(@"ndex, pageSize) {
                var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
                common.Select(whereItems, pageIndex, pageSize, function (result) {
                    bGrid.setResult(result);
                });
            }
            bGrid.init();
            common.search();
        }

        $(document).ready(function () {
            //初始化
            common.Init(function (iData) {
                //按钮初始化
                common.InitCommonds(common, $(""#CommonEnumBtns""), iData.Commands);
                //初始化grid
                initGrid(iData);
            });

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

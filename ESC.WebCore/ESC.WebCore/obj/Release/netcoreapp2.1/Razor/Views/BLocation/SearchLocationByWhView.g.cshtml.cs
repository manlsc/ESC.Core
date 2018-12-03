#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationByWhView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "86dc2e786f5cad22863ac177ea7fb65204a38a71"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BLocation_SearchLocationByWhView), @"mvc.1.0.view", @"/Views/BLocation/SearchLocationByWhView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/BLocation/SearchLocationByWhView.cshtml", typeof(AspNetCore.Views_BLocation_SearchLocationByWhView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"86dc2e786f5cad22863ac177ea7fb65204a38a71", @"/Views/BLocation/SearchLocationByWhView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_BLocation_SearchLocationByWhView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationByWhView.cshtml"
  
    ViewBag.Title = "SearchLocationByWhView";
    Layout = "~/Views/Shared/_LayoutSingle.cshtml";

#line default
#line hidden
            BeginContext(109, 471, true);
            WriteLiteral(@"<div id=""LocationForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>编码：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""LocationCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>名称：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""LocationDesc"" />
                <input type=""hidden""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 580, "\"", 610, 1);
#line 18 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationByWhView.cshtml"
WriteAttributeValue("", 588, ViewBag.TopLocationID, 588, 22, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(611, 398, true);
            WriteLiteral(@" class=""form-control"" name=""TopLocationID"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <span id=""LocationBtns"" class=""SearchBtns"">
                <button id=""btnSearch"" class=""btn btn-default"" style=""padding-right:5px;""><i class=""fa fa-search""></i>查询</button>
            </span>
        </li>
    </ul>
</div>
<div id=""LocationGrid"">
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1026, 2096, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""BLocation"", SearchForm: ""#LocationForm"", DataGrid: ""#LocationGrid"" });
        var bGrid = null;

        //查询
        common.search = function () {
            var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
            whereItems.push({
                field: ""LocationType"",
                condition: "">"",
                value: 1,
                datatype: ""int""
            });
            common.Select(whereItems, 0, 0, function (result) {
                bGrid.setResult(result);
            });
        }

        //初始化grid
        function initGrid(iData) {
            bGrid = new BUIGrid({
                render: common.DataGrid,
                common: common
            });
            //分页查询
            bGrid.OnPageSearch = function (pageIndex, pageSize) {
                var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
                whereItems.push");
                WriteLiteral(@"({
                    field: ""LocationType"",
                    condition: "">"",
                    value: 1,
                    datatype: ""int""
                });
                common.Select(whereItems, pageIndex, pageSize, function (result) {
                    bGrid.setResult(result);
                });
            }
            bGrid.init();
            common.search();
        }

        //获取返回值
        function getData() {
            return bGrid.getSelected();
        }

        $(document).ready(function () {
            //初始化
            common.Init(function (iData) {
                //初始化grid
                initGrid(iData);
            });

            //查询
            $(""#btnSearch"").click(function () {
                common.search();
            });

            //绑定回车查询
            $(common.SearchForm).find(""input:text"").keyup(function (e) {
                if (e.which == BUI.KeyCode.ENTER) {
                    common.search();
                }
   ");
                WriteLiteral("         });\r\n        });\r\n\r\n    </script>\r\n    ");
                EndContext();
            }
            );
            BeginContext(3125, 4, true);
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
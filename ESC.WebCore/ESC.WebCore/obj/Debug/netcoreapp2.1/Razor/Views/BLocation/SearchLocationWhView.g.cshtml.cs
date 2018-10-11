#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationWhView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ea00906d4acfcb3fc2b692da26026eb3120bcbcf"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BLocation_SearchLocationWhView), @"mvc.1.0.view", @"/Views/BLocation/SearchLocationWhView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/BLocation/SearchLocationWhView.cshtml", typeof(AspNetCore.Views_BLocation_SearchLocationWhView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ea00906d4acfcb3fc2b692da26026eb3120bcbcf", @"/Views/BLocation/SearchLocationWhView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_BLocation_SearchLocationWhView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationWhView.cshtml"
  
    ViewBag.Title = "SearchWH";
    Layout = "~/Views/Shared/_LayoutSingle.cshtml";

#line default
#line hidden
            BeginContext(95, 2392, true);
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
                <input type=""hidden"" value=""1"" class=""form-control"" name=""LocationType"" />
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
<script type=""text/javascript"">
    var common = new CommonBUI({ controller: ""BLocation"", SearchForm: ""#LocationForm"", DataGrid: ""#LocationGrid"" ");
            WriteLiteral(@"});
    var bGrid = null;

    //查询
    common.search = function () {
        var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
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
            initGrid(iData);");
            WriteLiteral(@"
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

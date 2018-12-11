#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseNotice\SearchDeataiView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d1f11ebaff6790470dd52071553fae61c2afc2c4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_WPurchaseNotice_SearchDeataiView), @"mvc.1.0.view", @"/Views/WPurchaseNotice/SearchDeataiView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/WPurchaseNotice/SearchDeataiView.cshtml", typeof(AspNetCore.Views_WPurchaseNotice_SearchDeataiView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d1f11ebaff6790470dd52071553fae61c2afc2c4", @"/Views/WPurchaseNotice/SearchDeataiView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_WPurchaseNotice_SearchDeataiView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseNotice\SearchDeataiView.cshtml"
  
    ViewData["Title"] = "SearchDeataiView";
    Layout = "~/Views/Shared/_LayoutSingle.cshtml";

#line default
#line hidden
            BeginContext(107, 276, true);
            WriteLiteral(@"<div id=""PurchaseNoticeLineForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>物料编码：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""MaterialCode"" />
                <input type=""hidden""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 383, "\"", 408, 1);
#line 12 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseNotice\SearchDeataiView.cshtml"
WriteAttributeValue("", 391, ViewBag.ParentID, 391, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(409, 603, true);
            WriteLiteral(@" class=""form-control"" name=""ParentID"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>批次：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""Batch"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <span id=""MaterialBtns"" class=""SearchBtns"">
                <button id=""btnSearch"" class=""btn btn-default"" style=""padding-right:5px;""><i class=""fa fa-search""></i>查询</button>
            </span>
        </li>
    </ul>
</div>
<div id=""PurchaseNoticeLineGrid"">
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1029, 1861, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""WPurchaseNotice"", SearchForm: ""#PurchaseNoticeLineForm"", DataGrid: ""#PurchaseNoticeLineGrid"" });
        var bGrid = null;

        //查询
        common.search = function () {
            var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
            common.CustomSelect(""SearchDetailSc"",whereItems, 0, 0, function (result) {
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
            bGrid.OnPageSearch = function (pageOutdex, pageSize) {
                var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
                common.CustomSelect(""SearchDetailSc"",whereItems, pageOutdex, pageSize, function (result) {
                    bGrid.setResult(result);
      ");
                WriteLiteral(@"          });
            }

            bGrid.init();
            common.search();
        }

        //获取返回值
        function getData() {
            return bGrid.getSelected();
        }

        $(document).ready(function () {
            //外键初始化
            common.ForeignInit(""WPurchaseNoticeLine"", function (fData) {
                common.initData = fData;
                initGrid(fData);
            });

            var datepicker = new BUI.Calendar.DatePicker({
                trigger: '.calendar',
                autoRender: true
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
            BeginContext(2893, 2, true);
            WriteLiteral("\r\n");
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

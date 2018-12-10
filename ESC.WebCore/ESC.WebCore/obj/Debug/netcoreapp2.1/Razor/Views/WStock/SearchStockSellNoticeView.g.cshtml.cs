#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WStock\SearchStockSellNoticeView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d40b7336ca54b93de46996f25612c05df7efca43"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_WStock_SearchStockSellNoticeView), @"mvc.1.0.view", @"/Views/WStock/SearchStockSellNoticeView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/WStock/SearchStockSellNoticeView.cshtml", typeof(AspNetCore.Views_WStock_SearchStockSellNoticeView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d40b7336ca54b93de46996f25612c05df7efca43", @"/Views/WStock/SearchStockSellNoticeView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_WStock_SearchStockSellNoticeView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WStock\SearchStockSellNoticeView.cshtml"
  
    ViewData["Title"] = "SearchStockSellNoticeView";
    Layout = "~/Views/Shared/_LayoutSingle.cshtml";

#line default
#line hidden
            BeginContext(116, 860, true);
            WriteLiteral(@"

<div id=""StockForm"">
    <ul class=""bui-search-ul"">        
        <li class=""bui-inline-block"">
            <label>货位编码：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""PositionCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>物料编码：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""MaterialCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>批次：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""Batch"" />
            </span>
        </li>       
        <li class=""bui-inline-block"">
            <span id=""StockBtns"" class=""SearchBtns"">
                <input type=""hidden"" class=""form-control"" name=""WarehouseID""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 976, "\"", 1004, 1);
#line 30 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WStock\SearchStockSellNoticeView.cshtml"
WriteAttributeValue("", 984, ViewBag.WarehouseID, 984, 20, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1005, 221, true);
            WriteLiteral(" />\r\n                <button id=\"btnSearch\" class=\"btn btn-default\" style=\"padding-right:5px;\"><i class=\"fa fa-search\"></i>查询</button>\r\n            </span>\r\n        </li>\r\n    </ul>\r\n</div>\r\n<div id=\"StockGrid\">\r\n</div>\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1243, 340, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""WStock"", SearchForm: ""#StockForm"", DataGrid: ""#StockGrid"" });
        var bGrid = null;

        //查询
        common.search = function () {
            var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
            var ParentID =");
                EndContext();
                BeginContext(1584, 16, false);
#line 46 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WStock\SearchStockSellNoticeView.cshtml"
                     Write(ViewBag.ParentID);

#line default
#line hidden
                EndContext();
                BeginContext(1600, 686, true);
                WriteLiteral(@";
            whereItems.push({ datatype: 'int', field: 'ParentID', condition: ""="", value: ParentID });
            common.CustomSelect(""SellNoticeSearch"",whereItems, 0, 0, function (result) {
                bGrid.setResult(result);
            });
        }

        //初始化grid
        function initGrid(iData) {
            bGrid = new BUIGrid({
                render: common.DataGrid,
                common: common,
                editable: false
            });

            //分页查询
            bGrid.OnPageSearch = function (pageIndex, pageSize) {
                var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
                var noticeId =");
                EndContext();
                BeginContext(2287, 16, false);
#line 64 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WStock\SearchStockSellNoticeView.cshtml"
                         Write(ViewBag.NoticeID);

#line default
#line hidden
                EndContext();
                BeginContext(2303, 1896, true);
                WriteLiteral(@";
                whereItems.push({ datatype: 'int', field: 'NoticeID', condition: ""="", value: noticeId });
                common.CustomSelect(""SellNoticeSearch"",whereItems, 0, 0, function (result) {
                    bGrid.setResult(result);
                });             
            }
            bGrid.init();
            common.search();
        }

        //初始化列显示隐藏
        function initStockColumns(iData) {
            for (var i = 0; i < iData.Columns.length; i++) {
                switch (iData.Columns[i].dataIndex) {
                    case ""TransitCount"":
                    case ""FrozenCount"":
                    case ""TakeCount"":
                    case ""StockCount"":
                    case ""StockInDate"":
                        iData.Columns[i].visible = false;
                        break;
                    case ""OutCount"":
                        iData.Columns[i].visible = true;
                        break;
                }
            }
        }

     ");
                WriteLiteral(@"   //获取返回值
        function getData() {
            return bGrid.getSelected();
        }

        $(document).ready(function () {
            //初始化
            common.Init(function (iData) {
                //按钮初始化
                initStockColumns(iData);
                //初始化grid
                initGrid(iData);
            });

            //查询
            $(""#btnSearch"").click(function () {
                common.search();
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
            BeginContext(4202, 2, true);
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
#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "94f893ca38095ca33920ed4204e28affc4a3d34b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_WOtherOut_Index), @"mvc.1.0.view", @"/Views/WOtherOut/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/WOtherOut/Index.cshtml", typeof(AspNetCore.Views_WOtherOut_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"94f893ca38095ca33920ed4204e28affc4a3d34b", @"/Views/WOtherOut/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_WOtherOut_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ESC.Infrastructure.ComboxData>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("selected", new global::Microsoft.AspNetCore.Html.HtmlString("selected"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "3", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "4", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(124, 445, true);
            WriteLiteral(@"
<div id=""OtherOutForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>出库单号：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""OtherOutCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>状态：</label>
            <span>
                <select class=""form-control"" name=""StockStatus"">
                    ");
            EndContext();
            BeginContext(569, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fe1c9dbd175a4643831f1e796f729711", async() => {
                BeginContext(606, 2, true);
                WriteLiteral("全部");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(617, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 21 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml"
                     for (int i = 0; i < Model.items.Count; i++)
                    {

#line default
#line hidden
            BeginContext(708, 24, true);
            WriteLiteral("                        ");
            EndContext();
            BeginContext(732, 67, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b81de437e2d7425caa67769a1d96a9de", async() => {
                BeginContext(771, 19, false);
#line 23 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml"
                                                         Write(Model.items[i].text);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 23 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml"
                           WriteLiteral(Model.items[i].value);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(799, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 24 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml"
                    }

#line default
#line hidden
            BeginContext(824, 243, true);
            WriteLiteral("                </select>\r\n            </span>\r\n        </li>\r\n        <li class=\"bui-inline-block\">\r\n            <label>出库类型：</label>\r\n            <span>\r\n                <select class=\"form-control\" name=\"StockOutType\">\r\n                    ");
            EndContext();
            BeginContext(1067, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "763905f9094a45398478073558980c1e", async() => {
                BeginContext(1104, 2, true);
                WriteLiteral("全部");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1115, 22, true);
            WriteLiteral("\r\n                    ");
            EndContext();
            BeginContext(1137, 31, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4aa1825cb7494d05b5e2bda522f0c740", async() => {
                BeginContext(1155, 4, true);
                WriteLiteral("其他出库");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1168, 22, true);
            WriteLiteral("\r\n                    ");
            EndContext();
            BeginContext(1190, 31, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "45a8623f05cd439195c3a371c4a68694", async() => {
                BeginContext(1208, 4, true);
                WriteLiteral("盘盈出库");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1221, 631, true);
            WriteLiteral(@"
                </select>
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>创建时间：</label>
            <span>
                <input class=""form-control calendar"" type=""text"" name=""CreateDate_begin"" />
            </span>
            <span>-</span>
            <span>
                <input class=""form-control calendar"" type=""text"" name=""CreateDate_end"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <span id=""OtherOutBtns"" class=""SearchBtns"">
            </span>
        </li>
    </ul>
</div>
<div id=""OtherOutGrid"">
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1869, 3682, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""WOtherOut"", SearchForm: ""#OtherOutForm"", DataGrid: ""#OtherOutGrid"" });
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
            if (deleteRow) {
                common.Delete(deleteRow, function () {
                    common.search();
                });
            }
        }

        //添加
        common.add = function () {
            window.location.href = ""../WOtherOut/AddView"";
        }

        //编辑
        common.edit = function () {
            var deleteRow = bGrid.getSelected();
            if (deleteRow) {
                window.location.hr");
                WriteLiteral(@"ef = ""../WOtherOut/UpdateView?ID="" + deleteRow.ID + ""&StockStatus="" + deleteRow.StockStatus;
            }
        }

        //审核
        common.approve = function () {
            var editRow = bGrid.getSelected();
            if (editRow == null) {
                CommonBUI.alert(""提示"", ""请选择审核的记录."", ""warning"");
                return false;
            }
            var data = ""app="" + CommonBUI.UrlEncode(JSON.stringify(editRow));
            common.post(common.controller, ""Approve"", data, function (rdata) {
                if (rdata.status == 0) {
                    CommonBUI.alert(""提示"", ""审核成功."", ""success"");
                    common.search();
                } else {
                    CommonBUI.alert(""提示"", rdata.message);
                }
            });
        }

        //初始化grid
        function initGrid(iData) {
            bGrid = new BUIGrid({
                render: common.DataGrid,
                common: common
            });

            //分页查询
            bG");
                WriteLiteral(@"rid.OnPageSearch = function (pageOutdex, pageSize) {
                var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
                common.Select(whereItems, pageOutdex, pageSize, function (result) {
                    bGrid.setResult(result);
                });
            }

            //初始化
            bGrid.onInit = function (dftGrid) {
                for (var i = 0; i < dftGrid.columns.length; i++) {
                    var col = dftGrid.columns[i];
                    if (col.dataIndex == ""OtherOutCode"") {
                        col.renderer = function (value, record, index) {
                            return '<a href=""../WOtherOut/UpdateView?ID=' + record.ID + ""&StockStatus="" + record.StockStatus + '"">' + value + '<a>';
                        }
                    }
                }
            }

            bGrid.init();
            common.search();
        }

        $(document).ready(function () {
            //初始化
            common.Init(functi");
                WriteLiteral(@"on (iData) {
                //按钮初始化
                common.InitCommonds(common, $(""#OtherOutBtns""), iData.Commands);
                //初始化grid
                initGrid(iData);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ESC.Infrastructure.ComboxData> Html { get; private set; }
    }
}
#pragma warning restore 1591

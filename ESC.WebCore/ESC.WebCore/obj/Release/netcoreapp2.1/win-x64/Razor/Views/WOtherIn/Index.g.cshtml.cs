#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherIn\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "84fcfed591de069503c47961f2e533ee9c6759a0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_WOtherIn_Index), @"mvc.1.0.view", @"/Views/WOtherIn/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/WOtherIn/Index.cshtml", typeof(AspNetCore.Views_WOtherIn_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"84fcfed591de069503c47961f2e533ee9c6759a0", @"/Views/WOtherIn/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_WOtherIn_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ESC.Infrastructure.ComboxData>
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
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherIn\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(124, 443, true);
            WriteLiteral(@"
<div id=""OtherInForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>入库单号：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""OtherInCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>状态：</label>
            <span>
                <select class=""form-control"" name=""StockStatus"">
                    ");
            EndContext();
            BeginContext(567, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e01c43a8f56f489fbf9bc2b134b6418b", async() => {
                BeginContext(604, 2, true);
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
            BeginContext(615, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 21 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherIn\Index.cshtml"
                     for (int i = 0; i < Model.items.Count; i++)
                    {

#line default
#line hidden
            BeginContext(706, 24, true);
            WriteLiteral("                        ");
            EndContext();
            BeginContext(730, 67, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "da315fd4b19b4e4e8b16987a8b5c1bc9", async() => {
                BeginContext(769, 19, false);
#line 23 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherIn\Index.cshtml"
                                                         Write(Model.items[i].text);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 23 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherIn\Index.cshtml"
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
            BeginContext(797, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 24 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherIn\Index.cshtml"
                    }

#line default
#line hidden
            BeginContext(822, 242, true);
            WriteLiteral("                </select>\r\n            </span>\r\n        </li>\r\n        <li class=\"bui-inline-block\">\r\n            <label>入库类型：</label>\r\n            <span>\r\n                <select class=\"form-control\" name=\"StockInType\">\r\n                    ");
            EndContext();
            BeginContext(1064, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6a07c618fdad4db99ecfdf7a6b31fa93", async() => {
                BeginContext(1101, 2, true);
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
            BeginContext(1112, 22, true);
            WriteLiteral("\r\n                    ");
            EndContext();
            BeginContext(1134, 31, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9c3e572426674f5ca6934791b916b66b", async() => {
                BeginContext(1152, 4, true);
                WriteLiteral("其他入库");
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
            BeginContext(1165, 22, true);
            WriteLiteral("\r\n                    ");
            EndContext();
            BeginContext(1187, 31, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a17e49bbbc924f5e8fbde1bdddb54b01", async() => {
                BeginContext(1205, 4, true);
                WriteLiteral("盘盈入库");
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
            BeginContext(1218, 629, true);
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
            <span id=""OtherInBtns"" class=""SearchBtns"">
            </span>
        </li>
    </ul>
</div>
<div id=""OtherInGrid"">
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1864, 3671, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""WOtherIn"", SearchForm: ""#OtherInForm"", DataGrid: ""#OtherInGrid"" });
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
            window.location.href = ""../WOtherIn/AddView"";
        }

        //编辑
        common.edit = function () {
            var deleteRow = bGrid.getSelected();
            if (deleteRow) {
                window.location.href =");
                WriteLiteral(@" ""../WOtherIn/UpdateView?ID="" + deleteRow.ID + ""&StockStatus="" + deleteRow.StockStatus;
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
            bGrid.O");
                WriteLiteral(@"nPageSearch = function (pageIndex, pageSize) {
                var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
                common.Select(whereItems, pageIndex, pageSize, function (result) {
                    bGrid.setResult(result);
                });
            };

            //初始化
            bGrid.onInit = function (dftGrid) {
                for (var i = 0; i < dftGrid.columns.length; i++) {
                    var col = dftGrid.columns[i];
                    if (col.dataIndex == ""OtherInCode"") {
                        col.renderer = function (value, record, index) {
                            return '<a href=""../WOtherIn/UpdateView?ID=' + record.ID + ""&StockStatus="" + record.StockStatus + '"">' + value + '<a>';
                        }
                    }
                }
            }
            bGrid.init();
            common.search();
        }

        $(document).ready(function () {
            //初始化
            common.Init(function (iData)");
                WriteLiteral(@" {
                //按钮初始化
                common.InitCommonds(common, $(""#OtherInBtns""), iData.Commands);
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

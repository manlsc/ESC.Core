#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WSell\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "247193c39fd9d6767398656492e79934656ccaf6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_WSell_Index), @"mvc.1.0.view", @"/Views/WSell/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/WSell/Index.cshtml", typeof(AspNetCore.Views_WSell_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"247193c39fd9d6767398656492e79934656ccaf6", @"/Views/WSell/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_WSell_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ESC.Infrastructure.ComboxData>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("selected", new global::Microsoft.AspNetCore.Html.HtmlString("selected"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WSell\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(124, 441, true);
            WriteLiteral(@"
<div id=""SellForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>出库单号：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""InNoticeCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>状态：</label>
            <span>
                <select class=""form-control"" name=""StockStatus"">
                    ");
            EndContext();
            BeginContext(565, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b91f60a073b94f8f992127f261823393", async() => {
                BeginContext(602, 2, true);
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
            BeginContext(613, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 21 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WSell\Index.cshtml"
                     for (int i = 0; i < Model.items.Count; i++)
                    {

#line default
#line hidden
            BeginContext(704, 24, true);
            WriteLiteral("                        ");
            EndContext();
            BeginContext(728, 67, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fbf5a2d9f81f40fa86eae671b069bd4b", async() => {
                BeginContext(767, 19, false);
#line 23 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WSell\Index.cshtml"
                                                         Write(Model.items[i].text);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 23 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WSell\Index.cshtml"
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
            BeginContext(795, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 24 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WSell\Index.cshtml"
                    }

#line default
#line hidden
            BeginContext(820, 621, true);
            WriteLiteral(@"                </select>
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
            <span id=""SellBtns"" class=""SearchBtns"">
            </span>
        </li>
    </ul>
</div>
<div id=""SellGrid"">
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1458, 4727, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""WSell"", SearchForm: ""#SellForm"", DataGrid: ""#SellGrid"" });
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
            window.location.href = ""../WSell/AddView"";
        }

        //编辑
        common.edit = function () {
            var deleteRow = bGrid.getSelected();
            if (deleteRow) {
                window.location.href = ""../WSell/U");
                WriteLiteral(@"pdateView?ID="" + deleteRow.ID + ""&StockStatus="" + deleteRow.StockStatus;
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

        //下推
        common.pushdown = function () {
            var editRow = bGrid.getSelected();
            if (editRow == null) {
                CommonBUI.alert(""提示"", ""请选择通知单."", ""warning"");
                return false;
     ");
                WriteLiteral(@"       }
            var data = ""app="" + CommonBUI.UrlEncode(JSON.stringify(editRow));
            common.post(common.controller, ""PushDown"", data, function (rdata) {
                if (rdata.status == 0) {
                    window.location.href = ""../WSellReturn/UpdateView?ID="" + rdata.result;
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
            bGrid.OnPageSearch = function (pageOutdex, pageSize) {
                var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
                common.Select(whereItems, pageOutdex, pageSize, function (result) {
                    bGrid.setResult(result);
                });
            }

            //初始化
            bGrid.onInit = function (dftGri");
                WriteLiteral(@"d) {
                for (var i = 0; i < dftGrid.columns.length; i++) {
                    var col = dftGrid.columns[i];
                    if (col.dataIndex == ""SellCode"") {
                        col.renderer = function (value, record, index) {
                            return '<a href=""../WSell/UpdateView?ID=' + record.ID + ""&StockStatus="" + record.StockStatus + '"">' + value + '<a>';
                        }
                    } else if (col.dataIndex == ""SourceCode"") {
                        col.renderer = function (value, record, index) {
                            if (value) {
                                return '<a href=""../WSellNotice/UpdateView?ID=' + record.SourceID + '"">' + value + '<a>';
                            }
                            return """";
                        }
                    }
                }
            }

            bGrid.init();
            common.search();
        }

        $(document).ready(function () {
            //初始化
     ");
                WriteLiteral(@"       common.Init(function (iData) {
                //按钮初始化
                common.InitCommonds(common, $(""#SellBtns""), iData.Commands);
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

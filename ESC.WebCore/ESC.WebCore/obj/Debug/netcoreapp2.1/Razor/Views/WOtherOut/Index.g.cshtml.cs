#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5001d579e527fec35ad2de5194638de27af40ca0"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5001d579e527fec35ad2de5194638de27af40ca0", @"/Views/WOtherOut/Index.cshtml")]
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
            BeginContext(125, 445, true);
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
            BeginContext(570, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "980b2d3c2510407da0f1bceecfa823c7", async() => {
                BeginContext(607, 2, true);
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
            BeginContext(618, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 21 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml"
                     for (int i = 0; i < Model.items.Count; i++)
                    {

#line default
#line hidden
            BeginContext(709, 24, true);
            WriteLiteral("                        ");
            EndContext();
            BeginContext(733, 67, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "66570fc3317f41c8bed843409da30b83", async() => {
                BeginContext(772, 19, false);
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
            BeginContext(800, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 24 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WOtherOut\Index.cshtml"
                    }

#line default
#line hidden
            BeginContext(825, 243, true);
            WriteLiteral("                </select>\r\n            </span>\r\n        </li>\r\n        <li class=\"bui-inline-block\">\r\n            <label>出库类型：</label>\r\n            <span>\r\n                <select class=\"form-control\" name=\"StockOutType\">\r\n                    ");
            EndContext();
            BeginContext(1068, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "38d17ded179f464c8006bc51fa960cd6", async() => {
                BeginContext(1105, 2, true);
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
            BeginContext(1116, 22, true);
            WriteLiteral("\r\n                    ");
            EndContext();
            BeginContext(1138, 31, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "8bdfb60062204cbca4c7309e8440cee3", async() => {
                BeginContext(1156, 4, true);
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
            BeginContext(1169, 22, true);
            WriteLiteral("\r\n                    ");
            EndContext();
            BeginContext(1191, 31, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "84e8d6d28a4347e18c8dd2ca369ad705", async() => {
                BeginContext(1209, 4, true);
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
            BeginContext(1222, 3925, true);
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
<script type=""text/javascript"">
    var common = new CommonBUI({ controller: ""WOtherOut"", SearchForm: ""#OtherOutForm"", DataGrid: ""#OtherOutGrid"" });
    var bGrid = null;

    //查询
    common.search = function () {
        var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
        common.Select(whereItems, 0, 0, function (result) {
            bGrid.setResult(result)");
            WriteLiteral(@";
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
            window.location.href = ""../WOtherOut/UpdateView?ID="" + deleteRow.ID + ""&StockStatus="" + deleteRow.StockStatus;
        }
    }

    //审核
    common.approve = function () {
        var editRow = bGrid.getSelected();
        if (editRow == null) {
            CommonBUI.alert(""提示"", ""请选择审核的记录."", ""warning"");
            return false;
        }
        var data=""app=""+CommonBUI.UrlEncode(JSON.stringify(editRow));
        common.post(common.controller, ""Approve"", data, function (rdata) {
            if (rdata.stat");
            WriteLiteral(@"us == 0) {
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
        bGrid.OnPageSearch = function (pageOutdex, pageSize) {
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
                        return '<a ");
            WriteLiteral(@"href=""../WOtherOut/UpdateView?ID=' + record.ID + ""&StockStatus="" + record.StockStatus + '"">' + value + '<a>';
                    }
                }
            }
        }

        bGrid.init();
        common.search();
    }

    $(document).ready(function () {
        //初始化
        common.Init(function (iData) {
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

</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ESC.Infrastructure.ComboxData> Html { get; private set; }
    }
}
#pragma warning restore 1591

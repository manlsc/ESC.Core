#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationGridView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "88c4d17a4d2e44f9a70296212aae371318b6c5be"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BLocation_SearchLocationGridView), @"mvc.1.0.view", @"/Views/BLocation/SearchLocationGridView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/BLocation/SearchLocationGridView.cshtml", typeof(AspNetCore.Views_BLocation_SearchLocationGridView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88c4d17a4d2e44f9a70296212aae371318b6c5be", @"/Views/BLocation/SearchLocationGridView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_BLocation_SearchLocationGridView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ESC.Infrastructure.ComboxDataItem>>
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
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationGridView.cshtml"
  
    ViewBag.Title = "SearchLocationGrid";
    Layout = "~/Views/Shared/_LayoutSingle.cshtml";

#line default
#line hidden
            BeginContext(151, 651, true);
            WriteLiteral(@"
<div id=""LocationForm"">
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
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>类型：</label>
            <span>
                <select class=""form-control"" name=""LocationType"">
                    ");
            EndContext();
            BeginContext(802, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9aa7943562ed4a64a45bb2fd1c7e6500", async() => {
                BeginContext(839, 2, true);
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
            BeginContext(850, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 26 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationGridView.cshtml"
                     foreach (var item in Model)
                    {

#line default
#line hidden
            BeginContext(925, 24, true);
            WriteLiteral("                        ");
            EndContext();
            BeginContext(949, 47, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fd6470f7e08843a48b1511d512eaf1d5", async() => {
                BeginContext(978, 9, false);
#line 28 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationGridView.cshtml"
                                               Write(item.text);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 28 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationGridView.cshtml"
                           WriteLiteral(item.value);

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
            BeginContext(996, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 29 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BLocation\SearchLocationGridView.cshtml"
                    }

#line default
#line hidden
            BeginContext(1021, 378, true);
            WriteLiteral(@"                </select>
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
                BeginContext(1416, 1710, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""BLocation"", SearchForm: ""#LocationForm"", DataGrid: ""#LocationGrid"" });
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
            common.");
                WriteLiteral(@"search();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ESC.Infrastructure.ComboxDataItem>> Html { get; private set; }
    }
}
#pragma warning restore 1591

#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BUnit\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7df08240ea0c33bac9fd3f3cc078191d02867c8f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BUnit_Index), @"mvc.1.0.view", @"/Views/BUnit/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/BUnit/Index.cshtml", typeof(AspNetCore.Views_BUnit_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7df08240ea0c33bac9fd3f3cc078191d02867c8f", @"/Views/BUnit/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_BUnit_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ESC.Infrastructure.ComboxDataItem>>
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
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BUnit\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(132, 635, true);
            WriteLiteral(@"
<div id=""UnitForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>编码：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""UnitCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>名称：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""UnitName"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>类型：</label>
            <span>
                <select class=""form-control"" name=""UnitType"">
                    ");
            EndContext();
            BeginContext(767, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "66c07dae11e944d38bf0b6ef778f8525", async() => {
                BeginContext(804, 2, true);
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
            BeginContext(815, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 26 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BUnit\Index.cshtml"
                     foreach (var item in Model)
                    {

#line default
#line hidden
            BeginContext(890, 24, true);
            WriteLiteral("                        ");
            EndContext();
            BeginContext(914, 47, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "881e105f208841a1bad967a1990ce42a", async() => {
                BeginContext(943, 9, false);
#line 28 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BUnit\Index.cshtml"
                                               Write(item.text);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 28 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BUnit\Index.cshtml"
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
            BeginContext(961, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 29 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BUnit\Index.cshtml"
                    }

#line default
#line hidden
            BeginContext(986, 239, true);
            WriteLiteral("                </select>\r\n            </span>\r\n        </li>\r\n        <li class=\"bui-inline-block\">\r\n            <span id=\"UnitBtns\" class=\"SearchBtns\">\r\n            </span>\r\n        </li>\r\n    </ul>\r\n</div>\r\n<div id=\"UnitGrid\">\r\n</div>\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1242, 3903, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""BUnit"", SearchForm: ""#UnitForm"", DataGrid: ""#UnitGrid"" });
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

            //外键查询
");
                WriteLiteral(@"            form.fSearch = function (opts) {
                searchOrg(form, opts);
            }

            form.show(900, 300);
        }

        //编辑
        common.edit = function () {
            var editRow = bGrid.getSelected();
            if (editRow == null) {
                CommonBUI.alert(""提示"", ""请选择编辑的记录."", ""warning"");
                return false;
            }
            var form = new BuiForm({ data: editRow, common: common, type: ""edit"" });

            //外键查询
            form.fSearch = function (opts) {
                searchOrg(form, opts);
            }

            form.save = function (data) {
                common.Update(data, function () {
                    common.search();
                    form.close();
                });
            }
            form.show(900, 300);
        }

        //选择组织机构
        function searchOrg(frm, opts) {
            var dialog = new BUI.Overlay.Dialog({
                title: ""选择组织"",
                width: 300");
                WriteLiteral(@",
                height: 500,
                mask: true,
                closeAction: ""destroy"",
                bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../SOrganization/SearchOrgView"" style=""width:100%;height:100%;""></iframe>',
                success: function () {
                    var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                    if (data) {
                        frm.setReturnValue(opts, data);
                        dialog.close();
                    } else {
                        CommonBUI.alert(""提示"", ""请选择组织机构."");
                    }
                }
            });
            dialog.show();
        }

        //初始化grid
        function initGrid(iData) {
            bGrid = new BUIGrid({
                render: common.DataGrid,
                common: common
            });

            //分页查询
            bGrid.OnPageSearch = function (pageIndex, pageSize) {
                var whereItems = ");
                WriteLiteral(@"BuiForm.GetWhereItems(common.SearchForm, common);
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
                common.InitCommonds(common, $(""#UnitBtns""), iData.Commands);
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
            BeginContext(5148, 2, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ESC.Infrastructure.ComboxDataItem>> Html { get; private set; }
    }
}
#pragma warning restore 1591

#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\AddView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7fa3a5cd1f020cefef6815c4270ec8a1d6c98c65"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SResource_AddView), @"mvc.1.0.view", @"/Views/SResource/AddView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/SResource/AddView.cshtml", typeof(AspNetCore.Views_SResource_AddView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7fa3a5cd1f020cefef6815c4270ec8a1d6c98c65", @"/Views/SResource/AddView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_SResource_AddView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ESC.Infrastructure.DomainObjects.SResource>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "0", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 1 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\AddView.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_LayoutSingle.cshtml";

#line default
#line hidden
            BeginContext(148, 1690, true);
            WriteLiteral(@"
<div id=""resource-layout"" class=""bui-view-port bui-border-layout"">
    <div class=""bui-layout-border"">
        <div class=""bui-border-top"">
            <div id=""resource-layout-north"" class=""bui-border-north bui-layout-item-border"">
                <table id=""ResourceForm"" style=""border-collapse:separate;"" width=""100%"">
                    <tr>
                        <td align=""right"" style=""padding:5px;"">
                            名称
                        </td>
                        <td style=""padding:5px;"">
                            <input class=""form-control"" type=""text"" name=""ResourceName"" style=""width: 180px;"" />
                        </td>
                        <td align=""right"" style=""padding:5px;"">
                            资源路径
                        </td>
                        <td style=""padding:5px;"">
                            <input class=""form-control"" type=""text"" name=""ResourceURL"" style=""width: 180px;"" />
                        </td>
                    ");
            WriteLiteral(@"    <td align=""right"" style=""padding:5px;"">
                            描述
                        </td>
                        <td style=""padding:5px;"">
                            <input class=""form-control"" type=""text"" name=""ResourceDesc"" style=""width: 180px;"" />
                        </td>
                    </tr>
                    <tr>
                        <td align=""right"" style=""padding:5px;"">
                            父资源
                        </td>
                        <td style=""padding:5px;"">
                            <select class=""form-control"" name=""ParentID"" style=""width: 180px;"">
                                ");
            EndContext();
            BeginContext(1838, 30, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b30d44ada8a48f0871f0254c3c13758", async() => {
                BeginContext(1856, 3, true);
                WriteLiteral("请选择");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1868, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 39 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\AddView.cshtml"
                                 for (int i = 0; i < Model.Count; i++)
                                {

#line default
#line hidden
            BeginContext(1977, 36, true);
            WriteLiteral("                                    ");
            EndContext();
            BeginContext(2013, 60, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b4d9e90ccd474fd896a855531d46e23e", async() => {
                BeginContext(2043, 21, false);
#line 41 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\AddView.cshtml"
                                                            Write(Model[i].ResourceDesc);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 41 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\AddView.cshtml"
                                       WriteLiteral(Model[i].ID);

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
            BeginContext(2073, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 42 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\AddView.cshtml"
                                }

#line default
#line hidden
            BeginContext(2110, 3023, true);
            WriteLiteral(@"                            </select>

                        </td>
                        <td align=""right"" style=""padding:5px;"">
                            排序
                        </td>
                        <td style=""padding:5px;"">
                            <input class=""form-control"" type=""text"" name=""OrderIndex"" style=""width: 180px;"" />
                        </td>
                        <td colspan=""2"">
                            <input name=""ID"" type=""hidden"" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class=""bui-border-middle"">
            <div id=""resource-layout-center"" style=""overflow-y:auto;"" class=""bui-border-center bui-layout-item-border"">
                <div id=""OperatorGrid"">
                </div>
            </div>
        </div>
    </div>
</div>

<script type=""text/javascript"">
    var common = new CommonBUI({ controller: ""SResource"", DataGrid: ""#OperatorGrid"" });");
            WriteLiteral(@"
    var oGrid;

    //初始化布局
    function initLayout() {
        var layoutControl = new BUI.Layout.BuiLayout({
            srcNode: ""#resource-layout"",
            children: [{
                srcNode: ""#resource-layout-north"",
                xclass: 'controller',
                region: 'north',
                height: 90
            }, {
                srcNode: ""#resource-layout-center"",
                xclass: 'controller',
                region: 'center'
            }]
        });
        layoutControl.render();
    }

    //初始化grid
    function initGrid(initData) {
        oGrid = new BUIGrid({
            render: common.DataGrid,
            common: common,
            pager: false,
            editable: true
        });
        oGrid.init();
    }

    function getData() {
        var obj = BuiForm.GetFormControl(""#ResourceForm"");
        if (BUI.isNullOrEmpty(obj.ResourceName)) {
            CommonBUI.alert(""提示"", ""请输入名称."");
            return false;
        }
");
            WriteLiteral(@"        if (BUI.isNullOrEmpty(obj.ResourceURL)) {
            CommonBUI.alert(""提示"", ""请输入资源路径."");
            return false;
        }
        if (BUI.isNullOrEmpty(obj.ResourceDesc)) {
            CommonBUI.alert(""提示"", ""请输入描述."");
            return false;
        }
        if (BUI.isNullOrEmpty(obj.OrderIndex)) {
            CommonBUI.alert(""提示"", ""请输入排序."");
            return false;
        }
        if (!BUI.isNumeric(obj.OrderIndex)) {
            CommonBUI.alert(""提示"", ""请输入正确的排序."");
            return false;
        }
        var oprs = oGrid.getResult();
        if (oprs) {
            obj.Operators = oprs;
        } else {
            return false;
        }
        return obj;
    }

    $(document).ready(function () {
        //初始化布局
        initLayout();

        //外键初始化
        common.ForeignInit(""OPerator"", function (fdata) {
            common.initData = fdata;
            initGrid();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ESC.Infrastructure.DomainObjects.SResource>> Html { get; private set; }
    }
}
#pragma warning restore 1591

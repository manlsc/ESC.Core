#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0dcc9bb8874b1708a5901188d563904ef0d0a2f8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SResource_UpdateView), @"mvc.1.0.view", @"/Views/SResource/UpdateView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/SResource/UpdateView.cshtml", typeof(AspNetCore.Views_SResource_UpdateView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0dcc9bb8874b1708a5901188d563904ef0d0a2f8", @"/Views/SResource/UpdateView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_SResource_UpdateView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ESC.Infrastructure.DomainObjects.SResource>>
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
            BeginContext(57, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
  
    ViewBag.Title = "UpdateView";
    Layout = "~/Views/Shared/_LayoutSingle.cshtml";
    ESC.Infrastructure.DomainObjects.SResource Res = ViewBag.Resource;

#line default
#line hidden
            BeginContext(226, 601, true);
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
                            <input class=""form-control"" type=""text""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 827, "\"", 852, 1);
#line 19 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
WriteAttributeValue("", 835, Res.ResourceName, 835, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(853, 326, true);
            WriteLiteral(@" name=""ResourceName"" style=""width: 180px;"" />
                        </td>
                        <td align=""right"" style=""padding:5px;"">
                            资源路径
                        </td>
                        <td style=""padding:5px;"">
                            <input class=""form-control"" type=""text""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1179, "\"", 1203, 1);
#line 25 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
WriteAttributeValue("", 1187, Res.ResourceURL, 1187, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1204, 323, true);
            WriteLiteral(@" name=""ResourceURL"" style=""width: 180px;"" />
                        </td>
                        <td align=""right"" style=""padding:5px;"">
                            描述
                        </td>
                        <td style=""padding:5px;"">
                            <input class=""form-control"" type=""text""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1527, "\"", 1552, 1);
#line 31 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
WriteAttributeValue("", 1535, Res.ResourceDesc, 1535, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1553, 440, true);
            WriteLiteral(@" name=""ResourceDesc"" style=""width: 180px;"" />
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
            BeginContext(1993, 30, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4849730bb8274b6b9efcdc56086aee45", async() => {
                BeginContext(2011, 3, true);
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
            BeginContext(2023, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 41 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
                                 for (int i = 0; i < Model.Count; i++)
                                {
                                    if (Model[i].ID == Res.ParentID)
                                    {

#line default
#line hidden
            BeginContext(2241, 40, true);
            WriteLiteral("                                        ");
            EndContext();
            BeginContext(2281, 69, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "74fe41b7a36447b6a75c6c32d637063c", async() => {
                BeginContext(2320, 21, false);
#line 45 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
                                                                         Write(Model[i].ResourceDesc);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            BeginWriteTagHelperAttribute();
#line 45 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
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
            BeginContext(2350, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 46 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
                                    }
                                    else
                                    {

#line default
#line hidden
            BeginContext(2472, 40, true);
            WriteLiteral("                                        ");
            EndContext();
            BeginContext(2512, 60, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fbc1f37ef9dd47fba59b9a1a2e1a9748", async() => {
                BeginContext(2542, 21, false);
#line 49 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
                                                                Write(Model[i].ResourceDesc);

#line default
#line hidden
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#line 49 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
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
            BeginContext(2572, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 50 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
                                    }
                                }

#line default
#line hidden
            BeginContext(2648, 318, true);
            WriteLiteral(@"                            </select>

                        </td>
                        <td align=""right"" style=""padding:5px;"">
                            排序
                        </td>
                        <td style=""padding:5px;"">
                            <input class=""form-control"" type=""text""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 2966, "\"", 2989, 1);
#line 59 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
WriteAttributeValue("", 2974, Res.OrderIndex, 2974, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2990, 199, true);
            WriteLiteral(" name=\"OrderIndex\" style=\"width: 180px;\" />\r\n                        </td>\r\n                        <td colspan=\"2\">\r\n                            <input class=\"form-control\" id=\"ResourceID\" name=\"ID\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 3189, "\"", 3204, 1);
#line 62 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SResource\UpdateView.cshtml"
WriteAttributeValue("", 3197, Res.ID, 3197, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3205, 422, true);
            WriteLiteral(@" type=""hidden"" />
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
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(3644, 2669, true);
                WriteLiteral(@"
    <script>
        var common = new CommonBUI({ controller: ""SResource"", DataGrid: ""#OperatorGrid"" });
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
        function initGrid() {
            oGrid = new BUIGrid({
                render: common.DataGrid,
                common: common,
                pager: false,
                editable: true
            });
            oGrid.init();

            var resId ");
                WriteLiteral(@"= $(""#ResourceID"").val();
            loadOperator(resId);
        }

        //加载按钮
        function loadOperator(resId) {
            common.SelectDetail(""SOperator"", resId, 1, 20, function (data) {
                oGrid.setResult(data);
            });
        }

        function getData() {
            var obj = BuiForm.GetFormControl(""#ResourceForm"");
            if (BUI.isNullOrEmpty(obj.ResourceName)) {
                CommonBUI.alert(""提示"", ""请输入名称."");
                return false;
            }
            if (BUI.isNullOrEmpty(obj.ResourceURL)) {
                CommonBUI.alert(""提示"", ""请输入资源路径."");
                return false;;
            }
            if (BUI.isNullOrEmpty(obj.ResourceDesc)) {
                CommonBUI.alert(""提示"", ""请输入描述."");
                return false;
            }
            if (BUI.isNullOrEmpty(obj.OrderIndex)) {
                CommonBUI.alert(""提示"", ""请输入排序."");
                return false;
            }
            if (!BUI.isNumeric(obj.OrderInde");
                WriteLiteral(@"x)) {
                CommonBUI.alert(""提示"", ""请输入正确的排序."");
                return false;
            }
            var oprs = oGrid.getDirtyData();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ESC.Infrastructure.DomainObjects.SResource>> Html { get; private set; }
    }
}
#pragma warning restore 1591

#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SUser\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ccc9d33753946b64dd1a77fada487e829c15d3a1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SUser_Index), @"mvc.1.0.view", @"/Views/SUser/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/SUser/Index.cshtml", typeof(AspNetCore.Views_SUser_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ccc9d33753946b64dd1a77fada487e829c15d3a1", @"/Views/SUser/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_SUser_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("selected", new global::Microsoft.AspNetCore.Html.HtmlString("selected"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "1", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "0", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 1 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SUser\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(84, 637, true);
            WriteLiteral(@"<div id=""UserForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>编码：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""UserCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>名称：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""UserName"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>是否管理员：</label>
            <span>
                <select class=""form-control"" name=""SuperUser"">
                    ");
            EndContext();
            BeginContext(721, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1540f84d7de54078800e20652c3282d8", async() => {
                BeginContext(758, 2, true);
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
            BeginContext(769, 22, true);
            WriteLiteral("\r\n                    ");
            EndContext();
            BeginContext(791, 28, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "03f31985ef7547609125d8fe95da964b", async() => {
                BeginContext(809, 1, true);
                WriteLiteral("是");
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
            BeginContext(819, 22, true);
            WriteLiteral("\r\n                    ");
            EndContext();
            BeginContext(841, 28, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "70fa25a49b8e4d6eb4506af2345edc10", async() => {
                BeginContext(859, 1, true);
                WriteLiteral("否");
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
            BeginContext(869, 623, true);
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
            <span id=""UserBtns"" class=""SearchBtns"">
            </span>
        </li>
    </ul>
</div>
<div id=""UserGrid"">
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(1509, 5533, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""SUser"", SearchForm: ""#UserForm"", DataGrid: ""#UserGrid"" });
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
                if (BUI.isNullOrEmpty(opts.text) || opts.evttype == ""click"") {
                    searchOrg(form, opts);
                } else {
                    //查询业务伙伴
                    common.get(""SOrganization"", ""GetOrganizationCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.length == 1) {
                            var data = rdata.result[0];
                            var target = opts.target;
                            target.setSearchValue(data.ID || 0);
                            target.setSearchText(data.OrgName || """");
                        } else {
                            searchOrg(form, opts);
                        }
                    });
                }
            }

            form.show(900, 300);
        }

        //编辑
        common.edit = function () {
            var editRow = bGrid.getSelected();
            if (editRow == null) {");
                WriteLiteral(@"
                CommonBUI.alert(""提示"", ""请选择编辑的记录."", ""warning"");
                return false;
            }
            var form = new BuiForm({ data: editRow, common: common, type: ""edit"" });

            //外键查询
            form.fSearch = function (opts) {
                if (BUI.isNullOrEmpty(opts.text) || opts.evttype == ""click"") {
                    searchOrg(form, opts);
                } else {
                    //查询业务伙伴
                    common.get(""SOrganization"", ""GetOrganizationCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.length == 1) {
                            var data = rdata.result[0];
                            var target = opts.target;
                            target.setSearchValue(data.ID || 0);
                            target.setSearchText(data.OrgName || """");
                        } else {
                            searchOrg(form, opts);
                        }
                    }");
                WriteLiteral(@");
                }
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
                width: 300,
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
     ");
                WriteLiteral(@"                   CommonBUI.alert(""提示"", ""请选择组织机构."");
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
                var whereItems = BuiForm.GetWhereItems(common.SearchForm, common);
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
                common.InitCommonds(common, $(""#UserBtns""), iData.Commands);
                //初始化grid
                initGrid(iData);
           ");
                WriteLiteral(@" });

            var datepicker = new BUI.Calendar.DatePicker({
                trigger: '.calendar',
                autoRender: true
            });


            //绑定回车查询
            $(""#UserForm"").find(""input:text"").keyup(function (e) {
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
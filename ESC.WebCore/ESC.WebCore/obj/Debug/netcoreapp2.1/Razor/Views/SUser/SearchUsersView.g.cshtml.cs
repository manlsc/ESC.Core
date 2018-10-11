#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SUser\SearchUsersView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b0ebd0780677f76c3c183c73217853f80056cb76"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SUser_SearchUsersView), @"mvc.1.0.view", @"/Views/SUser/SearchUsersView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/SUser/SearchUsersView.cshtml", typeof(AspNetCore.Views_SUser_SearchUsersView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b0ebd0780677f76c3c183c73217853f80056cb76", @"/Views/SUser/SearchUsersView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_SUser_SearchUsersView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\SUser\SearchUsersView.cshtml"
  
    ViewBag.Title = "SearchUsersView";
    Layout = "~/Views/Shared/_LayoutSingle.cshtml";

#line default
#line hidden
            BeginContext(100, 2744, true);
            WriteLiteral(@"
<div id=""UserForm"">
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
            <span id=""UserBtns"" class=""SearchBtns"">
                <button id=""btnSearch"" class=""btn btn-default"" style=""padding-right:5px;""><i class=""fa fa-search""></i>查询</button>
            </span>
        </li>
    </ul>
</div>
<div id=""UserGrid"">
</div>
<script type=""text/javascript"">
    var common = new CommonBUI({ controller: ""SUser"", SearchForm: ""#UserForm"", DataGrid: ""#UserGrid"" });
    var bGrid = null;

    //查询
    common.search = function () {
        var whereItems = BuiForm.GetWhereItems(");
            WriteLiteral(@"common.SearchForm, common);
        common.Select(whereItems, 0, 0, function (result) {
            bGrid.setResult(result);
        });
    }

    //初始化grid
    function initGrid(iData) {
        bGrid = new BUIGrid({
            render: common.DataGrid,
            common: common,
            checkbox: true
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

    //获取返回值
    function getData() {
        return bGrid.getSelected();
    }

    //初始化列显示隐藏
    function initColumns(iData) {
        for (var i = 0; i < iData.Columns.length; i++) {
            switch (iData.Columns[i].dataIndex) {
                case ""CreateDate"":
                case ""CreateBy"":
      ");
            WriteLiteral(@"          case ""UpdateDate"":
                case ""UpdateBy"":
                    iData.Columns[i].visible = false;
                    break;
            }
        }
    }

    $(document).ready(function () {
        //初始化
        common.Init(function (iData) {
            //初始化grid
            initColumns(iData);
            initGrid(iData);
        });

        //查询
        $(""#btnSearch"").click(function () {
            common.search();
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
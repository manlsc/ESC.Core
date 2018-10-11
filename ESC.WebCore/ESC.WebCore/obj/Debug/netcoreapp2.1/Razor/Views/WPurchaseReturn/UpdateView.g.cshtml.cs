#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseReturn\UpdateView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b1cb94927fb50e611d332c55916a730464232847"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_WPurchaseReturn_UpdateView), @"mvc.1.0.view", @"/Views/WPurchaseReturn/UpdateView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/WPurchaseReturn/UpdateView.cshtml", typeof(AspNetCore.Views_WPurchaseReturn_UpdateView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b1cb94927fb50e611d332c55916a730464232847", @"/Views/WPurchaseReturn/UpdateView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_WPurchaseReturn_UpdateView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseReturn\UpdateView.cshtml"
  
    ViewBag.Title = "UpdateView";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(91, 78, true);
            WriteLiteral("<div id=\"headerForm\" class=\"fa-border\">\r\n</div>\r\n<div id=\"bodyGrid\">\r\n</div>\r\n");
            EndContext();
#line 10 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseReturn\UpdateView.cshtml"
 if (ViewBag.StockStatus == "true")
{

#line default
#line hidden
            BeginContext(209, 241, true);
            WriteLiteral("    <div style=\"text-align:center;\" class=\"SearchBtns fa-border\">\r\n        <button id=\"btnSure\" type=\"button\" class=\"btn btn-primary\">确定</button>\r\n        <button id=\"btnCancel\" type=\"button\" class=\"btn btn-default\">取消</button>\r\n    </div>\r\n");
            EndContext();
#line 16 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseReturn\UpdateView.cshtml"
}

#line default
#line hidden
            BeginContext(453, 314, true);
            WriteLiteral(@"<script type=""text/javascript"">
    var common = new CommonBUI({ controller: ""WPurchaseReturn"", GridForm: ""#headerForm"", DataGrid: ""#bodyGrid"" });
    var fcommon = new CommonBUI({ controller: ""WPurchaseReturn"", GridForm: ""#headerForm"", DataGrid: ""#bodyGrid"" });
    var bGrid = null, bForm = null;
    var id=");
            EndContext();
            BeginContext(768, 10, false);
#line 21 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseReturn\UpdateView.cshtml"
      Write(ViewBag.ID);

#line default
#line hidden
            EndContext();
            BeginContext(778, 1187, true);
            WriteLiteral(@";

    //初始化表单
    function initForm(iData) {
        bForm = new BuiForm({ common: common, colCount: 4, data: {}, type: ""add"" });

        //外键选择
        bForm.fSearch = function (opts) {
            if (opts.param.table == ""BBusinessPartner"") {
                if (BUI.isNullOrEmpty(opts.text) || opts.evttype == ""click"") {
                    searchBp(bForm, opts);
                } else {
                    //查询业务伙伴
                    fcommon.get(""BBusinessPartner"", ""GetCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.length == 1) {
                            var data = rdata.result[0];
                            bForm.setReturnValue(opts, data);
                        } else {
                            searchBp(bForm, opts);
                        }
                    });
                }
            } else {
                CommonBUI.alert(""禁止"",""不能修改仓库."",""warning"");
                return false;
           ");
            WriteLiteral(" }\r\n        };\r\n\r\n        bForm.render(fcommon.GridForm);\r\n\r\n        searchParent();\r\n    }\r\n\r\n    //初始化grid\r\n    function initGrid(iData) {\r\n        var editable=");
            EndContext();
            BeginContext(1966, 19, false);
#line 56 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WPurchaseReturn\UpdateView.cshtml"
                Write(ViewBag.StockStatus);

#line default
#line hidden
            EndContext();
            BeginContext(1985, 3835, true);
            WriteLiteral(@";
        bGrid = new BUIGrid({
            render: fcommon.DataGrid,
            common: fcommon,
            pager: true,
            editable: editable
        });

        //外键选择
        bGrid.fSearch = function (opts) {
            searchStock(bGrid, opts);
        };
        bGrid.init();

        searchDetail(1,20);
    }

    //初始化列显示隐藏
    function initColumns(iData) {
        for (var i = 0; i < iData.Columns.length; i++) {
            switch (iData.Columns[i].dataIndex) {
                case ""CreateDate"":
                case ""CreateBy"":
                case ""UpdateDate"":
                case ""UpdateBy"":
                    iData.Columns[i].visible = false;
                    break;
            }
        }
    }

    //选择业务伙伴
    function searchBp(frm, opts) {
        var dialog = new BUI.Overlay.Dialog({
            title: ""选择业务伙伴"",
            width: 800,
            height: 600,
            mask: true,
            closeAction: ""destroy"",
            bodyC");
            WriteLiteral(@"ontent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BBusinessPartner/SearchBusinessPartnerView"" style=""width:100%;height:100%;""></iframe>',
            success: function () {
                var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                if (data) {
                    frm.setReturnValue(opts, data);
                    dialog.close();
                } else {
                    CommonBUI.alert(""提示"", ""请选择业务伙伴."");
                }
            }
        });
        dialog.show();
    }

    //选择货位
    function searchStock(bGrid, opts) {
        var data = bForm.getData();
        if (!data) {
            return false;
        }

        var dialog = new BUI.Overlay.Dialog({
            title: ""选择库存"",
            width: 800,
            height: 600,
            mask: true,
            closeAction: ""destroy"",
            bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../WStock/SearchWhStockView?War");
            WriteLiteral(@"ehouseID=' + data.WarehouseID + '"" style=""width:100%;height:100%;""></iframe>',
            success: function () {
                var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                if (data) {
                    bGrid.setReturnValue(opts, data);
                    dialog.close();
                } else {
                    CommonBUI.alert(""提示"", ""请选择库存."");
                }
            }
        });
        dialog.show();
    }

    //查询主表
    function searchParent(){
        common.SearchSingle(id,function(result){
            bForm.setData(result);
        });
    }

    //查询明细
    function searchDetail(pageIndex, pageSize){
        common.SelectDetail(""WTransferhOutLine"",id, pageIndex, pageSize, function (result) {
            bGrid.setResult(result);
        });
    }

    $(document).ready(function () {
        //初始化
        common.Init(function (iData) {
            initForm(iData);
        });

        //外键初始化
        fcommon.ForeignIn");
            WriteLiteral(@"it(""WPurchaseReturnLine"", function (fData) {
            fcommon.initData = fData;
            initColumns(fData);
            initGrid(fData);
        });

        $(""#btnSure"").click(function () {
            var data = bForm.getData();
            if (data) {
                var lines = bGrid.getDirtyData();
                if (lines) {
                        data.Lines = lines;
                        common.Update(data, function () {
                            window.location.href = ""../WPurchaseReturn/Index"";
                        });
                }
            }
        });

        $(""#btnCancel"").click(function () {
            window.location.href = ""../WPurchaseReturn/Index"";
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

#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WSellReturn\AddView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3d00db0459f4a0ac8f449522be4a38d7f55d1de1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_WSellReturn_AddView), @"mvc.1.0.view", @"/Views/WSellReturn/AddView.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/WSellReturn/AddView.cshtml", typeof(AspNetCore.Views_WSellReturn_AddView))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3d00db0459f4a0ac8f449522be4a38d7f55d1de1", @"/Views/WSellReturn/AddView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_WSellReturn_AddView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\WSellReturn\AddView.cshtml"
  
    ViewBag.Title = "AddView";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(88, 305, true);
            WriteLiteral(@"
<div id=""headerForm"" class=""fa-border"">
</div>
<div id=""bodyGrid"">
</div>
<div style=""text-align:center;"" class=""SearchBtns fa-border"">
    <button id=""btnSure"" type=""button"" class=""btn btn-primary"">确定</button>
    <button id=""btnCancel"" type=""button"" class=""btn btn-default"">取消</button>
</div>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(410, 9765, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var common = new CommonBUI({ controller: ""WSellReturn"", GridForm: ""#headerForm"", DataGrid: ""#bodyGrid"" });
        var fcommon = new CommonBUI({ controller: ""WSellReturn"", GridForm: ""#headerForm"", DataGrid: ""#bodyGrid"" });
        var bGrid = null, bForm = null;

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
                                var data = ");
                WriteLiteral(@"rdata.result[0];
                                bForm.setReturnValue(opts, data);
                            } else {
                                searchBp(bForm, opts);
                            }
                        });
                    }
                } else {
                    if (BUI.isNullOrEmpty(opts.text) || opts.evttype == ""click"") {
                        searchLocation(bForm, opts);
                    } else {
                        //查询业务伙伴
                        fcommon.get(""BLocation"", ""GetLocationCodeNameWh"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                            if (rdata.result.length == 1) {
                                var data = rdata.result[0];
                                bForm.setReturnValue(opts, data);
                            } else {
                                searchLocation(bForm, opts);
                            }
                        });
                    }
                }
      ");
                WriteLiteral(@"      };

            bForm.render(fcommon.GridForm);
        }

        //初始化grid
        function initGrid(iData) {
            bGrid = new BUIGrid({
                render: fcommon.DataGrid,
                common: fcommon,
                pager: false,
                editable: true
            });

            //外键选择
            bGrid.fSearch = function (opts) {
                var data = bForm.getData();
                if (data) {
                    if (data.WarehouseID) {
                    } else {
                        CommonBUI.alert(""提示"", ""请选择仓库."");
                        return false;
                    }
                } else {
                    CommonBUI.alert(""提示"", ""请选择仓库."");
                    return false;
                }

                if (BUI.isNullOrEmpty(opts.text) || opts.evttype == ""click"") {
                    if (opts.param.table == ""BLocation"") {
                        searchStock(bGrid, opts);
                    } else {
            ");
                WriteLiteral(@"            searchMaterial(bGrid, opts);
                    }
                } else {   //如果回车查询
                    if (opts.param.table == ""BLocation"") {
                        //查询存储单元
                        fcommon.get(""BLocation"", ""GetLocationCodeNameWithWh"", ""words="" + CommonBUI.UrlEncode(opts.text) + ""&TopLocationID="" + bForm.data.WarehouseID, function (rdata) {
                            if (rdata.result.length == 1) {
                                var data = rdata.result[0];
                                bGrid.setReturnValue(opts, data);
                            } else {
                                searchStock(bGrid, opts);
                            }
                        });
                    } else {
                        //查询物料
                        fcommon.get(""BMaterial"", ""GetMagerialCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                            if (rdata.result.length == 1) {
                                var da");
                WriteLiteral(@"ta = rdata.result[0];
                                bGrid.setReturnValue(opts, data);
                            } else {
                                searchMaterial(bGrid, opts);
                            }
                        });
                    }
                }
            };

            bGrid.init();
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
  ");
                WriteLiteral(@"              mask: true,
                closeAction: ""destroy"",
                bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BBusinessPartner/SearchBusinessPartnerView"" style=""width:100%;height:100%;""></iframe>',
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
            var dialog = new BUI.Overlay.Dialog({
                title: ""选择存储单元"",
                width: 800,
                height: 600,
                mask: true,
                closeAction: ""destroy"",
                bodyContent: '<iframe id=""ifrmSe");
                WriteLiteral(@"arch"" scrolling=""auto"" frameborder=""0""  src=""../BLocation/SearchLocationByWhView?TopLocationID=' + bForm.data.WarehouseID + '"" style=""width:100%;height:100%;""></iframe>',
                success: function () {
                    var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                    if (data) {
                        bGrid.setReturnValue(opts, data);
                        dialog.close();
                    } else {
                        CommonBUI.alert(""提示"", ""请选择存储单元."");
                    }
                }
            });
            dialog.show();
        }

        //选择仓库
        function searchLocation(frm, opts) {
            var dialog = new BUI.Overlay.Dialog({
                title: ""选择存储单元"",
                width: 800,
                height: 600,
                mask: true,
                closeAction: ""destroy"",
                bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BLocation/SearchLocationWhView");
                WriteLiteral(@""" style=""width:100%;height:100%;""></iframe>',
                success: function () {
                    var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                    if (data) {
                        frm.setReturnValue(opts, data);
                        dialog.close();
                    } else {
                        CommonBUI.alert(""提示"", ""选择存储单元."");
                    }
                }
            });
            dialog.show();
        }

        //选择物料
        function searchMaterial(bGrid, opts) {
            var dialog = new BUI.Overlay.Dialog({
                title: ""选择物料"",
                width: 800,
                height: 600,
                mask: true,
                closeAction: ""destroy"",
                bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BMaterial/SearchMaterialView"" style=""width:100%;height:100%;""></iframe>',
                success: function () {
                    var data = CommonBUI.Ex");
                WriteLiteral(@"cuteIFrameMethod(""ifrmSearch"", ""getData"");
                    if (data) {
                        bGrid.setReturnValue(opts, data);
                        dialog.close();
                    } else {
                        CommonBUI.alert(""提示"", ""请选择物料."");
                    }
                }
            });
            dialog.show();
        }

        $(document).ready(function () {
            //初始化
            common.Init(function (iData) {
                initForm(iData);
            });

            //外键初始化
            fcommon.ForeignInit(""WSellReturnLine"", function (fData) {
                fcommon.initData = fData;
                initColumns(fData);
                initGrid(fData);
            });

            $(""#btnSure"").click(function () {
                var data = bForm.getData();
                if (data) {
                    var lines = bGrid.getResult();
                    if (lines) {
                        if (lines.length > 0) {
                     ");
                WriteLiteral(@"       data.Lines = lines;
                            common.Insert(data, function () {
                                window.location.href = ""../WSellReturn/Index"";
                            });
                        } else {
                            CommonBUI.alert(""提示"", ""订单明细不能为空"")
                        }
                    }
                }
            });

            $(""#btnCancel"").click(function () {
                window.location.href = ""../WSellReturn/Index"";
            });
        });

    </script>
");
                EndContext();
            }
            );
            BeginContext(10178, 2, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591

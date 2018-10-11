#pragma checksum "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BMaterial\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9e93d6ca9fe578f2f173d3b6798c90b8e29b6cef"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BMaterial_Index), @"mvc.1.0.view", @"/Views/BMaterial/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/BMaterial/Index.cshtml", typeof(AspNetCore.Views_BMaterial_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9e93d6ca9fe578f2f173d3b6798c90b8e29b6cef", @"/Views/BMaterial/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a3ef9c5565b308f1245659cc2af715730cd79da", @"/Views/_ViewImports.cshtml")]
    public class Views_BMaterial_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "E:\demo\ESC.Core\ESC.WebCore\ESC.WebCore\Views\BMaterial\Index.cshtml"
  
    ViewBag.Title = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(86, 12394, true);
            WriteLiteral(@"
<div id=""MaterialForm"">
    <ul class=""bui-search-ul"">
        <li class=""bui-inline-block"">
            <label>编码：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""MaterialCode"" />
            </span>
        </li>
        <li class=""bui-inline-block"">
            <label>名称：</label>
            <span>
                <input type=""text"" class=""form-control"" name=""MaterialName"" />
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
            <span id=""MaterialBtns"" class=""SearchBtns"">
            </span>
        </li>
    </ul>
</div>
<div id=""MaterialG");
            WriteLiteral(@"rid"">
</div>
<script type=""text/javascript"">
    var common = new CommonBUI({ controller: ""BMaterial"", SearchForm: ""#MaterialForm"", DataGrid: ""#MaterialGrid"" });
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
        form.fSearch = function (opts) {
            if (BUI.isNullOrEmpty(opts.te");
            WriteLiteral(@"xt) || opts.evttype == ""click"") {
                if (opts.param.table == ""Organization"") {
                    searchOrg(form, opts);
                } else if (opts.param.table == ""BMaterialGroup"") {
                    searchGoup(form, opts);
                } else {
                    searchUnit(form, opts);
                }
            } else {
                if (opts.param.table == ""Organization"") {
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
               ");
            WriteLiteral(@"     });
                } else if (opts.param.table == ""BMaterialGroup"") {
                    //查询物料组
                    common.get(""BMaterialGroup"", ""GetGroupCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.length == 1) {
                            var data = rdata.result[0];
                            var target = opts.target;
                            target.setSearchValue(data.ID || 0);
                            target.setSearchText(data.GroupName || """");
                        } else {
                            searchGoup(form, opts);
                        }
                    });
                } else {
                    common.get(""BUnit"", ""GetUnitCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.length == 1) {
                            var data = rdata.result[0];
                            var target = opts.target;
                     ");
            WriteLiteral(@"       target.setSearchValue(data.ID || 0);
                            target.setSearchText(data.UnitName || """");
                        } else {
                            searchGoup(form, opts);
                        }
                    });
                }
            }
        }

        form.show(800, 300);
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
            if (BUI.isNullOrEmpty(opts.text) || opts.evttype == ""click"") {
                if (opts.param.table == ""Organization"") {
                    searchOrg(form, opts);
                } else if (opts.param.table == ""BMaterialGroup"") {
                    searchGoup(form, opts);
                } else {
     ");
            WriteLiteral(@"               searchUnit(form, opts);
                }
            } else {
                if (opts.param.table == ""Organization"") {
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
                } else if (opts.param.table == ""BMaterialGroup"") {
                    //查询物料组
                    common.get(""BMaterialGroup"", ""GetGroupCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.le");
            WriteLiteral(@"ngth == 1) {
                            var data = rdata.result[0];
                            var target = opts.target;
                            target.setSearchValue(data.ID || 0);
                            target.setSearchText(data.GroupName || """");
                        } else {
                            searchGoup(form, opts);
                        }
                    });
                } else {
                    common.get(""BUnit"", ""GetUnitCodeName"", ""words="" + CommonBUI.UrlEncode(opts.text), function (rdata) {
                        if (rdata.result.length == 1) {
                            var data = rdata.result[0];
                            var target = opts.target;
                            target.setSearchValue(data.ID || 0);
                            target.setSearchText(data.UnitName || """");
                        } else {
                            searchGoup(form, opts);
                        }
                    });
                }
       ");
            WriteLiteral(@"     }
        }

        form.save = function (data) {
            common.Update(data, function () {
                common.search();
                form.close();
            });
        }
        form.show(800, 300);
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
                    CommonBUI.alert(""提示"", ""请选择组织机构."");
                }
            }
        });
        dialo");
            WriteLiteral(@"g.show();
    }

    //选择物料组
    function searchGoup(frm, opts) {
        var dialog = new BUI.Overlay.Dialog({
            title: ""选择物料组"",
            width: 300,
            height: 500,
            mask: true,
            closeAction: ""destroy"",
            bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BMaterialGroup/SearchMaterialGroupView"" style=""width:100%;height:100%;""></iframe>',
            success: function () {
                var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                if (data) {
                    frm.setReturnValue(opts, data);
                    dialog.close();
                } else {
                    CommonBUI.alert(""提示"", ""请选择存储单元."");
                }
            }
        });
        dialog.show();
    }


    //选择存储单元
    function searchUnit(frm, opts) {
        var dialog = new BUI.Overlay.Dialog({
            title: ""选择计量单位"",
            width: 800,
            height: 500,
     ");
            WriteLiteral(@"       mask: true,
            closeAction: ""destroy"",
            bodyContent: '<iframe id=""ifrmSearch"" scrolling=""auto"" frameborder=""0""  src=""../BUnit/SearchUnitView"" style=""width:100%;height:100%;""></iframe>',
            success: function () {
                var data = CommonBUI.ExcuteIFrameMethod(""ifrmSearch"", ""getData"");
                if (data) {
                    frm.setReturnValue(opts, data);
                    dialog.close();
                } else {
                    CommonBUI.alert(""提示"", ""请选择存储单元."");
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
          ");
            WriteLiteral(@"      bGrid.setResult(result);
            });
        }
        bGrid.init();
        common.search();
    }

    //初始化上传
    function initUpload() {
        var btn = document.getElementById('BMaterialupload');
        if (btn) {
            var uploader = new ss.SimpleUpload({
                button: btn,
                url: 'FileUpload',
                name: 'uploadfile',
                responseType: 'json',
                startXHR: function () {
                },
                onSubmit: function () {
                    btn.innerHTML = '上传中...';
                },
                onComplete: function (filename, rdata) {
                    btn.innerHTML = '上传';
                    if (!rdata) {
                        CommonBUI.alert(""提示"", ""文件上传失败."", ""warning"");
                        return;
                    }
                    if (rdata.status == 0) {
                        CommonBUI.alert(""提示"", ""文件上传成功."", ""success"");
                    } else {
            ");
            WriteLiteral(@"            if (rdata.message) {
                            CommonBUI.alert(""提示"", rdata.message, ""warning"");
                        } else {
                            CommonBUI.alert(""提示"", ""文件上传失败."", ""warning"");
                        }
                    }
                },
                onError: function () {
                    CommonBUI.alert(""提示"", ""文件上传失败."", ""warning"");
                    return;
                }
            });
        }

    }

    $(document).ready(function () {
        //初始化
        common.Init(function (iData) {
            //按钮初始化
            common.InitCommonds(common, $(""#MaterialBtns""), iData.Commands);
            //初始化grid
            initGrid(iData);

            initUpload();
        });

        var datepicker = new BUI.Calendar.DatePicker({
            trigger: '.calendar',
            autoRender: true
        });

        //绑定回车查询
        $(common.SearchForm).find(""input:text"").keyup(function (e) {
            if (e.which == BU");
            WriteLiteral("I.KeyCode.ENTER) {\r\n                common.search();\r\n            }\r\n        });\r\n    });\r\n\r\n</script>\r\n\r\n");
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

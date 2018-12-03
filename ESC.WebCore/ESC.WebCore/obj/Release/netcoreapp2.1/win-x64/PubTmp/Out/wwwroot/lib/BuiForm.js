(function (win, $, BUI) {
    /*
    * @desc 构造
    * @param {object} options:参数列表
    *                 {
    *                   common:"公共封装",
    *                   type:"表单类型", search add edit
    *                   colCount:列数, 
    *                   data:编辑数据,
    *                   iform:内部表单,
    *                   idialog:内部的弹框
    *                   }
    */
    var BuiForm = function (options) {
        var bForm = this;
        $.extend(bForm, { colCount: 3, data: {}, type: "search" }, options);
    }

    /*
    * @desc 获取查询条件
    * @formId {string} 表单ID
    * @common {object} 初始化信息
    */
    BuiForm.GetWhereItems = function (formId, common) {
        var whereObject = BuiForm.GetFormControl(formId);
        var whereItems = [], columns = common.initData.Columns;
        for (var key in whereObject) {
            if (key.indexOf("_begin") > 0) {
                for (var i = 0; i < columns.length; i++) {
                    var field = columns[i].get("dataIndex");
                    if (field + "_begin" == key) {
                        var dtype = CommonBUI.ConvertCshapType(columns[i].get("datatype"));
                        whereItems.push({ datatype: dtype, field: field, condition: ">=", value: whereObject[key] });
                        break;
                    }
                }
            }
            else if (key.indexOf("_end") > 0) {
                for (var i = 0; i < columns.length; i++) {
                    var field = columns[i].get("dataIndex");
                    if (field + "_end" == key) {
                        var dtype = CommonBUI.ConvertCshapType(columns[i].get("datatype"));
                        whereItems.push({ datatype: dtype, field: field, condition: "<=", value: whereObject[key] });
                        break;
                    }
                }
            } else {
                for (var i = 0; i < columns.length; i++) {
                    var field = columns[i].get("dataIndex");
                    if (field == key) {
                        var dtype = CommonBUI.ConvertCshapType(columns[i].get("datatype"));
                        whereItems.push({ datatype: dtype, field: field, condition: "=", value: whereObject[key] });
                        break;
                    }
                }
            }
        }
        return whereItems;
    }

    /*
   * @desc cols转换bui字符串
   * @param {array} cols:列集合
   */
    BuiForm.prototype.toEasyString = function (cols) {
        var bForm = this, count = bForm.colCount;
        var less = count - 1, index = 0;
        var sb = '<form id="' + bForm.FormId + '" method="post"><table style="border-collapse:separate;" width="100%" >';
        for (var i = 0; i < cols.length; i++) {
            var col = cols[i];
            var visible, dataIndex, title;
            if (col.get) {
                visible = col.get("visible");
                dataIndex = col.get("dataIndex");
                title = col.get("title");
            } else {
                visible = col.visible;
                dataIndex = col.dataIndex;
                title = col.title;
            }

            //隐藏的不显示           
            if (visible == "false" || visible == false) { continue; }

            //显示字段
            if (BUI.isNullOrEmpty(dataIndex)) { continue; }
            var fieldId = bForm.common.controller + dataIndex;

            if (index % count == 0) {
                sb = sb + '<tr>';
            }

            sb += '<td style="padding:5px;">' + title + '</td>';
            sb += '<td style="padding:5px;" id="' + fieldId + '"></td>';
            if (index % count == less) {
                sb = sb + '</tr>';
            }
            index++;
        }
        var combine = count - (index % count);
        if (combine < count) {
            for (var i = 0; i < combine; i++) {
                sb = sb + '<td align="right"></td><td></td>';
            }
        }
        sb = sb + '</tr>';
        sb += '</table></form> ';
        return sb;
    }

    /* 
    * @desc 渲染form并赋值
    * @param {array} cols:列集合
    * @param {object} data:数据
    */
    BuiForm.prototype.getChildren = function (cols, data) {
        var bForm = this, children = [];
        for (var i = 0; i < cols.length; i++) {
            var col = cols[i];
            var visible;
            if (col.get) {
                visible = col.get("visible");
            } else {
                visible = col.visible;
            }
            if (visible) {
                var cfg = getChildConfig(bForm.common.controller, cols[i], bForm);
                if (!BUI.isNullOrEmpty(data[cfg.name])) {
                    cfg.value = data[cfg.name];
                }
                //searchbox特殊处理
                if (cfg.xclass == "form-field-search") {
                    cfg.text = data[cfg.displayfield];
                }
                children.push(cfg);
            }
        }
        return children;
    }

    /*
    * @desc 显示
    * @param {number} 宽度
    * @param {number} 高度
    */
    BuiForm.prototype.show = function (width, height) {
        var bForm = this, w = width || 800, h = height || 600;
        //构造唯一ID
        bForm.DialogId = bForm.common.controller + "BFDialog";
        bForm.FormId = bForm.common.controller + "BFForm";

        //构造html
        var content = "", title = "添加", children = [];
        content = bForm.toEasyString(bForm.common.initData.Columns);
        children = bForm.getChildren(bForm.common.initData.Columns, bForm.data);
        if (bForm.type == "edit") {
            title = "编辑";
        }

        var form;
        var dialog = new BUI.Overlay.Dialog({
            title: title,
            width: width,
            height: height,
            mask: true,
            closeAction: "destroy",
            id: bForm.DialogId,
            listeners: {
                show: function () {
                    form = new BUI.Form.Form({
                        srcNode: "#" + bForm.FormId,
                        children: children
                    });
                    form.render();
                    bForm.iform = form;
                },
                closing: function () {
                    form && form.destructor();
                    bForm.iform = null;
                    dialog && dialog.destructor();
                }
            },
            bodyContent: content,
            success: function () {
                var record = form.getData(true);
                if (record !== false) {
                    if (bForm.type == "add") {
                        for (var k in record) {
                            if (!BUI.isNullOrEmpty(record[k])) {
                                bForm.data[k] = record[k];
                            }
                        }
                    } else if (bForm.type == "edit") {
                        for (var k in record) {
                            bForm.data[k] = record[k];
                        }
                    }
                    bForm.save.call(bForm, bForm.data);
                }
            }
        });
        bForm.idialog = dialog;
        dialog.show();
    }

    /*
     * 关闭
     */
    BuiForm.prototype.close = function () {
        var bForm = this;
        bForm.idialog.close();
    }

    /*
    * 渲染
    * @param {container} 容器
    */
    BuiForm.prototype.render = function (container) {
        var bForm = this;
        //构造唯一ID
        bForm.FormId = bForm.common.controller + "BFForm";

        //构造html
        var content = "", children = [];
        content = bForm.toEasyString(bForm.common.initData.Columns);
        $(container).html(content);
        children = bForm.getChildren(bForm.common.initData.Columns, bForm.data);
        var form = new BUI.Form.Form({
            srcNode: "#" + bForm.FormId,
            children: children
        });
        form.render();
        bForm.iform = form;
    }

    /*
     * 获取值
     */
    BuiForm.prototype.getData = function () {
        var bForm = this;
        var record = bForm.iform.getData(true);
        if (record !== false) {
            if (bForm.type == "add") {
                for (var k in record) {
                    if (!BUI.isNullOrEmpty(record[k])) {
                        bForm.data[k] = record[k];
                    }
                }
            } else if (bForm.type == "edit") {
                for (var k in record) {
                    bForm.data[k] = record[k];
                }
            }
            return bForm.data;
        }
        return false;
    }

    /*
    * @desc 保存
    * @param {object} data:数据
    */
    BuiForm.prototype.save = function (data) {
        return true;
    }

    /*
   * @desc 外键查询
   * @param {object} opts:参数
   */
    BuiForm.prototype.fSearch = function (opts) {

    }

    /*
    * @desc 设置返回值
    * @param {ForegionObject} fobject:外键实体
    * @param {object} data:回填数据
    */
    BuiForm.prototype.setReturnValue = function (opts, data) {
        var bForm = this, children = bForm.iform.get("children");
        var target = opts.target, fobject = opts.param;
        //为当前searchbox赋值
        target.setSearchValue(data[fobject.foreignKey] || 0);
        target.setSearchText(data[fobject.foreignValue] || "");

        //为其他字段赋值
        for (var f in fobject.returnDic) {
            for (var k = 0; k < children.length; k++) {
                if (f == children[k].get("name")) {
                    children[k].setControlValue(data[fobject.returnDic[f]]);
                    break;
                }
            }
        }
    }

    /*
   * @desc 设置返回值
   * @param {object} data:回填数据
   */
    BuiForm.prototype.setData = function (data) {
        var bForm = this, children = bForm.iform.get("children");
        bForm.data = data;
        for (var k = 0; k < children.length; k++) {
            var f = children[k].get("name");
            children[k].setControlValue(data[f]);
            var xclass = children[k].get("xclass");
            if (xclass == "form-field-search") {
                var displayfield = children[k].get("displayfield");
                children[k].setSearchText(data[displayfield]);
            }
        }
    }

    /*
    * @desc获取表单内容
    * @param {string} 表单id
    */
    BuiForm.GetFormControl = function (formId) {
        var data = {};
        $(formId + " .form-control").each(function () {
            var val = $(this).val();
            if (val || val == "0") {
                data[this.name] = val;
            }
        });
        return data;
    }

    /*
    * @desc 类型转换
    * @param {string} datatype:数据库类型
    */
    function getChildConfig(controller, col, bForm) {
        var cfg = CommonBUI.GetColumnConfig(col);
        if (col.get) {
            cfg.render = "#" + controller + col.get("dataIndex");
            cfg.name = col.get("dataIndex");
            cfg.disabled = col.get("disabled");
            cfg.displayfield = col.get("displayfield");
        } else {
            cfg.render = "#" + controller + col.dataIndex;
            cfg.name = col.dataIndex;
            cfg.disabled = col.disabled;
            cfg.displayfield = col.displayfield;
        }

        //searchbox 绑定查询事件
        switch (cfg.xclass) {
            case "form-field-search":
                cfg.search.listeners = {
                    search: function (opts) {
                        bForm.fSearch(opts);
                    }
                };
                break;
        }
        return cfg;
    }

    win.BuiForm = BuiForm;

})(window, jQuery, window.BUI);
(function (win, $) {
    var defaults = { isReturn: true, SearchForm: "", DataGrid: "" };

    /*
    * @desc 构造函数
    * @param {object} options:参数对象
    */
    var CommonBUI = function (options) {
        var target = this, src, name, copy;
        if (defaults) {
            for (name in defaults) {
                target[name] = defaults[name];
            }
        }

        if (options) {
            for (name in options) {
                copy = options[name];
                src = target[name];
                if (copy === src) continue;
                target[name] = copy;
            }
        }
    }

    /*********************************  实例方法  ******************************************/

    /*
     * @desc 查询
     * @param {array} whereItems:查询条件
     * @param {number} pageNumber:页码
     * @param {number} pageSize:页大小
     * @param {function} success:成功调用函数
     */
    CommonBUI.prototype.Select = function (whereItems, pageNumber, pageSize, success) {
        var bui = this;
        bui.CustomSelect("Search", whereItems, pageNumber, pageSize, success);
    }

    /*
    * @desc 自定义查询
    * @param {string} method:方法名
    * @param {array} whereItems:查询条件
    * @param {number} pageNumber:页码
    * @param {number} pageSize:页大小
    * @param {function} success:成功调用函数
    */
    CommonBUI.prototype.CustomSelect = function (method, whereItems, pageNumber, pageSize, success) {
        var bui = this;

        var n = pageNumber > 0 ? pageNumber : 1;
        var s = pageSize > 0 ? pageSize : 20;
        var whereParam = "page=" + n + "&rows=" + s;

        if (whereItems.length > 0) {
            whereParam += "&whereItems=" + CommonBUI.UrlEncode(JSON.stringify(whereItems));
        }
        bui.ajax("../" + bui.controller + "/" + method, whereParam, "GET", "json",
            function (result) {
                success.call(bui, result);
            },
            function (err) {
                CommonBUI.alert("异常", err.responseText, "error");
            });
    }

    /*
     * @desc 外键查询
     * @param {object} foreign:外键
     * @param {number} pageNumber:页码
     * @param {number} pageSize:页大小
     * @param {function} success:成功调用函数
     * @param {object} extParam:扩展参数
     */
    CommonBUI.prototype.ForeignSelect = function (foreign, pageNumber, pageSize, success, extParam) {
        var bui = this;
        var n = pageNumber > 0 ? pageNumber : 1;
        var s = pageSize > 0 ? pageSize : 20;
        if ($.type(foreign) != "string") {
            foreign = JSON.stringify(foreign);
        }
        var whereParam = "foreign=" + foreign + "&page=" + n + "&rows=" + s;
        if (extParam) {
            if ($.type(extParam) != "string") {
                extParam = JSON.stringify(extParam);
            }
            whereParam = whereParam + "&extParam=" + extParam;
        }
        bui.ajax("../" + bui.controller + "/ForeignSearch", whereParam, "GET", "json", function (result) {
            success.call(bui, result);
        }, function (err) {
            CommonBUI.alert("异常", err.responseText, "error");
        });
    }

    /*
     * @desc 外键查询
     * @param {string} table:表名
     * @param {number} parentID:主表ID
     * @param {number} pageNumber:页码
     * @param {number} pageSize:页大小
     * @param {function} success:成功调用函数
     */
    CommonBUI.prototype.SelectDetail = function (table, parentID, pageNumber, pageSize, success) {
        var bui = this;
        var n = pageNumber > 0 ? pageNumber : 1;
        var s = pageSize > 0 ? pageSize : 20;
        var whereParam = "table=" + table + "&parentId=" + parentID + "&page=" + n + "&rows=" + s;
        bui.ajax("../" + bui.controller + "/SearchDetail", whereParam, "GET", "json", function (result) {
            success.call(bui, result);
        }, function (err) {
            CommonBUI.alert("异常", err.responseText, "error");
        });
    }

    /*
    * @desc 主键查询
    * @param {string} Id:主键
    * @param {function} success:成功调用函数
    */
    CommonBUI.prototype.SearchSingle = function (Id, success) {
        var bui = this;
        bui.ajax("../" + bui.controller + "/SearchSingle", "Id=" + Id, "GET", "json", function (result) {
            success.call(bui, result);
        }, function (err) {
            CommonBUI.alert("异常", err.responseText, "error");
        });
    }

    /********************************Delete 删除*******************************************/
    /*
      * @desc 删除
      * @param {array} deleteRows:删除数据
      * @param {function} success:成功调用函数
      * @param {function} errf:错误调用函数
      */
    CommonBUI.prototype.Delete = function (deleteRows, success, errf) {
        var bui = this, isNotExits = true;
        if (deleteRows) {
            isNotExits = false;
        }
        if (isNotExits) {
            CommonBUI.alert("警告", "请选择要删除的记录", "warning");
            return false;
        }

        BUI.Message.Confirm("确认要删除吗?", function () {
            var deleteData = "delete=" + JSON.stringify(deleteRows);
            bui.ajax("../" + bui.controller + "/Delete", deleteData, "POST", "json",
                function (result) {
                    if (result.status == 0) {
                        CommonBUI.alert("成功", "删除成功.", "success");
                        if (success) {
                            success.call(bui, result);
                        }
                    } else {
                        CommonBUI.alert("异常", result.message, "error");
                        if (errf) {
                            errf.call(bui, result);
                        }
                    }
                },
                function (err) {
                    CommonBUI.alert("异常", err.responseText, "error");
                });
        });
    }

    /********************************Add 添加*******************************************/
    /*
    * @desc 添加
    * @param {object} data:添加数据
    * @param {function} success:成功调用函数
    * @param {function} errf:错误调用函数
    */
    CommonBUI.prototype.Insert = function (data, success, errf) {
        var bui = this;
        var addItems = "add=" + JSON.stringify(data);
        bui.post(bui.controller, "Add", addItems, function (result) {
            if (result.status == 0) {
                CommonBUI.alert("成功", "添加成功.", "success");
                if (success) {
                    success.call(bui, result);
                }
            } else {
                CommonBUI.alert("错误", result.message, "error");
                if (errf) {
                    errf.call(bui, result);
                }
            }
        });
    }

    /********************************Update 编辑*******************************************/
    /*
    * @desc 更新
    * @param {object} data:更新数据
    * @param {function} success:成功调用函数
    * @param {function} errf:错误调用函数
    */
    CommonBUI.prototype.Update = function (data, success, errf) {
        var bui = this;
        var updateItems = "update=" + JSON.stringify(data);
        bui.post(bui.controller, "Update", updateItems, function (result) {
            if (result.status == 0) {
                CommonBUI.alert("成功", "更新成功.", "success");
                if (success) {
                    success.call(bui, result);
                }
            } else {
                CommonBUI.alert("错误", result.message, "error");
                if (errf) {
                    errf.call(bui, result);
                }
            }
        });
    }

    /********************************init 初始化*******************************************/
    /*
    * @desc 初始化
    * @param {function} success:成功调用函数
    */
    CommonBUI.prototype.Init = function (success) {
        var bui = this;
        bui.ajax("../" + bui.controller + "/Init", null, "GET", "json", function (result) {
            bui.initData = result;
            success.call(bui, result);
        }, function (err) {
            CommonBUI.alert("异常", err.responseText, "error");
        });
    }

    /*
    * @desc 外键初始化
    * @param {string} table:表名
    * @param {function} success:成功调用函数
    */
    CommonBUI.prototype.ForeignInit = function (table, success) {
        var bui = this;
        bui.ajax("../" + bui.controller + "/ForeignInit", "table=" + table, "GET", "json", function (result) {
            success.call(bui, result);
        }, function (err) {
            CommonBUI.alert("错误", err.responseText, "error");
        });
    }

    /*
    * @desc 初始化按钮
    * @param {CommonBUI} bui:数据接口
    * @param {string} btnContainer:按钮容器
    * @param {array} commands:按钮列表
    */
    CommonBUI.prototype.InitCommonds = function (common, btnContainer, commands) {
        for (var i = 0; i < commands.length; i++) {
            var opt = commands[i];
            var btn = $('<button id="' + common.controller + opt.onClick + '" data-click="' + opt.onClick + '" class="btn btn-default" style="padding-right:5px;"><i class="' + opt.iconClass + '"></i>' + opt.CommandName + '</button>');
            btn.appendTo(btnContainer).click(function () {
                var onclick = $(this).attr("data-click");
                common[onclick] && common[onclick].call(common);
            });
        }
    }

    /*
     * @desc get请求
     * @param {string} controller:控制器
     * @param {string} method:请求方法
     * @param {string\object} data:数据
     * @param {function} success:成功调用函数
     * @param {function} error:失败调用函数
     */
    CommonBUI.prototype.get = function (controller, method, data, success, error) {
        var bui = this;
        bui.ajax("../" + controller + "/" + method, data, "GET", "json",
            function (result) {
                if (success) {
                    success.call(bui, result);
                } else {
                    CommonBUI.alert("信息", JSON.stringify(result), "error");
                }
            },
            function (err) {
                CommonBUI.alert("异常", err.responseText, "error");
            });
    }

    /*
    * @desc post请求
    * @param {string} controller:控制器
    * @param {string} method:请求方法
    * @param {string\object} data:数据
    * @param {function} success:成功调用函数
    * @param {function} error:失败调用函数
    */
    CommonBUI.prototype.post = function (controller, method, data, success, error) {
        var bui = this;
        bui.ajax("../" + controller + "/" + method, data, "POST", "json",
            function (result) {
                if (success) {
                    success.call(bui, result);
                } else {
                    CommonBUI.alert("信息", JSON.stringify(result), "error");
                }
            },
            function (err) {
                CommonBUI.alert("异常", err.responseText, "error");
            });
    }

    /*
    * @desc ajax请求
    * @param {string} url:路径
    * @param {string} data:数据
    * @param {string} type:请求方法
    * @param {string} datatype:返回值类型
    * @param {function} success:成功调用函数
    * @param {function} error:失败调用函数
    * @param {boole} async:是否异步
    */
    CommonBUI.prototype.ajax = function (url, data, type, datatype, success, error) {
        var bui = this;
        if (type == "POST") {
            if (bui.isReturn) {
                bui.isReturn = false;
            }
            else {
                CommonBUI.alert("信息", "系统正在处理中,请稍等...", "warning");
                return false;
            }
        }
        $.ajax({
            cache: false,
            url: url,
            data: data,
            type: type,
            datatype: datatype,
            success: function (result) {
                bui.isReturn = true;
                if (success && $.type(success) === "function") {
                    success.call(bui, result);
                } else {
                    CommonBUI.alert("信息", JSON.stringify(result), "error");
                }
            },
            error: function (err) {
                bui.isReturn = true;
                if (error && $.type(error) === "function") {
                    error.call(bui, err);
                } else {
                    CommonBUI.alert("异常", err.responseText, "error");
                }
            }
        });
    }

    /*******************************************  静态方法  ****************************************/

    /*
    * @desc url编码
    * @param {string} url:路径
    */
    CommonBUI.UrlEncode = function (url) {
        return window.encodeURIComponent(url);
    }

    /*
    * @desc url解码
    * @param {string} url:路径
    */
    CommonBUI.UrlDecode = function (url) {
        return window.decodeURIComponent(url);
    }

    /*
    * @desc 提示框
    * @param {string} msg:提示内容
    * @param {string} icon:图标
    */
    CommonBUI.alert = function (title, msg, icon) {
        var icon = icon || "info";
        BUI.Message.Alert(msg, $.noop, icon);
        return false;
    }

    /*
    * @desc 执行子iframe的方法
    * @param {string} ifrmId:iframe Id
    * @param {string} method:执行的方法
    */
    CommonBUI.ExcuteIFrameMethod = function (ifrmId, method) {
        var win = document.getElementById(ifrmId).contentWindow;
        return win[method]();
    }

    /*
   * @desc 销毁子iframe
   * @param {string} ifrmId:iframe Id
   */
    CommonBUI.ClearIframe = function (ifrmId) {
        var el = document.getElementById(ifrmId);
        if (el) {
            iframe = el.contentWindow;
            el.src = "about:blank";
            try {
                iframe.document.write("");
                iframe.document.clear();
            } catch (e) {
                document.body.removeChild(el);
            }
        }
    }

    /*
     * @desc 执行父类方法
     * @param {string} method方法名
     */
    CommonBUI.ExcuteParentMethod = function (method) {
        var win = window.parent.window;
        return win[method]();
    }

    /*******************************************  帮助方法  ****************************************/

    CommonBUI.GetColumnConfig = function (col) {
        var cfg = {};
        var combox, foregin, required, datatype;
        if (col.get) {
            combox = col.get("combox");
            foreign = col.get("foreign");
            required = col.get("required");
            datatype = CommonBUI.ConvertCshapType(col.get("datatype"));
        } else {
            combox = col.combox;
            foreign = col.foreign;
            required = col.required;
            datatype = CommonBUI.ConvertCshapType(col.datatype);
        }

        switch (datatype) {
            case "datetime":
                cfg.xclass = "form-field-date";
                break;
            case "int":
                if (combox) {
                    cfg.xclass = "form-field-select";
                    cfg.items = combox;
                    cfg.value = 0;
                } else if (foreign) {
                    cfg.xclass = "form-field-search";
                    cfg.search = {
                        searchParam: foreign
                    }
                } else {
                    cfg.xclass = "form-field-number";
                }
                break;
            default:
                if (combox) {
                    cfg.xclass = "form-field-select";
                    cfg.items = combox;
                    cfg.value = "0";
                } else if (foreign) {
                    cfg.xclass = "form-field-search";
                    cfg.search = {
                        searchParam: foreign
                    }
                } else {
                    cfg.xclass = "form-field-text";
                }
                break;
        }
        if (required) {
            cfg.rules = { required: true };
        }
        return cfg;
    }

    /*
    * @desc 获取列类型
    * @param {string} datatype:数据库类型
    */
    CommonBUI.ConvertCshapType = function (datatype) {
        var dtype = "int";
        switch (datatype) {
            case "date":
            case "datetime":
            case "time":
            case "datetime2":
            case "timestamp":
                dtype = "datetime";
                break;
            case "smallmoney":
            case "smallint":
            case "tinyint":
            case "int":
            case "int16":
            case "int32":
            case "int64":
            case "money":
            case "bigint":
            case "decimal":
            case "numeric":
            case "float":
                dtype = "int";
                break;
            default:
                dtype = "string";
                break;
        }
        return dtype;
    }

    win.CommonBUI = CommonBUI;

})(window, jQuery);
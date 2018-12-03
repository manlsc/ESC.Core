(function (win, $) {

    /*
    * @desc 构造
    * @param {object} options:参数列表
    *                 {
    *                   common:"公共封装",
    *                   checkbox:是否显示复选框,
    *                   pager:是否分页,
    *                   render:表格容器，
    *                   type:"类型 add search edit",
    *                   editable:'下拉类表和选择框是否可以编辑',
    *                   store:"内部数据",
    *                   rediting:"行编辑"
    *                   }
    */
    var BUIGrid = function (options) {
        var target = this;
        $.extend(target, { checkbox: false, pager: true, pageNumber: 1, pageSize: 20, render: "", type: "search", editable: false }, options);

        //初始化store
        if (BUI.isNullOrEmpty(target.store)) {
            target.store = new BUI.Data.Store({ isAjax: true });
        }
    }

    /*
     * @desc 构造
     */
    BUIGrid.prototype.init = function () {
        var bGrid = this, initData = bGrid.common.initData, Grid = BUI.Grid.Grid;
        var dftGrid = {
            render: bGrid.render,
            columns: initData.Columns,
            store: bGrid.store,
            plugins: []
        };

        //添加、删除
        if (bGrid.editable) {
            dftGrid.tbar = {
                items: [
                    { text: '添加行', value: 'add' },
                    { text: '删除行', value: 'delete' }],
                listeners: {
                    itemclick: function (e) {
                        if (e.item.value == "add") {
                            bGrid.add({});
                        } else {
                            var selections = bGrid.getSelected();
                            bGrid.remove(selections);
                        }
                    }
                }
            };



            //添加编辑器
            var rediting = new BUI.Grid.Plugins.RowEditing({
                triggerSelected: true //触发编辑的时候不选中行
            });
            dftGrid.plugins.push(rediting);
            bGrid.rediting = rediting;

            //设置编辑对象
            bGrid.setEditor(dftGrid.columns);
        }

        //显示复选框
        if (bGrid.checkbox) {
            dftGrid.plugins.push(BUI.Grid.Plugins.CheckSelection);
        }

        //分页
        if (bGrid.pager) {
            dftGrid.bbar = {
                pagingBar: true
            };
            bGrid.store.onAjax = function (params, callback) {
                bGrid.OnPageSearch(params.pageIndex, params.pageSize);
            }
        }

        //显示外键
        for (var i = 0; i < initData.Columns.length; i++) {
            var col = initData.Columns[i], display = col.displayfield;
            if (!BUI.isNullOrEmpty(display)) {
                col.renderer = new Function("value", "row", "index", "return row." + display);
            }
        }

        //展开
        //if (initData.Tabs.length > 0) {
        //    var cascade = new BUI.Grid.Plugins.Cascade({
        //        renderer: function (record) {
        //            return '<div class="bui-inner-grid"></div>';
        //        }
        //    });
        //    dftGrid.plugins.push(cascade);
        //    bGrid.cascade = cascade;
        //}

        bGrid.onInit(dftGrid);

        //初始化grid
        bGrid.grid = new Grid(dftGrid);
        bGrid.grid.render();

        ////展开
        //if (bGrid.cascade) {
        //    bGrid.cascade.on('expand', function (ev) {
        //        var data = ev.record,
        //          row = ev.row,
        //          sgrid = $(row).data('sub-grid');
        //        if (!sgrid) {
        //            var container = $(row).find('.bui-inner-grid');
        //            var scommon = new CommonBUI({ controller: bGrid.common.controller, DataGrid: container });
        //            //外键初始化
        //            scommon.ForeignInit(initData.Tabs[0].table, function (fData) {
        //                scommon.initData = fData;
        //                sgrid = new BUIGrid({
        //                    render: scommon.DataGrid,
        //                    common: scommon,
        //                    pager: true
        //                });
        //                sgrid.OnPageSearch = function (pageIndex, pageSize) {
        //                    scommon.SelectDetail(initData.Tabs[0].table, data.ID, pageIndex, pageSize, function (result) {
        //                        sgrid.setResult(result);
        //                    });
        //                };
        //                sgrid.init();
        //                scommon.SelectDetail(initData.Tabs[0].table, data.ID, 1, 10, function (result) {
        //                    sgrid.setResult(result);
        //                });
        //                $(row).data('sub-grid', sgrid);
        //            });                 
        //        }
        //    });
        //}
    }

    /*
    * @desc 返回当前分页的数据行
    */
    BUIGrid.prototype.getResult = function () {
        var _self = this, result = false;
        if (_self.rediting) {
            if (_self.rediting.isValid()) {
                var curEditor = _self.rediting.get("curEditor");
                if (curEditor) {
                    _self.rediting.acceptEidtor(curEditor);
                }
                result = _self.store.getResult();
            }
        } else {
            result = _self.store.getResult();
        }
        if (result) {
            result = BUI.Array.filter(result, function (value, index) {
                return !$.isEmptyObject(value);
            })
        }
        return result;
    }

    /*
    * @desc 获取增删改数据
    */
    BUIGrid.prototype.getDirtyData = function () {
        var _self = this, result = false;
        if (_self.rediting) {
            if (_self.rediting.isValid()) {
                var curEditor = _self.rediting.get("curEditor");
                if (curEditor) {
                    _self.rediting.acceptEidtor(curEditor);
                }
                result = _self.store.getDirtyData();
            } else {
                return result;
            }
        } else {
            result = _self.store.getDirtyData();
        }
        var rest = [];
        for (var i = 0; i < result.add.length; i++) {
            if (!$.isEmptyObject(result.add[i])) {
                result.add[i].CURD = "add";
                rest.push(result.add[i]);
            }
        }
        for (var i = 0; i < result.update.length; i++) {
            result.update[i].CURD = "update";
            rest.push(result.update[i]);
        }
        for (var i = 0; i < result.remove.length; i++) {
            if (result.remove[i]) {
                result.remove[i].CURD = "delete";
                rest.push(result.remove[i]);
            }
        }
        return rest;
    }

    /*
    * @desc 加载数据
    * @param {object} data:数据
    */
    BUIGrid.prototype.setResult = function (data) {
        this.store.setResult(data);
    }

    /*
    * @desc 清空
    */
    BUIGrid.prototype.clear = function () {
        this.store.set("pageIndex", 1);
        this.store.setResult({ total: 0, rows: [] });
    }

    /*
     * @desc 分页查询
     * @param {number} pageIndex:页码
     * @param {object} pageSize:页大小
     */
    BUIGrid.prototype.OnPageSearch = function (pageIndex, pageSize) { }

    /*
    * @desc 添加
    * @param {object} rowData:数据
    */
    BUIGrid.prototype.add = function (rowData) {
        var _self = this;
        if (_self.rediting) {
            if (_self.rediting.isValid()) {
                _self.store.add(rowData);
            }
        } else {
            _self.store.add(rowData);
        }
    }

    /*
    * @desc 删除
    */
    BUIGrid.prototype.remove = function (rowData) {
        var _self = this;
        if (_self.rediting) {
            var curEditor = _self.rediting.get("curEditor");
            if (curEditor) {
                var row = _self.grid.findElement(rowData);
                if (row == curEditor.row) {
                    _self.rediting.set("curEditor", null);
                }
            }
        }
        this.store.remove(rowData);
    }

    /*
   * @desc 更新
   */
    BUIGrid.prototype.update = function (rowData) {
        this.store.update(rowData);
    }

    /*
     * @desc 获取选中行
     */
    BUIGrid.prototype.getSelected = function () {
        if (this.checkbox) {
            return this.grid.getSelection();
        } else {
            return this.grid.getSelected();
        }
    }

    /*
  * @desc 设置编辑器
  * @param {array} columns:列集合
  */
    BUIGrid.prototype.setEditor = function (columns) {
        var self = this;
        for (var i = 0; i < columns.length; i++) {
            if (columns[i].disabled) continue;
            if (columns[i].visible) {
                columns[i].editor = CommonBUI.GetColumnConfig(columns[i]);
                switch (columns[i].editor.xclass) {
                    case "form-field-search":
                        columns[i].editor.search.listeners = {
                            search: function (opts) {
                                self.fSearch(opts);
                            }
                        };
                        break;
                }

            }
        }
    }

    /**
    * @desc  searchbox设置返回值   
    * @param {foreignobject} opts:查询框外键参数
    * @param {object} data:返回数据
    */
    BUIGrid.prototype.setReturnValue = function (opts, data) {
        var self = this;

        //获取编辑的行和列信息
        var row = $(self.rediting.get("curEditor").row);
        var record = self.rediting.get("record");
        var cols = bGrid.common.initData.Columns;

        //设置当前searchbox返回值
        var target = opts.target;
        target.setSearchValue(data[opts.param.foreignKey]);
        target.setSearchText(data[opts.param.foreignValue]);

        //设置searchbox的值
        record[opts.param.dataindex] = data[opts.param.foreignKey];
        for (var i = 0; i < cols.length; i++) {
            if (opts.param.dataindex == cols[i].get("dataIndex")) {
                var displayfield = cols[i].get("displayfield");
                record[displayfield] = data[opts.param.foreignValue];
                break;
            }
        }

        //赋值
        for (var f in opts.param.returnDic) {
            for (var i = 0; i < cols.length; i++) {
                if (f == cols[i].get("dataIndex")) {
                    var disabled = cols[i].get("disabled"),
                        visible = cols[i].get("visible");
                    if (visible) { //如果显示
                        if (disabled) {  //如果禁用
                            var cell = self.grid.findCell(cols[i].get("id"), row);
                            var displayfield = cols[i].get("displayfield");
                            if (BUI.isNullOrEmpty(displayfield)) {
                                cell.find(".bui-grid-cell-inner").html(data[opts.param.returnDic[f]]);
                            } else {
                                cell.find(".bui-grid-cell-inner").html(data[opts.param.returnDic[displayfield]]);
                            }
                        } else {  //如果可编辑
                            var chds = self.rediting.get("curEditor").children;
                            for (var i = 0; i < chds.length; i++) {
                                if (chds[i].get("name") == f) {
                                    chds[i].setControlValue(data[opts.param.returnDic[f]]);
                                }
                            }
                        }
                    }
                    record[f] = data[opts.param.returnDic[f]];
                    break;
                }
            }
        }
    }

    /*
    * @desc 初始化
    * @param {object} options:初始化参数
    */
    BUIGrid.prototype.onInit = function (options) {

    }

    /**
    * 外键查询
    */
    BUIGrid.prototype.fSearch = function (opts) {
    }

    win.BUIGrid = BUIGrid;

})(window, jQuery);
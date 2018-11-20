//BUI.Grid.SimpleGrid
(function (BUI, $) {
    "use strict";
    BUI.Grid = {};
    var List = BUI.List,
        Component = BUI.Component,
        UIBase = Component.UIBase,
        PREFIX = BUI.prefix,
        CLS_GRID = PREFIX + "grid",
        CLS_GRID_BORDER = PREFIX + "grid-border",
        CLS_GRID_ROW = CLS_GRID + "-row";
    var simpleGrid = BUI.List.DomList.extend({
        renderUI: function () {
            this.setColumns();
        },
        bindUI: function () {
            var _self = this,
                itemCls = _self.get("itemCls"),
                hoverCls = itemCls + "-hover",
                el = _self.get("el");
            el.delegate("." + itemCls, "mouseover",
                function (ev) {
                    var sender = $(ev.currentTarget);
                    sender.addClass(hoverCls);
                }).delegate("." + itemCls, "mouseout",
                    function (ev) {
                        var sender = $(ev.currentTarget);
                        sender.removeClass(hoverCls);
                    });
        },
        showData: function (data) {
            var _self = this;
            _self.clearData();
            BUI.each(data,
                function (record, index) {
                    _self._createRow(record, index);
                });
        },
        clearData: function () {
            var _self = this,
                tbodyEl = _self.get("itemContainer");
            tbodyEl.empty();
        },
        _uiSetColumns: function (columns) {
            var _self = this;
            _self.clearData();
            _self.setColumns(columns);
        },
        setColumns: function (columns) {
            var _self = this,
                headerRowEl = _self.get("headerRowEl");
            columns = columns || _self.get("columns");
            headerRowEl.empty();
            BUI.each(columns,
                function (column) {
                    _self._createColumn(column, headerRowEl);
                });
        },
        _createColumn: function (column, parent) {
            var _self = this,
                columnTpl = BUI.substitute(_self.get("columnTpl"), column);
            $(columnTpl).appendTo(parent);
        },
        getItemTpl: function (record, index) {
            var _self = this,
                columns = _self.get("columns"),
                rowTpl = _self.get("rowTpl"),
                cellsTpl = [],
                rowEl;
            BUI.each(columns,
                function (column) {
                    var dataIndex = column["dataIndex"];
                    cellsTpl.push(_self._getCellTpl(column, dataIndex, record));
                });
            rowTpl = BUI.substitute(rowTpl, {
                cellsTpl: cellsTpl.join("")
            });
            return rowTpl;
        },
        _getCellTpl: function (column, dataIndex, record) {
            var _self = this,
                renderer = column.renderer,
                text = renderer ? renderer(record[dataIndex], record) : record[dataIndex],
                cellTpl = _self.get("cellTpl");
            return BUI.substitute(cellTpl, {
                elCls: column.elCls,
                text: text
            });
        },
        _uiSetInnerBorder: function (v) {
            var _self = this, el = _self.get("el");
            if (v) {
                el.addClass(CLS_GRID_BORDER);
            } else {
                el.removeClass(CLS_GRID_BORDER);
            }
        },
        _uiSetTableCls: function (v) {
            var _self = this,
                tableEl = _self.get("el").find("table");
            tableEl.attr("class", v);
        }
    },
        {
            ATTRS: {
                itemCls: {
                    value: CLS_GRID_ROW
                },
                tableCls: {
                    value: CLS_GRID + "-table"
                },
                columns: {
                    sync: false,
                    value: []
                },
                innerBorder: {
                    value: true
                },
                headerRowEl: {
                    valueFn: function () {
                        var _self = this,
                            thead = _self.get("el").find("thead");
                        return thead.children("tr");
                    }
                },
                itemContainer: {
                    valueFn: function () {
                        return this.get("el").find("tbody");
                    }
                },
                tpl: {
                    value: '<table cellspacing="0" class="{tableCls}" cellpadding="0"><thead><tr></tr></thead><tbody></tbody></table>'
                },
                rowTpl: {
                    value: '<tr class="' + CLS_GRID_ROW + '">{cellsTpl}</tr>'
                },
                cellTpl: {
                    value: '<td class="' + CLS_GRID + '-cell {elCls}"><div class="' + CLS_GRID + '-cell-inner">{text}</div></td>'
                },
                columnTpl: {
                    value: '<th class="' + CLS_GRID + '-hd {elCls}" width="{width}"><div class="' + CLS_GRID + '-hd-inner">{title}</div></th>'
                },
                events: {
                    value: {}
                }
            }
        },
        {
            xclass: "simple-grid"
        });
    BUI.Grid.SimpleGrid = simpleGrid;
})(window.BUI, jQuery);
//BUI.Grid.Column
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        CLS_HD_TITLE = PREFIX + "grid-hd-title",
        SORT_PREFIX = "sort-",
        SORT_ASC = "ASC",
        SORT_DESC = "DESC";
    var column = BUI.Component.Controller.extend({
        setTplContent: function (attrs) {
            var _self = this,
                sortTpl = _self.get("sortTpl"),
                el = _self.get("el"),
                titleEl;
            var content = _self.get("content"),
                tpl = _self.getTpl(attrs);
            if (!content && tpl) {
                el.empty();
                el.html(tpl);
            }
            titleEl = el.find("." + CLS_HD_TITLE);
            $(sortTpl).insertAfter(titleEl);
        },
        _toggleSortState: function () {
            var _self = this,
                sortState = _self.get("sortState"),
                v = sortState ? sortState === SORT_ASC ? SORT_DESC : SORT_ASC : SORT_ASC;
            _self.set("sortState", v);
        },
        _uiSetWidth: function (v) {
            if (v) {
                this.get("el").css("width", v + "px");
                this.set("originWidth", v);
            }
        },
        _uiSetTitle: function (title) {
            if (!this.get("rendered")) {
                return;
            }
            this.setTplContent();
        },
        _uiSetSortable: function (v) {
            if (!this.get("rendered")) {
                return;
            }
            this.setTplContent();
        },
        _uiSetTpl: function (v) {
            if (!this.get("rendered")) {
                return;
            }
            this.setTplContent();
        },
        _uiSetSortState: function (v) {
            var _self = this,
                el = _self.get("el"),
                method = v ? "addClass" : "removeClass",
                ascCls = SORT_PREFIX + "asc",
                desCls = SORT_PREFIX + "desc";
            el.removeClass(ascCls + " " + desCls);
            if (v === "ASC") {
                el.addClass(ascCls);
            } else if (v === "DESC") {
                el.addClass(desCls);
            }
        }
    },
        {
            ATTRS: {
                elTagName: {
                    value: "th"
                },
                sortTpl: {
                    getter: function () {
                        var _self = this,
                            sortable = _self.get("sortable");
                        if (sortable) {
                            return '<span class="' + PREFIX + 'grid-sort-icon">&nbsp;</span>';
                        }
                        return "";
                    }
                },
                dataIndex: {
                    value: ""
                },
                /**
                 * 编辑器
                 */
                editor: {},
                focusable: {
                    value: false
                },
                fixed: {
                    value: false
                },
                id: {},
                visible: {},
                renderer: {},
                resizable: {
                    value: true
                },
                sortable: {
                    sync: false,
                    value: true
                },
                sortState: {
                    value: null
                },
                title: {
                    sync: false,
                    value: "&#160;"
                },
                width: {
                    value: 180
                },
                tpl: {
                    sync: false,
                    value: '<div class="' + PREFIX + 'grid-hd-inner">' + '<span class="' + CLS_HD_TITLE + '">{title}</span>' + "</div>"
                },
                events: {
                    value: {
                        afterWidthChange: true,
                        afterSortStateChange: true,
                        afterVisibleChange: true,
                        click: true,
                        resize: true,
                        move: true
                    }
                }
            }
        },
        {
            xclass: "grid-hd",
            priority: 1
        });
    column.Empty = column.extend({},
        {
            ATTRS: {
                type: {
                    value: "empty"
                },
                sortable: {
                    value: false
                },
                width: {
                    value: null
                },
                tpl: {
                    value: '<div class="' + PREFIX + 'grid-hd-inner"></div>'
                }
            }
        },
        {
            xclass: "grid-hd-empty",
            priority: 1
        });
    BUI.Grid.Column = column;
})(window.BUI, jQuery);
//BUI.Grid.Header
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        Column = BUI.Grid.Column,
        Controller = BUI.Component.Controller,
        CLS_SCROLL_WITH = 17;
    var header = Controller.extend({
        initializer: function () {
            var _self = this,
                children = _self.get("children"),
                columns = _self.get("columns"),
                emptyColumn;
            $.each(columns,
                function (index, item) {
                    var columnControl = _self._createColumn(item);
                    children[index] = columnControl;
                    columns[index] = columnControl;
                });
            emptyColumn = _self._createEmptyColumn();
            children.push(emptyColumn);
            _self.set("emptyColumn", emptyColumn);
        },
        getContentElement: function () {
            return this.get("el").find("tr");
        },
        addColumn: function (c, index) {
            var _self = this,
                insertIndex = index,
                columns = _self.get("columns");
            c = _self._createColumn(c);
            if (index === undefined) {
                index = columns.length;
                insertIndex = _self.get("children").length - 1;
            }
            columns.splice(index, 0, c);
            _self.addChild(c, insertIndex);
            _self.fire("add", {
                column: c,
                index: index
            });
            return c;
        },
        removeColumn: function (c) {
            var _self = this,
                columns = _self.get("columns"),
                index;
            c = BUI.isNumber(c) ? columns[c] : c;
            index = BUI.Array.indexOf(c, columns);
            columns.splice(index, 1);
            _self.fire("remove", {
                column: c,
                index: index
            });
            return _self.removeChild(c, true);
        },
        bindUI: function () {
            var _self = this;
            _self._bindColumnsEvent();
        },
        getColumns: function () {
            return this.get("columns");
        },
        getColumnsWidth: function () {
            var _self = this,
                columns = _self.getColumns(),
                totalWidth = 0;
            $.each(columns,
                function (index, column) {
                    if (column.get("visible")) {
                        totalWidth += column.get("el").outerWidth();
                    }
                });
            return totalWidth;
        },
        getColumnByIndex: function (index) {
            var _self = this,
                columns = _self.getColumns(),
                result = columns[index];
            return result;
        },
        getColumn: function (func) {
            var _self = this,
                columns = _self.getColumns(),
                result = null;
            $.each(columns,
                function (index, column) {
                    if (func(column)) {
                        result = column;
                        return false;
                    }
                });
            return result;
        },
        getColumnById: function (id) {
            var _self = this;
            return _self.getColumn(function (column) {
                return column.get("id") === id;
            });
        },
        getColumnIndex: function (column) {
            var _self = this,
                columns = _self.getColumns();
            return BUI.Array.indexOf(column, columns);
        },
        scrollTo: function (obj) {
            var _self = this,
                el = _self.get("el");
            if (obj.top !== undefined) {
                el.scrollTop(obj.top);
            }
            if (obj.left !== undefined) {
                el.scrollLeft(obj.left);
            }
        },
        _bindColumnsEvent: function () {
            var _self = this;
            _self.on("afterWidthChange",
                function (e) {
                    var sender = e.target;
                    if (sender !== _self) {
                        _self.setTableWidth();
                    }
                });
            _self.on("afterVisibleChange",
                function (e) {
                    var sender = e.target;
                    if (sender !== _self) {
                        _self.setTableWidth();
                    }
                });
            _self.on("afterSortStateChange",
                function (e) {
                    var sender = e.target,
                        columns = _self.getColumns(),
                        val = e.newVal;
                    if (val) {
                        $.each(columns,
                            function (index, column) {
                                if (column !== sender) {
                                    column.set("sortState", "");
                                }
                            });
                    }
                });
            _self.on("add",
                function () {
                    _self.setTableWidth();
                });
            _self.on("remove",
                function () {
                    _self.setTableWidth();
                });
        },
        _createColumn: function (cfg) {
            if (cfg instanceof Column) {
                return cfg;
            }
            if (!cfg.id) {
                cfg.id = BUI.guid("col");
            }
            return new Column(cfg);
        },
        _createEmptyColumn: function () {
            return new Column.Empty();
        },
        setTableWidth: function () {
            var _self = this,
                width = _self.get("width"),
                totalWidth = 0,
                emptyColumn = null;
            if (width == "auto") {
                return;
            }
            totalWidth = _self.getColumnsWidth();
            emptyColumn = _self.get("emptyColumn");
            if (width < totalWidth - 3) {
                emptyColumn.get("el").width(CLS_SCROLL_WITH);
            } else {
                emptyColumn.get("el").width("auto");
            }

        },
        _uiSetWidth: function (w) {
            var _self = this;
            _self.get("el").width(w);
            _self.setTableWidth();
        }
    },
        {
            ATTRS: {
                columns: {
                    value: []
                },
                emptyColumn: {},
                focusable: {
                    value: false
                },
                tpl: {
                    value: '<table cellspacing="0" class="' + PREFIX + 'grid-table" cellpadding="0"><thead><tr></tr></thead></table>'
                },
                events: {
                    value: {
                        add: false,
                        remove: false
                    }
                }
            }
        },
        {
            xclass: "grid-header",
            priority: 1
        });
    BUI.Grid.Header = header;
})(window.BUI, jQuery);
//BUI.Grid.Grid
(function (BUI, $) {
    "use strict";
    var Mask = BUI.Mask,
        Component = BUI.Component,
        toolbar = BUI.Toolbar,
        List = BUI.List,
        Header = BUI.Grid.Header,
        Column = BUI.Grid.Column;
    function isPercent(str) {
        if (BUI.isString(str)) {
            return str.indexOf("%") !== -1;
        }
        return false;
    }
    var PREFIX = BUI.prefix,
        CLS_GRID_HEADER_CONTAINER = PREFIX + "grid-header-container",
        CLS_GRID_BODY = PREFIX + "grid-body",
        CLS_GRID_WITH = PREFIX + "grid-width",
        CLS_GRID_HEIGHT = PREFIX + "grid-height",
        CLS_GRID_BORDER = PREFIX + "grid-border",
        CLS_GRID_TBAR = PREFIX + "grid-tbar",
        CLS_GRID_BBAR = PREFIX + "grid-bbar",
        CLS_BUTTON_BAR = PREFIX + "grid-btn-bar",
        CLS_GRID_ROW = PREFIX + "grid-row",
        CLS_GRID_CELL = PREFIX + "grid-cell",
        CLS_GRID_CELL_INNER = PREFIX + "grid-cell-inner",
        CLS_TD_PREFIX = "grid-td-",
        CLS_CELL_EMPTY = PREFIX + "grid-cell-empty",
        CLS_SCROLL_WITH = "17",
        CLS_HIDE = PREFIX + "hidden",
        ATTR_COLUMN_FIELD = "data-column-field",
        WIDTH_BORDER = 2;
    function getInnerWidth(width) {
        var _self = this;
        if (BUI.isNumber(width)) {
            width -= WIDTH_BORDER;
        }
        return width;
    }
    var grid = List.DomList.extend({
        createDom: function () {
            var _self = this,
                render = _self.get("render"),
                width = _self.get("width"),
                autoFit = _self.get("autoFit");
            if (width) {
                _self.set("width", width);
                _self.get("el").addClass(CLS_GRID_WITH);
            }
            if (_self.get("height")) {
                _self.get("el").addClass(CLS_GRID_HEIGHT);
            }
            if (_self.get("innerBorder")) {
                _self.get("el").addClass(CLS_GRID_BORDER);
            }
            if (autoFit) {
                _self._autoFit();
            }
        },
        renderUI: function () {
            var _self = this,
                el = _self.get("el"),
                bodyEl = el.find("." + CLS_GRID_BODY);
            _self.set("bodyEl", bodyEl);
            _self._setTableTpl();
            _self._initHeader();
            _self._initBars();
            _self.resetHeaderRow();
        },
        _setTableTpl: function (tpl) {
            var _self = this,
                bodyEl = _self.get("bodyEl");
            tpl = tpl || _self.get("tableTpl");
            $(tpl).appendTo(bodyEl);
            var tableEl = bodyEl.find("table"),
                tbodyEl = bodyEl.find("tbody");
            _self.set("tableEl", tableEl);
            _self.set("tbodyEl", tbodyEl);
            _self.set("itemContainer", tbodyEl);
            _self._setTableCls(_self.get("tableCls"));
        },
        _uiSetTableCls: function (v) {
            this._setTableCls(v);
        },
        _setTableCls: function (cls) {
            var _self = this,
                tableEl = _self.get("tableEl");
            tableEl.attr("class", cls);
        },
        resetHeaderRow: function () {
            if (!this.get("useHeaderRow")) {
                return;
            }
            var _self = this,
                headerRowEl = _self.get("headerRowEl"),
                tbodyEl = _self.get("tbodyEl");
            if (headerRowEl) {
                headerRowEl.remove();
            }
            headerRowEl = _self._createHeaderRow();
            headerRowEl.prependTo(tbodyEl);
            _self.set("headerRowEl", headerRowEl);
        },
        _createHeaderRow: function () {
            var _self = this,
                columns = _self._getColumns(),
                tbodyEl = _self.get("tbodyEl"),
                rowTpl = _self.get("headerRowTpl"),
                rowEl,
                cellsTpl = [];
            $.each(columns,
                function (index, column) {
                    if (column.get("visible")) {
                        cellsTpl.push(_self._getHeaderCellTpl(column));
                    }
                });
            if (_self.get("useEmptyCell")) {
                cellsTpl.push(_self._getEmptyCellTpl());
            }
            rowTpl = BUI.substitute(rowTpl, {
                cellsTpl: cellsTpl.join("")
            });
            rowEl = $(rowTpl).appendTo(tbodyEl);
            return rowEl;
        },
        _getColumns: function () {
            return this.get("columns");
        },
        _getHeaderCellTpl: function (column) {
            var _self = this,
                headerCellTpl = _self.get("headerCellTpl"),
                width = column.get("width");
            if (BUI.isNumeric(width)) {
                width = width + "px";
            }
            return BUI.substitute(headerCellTpl, {
                id: column.get("id"),
                width: width,
                hideCls: !column.get("visible") ? CLS_HIDE : ""
            });
        },
        _getEmptyCellTpl: function () {
            return '<td class="' + CLS_GRID_CELL + " " + CLS_CELL_EMPTY + '">&nbsp;</td>';
        },
        getItemTpl: function (record, index) {
            var _self = this,
                columns = _self._getColumns(),
                tbodyEl = _self.get("tbodyEl"),
                rowTpl = _self.get("rowTpl"),
                cellsTpl = [],
                rowEl;
            BUI.each(columns,
                function (column) {
                    if (column.get("visible")) {
                        var dataIndex = column.get("dataIndex");
                        cellsTpl.push(_self._getCellTpl(column, dataIndex, record, index));
                    }
                });
            if (_self.get("useEmptyCell")) {
                cellsTpl.push(_self._getEmptyCellTpl());
            }
            rowTpl = BUI.substitute(rowTpl, {
                cellsTpl: cellsTpl.join("")
            });
            return rowTpl;
        },
        _getCellTpl: function (column, dataIndex, record, index) {
            var _self = this,
                cellText = _self._getCellText(column, record, index) || column.get("cellTpl"),
                cellText = cellText || "&nbsp;",
                cellTpl = _self.get("cellTpl");
            return BUI.substitute(cellTpl, {
                elCls: column.get("elCls"),
                id: column.get("id"),
                dataIndex: dataIndex,
                cellText: cellText,
                hideCls: !column.get("visible") ? CLS_HIDE : ""
            });
        },
        _getCellText: function (column, record, index) {
            try {
                var _self = this,
                    dataIndex = column.get("dataIndex"),
                    renderer = column.get("renderer"),
                    text = renderer ? renderer(record[dataIndex], record, index) : record[dataIndex];
                return text;
            } catch (ex) {
                throw "column:" + column.get("title") + " fomat error!";
            }
        },
        bindUI: function () {
            var _self = this, autoFit = _self.get("autoFit"), store = _self.get('store');
            _self._bindHeaderEvent();
            _self._bindBodyEvent();
            _self._bindItemsEvent();
            if (autoFit) {
                _self._bindAutoFit();
            }
            if (!store) {
                return;
            }
            store.on("load",
                function (e) {
                    _self.onLoad(e);
                });
            store.on("add",
                function (e) {
                    _self.onAdd(e);
                });
            store.on("remove",
                function (e) {
                    _self.onRemove(e);
                });
            store.on("update",
                function (e) {
                    _self.onUpdate(e);
                });
        },
        addColumn: function (column, index) {
            var _self = this,
                header = _self.get("header");
            if (header) {
                column = header.addColumn(column, index);
            } else {
                column = new Column(column);
                _self.get("columns").splice(index, 0, column);
            }
            return column;
        },
        clearData: function () {
            this.clearItems();
        },
        getRecords: function () {
            return this.getItems();
        },
        findColumn: function (id) {
            var _self = this,
                header = _self.get("header");
            if (BUI.isNumber(id)) {
                return header.getColumnByIndex(id);
            } else {
                return header.getColumnById(id);
            }
        },
        findColumnByField: function (field) {
            var _self = this,
                header = _self.get("header");
            return header.getColumn(function (column) {
                return column.get("dataIndex") === field;
            });
        },
        findCell: function (id, record) {
            var _self = this,
                rowEl = null;
            if (record instanceof $) {
                rowEl = record;
            } else {
                rowEl = _self.findRow(record);
            }
            if (rowEl) {
                var cls = CLS_TD_PREFIX + id;
                return rowEl.find("." + cls);
            }
            return null;
        },
        findRow: function (record) {
            var _self = this;
            return $(_self.findElement(record));
        },
        removeColumn: function (column) {
            var _self = this;
            _self.get("header").removeColumn(column);
        },
        showData: function (data) {
            var _self = this;
            _self.set("items", data);
        },
        resetColumns: function () {
            var _self = this,
                store = _self.get("store");
            _self.resetHeaderRow();
            if (store) {
                _self.onLoad();
            }
        },
        _bindScrollEvent: function () {
            var _self = this,
                el = _self.get("el"),
                bodyEl = el.find("." + CLS_GRID_BODY),
                header = _self.get("header");
            bodyEl.on("scroll",
                function () {
                    var left = bodyEl.scrollLeft(),
                        top = bodyEl.scrollTop();
                    header.scrollTo({
                        left: left,
                        top: top
                    });
                    _self.fire("scroll", {
                        scrollLeft: left,
                        scrollTop: top,
                        bodyWidth: bodyEl.width(),
                        bodyHeight: bodyEl.height()
                    });
                });
        },
        _bindHeaderEvent: function () {
            var _self = this,
                header = _self.get("header"),
                store = _self.get("store");
            header.on("afterWidthChange",
                function (e) {
                    var sender = e.target;
                    if (sender !== header) {
                        _self.resetColumnsWidth(sender);
                    }
                });
            header.on("afterSortStateChange",
                function (e) {
                    var column = e.target,
                        val = e.newVal;
                    if (val && store) {
                        store.sort(column.get("dataIndex"), column.get("sortState"));
                    }
                });
            header.on("afterVisibleChange",
                function (e) {
                    var sender = e.target;
                    if (sender !== header) {
                        _self.setColumnVisible(sender);
                        _self.fire("columnvisiblechange", {
                            column: sender
                        });
                    }
                });
            header.on("click",
                function (e) {
                    var sender = e.target;
                    if (sender !== header) {
                        _self.fire("columnclick", {
                            column: sender,
                            domTarget: e.domTarget
                        });
                    }
                });
            header.on("add",
                function (e) {
                    if (_self.get("rendered")) {
                        _self.fire("columnadd", {
                            column: e.column,
                            index: e.index
                        });
                        _self.resetColumns();
                    }
                });
            header.on("remove",
                function (e) {
                    if (_self.get("rendered")) {
                        _self.resetColumns();
                        _self.fire("columnremoved", {
                            column: e.column,
                            index: e.index
                        });
                    }
                });
        },
        _bindBodyEvent: function () {
            var _self = this;
            _self._bindScrollEvent();
        },
        _bindItemsEvent: function () {
            var _self = this,
                store = _self.get("store");
            _self.on("itemsshow",
                function () {
                    _self.fire("aftershow");
                });
            _self.on("itemsclear",
                function () {
                    _self.fire("clear");
                });
            _self.on("itemclick",
                function (ev) {
                    var target = ev.domTarget,
                        record = ev.item,
                        cell = $(target).closest("." + CLS_GRID_CELL),
                        rowEl = $(target).closest("." + CLS_GRID_ROW),
                        rst;
                    if (cell.length) {
                        rst = _self.fire("cellclick", {
                            record: record,
                            row: rowEl[0],
                            cell: cell[0],
                            field: cell.attr(ATTR_COLUMN_FIELD),
                            domTarget: target,
                            domEvent: ev.domEvent
                        });
                    }
                    if (rst === false) {
                        return rst;
                    }
                    return _self.fire("rowclick", {
                        record: record,
                        row: rowEl[0],
                        domTarget: target
                    });
                });
            _self.on("itemunselected",
                function (ev) {
                    _self.fire("rowunselected", getEventObj(ev));
                });
            _self.on("itemselected",
                function (ev) {
                    _self.fire("rowselected", getEventObj(ev));
                });
            _self.on("itemrendered",
                function (ev) {
                    _self.fire("rowcreated", getEventObj(ev));
                });
            _self.on("itemremoved",
                function (ev) {
                    _self.fire("rowremoved", getEventObj(ev));
                });
            _self.on("itemupdated",
                function (ev) {
                    _self.fire("rowupdated", getEventObj(ev));
                });
            function getEventObj(ev) {
                return {
                    record: ev.item,
                    row: ev.domTarget,
                    domTarget: ev.domTarget
                };
            }
        },
        _bindAutoFit: function () {
            var _self = this,
                handler;

            $(window).on("resize",
                function () {
                    function autoFit() {
                        clearTimeout(handler);
                        handler = setTimeout(function () {
                            _self._autoFit(grid);
                        }, 200);
                        _self.set("handler", handler);
                    }
                    autoFit();
                });

        },
        _getInnerWidth: function (width) {
            width = width || this.get("width");
            return getInnerWidth(width);
        },
        _initHeader: function () {
            var _self = this,
                header = _self.get("header"),
                container = _self.get("el").find("." + CLS_GRID_HEADER_CONTAINER);
            if (!header) {
                header = new Header({
                    columns: _self.get("columns"),
                    tableCls: _self.get("tableCls"),
                    width: _self._getInnerWidth(),
                    render: container,
                    parent: _self
                }).render();
                _self.set("header", header);
            }
        },
        _initBars: function () {
            var _self = this,
                bbar = _self.get("bbar"),
                tbar = _self.get("tbar");
            _self._initBar(bbar, CLS_GRID_BBAR, "bbar");
            _self._initBar(tbar, CLS_GRID_TBAR, "tbar");
        },
        _initBar: function (bar, cls, name) {
            var _self = this,
                store = null,
                pagingBarCfg = null;
            if (bar) {
                if (!bar.xclass && !(bar instanceof Component.Controller)) {
                    bar.xclass = "button-group";
                    bar.elCls = CLS_BUTTON_BAR;

                    if (bar.pagingBar) {
                        store = _self.get("store");
                        bar = {
                            xclass: "pagingbar",
                            store: store,
                            pageSize: store.pageSize
                        };
                        if (bar.pagingBar !== true) {
                            pagingBarCfg = BUI.merge(pagingBarCfg, bar.pagingBar);
                        }
                    }
                }
                if (bar.xclass) {
                    var barContainer = _self.get("el").find("." + cls);
                    barContainer.show();
                    bar.render = barContainer;
                    bar.elTagName = "div";
                    bar.autoRender = true;
                    bar = _self.addChild(bar);
                }
                _self.set(name, bar);
            }
            return bar;
        },
        _uiSetWidth: function (w) {
            var _self = this;
            _self.get("el").width(w);
            _self.setBodyWidth(_self._getInnerWidth(w));
            _self.get("el").addClass(CLS_GRID_WITH);
            if (_self.get("rendered")) {
                if (!isPercent(w)) {
                    _self.get("header").set("width", _self._getInnerWidth(w));
                } else {
                    _self.get("header").set("width", "100%");
                }
            }
            _self.setTableWidth();
        },
        _uiSetHeight: function (h, obj) {
            var _self = this,
                header = _self.get("header");
            _self.get("el").height(h);
            _self.get("el").addClass(CLS_GRID_HEIGHT);
            _self.setBodyHeight(h);
            if (_self.get("rendered")) {
                header.setTableWidth();
            }
        },
        resetColumnsWidth: function (column, width) {
            var _self = this,
                headerRowEl = _self.get("headerRowEl"),
                cell = _self.findCell(column.get("id"), headerRowEl);
            width = width || column.get("width");
            if (cell) {
                cell.width(width);
            }
            _self.setTableWidth();
        },
        setTableWidth: function (columnsWidth) {
            if (!columnsWidth && isPercent(this.get("width"))) {
                this.get("tableEl").width("100%");
                return;
            }
            var _self = this,
                width = _self._getInnerWidth(),
                height = _self.get("height"),
                tableEl = _self.get("tableEl");
            if (!isPercent(columnsWidth)) {
                columnsWidth = columnsWidth || _self._getColumnsWidth();
                if (!width) {
                    return;
                }
                if (width >= columnsWidth) {
                    columnsWidth = width;
                    if (height) {
                        columnsWidth = width - CLS_SCROLL_WITH;
                    }
                }
            }
            tableEl.width(columnsWidth);
        },
        setBodyWidth: function (width) {
            var _self = this,
                bodyEl = _self.get("bodyEl");
            width = width || _self._getInnerWidth();
            bodyEl.width(width);
        },
        setBodyHeight: function (height) {
            var _self = this,
                bodyEl = _self.get("bodyEl"),
                bodyHeight = height,
                siblings = bodyEl.siblings();
            BUI.each(siblings,
                function (item) {
                    if ($(item).css("display") !== "none") {
                        bodyHeight -= $(item).outerHeight();
                    }
                });
            bodyEl.height(bodyHeight);
        },
        setColumnVisible: function (column) {
            var _self = this,
                hide = !column.get("visible"),
                colId = column.get("id"),
                tbodyEl = _self.get("tbodyEl"),
                cells = $("." + CLS_TD_PREFIX + colId, tbodyEl);
            if (hide) {
                cells.hide();
            } else {
                cells.show();
            }
        },
        updateItem: function (record) {
            var _self = this,
                items = _self.getItems(),
                index = BUI.Array.indexOf(record, items),
                columns = _self._getColumns(),
                element = null,
                tpl;
            if (index >= 0) {
                element = _self.findElement(record);
                BUI.each(columns,
                    function (column) {
                        var cellEl = _self.findCell(column.get("id"), $(element)),
                            textTpl = _self._getCellText(column, record, index);
                        var tpl = '<div class="' + CLS_GRID_CELL_INNER + '">' + textTpl + '</div>';
                        cellEl.html(tpl);
                    });
                return element;
            }
        },
        showEmptyText: function () {
            var _self = this,
                bodyEl = _self.get("bodyEl"),
                emptyDataTpl = _self.get("emptyDataTpl"),
                emptyEl = _self.get("emptyEl");
            if (emptyEl) {
                emptyEl.remove();
            }
            var emptyEl = $(emptyDataTpl).appendTo(bodyEl);
            _self.set("emptyEl", emptyEl);
        },
        clearEmptyText: function () {
            var _self = this,
                emptyEl = _self.get("emptyEl");
            if (emptyEl) {
                emptyEl.remove();
            }
        },
        _getColumnsWidth: function () {
            var _self = this,
                columns = _self.get("columns"),
                totalWidth = 0;
            BUI.each(columns,
                function (column) {
                    if (column.get) {
                        if (column.get("visible")) {
                            totalWidth += column.get("el").outerWidth();
                        }
                    } else {
                        if (column.visible !== false) {
                            totalWidth += column.width || 180;
                        }
                    }
                });
            return totalWidth;
        },
        _uiSetInnerBorder: function (v) {
            var _self = this,
                el = _self.get("el");
            if (v) {
                el.addClass(CLS_GRID_BORDER);
            } else {
                el.removeClass(CLS_GRID_BORDER);
            }
        },
        _autoFit: function () {
            var _self = this,
                render = _self.get('render'),
                appendWidth = 2,
                appendheight = 20,
                parentHeight = 0,
                parentWidth = 0,
                parent = $(render).parent(),
                siblings = $(render).siblings();
            if (parent.is("body")) {
                parentHeight = BUI.viewportHeight();
                parentWidth = BUI.viewportWidth();
            } else {
                parentHeight = parent.height();
                parentWidth = parent.width();
            }
            BUI.each(siblings,
                function (elem) {
                    if (elem.nodeName == 'SCRIPT' || elem.nodeName == 'script') {
                        return;
                    }
                    var node = $(elem);
                    if (node.css("position") !== "absolute") {
                        appendheight += node.outerHeight();
                    }
                });
            _self.set('width', parentWidth - appendWidth);
            _self.set('height', parentHeight - appendheight);
        },
        onAdd: function (e) {
            var _self = this,
                store = _self.get('store'),
                item = e.record;
            if (_self.getCount() == 0) { //初始为空时，列表跟Store不同步
                _self.setItems(store.getResult());
            } else {
                _self.addItemToView(item, e.index);
            }

        },
        onRemove: function (e) {
            var _self = this,
                item = e.record;
            _self.removeItem(item);
        },
        onUpdate: function (e) {
            this.updateItem(e.record);
        },
        onLocalSort: function (e) {
            var _self = this,
                store = _self.get("store");
            if (this.get('frontSortable')) {
                store.sort(e.field, e.direction);
            } else {
                this.onLoad(e);
            }
        },
        onLoad: function () {
            var _self = this,
                store = _self.get("store"),
                items = store.getResult();
            _self.set('items', items);
            if (_self.get("emptyDataTpl")) {
                if (store && store.getCount() == 0) {
                    _self.showEmptyText();
                } else {
                    _self.clearEmptyText();
                }
            }
        }
    },
        {
            ATTRS: {
                store: {
                    value: {}
                },
                header: {},
                bbar: {},
                itemCls: {
                    value: CLS_GRID_ROW
                },
                columns: {
                    value: []
                },
                items: {},
                emptyDataTpl: {},
                headerRowTpl: {
                    value: '<tr class="' + PREFIX + 'grid-header-row">{cellsTpl}</tr>'
                },
                headerCellTpl: {
                    value: '<td class="{hideCls} ' + CLS_TD_PREFIX + '{id}" style="width: {width}; height: 0;"></td>'
                },
                rowTpl: {
                    value: '<tr class="' + CLS_GRID_ROW + ' {oddCls}">{cellsTpl}</tr>'
                },
                cellTpl: {
                    value: '<td  class="{elCls} {hideCls} ' + CLS_GRID_CELL + " " + CLS_TD_PREFIX + '{id}" data-column-id="{id}" data-column-field = "{dataIndex}" >' + '<div class="' + CLS_GRID_CELL_INNER + '" >{cellText}</div>' + "</td>"
                },
                events: {
                    value: {
                        aftershow: false,
                        clear: false,
                        cellclick: false,
                        columnclick: false,
                        rowclick: false,
                        rowcreated: false,
                        rowremoved: false,
                        rowselected: false,
                        rowunselected: false,
                        scroll: false
                    }
                },
                tbar: {},
                tableCls: {
                    sync: false,
                    value: PREFIX + "grid-table"
                },
                tableTpl: {
                    value: '<table cellspacing="0" cellpadding="0" ><tbody></tbody></table>'
                },
                tpl: {
                    value: '<div class="' + CLS_GRID_TBAR + '" style="display:none"></div><div class="' + CLS_GRID_HEADER_CONTAINER + '"></div><div class="' + CLS_GRID_BODY + '"></div><div style="display:none" class="' + CLS_GRID_BBAR + '"></div>'
                },
                innerBorder: {
                    sync: false,
                    value: true
                },
                useEmptyCell: {
                    value: true
                },
                useHeaderRow: {
                    value: true
                },
                autoFit: {
                    value: true
                }
            }
        },
        {
            xclass: "grid"
        });
    BUI.Grid.Grid = grid;
})(window.BUI, jQuery);
//BUI.Grid.Format
(function (BUI, $) {
    "use strict";
    function formatTimeUnit(v) {
        if (v < 10) {
            return "0" + v;
        }
        return v;
    }
    var Format = {
        dateRenderer: function (d) {
            if (!d) {
                return "";
            }
            if (BUI.isString(d)) {
                return d;
            }
            var date = null;
            try {
                date = new Date(d);
            } catch (e) {
                return "";
            }
            if (!date || !date.getFullYear) {
                return "";
            }
            return date.getFullYear() + "-" + formatTimeUnit(date.getMonth() + 1) + "-" + formatTimeUnit(date.getDate());
        },
        datetimeRenderer: function (d) {
            if (!d) {
                return "";
            }
            if (BUI.isString(d)) {
                return d;
            }
            var date = null;
            try {
                date = new Date(d);
            } catch (e) {
                return "";
            }
            if (!date || !date.getFullYear) {
                return "";
            }
            return date.getFullYear() + "-" + formatTimeUnit(date.getMonth() + 1) + "-" + formatTimeUnit(date.getDate()) + " " + formatTimeUnit(date.getHours()) + ":" + formatTimeUnit(date.getMinutes()) + ":" + formatTimeUnit(date.getSeconds());
        },
        cutTextRenderer: function (length) {
            return function (value) {
                value = value || "";
                if (value.toString().length > length) {
                    return value.toString().substring(0, length) + "...";
                }
                return value;
            };
        },
        enumRenderer: function (enumObj) {
            return function (value) {
                return enumObj[value] || "";
            };
        },
        multipleItemsRenderer: function (enumObj) {
            var enumFun = Format.enumRenderer(enumObj);
            return function (values) {
                var result = [];
                if (!values) {
                    return "";
                }
                if (!BUI.isArray(values)) {
                    values = values.toString().split(",");
                }
                $.each(values,
                    function (index, value) {
                        result.push(enumFun(value));
                    });
                return result.join(",");
            };
        },
        moneyCentRenderer: function (v) {
            if (BUI.isString(v)) {
                v = parseFloat(v);
            }
            if ($.isNumberic(v)) {
                return (v * .01).toFixed(2);
            }
            return v;
        }
    };
    BUI.Grid.Format = Format;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.Selection
//plugins : [Grid.Plugins.CheckSelection]
(function (BUI, $) {
    "use strict";
    BUI.Grid.Plugins = {};
    var PREFIX = BUI.prefix,
        CLS_CHECKBOX = PREFIX + "grid-checkBox",
        CLS_CHECK_ICON = "bui-grid-checkbox";
    function checkSelection(config) {
        checkSelection.superclass.constructor.call(this, config);
    }
    BUI.extend(checkSelection, BUI.Base);
    checkSelection.ATTRS = {
        width: {
            value: 40
        },
        column: {},
        cellInner: {
            value: '<div class="' + CLS_CHECKBOX + '-container"><span class="' + CLS_CHECK_ICON + '"></span></div>'
        }
    };
    BUI.augment(checkSelection, {
        createDom: function (grid) {
            var _self = this;
            var cfg = {
                title: "",
                width: _self.get("width"),
                fixed: true,
                resizable: false,
                sortable: false,
                tpl: '<div class="' + PREFIX + 'grid-hd-inner">' + _self.get("cellInner") + "</div>",
                cellTpl: _self.get("cellInner")
            },
                checkColumn = grid.addColumn(cfg, 0);
            grid.set("multipleSelect", true);
            _self.set("column", checkColumn);
        },
        bindUI: function (grid) {
            var _self = this,
                col = _self.get("column"),
                colEl = col.get("el"),
                checkBox = colEl.find("." + CLS_CHECK_ICON);
            checkBox.on("click",
                function () {
                    var checked = colEl.hasClass("checked");
                    if (!checked) {
                        grid.setAllSelection();
                        colEl.addClass("checked");
                    } else {
                        grid.clearSelection();
                        colEl.removeClass("checked");
                    }
                });
            grid.on("rowunselected",
                function (e) {
                    colEl.removeClass("checked");
                });
            grid.on("clear",
                function () {
                    colEl.removeClass("checked");
                });
        }
    });
    BUI.Grid.Plugins.CheckSelection = checkSelection;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.RowNumber
//plugins : [Grid.Plugins.RowNumber]
(function (BUI, $) {
    "use strict";
    var CLS_NUMBER = "bui-grid-rownumber";
    function RowNumber(config) {
        RowNumber.superclass.constructor.call(this, config);
    }
    BUI.extend(RowNumber, BUI.Base);
    RowNumber.ATTRS = {
        width: {
            value: 40
        },
        column: {}
    };
    BUI.augment(RowNumber, {
        createDom: function (grid) {
            var _self = this;
            var cfg = {
                title: "",
                width: _self.get("width"),
                fixed: true,
                resizable: false,
                sortable: false,
                renderer: function (value, obj, index) {
                    return index + 1;
                },
                elCls: CLS_NUMBER
            },
                column = grid.addColumn(cfg, 0);
            _self.set("column", column);
        }
    });
    BUI.Grid.Plugins.RowNumber = RowNumber;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.ColumnGroup  暂时无用
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        CLS_HD_TITLE = PREFIX + "grid-hd-title",
        CLS_GROUP = PREFIX + "grid-column-group",
        CLS_GROUP_HEADER = PREFIX + "grid-group-header",
        CLS_DOUBLE = PREFIX + "grid-db-hd";
    var Group = function (cfg) {
        Group.superclass.constructor.call(this, cfg);
    };
    Group.ATTRS = {
        groups: {
            value: []
        },
        columnTpl: {
            value: '<th class="bui-grid-hd center" colspan="{colspan}"><div class="' + PREFIX + 'grid-hd-inner">' + '<span class="' + CLS_HD_TITLE + '">{title}</span>' + "</div></th>"
        }
    };
    BUI.extend(Group, BUI.Base);
    BUI.augment(Group, {
        renderUI: function (grid) {
            var _self = this,
                groups = _self.get("groups"),
                header = grid.get("header"),
                headerEl = header.get("el"),
                columns = header.get("children"),
                wraperEl = $('<tr class="' + CLS_GROUP + '"></tr>').prependTo(headerEl.find("thead"));
            headerEl.addClass(CLS_GROUP_HEADER);
            BUI.each(groups,
                function (group) {
                    var tpl = _self._getGroupTpl(group),
                        gEl = $(tpl).appendTo(wraperEl);
                    group.el = gEl;
                    for (var i = group.from; i <= group.to; i++) {
                        var column = columns[i];
                        if (column) {
                            column.set("group", group);
                        }
                    }
                });
            var afterEl;
            for (var i = columns.length - 1; i >= 0; i--) {
                var column = columns[i],
                    group = column.get("group");
                if (group) {
                    afterEl = group.el;
                } else {
                    var cEl = column.get("el");
                    cEl.addClass(CLS_DOUBLE);
                    cEl.attr("rowspan", 2);
                    if (afterEl) {
                        cEl.insertBefore(afterEl);
                    } else {
                        cEl.appendTo(wraperEl);
                    }
                    afterEl = cEl;
                }
            }
            if (groups[0].from !== 0) {
                var firstCol = columns[groups[0].from];
                if (firstCol) {
                    firstCol.get("el").css("border-left-width", 1);
                }
            }
        },
        _getGroupTpl: function (group) {
            var _self = this,
                columnTpl = _self.get("columnTpl"),
                colspan = group.to - group.from + 1;
            return BUI.substitute(columnTpl, {
                colspan: colspan,
                title: group.title
            });
        }
    });
    BUI.Grid.Plugins.ColumnGroup = Group;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.Cascade
//var cascade = new Grid.Plugins.Cascade({
//    renderer: function (record) {
//        return '<div style="padding: 10px 20px;"><h2>详情信息</h2><p>' + record.detail + '</p></div>';
//    }
//});
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        CLS_GRID_CASCADE = "",
        DATA_RECORD = "data-record",
        CLS_CASCADE = PREFIX + "grid-cascade",
        CLS_CASCADE_EXPAND = CLS_CASCADE + "-expand",
        CLS_CASCADE_ROW = CLS_CASCADE + "-row",
        CLS_CASCADE_CELL = CLS_CASCADE + "-cell",
        CLS_CASCADE_ROW_COLLAPSE = CLS_CASCADE + "-collapse";
    var cascade = function (config) {
        cascade.superclass.constructor.call(this, config);
    };
    BUI.extend(cascade, BUI.Base);
    cascade.ATTRS = {
        width: {
            value: 40
        },
        cellInner: {
            value: '<span class="' + CLS_CASCADE + '"><i class="' + CLS_CASCADE + '-icon"></i></span>'
        },
        rowTpl: {
            value: '<tr class="' + CLS_CASCADE_ROW + '"><td class="' + CLS_CASCADE_CELL + '"></td></tr>'
        },
        renderer: {},
        events: ["expand", "collapse", "removed"]
    };
    BUI.augment(cascade, {
        initializer: function (grid) {
            var _self = this;
            var cfg = {
                title: "",
                elCls: "center",
                width: _self.get("width"),
                resizable: false,
                fixed: true,
                sortable: false,
                cellTpl: _self.get("cellInner")
            },
                expandColumn = grid.addColumn(cfg, 0);
            //grid.set("innerBorder", false);
            _self.set("grid", grid);
        },
        bindUI: function (grid) {
            var _self = this;
            grid.on("cellclick",
                function (ev) {
                    var sender = $(ev.domTarget),
                        cascadeEl = sender.closest("." + CLS_CASCADE);
                    if (cascadeEl.length) {
                        if (!cascadeEl.hasClass(CLS_CASCADE_EXPAND)) {
                            _self._onExpand(ev.record, ev.row, cascadeEl);
                        } else {
                            _self._onCollapse(ev.record, ev.row, cascadeEl);
                        }
                    }
                });
            grid.on("columnvisiblechange",
                function () {
                    _self._resetColspan();
                });
            grid.on("rowremoved",
                function (ev) {
                    _self.remove(ev.record);
                });
            grid.on("clear",
                function () {
                    _self.removeAll();
                });
        },
        expandAll: function () {
            var _self = this,
                grid = _self.get("grid"),
                records = grid.getRecords();
            $.each(records,
                function (index, record) {
                    _self.expand(record);
                });
        },
        expand: function (record) {
            var _self = this,
                grid = _self.get("grid");
            var row = grid.findRow(record);
            if (row) {
                _self._onExpand(record, row);
            }
        },
        collapse: function (record) {
            var _self = this,
                grid = _self.get("grid");
            var row = grid.findRow(record);
            if (row) {
                _self._onCollapse(record, row);
            }
        },
        removeAll: function () {
            var _self = this,
                rows = _self._getAllCascadeRows();
            rows.each(function (index, row) {
                _self._removeCascadeRow(row);
            });
        },
        remove: function (record) {
            var _self = this,
                cascadeRow = _self._findCascadeRow(record);
            if (cascadeRow) {
                _self._removeCascadeRow(cascadeRow);
            }
        },
        collapseAll: function () {
            var _self = this,
                grid = _self.get("grid"),
                records = grid.getRecords();
            $.each(records,
                function (index, record) {
                    _self.collapse(record);
                });
        },
        _getRowRecord: function (cascadeRow) {
            return $(cascadeRow).data(DATA_RECORD);
        },
        _removeCascadeRow: function (row) {
            this.fire("removed", {
                record: $(row).data(DATA_RECORD),
                row: row
            });
            $(row).remove();
        },
        _findCascadeRow: function (record) {
            var _self = this,
                rows = _self._getAllCascadeRows(),
                result = null;
            $.each(rows,
                function (index, row) {
                    if (_self._getRowRecord(row) === record) {
                        result = row;
                        return false;
                    }
                });
            return result;
        },
        _getAllCascadeRows: function () {
            var _self = this,
                grid = _self.get("grid");
            return grid.get("el").find("." + CLS_CASCADE_ROW);
        },
        _getCascadeRow: function (gridRow) {
            var nextRow = $(gridRow).next();
            if (nextRow.hasClass(CLS_CASCADE_ROW)) {
                return nextRow;
            }
            return null;
        },
        _getRowContent: function (record) {
            var _self = this,
                renderer = _self.get("renderer"),
                content = renderer ? renderer(record) : "";
            return content;
        },
        _createCascadeRow: function (record, gridRow) {
            var _self = this,
                rowTpl = _self.get("rowTpl"),
                content = _self._getRowContent(record),
                rowEl = $(rowTpl).insertAfter(gridRow);
            rowEl.find("." + CLS_CASCADE_CELL).append($(content));
            rowEl.data(DATA_RECORD, record);
            return rowEl;
        },
        _onExpand: function (record, row, cascadeEl) {
            var _self = this,
                cascadeRow = _self._getCascadeRow(row),
                colspan = _self._getColumnCount(row);
            cascadeEl = cascadeEl || $(row).find("." + CLS_CASCADE);
            cascadeEl.addClass(CLS_CASCADE_EXPAND);
            if (!cascadeRow || !cascadeRow.length) {
                cascadeRow = _self._createCascadeRow(record, row);
            }
            $(cascadeRow).removeClass(CLS_CASCADE_ROW_COLLAPSE);
            _self._setColSpan(cascadeRow, row);
            _self.fire("expand", {
                record: record,
                row: cascadeRow[0]
            });
        },
        _onCollapse: function (record, row, cascadeEl) {
            var _self = this,
                cascadeRow = _self._getCascadeRow(row);
            cascadeEl = cascadeEl || $(row).find("." + CLS_CASCADE);
            cascadeEl.removeClass(CLS_CASCADE_EXPAND);
            if (cascadeRow && cascadeRow.length) {
                $(cascadeRow).addClass(CLS_CASCADE_ROW_COLLAPSE);
                _self.fire("collapse", {
                    record: record,
                    row: cascadeRow[0]
                });
            }
        },
        _getColumnCount: function (row) {
            return $(row).children().filter(function () {
                return $(this).css("display") !== "none";
            }).length;
        },
        _setColSpan: function (cascadeRow, gridRow) {
            gridRow = gridRow || $(cascadeRow).prev();
            var _self = this,
                colspan = _self._getColumnCount(gridRow);
            $(cascadeRow).find("." + CLS_CASCADE_CELL).attr("colspan", colspan);
        },
        _resetColspan: function () {
            var _self = this,
                cascadeRows = _self._getAllCascadeRows();
            $.each(cascadeRows,
                function (index, cascadeRow) {
                    _self._setColSpan(cascadeRow);
                });
        },
        destructor: function () {
            var _self = this;
            _self.removeAll();
            _self.off();
            _self.clearAttrVals();
        }
    });
    BUI.Grid.Plugins.Cascade = cascade;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.Editing
(function (BUI, $) {
    "use strict";
    var CLS_CELL_INNER = BUI.prefix + "grid-cell-inner";
    function Editing(config) {
        Editing.superclass.constructor.call(this, config);
    }
    BUI.extend(Editing, BUI.Base);
    Editing.ATTRS = {
        showError: {
            value: true
        },
        record: {},
        curEditor: {},
        fields: {
            shared: false,
            value: []
        },
        triggerSelected: {
            value: true
        }
    };
    BUI.augment(Editing, {
        initializer: function (grid) {
            var _self = this;
            _self.set("grid", grid);
        },
        renderUI: function () {
            var _self = this,
                grid = _self.get("grid");
            _self.initEditors();
            _self._initGridEvent(grid);
        },
        initEditors: function () {
            var _self = this,
                grid = _self.get("grid"),
                fields = [],
                columns = grid.get("columns");
            BUI.each(columns,
                function (column) {
                    var field = _self.getFieldConfig(column);
                    if (field) {
                        field.name = column.get("dataIndex");
                        field.colId = column.get("id");
                        field.width = column.get("width");
                        field.displayfield = column.get("displayfield");
                        field.forceFit = true;
                        if (field.validator) {
                            field.validator = _self.wrapValidator(field.validator);
                        }
                        //控件宽度小于列宽度
                        field.width = parseInt(field.width - 1);

                        fields.push(field);
                    }
                });
            _self.set("fields", fields);
        },
        acceptEidtor: function (editor) {
            var _self = this,
                grid = _self.get("grid"),
                store = grid.get("store");
            var record = _self.get("record");
            _self.updateRecord(store, record, editor);
            editor.destroy && editor.destroy();
            _self.set("curEditor", null);
        },
        _getCurEditor: function () {
            return this.get("curEditor");
        },
        _initGridEvent: function (grid) { },
        getFieldConfig: function (column) {
            return column.get("editor");
        },
        wrapValidator: function (validator) {
            var _self = this;
            return function (value) {
                var record = _self.get("record");
                return validator(value, record);
            };
        },
        onColumnVisibleChange: function (column) { },
        getEditorConstructor: function (c) {
            var _self = this,
                defaultCls = "form-field";
            if (!c.xclass) {
                if (!c.xtype) {
                    c.xclass = defaultCls;
                } else {
                    c.xclass = defaultCls + "-" + c.xtype;
                }
            }
            return BUI.Component.create(c, _self);
        },
        getEditor: function (options) { },
        getEditValue: function (options) { },
        showEditor: function (editor, options) { },
        beforeShowEditor: function (editor, options) { },
        isValid: function () {
            var _self = this, curEditor = _self.get("curEditor");
            if (curEditor) {
                var value = curEditor.getControlValue();
                var errMsg = curEditor.getValidError(value);
                var name = curEditor.get("name");
                if (BUI.isNullOrEmpty(errMsg)) {
                    return true
                } else {
                    _self.showError(errMsg, curEditor);
                    return false;
                }
            } else {
                return true;
            }
        },
        showError: function (errMsg, curEditor) {
            BUI.Message.Alert(errMsg, function () {
                curEditor.focus();
            }, 'error');
        },
        updateRecord: function (store, record, editor) { },
        cancelRecord: function (store, record) { },
        cancel: function () {
            var _self = this,
                curEditor = _self._getCurEditor();
            if (curEditor) {
                var record = _self.get("record");
                _self.cancelRecord(record);
                curEditor.destroy && curEditor.destroy();
                _self.set("curEditor", null);
            }
        },
        destructor: function () {
            var _self = this,
                editors = _self.get("editors");
            BUI.each(editors,
                function (editor) {
                    editor.destroy && editor.destroy();
                });
            _self.off();
            _self.clearAttrVals();
        }
    });
    BUI.Grid.Plugins.Editing = Editing;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.CellEditing
//var editing = new Grid.Plugins.CellEditing({
//    triggerSelected: false //触发编辑的时候不选中行
//});
(function (BUI, $) {
    "use strict";
    var Editing = BUI.Grid.Plugins.Editing,
        CLS_BODY = BUI.prefix + "grid-body",
        CLS_CELL = BUI.prefix + "grid-cell";
    var CellEditing = function (config) {
        CellEditing.superclass.constructor.call(this, config);
    };
    BUI.extend(CellEditing, Editing);
    BUI.augment(CellEditing, {
        getEditor: function (ev) {
            if (!ev.field) {
                return null;
            }
            var _self = this,
                fields = _self.get("fields"),
                editor = null;
            BUI.each(fields,
                function (cfg) {
                    if (cfg.name === ev.field) {
                        editor = _self.getEditorConstructor(cfg);
                        return false;
                    }
                });
            return editor;
        },
        resetWidth: function (editor, width) {
            editor.set("width", width);
        },
        _initGridEvent: function (grid) {
            var _self = this;
            grid.on("cellclick",
                function (ev) {
                    var editor = null,
                        domTarget = ev.domTarget,
                        curEditor = _self._getCurEditor();
                    if (curEditor) {
                        if (curEditor.get("render")[0] == ev.cell) {
                            return false;
                        } else {
                            if (curEditor.isValid()) {
                                _self.acceptEidtor(curEditor);
                            } else {
                                return false;
                            }
                        }
                    }
                    editor = _self.getEditor(ev);
                    if (editor) {
                        _self.showEditor(editor, ev);
                        if (!_self.get("triggerSelected")) {
                            return false;
                        }
                    }
                });
        },
        showEditor: function (editor, options) {
            var _self = this,
                value = _self.getEditValue(options);
            _self.beforeShowEditor(editor, options);
            $(options.cell).html("");
            _self.set("record", options.record);
            _self.fire("beforeeditorshow", {
                editor: editor,
                record: options.record
            });
            editor.set("render", $(options.cell));
            editor.set("value", value);
            editor.render();
            _self.set("curEditor", editor);
            _self.fire("editorshow", {
                editor: editor,
                record: options.record
            });
        },
        updateRecord: function (store, record, editor) {
            var _self = this,
                value = editor.getControlValue(),
                fieldName = editor.get("name"),
                preValue = record[fieldName];
            value = BUI.isDate(value) ? value.getTime() : value;
            if (preValue !== value) {
                store.setValue(record, fieldName, value);
            } else {
                var grid = _self.get("grid");
                grid.updateItem(record);
            }
        },
        cancelRecord: function (record) {
            var _self = this;
            var grid = _self.get("grid");
            grid.updateItem(record);
        },
        getEditValue: function (options) {
            if (options.record && options.field) {
                var value = options.record[options.field];
                return value == null ? "" : value;
            }
            return "";
        }
    });
    BUI.Grid.Plugins.CellEditing = CellEditing;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.RowEditing
//var editing = new Grid.Plugins.RowEditing({
//    triggerSelected: false //触发编辑的时候不选中行
//});
(function (BUI, $) {
    "use strict";
    var Editing = BUI.Grid.Plugins.Editing, CLS_ROW = BUI.prefix + "grid-row";
    var RowEditing = function (config) {
        RowEditing.superclass.constructor.call(this, config);
    };
    BUI.extend(RowEditing, Editing);
    BUI.augment(RowEditing, {
        updateRecord: function (store, record, editor) {
            var _self = this, rst = {};
            BUI.each(editor.children, function (c, k) {
                var name = c.get("name"),
                    value = c.getControlValue() || "";
                if (!rst[name]) {
                    rst[name] = value;
                } else if (BUI.isArray(rst[name]) && value != null) {
                    rst[name].push(value);
                } else if (value != null) {
                    var arr = [rst[name]];
                    arr.push(value);
                    rst[name] = arr;
                }
            });
            BUI.each(rst, function (v, k) {
                if (BUI.isDate(v)) {
                    rst[k] = v.getTime();
                }
            });
            BUI.mix(record, rst);
            store.update(record);
        },
        _initGridEvent: function (grid) {
            var _self = this;
            grid.on("rowclick",
                function (ev) {
                    var editor = null,
                        domTarget = ev.domTarget,
                        curEditor = _self._getCurEditor();
                    if (curEditor) {
                        if (curEditor.row == ev.row) {
                            return false;
                        } else {
                            if (_self.isValid()) {
                                _self.acceptEidtor(curEditor);
                            } else {
                                return false;
                            }
                        }
                    }
                    editor = _self.getEditor(ev);
                    if (editor) {
                        _self.showEditor(editor, ev);
                        if (!_self.get("triggerSelected")) {
                            return false;
                        }
                    }
                });
        },
        showEditor: function (editor, options) {
            var _self = this,
                value = _self.getEditValue(options);
            _self.beforeShowEditor(editor, options);

            _self.set("record", options.record);
            _self.fire("beforeeditorshow", {
                editor: editor,
                record: options.record
            });
            for (var i = 0; i < editor.children.length; i++) {
                editor.children[i].get("render").html("");
                editor.children[i].render();
            }

            _self.set("curEditor", editor);
            _self.fire("editorshow", {
                editor: editor,
                record: options.record
            });
        },
        cancelRecord: function (record) {
            var _self = this;
            var grid = _self.get("grid");
            grid.updateItem(record);
        },
        getEditor: function (ev) {
            var _self = this,
                fields = _self.get("fields"),
                children = [];
            var tds = $(ev.row).children();
            for (var i = 0; i < fields.length; i++) {
                for (var j = 0; j < tds.length; j++) {
                    var td = $(tds[j]);
                    if (fields[i].name == td.attr("data-column-field")) {
                        fields[i].render = td;
                        fields[i].value = ev.record[fields[i].name];
                        if (!BUI.isNullOrEmpty(fields[i].displayfield)) {
                            fields[i].text = ev.record[fields[i].displayfield];
                        }
                        var editor = _self.getEditorConstructor(fields[i]);
                        children.push(editor);
                        break;
                    }
                }
            }
            var rowEditor = {
                row: ev.row,
                children: children,
                destroy: function () {
                    BUI.each(this.children, function (c, k) {
                        c.destroy && c.destroy();
                    });
                }
            };
            return rowEditor;
        },
        getEditValue: function (options) {
            return options.record;
        },
        isValid: function () {
            var _self = this, curEditor = _self.get("curEditor");
            if (curEditor) {
                var children = curEditor.children,
                    isValid = true;
                BUI.each(children,
                    function (item) {
                        if (!item.get("disabled") && !item.isValid()) {
                            isValid = false;
                            return false;
                        }
                    });
                return isValid;
            } else {
                return true;
            }
        }
    });
    BUI.Grid.Plugins.RowEditing = RowEditing;
})(window.BUI, jQuery);
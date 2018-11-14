//BUI.List.ChildList +
(function (BUI, $) {
    "use strict";
    BUI.List = {};
    var Component = BUI.Component,
        FIELD_PREFIX = "data-";

    /**
	 * 清空选择
	 * @param {Object} child
	 */
    function clearSelected(child) {
        if (child.selected) {
            child.selected = false;
        }
        if (child.set) {
            child.set("selected", false);
        }
    }

    /**
	 * 设置child默认配置
	 * @param {Object} self
	 * @param {Object} child
	 */
    function beforeAddChild(self, child) {
        var c = child.isController ? child.getAttrVals() : child,
            defaultTpl = self.get("childTpl"),
            defaultStatusCls = self.get("childStatusCls"),
            defaultTplRender = self.get("childTplRender");
        if (defaultTpl && !c.tpl) {
            setChildAttr(child, "tpl", defaultTpl);
        }
        if (defaultTplRender && !c.tplRender) {
            setChildAttr(child, "tplRender", defaultTplRender);
        }
        if (defaultStatusCls) {
            var statusCls = c.statusCls || child.isController ? child.get("statusCls") : {};
            BUI.each(defaultStatusCls,
                function (v, k) {
                    if (v && !statusCls[k]) {
                        statusCls[k] = v;
                    }
                });
            setChildAttr(child, "statusCls", statusCls);
        }
    }

    /**
	 * 设置属性值
	 * @param {Object} child
	 * @param {Object} name
	 * @param {Object} val
	 */
    function setChildAttr(child, name, val) {
        if (child.isController) {
            child.set(name, val);
        } else {
            child[name] = val;
        }
    }

    var childList = Component.Controller.extend({
        /**
		 *初始化列表
		 */
        initializer: function () {
            var _self = this;
            _self.on("beforeRenderUI",
                function () {
                    _self._beforeRenderUI();
                });
        },
        _beforeRenderUI: function () {
            var _self = this,
                children = _self.get("children");
            BUI.each(children,
                function (child) {
                    beforeAddChild(_self, child);
                });
        },
        /**
		 * 绑定事件
		 */
        bindUI: function () {
            var _self = this,
                selectedEvent = _self.get("selectedEvent");
            _self.on(selectedEvent,
                function (e) {
                    var child = e.target;
                    if (child.get("disabled")) {
                        return;
                    }
                    if (!child.get("selected")) {
                        _self.setSelected(child);
                    } else if (_self.get("multipleSelect")) {
                        _self.clearSelected(child);
                    }
                });
            _self.on("click",
                function (e) {
                    if (e.target !== _self) {
                        _self.fire("childclick", {
                            child: e.target,
                            domTarget: e.domTarget,
                            domEvent: e
                        });
                    }
                });
            _self.on("beforeAddChild",
                function (ev) {
                    beforeAddChild(_self, ev.child);
                });
            _self.on("beforeRemoveChild",
                function (ev) {
                    var child = ev.child,
                        selected = child.get("selected");
                    if (selected) {
                        if (_self.get("multipleSelect")) {
                            _self.clearSelected(child);
                        } else {
                            _self.setSelected(null);
                        }
                    }
                    child.set("selected", false);
                });
        },
        /**
		 * 添加控件的子控件集合
		 * @param {Object} children
		 */
        addChildren: function (children) {
            var _self = this;
            BUI.each(children,
                function (child) {
                    _self.addChild(child);
                });
        },
        /**
		 * 删除控件的子控件集合
		 * @param {Object} children
		 */
        removeChildren: function (children) {
            var _self = this;
            BUI.each(children,
                function (child) {
                    var idField = _self.get("idField");
                    if (!(child instanceof BUI.Component.Controller)) {
                        child = _self.findChildByField(idField, child[idField]);
                    }
                    _self.removeChild(child, true);
                });
        },
        /**
		 *删除指定索引的子控件 
		 * @param {Object} index
		 */
        removeChildAt: function (index) {
            this.removeChild(this.getChildAt(index), true);
        },
        /**
		 *获取子控件 
		 */
        getChildren: function () {
            return this.get("children");
        },
        /**
		 *获取子控件数量 
		 */
        getChildCount: function () {
            return this.get("children").length;
        },
        /**
		 * 获取第一个子控件
		 */
        getFirstChild: function () {
            return this.getChildAt(0);
        },
        /**
		 * 获取最后一个子控件
		 */
        getLastChild: function () {
            return this.getChildAt(this.getChildCount() - 1);
        },
        /**
		 * 获取指定索引的子控件
		 * @param {Object} index
		 */
        getChildAt: function (index) {
            return this.getChildren()[index] || null;
        },
        /**
		 *通过ID获取子控件 
		 * @param {Object} id
		 */
        getChild: function (id) {
            var field = this.get("idField");
            return this.findChildByField(field, id);
        },
        /**
		 * 获取选中的项的子控件 
		 */
        getSelection: function () {
            var _self = this,
                children = _self.getChildren(),
                rst = [];
            BUI.each(children,
                function (child) {
                    if (_self.isChildSelected(child)) {
                        rst.push(child);
                    }
                });
            return rst;
        },
        /**
		 * 获取选中的第一项
		 */
        getSelected: function () {
            return this.getSelection()[0];
        },
        /**
		 * 获取选中的值集合
		 */
        getSelectionValues: function () {
            var _self = this,
                field = _self.get("idField"),
                children = _self.getSelection();
            return $.map(children,
                function (child) {
                    return _self.getValueByField(child, field);
                });
        },
        /**
		 * 获取选中的第一项的值
		 */
        getSelectedValue: function () {
            var _self = this,
                field = _self.get("idField"),
                child = _self.getSelected();
            return _self.getValueByField(child, field);
        },
        /**
		 * 获取选中的文本集合
		 */
        getSelectionText: function () {
            var _self = this,
                itechildrenms = _self.getSelection();
            return $.map(children,
                function (child) {
                    return _self.getChildText(child);
                });
        },
        /**
		 * 获取选中的第一项的文本
		 */
        getSelectedText: function () {
            var _self = this,
                child = _self.getSelected();
            return _self.getChildText(child);
        },
        /**
		 * 获取特定字段的值
		 * @param {Object} child
		 * @param {Object} field
		 */
        getValueByField: function (child, field) {
            return child && child.get(field);
        },
        /**
		 * 获取特定字段的文本
		 * @param {Object} child
		 */
        getChildText: function (child) {
            return child.get("el").text();
        },
        /**
		 * 清空选择
		 */
        clearSelection: function () {
            var _self = this,
                selection = _self.getSelection();
            BUI.each(selection,
                function (child) {
                    _self.clearSelected(child);
                });
        },
        /**
		 * 取消选中
		 * @param {Object} child
		 */
        clearSelected: function (child) {
            var _self = this;
            child = child || _self.getSelected();
            if (child) {
                _self.setChildSelected(child, false);
            }
        },
        clearControl: function () {
            this.clearChildren(true);
        },
        /**
		 * 选中特定子控件
		 * @param {Object} children
		 */
        setSelection: function (children) {
            var _self = this;
            children = BUI.isArray(children) ? children : [children];
            BUI.each(children,
                function (child) {
                    _self.setSelected(child);
                });
        },
        /**
		 * 选择特定子控件
		 * @param {Object} child
		 */
        setSelected: function (child) {
            var _self = this,
                multipleSelect = _self.get("multipleSelect");
            if (!_self.isChildselectable(child)) {
                return;
            }
            if (!multipleSelect) {
                var selectedChild = _self.getSelected();
                if (child != selectedChild) {
                    _self.clearSelected(selectedChild);
                }
            }
            _self.setChildSelected(child, true);
        },
        setChildSelected: function (child, selected) {
            var _self = this,
                isSelected;
            if (child) {
                isSelected = _self.isChildSelected(child);
                if (isSelected == selected) {
                    return;
                }
            }
            if (_self.fire("beforeselectedchange", {
                child: child,
                selected: selected
            }) !== false) {
                _self.setChildSelectedStatus(child, selected);
            }
        },
        setSelectedByField: function (field, value) {
            if (!value) {
                value = field;
                field = this.get("idField");
            }
            var _self = this,
                child = _self.findChildByField(field, value);
            _self.setSelected(child);
        },
        setSelectionByField: function (field, values) {
            if (!values) {
                values = field;
                field = this.get("idField");
            }
            var _self = this;
            BUI.each(values,
                function (value) {
                    _self.setSelectedByField(field, value);
                });
        },
        /**
		 * 设置选中状态
		 * @param {Object} child
		 * @param {Object} selected
		 */
        setChildSelectedStatus: function (child, selected) {
            var _self = this,
                chd = null;
            if (child) {
                child.set("selected", selected);
                chd = child.get("el");
            }
            _self.afterSelected(child, selected, chd);
        },
        /**
		 * 全选
		 */
        setAllSelection: function () {
            var _self = this,
                children = _self.getChildren();
            _self.setSelection(children);
        },
        /**
		 * 更新子控件
		 * @param {Object} child
		 */
        updateChild: function (child) {
            var _self = this,
                idField = _self.get("idField"),
                chd = _self.findChildByField(idField, child[idField]);
            if (chd) {
                chd.setTplContent();
            }
            return chd;
        },
        indexOfChild: function (child) {
            return BUI.Array.indexOf(child, this.getChildren());
        },
        /**
		 * 子控件是否可以选择
		 * @param {Object} child
		 */
        isChildselectable: function (child) {
            return true;
        },
        /**
		 * 子控件是否被选中
		 * @param {Object} child
		 */
        isChildSelected: function (child) {
            return child ? child.get("selected") : false;
        },
        /**
		 * 获取特定字段的child控件
		 * @param {Object} field
		 * @param {Object} value
		 * @param {Object} root
		 */
        findChildByField: function (field, value, root) {
            root = root || this;
            var _self = this,
                children = root.get("children"),
                result = null;
            BUI.each(children,
                function (child) {
                    if (child.get(field) == value) {
                        result = child;
                    } else if (child.get("children").length) {
                        result = _self.findChildByField(field, value, child);
                    }
                    if (result) {
                        return false;
                    }
                });
            return result;
        },
        afterSelected: function (child, selected, element) {
            var _self = this;
            if (selected) {
                _self.fire("childselected", {
                    child: child,
                    domTarget: element
                });
                _self.fire("selectedchange", {
                    child: child,
                    domTarget: element,
                    selected: selected
                });
            } else {
                _self.fire("childunselected", {
                    child: child,
                    domTarget: element
                });
                if (_self.get("multipleSelect")) {
                    _self.fire("selectedchange", {
                        child: child,
                        domTarget: element,
                        selected: selected
                    });
                }
            }
        }
    }, {
            ATTRS: {
                /**
                 * 主键字段
                 */
                idField: {
                    value: "id"
                },
                /**
                 * 子控件模版
                 */
                childTpl: {},
                /**
                 * 子控件渲染方法
                 */
                childTplRender: {},
                childStatusCls: {},
                /**
                 * 是否多选
                 */
                multipleSelect: {
                    value: false
                },
                /**
                 * 选择事件
                 */
                selectedEvent: {
                    value: "click"
                },
                events: {
                    value: {
                        childclick: true,
                        selectedchange: false,
                        beforeselectedchange: false,
                        childselected: false,
                        childunselected: false
                    }
                }
            }
        }, {
            xclass: "childlist"
        });
    BUI.List.ChildList = childList;
})(window.BUI, jQuery);
//BUI.List.NavItem +
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component;
    var NavItem = Component.Controller.extend({
        _uiSetSelected: function (v) {
            var _self = this,
                cls = _self.getStatusCls("selected"),
                el = _self.get("el");
            if (v) {
                el.addClass(cls);
            } else {
                el.removeClass(cls);
            }
        },
        _uiSetDisabled: function (v) {
            var _self = this,
                cls = _self.getStatusCls("disabled"),
                el = _self.get("el");
            if (v) {
                el.addClass(cls);
            } else {
                el.removeClass(cls);
            }
        }
    }, {
            ATTRS: {
                disabled: {
                    value: false
                },
                statusCls: {
                    value: {
                        selected: 'active',
                        disabled: 'disabled'
                    }
                },
                selected: {
                    value: false
                },
                elTagName: {
                    value: "li"
                },
                tpl: {
                    value: '<a href="###">{text}</a>'
                },
                elCls: {
                    value: "nav-item"
                }
            }
        }, {
            xclass: "nav-item"
        });
    BUI.List.NavItem = NavItem;
})(window.BUI, jQuery);
//BUI.List.Nav +
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        ChildList = BUI.List.ChildList;
    var Nav = ChildList.extend({}, {
        ATTRS: {
            elTagName: {
                value: "ul"
            },
            idField: {
                value: "id"
            },
            defaultChildClass: {
                value: "nav-item"
            },
            elCls: {
                value: "nav"
            }
        }
    }, {
            xclass: "list"
        });
    BUI.List.Nav = Nav;
})(window.BUI, jQuery);
//BUI.List.NavTabItem
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        Close = Component.UIBase.Close,
        CLS_TITLE = "nav-item";
    var item = Component.Controller.extend({
        renderUI: function () {
            this._resetPanelVisible();
        },
        bindUI: function () {
            var _self = this,
                el = _self.get("el"),
                eventName = _self._getVisibleEvent();
            _self.on(eventName,
                function (ev) {
                    _self._setPanelVisible(ev.newVal);
                });
        },
        _uiSetTitle: function (v) {
            var _self = this,
                el = _self.get("el"),
                titleEl = el.find("." + CLS_TITLE);
            titleEl.text(v);
        },
        _resetPanelVisible: function () {
            var _self = this,
                status = _self.get("panelVisibleStatus"),
                visible = _self.get(status);
            _self._setPanelVisible(visible);
        },
        _getVisibleEvent: function () {
            var _self = this,
                status = _self.get("panelVisibleStatus");
            return "after" + BUI.ucfirst(status) + "Change";
        },
        _setPanelVisible: function (visible) {
            var _self = this,
                panel = _self.get("panel"),
                method = visible ? "show" : "hide";
            $(panel)[method]();
        },
        _uiSetSelected: function (v) {
            var _self = this,
                cls = _self.getStatusCls("selected"),
                el = _self.get("el");
            if (v) {
                el.addClass(cls);
            } else {
                el.removeClass(cls);
            }
        },
        _setPanelContent: function (panel, content) {
            $(panel).html(content);
        },
        _uiSetPanelContent: function (v) {
            var _self = this,
                panel = _self.get("panel");
            _self._setPanelContent(panel, v);
        },
        _uiSetPanel: function (v) {
            var _self = this,
                content = _self.get("panelContent");
            if (content) {
                _self._setPanelContent(v, content);
            }
            _self._resetPanelVisible();
        }
    },
        {
            ATTRS: {
                selected: {
                    sync: true,
                    value: false
                },
                title: {
                    sync: false
                },
                statusCls: {
                    value: {
                        selected: 'active',
                        disabled: 'disabled'
                    }
                },
                elCls: {
                    value: CLS_TITLE
                },
                elTagName: {
                    value: "li"
                },
                tpl: {
                    value: '<a href="###">{title}</a>'
                },
                panel: {},
                panelContent: {},
                panelVisibleStatus: {
                    value: "selected"
                }
            }
        },
        {
            xclass: "nav-tab-item"
        });
    BUI.List.NavTabItem = item;
})(window.BUI, jQuery);
//BUI.List.NavTab
(function (BUI, $) {
    "use strict";
    BUI.Tab = {};
    var List = BUI.List,
        Component = BUI.Component;
    var NavTab = List.ChildList.extend({
        renderUI: function () {
            var _self = this,
                children = _self.get("children"),
                panelContainer = _self._initPanelContainer(),
                panels = panelContainer.children();
            BUI.each(children,
                function (item, index) {
                    var panel = panels[index];
                    _self._initPanelItem(item, panel);
                });
        },
        _initPanelContainer: function () {
            var _self = this,
                panelContainer = _self.get("panelContainer");
            if (panelContainer && BUI.isString(panelContainer)) {
                if (panelContainer.indexOf("#") == 0) {
                    panelContainer = $(panelContainer);
                } else {
                    panelContainer = _self.get("el").find(panelContainer);
                }
                _self.setInternal("panelContainer", panelContainer);
            }
            return panelContainer;
        },
        _initPanelItem: function (item, panel) {
            var _self = this;
            if (item.set) {
                if (!item.get("panel")) {
                    panel = panel || _self._getPanel(item.getAttrVals());
                    item.set("panel", panel);
                }
            } else {
                if (!item.panel) {
                    panel = panel || _self._getPanel(item);
                    item.panel = panel;
                }
            }
        },
        _getPanel: function (item) {
            var _self = this,
                panelContainer = _self.get("panelContainer"),
                panelTpl = BUI.substitute(_self.get("panelTpl"), item);
            return $(panelTpl).appendTo(panelContainer);
        }
    },
        {
            ATTRS: {
                elTagName: {
                    value: "div"
                },
                childContainer: {
                    value: ".nav"
                },
                tpl: {
                    value: '<ul class="nav nav-tabs"></ul><div class="tab-content"></div>'
                },
                panelTpl: {
                    value: '<div class="tab-pane"></div>'
                },
                panelContainer: {
                    value: ".tab-content"
                },
                defaultChildClass: {
                    value: "nav-tab-item"
                }
            }
        },
        {
            xclass: "nav-tab"
        });
    BUI.List.NavTab = NavTab;
})(window.BUI, jQuery);

//BUI.List.DomList
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        FIELD_PREFIX = "data-";

    function getItemStatusCls(name, self) {
        var _self = self,
            itemCls = _self.get("itemCls"),
            itemStatusCls = _self.get("itemStatusCls");
        if (itemStatusCls && itemStatusCls[name]) {
            return itemStatusCls[name];
        }
        return itemCls + "-" + name;
    }

    var domList = Component.Controller.extend({
        bindUI: function () {
            var _self = this,
                selectedEvent = _self.get("selectedEvent"),
                itemCls = _self.get("itemCls"),
                itemContainer = _self.getItemContainer();
            itemContainer.delegate("." + itemCls, "click",
                function (ev) {
                    if (_self.get("disabled")) {
                        return;
                    }
                    var itemEl = $(ev.currentTarget),
                        item = _self.getItemByElement(itemEl);
                    if (_self.isItemDisabled(item, itemEl)) {
                        return;
                    }
                    var rst = _self.fire("itemclick", {
                        item: item,
                        element: itemEl[0],
                        domTarget: ev.target,
                        domEvent: ev
                    });
                    if (rst !== false && selectedEvent == "click" && _self.isItemSelectable(item)) {
                        setItemSelectedStatus(item, itemEl);
                    }
                });
            if (selectedEvent !== "click") {
                itemContainer.delegate("." + itemCls, selectedEvent,
                    function (ev) {
                        if (_self.get("disabled")) {
                            return;
                        }
                        var itemEl = $(ev.currentTarget),
                            item = _self.getItemByElement(itemEl);
                        if (_self.isItemDisabled(item, itemEl)) {
                            return;
                        }
                        if (_self.isItemSelectable(item)) {
                            setItemSelectedStatus(item, itemEl);
                        }
                        return false;
                    });
            }
            itemContainer.delegate("." + itemCls, "dblclick",
                function (ev) {
                    if (_self.get("disabled")) {
                        return;
                    }
                    var itemEl = $(ev.currentTarget),
                        item = _self.getItemByElement(itemEl);
                    if (_self.isItemDisabled(item, itemEl)) {
                        return;
                    }
                    _self.fire("itemdblclick", {
                        item: item,
                        element: itemEl[0],
                        domTarget: ev.target
                    });
                });

            function setItemSelectedStatus(item, itemEl) {
                var multipleSelect = _self.get("multipleSelect"),
                    isSelected;
                isSelected = _self.isItemSelected(item, itemEl);
                if (!isSelected) {
                    if (!multipleSelect) {
                        _self.clearSelected();
                    }
                    _self.setItemSelected(item, true, itemEl);
                } else if (multipleSelect) {
                    _self.setItemSelected(item, false, itemEl);
                } else if (_self.get("cancelSelected")) {
                    _self.setSelected(null);
                }
            }
            _self.on("itemrendered itemupdated",
                function (ev) {
                    var item = ev.item,
                        element = ev.element;
                    _self._syncItemStatus(item, element);
                });
        },
        _uiSetItems: function (items) {
            var _self = this;
            if (_self.get("srcNode") && !_self.get("rendered")) {
                return;
            }
            this.setItems(items);
        },
        _syncItemStatus: function (item, element) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields");
            BUI.each(itemStatusFields,
                function (v, k) {
                    if (item[v] != null) {
                        var cls = _self.getItemStatusCls(k),
                            method = item[v] ? "addClass" : "removeClass";
                        if (element) {
                            $(element)[method](cls);
                        }
                    }
                });
        },
        /**
		 * 设置列表记录
		 * @param {Object} items
		 */
        setItems: function (items) {
            var _self = this;
            if (items != _self.getItems()) {
                _self.setInternal("items", items);
            }
            _self.clearControl();
            _self.fire("beforeitemsshow");
            BUI.each(items,
                function (item, index) {
                    _self.addItemToView(item, index);
                });
            _self.fire("itemsshow");
        },
        /**
		 * 添加列表
		 * @param {Object} items
		 */
        addItems: function (items) {
            var _self = this;
            BUI.each(items,
                function (item) {
                    _self.addItem(item);
                });
        },
        /**
		 * 添加单个列表
		 * @param {Object} item
		 */
        addItem: function (item) {
            return this.addItemAt(item, this.getCount());
        },
        /**
		 * 在指定位置添加选项,选项值为一个对象
		 * @param {Object} item
		 * @param {Object} index
		 */
        addItemAt: function (item, index) {
            var _self = this,
                items = _self.get("items");
            if (index === undefined) {
                index = items.length;
            }
            items.splice(index, 0, item);
            _self.addItemToView(item, index);
            return item;
        },
        /**
		 * 添加列表到html dom
		 * @param {Object} item
		 * @param {Object} index
		 */
        addItemToView: function (item, index) {
            var _self = this,
                listEl = _self.getItemContainer(),
                itemCls = _self.get("itemCls"),
                dataField = _self.get("dataField"),
                tpl = _self.getItemTpl(item, index),
                elem = $(tpl);
            if (index !== undefined) {
                var target = listEl.find("." + itemCls)[index];
                if (target) {
                    elem.insertBefore(target);
                } else {
                    elem.appendTo(listEl);
                }
            } else {
                elem.appendTo(listEl);
            }
            elem.addClass(itemCls);
            elem.data(dataField, item);
            _self.fire("itemrendered", {
                item: item,
                domTarget: $(elem)[0],
                element: elem
            });
            return elem;
        },
        /**
		 * 删除列表项
		 * @param {Object} items
		 */
        removeItems: function (items) {
            var _self = this;
            BUI.each(items,
                function (item) {
                    _self.removeItem(item);
                });
        },
        /**
		 * 删除列表
		 * @param {Object} item
		 */
        removeItem: function (item) {
            var _self = this,
                items = _self.get("items"),
                element = _self.findElement(item),
                index;
            index = BUI.Array.indexOf(item, items);
            if (index !== -1) {
                items.splice(index, 1);
            }
            $(element).remove();
            _self.fire("itemremoved", {
                item: item,
                domTarget: $(element)[0],
                element: element
            });
        },
        /**
		 * 删除制定位置列表
		 * @param {Object} index
		 */
        removeItemAt: function (index) {
            this.removeItem(this.getItemAt(index));
        },
        /**
		 * 获取所有列表
		 */
        getItems: function () {
            return this.get("items");
        },
        /**
		 * 获取列表总数
		 */
        getCount: function () {
            var items = this.getItems();
            return items ? items.length : 0;
        },
        /**
		 * 获取值，通过字段
		 * @param {Object} item
		 * @param {Object} field
		 */
        getValueByField: function (item, field) {
            return item && item[field];
        },
        /**
		 * 获取记录中的状态值，未定义则为undefined
		 * @param {Object} item
		 * @param {Object} status
		 */
        getStatusValue: function (item, status) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields"),
                field = itemStatusFields[status];
            return item[field];
        },
        /**
         * 更改状态值对应的字段
         * @protected
         * @param  {String} status 状态名
         * @return {String} 状态对应的字段
         */
        getStatusField: function (status) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields");
            return itemStatusFields[status];
        },
        /**
         * 获取DOM结构中的数据
         * @protected
         * @param {HTMLElement} element DOM 结构
         * @return {Object} 该项对应的值
         */
        getItemByElement: function (element) {
            var _self = this,
                dataField = _self.get("dataField");
            return $(element).data(dataField);
        },
        /**
		 * 获取选择列表项
		 */
        getSelected: function () {
            var _self = this,
                element = _self.getFirstElementByStatus("selected");
            return _self.getItemByElement(element) || null;
        },
        /**
		 * 获取特定状态列表项
		 * @param {Object} status
		 */
        getItemsByStatus: function (status) {
            var _self = this,
                elements = _self.getElementsByStatus(status),
                rst = [];
            BUI.each(elements,
                function (element) {
                    rst.push(_self.getItemByElement(element));
                });
            return rst;
        },
        /**
		 * 获取特定项目文本
		 * @param {Object} item
		 */
        getItemText: function (item) {
            var _self = this,
                textGetter = _self.get("textGetter");
            if (!item) {
                return "";
            }
            if (textGetter) {
                return textGetter(item);
            } else {
                return $(_self.findElement(item)).text();
            }
        },
        /**
		 * 获取第一个列表
		 */
        getFirstItem: function () {
            return this.getItemAt(0);
        },
        /**
		 * 获取最后一个列表
		 */
        getLastItem: function () {
            return this.getItemAt(this.getCount() - 1);
        },
        /**
		 * 获取特定索引项目
		 * @param {Object} index
		 */
        getItemAt: function (index) {
            return this.getItems()[index] || null;
        },
        /**
		 * 通过ID获取项目
		 * @param {Object} id
		 */
        getItem: function (id) {
            var field = this.get("idField");
            return this.findItemByField(field, id);
        },
        /**
		 * 获取选中项
		 */
        getSelection: function () {
            var _self = this,
                elements = _self.getSelectedElements(),
                rst = [];
            BUI.each(elements,
                function (elem) {
                    rst.push(_self.getItemByElement(elem));
                });
            return rst;
        },
        /**
		 * 获取选中项值
		 */
        getSelectedValue: function () {
            var _self = this,
                field = _self.get("idField"),
                item = _self.getSelected();
            return _self.getValueByField(item, field);
        },
        /**
		 * 获取选中项值
		 */
        getSelectionValues: function () {
            var _self = this,
                field = _self.get("idField"),
                items = _self.getSelection();
            return $.map(items,
                function (item) {
                    return _self.getValueByField(item, field);
                });
        },
        /**
		 * 获取选中项文本
		 */
        getSelectionText: function () {
            var _self = this,
                items = _self.getSelection();
            return $.map(items,
                function (item) {
                    return _self.getItemText(item);
                });
        },
        /**
		 * 获取选中项文本
		 */
        getSelectedText: function () {
            var _self = this,
                item = _self.getSelected();
            return _self.getItemText(item);
        },
        /**
		 * 清空状态
		 * @param {Object} item
		 * @param {Object} status
		 * @param {Object} element
		 */
        clearItemStatus: function (item, status, element) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields");
            element = element || _self.findElement(item);
            if (status) {
                _self.setItemStatus(item, status, false, element);
            } else {
                BUI.each(itemStatusFields,
                    function (v, k) {
                        _self.setItemStatus(item, k, false, element);
                    });
                if (!itemStatusFields["selected"]) {
                    _self.setItemSelected(item, false);
                }
                _self.setItemStatus(item, "hover", false);
            }
        },
        /**
		 * 清空选择
		 */
        clearSelection: function () {
            var _self = this,
                selection = _self.getSelection();
            BUI.each(selection,
                function (item) {
                    _self.clearSelected(item);
                });
        },
        /**
		 * 清空特定项选择
		 * @param {Object} item
		 */
        clearSelected: function (item) {
            var _self = this;
            item = item || _self.getSelected();
            if (item) {
                _self.setItemSelected(item, false);
            }
        },
        clearItems: function () {
            var _self = this,
                items = _self.getItems();
            items.splice(0);
            _self.clearControl();
        },
        /**
         * 清空列表项
         */
        clearControl: function () {
            this.fire("beforeitemsclear");
            var _self = this,
                listEl = _self.getItemContainer(),
                itemCls = _self.get("itemCls");
            listEl.find("." + itemCls).remove();
            this.fire("itemsclear");
        },
        setStatusValue: function (item, status, value) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields"),
                field = itemStatusFields[status];
            if (field) {
                item[field] = value;
            }
        },
        /**
         * 全选
         */
        setAllSelection: function () {
            var _self = this,
                items = _self.getItems();
            _self.setSelection(items);
        },
        /**
         * 设置禁用
         * @param {Object} item
         * @param {Object} disabled
         */
        setItemDisabled: function (item, disabled) {
            var _self = this;
            _self.setItemStatus(item, "disabled", disabled);
        },
        /**
         * 设置列表项选中/未选中
         * @param {Object} item
         * @param {Object} selected
         */
        setItemSelected: function (item, selected) {
            var _self = this,
                isSelected;
            if (item) {
                isSelected = _self.isItemSelected(item);
                if (isSelected == selected) {
                    return;
                }
            }
            if (_self.fire("beforeselectedchange", {
                item: item,
                selected: selected
            }) !== false) {
                _self.setItemSelectedStatus(item, selected);
            }
        },
        /**
         * 根据特定字段值选中列表项
         * @param {Object} field
         * @param {Object} value
         */
        setSelectedByField: function (field, value) {
            if (!value) {
                value = field;
                field = this.get("idField");
            }
            var _self = this,
                item = _self.findItemByField(field, value);
            _self.setSelected(item);
        },
        /**
		 * 通过特定字段选中项目
		 * @param {Object} field
		 * @param {Object} values
		 */
        setSelectionByField: function (field, values) {
            if (!values) {
                values = field;
                field = this.get("idField");
            }
            var _self = this;
            BUI.each(values,
                function (value) {
                    _self.setSelectedByField(field, value);
                });
        },
        /**
		 * 选中项目
		 * @param {Object} items
		 */
        setSelection: function (items) {
            var _self = this;
            items = BUI.isArray(items) ? items : [items];
            BUI.each(items,
                function (item) {
                    _self.setSelected(item);
                });
        },
        /**
		 * 选中项目
		 * @param {Object} item
		 */
        setSelected: function (item) {
            var _self = this,
                multipleSelect = _self.get("multipleSelect");
            if (!_self.isItemSelectable(item)) {
                return;
            }
            if (!multipleSelect) {
                var selectedItem = _self.getSelected();
                if (item != selectedItem) {
                    _self.clearSelected(selectedItem);
                }
            }
            _self.setItemSelected(item, true);
        },
        /**
		 * 设置特定状态
		 * @param {Object} item
		 * @param {Object} status
		 * @param {Object} value
		 * @param {Object} element
		 */
        setItemStatus: function (item, status, value, element) {
            var _self = this;
            if (item) {
                element = element || _self.findElement(item);
            }
            if (!_self.isItemDisabled(item, element) || status === "disabled") {
                if (item) {
                    if (status === "disabled" && value) {
                        _self.clearItemStatus(item);
                    }
                    _self.setStatusValue(item, status, value);
                    _self.setItemStatusCls(status, element, value);
                    _self.fire("itemstatuschange", {
                        item: item,
                        status: status,
                        value: value,
                        element: element
                    });
                }
                if (status === "selected") {
                    _self.afterSelected(item, value, element);
                }
            }
        },
        /**
		 * 设置选中状态
		 * @param {Object} item
		 * @param {Object} selected
		 * @param {Object} element
		 */
        setItemSelectedStatus: function (item, selected, element) {
            var _self = this;
            element = element || _self.findElement(item);
            _self.setItemStatus(item, "selected", selected, element);
        },
        /**
		 * 更新项目
		 * @param {Object} item
		 */
        updateItem: function (item) {
            var _self = this,
                items = _self.getItems(),
                index = BUI.Array.indexOf(item, items),
                element = null,
                tpl;
            if (index >= 0) {
                element = _self.findElement(item);
                tpl = _self.getItemTpl(item, index);
                if (element) {
                    $(element).html(tpl);
                }
            }
            _self.fire("itemupdated", {
                item: item,
                domTarget: $(element)[0],
                element: element
            });
        },
        /**
		 * 选项是否存在某种状态
		 * @param {Object} item
		 * @param {Object} status
		 * @param {Object} elem
		 */
        hasStatus: function (item, status, elem) {
            if (!item) {
                return false;
            }
            var _self = this,
                elem = elem || _self.findElement(item);
            var cls = _self.getItemStatusCls(status);
            return $(elem).hasClass(cls);

        },
        /**
		 * 通过列表获取对应dom
		 * @param {Object} item
		 */
        findElement: function (item) {
            var _self = this,
                elements = _self.getAllElements(),
                result = null;
            if (BUI.isString(item)) {
                item = _self.getItem(item);
            }
            BUI.each(elements,
                function (element) {
                    if (_self.getItemByElement(element) == item) {
                        result = element;
                        return false;
                    }
                });
            return result;
        },
        /**
		 * 通过特定字段获取列表
		 * @param {Object} field
		 * @param {Object} value
		 */
        findItemByField: function (field, value) {
            var _self = this,
                items = _self.get("items"),
                result = null;
            BUI.each(items,
                function (item) {
                    if (item[field] != null && item[field] == value) {
                        result = item;
                        return false;
                    }
                });
            return result;
        },
        /**
         * 判断列表项是否被选中
         * @param {Object} item
         * @param {Object} element
         */
        isItemSelected: function (item, element) {
            var _self = this,
                cls = _self.getItemStatusCls("selected");
            element = element || _self.findElement(item);
            return element && $(element).hasClass(cls);
        },
        /**
		 * 列表是否禁用
		 * @param {Object} item
		 * @param {Object} element
		 */
        isItemDisabled: function (item, element) {
            return this.hasStatus(item, "disabled", element);
        },
        /**
		 * 列表是否可以选择
		 * @param {Object} item
		 */
        isItemSelectable: function (item) {
            return true;
        },
        indexOfItem: function (item) {
            return BUI.Array.indexOf(item, this.getItems());
        },
        /**
		 * 获取列表容器
		 */
        getItemContainer: function () {
            var container = this.get("itemContainer");
            if (container.length) {
                return container;
            }
            return this.get("el");
        },
        /**
		 * 获取列表项对应状态的样式
		 * @param {Object} name
		 */
        getItemStatusCls: function (name) {
            return getItemStatusCls(name, this);
        },
        /**
		 * 获取列表模版
		 * @param {Object} item
		 * @param {Object} index
		 */
        getItemTpl: function (item, index) {
            var _self = this,
                render = _self.get("itemTplRender"),
                itemTpl = _self.get("itemTpl");
            if (render) {
                return render(item, index);
            }
            return BUI.substitute(itemTpl, item);
        },
        /**
		 * 获取所有列表dom
		 */
        getAllElements: function () {
            var _self = this,
                itemCls = _self.get("itemCls"),
                el = _self.get("el");
            return el.find("." + itemCls);
        },
        /**
		 * 通过状态获取第一个dom
		 * @param {Object} name
		 */
        getFirstElementByStatus: function (name) {
            var _self = this,
                cls = _self.getItemStatusCls(name),
                el = _self.get("el");
            return el.find("." + cls)[0];
        },
        /**
		 * 通过样式查找DOM元素
		 * @param {Object} status
		 */
        getElementsByStatus: function (status) {
            var _self = this,
                cls = _self.getItemStatusCls(status),
                el = _self.get("el");
            return el.find("." + cls);
        },
        /**
		 * 通过样式查找DOM元素
		 */
        getSelectedElements: function () {
            var _self = this,
                cls = _self.getItemStatusCls("selected"),
                el = _self.get("el");
            return el.find("." + cls);
        },
        /**
		 * 设置列表项选中
		 * @param {Object} name
		 * @param {Object} elem
		 * @param {Object} value
		 */
        setItemStatusCls: function (name, elem, value) {
            var _self = this,
                cls = _self.getItemStatusCls(name),
                method = value ? "addClass" : "removeClass";
            if (elem) {
                $(elem)[method](cls);
            }
        },
        afterSelected: function (item, selected, elem) {
            var _self = this;
            if (selected) {
                _self.fire("itemselected", {
                    item: item,
                    domTarget: elem
                });
                _self.fire("selectedchange", {
                    item: item,
                    domTarget: elem,
                    selected: selected
                });
            } else {
                _self.fire("itemunselected", {
                    item: item,
                    domTarget: elem
                });
                if (_self.get("multipleSelect")) {
                    _self.fire("selectedchange", {
                        item: item,
                        domTarget: elem,
                        selected: selected
                    });
                }
            }
        }
    }, {
            ATTRS: {
                items: {
                    shared: false
                },
                /**
                 * 主键字段
                 */
                idField: {
                    value: "value"
                },
                /**
                 * 列表项的默认模板。
                 */
                itemTpl: {},
                /**
                 * 列表项渲染函数
                 */
                itemTplRender: {},
                itemStatusCls: {
                    value: {}
                },
                multipleSelect: {},
                selectedEvent: {
                    value: "click"
                },
                dataField: {
                    value: "data-item"
                },
                /**
                 *  选项所在容器，如果未设定，使用 el
                 */
                itemContainer: {},
                /**
                 * 选项状态对应的选项值
                 * 
                 *   - 此字段用于将选项记录的值跟显示的DOM状态相对应
                 *   - 例如：下面记录中 <code> checked : true </code>，可以使得此记录对应的DOM上应用对应的状态(默认为 'list-item-checked')
                 *     <pre><code>{id : '1',text : 1,checked : true}</code></pre>
                 *   - 当更改DOM的状态时，记录中对应的字段属性也会跟着变化
                 * <pre><code>
                 *   var list = new List.SimpleList({
                 *   render : '#t1',
                 *   idField : 'id', //自定义样式名称
                 *   itemStatusFields : {
                 *     checked : 'checked',
                 *     disabled : 'disabled'
                 *   },
                 *   items : [{id : '1',text : '1',checked : true},{id : '2',text : '2',disabled : true}]
                 * });
                 * list.render(); //列表渲染后，会自动带有checked,和disabled对应的样式
                 *
                 * var item = list.getItem('1');
                 * list.hasStatus(item,'checked'); //true
                 *
                 * list.setItemStatus(item,'checked',false);
                 * list.hasStatus(item,'checked');  //false
                 * item.checked;                    //false
                 * 
                 * </code></pre>
                 * ** 注意 **
                 * 此字段跟 {@link #itemStatusCls} 一起使用效果更好，可以自定义对应状态的样式
                 * @cfg {Object} itemStatusFields
                 */
                itemStatusFields: {
                    value: {}
                },
                /**
                 * 项的样式，用来获取子项
                 */
                itemCls: {},
                /**
                 * 是否允许取消选中，在多选情况下默认允许取消，单选情况下不允许取消,注意此属性只有单选情况下生效
                 */
                cancelSelected: {
                    value: false
                },
                /**
                 * 获取项的文本，默认获取显示的文本
                 */
                textGetter: {},
                events: {
                    value: {
                        itemrendered: true,
                        itemremoved: true,
                        itemupdated: true,
                        itemclick: false,
                        itemsshow: false,
                        beforeitemsshow: false,
                        itemsclear: false,
                        itemdblclick: false,
                        beforeitemsclear: false,
                        selectedchange: false,
                        beforeselectedchange: false,
                        itemselected: false,
                        itemunselected: false
                    }
                }
            }
        }, {
            xclass: "domlist"
        });
    BUI.List.DomList = domList;
})(window.BUI, jQuery);
//BUI.List.ButtonGroup +
(function (BUI, $) {
    "use strict";
    var UIBase = BUI.Component.UIBase,
        DomList = BUI.List.DomList,
        CLS_ITEM = "btn";
    var ButtonGroup = DomList.extend({},
        {
            ATTRS: {
                focusable: {
                    value: false
                },
                elCls: {
                    value: "btn-group"
                },
                items: {

                },
                itemCls: {
                    value: CLS_ITEM
                },
                idField: {
                    value: "value"
                },
                itemTpl: {
                    value: '<button type="button" class="' + CLS_ITEM + ' btn-default">{text}</button>'
                },
                itemStatusCls: {
                    value: {
                        selected: 'active',
                        disabled: 'disabled'
                    }
                },
                itemContainer: {
                    valueFn: function () {
                        return this.get("el");
                    }
                }
            }
        }, {
            xclass: "button-group",
            prority: 0
        });
    BUI.List.ButtonGroup = ButtonGroup;
})(window.BUI, jQuery);
//BUI.List.ListGroup +
(function (BUI, $) {
    "use strict";
    var UIBase = BUI.Component.UIBase,
        DomList = BUI.List.DomList,
        CLS_ITEM = "list-group-item";
    var ListGroup = DomList.extend({},
        {
            ATTRS: {
                focusable: {
                    value: false
                },
                elCls: {
                    value: "list-group"
                },
                items: {

                },
                elTagName: {
                    value: "ul"
                },
                itemCls: {
                    value: CLS_ITEM
                },
                idField: {
                    value: "value"
                },
                itemTpl: {
                    value: '<li class="' + CLS_ITEM + '">{text}</li>'
                },
                itemStatusCls: {
                    value: {
                        selected: 'active',
                        disabled: 'disabled'
                    }
                },
                itemStatusFields: {
                    value: {
                        selected: 'active',
                        disabled: 'disabled'
                    }
                },
                itemContainer: {
                    valueFn: function () {
                        return this.get("el");
                    }
                }
            }
        }, {
            xclass: "list-group",
            prority: 0
        });
    BUI.List.ListGroup = ListGroup;
})(window.BUI, jQuery);
//BUI.List.DropdownMenu +
(function (BUI, $) {
    "use strict";
    var UIBase = BUI.Component.UIBase,
        DomList = BUI.List.DomList,
        CLS_ITEM = "dropdown-item";
    var DropdownMenu = DomList.extend({  /**
         * 设置元素显示隐藏
         */
        _uiSetVisible: function (isVisible) {
            var self = this,
                el = self.get("el");
            if (isVisible) {
                el.addClass("show");
            } else {
                el.removeClass("show");
            }
        }
    },
        {
            ATTRS: {
                focusable: {
                    value: false
                },
                elCls: {
                    value: "dropdown-menu"
                },
                items: {

                },
                visible: {
                    sync: false,
                    value: false
                },
                itemCls: {
                    value: CLS_ITEM
                },
                idField: {
                    value: "value"
                },
                itemTpl: {
                    value: '<li class="' + CLS_ITEM + '"><a href="###">{text}</a></li>'
                },
                elTagName: {
                    value: "ul"
                },
                itemStatusCls: {
                    value: {
                        selected: 'active',
                        disabled: 'disabled'
                    }
                },
                itemStatusFields: {
                    value: {
                        selected: 'active',
                        disabled: 'disabled'
                    }
                },
                itemContainer: {
                    valueFn: function () {
                        return this.get("el");
                    }
                }
            }
        }, {
            xclass: "dropdown-menu",
            prority: 0
        });
    BUI.List.DropdownMenu = DropdownMenu;
})(window.BUI, jQuery);

////BUI.List.SimpleList +
//(function (BUI, $) {
//    "use strict";
//    var UIBase = BUI.Component.UIBase,
//        DomList = BUI.List.DomList,
//        CLS_ITEM = BUI.prefix + "list-item";
//    var simpleList = DomList.extend({
//        bindUI: function () {
//            var _self = this,
//                itemCls = _self.get("itemCls"),
//                itemContainer = _self.getItemContainer();
//            itemContainer.delegate("." + itemCls, "mouseover",
//                function (ev) {
//                    if (_self.get("disabled")) {
//                        return;
//                    }
//                    var element = ev.currentTarget,
//                        item = _self.getItemByElement(element);
//                    if (_self.isItemDisabled(ev.item, ev.currentTarget)) {
//                        return;
//                    }
//                    _self.setItemStatus(item, "hover", true, element);

//                }).delegate("." + itemCls, "mouseout",
//                    function (ev) {
//                        if (_self.get("disabled")) {
//                            return;
//                        }
//                        var element = $(ev.currentTarget);
//                        _self.setItemStatusCls("hover", element, false);
//                    });
//        }
//    }, {
//            ATTRS: {
//                focusable: {
//                    value: false
//                },
//                items: {

//                },
//                itemCls: {
//                    value: CLS_ITEM
//                },
//                idField: {
//                    value: "value"
//                },
//                listSelector: {
//                    value: "ul"
//                },
//                itemTpl: {
//                    value: '<li class="' + CLS_ITEM + '">{text}</li>'
//                },
//                tpl: {
//                    value: '<ul></ul>'
//                },
//                itemContainer: {
//                    valueFn: function () {
//                        return this.get("el").find(this.get("listSelector"));
//                    }
//                }
//            }
//        }, {
//            xclass: "simple-list",
//            prority: 0
//        });
//    BUI.List.SimpleList = simpleList;
//})(window.BUI, jQuery);
//BUI.Tab.TabPanel
(function (BUI, $) {
    "use strict";
    BUI.Tab = {};
    var List = BUI.List,
    Component = BUI.Component;
    var tabPanel = List.ChildList.extend({
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
                value: "ul"
            },
            tpl: {
                value: '<div class="bui-tab"><ul class="bui-tab-title"></ul><div class="bui-tab-content"></div></div>'
            },
            panelTpl: {
                value: '<div></div>'
            },
            panelContainer: {
                value: ".bui-tab-content"
            },
            defaultChildClass: {
                value: "tab-panel-item"
            }
        }
    },
    {
        xclass: "tab-panel"
    });
    BUI.Tab.TabPanel = tabPanel;
})(window.BUI, jQuery);
//BUI.Tab.TabPanelItem
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
    Close = Component.UIBase.Close,
    CLS_TITLE = "bui-tab-item-text";
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
            if (panel) {
                $(panel)[method]();
            }
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
            elTagName: {
                value: "li"
            },
            tpl: {
                value: '<span class="' + CLS_TITLE + '">{title}</span>'
            },
            selectable: {
                value: true
            },
            panel: {},
            panelContent: {},
            panelVisibleStatus: {
                value: "selected"
            }
        }
    },
    {
        xclass: "tab-panel-item"
    });
    BUI.Tab.TabPanelItem = item;
})(window.BUI, jQuery);
//BUI.Tab.NavTabItem
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
    CLS_ITEM_CLOSE = "bui-tab-close",
    CLS_NAV_ACTIVED = "bui-tab-nav-item-selected",
    CLS_CONTENT = "tab-content";
    var navTabItem = Component.Controller.extend({
        createDom: function () {
            var _self = this,
            parent = _self.get("parent");
            if (parent) {
                _self.set("tabContentContainer", parent.getTabContentContainer());
            }
        },
        renderUI: function () {
            var _self = this,
            contentContainer = _self.get("tabContentContainer"),
            contentTpl = _self.get("tabContentTpl");
            if (contentContainer) {
                var tabContentEl = $(contentTpl).appendTo(contentContainer);
                _self.set("tabContentEl", tabContentEl);
            }
        },
        bindUI: function () {
            var _self = this,
            el = _self.get("el");
            el.on("click",
            function (ev) {
                var sender = $(ev.target);
                if (sender.hasClass(CLS_ITEM_CLOSE)) {
                    if (_self.fire("closing") !== false) {
                        _self.close();
                    }
                } else {
                    _self.fire("itemclick");
                }
            });
        },
        destructor: function () {
            var _self = this,
            tabContentEl = _self.get("tabContentEl");
            if (tabContentEl) {
                tabContentEl.remove();
            }
        },
        setTabContentVisible: function (v) {
            var _self = this,
            tabContentEl = _self.get("tabContentEl");
            if (tabContentEl) {
                if (v) {
                    tabContentEl.show();
                } else {
                    tabContentEl.hide();
                }
            }
        },
        close: function () {
            this.fire("closed");
        },
        _uiSetHref: function (href) {
            var _self = this, tabContentEl = _self.get("tabContentEl");
            href = href || _self.get("href");
            if (tabContentEl) {
                $("iframe", tabContentEl).attr("src", href);
            }
        },
        _uiSetSelected: function (v) {
            var _self = this,
            parent = _self.get("parent"),
            el = _self.get("el");
            _self.setTabContentVisible(v);
            if (v) {
                el.addClass(CLS_NAV_ACTIVED);
            } else {
                el.removeClass(CLS_NAV_ACTIVED);
            }
        },
        _uiSetCloseable: function (v) {
            var _self = this,
            el = _self.get("el"),
            closeEl = el.find("." + CLS_ITEM_CLOSE);
            if (v) {
                closeEl.show();
            } else {
                closeEl.hide();
            }
        }
    },
    {
        ATTRS: {
            elTagName: {
                value: "li"
            },
            selected: {
                value: false
            },
            closeable: {
                value: true
            },
            href: {
                value: ""
            },
            events: {
                value: {
                    click: true,
                    closing: true,
                    closed: true,
                    itemclick: true
                }
            },
            tabContentContainer: {},
            tabContentTpl: {
                value: '<div class="' + CLS_CONTENT + '" style="display:none;"><iframe src="" width="100%" height="100%" frameborder="0"></iframe></div>'
            },
            visible: {
                value: true
            },
            title: {
                value: ""
            },
            tpl: {
                value: '<span>{title}<i class="iconfont icon-close bui-tab-close"></i></span>'
            }
        }
    },
    {
        xclass: "nav-tab-item",
        priority: 0
    });
    BUI.Tab.NavTabItem = navTabItem;
})(window.BUI, jQuery);
//BUI.Tab.NavTab
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
    CLS_NAV_LIST = "bui-tab-title";
    var navTab = Component.Controller.extend({
        addTab: function (config, reload) {
            var _self = this,
            id = config.id || BUI.guid("tab-item"),
            item = _self.getItemById(id);
            if (item) {
                _self._setItemSelected(item);
            } else {
                config = BUI.mix({
                    id: id,
                    visible: true,
                    seleted: true,
                    xclass: "nav-tab-item"
                },
                config);
                _self.addChild(config);
                item = _self.getItemById(id);
                _self._setItemSelected(item);
            }
            return item;
        },
        getTabContentContainer: function () {
            return this.get("el").find(".bui-tab-content");
        },
        getContentElement: function () {
            var _self = this,
            listEl = _self.get("listEl");
            if (!listEl) {
                var el = _self.get("el");
                listEl = el.find("." + CLS_NAV_LIST);
                _self.setInternal("listEl", listEl);
            }

            return listEl;
        },
        bindUI: function () {
            var _self = this;
            _self.on("itemclick",
            function (ev) {
                var item = ev.target;
                _self._setItemSelected(item);
            });
            _self.on("closed",
            function (ev) {
                var item = ev.target;
                _self._closeItem(item);
            });
        },
        setSelected: function (id) {
            var _self = this,
            item = _self.getItemById(id);
            _self._setItemSelected(item);
        },
        getSelectedItem: function () {
            var _self = this,
            children = _self.get("children"),
            result = null;
            BUI.each(children,
            function (item) {
                if (item.get("selected")) {
                    result = item;
                    return false;
                }
            });
            return result;
        },
        getItemById: function (id) {
            var _self = this,
            children = _self.get("children"),
            result = null;
            BUI.each(children,
            function (item) {
                if (item.get("id") === id) {
                    result = item;
                    return false;
                }
            });
            return result;
        },
        _closeItem: function (item) {
            var _self = this,
            index = _self._getIndex(item),
            selectedItem = _self.getSelectedItem(),
            preItem = _self.get("preItem") || _self._getItemByIndex(index - 1),
            nextItem = _self._getItemByIndex(index + 1);

            _self.removeChild(item, true);
            if (selectedItem === item) {
                if (preItem) {
                    _self._setItemSelected(preItem);
                } else {
                    _self._setItemSelected(nextItem);
                }
            }
            return false;
        },
        closeAll: function () {
            var _self = this,
            children = _self.get("children");
            BUI.each(children,
            function (item) {
                if (item.get("closeable")) {
                    item.close();
                }
            });
        },
        closeOther: function (curItem) {
            var _self = this,
            children = _self.get("children");
            BUI.each(children,
            function (item) {
                if (curItem !== item) {
                    item.close();
                }
            });
        },
        _getItemByIndex: function (index) {
            var _self = this,
            children = _self.get("children");
            return children[index];
        },
        _getIndex: function (item) {
            var _self = this,
            children = _self.get("children");
            return BUI.Array.indexOf(item, children);
        },
        _uiSetHeight: function (v) {
            var _self = this,
            el = _self.get("el"),
            barEl = el.find(".bui-tab-title"),
            containerEl = _self.getTabContentContainer();
            if (v) {
                containerEl.height(v - barEl.height());
            }
            el.height(v);
        },
        _setItemSelected: function (item) {
            var _self = this,
            preSelectedItem = _self.getSelectedItem();
            if (item === preSelectedItem) {
                return;
            }
            if (preSelectedItem) {
                preSelectedItem.set("selected", false);
            }
            _self.set("preItem", preSelectedItem);
            if (item) {
                if (!item.get("selected")) {
                    item.set("selected", true);
                }
                _self.fire("activeChange", {
                    item: item
                });
                _self.fire("selectedchange", {
                    item: item
                });
            }
        },
        syncFit: function () {
            var _self = this,
                render = _self.get("render"),
                pHeight = $(render).height();
            _self.set("height", pHeight - 5);
        }
    },
    {
        ATTRS: {
            defaultChildClass: {
                value: "nav-tab-item"
            },
            tpl: {
                value: '<div class="bui-tab"><ul class="bui-tab-title"></ul><div class="bui-tab-content"></div></div>'
            },
            events: {
                value: {
                    itemclick: false,
                    selectedchange: false
                }
            }
        }
    },
    {
        xclass: "nav-tab",
        priority: 0
    });
    BUI.Tab.NavTab = navTab;
})(window.BUI, jQuery);
//BUI.Pagination.BarItem  +
(function (BUI, $) {
    "use strict";
    BUI.Pagination = {};
    var BarItem = BUI.Component.Controller.extend({
        renderUI: function () {
            var el = this.get("el");
            if (!el.attr("id")) {
                el.attr("id", this.get("id"));
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
        }
    },
        {
            ATTRS: {
                selected: {
                    sync: false,
                    value: false
                },
                elTagName: {
                    value: "li"
                },

                focusable: {
                    value: false
                },
                tpl: {
                    value: "<span>{text}</span>"
                }
            }
        },
        {
            xclass: "bar-item",
            priority: 1
        });
    var ButtonBarItem = BarItem.extend({
        _uiSetDisabled: function (value) {
            var _self = this,
                el = _self.get("el"),
                method = value ? "addClass" : "removeClass";
            el[method]("disabled");
            el.children("a").attr("disabled", value)[method]("disabled");
        },
        _uiSetText: function (v) {
            var _self = this,
                el = _self.get("el");
            el.children("a").text(v);
        },
        _uiSetBtnCls: function (v) {
            var _self = this,
                el = _self.get("el");
            el.children("a").addClass(v);
        }
    },
        {
            ATTRS: {
                tpl: {
                    value: '<a href="#" class="{btnCls}">{text}</a >'
                },
                btnCls: {
                    value: "page-link"
                },
                text: {
                    sync: false,
                    value: ""
                }
            }
        },
        {
            xclass: "bar-item-button",
            priority: 2
        });
    var TextBarItem = BarItem.extend({
        _uiSetText: function (text) {
            var _self = this,
                el = _self.get("el");
            el.html(text);
        }
    },
        {
            ATTRS: {
                text: {
                    value: ""
                }
            }
        },
        {
            xclass: "bar-item-text",
            priority: 2
        });
    BarItem.types = {
        button: ButtonBarItem,
        text: TextBarItem
    };
    BUI.Pagination.BarItem = BarItem;
})(window.BUI, jQuery);
//BUI.Pagination.PagingBar  +
(function (BUI, $) {
    "use strict";
    var ChildList = BUI.List.ChildList,
        Component = BUI.Component;
    var PREFIX = BUI.prefix,
        ID_FIRST = "first",
        ID_PREV = "prev",
        ID_NEXT = "next",
        ID_LAST = "last",
        ID_SKIP = "skip",
        ID_REFRESH = "refresh",
        ID_TOTAL_PAGE = "totalPage",
        ID_CURRENT_PAGE = "curPage",
        ID_TOTAL_COUNT = "totalCount";
    var PagingBar = ChildList.extend({
        initializer: function () {
            var _self = this,
                children = _self.get("children"),
                store = _self.get("store");
            if (children.length < 1) {
                var items = _self._getItems();
                BUI.each(items,
                    function (item) {
                        children.push(item);
                    });
            }
            if (store && store.get("pageSize")) {
                _self.set("pageSize", store.get("pageSize"));
            }
        },
        bindUI: function () {
            var _self = this,
                store = _self.get("store");
            if (!store) {
                return;
            }
            store.on("load",
                function (e) {
                    _self.onLoad(e);
                });
            _self._bindButtonEvent();
        },
        _bindButtonEvent: function () {
            var _self = this;
            _self._bindButtonItemEvent(ID_FIRST,
                function () {
                    _self.jumpToPage(1);
                });
            _self._bindButtonItemEvent(ID_PREV,
                function () {
                    _self.jumpToPage(_self.get("curPage") - 1);
                });
            _self._bindButtonItemEvent(ID_NEXT,
                function () {
                    _self.jumpToPage(_self.get("curPage") + 1);
                });
            _self._bindButtonItemEvent(ID_LAST,
                function () {
                    _self.jumpToPage(_self.get("totalPage"));
                });
            _self._bindButtonItemEvent(ID_SKIP,
                function () {
                    handleSkip();
                });
            _self._bindButtonItemEvent(ID_REFRESH,
                function () {
                    _self.jumpToPage(_self.get("curPage"));
                });
            var curPage = _self.getChild(ID_CURRENT_PAGE);
            if (curPage) {
                curPage.get("el").on("keydown",
                    function (event) {
                        event.stopPropagation();
                        if (event.keyCode === 13) {
                            handleSkip();
                        }
                    });
            }
            function handleSkip() {
                var value = parseInt(_self._getCurrentPageValue(), 10);
                if (_self._isPageAllowRedirect(value)) {
                    _self.jumpToPage(value);
                } else {
                    _self._setCurrentPageValue(_self.get("curPage"));
                }
            }
        },
        _bindButtonItemEvent: function (id, func) {
            var _self = this,
                item = _self.getChild(id);
            if (item) {
                item.on("click", func);
            }
        },
        onLoad: function (params) {
            var _self = this,
                store = _self.get("store");
            _self._afterStoreLoad(store, params);
        },
        _afterStoreLoad: function (store, params) {
            var _self = this,
                pageSize = _self.get("pageSize"),
                pageIndex = store.get("pageIndex"),
                start = 0,
                end,
                totalCount,
                curPage,
                totalPage;
            start = (pageIndex - 1) * pageSize + 1;
            totalCount = store.getTotalCount();
            end = totalCount - start > pageSize ? start + store.getCount() - 1 : totalCount;
            totalPage = parseInt((totalCount + pageSize - 1) / pageSize, 10);
            totalPage = totalPage > 0 ? totalPage : 1;
            _self.set("start", start);
            _self.set("end", end);
            _self.set("totalCount", totalCount);
            _self.set("curPage", pageIndex);
            _self.set("totalPage", totalPage);
            _self._setAllButtonsState();
            _self._setNumberPages();
        },
        jumpToPage: function (page) {
            if (page <= 0 || page > this.get("totalPage")) {
                return;
            }
            var _self = this,
                store = _self.get("store"),
                pageSize = _self.get("pageSize");
            var result = _self.fire("beforepagechange", {
                from: _self.get("curPage"),
                to: page
            });
            if (store && result !== false) {
                store.load({
                    pageSize: pageSize,
                    pageIndex: page
                });
            }
        },
        _getItems: function () {
            var _self = this,
                items = [];
            items.push(_self._getButtonItem(ID_FIRST));
            items.push(_self._getButtonItem(ID_PREV));
            items.push(_self._getTextItem(ID_TOTAL_PAGE));
            items.push(_self._getTextItem(ID_CURRENT_PAGE));
            items.push(_self._getButtonItem(ID_SKIP));
            items.push(_self._getButtonItem(ID_NEXT));
            items.push(_self._getButtonItem(ID_LAST));
            items.push(_self._getTextItem(ID_TOTAL_COUNT));
            return items;
        },
        _getButtonItem: function (id) {
            var _self = this;
            return {
                id: id,
                xclass: "bar-item-button",
                text: _self.get(id + "Text"),
                disabled: true,
                elCls: id + "Cls"
            };
        },
        _getTextItem: function (id) {
            var _self = this;
            return {
                id: id,
                xclass: "bar-item-text",
                text: _self._getTextItemTpl(id),
                elCls: "bar-item-" + id
            };
        },
        _getTextItemTpl: function (id) {
            var _self = this,
                obj = _self.getAttrVals();
            return BUI.substitute(this.get(id + "Tpl"), obj);
        },
        _isPageAllowRedirect: function (value) {
            var _self = this;
            return value && value > 0 && value <= _self.get("totalPage") && value !== _self.get("curPage");
        },
        _setAllButtonsState: function () {
            var _self = this,
                store = _self.get("store");
            if (store) {
                _self._setButtonsState([ID_PREV, ID_NEXT, ID_FIRST, ID_LAST, ID_SKIP], true);
            }
            if (_self.get("curPage") === 1) {
                _self._setButtonsState([ID_PREV, ID_FIRST], false);
            }
            if (_self.get("curPage") === _self.get("totalPage")) {
                _self._setButtonsState([ID_NEXT, ID_LAST], false);
            }
        },
        _setButtonsState: function (buttons, enable) {
            var _self = this,
                children = _self.get("children");
            BUI.each(children,
                function (child) {
                    if (BUI.Array.indexOf(child.get("id"), buttons) !== -1) {
                        child.set("disabled", !enable);
                    }
                });
        },
        _setNumberPages: function () {
            var _self = this,
                items = _self.getChildren();
            BUI.each(items,
                function (item) {
                    if (item.__xclass === "bar-item-text") {
                        item.set("content", _self._getTextItemTpl(item.get("id")));
                    }
                });
        },
        _getCurrentPageValue: function (curItem) {
            var _self = this;
            curItem = curItem || _self.getChild(ID_CURRENT_PAGE);
            if (curItem) {
                var textEl = curItem.get("el").find("input");
                return textEl.val();
            }
        },
        _setCurrentPageValue: function (value, curItem) {
            var _self = this;
            curItem = curItem || _self.getChild(ID_CURRENT_PAGE);
            if (curItem) {
                var textEl = curItem.get("el").find("input");
                textEl.val(value);
            }
        }
    },
        {
            ATTRS: {
                firstText: {
                    value: "首 页"
                },
                prevText: {
                    value: "上一页"
                },
                nextText: {
                    value: "下一页"
                },
                lastText: {
                    value: "末 页"
                },
                skipText: {
                    value: "确 定"
                },
                refreshText: {
                    value: "刷 新"
                },
                totalPageTpl: {
                    value: "共 {totalPage} 页"
                },
                curPageTpl: {
                    value: '第 <input type="text" ' + 'autocomplete="off" class="form-control ' + PREFIX + 'pb-page" size="20" value="{curPage}" name="inputItem"> 页'
                },
                totalCountTpl: {
                    value: "共{totalCount}条记录"
                },
                curPage: {
                    value: 0
                },
                totalPage: {
                    value: 0
                },
                totalCount: {
                    value: 0
                },
                pageSize: {
                    value: 20
                },
                elCls: {
                    value: ""
                },
                defaultChildClass: {
                    value: "bar-item"
                },
                elTagName: {
                    value: "ul"
                },
                idField: {
                    value: "id"
                },
                store: {}
            }
        },
        {
            xclass: "pagingbar",
            priority: 2
        });
    BUI.Pagination.PagingBar = PagingBar;
})(window.BUI, jQuery);
//BUI.Pagination.NumberPagingBar   +
(function (BUI, $) {
    "use strict";
    var DomList = BUI.List.DomList,
        Component = BUI.Component;
    var PREFIX = BUI.prefix,
        CLS_ITEM = "page-item",
        ID_PREV = "prev",
        ID_NEXT = "next";
    var NumberPagingBar = DomList.extend({
        initializer: function () {
            var _self = this,
                store = _self.get("store");
            if (store && store.get("pageSize")) {
                _self.set("pageSize", store.get("pageSize"));
            }
        },
        bindUI: function () {
            var _self = this,
                store = _self.get("store");
            if (!store) {
                return;
            }
            store.on("load",
                function (e) {
                    _self.onLoad(e);
                });
            _self._bindButtonEvent();
        },
        _bindButtonEvent: function () {
            var _self = this;

            _self.on('itemclick', function (ev) {
                var item = ev.item;
                if (item.value == ID_NEXT) {
                    _self.jumpToPage(_self.get("curPage") + 1);
                } else if (item.value == ID_PREV) {
                    _self.jumpToPage(_self.get("curPage") - 1);
                } else if (item.value == 'ellipsis') {
                    return false;
                }
                else {
                    _self.jumpToPage(item.value);
                }
                return false;
            });
        },
        onLoad: function (params) {
            var _self = this,
                store = _self.get("store");
            _self._afterStoreLoad(store, params);
        },
        _afterStoreLoad: function (store, params) {
            var _self = this,
                pageSize = _self.get("pageSize"),
                pageIndex = store.get("pageIndex"),
                start = 0,
                end,
                totalCount,
                curPage,
                totalPage;
            start = (pageIndex - 1) * pageSize + 1;
            totalCount = store.getTotalCount();
            end = totalCount - start > pageSize ? start + store.getCount() - 1 : totalCount;
            totalPage = parseInt((totalCount + pageSize - 1) / pageSize, 10);
            totalPage = totalPage > 0 ? totalPage : 1;
            _self.set("start", start);
            _self.set("end", end);
            _self.set("totalCount", totalCount);
            _self.set("curPage", pageIndex);
            _self.set("totalPage", totalPage);
            _self._setNumberPages();
            _self._setAllButtonsState();
        },
        jumpToPage: function (page) {
            if (page <= 0 || page > this.get("totalPage")) {
                return;
            }
            var _self = this,
                store = _self.get("store"),
                pageSize = _self.get("pageSize");
            var result = _self.fire("beforepagechange", {
                from: _self.get("curPage"),
                to: page
            });
            if (store && result !== false) {
                store.load({
                    pageSize: pageSize,
                    pageIndex: page
                });
            }
        },
        _setAllButtonsState: function () {
            var _self = this,
                store = _self.get("store");
            if (_self.get("curPage") === 1) {
                _self._setButtonsState([ID_PREV], true);
            }
            if (_self.get("curPage") === _self.get("totalPage")) {
                _self._setButtonsState([ID_NEXT], true);
            }
        },
        _setButtonsState: function (buttons, enable) {
            var _self = this,
                items = _self.get("items");
            BUI.each(items,
                function (item) {
                    if (BUI.Array.indexOf(item.value, buttons) !== -1) {
                        _self.setItemDisabled(item, enable);
                    }
                });
        },
        _setNumberPages: function () {
            var _self = this,
                curPage = _self.get('curPage'),
                totalPage = _self.get('totalPage'),
                numberItems = _self._getNumberItems(curPage, totalPage),
                curItem,
                items = [];

            items.push({
                value: ID_PREV,
                text: _self.get(ID_PREV + 'Text')
            });
            BUI.each(numberItems, function (item) {
                if (item.value == curPage) {
                    curItem = item;
                }
                items.push(item);
            });
            items.push({
                value: ID_NEXT,
                text: _self.get(ID_NEXT + 'Text')
            });

            _self.set("items", items);
            _self.setSelected(curItem);
        },
        //获取所有页码按钮的配置项
        _getNumberItems: function (curPage, totalPage) {
            var _self = this,
                result = [],
                maxLimitCount = _self.get('maxLimitCount'),
                showRangeCount = _self.get('showRangeCount'),
                maxPage;

            function addNumberItem(from, to) {
                for (var i = from; i <= to; i++) {
                    result.push(_self._getNumberItem(i));
                }
            }

            function addEllipsis() {
                result.push(_self._getEllipsisItem());
            }

            if (totalPage < maxLimitCount) {
                maxPage = totalPage;
                addNumberItem(1, totalPage);
            } else {
                var startNum = (curPage <= maxLimitCount) ? 1 : (curPage - showRangeCount),
                    lastLimit = curPage + showRangeCount,
                    endNum = lastLimit < totalPage ? (lastLimit > maxLimitCount ? lastLimit : maxLimitCount) : totalPage;
                if (startNum > 1) {
                    addNumberItem(1, 1);
                    if (startNum > 2) {
                        addEllipsis();
                    }
                }
                maxPage = endNum;
                addNumberItem(startNum, endNum);
            }

            if (maxPage < totalPage) {
                if (maxPage < totalPage - 1) {
                    addEllipsis();
                }
                addNumberItem(totalPage, totalPage);
            }

            return result;
        },
        //获取省略号
        _getEllipsisItem: function () {
            var _self = this;
            return {
                disabled: true,
                value: "ellipsis",
                text: _self.get('ellipsisText')
            };
        },
        //生成页面按钮配置项
        _getNumberItem: function (page) {
            var _self = this;
            return {
                value: page,
                text: page
            };
        }
    },
        {
            ATTRS: {
                prevText: {
                    value: "上一页"
                },
                nextText: {
                    value: "下一页"
                },
                curPage: {
                    value: 0
                },
                totalPage: {
                    value: 0
                },
                totalCount: {
                    value: 0
                },
                pageSize: {
                    value: 20
                },
                elCls: {
                    value: "pagination"
                },
                itemCls: {
                    value: CLS_ITEM
                },
                itemTpl: {
                    value: '<li class="' + CLS_ITEM + '"><a href="#" class="page-link">{text}</a ></li>'
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
                },
                maxLimitCount: {
                    value: 6
                },
                showRangeCount: {
                    value: 1
                },
                ellipsisText: {
                    value: '...'
                },
                store: {}
            }
        },
        {
            xclass: "pagingnumberbar",
            priority: 2
        });
    BUI.Pagination.NumberPagingBar = NumberPagingBar;
})(window.BUI, jQuery);

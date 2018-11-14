//BUI.Layout.Item
(function (BUI, $) {
    "use strict";
    BUI.Layout = {};
    function parseValue(attrs, value) {
        if (!BUI.isString(value)) {
            return value;
        }
        if (value.indexOf("{") != -1) {
            value = BUI.substitute(value, attrs);
            value = BUI.JSON.looseParse(value);
        }
        return value;
    }
    var Item = function (config) {
        Item.superclass.constructor.call(this, config);
        this.init();
    };
    Item.ATTRS = {
        fit: {
            value: "none"
        },
        layout: {},
        control: {},
        wraperCls: {},
        container: {},
        srcNode: {},
        cssProperties: {
            value: ["width", "height"]
        },
        attrProperties: {},
        statusProperties: {},
        tplProperties: {},
        el: {},
        elCls: {},
        tpl: {}
    };
    BUI.extend(Item, BUI.Base);
    BUI.augment(Item, {
        init: function () {
            var _self = this,
            el = _self._wrapControl();
            _self.set("el", el);
            _self.syncItem();
        },
        _wrapControl: function () {
            var _self = this,
            control = _self.get("control"),
            controlEl = control.get("el"),
            elCls = _self.get("elCls"),
            container = _self._getContainer(controlEl),
            tpl = BUI.substitute(_self.get("tpl"), _self.getLayoutAttrs()),
            node = $(tpl).appendTo(container),
            bodyEl;
            if (elCls) {
                node.addClass(elCls);
            }
            bodyEl = _self.getControlContainer(node);
            controlEl.appendTo(bodyEl);
            _self.set("bodyEl", bodyEl);
            return node;
        },
        getControlContainer: function (el) {
            var _self = this,
            wraperCls = _self.get("wraperCls");
            if (wraperCls) {
                return el.find("." + wraperCls);
            }
            return el;
        },
        syncItem: function (attrs) {
            attrs = attrs || this.getLayoutAttrs();
            var _self = this,
            el = _self.get("el"),
            css = _self._getSyncCss(attrs),
            attr = _self._getSyncAttr(attrs);
            el.css(css);
            el.attr(attr);
            _self.syncStatus(el, attrs);
            _self.syncElements(el, attrs);
            _self.syncFit();
        },
        syncStatus: function (el, attrs) {
            el = el || this.get("el");
            attrs = attrs || this.getLayoutAttrs();
            var _self = this,
            statusProperties = _self.get("statusProperties");
            if (statusProperties) {
                BUI.each(statusProperties,
                function (status) {
                    var value = _self.get(status);
                    if (value != null) {
                        var m = value ? "addClass" : "removeClass",
                        cls = "bui-" + status;
                        el[m](cls);
                    }
                });
            }
        },
        syncElements: function (el, attrs) {
            var _self = this,
            tplProperties = _self.get("tplProperties");
            if (tplProperties) {
                BUI.each(tplProperties,
                function (item) {
                    _self.synTpl(el, item, attrs);
                });
            }
        },
        synTpl: function (el, item, attrs) {
            var _self = this,
            name = item.name,
            elName = "_" + name + "El",
            tpl, m, tplEl = _self.get(elName);
            if (attrs[name]) {
                if (!tplEl) {
                    tpl = _self.get(item.value);
                    tpl = BUI.substitute(tpl, attrs);
                    m = item.prev ? "prependTo" : "appendTo";
                    tplEl = $(tpl)[m](el);
                    _self.set(elName, tplEl);
                }
            } else if (tplEl) {
                tplEl.remove();
            }
        },
        syncFit: function () {
            var _self = this,
            control = _self.get("control"),
            fit = _self.get("fit");
            if (fit === "none") {
                return;
            }
            if (fit === "width") {
                _self._syncControlWidth(control);
                return;
            }
            if (fit === "height") {
                _self._syncControlHeight(control);
                return;
            }
            if (fit === "both") {
                _self._syncControlWidth(control);
                _self._syncControlHeight(control);
            }
        },
        _syncControlWidth: function (control) {
            var _self = this,
            width = _self.get("width") || _self.get("el").width(),
            appendWidth = control.getAppendWidth();
            control.set("width", width - appendWidth);
        },
        _syncControlHeight: function (control) {
            var _self = this,
            height = _self.getFitHeight(),
            appendHeight = control.getAppendHeight();
            control.set("height", height - appendHeight);
        },
        getFitHeight: function () {
            var _self = this,
            el = _self.get("el"),
            bodyEl = _self.get("bodyEl"),
            siblings,
            outerHeight = _self.get("height") || el.height(),
            height = outerHeight;
            if (bodyEl[0] == el[0]) {
                return outerHeight;
            }
            siblings = bodyEl.siblings();
            BUI.each(siblings,
            function (elem) {
                var node = $(elem);
                if (node.css("position") !== "absolute") {
                    height -= node.outerHeight();
                }
            });
            return height;
        },
        getLayoutAttrs: function () {
            return this.getAttrVals();
        },
        _getSyncCss: function (attrs) {
            var _self = this,
            properties = _self.get("cssProperties"),
            dynacAttrs = _self._getDynacAttrs(),
            css = {};
            BUI.each(properties,
            function (p) {
                css[p] = parseValue(dynacAttrs, attrs[p]);
            });
            return css;
        },
        _getDynacAttrs: function () {
            var _self = this,
            container = _self.get("container");
            return {
                width: container.width(),
                height: container.height()
            };
        },
        _getSyncAttr: function (attrs) {
            var _self = this,
            properties = _self.get("attrProperties"),
            attr = {};
            BUI.each(properties,
            function (p) {
                attr[p] = attrs[p];
            });
            return attr;
        },
        _getContainer: function (controlEl) {
            var _self = this,
            container = _self.get("container");
            if (container) {
                return container;
            }
            return controlEl.parent();
        },
        getElement: function () {
            return this.get("el");
        },
        destroy: function () {
            var _self = this;
            _self.get("el").remove();
            _self.off();
            _self.clearAttrVals();
        }
    });
    BUI.Layout.Item = Item;
})(window.BUI, jQuery);
//BUI.Layout.Abstract
(function (BUI, $) {
    "use strict";
    var Item = BUI.Layout.Item,
    Component = BUI.Component;
    var Abstract = Component.Controller.extend({
        initializer: function (control) {
            var _self = this;
            _self.set("control", _self);
        },
        renderUI: function () {
            this._initWraper();
            this.initItems();
        },
        bindUI: function () {
            var _self = this,
            control = _self.get("control"),
            layoutEvents = _self.get("layoutEvents").join(" ");
            control.on("afterAddChild",
            function (ev) {
                var child = ev.child;
                _self.addItem(child);
            });
            control.on("afterRemoveChild",
            function (ev) {
                _self.removeItem(ev.child);
            });
            control.on(layoutEvents,
            function () {
                _self.resetLayout();
            });
            _self.appendEvent(control);
        },
        appendEvent: function (control) { },
        _initWraper: function () {
            var _self = this,
            control = _self.get("control"),
            controlEl = control.get("contentEl");
            _self.set("container", controlEl);
            _self.afterWraper();
        },
        afterWraper: function () { },
        initItems: function () {
            var _self = this,
            control = _self.get("control"),
            items = [],
            controlChildren = control.get("children");
            _self.set("items", items);
            for (var i = 0; i < controlChildren.length; i++) {
                _self.addItem(controlChildren[i]);
            }
        },
        addItem: function (control) {
            var _self = this,
            items = _self.getItems(),
            item = _self.initItem(control);
            items.push(item);
            return item;
        },
        initItem: function (controlChild) {
            var _self = this,
            c = _self.get("itemConstructor"),
            cfg = _self.getItemCfg(controlChild);
            return new c(cfg);
        },
        getItemCfg: function (controlChild) {
            var _self = this,
            defaultCfg = _self.get("defaultCfg"),
            cfg = BUI.mix({},
            defaultCfg, {
                control: controlChild,
                tpl: _self.get("itemTpl"),
                layout: _self,
                wraperCls: _self.get("wraperCls")
            },
            controlChild.get("layout"));
            cfg.container = _self.getItemContainer(cfg);
            return cfg;
        },
        getItemContainer: function (itemAttrs) {
            return this.get('container');
        },
        getNextItem: function (item) {
            var _self = this,
            index = _self.getItemIndex(item),
            count = _self.getCount(),
            next = (index + 1) % count;
            return _self.getItemAt(next);
        },
        removeItem: function (control) {
            var _self = this,
            items = _self.getItems(),
            item = _self.getItem(control);
            if (item) {
                item.destroy();
                BUI.Array.remove(items, item);
            }
        },
        getItemBy: function (fn) {
            var _self = this,
            items = _self.getItems(),
            rst = null;
            BUI.each(items,
            function (item) {
                if (fn(item)) {
                    rst = item;
                    return false;
                }
            });
            return rst;
        },
        getItemsBy: function (fn) {
            var _self = this,
            items = _self.getItems(),
            rst = [];
            BUI.each(items,
            function (item) {
                if (fn(item)) {
                    rst.push(item);
                }
            });
            return rst;
        },
        getItem: function (control) {
            return this.getItemBy(function (item) {
                return item.get("control") == control;
            });
        },
        getCount: function () {
            return this.getItems().length;
        },
        getItemAt: function (index) {
            return this.getItems()[index];
        },
        getItemIndex: function (item) {
            var items = this.getItems();
            return BUI.Array.indexOf(item, items);
        },
        getItems: function () {
            return this.get("items");
        },
        resetLayout: function () {
            var _self = this,
            items = _self.getItems();
            BUI.each(items,
            function (item) {
                item.syncItem();
            });
        },
        clearLayout: function () {
            var _self = this,
            items = _self.getItems();
            BUI.each(items,
            function (item) {
                item.destroy();
            });
        },
        reset: function () {
            this.resetLayout();
        },
        getItemByElement: function (element) {
            return this.getItemBy(function (item) {
                return $.contains(item.get("el")[0], element[0]);
            });
        },
        bindCollapseEvent: function () {
            var _self = this,
            triggerCls = _self.get("triggerCls"),
            el = _self.get("container");
            el.delegate("." + triggerCls, "click",
            function (ev) {
                var sender = $(ev.currentTarget),
                item = _self.getItemByElement(sender);
                _self.toggleCollapse(item);
            });
        },
        //获取展开的item
        getExpandedItem: function () {
            return this.getItemBy(function (item) {
                return !item.get("collapsed");
            });
        },
        //展开item
        expandItem: function (item) {
            var _self = this,
            duration = _self.get("duration"),
            range = _self.getCollapsedRange(item),
            activeItem;
            if (item.get("collapsed")) {
                if (_self.get("accordion")) {
                    activeItem = _self.getExpandedItem();
                    if (activeItem) {
                        _self.beforeCollapsed(activeItem, range);
                        activeItem.collapse(duration,
                        function () {
                            _self.afterCollapsed(activeItem);
                        });
                    }
                }
                _self.beforeExpanded(item, range);
                item.expand(range, duration,
                function () {
                    _self.afterExpanded(item);
                });
            }
        },
        afterExpanded: function (item) { },
        beforeExpanded: function (item, range) { },
        //折叠item
        collapseItem: function (item) {
            var _self = this,
            duration = _self.get("duration"),
            range = _self.getCollapsedRange(item),
            nextItem;
            if (!item.get("collapsed")) {
                if (_self.get("accordion")) {
                    nextItem = _self.getNextItem(item);
                    _self.beforeExpanded(nextItem, range);
                    nextItem.expand(range, duration,
                    function () {
                        _self.afterExpanded(nextItem);
                    });
                }
                _self.beforeCollapsed(item, range);
                item.collapse(duration,
                function () {
                    _self.afterCollapsed(item);
                });
            }
        },
        beforeCollapsed: function (item, range) { },
        afterCollapsed: function (item) { },
        getCollapsedRange: function (item) { },
        toggleCollapse: function (item) {
            var _self = this;
            if (item.get("collapsed")) {
                _self.expandItem(item);
            } else {
                _self.collapseItem(item);
            }
        },
        destroy: function () {
            var _self = this;
            _self.clearLayout();
            _self.off();
            _self.clearAttrVals();
        }
    },
    {
        ATTRS: {
            itemConstructor: {
                value: Item
            },
            control: {},
            layoutEvents: {
                value: ["afterWidthChange", "afterHeightChange"]
            },
            items: {},
            elCls: {},
            defaultCfg: {
                value: {}
            },
            wraperCls: {},
            container: {},
            itemTpl: {
                value: "<div></div>"
            },
            triggerCls: {},
            duration: {
                value: 400
            },
            accordion: {
                value: false
            }
        }
    });
    BUI.Layout.Abstract = Abstract;
})(window.BUI, jQuery);
//BUI.Layout.BorderItem
(function (BUI, $) {
    "use strict";
    var Base = BUI.Layout.Item,
    CLS_COLLAPSED = "bui-collapsed",
    REGINS = {
        NORTH: "north",
        EAST: "east",
        SOUTH: "south",
        WEST: "west",
        CENTER: "center"
    };
    var Border = function (config) {
        Border.superclass.constructor.call(this, config);
    };
    Border.ATTRS = {
        region: {},
        titleTpl: {
            value: '<div class="bui-border-title bui-border-title-{region}">{title}</div>'
        },
        collapseTpl: {
            value: '<s class="bui-collapsed-btn bui-collapsed-{region}"></s>'
        },
        collapsable: {
            value: false
        },
        collapsed: {
            value: false
        },
        leftRange: {
            value: 28
        },
        tplProperties: {
            value: [{
                name: "title",
                value: "titleTpl",
                prev: true
            },
            {
                name: "collapsable",
                value: "collapseTpl",
                prev: true
            }]
        },
        statusProperties: {
            value: ["collapsed"]
        }
    };
    Border.REGINS = REGINS;
    BUI.extend(Border, Base);
    BUI.augment(Border, {
        syncElements: function (el, attrs) {
            Border.superclass.syncElements.call(this, el, attrs);
            var _self = this,
            el = _self.get("el"),
            property = _self.getCollapseProperty();
            if (_self.get("collapsed") && _self.get(property) == el[property]()) {
                _self.collapse(0);
            }
        },
        expand: function (range, duration, callback) {
            var _self = this,
            property = _self.getCollapseProperty(),
            el = _self.get("el"),
            toRange = _self.get(property),
            css = {};
            css[property] = toRange;
            el.css(css);
            _self.set("collapsed", false);
            el.removeClass(CLS_COLLAPSED);
            callback && callback();
        },
        getCollapseProperty: function () {
            var _self = this,
            region = _self.get("region");
            if (region == REGINS.SOUTH || region == REGINS.NORTH) {
                return "height";
            }
            return "width";
        },
        _getLeftRange: function () {
            var _self = this,
            el = _self.get("el"),
            left = _self.get("leftRange");
            return left;
        },
        getCollapsedRange: function () {
            var _self = this,
            property = _self.getCollapseProperty(),
            el = _self.get("el"),
            val = _self.get(property);
            if (BUI.isString(val)) {
                var dynacAttrs = _self._getDynacAttrs();
                if (val.indexOf("{") != -1) {
                    val = BUI.substitute(val, dynacAttrs);
                    val = BUI.JSON.looseParse(val);
                } else if (val.indexOf("%") != -1) {
                    val = parseInt(val, 10) * .01 * dynacAttrs[property];
                } else {
                    val = parseInt(val, 10);
                }
            }
            return val - _self._getLeftRange(property);
        },
        collapse: function (duration, callback) {
            var _self = this,
            property = _self.getCollapseProperty(),
            el = _self.get("el"),
            left = _self._getLeftRange(property),
            css = {};
            css[property] = left;
            el.css(css);
            _self.set("collapsed", true);
            el.addClass(CLS_COLLAPSED);
            callback && callback();
        }
    });
    BUI.Layout.BorderItem = Border;
})(window.BUI, jQuery);
//BUI.Layout.Border
(function (BUI, $) {
    "use strict";
    var Abstract = BUI.Layout.Abstract,
    Item = BUI.Layout.BorderItem,
    CLS_TOP = "bui-border-top",
    CLS_MIDDLE = "bui-border-middle",
    CLS_BOTTOM = "bui-border-bottom",
    REGINS = Item.REGINS;
    var Border = Abstract.extend({
        appendEvent: function () {
            this.bindCollapseEvent();
        },
        afterWraper: function () {
            var _self = this,
            container = _self.get("container"),
            topEl = container.find("." + CLS_TOP),
            middleEl = container.find("." + CLS_MIDDLE),
            bottomEl = container.find("." + CLS_BOTTOM);
            _self.set("topEl", topEl);
            _self.set("middleEl", middleEl);
            _self.set("bottomEl", bottomEl);
        },
        syncUI: function () {
            this._setMiddleDimension();
        },
        _setMiddleDimension: function () {
            var _self = this,
            middleEl = _self.get("middleEl"),
            middleHeight = _self._getMiddleHeight(),
            left = _self._getMiddleLeft(),
            right = _self._getMiddleRight(),
            items = _self.get("items"),
            center = _self.getItemsByRegion("center")[0];
            middleEl.height(middleHeight);
            if (center) {
                var el = center.get("el");
                el.css({
                    marginLeft: left,
                    marginRight: right
                });
            }
            _self._fitMiddleControl();
        },
        _fitMiddleControl: function () {
            var _self = this,
            items = _self.getItems();
            BUI.each(items,
            function (item) {
                var region = item.get("region");
                if (region == REGINS.EAST || region == REGINS.WEST || region == REGINS.CENTER) {
                    item.syncFit();
                }
            });
        },
        _getMiddleHeight: function () {
            var _self = this,
            container = _self.get("container"),
            totalHeight = container.height(),
            middleEl = _self.get("middleEl"),
            topEl = _self.get("topEl"),
            appendHeight,
            middleHeight;
            if (topEl.children().length) {
                middleHeight = totalHeight - topEl.outerHeight() - _self.get("bottomEl").outerHeight();
            } else {
                middleHeight = totalHeight - _self.get("bottomEl").outerHeight();
            }
            appendHeight = middleEl.outerHeight() - middleEl.height();
            return middleHeight - appendHeight;
        },
        getItemsByRegion: function (region) {
            return this.getItemsBy(function (item) {
                return item.get("region") === region;
            });
        },
        _getMiddleLeft: function () {
            var _self = this,
            westItems = _self.getItemsByRegion("west"),
            leftWidth = 0;
            BUI.each(westItems,
            function (item) {
                leftWidth += item.get("el").outerWidth();
            });
            return leftWidth;
        },
        _getMiddleRight: function () {
            var _self = this,
            eastItems = _self.getItemsByRegion("east"),
            rightWidth = 0;
            BUI.each(eastItems,
            function (item) {
                rightWidth += item.get("el").outerWidth();
            });
            return rightWidth;
        },
        getItemContainer: function (itemAttrs) {
            var _self = this,
            rst;
            switch (itemAttrs.region) {
                case REGINS.NORTH:
                    rst = _self.get("topEl");
                    break;

                case REGINS.SOUTH:
                    rst = _self.get("bottomEl");
                    break;

                default:
                    rst = _self.get("middleEl");
                    break;
            }
            return rst;
        },
        beforeExpanded: function (item, range) {
            this.beforeCollapsedChange(item, range, false);
        },
        beforeCollapsedChange: function (item, range, collapsed) {
            var _self = this,
            property = item.getCollapseProperty(),
            factor = collapsed ? 1 : -1,
            duration = _self.get("duration");
            if (property == "height") {
                _self._setMiddleHeight(range * factor, duration);
            } else {
                _self._setCenterWidth(item.get("region"), range * factor * -1, duration);
            }
        },
        _setMiddleHeight: function (range, duration) {
            var _self = this,
            middleEl = _self.get("middleEl"),
            preHeight = middleEl.height(),
            height = preHeight + range;
            middleEl.height(height);
        },
        _setCenterWidth: function (region, range, duration) {
            var _self = this,
            center = _self.getItemsByRegion("center")[0],
            property = region == REGINS.EAST ? "marginRight" : "marginLeft",
            centerEl,
            prev,
            css = {};
            if (center) {
                centerEl = center.get("el");
            }
            prev = parseFloat(centerEl.css(property));
            css[property] = range + prev;
            centerEl.css(css);
        },
        getCollapsedRange: function (item) {
            return item.getCollapsedRange();
        },
        beforeCollapsed: function (item, range) {
            this.beforeCollapsedChange(item, range, true);
        },
        afterExpanded: function () {
            this._fitMiddleControl();
        },
        afterCollapsed: function () {
            this._fitMiddleControl();
        },
        resetLayout: function () {
            var _self = this;
            Border.superclass.resetLayout.call(_self);
            _self._setMiddleDimension();
        }
    },
    {
        ATTRS: {
            layoutEvents: {
                value: ["afterAddChild", "afterRemoveChild"]
            },
            itemConstructor: {
                value: Item
            },
            wraperCls: {
                value: "bui-border-body"
            },
            duration: {
                value: 200
            },
            elCls: {
                value: 'bui-border-layout'
            },
            triggerCls: {
                value: "bui-collapsed-btn"
            },
            tpl: {
                value: '<div class="bui-layout-border"><div class="' + CLS_TOP + '"></div><div class="' + CLS_MIDDLE + '"></div><div class="' + CLS_BOTTOM + '"></div></div>'
            },
            itemTpl: {
                value: '<div class="bui-border-{region} bui-layout-item-border"><div class="bui-border-body"></div></div>'
            },
            childContainer: {
                value: ".bui-layout-border"
            }
        }
    },
    {
        xclass: "layout-border",
    });

    BUI.Layout.Border = Border;
})(window.BUI, jQuery);
//BUI.Layout.Tab
(function (BUI, $) {
    "use strict";
    var CLS_COLLAPSED = "bui-collapsed",
    Base = BUI.Layout.Item;
    var Tab = function (config) {
        Tab.superclass.constructor.call(this, config);
    };
    Tab.ATTRS = {
        collapsed: {
            value: true
        },
        statusProperties: {
            value: ["collapsed"]
        }
    };
    BUI.extend(Tab, Base);
    BUI.augment(Tab, {
        expand: function (bodyHeight, duration) {
            var _self = this,
            el = _self.get("el"),
            bodyEl = _self.get("bodyEl");
            bodyEl.animate({
                height: bodyHeight
            },
            duration,
            function () {
                el.removeClass(CLS_COLLAPSED);
                _self.syncFit();
            });
            _self.set("collapsed", false);
        },
        collapse: function (duration) {
            var _self = this,
            el = _self.get("el"),
            bodyEl = _self.get("bodyEl");
            bodyEl.animate({
                height: 0
            },
            duration,
            function () {
                el.addClass(CLS_COLLAPSED);
            });
            _self.set("collapsed", true);
        }
    });
    BUI.Layout.Tab = Tab;
})(window.BUI, jQuery);
//BUI.Layout.Accordion
(function (BUI, $) {
    "use strict";
    var CLS_ITEM = "bui-layout-item-accordion",
    Abstract = BUI.Layout.Abstract,
    Item = BUI.Layout.Tab;
    var Accordion = Abstract.extend({
        appendEvent: function (control) {
            this.bindCollapseEvent();
        },
        getActivedItem: function () {
            return this.getExpandedItem();
        },
        syncUI: function () {
            this._resetActiveItem();
        },
        _resetActiveItem: function () {
            var _self = this,
            activeItem = _self.getActivedItem() || _self.getItems()[0];
            activeItem.expand(_self.getCollapsedRange(), 0);
        },
        resetLayout: function () {
            var _self = this;
            Accordion.superclass.resetLayout.call(_self);
            _self._resetActiveItem();
        },
        getCollapsedRange: function () {
            var _self = this,
            container = _self.get("container"),
            outerHeight = container.height(),
            titleEls = container.find("." + _self.get("titleCls")),
            bodyHeight = outerHeight;
            BUI.each(titleEls,
            function (element) {
                bodyHeight -= $(element).outerHeight();
            });
            return bodyHeight;
        }
    },
    {
        ATTRS: {
            itemConstructor: {
                value: Item
            },
            wraperCls: {
                value: "bui-accordion-body"
            },
            titleCls: {
                value: "bui-accordion-title"
            },
            triggerCls: {
                value: "bui-accordion-title"
            },
            layoutEvents: {
                value: ["afterAddChild", "afterRemoveChild"]
            },
            duration: {
                value: 400
            },
            accordion: {
                value: true
            },
            itemTpl: {
                value: '<div class="' + CLS_ITEM + '"><div class="bui-accordion-title">{title}<i class="bui-expand-button icon-white"></i></div><div class="bui-accordion-body"></div></div>'
            }
        }
    },
    {
        xclass: "layout-accordion"
    });

    BUI.Layout.Accordion = Accordion;
})(window.BUI, jQuery);
//BUI.Layout.Viewport
(function (BUI, $) {
    "use strict";
    var CLS_VIEW_CONTAINER = "bui-viewport-container",
    win = window;
    var Viewport = BUI.Layout.Border.extend({
        renderUI: function () {
            this.reset();
            var _self = this,
            render = _self.get("render");
            $(render).addClass(CLS_VIEW_CONTAINER);
        },
        bindUI: function () {
            var _self = this;
            $(win).on("resize", BUI.wrapBehavior(_self, "onResize"));
        },
        onResize: function () {
            this.reset();
        },
        reset: function () {
            var _self = this,
            el = _self.get("el"),
            viewportHeight = BUI.viewportHeight(),
            viewportWidth = BUI.viewportWidth(),
            appendWidth = _self.getAppendWidth(),
            appendHeight = _self.getAppendHeight();
            _self.set("width", viewportWidth - appendWidth);
            _self.set("height", viewportHeight - appendHeight);
            _self.fire("resize");
        },
        destructor: function () {
            $(win).off("resize", BUI.getWrapBehavior(this, "onResize"));
        }
    },
    {
        ATTRS: {
            render: {
                value: "body"
            }
        }
    },
    {
        xclass: "view-port"
    });
    BUI.Layout.Viewport = Viewport;
})(window.BUI, jQuery);
////BUI.Layout.BuiItem
//(function (BUI, $) {
//    "use strict";
//    var CLS_COLLAPSED = "bui-collapsed",
//   REGINS = {
//       NORTH: "north",
//       WEST: "west",
//       CENTER: "center"
//   };

//    var BuiItem = function (config) {
//        BuiItem.superclass.constructor.call(this, config);
//        this.init();
//    };

//    BuiItem.ATTRS = {
//        region: {},
//        collapsed: {
//            value: false
//        },
//        cssProperties: {
//            value: ["width", "height"]
//        },
//        leftRange: {
//            value: 28
//        },
//        width: {
//            value: 0
//        },
//        height: {
//            value: 0
//        },
//        layout: {},
//        control: {}
//    };

//    BUI.extend(BuiItem, BUI.Base);

//    BUI.augment(BuiItem, {
//        init: function () {
//            var _self = this,
//            el = _self._wrapControl();
//            _self.set("el", el);
//            _self.syncFit();
//        },
//        _wrapControl: function () {
//            var _self = this,
//            control = _self.get("control"),
//            controlEl = control.get("el");
//            _self.set("region", control.get("region"));
//            _self.set("width", control.get("width"));
//            return controlEl;
//        },
//        syncFit: function () {
//            var _self = this,
//            control = _self.get("control"),
//            region = _self.get("region");

//            if (region === REGINS.WEST) {
//                _self._syncControlHeight(control);
//                return;
//            }
//            if (region === REGINS.CENTER) {
//                _self._syncControlWidth(control);
//                _self._syncControlHeight(control);
//            }
//        },
//        _syncControlWidth: function (control) {
//            var _self = this,
//            width = _self.get("width") || _self.get("container").width(),
//            appendWidth = control.getAppendWidth(),
//            leftWidth = _self.get("layout")._getMiddleLeft();
//            control.set("width", width - appendWidth - leftWidth);
//        },
//        _syncControlHeight: function (control) {
//            var _self = this,
//            height = _self.getFitHeight(),
//            appendHeight = control.getAppendHeight();
//            control.set("height", height - appendHeight);
//        },
//        getFitHeight: function () {
//            var _self = this,
//            el = _self.get("container"),
//            siblings,
//            outerHeight = _self.get("height") || el.height();
//            return outerHeight;
//        },     
//        getElement: function () {
//            return this.get("el");
//        },
//        destroy: function () {
//            var _self = this;
//            _self.get("el").remove();
//            _self.off();
//            _self.clearAttrVals();
//        }
//    });
//    BuiItem.REGINS = REGINS;
//    BUI.Layout.BuiItem = BuiItem;
//})(window.BUI, jQuery);
////BUI.Layout.BuiLayout
//(function (BUI, $) {
//    "use strict";
//    var Component = BUI.Component,
//        BuiItem = BUI.Layout.BuiItem,
//        REGINS = BuiItem.REGINS,
//         CLS_TOP = "bui-border-top",
//    CLS_MIDDLE = "bui-border-middle",
//    CLS_VIEW_CONTAINER = "bui-viewport-container";
//    var BuiLayout = Component.Controller.extend({
//        initializer: function (control) {
//            var _self = this;
//            _self.set("control", _self);
//        },
//        renderUI: function () {
//            this._initWraper();
//            this.initItems();
//            this.reset();
//        },
//        bindUI: function () {
//            var _self = this,
//            control = _self.get("control"),
//            layoutEvents = _self.get("layoutEvents").join(" ");
//            control.on(layoutEvents,
//            function () {
//                _self.resetLayout();
//            });
//            $(window).on("resize", BUI.wrapBehavior(_self, "reset"));
//        },
//        syncUI: function () {
//            this._setMiddleDimension();
//        },
//        _initWraper: function () {
//            var _self = this,
//           controlEl = _self.get("contentEl"),
//           topEl = controlEl.find("." + CLS_TOP),
//           middleEl = controlEl.find("." + CLS_MIDDLE),
//             render = _self.get("render");
//            $(render).addClass(CLS_VIEW_CONTAINER);
//            _self.set("topEl", topEl);
//            _self.set("middleEl", middleEl);
//        },
//        initItems: function () {
//            var _self = this,
//            control = _self.get("control"),
//            items = [],
//            controlChildren = control.get("children");
//            _self.set("items", items);
//            for (var i = 0; i < controlChildren.length; i++) {
//                _self.addItem(controlChildren[i]);
//            }
//        },
//        addItem: function (control) {
//            var _self = this,
//            items = _self.getItems(),
//            item = _self.initItem(control);
//            items.push(item);
//            return item;
//        },
//        initItem: function (controlChild) {
//            var _self = this,
//            cfg = _self.getItemCfg(controlChild);
//            return new BuiItem(cfg);
//        },
//        getItemCfg: function (controlChild) {
//            var _self = this;
//            return {
//                control: controlChild,
//                layout: _self,
//                container: _self.getItemContainer(controlChild.get("region"))
//            };
//        },
//        getItems: function () {
//            return this.get("items");
//        },
//        getItemsBy: function (fn) {
//            var _self = this,
//            items = _self.getItems(),
//            rst = [];
//            BUI.each(items,
//            function (item) {
//                if (fn(item)) {
//                    rst.push(item);
//                }
//            });
//            return rst;
//        },
//        getItemsByRegion: function (region) {
//            return this.getItemsBy(function (item) {
//                return item.get("region") === region;
//            });
//        },
//        _fitMiddleControl: function () {
//            var _self = this,
//            items = _self.getItems();
//            BUI.each(items,
//            function (item) {
//                var region = item.get("region");
//                if (region == REGINS.WEST || region == REGINS.CENTER) {
//                    item.syncFit();
//                }
//            });
//        },
//        _setMiddleDimension: function () {
//            var _self = this,
//            middleEl = _self.get("middleEl"),
//            middleHeight = _self._getMiddleHeight(),
//            left = _self._getMiddleLeft(),
//            items = _self.get("items"),
//            center = _self.getItemsByRegion("center")[0];
//            middleEl.height(middleHeight);
//            if (center) {
//                var el = center.get("el");
//                el.css({
//                    marginLeft: left
//                });
//            }
//            _self._fitMiddleControl();
//        },
//        _getMiddleHeight: function () {
//            var _self = this,
//            container = _self.get("contentEl"),
//            totalHeight = container.height(),
//            middleEl = _self.get("middleEl"),
//            topEl = _self.get("topEl"),
//            appendHeight,
//            middleHeight;
//            if (topEl.children().length) {
//                middleHeight = totalHeight - topEl.outerHeight();
//            } else {
//                middleHeight = totalHeight;
//            }
//            appendHeight = middleEl.outerHeight() - middleEl.height();
//            return middleHeight - appendHeight;
//        },
//        _getMiddleLeft: function () {
//            var _self = this,
//            westItems = _self.getItemsByRegion("west"),
//            leftWidth = 0;
//            BUI.each(westItems,
//            function (item) {
//                leftWidth += item.get("el").outerWidth();
//            });
//            return leftWidth;
//        },
//        resetLayout: function () {
//            var _self = this,
//            items = _self.getItems();
//            BUI.each(items,
//            function (item) {
//                item.syncFit();
//            });
//            _self._setMiddleDimension();
//        },
//        reset: function () {
//            var _self = this,
//                    render = _self.get("render"),
//                    el = _self.get("el");
//            if (render == "body") {
//                var viewportHeight = BUI.viewportHeight(),
//                  viewportWidth = BUI.viewportWidth(),
//                  appendWidth = _self.getAppendWidth(),
//                  appendHeight = _self.getAppendHeight();
//                _self.set("width", viewportWidth - appendWidth);
//                _self.set("height", viewportHeight - appendHeight);
//            } else {
//                var viewportHeight = el.parent().height(),
//               viewportWidth = el.parent().width();
//                _self.set("width", viewportWidth);
//                _self.set("height", viewportHeight);
//            }
//            _self.fire("resize");

//        },       
//        getItemContainer: function (region) {
//            var _self = this,
//            rst;
//            switch (region) {
//                case REGINS.NORTH:
//                    rst = _self.get("topEl");
//                    break;
//                default:
//                    rst = _self.get("middleEl");
//                    break;
//            }
//            return rst;
//        }
//    }, {
//        ATTRS: {
//            layoutEvents: {
//                value: ["afterWidthChange", "afterHeightChange"]
//            },
//            items: {},
//            render: {
//                value: "body"
//            }
//        }
//    }, {
//        xclass: "bui-layout"
//    });

//    BUI.Layout.BuiLayout = BuiLayout;
//})(window.BUI, jQuery);
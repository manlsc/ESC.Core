//BUI.Layout.BuiLayout
(function (BUI, $) {
    "use strict";
    BUI.Layout = {};
    var Component = BUI.Component,
        BuiItem = BUI.Layout.BuiItem,
        REGINS = {
            NORTH: "north",
            WEST: "west",
            CENTER: "center"
        },
        CLS_TOP = "bui-border-top",
        CLS_MIDDLE = "bui-border-middle",
        CLS_VIEW_CONTAINER = "bui-viewport-container";
    var BuiLayout = Component.Controller.extend({
        renderUI: function () {
            this._initWraper();
        },
        bindUI: function () {
            var _self = this,
                layoutEvents = _self.get("layoutEvents").join(" ");
            //_self.on(layoutEvents,
            //    function () {
            //        _self._fitMiddleControl();
            //    });
            $(window).on("resize", BUI.wrapBehavior(_self, "reset"));
        },
        syncUI: function () {
            this.reset();
        },
        _initWraper: function () {
            var _self = this,
                controlEl = _self.get("contentEl"),
                topEl = controlEl.find("." + CLS_TOP),
                middleEl = controlEl.find("." + CLS_MIDDLE),
                render = _self.get("render");
            $(render).addClass(CLS_VIEW_CONTAINER);
            _self.set("topEl", topEl);
            _self.set("middleEl", middleEl);
        },
        getChildByRegion: function (region) {
            return this.getChildrenBy(function (child) {
                return child.get("region") === region;
            });
        },
        _fitMiddleControl: function () {
            var _self = this,
                children = _self.get("children");
            BUI.each(children,
                function (child) {
                    _self.syncFit(child);
                });
        },
        _setMiddleDimension: function () {
            var _self = this,
                middleEl = _self.get("middleEl"),
                middleHeight = _self._getMiddleHeight(),
                left = _self._getMiddleLeft(),
                center = _self.getChildByRegion("center")[0];
            middleEl.height(middleHeight);
            if (center) {
                var el = center.get("el");
                el.css({
                    marginLeft: left
                });
            }
            _self._fitMiddleControl();
        },
        _getMiddleHeight: function () {
            var _self = this,
                container = _self.get("contentEl"),
                totalHeight = container.height(),
                middleEl = _self.get("middleEl"),
                topEl = _self.get("topEl"),
                appendHeight,
                middleHeight;
            if (topEl.children().length) {
                middleHeight = totalHeight - topEl.outerHeight();
            } else {
                middleHeight = totalHeight;
            }
            appendHeight = middleEl.outerHeight() - middleEl.height();
            return middleHeight - appendHeight;
        },
        _getMiddleLeft: function () {
            var _self = this,
                westItems = _self.getChildByRegion("west"),
                leftWidth = 0;
            BUI.each(westItems,
                function (child) {
                    leftWidth += child.get("el").outerWidth();
                });
            return leftWidth;
        },
        _getMiddleWidth: function () {
            var _self = this,
                container = _self.get("contentEl"),
                totalWidth = container.width(),
                appendWidth;

            appendWidth = container.outerWidth() - container.width();
            return totalWidth - appendWidth;
        },
        reset: function () {
            var _self = this,
                render = _self.get("render"),
                el = _self.get("el");
            if (render == "body") {
                var viewportHeight = BUI.viewportHeight(),
                    viewportWidth = BUI.viewportWidth(),
                    appendWidth = _self.getAppendWidth(),
                    appendHeight = _self.getAppendHeight();
                _self.set("width", viewportWidth - appendWidth);
                _self.set("height", viewportHeight - appendHeight);
            } else {
                var viewportHeight = el.parent().height(),
                    viewportWidth = el.parent().width();
                _self.set("width", viewportWidth);
                _self.set("height", viewportHeight);
            }
            _self._setMiddleDimension();
            _self.fire("resize");

        },
        getItemContainer: function (region) {
            var _self = this,
                rst;
            switch (region) {
                case REGINS.NORTH:
                    rst = _self.get("topEl");
                    break;
                default:
                    rst = _self.get("middleEl");
                    break;
            }
            return rst;
        },
        syncFit: function (child) {
            var _self = this,
                region = child.get("region");
            if (region === REGINS.WEST) {
                _self._syncControlHeight(child, region);
                return;
            }
            if (region === REGINS.CENTER) {
                _self._syncControlWidth(child, region);
                _self._syncControlHeight(child, region);
            }
        },
        _syncControlWidth: function (child, region) {
            var _self = this,
                width = _self._getMiddleWidth(),
                appendWidth = child.getAppendWidth(),
                leftWidth = _self._getMiddleLeft();
            child.set("width", width - appendWidth - leftWidth);
        },
        _syncControlHeight: function (child, region) {
            var _self = this,
                height = _self.getFitHeight(child, region),
                appendHeight = child.getAppendHeight();
            child.set("height", height - appendHeight);
        },
        getFitHeight: function (child, region) {
            var _self = this,
                outerHeight = _self._getMiddleHeight();
            return outerHeight;
        }
    }, {
            ATTRS: {
                layoutEvents: {
                    value: ["afterWidthChange", "afterHeightChange"]
                },
                render: {
                    value: "body"
                }
            }
        }, {
            xclass: "bui-layout"
        });

    BUI.Layout.BuiLayout = BuiLayout;
})(window.BUI, jQuery);
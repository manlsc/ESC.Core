//BUI.Overlay
(function (BUI, $) {
    "use strict";
    BUI.Overlay = {};
    var Component = BUI.Component,
        CLS_ARROW = 'tooltip-arrow',
        UIBase = Component.UIBase,
        wrapBehavior = BUI.wrapBehavior,
        getWrapBehavior = BUI.getWrapBehavior,
        win = window;

    //获取当前节点放置的位置
    function getElFuturePos(elRegion, refNodeRegion, points, offset) {
        var xy, diff, p1, p2;
        xy = {
            left: elRegion.left,
            top: elRegion.top
        };
        p1 = getAlignOffset(refNodeRegion, points[0]);
        p2 = getAlignOffset(elRegion, points[1]);
        diff = [p2.left - p1.left, p2.top - p1.top];
        return {
            left: xy.left - diff[0] + +offset[0],
            top: xy.top - diff[1] + +offset[1]
        };
    }
    //获取当前节点所占位置
    function getRegion(node) {
        var offset, w, h;
        if (node.length && !$.isWindow(node[0])) {
            offset = node.offset();
            w = node.outerWidth();
            h = node.outerHeight();
            if (w < 1) {
                w = node[0].offsetWidth || node[0].clientWidth;
            }
            if (h < 1) {
                h = node[0].offsetHeight || node[0].clientHieght;
            }
        } else {
            offset = {
                left: BUI.scrollLeft(),
                top: BUI.scrollTop()
            };
            w = BUI.viewportWidth();
            h = BUI.viewportHeight();
        }
        offset.width = w;
        offset.height = h;
        return offset;
    }

    function getAlignOffset(region, align) {
        var V = align.charAt(0),
            H = align.charAt(1),
            w = region.width,
            h = region.height,
            x,
            y;
        x = region.left;
        y = region.top;
        if (V === "c") {
            y += h / 2;
        } else if (V === "b") {
            y += h;
        }
        if (H === "c") {
            x += w / 2;
        } else if (H === "r") {
            x += w;
        }
        return {
            left: x,
            top: y
        };
    }

    function isExcept(self, elem) {
        var hideExceptNode = self.get("hideExceptNode");
        if (hideExceptNode && hideExceptNode.length) {
            return $.contains(hideExceptNode[0], elem);
        }
        return false;
    }

    var overlay = Component.Controller.extend({
        /**
		 * 移动到制定坐标
		 * @param {Object} x
		 * @param {Object} y
		 */
        move: function (x, y) {
            var self = this;
            if (BUI.isArray(x)) {
                y = x[1];
                x = x[0];
            }
            self.set("xy", [x, y]);
            return self;
        },
        /**
		 * 显示元素
		 */
        show: function () {
            var _self = this,
                el = _self.get("el");
            if (!_self.get("rendered")) {
                _self.render();
                el = _self.get("el");
            }
            _self.set("visible", true);

            var align = _self.get("align");
            _self.set("align", align);

            var delay = _self.get("autoHideDelay"),
                delayHandler = _self.get("delayHandler");
            if (delay) {
                delayHandler && clearTimeout(delayHandler);
                delayHandler = setTimeout(function () {
                    _self.hide();
                    _self.set("delayHandler", null);
                }, delay);
                _self.set("delayHandler", delayHandler);
            }
        },
        /**
		 * 隐藏元素
		 */
        hide: function () {
            var _self = this;
            _self.set("visible", false);
        },
        /**
		 * 创建dom
		 */
        createDom: function () {
            this._setTrigger();
        },
        renderUI: function () {
            var _self = this,
                align = _self.get("align"),
                el = _self.get('el'),
                arrowContainer = _self.get('arrowContainer'),
                container = arrowContainer ? el.one(arrowContainer) : el;
            if (align && !align.node) {
                align.node = _self.get("render") || _self.get("trigger");
            }
            if (_self.get('showArrow')) {
                $(_self.get('arrowTpl')).appendTo(container);
            }
        },
        bindUI: function () {
            var _self = this,
                triggerActiveCls = _self.get("triggerActiveCls");
            if (triggerActiveCls) {
                _self.on("hide",
                    function () {
                        var curTrigger = _self.get("curTrigger");
                        if (curTrigger) {
                            curTrigger.removeClass(triggerActiveCls);
                        }
                    });
            }
            _self.on("afterVisibleChange",
                function (ev) {
                    var visible = ev.newVal;
                    if (_self.get("autoHide")) {
                        if (visible) {
                            _self._bindHideEvent();
                        } else {
                            _self._clearHideEvent();
                        }
                    }
                });
            var fn = BUI.wrapBehavior(_self, "handleWindowResize");
            _self.on("show",
                function () {
                    $(window).on("resize", fn);
                });
            _self.on("hide",
                function () {
                    $(window).off("resize", fn);
                });
        },
        handleWindowResize: function () {
            var _self = this,
                align = _self.get("align");
            _self.set("align", align);
        },
        align: function (refNode, points, offset, overflow) {
            refNode = $(refNode || win);
            offset = offset && [].concat(offset) || [0, 0];
            overflow = overflow || {};
            var self = this,
                el = self.get("el"),
                fail = 0,
                //当前节点所占的区域, left/top/width/height
                elRegion = getRegion(el),
                //参照节点所占的区域, left/top/width/height
                refNodeRegion = getRegion(refNode),
                //当前节点将要被放置的位置
                elFuturePos = getElFuturePos(elRegion, refNodeRegion, points, offset),
                //当前节点将要所处的区域
                newElRegion = BUI.merge(elRegion, elFuturePos);

            if (newElRegion.left != elRegion.left) {
                self.setInternal("x", null);
                self.set("x", newElRegion.left);
            }
            if (newElRegion.top != elRegion.top) {
                self.setInternal("y", null);
                self.set("y", newElRegion.top);
            }
            if (newElRegion.width != elRegion.width) {
                el.width(el.width() + newElRegion.width - elRegion.width);
            }
            if (newElRegion.height != elRegion.height) {
                el.height(el.height() + newElRegion.height - elRegion.height);
            }
            return self;
        },
        center: function (node) {
            var self = this;
            self.set("align", {
                node: node,
                points: ["cc", "cc"],
                offset: [0, 0]
            });
            return self;
        },
        _setTrigger: function () {
            var _self = this,
                //显示菜单的事件
                triggerEvent = _self.get("triggerEvent"),
                //因为触发元素发生改变而导致控件隐藏
                triggerHideEvent = _self.get("triggerHideEvent"),
                //触发显示时的回调函数
                triggerCallback = _self.get("triggerCallback"),
                //触发显示控件的DOM选择器
                triggerActiveCls = _self.get("triggerActiveCls") || "",
                trigger = _self.get("trigger"),
                //是否使用代理的方式触发显示控件
                isDelegate = _self.get("delegateTrigger"),

                triggerEl = $(trigger);

            function tiggerShow(ev) {
                if (_self.get("disabled")) {
                    return;
                }
                var prevTrigger = _self.get("curTrigger"),
                    curTrigger = isDelegate ? $(ev.currentTarget) : $(this),
                    align = _self.get("align");
                if (!prevTrigger || prevTrigger[0] != curTrigger[0]) {
                    if (prevTrigger) {
                        prevTrigger.removeClass(triggerActiveCls);
                    }
                    _self.set("curTrigger", curTrigger);
                    _self.fire("triggerchange", {
                        prevTrigger: prevTrigger,
                        curTrigger: curTrigger
                    });
                }
                curTrigger.addClass(triggerActiveCls);
                if (_self.get("autoAlign")) {
                    align.node = curTrigger;
                }
                _self.show();
                triggerCallback && triggerCallback(ev);
            }

            function tiggerHide(ev) {
                var toElement = ev.toElement || ev.relatedTarget;
                if (!toElement || !_self.containsElement(toElement)) {
                    _self.hide();
                }
            }
            if (triggerEvent) {
                if (isDelegate && BUI.isString(trigger)) {
                    $(document).delegate(trigger, triggerEvent, tiggerShow);
                } else {
                    triggerEl.on(triggerEvent, tiggerShow);
                }
            }
            if (triggerHideEvent) {
                if (isDelegate && BUI.isString(trigger)) {
                    $(document).delegate(trigger, triggerHideEvent, tiggerHide);
                } else {
                    triggerEl.on(triggerHideEvent, tiggerHide);
                }
            }
        },
        handleMoveOuter: function (ev) {
            var _self = this,
                target = ev.toElement || ev.relatedTarget;
            if (!_self.containsElement(target) && !isExcept(_self, target)) {
                if (_self.fire("autohide") !== false) {
                    _self.hide();
                }
            }
        },
        handleDocumentClick: function (ev) {
            var _self = this,
                target = ev.target;
            if (!_self.containsElement(target) && !isExcept(_self, target)) {
                if (_self.fire("autohide") !== false) {
                    _self.hide();
                }
            }
        },
        destructor: function () {
            var _self = this,
                //显示菜单的事件
                triggerEvent = _self.get("triggerEvent"),
                trigger = _self.get("trigger"),
                //是否使用代理的方式触发显示控件
                isDelegate = _self.get("delegateTrigger"),
                triggerEl = $(trigger);
            if (triggerEvent) {
                if (isDelegate && BUI.isString(trigger)) {
                    $(document).undelegate(trigger, triggerEvent);
                } else {
                    triggerEl.off(triggerEvent);
                }
            }
        },
        _bindHideEvent: function () {
            var _self = this,
                trigger = _self.get("curTrigger"),
                autoHideType = _self.get("autoHideType");
            if (autoHideType === "click") {
                $(document).on("mousedown", wrapBehavior(_self, "handleDocumentClick"));
            } else {
                _self.get("el").on("mouseleave", wrapBehavior(_self, "handleMoveOuter"));
                if (trigger) {
                    $(trigger).on("mouseleave", wrapBehavior(_self, "handleMoveOuter"));
                }
            }
        },
        _clearHideEvent: function () {
            var _self = this,
                trigger = _self.get("curTrigger"),
                autoHideType = _self.get("autoHideType");
            if (autoHideType === "click") {
                $(document).off("mousedown", getWrapBehavior(_self, "handleDocumentClick"));
            } else {
                _self.get("el").off("mouseleave", getWrapBehavior(_self, "handleMoveOuter"));
                if (trigger) {
                    $(trigger).off("mouseleave", getWrapBehavior(_self, "handleMoveOuter"));
                }
            }
        },
        _uiSetAlign: function (v, ev) {
            var alignCls = "",
                el, selfAlign;
            if (v && v.points) {
                this.align(v.node, v.points, v.offset, v.overflow);
                this.set("cachePosition", null);
            }
        },
        _uiSetX: function (v) {
            if (v != null) {
                var _self = this,
                    el = _self.get("el");
                el.offset({
                    left: v
                });
                _self.setInternal("left", v);
                if (v != -999) {
                    this.set("cachePosition", null);
                }
            }
        },
        _uiSetY: function (v) {
            if (v != null) {
                var _self = this,
                    el = _self.get("el");
                el.offset({
                    top: v
                });

                _self.setInternal("top", v);
                if (v != -999) {
                    this.set("cachePosition", null);
                }
            }
        },
        _uiSetLeft: function (v) {
            if (v != null) {
                var _self = this,
                    el = _self.get("el");
                el.css({
                    left: v
                });
                _self.setInternal("x", v);
            }
        },
        _uiSetZIndex: function (x) {
            this.get("el").css("z-index", x);
        },
        _uiSetTop: function (v) {
            if (v != null) {
                var _self = this,
                    el = _self.get("el");
                el.css({
                    top: v
                });
                _self.setInternal("y", v);
            }
        }
    }, {
            ATTRS: {
                x: {
                    valueFn: function () {
                        var self = this;
                        return self.get("el") && self.get("el").offset().left;
                    }
                },
                y: {
                    valueFn: function () {
                        var self = this;
                        return self.get("el") && self.get("el").offset().top;
                    }
                },
                left: {},
                top: {},
                xy: {
                    setter: function (v) {
                        var self = this,
                            xy = $.makeArray(v);
                        if (xy.length) {
                            xy[0] && self.set("x", xy[0]);
                            xy[1] && self.set("y", xy[1]);
                        }
                        return v;
                    },
                    getter: function () {
                        return [this.get("x"), this.get("y")];
                    }
                },
                zIndex: {},
                autoHideDelay: {},
                closeable: {
                    value: false
                },
                visibleMode: {
                    value: "visibility"
                },
                visible: {
                    value: false
                },
                align: {
                    shared: false,
                    value: {}
                },
                autoHideType: {
                    value: "click"
                },
                autoHide: {
                    value: false
                },
                hideExceptNode: {},
                trigger: {},
                delegateTrigger: {
                    value: false
                },
                autoAlign: {
                    value: true
                },
                arrowTpl: {
                    value: '<div class="' + CLS_ARROW + '"></div>'
                },
                showArrow: {
                    value: false
                },
                arrowContainer: {},
                triggerActiveCls: {},
                curTrigger: {},
                triggerCallback: {},
                triggerEvent: {
                    value: "click"
                },
                triggerHideEvent: {},
                events: {
                    value: {
                        autohide: false,
                        triggerchange: false
                    }
                }
            }
        }, {
            xclass: "overlay"
        });
    BUI.Overlay.Overlay = overlay;
})(window.BUI, jQuery);
//BUI.Overlay.Dialog
(function (BUI, $) {
    "use strict";
    var Overlay = BUI.Overlay.Overlay,
        UIBase = BUI.Component.UIBase,
        CLS_TITLE = "modal-title",
        HEIGHT_PADDING = 15,
        maskMap = {};
    var actions = {
        hide: "hide",
        destroy: "destroy",
        remove: "remove"
    };
    var dragBackId = BUI.guid("drag");

    function getMaskCls(self) {
        return self.get("prefixCls") + "mask";
    }

    function initMask(maskCls) {
        var mask = $('<div style="position: fixed;" class="' + maskCls + '"></div>').prependTo("body");
        mask.on("mousedown",
            function (e) {
                e.preventDefault();
            });
        return mask;
    }

    //设置header body footer内容
    function _setStdModRenderContent(self, part, v) {
        part = self.get(part);
        if (BUI.isString(v)) {
            part.html(v);
        } else {
            part.html("").append(v);
        }
    }
    var dialog = Overlay.extend({
        renderUI: function () {
            var self = this;
            var contentEl = self.get("contentEl");
            self.setInternal("header", contentEl.find(".modal-header"));
            self.setInternal("body", contentEl.find(".modal-body"));
            self.setInternal("footer", contentEl.find(".modal-footer"));
            self.setInternal("closeBtn", contentEl.find(".close"));
        },
        bindUI: function () {
            var _self = this,
                dragNode = _self.get("dragNode");
            _self.on("closeclick",
                function () {
                    return _self.onCancel();
                });

            if (_self.get("mask")) {
                _self.on("show",
                    function () {
                        _self._maskExtShow();
                    });
                _self.on("hide",
                    function () {
                        _self._maskExtHide();
                    });
            }

            if (!dragNode) {
                return;
            }
            dragNode.on("mousedown",
                function (e) {
                    if (e.which == 1) {
                        e.preventDefault();
                        _self.set("draging", {
                            elX: _self.get("x"),
                            elY: _self.get("y"),
                            startX: e.pageX,
                            startY: e.pageY
                        });
                        registEvent();
                    }
                });

            function mouseMove(e) {
                var draging = _self.get("draging");
                if (draging) {
                    e.preventDefault();
                    _self._dragMoveTo(e.pageX, e.pageY, draging);
                }
            }

            function mouseUp(e) {
                if (e.which == 1) {
                    _self.set("draging", false);
                    unregistEvent();
                }
            }

            function registEvent() {
                $(document).on("mousemove", mouseMove);
                $(document).on("mouseup", mouseUp);
            }

            function unregistEvent() {
                $(document).off("mousemove", mouseMove);
                $(document).off("mouseup", mouseUp);
            }
        },
        _dragMoveTo: function (x, y, draging) {
            var _self = this,
                draging = draging || _self.get("draging"),
                offsetX = draging.startX - x,
                offsetY = draging.startY - y;
            _self.set("xy", [draging.elX - offsetX, draging.elY - offsetY]);
        },
        getContentElement: function () {
            return this.get("body");
        },
        close: function () {
            var self = this,
                action = actions[self.get("closeAction") || "hide"];
            if (self.fire("closing", {
                action: action
            }) !== false) {
                self.fire("beforeclosed", {
                    action: action
                });
                if (action == "remove") {
                    self[action](true);
                } else {
                    self[action]();
                }
                self.fire("closed", {
                    action: action
                });
            }
        },
        onCancel: function () {
            var _self = this,
                cancel = _self.get("cancel");
            return cancel.call(this);
        },
        _maskExtShow: function () {
            var self = this,
                zIndex, maskCls = getMaskCls(self),
                maskDesc = maskMap[maskCls],
                maskShared = self.get("maskShared"),
                mask = self.get("maskNode");
            if (!mask) {
                if (maskShared) {
                    if (maskDesc) {
                        mask = maskDesc.node;
                    } else {
                        mask = initMask(maskCls);
                        maskDesc = maskMap[maskCls] = {
                            num: 0,
                            node: mask
                        };
                    }
                } else {
                    mask = initMask(maskCls);
                }
                self.setInternal("maskNode", mask);
            }
            if (zIndex = self.get("zIndex")) {
                mask.css("z-index", zIndex - 1);
            }
            if (maskShared) {
                maskDesc.num++;
            }
            if (!maskShared || maskDesc.num == 1) {
                mask.show();
            }
            $("body").addClass("bui-masked-relative");
        },
        _maskExtHide: function () {
            var self = this,
                maskCls = getMaskCls(self),
                maskDesc = maskMap[maskCls],
                maskShared = self.get("maskShared"),
                mask = self.get("maskNode");
            if (maskShared && maskDesc) {
                maskDesc.num = Math.max(maskDesc.num - 1, 0);
                if (maskDesc.num == 0) {
                    mask.hide();
                }
            } else if (mask) {
                mask.hide();
            }
            $("body").removeClass("bui-masked-relative");
        },
        _uiSetButtons: function (buttons) {
            var _self = this,
                footer = _self.get("footer");
            footer.children().remove();
            BUI.each(buttons,
                function (conf) {
                    _self._createButton(conf, footer);
                });
        },
        _createButton: function (conf, parent) {
            var _self = this,
                temp = '<button type="button" class="' + conf.elCls + '">' + conf.text + "</button>",
                btn = $(temp).appendTo(parent);
            btn.on("click",
                function () {
                    conf.handler.call(_self, _self, this);
                });
        },
        _uiSetBodyStyle: function (v) {
            this.get("body").css(v);
        },
        _uiSetHeaderStyle: function (v) {
            this.get("header").css(v);
        },
        _uiSetFooterStyle: function (v) {
            this.get("footer").css(v);
        },
        _uiSetBodyContent: function (v) {
            _setStdModRenderContent(this, "body", v);
        },
        _uiSetHeaderContent: function (v) {
            _setStdModRenderContent(this, "header", v);
        },
        _uiSetFooterContent: function (v) {
            _setStdModRenderContent(this, "footer", v);
        },
        _uiSetTitle: function (v) {
            var _self = this,
                el = _self.get("header");
            el.find("." + CLS_TITLE).html(v);
        },
        _uiSetContentId: function (v) {
            var _self = this,
                body = _self.get("body"),
                children = $("#" + v).children();
            children.appendTo(body);
        },
        _uiSetHeight: function (v) {
            var _self = this,
                bodyHeight = v,
                header = _self.get("header"),
                body = _self.get("body"),
                footer = _self.get("footer");
            bodyHeight -= header.outerHeight() + footer.outerHeight();
            bodyHeight -= HEIGHT_PADDING * 2;
            body.height(bodyHeight);
        },
        _uiSetCloseable: function (v) {
            var self = this,
                btn = self.get("closeBtn");
            if (v) {              
            } else {
                if (btn) {
                    btn.remove();
                }
            }
            if (v) {
                btn.on("click",
                    function (ev) {
                        if (self.fire("closeclick", {
                            domTarget: ev.target
                        }) !== false) {
                            self.close();
                        }
                        return false;
                    });
            }
        },
        destructor: function () {
            var _self = this,
                contentId = _self.get("contentId"),
                body = _self.get("body"),
                closeAction = _self.get("closeAction");
            if (closeAction == "destroy") {
                _self.hide();
                if (contentId) {
                    body.children().appendTo("#" + contentId);
                }
            }

            var btn = _self.get("closeBtn");
            btn && btn.detach();

            var maskShared = _self.get("maskShared"),
                mask = _self.get("maskNode");
            if (_self.get("maskNode")) {
                if (maskShared) {
                    if (_self.get("visible")) {
                        _self._maskExtHide();
                    }
                } else {
                    mask.remove();
                }
            }
        }
    }, {
            ATTRS: {
                zIndex: {
                    value: 1050
                },
                header: {},
                body: {},
                footer: {},
                bodyStyle: {},
                footerStyle: {},
                headerStyle: {},
                headerContent: {},
                bodyContent: {},
                footerContent: {},               
                buttons: {
                    value: [{
                        text: "确定",
                        elCls: "btn btn-primary",
                        handler: function () {
                            var _self = this,
                                success = _self.get("success");
                            if (success) {
                                success.call(_self);
                            }
                        }
                    },
                    {
                        text: "取消",
                        elCls: "btn btn-default",
                        handler: function (dialog, btn) {
                            if (this.onCancel() !== false) {
                                this.close();
                            }
                        }
                    }
                    ]
                },
                contentId: {},
                success: {
                    value: function () {
                        this.close();
                    }
                },
                cancel: {
                    value: function () { }
                },
                dragNode: {
                    valueFn: function () {
                        return this.get("header");
                    }
                },
                title: {
                    value: "标题"
                },
                align: {
                    sync: false,
                    value: {
                        node: window,
                        points: ["cc", "cc"]
                    }
                },
                mask: {
                    value: true
                },
                maskNode: {},
                maskShared: {
                    value: false
                },
                closeable: {
                    value: true
                },
                closeBtn: {
                    value: false
                },
                closeAction: {
                    value: "hide"
                },
                draging: {
                    setter: function (v) {
                        if (v === true) {
                            return {};
                        }
                    },
                    value: null
                },
                elCls: {
                    value: "modal-dialog"
                },
                tpl: {
                    value: '<div class="modal-content">\
                                <div class="modal-header">\
                                    <button type="button" class="close"><span>&times;</span></button>\
                                    <h4 class="modal-title"></h4>\
                                </div>\
                                <div class= "modal-body">\
                                </div>\
                                <div class="modal-footer">\
                                </div>\
                           </div>'
                }
            }
        }, {
            xclass: "dialog"
        });
    BUI.Overlay.Dialog = dialog;
})(window.BUI, jQuery);
//BUI.Message
(function (BUI, $) {
    "use strict";
    var Dialog = BUI.Overlay.Dialog,
        PREFIX = BUI.prefix,
        iconText = {
            info: {
                iconfont: "iconfont-lg icon-info",
                cls: "text-info"
            },
            error: {
                iconfont: "iconfont-lg icon-error",
                cls: "text-danger"
            },
            success: {
                iconfont: "iconfont-lg icon-successful",
                cls: "text-success"
            },
            question: {
                iconfont: "iconfont-lg icon-question",
                cls: "text-primary"
            },
            warning: {
                iconfont: "iconfont-lg icon-warning",
                cls: "text-warning"
            }
        },
        message = {};

    message.Alert = function (msg, callback, icon) {
        if (BUI.isString(callback)) {
            icon = callback;
            callback = null;
        }
        icon = icon || "success";
        var bodyContent = '<div><span class="bui-messager"><i class="' + iconText[icon].iconfont + ' ' + iconText[icon].cls + '"></i></span><span>' + msg + '</span></div>';
        var alt = new Dialog({
            mask: true,
            width: 200,
            bodyContent: bodyContent,
            buttons: [{
                text: "确定",
                elCls: "btn btn-primary",
                handler: function () {
                    if (callback) {
                        var rt = callback.call(this);
                        if (rt !== false) {
                            this.destroy();
                        }
                    } else {
                        this.destroy();
                    }
                }
            }]
        });
        alt.show();
        return alt;
    };
    message.Confirm = function (msg, callback) {
        if (BUI.isString(callback)) {
            icon = callback;
            callback = null;
        }
        var bodyContent = '<div><span class="bui-messager"><i class="' + iconText["question"].iconfont + ' ' + iconText["question"].cls + '"></i> </span><span>' + msg + '</span></div>';
        var cfg = new Dialog({
            mask: true,
            width: 200,
            bodyContent: bodyContent,
            buttons: [{
                text: "确定",
                elCls: "btn btn-primary",
                handler: function () {
                    if (callback) {
                        var rt = callback.call(this);
                        if (rt !== false) {
                            this.destroy();
                        }
                    } else {
                        this.destroy();
                    }
                }
            },
            {
                text: "取消",
                elCls: "btn",
                handler: function () {
                    this.destroy();
                }
            }
            ]
        });
        cfg.show();
        return cfg;
    }
    message.Show = function (config) {
        config = config || {};
        var msg = new Dialog(config);
        msg.show();
        return msg;
    };
    BUI.Message = message;
})(window.BUI, jQuery);
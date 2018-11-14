//BUI.Slider
(function (BUI, $) {
    "use strict";
    var doc = document, CLS_HANDLE = "x-slider-handle", CLS_VERTICLE = "x-slider-vertical", CLS_HORI = "x-slider-horizontal", CLS_BACK = "x-slider-back", CLS_START = CLS_HANDLE + "-start", CLS_END = CLS_HANDLE + "-end";
    function parsePercet(v, total) {
        if (v > total) {
            v = total;
        }
        if (v < 0) {
            v = 0;
        }
        return v / total * 100;
    }

    var Slider = BUI.Component.Controller.extend({
        renderUI: function () {
            var _self = this, isVertical = _self.get("isVertical");
            if (isVertical) {
                _self.get("el").addClass(CLS_VERTICLE);
            } else {
                _self.get("el").addClass(CLS_HORI);
            }
            _self.on("beforeSyncUI", function () {
                var bTpl = _self.get("backTpl"), hTpl = _self.get("handleTpl");
                _self._setBackTpl(bTpl);
                _self._handleTpl(hTpl);
            });
        },
        slideTo: function (v) {
            this.set("value", v);
        },
        bindUI: function () {
            var _self = this, el = _self.get("el");
            el.find(".x-slider-handle").on("click", function (ev) {
                ev.preventDefault();
            });
            el.on("mousedown", function (ev) {
                var sender = $(ev.target), offset = el.offset();
                if (sender.hasClass("x-slider-handle")) {
                    ev.preventDefault();
                    _self._handleDrag(ev);
                } else {
                    offset = {
                        left: ev.pageX - offset.left,
                        top: ev.pageY - offset.top
                    };
                    _self._slideByOffset(offset, true);
                }
            });
        },
        setRange: function (start, end, anim) {
            if (start > end) {
                start = end;
            }
            var _self = this, backEl = _self.get("backEl"), isVertical = _self.get("isVertical"), handleEl = _self.get("handleEl"), handleCount = handleEl.length, range = end - start, duration = anim ? _self.get("duration") : null,
                rangeAttr = isVertical ? "height" : "width", posAttr = isVertical ? "bottom" : "left", method = anim ? "animate" : "css", backCss = {}, handleCss = {};
            function getPos(pos) {
                return pos + "%";
            }
            if (backEl) {
                backCss[rangeAttr] = range + "%";
                backCss[posAttr] = start + "%";
                backEl[method](backCss, duration);
            }
            if (handleCount === 1) {
                handleCss[posAttr] = getPos(end);
                handleEl[method](handleCss, duration);
            } else if (handleCount === 2) {
                handleCss[posAttr] = getPos(start);
                if (handleEl[0].style[posAttr] !== handleCss[posAttr]) {
                    $(handleEl[0])[method](handleCss, duration);
                }
                handleCss[posAttr] = getPos(end);
                if (handleEl[1].style[posAttr] !== handleCss[posAttr]) {
                    $(handleEl[1])[method](handleCss, duration);
                }
            }
        },
        _setBackTpl: function (v) {
            var _self = this, el = _self.get("el"), backEl = $(v).appendTo(el);
            backEl.addClass(CLS_BACK);
            _self.setInternal("backEl", backEl);
        },
        _handleTpl: function (v) {
            var _self = this, el = _self.get("el"), range = _self.get("range"), handleEl;
            if (!range) {
                _self._createHandleEl(v);
            } else {
                _self._createHandleEl(v, CLS_START);
                _self._createHandleEl(v, CLS_END);
            }
            handleEl = el.find("." + CLS_HANDLE);
            _self.setInternal("handleEl", handleEl);
        },
        _createHandleEl: function (tpl, cls) {
            var _self = this, el = _self.get("el"), handleEl = $(tpl).appendTo(el);
            handleEl.addClass(CLS_HANDLE);
            handleEl.attr("tabindex", "0");
            if (cls) {
                handleEl.addClass(cls);
            }
        },
        _slideByOffset: function (offset, anim) {
            var _self = this, curVal = _self.get("value"), value = _self._formatValue(offset);
            if (curVal === value || BUI.isArray(value) && BUI.Array.equals(value, curVal)) {
                return;
            }
            if (anim) {
                _self.set("value", value);
            } else {
                _self.setInternal("value", value);
                _self._setValue(value, false);
            }
        },
        _handleDrag: function (ev) {
            var _self = this, isVertical = _self.get("isVertical"), handleEl = $(ev.target), pos = handleEl.position();
            if (ev.which == 1) {
                _self.set("draging", {
                    elX: pos.left,
                    elY: isVertical ? pos.top + handleEl.height() : pos.top,
                    startX: ev.pageX,
                    startY: ev.pageY
                });
                registEvent();
            }
            function mouseMove(e) {
                var draging = _self.get("draging");
                if (draging) {
                    e.preventDefault();
                    var endX = e.pageX, endY = e.pageY, curOffset = {};
                    curOffset.left = draging.elX + (endX - draging.startX);
                    curOffset.top = draging.elY + (endY - draging.startY);
                    _self._slideByOffset(curOffset, false);
                }
            }
            function registEvent() {
                $(doc).on("mousemove", mouseMove);
                $(doc).on("mouseup", mouseUp);
            }
            function unregistEvent() {
                $(doc).off("mousemove", mouseMove);
                $(doc).off("mouseup", mouseUp);
            }
            function mouseUp(e) {
                if (e.which == 1) {
                    _self.set("draging", false);
                    unregistEvent();
                }
            }
        },
        _getCalcValue: function (offset) {
            var _self = this, el = _self.get("el"), max = _self.get("max"), min = _self.get("min"), step = _self.get("step"), isVertical = _self.get("isVertical"), total = isVertical ? el.height() : el.width(), pos, calValue;
            if (isVertical) {
                pos = parsePercet(el.height() - offset.top, total);
            } else {
                pos = parsePercet(offset.left, total);
            }
            calValue = (max - min) * pos / 100 + min;
            if (step) {
                calValue = parseInt(calValue, 10);
                var left = calValue % step;
                if (left) {
                    calValue = calValue + (step - left);
                }
            }
            return calValue;
        },
        _formatValue: function (offset) {
            var _self = this, curVal = _self.get("value"), calValue = _self._getCalcValue(offset);
            if (BUI.isNumber(curVal)) {
                return calValue;
            }
            if (BUI.isArray(curVal)) {
                var disStart = Math.abs(curVal[0] - calValue), disEnd = Math.abs(curVal[1] - calValue);
                if (disStart < disEnd) {
                    return [calValue, curVal[1]];
                }
                return [curVal[0], calValue];
            }
            return curVal;
        },
        _uiSetValue: function (v) {
            this._setValue(v, true);
        },
        _setValue: function (value, anim) {
            var _self = this, min = _self.get("min"), max = _self.get("max"), total = max - min, start, end;
            if (min == max) {
                start = 0;
                end = 100;
            } else if (BUI.isNumber(value)) {
                start = 0;
                end = parsePercet(value - min, total);
            } else if (BUI.isArray(value)) {
                start = parsePercet(value[0] - min, total);
                end = parsePercet(value[1] - min, total);
            }
            _self.setRange(start, end, anim);
            _self.fire("change", {
                value: value
            });
        }
    }, {
        ATTRS: {
            backEl: {},
            handleEl: {},
            min: {
                value: 0
            },
            duration: {
                value: 400
            },
            max: {
                value: 100
            },
            value: {
            },
            step: {
                value: 1
            },
            handleTpl: {
                value: "<span></span>"
            },
            isVertical: {
                value: false
            },
            range: {
                value: false
            },
            backTpl: {
                value: "<div></div>"
            }
        }
    }, {
        xclass: "slider"
    });
    BUI.Slider = Slider;
})(window.BUI, jQuery);
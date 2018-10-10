//BUI.Tooltip.Tip
(function (BUI, $) {
    "use strict";
    BUI.Tooltip = {};
    var Overlay = BUI.Overlay,
        MAP_TYPES = {
            left: ["cl", "cr"],
            right: ["cr", "cl"],
            top: ["tc", "bc"],
            bottom: ["bc", "tc"]           
        };

    function getOffset(type, offset) {
        if (type === "left") {
            return [-1 * offset, -4];
        }
        if (type === "right") {
            return [offset, -4];
        }
        if (type.indexOf("top")) {
            return [0, offset];
        }
        if (type.indexOf("bottom")) {
            return [0, -1 * offset];
        }
    }

    var Tip = Overlay.Overlay.extend({
        _uiSetAlignType: function (type, ev) {
            var _self = this, offset = _self.get("offset"), align = _self.get("align") || {}, points = MAP_TYPES[type];
            if (ev && ev.prevVal) {
                _self.get("el").removeClass(type);
            }
            if (type) {
                _self.get("el").addClass(type);
            }
            if (points) {
                align.points = points;
                if (offset) {
                    align.offset = getOffset(type, offset);
                }
                _self.set("align", align);
            }
        },
        _getTitleContainer: function () {
            return this.get("el");
        },
        _uiSetTitle: function (title) {
            var _self = this, titleTpl = _self.get("titleTpl"), container = _self._getTitleContainer(), titleEl = _self.get("titleEl"), tem;
            if (titleEl) {
                titleEl.remove();
            }
            title = title || "";
            if (BUI.isString(title)) {
                title = {
                    title: title
                };
            }
            tem = BUI.substitute(titleTpl, title);
            titleEl = $(tem).appendTo(container);
            _self.set("titleEl", titleEl);
        }
    }, {
        ATTRS: {
            delegateTrigger: {
            },
            alignType: {
            },
            title: {
            },
            showArrow: {
                value: true
            },
            arrowContainer: {
                
            },
            autoHide: {
                value: true
            },
            autoHideType: {
                value: "leave"
            },
            offset: {
                value: 0
            },
            elCls:{
                value: "tooltip in"
            },
            triggerEvent: {
                value: "mouseover"
            },
            titleTpl: {
                value: '<div class="tooltip-inner">{title}</div>'
            }
        }
    }, {
        xclass: "tooltip"
    });
    BUI.Tooltip.Tip = Tip;
})(window.BUI, jQuery);
//BUI.Tooltip.Tips
(function (BUI, $) {
    function isObjectString(str) {
        return /^{.*}$/.test(str);
    }
    var Tip = BUI.Tooltip.Tip,
      Tips = function (config) {
          Tips.superclass.constructor.call(this, config);
      };
    Tips.ATTRS = {
        tip: {
        },
        defaultAlignType: {
        }
    };
    BUI.extend(Tips, BUI.Base);
    BUI.augment(Tips, {
        _init: function () {
            this._initDom();
            this._initEvent();
        },
        _initDom: function () {
            var _self = this, tip = _self.get('tip'), defaultAlignType;
            if (tip && !tip.isController) {
                defaultAlignType = tip.alignType;
                tip = new Tip(tip);
                tip.render();
                _self.set('tip', tip);
                if (defaultAlignType) {
                    _self.set('defaultAlignType', defaultAlignType);
                }
            }
        },
        _initEvent: function () {
            var _self = this,
              tip = _self.get('tip');
            tip.on('triggerchange', function (ev) {
                var curTrigger = ev.curTrigger;
                _self._replaceTitle(curTrigger);
                _self._setTitle(curTrigger, tip);
            });
        },
        _replaceTitle: function (triggerEl) {
            var title = triggerEl.attr('title');
            if (title) {
                triggerEl.attr('data-title', title);
                triggerEl[0].removeAttribute('title');
            }
        },
        _setTitle: function (triggerEl, tip) {
            var _self = this,
              title = triggerEl.attr('data-title'),
              alignType = triggerEl.attr('data-align') || _self.get('defaultAlignType');

            if (isObjectString(title)) {
                title = BUI.JSON.looseParse(title);
            }
            tip.set('title', title);
            if (alignType) {
                tip.set('alignType', alignType);
            }
        },
        render: function () {
            this._init();
            return this;
        }
    });
    BUI.Tooltip.Tips = Tips;
})(window.BUI, jQuery);
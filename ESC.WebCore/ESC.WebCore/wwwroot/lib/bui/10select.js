//BUI.Select.Select
(function (BUI, $) {
    "use strict";
    BUI.Select = {};
    var ListPicker = BUI.Picker.ListPicker,
    PREFIX = BUI.prefix;
    function formatItems(items) {
        if ($.isPlainObject(items)) {
            var tmp = [];
            BUI.each(items,
            function (v, n) {
                tmp.push({
                    value: n,
                    text: v
                });
            });
            return tmp;
        }
        var rst = [];
        BUI.each(items,
        function (item, index) {
            if (BUI.isString(item)) {
                rst.push({
                    value: item,
                    text: item
                });
            } else {
                rst.push(item);
            }
        });
        return rst;
    }
    var Component = BUI.Component,
    Picker = ListPicker,
    CLS_INPUT = PREFIX + "select-input",
    select = Component.Controller.extend({
        initializer: function () {
            var _self = this,
            multipleSelect = _self.get("multipleSelect"),
            xclass,
            picker = _self.get("picker"),
            list;
            if (!picker) {
                xclass = multipleSelect ? "listbox" : "simple-list";
                list = _self.get("list") || {};
                list = BUI.mix(list, {
                    xclass: xclass,
                    elCls: PREFIX + "select-list",
                    items: formatItems(_self.get("items"))
                });
                picker = new Picker({
                    children: [list],
                    valueField: _self.get("valueField")
                });
                _self.set("picker", picker);
            } else {
                if (_self.get("valueField")) {
                    picker.set("valueField", _self.get("valueField"));
                }
            }
            if (multipleSelect) {
                picker.set("hideEvent", "");
            }
        },
        renderUI: function () {
            var _self = this,
            picker = _self.get("picker"),
            textEl = _self._getTextEl();
            picker.set("trigger", _self.getTrigger());
            picker.set("triggerEvent", _self.get("triggerEvent"));
            picker.set("autoSetValue", _self.get("autoSetValue"));
            picker.set("textField", textEl);
            picker.set("width", _self.get("width") || 200);
            picker.render();
            _self.set("list", picker.get("list"));
        },
        bindUI: function () {
            var _self = this,
            picker = _self.get("picker"),
            list = picker.get("list");
            picker.on("selectedchange",
            function (ev) {
                if (ev.item) {
                    _self.fire("change", {
                        text: ev.text,
                        value: ev.value,
                        item: ev.item
                    });
                }
            });
            if (_self.get("autoSetValue")) {
                list.on("itemsshow",
                function () {
                    _self._syncValue();
                });
            }
            picker.on("show",
            function () {
                if (_self.get("forceFit")) {
                    picker.set("width", _self.get("el").outerWidth());
                }
            });
        },
        containsElement: function (elem) {
            var _self = this,
            picker = _self.get("picker");
            return Component.Controller.prototype.containsElement.call(this, elem) || picker.containsElement(elem);
        },
        getTrigger: function () {
            return this.get("el");
        },
        _uiSetItems: function (items) {
            if (!items) {
                return;
            }
            var _self = this,
            picker = _self.get("picker"),
            list = picker.get("list");
            list.set("items", formatItems(items));
            _self._syncValue();
        },
        _syncValue: function () {
            var _self = this,
            picker = _self.get("picker"),
            valueField = _self.get("valueField");
            if (valueField) {
                picker.setSelectedValue($(valueField).val());
            }
        },
        _uiSetName: function (v) {
            var _self = this,
            textEl = _self._getTextEl();
            if (v) {
                textEl.attr("name", v);
            }
        },
        _uiSetWidth: function (v) {
            var _self = this;
            if (v != null) {
                var el = _self.get("el");
                el.width(v);
                if (_self.get("forceFit")) {
                    var textEl = _self._getTextEl(),
                    iconEl = el.find(".input-group-btn"),
                    appendWidth = textEl.outerWidth() - textEl.width(),
                    width = v - iconEl.outerWidth() - appendWidth;
                    textEl.width(width);
                }
                if (_self.get("forceFit")) {
                    var picker = _self.get("picker");
                    picker.set("width", v);
                }
            }
        },
        _uiSetDisabled: function (v) {
            var _self = this,
            picker = _self.get("picker"),
            textEl = _self._getTextEl();
            picker.set("disabled", v);
            textEl && textEl.attr("disabled", v);
        },
        _getTextEl: function () {
            var _self = this,
            el = _self.get("el");
            return el.is("input") ? el : el.find("input");
        },
        destructor: function () {
            var _self = this,
            picker = _self.get("picker");
            if (picker) {
                picker.destroy();
            }
        },
        _getList: function () {
            var _self = this,
            picker = _self.get("picker"),
            list = picker.get("list");
            return list;
        },
        getSelectedValue: function () {
            return this.get("picker").getSelectedValue();
        },
        setSelectedValue: function (value) {
            var _self = this,
            picker = _self.get("picker");
            picker.setSelectedValue(value);
        },
        getSelectedText: function () {
            return this.get("picker").getSelectedText();
        }
    },
    {
        ATTRS: {
            picker: {},
            list: {},
            valueField: {},
            focusable: {
                value: true
            },
            autoSetValue: {
                value: true
            },
            multipleSelect: {
                value: false
            },
            name: {},
            items: {
                sync: false
            },
            forceFit: {
                value: true
            },
            inputCls: {
                value: CLS_INPUT
            },
            events: {
                value: {
                    change: false
                }
            },
            elCls: {
                value: "input-group"
            },
            tpl: {
                value: '<input type="text" class="form-control ' + CLS_INPUT + '"/><span class="input-group-btn"><button class="btn btn-default" type="button"><i class="iconfont icon-down"></i></button></span>'
            },
            triggerEvent: {
                value: "click"
            }
        }
    },
    {
        xclass: "select"
    });
    BUI.Select.Select = select;
})(window.BUI, jQuery);
//BUI.Select.Search
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
    Component = BUI.Component,
    KeyCode = BUI.KeyCode,
    CLS_INPUT = PREFIX + "search-input",
    search = Component.Controller.extend({
        bindUI: function () {
            var _self = this,
            textEl = _self._getTextEl(),
            searchEl = _self._getSearchEl(),
            valueField = _self.get('valueField');

            textEl.on('keydown',
            function (ev) {
                var disable = _self.get("disabled");
                if (disable) {
                    return false;
                }
                if (ev.which == KeyCode.ENTER) {
                    var val = "";
                    if (valueField) {
                        val = $(valueField).val();
                    }
                    var txt = textEl.val();
                    var searchParam = _self.get('searchParam') || {};
                    _self.fire("search", {
                        param: searchParam,
                        value: val,
                        text: txt,
                        evttype: "keydown"
                    });
                }
            });

            searchEl.on('click',
            function (ev) {
                var disable = _self.get("disabled");
                if (disable) {
                    return false;
                }
                var val = "";
                if (valueField) {
                    val = $(valueField).val();
                }
                var txt = textEl.val();
                var searchParam = _self.get('searchParam') || {};
                _self.fire("search", {
                    param: searchParam,
                    value: val,
                    text: txt,
                    evttype: "click"
                });
            });
        },
        getTrigger: function () {
            return this.get("el");
        },
        _uiSetName: function (v) {
            var _self = this,
            textEl = _self._getTextEl();
            if (v) {
                textEl.attr("name", v);
            }
        },
        _uiSetWidth: function (v) {
            var _self = this;
            if (v != null) {
                var el = _self.get("el");
                el.width(v);
                if (_self.get("forceFit")) {
                    var textEl = _self._getTextEl(),
                    iconEl = el.find(".input-group-btn"),
                    appendWidth = textEl.outerWidth() - textEl.width(),
                    width = v - iconEl.outerWidth() - appendWidth;
                    textEl.width(width);
                }
            }
        },
        _uiSetDisabled: function (v) {
            var _self = this,
            textEl = _self._getTextEl();
            textEl && textEl.attr("disabled", v);
        },
        _getTextEl: function () {
            var _self = this,
            el = _self.get("el");
            return el.is("input") ? el : el.find("input");
        },
        _getSearchEl: function () {
            var _self = this,
            el = _self.get("el");
            return el.is("span") ? el : el.find("span");
        },
        getSearchValue: function () {
            var _self = this,
            valueField = _self.get('valueField');
            var val = "";
            if (valueField) {
                val = $(valueField).val();
            }
            return val;
        },
        setSearchValue: function (val) {
            var _self = this,
            valueField = _self.get('valueField');
            val = BUI.isNullOrEmpty(val) ? '' : val.toString();
            var oldval = _self.getSearchValue();
            if (val != oldval) {
                valueField.val(val);
                _self.fire("change", { oldValue: oldval, newValue: val });
            }
        },
        setSearchText: function (txt) {
            var _self = this,
            textEl = _self._getTextEl();
            textEl.val(txt);
        },
        getSearchText: function () {
            var _self = this,
            textEl = _self._getTextEl();
            return textEl.val() || "";
        }
    },
    {
        ATTRS: {
            valueField: {},
            focusable: {
                value: true
            },
            forceFit: {
                value: true
            },
            searchParam: {},
            name: {},
            events: {
                value: {
                    search: false
                }
            },
            inputCls: {
                value: CLS_INPUT
            },
            elCls: {
                value: "input-group"
            },
            tpl: {
                value: '<input type="text" class="form-control ' + CLS_INPUT + '"/><span class="input-group-btn"><button class="btn btn-default" type="button"><i class="iconfont icon-search"></i></button></span>'
            }
        }
    },
    {
        xclass: "search"
    });
    BUI.Select.Search = search;
})(window.BUI, jQuery);
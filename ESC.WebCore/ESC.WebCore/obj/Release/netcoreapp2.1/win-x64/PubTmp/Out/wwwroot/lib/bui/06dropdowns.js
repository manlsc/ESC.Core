//BUI.Select.Dropdown 
(function (BUI, $) {
    "use strict";
    BUI.Select = {};
    var DropdownMenu = BUI.List.DropdownMenu,
        Component = BUI.Component,
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

    var BUTTON_TOGGLE = "dropdown-toggle",
        Dropdown = Component.Controller.extend({
            initializer: function () {
                var _self = this,
                    multipleSelect = _self.get("multipleSelect"),
                    dropdownMenu = _self.get("dropdownMenu");
                if (!dropdownMenu) {
                    dropdownMenu = new DropdownMenu({
                        multipleSelect: multipleSelect,
                        items: formatItems(_self.get("items"))
                    });
                    _self.set("dropdownMenu", dropdownMenu);
                }
            },
            renderUI: function () {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                dropdownMenu.set("render", _self.get("el"));
                dropdownMenu.set("width", _self.get("width") || 200);
                dropdownMenu.render();
            },
            bindUI: function () {
                var _self = this,
                    triggerEl = _self.getTrigger(),
                    triggerEvent = _self.get("triggerEvent"),
                    dropdownMenu = _self.get("dropdownMenu");

                triggerEl.on(triggerEvent, function () {
                    dropdownMenu.toggle();
                });


                dropdownMenu.on("itemclick",
                    function (ev) {
                        dropdownMenu.hide();

                        if (ev.item) {
                            _self.fire("change", {
                                text: ev.text,
                                value: ev.value,
                                item: ev.item
                            });
                        }
                    });
            },
            containsElement: function (elem) {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                return Component.Controller.prototype.containsElement.call(this, elem) || picker.containsElement(elem);
            },
            getTrigger: function () {
                return this.get("el").find(this.get("triggerCls"));
            },
            _uiSetItems: function (items) {
                if (!items) {
                    return;
                }
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                dropdownMenu.set("items", formatItems(items));
            },
            _uiSetWidth: function (v) {
                var _self = this;
                if (v != null) {
                    var el = _self.get("el");
                    el.width(v);
                    if (_self.get("forceFit")) {
                        var dropdownMenu = _self.get("dropdownMenu");
                        dropdownMenu.set("width", v);
                    }
                }
            },
            _uiSetDisabled: function (v) {
                var _self = this,
                    triggerEl = _self.getTrigger();
                if (v) {
                    triggerEl.attr("disabled", v);
                }
            },
            destructor: function () {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                if (dropdownMenu) {
                    dropdownMenu.destroy();
                }
            },
            /*
            * 获取选中项值
            */
            getSelectedValue: function () {
                return this.get("dropdownMenu").getSelectedValue();
            },
            /*
            * 设置选中项
            */
            setSelectedValue: function (value) {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                dropdownMenu.setSelectedByField(value);
            },
            /*
             * 获取选中项文本
             */
            getSelectedText: function () {
                return this.get("dropdownMenu").getSelectedText();
            }
        },
            {
                ATTRS: {
                    dropdownMenu: {},                 
                    focusable: {
                        value: false
                    },
                    multipleSelect: {
                        value: false
                    },
                    items: {
                        sync: false
                    },
                    forceFit: {
                        value: true
                    },
                    triggerCls: {
                        value: "." + BUTTON_TOGGLE
                    },
                    events: {
                        value: {
                            change: false
                        }
                    },
                    //样式
                    elCls: {
                        value: "btn-group"
                    },
                    //默认名称
                    btnText: {
                        value: "按钮"
                    },
                    tpl: {
                        value: '<button type="button" class="btn btn-default ' + BUTTON_TOGGLE + '">{btnText}<span class="caret"></span></button >'
                    },
                    //触发事件
                    triggerEvent: {
                        value: "click"
                    }
                }
            },
            {
                xclass: "dropdwons"
            });
    BUI.Select.Dropdown = Dropdown;
})(window.BUI, jQuery);
//BUI.Select.Select
(function (BUI, $) {
    "use strict";
    var DropdownMenu = BUI.List.DropdownMenu,
        Component = BUI.Component,
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

    var BUTTON_TOGGLE = "dropdown-toggle",
        CLS_INPUT = PREFIX + "select-input",
        Select = Component.Controller.extend({
            initializer: function () {
                var _self = this,
                    multipleSelect = _self.get("multipleSelect"),
                    dropdownMenu = _self.get("dropdownMenu");
                if (!dropdownMenu) {
                    dropdownMenu = new DropdownMenu({
                        multipleSelect: multipleSelect,
                        items: formatItems(_self.get("items")),
                        listeners: {
                            itemsshow: function () {
                                _self._syncValue();
                            }
                        }
                    });
                    _self.set("dropdownMenu", dropdownMenu);
                }
            },
            renderUI: function () {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                dropdownMenu.set("render", _self.get("el"));
                dropdownMenu.set("width", _self.get("width") || 200);
                dropdownMenu.render();
            },
            bindUI: function () {
                var _self = this,
                    triggerEl = _self.getTrigger(),
                    textEl = _self._getTextEl(),
                    triggerEvent = _self.get("triggerEvent"),
                    dropdownMenu = _self.get("dropdownMenu"),
                    valueField = _self.get("valueField");

                triggerEl.on(triggerEvent, function () {
                    dropdownMenu.toggle();
                });

                textEl.on(triggerEvent, function () {
                    dropdownMenu.toggle();
                });
             
                //选中列表
                dropdownMenu.on("itemclick",
                    function (ev) {
                        //隐藏列表
                        dropdownMenu.hide();
                        //回填text
                        textEl.val(ev.item.text);
                        //回填value
                        if (valueField) {
                            $(valueField).val(ev.item.value)
                        }
                        //触发change事件
                        if (ev.item) {
                            _self.fire("change", {
                                text: ev.text,
                                value: ev.value,
                                item: ev.item
                            });
                        }
                    });
            },
            containsElement: function (elem) {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                return Component.Controller.prototype.containsElement.call(this, elem) || picker.containsElement(elem);
            },
            getTrigger: function () {
                return this.get("el").find(this.get("triggerCls"));
            },
            _uiSetItems: function (items) {
                if (!items) {
                    return;
                }
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                dropdownMenu.set("items", formatItems(items));
                _self._syncValue();
            },
            _syncValue: function () {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu"),
                    textEl = _self._getTextEl(),
                    valueField = _self.get("valueField");
                if (valueField) {
                    dropdownMenu.setSelectedByField($(valueField).val());
                }
                if (textEl) {
                    textEl.val(dropdownMenu.getSelectedText());
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
                        var dropdownMenu = _self.get("dropdownMenu");
                        dropdownMenu.set("width", v);
                    }
                }
            },
            _uiSetDisabled: function (v) {
                var _self = this,
                    triggerEl = _self.getTrigger(),
                    textEl = _self._getTextEl();
                if (v) {
                    triggerEl.attr("disabled", v);
                    textEl && textEl.attr("disabled", v);
                }
            },
            destructor: function () {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                if (dropdownMenu) {
                    dropdownMenu.destroy();
                }
            },
            _getTextEl: function () {
                var _self = this,
                    el = _self.get("el");
                return el.is("input") ? el : el.find("input");
            },
            /*
            * 获取选中项值
            */
            getSelectedValue: function () {
                return this.get("dropdownMenu").getSelectedValue();
            },
            /*
            * 设置选中项
            */
            setSelectedValue: function (value) {
                var _self = this,
                    dropdownMenu = _self.get("dropdownMenu");
                dropdownMenu.setSelectedByField(value);
            },
            /*
             * 获取选中项文本
             */
            getSelectedText: function () {
                return this.get("dropdownMenu").getSelectedText();
            }
        },
            {
                ATTRS: {
                    dropdownMenu: {},
                    valueField: {
                      
                    },
                    focusable: {
                        value: false
                    },
                    multipleSelect: {
                        value: false
                    },
                    items: {
                        sync: false
                    },
                    forceFit: {
                        value: true
                    },
                    triggerCls: {
                        value: "." + BUTTON_TOGGLE
                    },
                    inputCls: {
                        value: CLS_INPUT
                    },
                    events: {
                        value: {
                            change: false
                        }
                    },
                    //样式
                    elCls: {
                        value: "input-group"
                    },
                    tpl: {
                        value: '<input type="text" class="form-control ' + CLS_INPUT + '"><span class= "input-group-btn" ><button type="button" class="btn btn-default"><i class="iconfont icon-down"></i></button ></span >',
                    },
                    //触发事件
                    triggerEvent: {
                        value: "click"
                    }
                }
            },
            {
                xclass: "select"
            });
    BUI.Select.Select = Select;
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
                            iconEl = el.find(".input-group-append"),
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
                return el.find("button");
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
                        value: '<input type="text" class="form-control ' + CLS_INPUT + '"><span class= "input-group-btn" ><button type="button" class="btn btn-default"><i class="iconfont icon-search"></i></button ></span >',
                    }
                }
            },
            {
                xclass: "search"
            });
    BUI.Select.Search = search;
})(window.BUI, jQuery);
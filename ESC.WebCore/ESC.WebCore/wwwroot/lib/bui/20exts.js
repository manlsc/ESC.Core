//BUI.Picker.TreePicker
(function (BUI, $) {
    "use strict";
    var Tree = BUI.Tree.TreeList,
    Picker = BUI.Picker.Picker,
    treePicker = Picker.extend({
        initializer: function () {
            var _self = this,
            children = _self.get('children'),
            tree = _self.get('tree');
            if (!tree) {
                children.push({});
            }
        },
        setSelectedValue: function (value) {
            value = BUI.isNullOrEmpty(value) ? '' : value.toString();
            var _self = this,
              tree = _self.get('tree');
            if (!_self.get('isInit')) {
                _self._initControl();
            }
            if (_self.get('selectStatus') === 'selected') { //如果不使用勾选
                //if (value) {
                //    tree.expandNode(value);
                //}
                var selectedValue = _self.getSelectedValue();
                if (value !== selectedValue && tree.getCount()) {
                    if (tree.get('multipleSelect')) {
                        tree.clearSelection();
                    }
                    tree.setSelectionByField(value.split(','));
                }
            } else {
                tree.clearAllChecked();
                var arr = value.split(',');
                BUI.each(arr, function (id) {
                    tree.setChecked(id);
                });
            }
        },
        getSelectedValue: function () {
            var _self = this,
            tree = _self.get('tree');
            if (_self.get('selectStatus') === 'selected') { //如果不使用勾选
                return tree.getSelectionValues().join(',');
            }

            var nodes = tree.getCheckedNodes();
            nodes = _self._getFilterNodes(nodes);
            return BUI.Array.map(nodes, function (node) {
                return node.id;
            }).join(',');
        },
        getSelectedText: function () {
            var _self = this,
            tree = _self.get('tree');

            if (_self.get('selectStatus') === 'selected') { //如果不使用勾选
                return tree.getSelectionText().join(',');
            }

            var nodes = tree.getCheckedNodes();
            nodes = _self._getFilterNodes(nodes);
            return BUI.Array.map(nodes, function (node) {
                return node.text;
            }).join(',');
        },
        onChange: function (selText, selValue, ev) {
            var _self = this,
            curTrigger = _self.get('curTrigger');
            _self.fire('selectedchange', {
                value: selValue,
                text: selText,
                curTrigger: curTrigger,
                item: ev.item
            });
        },
        //获取过滤的节点itemunselected
        _getFilterNodes: function (nodes) {
            var _self = this,
              filter = _self.get('filter');
            if (filter) {
                nodes = BUI.Array.filter(nodes, filter);
            }
            return nodes;
        }
    },
    {
        ATTRS: {
            defaultChildClass: {
                value: 'tree-list'
            },
            selectStatus: {
                value: 'selected'
            },
            changeEvent: {
                getter: function () {
                    return this.get('selectStatus') + 'change';
                }
            },
            hideEvent: {
                getter: function (v) {
                    if (this.get('selectStatus') === 'checked') {
                        return null;
                    }
                    return v;
                }
            },
            filter: {
            },
            tree: {
                getter: function () {
                    return this.get('children')[0];
                }
            }
        }
    },
    {
        xclass: 'tree-picker'
    });
    BUI.Picker.TreePicker = treePicker;
})(window.BUI, jQuery);
//BUI.Select.TreeSelect
(function (BUI, $) {
    "use strict";
    var TreePicker = BUI.Picker.TreePicker,
    PREFIX = BUI.prefix;
    var Component = BUI.Component,
    CLS_INPUT = PREFIX + "tree-input",
    treeSelect = Component.Controller.extend({
        initializer: function () {
            var _self = this,
            multipleSelect = _self.get("multipleSelect"),
            store = new BUI.Data.TreeStore(),
            picker = _self.get("picker"),
            tree;
            if (!picker) {
                var checkType = multipleSelect ? "all" : "none";
                tree = _self.get("tree") || {};
                tree = BUI.mix(tree, {
                    checkType: checkType,
                    xclass: "tree-list",
                    store: store
                });
                picker = new TreePicker({
                    children: [tree],
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
            nodes = _self.get("nodes"),
            textEl = _self._getTextEl();
            picker.set("trigger", _self.getTrigger());
            picker.set("triggerEvent", _self.get("triggerEvent"));
            picker.set("autoSetValue", _self.get("autoSetValue"));
            picker.set("textField", textEl);
            picker.set("width", _self.get("width") || 180);
            picker.render();
            picker.get("tree").get("store").setResult(nodes);
            _self.set("tree", picker.get("tree"));
        },
        bindUI: function () {
            var _self = this,
            picker = _self.get("picker"),
            tree = picker.get("tree");
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
                tree.on("itemsshow",
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
        _uiSetNodes: function (nodes) {
            if (!nodes) {
                return;
            }
            var _self = this,
            picker = _self.get("picker"),
            tree = picker.get("tree");
            tree.get("store").setResult(nodes);
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
                    iconEl = el.find(".input-group-button"),
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
            tree: {},
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
            nodes: {
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
                value: '<input type="text" class="form-control ' + CLS_INPUT + '"/><span class="input-group-button"><button class="btn btn-default" type="button"><i class="fa fa-caret-down"></i></button></span>'
            },
            triggerEvent: {
                value: "click"
            }
        }
    },
    {
        xclass: "tree-select"
    });
    BUI.Select.TreeSelect = treeSelect;
})(window.BUI, jQuery);
//BUI.Form.Field.Tree
(function (BUI, $) {
    "use strict";
    var Select = BUI.Select,
    Field = BUI.Form.Field.Base;
    var treeField = Field.extend({
        renderUI: function () {
            var _self = this,
            innerControl = _self.getInnerControl(),
            tree = _self.get("tree");
            if (_self.get("srcNode") && innerControl.is("tree")) {
                return;
            }
            if ($.isPlainObject(tree)) {
                _self._initTree(tree);
            }
        },
        _initTree: function (tree) {
            var _self = this,
            nodes = _self.get("nodes");
            tree.render = _self.getControlContainer();
            tree.valueField = _self.getInnerControl();
            tree.autoRender = true;
            tree.nodes = nodes;
            tree = new Select.TreeSelect(tree);
            _self.set("tree", tree);
            _self.set("isCreate", true);
            _self.get("children").push(tree);
        },
        setControlValue: function (value) {
            var _self = this,
            tree = _self.get("tree");
            if (tree && tree.set && tree.getSelectedValue() !== value) {
                tree.setSelectedValue(value);
            }
        },
        getSelectedText: function () {
            var _self = this,
            tree = _self.get("tree"),
            innerControl = _self.getInnerControl();
            if (innerControl.is("tree")) {
                var dom = innerControl[0],
                item = dom.options[dom.selectedIndex];
                return item ? item.text : "";
            } else {
                return tree.getSelectedText();
            }
        },
        setInnerWidth: function (width) {
            var _self = this,
            innerControl = _self.getInnerControl(),
            tree = _self.get("tree"),
            appendWidth = innerControl.outerWidth() - innerControl.width();
            innerControl.width(width - appendWidth);
            if (tree && tree.set) {
                tree.set("width", width);
            }
        }
    },
    {
        ATTRS: {
            nodes: {},
            controlTpl: {
                value: '<input type="hidden"/>'
            },
            emptyText: {
                value: "请选择"
            },
            tree: {
                shared: false,
                value: {}
            }
        }
    },
    {
        xclass: "form-field-tree"
    });
    BUI.Form.Field.Tree = treeField;
})(window.BUI, jQuery);
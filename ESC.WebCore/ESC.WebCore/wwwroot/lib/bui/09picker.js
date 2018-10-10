//BUI.Picker
(function (BUI, $) {
    "use strict";
    BUI.Picker = {};
    var Overlay = BUI.Overlay.Overlay;
    var picker = Overlay.extend({
        bindUI: function () {
            var _self = this;
            _self.on('show',
            function (ev) {
                if (!_self.get('isInit')) {
                    _self._initControl();
                }
                if (_self.get('autoSetValue')) {
                    var valueField = _self.get('valueField') || _self.get('textField') || _self.get('curTrigger'),
                    val = $(valueField).val();
                    _self.setSelectedValue(val);
                }
            });
        },
        _initControl: function () {
            var _self = this;
            if (_self.get('isInit')) {
                return;
            }
            if (!_self.get('innerControl')) {
                var control = _self.createControl();
                _self.get('children').push(control);
            }
            _self.initControlEvent();
            _self.set('isInit', true);
        },
        initControl: function () {
            this._initControl();
        },
        createControl: function () { },
        initControlEvent: function () {
            var _self = this,
            innerControl = _self.get('innerControl'),
            trigger = $(_self.get('trigger')),
            hideEvent = _self.get('hideEvent');
            innerControl.on(_self.get('changeEvent'),
            function (e) {
                var curTrigger = _self.get('curTrigger'),
                textField = _self.get('textField') || curTrigger || trigger,
                valueField = _self.get('valueField'),
                selValue = _self.getSelectedValue(),
                isChange = false;
                if (textField) {
                    var selText = _self.getSelectedText(),
                    preText = $(textField).val();
                    if (selText != preText) {
                        $(textField).val(selText);
                        isChange = true;
                        $(textField).trigger('change');
                    }
                }
                if (valueField && _self.get('autoSetValue')) {
                    var preValue = $(valueField).val();
                    if (valueField != preValue) {
                        $(valueField).val(selValue);
                        isChange = true;
                        $(valueField).trigger('change');
                    }
                }
                if (isChange) {
                    _self.onChange(selText, selValue, e);
                }
            });
            if (hideEvent) {
                innerControl.on(_self.get('hideEvent'),
                function () {
                    var curTrigger = _self.get('curTrigger');
                    try {
                        if (curTrigger) {
                            curTrigger.focus();
                        }
                    } catch (e) {
                        BUI.log(e);
                    }
                    _self.hide();
                });
            }
        },
        setSelectedValue: function (val) { },
        getSelectedValue: function () { },
        getSelectedText: function () { },
        focus: function () {
            this.get('innerControl').focus();
        },
        onChange: function (selText, selValue, ev) {
            var _self = this,
            curTrigger = _self.get('curTrigger');
            _self.fire('selectedchange', {
                value: selValue,
                text: selText,
                curTrigger: curTrigger
            });
        },
        _uiSetValueField: function (v) {
            var _self = this;
            if (v != null && v !== '' && _self.get('autoSetValue')) {
                _self.setSelectedValue($(v).val());
            }
        },
        _getTextField: function () {
            var _self = this;
            return _self.get('textField') || _self.get('curTrigger');
        }
    },
    {
        ATTRS: {
            innerControl: {
                getter: function () {
                    return this.get('children')[0];
                }
            },
            triggerEvent: {
                value: 'click'
            },
            autoSetValue: {
                value: true
            },
            changeEvent: {
                value: 'selectedchange'
            },
            autoHide: {
                value: true
            },
            hideEvent: {
                value: 'itemclick'
            },
            zIndex: {
                value: 1090
            },
            textField: {},
            align: {
                value: {
                    points: ['bl', 'tl'],
                    offset: [0, 0]
                }
            },
            valueField: {}
        }
    },
    {
        xclass: 'picker'
    });
    BUI.Picker.Picker = picker;
})(window.BUI, jQuery);
//BUI.Picker.ListPicker
(function (BUI, $) {
    "use strict";
    var List = BUI.List,
    Picker = BUI.Picker.Picker,
    listPicker = Picker.extend({
        initializer: function () {
            var _self = this,
            children = _self.get('children'),
            list = _self.get('list');
            if (!list) {
                children.push({});
            }
        },
        setSelectedValue: function (val) {
            val = BUI.isNullOrEmpty(val) ? '' : val.toString();
            if (!this.get('isInit')) {
                this._initControl();
            }
            var _self = this,
            list = _self.get('list'),
            selectedValue = _self.getSelectedValue();
            if (val !== selectedValue && list.getCount()) {
                if (list.get('multipleSelect')) {
                    list.clearSelection();
                }
                list.setSelectionByField(val.split(','));
            }
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
        getSelectedValue: function () {
            return this.get('list').getSelectionValues().join(',');
        },
        getSelectedText: function () {
            return this.get('list').getSelectionText().join(',');
        }
    },
    {
        ATTRS: {
            defaultChildClass: {
                value: 'simple-list'
            },
            list: {
                getter: function () {
                    return this.get('children')[0];
                }
            }
        }
    },
    {
        xclass: 'list-picker'
    });
    BUI.Picker.ListPicker = listPicker;
})(window.BUI, jQuery);
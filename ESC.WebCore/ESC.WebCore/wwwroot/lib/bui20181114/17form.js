//BUI.Form.Rule
(function (BUI, $) {
    "use strict";
    BUI.Form = {};
    var Rule = function (config) {
        Rule.superclass.constructor.call(this, config);
    };
    BUI.extend(Rule, BUI.Base);
    Rule.ATTRS = {
        /**
    	 * 验证名称
    	 */
        name: {},
        /**
         * 错误消息
         */
        msg: {},
        /**
         * 验证器
         */
        validator: {
            value: function (value, baseValue, formatedMsg, control) { }
        }
    };
    function valid(self, value, baseValue, msg, control) {
        if (BUI.isArray(baseValue) && BUI.isString(baseValue[1])) {
            if (baseValue[1]) {
                msg = baseValue[1];
            }
            baseValue = baseValue[0];
        }
        var validator = self.get("validator"),
            formatedMsg = formatError(self, baseValue, msg),
            valid = true;
        value = value == null ? "" : value;
        return validator.call(self, value, baseValue, formatedMsg, control);
    }
    /**
     * 格式化错误信息
     * @param {Object} self
     * @param {Object} values
     * @param {Object} msg
     */
    function formatError(self, values, msg) {
        var ars = parseParams(values);
        msg = msg || self.get("msg");
        return BUI.substitute(msg, ars);
    }
    function parseParams(values) {
        if (values == null) {
            return {};
        }
        if ($.isPlainObject(values)) {
            return values;
        }
        var ars = values,
            rst = {};
        if (BUI.isArray(values)) {
            for (var i = 0; i < ars.length; i++) {
                rst[i] = ars[i];
            }
            return rst;
        }
        return {
            "0": values
        };
    }
    BUI.augment(Rule, {
        valid: function (value, baseValue, msg, control) {
            var _self = this;
            return valid(_self, value, baseValue, msg, control);
        }
    });
    BUI.Form.Rule = Rule;
})(window.BUI, jQuery);
//BUI.Form.Rules
(function (BUI, $) {
    "use strict";
    var Rule = BUI.Form.Rule;
    function toNumber(value) {
        return parseFloat(value);
    }
    function toDate(value) {
        return BUI.Date.parse(value);
    }
    var ruleMap = {};
    var rules = {
        add: function (rule) {
            var name;
            if ($.isPlainObject(rule)) {
                name = rule.name;
                ruleMap[name] = new Rule(rule);
            } else if (rule.get) {
                name = rule.get("name");
                ruleMap[name] = rule;
            }
            return ruleMap[name];
        },
        remove: function (name) {
            delete ruleMap[name];
        },
        get: function (name) {
            return ruleMap[name];
        },
        valid: function (name, value, baseValue, msg, control) {
            var rule = rules.get(name);
            if (rule) {
                return rule.valid(value, baseValue, msg, control);
            }
            return null;
        },
        isValid: function (name, value, baseValue, control) {
            return rules.valid(name, value, baseValue, control) == null;
        }
    };
    var required = rules.add({
        name: "required",
        msg: "不能为空！",
        validator: function (value, required, formatedMsg) {
            if (required !== false && /^\s*$/.test(value)) {
                return formatedMsg;
            }
        }
    });
    var equalTo = rules.add({
        name: "equalTo",
        msg: "两次输入不一致！",
        validator: function (value, equalTo, formatedMsg) {
            var el = $(equalTo);
            if (el.length) {
                equalTo = el.val();
            }
            return value === equalTo ? undefined : formatedMsg;
        }
    });
    var min = rules.add({
        name: "min",
        msg: "输入值不能小于{0}！",
        validator: function (value, min, formatedMsg) {
            if (BUI.isString(value)) {
                value = value.replace(/\,/g, "");
            }
            if (value !== "" && toNumber(value) < toNumber(min)) {
                return formatedMsg;
            }
        }
    });
    var max = rules.add({
        name: "max",
        msg: "输入值不能大于{0}！",
        validator: function (value, max, formatedMsg) {
            if (BUI.isString(value)) {
                value = value.replace(/\,/g, "");
            }
            if (value !== "" && toNumber(value) > toNumber(max)) {
                return formatedMsg;
            }
        }
    });
    var length = rules.add({
        name: "length",
        msg: "输入值长度为{0}！",
        validator: function (value, len, formatedMsg) {
            if (value != null) {
                value = $.trim(value.toString());
                if (len != value.length) {
                    return formatedMsg;
                }
            }
        }
    });
    var minlength = rules.add({
        name: "minlength",
        msg: "输入值长度不小于{0}！",
        validator: function (value, min, formatedMsg) {
            if (value != null) {
                value = $.trim(value.toString());
                var len = value.length;
                if (len < min) {
                    return formatedMsg;
                }
            }
        }
    });
    var maxlength = rules.add({
        name: "maxlength",
        msg: "输入值长度不大于{0}！",
        validator: function (value, max, formatedMsg) {
            if (value) {
                value = $.trim(value.toString());
                var len = value.length;
                if (len > max) {
                    return formatedMsg;
                }
            }
        }
    });
    var regexp = rules.add({
        name: "regexp",
        msg: "输入值不符合{0}！",
        validator: function (value, regexp, formatedMsg) {
            if (regexp) {
                return regexp.test(value) ? undefined : formatedMsg;
            }
        }
    });
    var email = rules.add({
        name: "email",
        msg: "不是有效的邮箱地址！",
        validator: function (value, baseValue, formatedMsg) {
            value = $.trim(value);
            if (value) {
                return /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(value) ? undefined : formatedMsg;
            }
        }
    });
    var date = rules.add({
        name: "date",
        msg: "不是有效的日期！",
        validator: function (value, baseValue, formatedMsg) {
            if (BUI.isNumber(value)) {
                return;
            }
            if (BUI.isDate(value)) {
                return;
            }
            value = $.trim(value);
            if (value) {
                return BUI.Date.isDateString(value) ? undefined : formatedMsg;
            }
        }
    });
    var minDate = rules.add({
        name: "minDate",
        msg: "输入日期不能小于{0}！",
        validator: function (value, minDate, formatedMsg) {
            if (value) {
                var date = toDate(value);
                if (date && date < toDate(minDate)) {
                    return formatedMsg;
                }
            }
        }
    });
    var maxDate = rules.add({
        name: "maxDate",
        msg: "输入日期不能大于{0}！",
        validator: function (value, maxDate, formatedMsg) {
            if (value) {
                var date = toDate(value);
                if (date && date > toDate(maxDate)) {
                    return formatedMsg;
                }
            }
        }
    });
    var mobile = rules.add({
        name: "mobile",
        msg: "不是有效的手机号码！",
        validator: function (value, baseValue, formatedMsg) {
            value = $.trim(value);
            if (value) {
                return /^\d{11}$/.test(value) ? undefined : formatedMsg;
            }
        }
    });
    var number = rules.add({
        name: "number",
        msg: "不是有效的数字！",
        validator: function (value, baseValue, formatedMsg) {
            if (BUI.isNumber(value)) {
                return;
            }
            value = value.replace(/\,/g, "");
            return !isNaN(value) ? undefined : formatedMsg;
        }
    });
    BUI.Form.Rules = rules;
})(window.BUI, jQuery);
//BUI.Form.Field.Base
(function (BUI, $) {
    "use strict";
    BUI.Form.Field = {};
    var Component = BUI.Component,
        Rules = BUI.Form.Rules,
        Tooltip = BUI.Tooltip,
        CLS_FIELD_ERROR = "has-error";
    var field = Component.Controller.extend({
        renderUI: function () {
            var _self = this,
                control = _self.get("control");
            if (!control) {
                var controlTpl = _self.get("controlTpl"),
                    container = _self.getControlContainer();
                if (controlTpl) {
                    var control = $(controlTpl).appendTo(container);
                    _self.set("control", control);
                }
            } else {
                _self.set("controlContainer", control.parent());
            }            
        },
        bindUI: function () {
            var _self = this;
            _self.on("afterDisabledChange",
                function (ev) {
                    var disabled = ev.newVal;
                    if (disabled) {
                        _self.clearErrors(false, false);
                    } else {
                        _self.valid();
                    }
                });
        },
        validRules: function (rules, value) {
            if (!rules) {
                return null;
            }
            if (this.get("pauseValid")) {
                return null;
            }
            var _self = this,
                messages = _self._getValidMessages(),
                error = null;
            for (var name in rules) {
                if (rules.hasOwnProperty(name)) {
                    var baseValue = rules[name];
                    error = Rules.valid(name, value, baseValue, messages[name], _self);
                    if (error) {
                        break;
                    }
                }
            }
            return error;
        },
        _getValidMessages: function () {
            var _self = this,
                defaultMessages = _self.get("defaultMessages"),
                messages = _self.get("messages");
            return BUI.merge(defaultMessages, messages);
        },
        getValidError: function (value) {
            var _self = this,
                validator = _self.get("validator"),
                error = null;
            error = _self.validRules(_self.get("defaultRules"), value) || _self.validRules(_self.get("rules"), value);
            if (!error && !this.get("pauseValid")) {
                if (_self.parseValue) {
                    value = _self.parseValue(value);
                }
                error = validator ? validator.call(this, value) : "";
            }
            return error;
        },
        showErrors: function (errors) {
            var _self = this,
                errors = errors || _self.getErrors();
            _self.clearViewErrors();
            if (!_self.get("showError")) {
                return;
            }

            if (errors && errors.length) {
                _self.showError(errors[0]);
            }
        },
        /**
         * 添加验证
         * @param {Object} name
         * @param {Object} value
         * @param {Object} message
         */
        addRule: function (name, value, message) {
            var _self = this,
                rules = _self.get("rules"),
                messages = _self.get("messages");
            rules[name] = value;
            if (message) {
                messages[name] = message;
            }
        },
        /**
         * 添加验证
         * @param {Object} rules
         * @param {Object} messages
         */
        addRules: function (rules, messages) {
            var _self = this;
            BUI.each(rules,
                function (value, name) {
                    var msg = messages ? messages[name] : null;
                    _self.addRule(name, value, msg);
                });
        },
        /**
         * 删除验证
         * @param {Object} name
         */
        removeRule: function (name) {
            var _self = this,
                rules = _self.get("rules");
            delete rules[name];
        },
        /**
         * 清空验证
         */
        clearRules: function () {
            var _self = this;
            _self.set("rules", {});
        },
        /**
         * 获取控件容器
         */
        getControlContainer: function () {
            var _self = this,
                el = _self.get("el"),
                controlContainer = _self.get("controlContainer");
            if (controlContainer) {
                if (BUI.isString(controlContainer)) {
                    controlContainer = el.find(controlContainer);
                }
            }
            return controlContainer && controlContainer.length ? controlContainer : el;
        },
        /**
         * 删除错误提示
         */
        clearViewErrors: function () {
            var _self = this,
                errTooltip = _self.get("errTooltip");
            if (errTooltip) {
                errTooltip.destroy();
                _self.set("errTooltip", null);
            }
            _self.get("el").removeClass(CLS_FIELD_ERROR);
        },
        /**
         * 显示错误提示
         * @param {Object} msg
         */
        showError: function (msg) {
            var _self = this,
                el = _self.get("el"),
                errTooltip = _self.get("errTooltip");
            if (errTooltip) {
                return;
            }
            errTooltip = new Tooltip.Tip({
                trigger: el,
                alignType: 'right',
                title: msg
            });
            errTooltip.render();
            _self.set("errTooltip", errTooltip);
            _self.get("el").addClass(CLS_FIELD_ERROR);
        },
        isField: true,
        onChange: function () {
            this.fire("change");
        },
        isCurrentValue: function (value) {
            return value == this.get("value");
        },
        _clearError: function () {
            this.set("error", null);
            this.clearViewErrors();
        },
        _setError: function (msg) {
            this.set("error", msg);
            this.showErrors();
        },
        getControlValue: function (innerControl) {
            var _self = this;
            innerControl = innerControl || _self.getInnerControl();
            return innerControl.val();
        },
        /**
         * 设置控制器value
         * @param {Object} value
         */
        setControlValue: function (value) {
            var _self = this,
                innerControl = _self.getInnerControl();
            innerControl.val(value);
        },
        parseValue: function (value) {
            return value;
        },
        onValid: function () {
            var _self = this,
                value = _self.getControlValue();
            value = _self.parseValue(value);
            if (!_self.isCurrentValue(value)) {
                _self.setInternal('value', value);
                _self.onChange();
            }
        },
        valid: function () {
            var _self = this;
            _self.validControl();
        },
        validControl: function (value) {
            var _self = this,
                errorMsg;
            var value = value || _self.getControlValue(),
                preError = _self.get("error");
            var errorMsg = _self.getValidError(value);
            if (errorMsg) {
                _self._setError(errorMsg);
                _self.fire("error", {
                    msg: errorMsg,
                    value: value
                });
                if (preError !== errorMsg) {
                    _self.fire("validchange", {
                        valid: false
                    });
                }
            } else {
                _self._clearError();
                _self.fire("valid");
                if (preError) {
                    _self.fire("validchange", {
                        valid: true
                    });
                }
            }
            return !errorMsg;
        },
        focus: function () {
            this.getInnerControl().focus();
        },
        change: function () {
            var control = this.getInnerControl();
            control.change();
        },
        blur: function () {
            this.getInnerControl().blur();
        },
        isValid: function () {
            var _self = this;
            _self.validControl();
            return !_self.get("error");
        },
        getError: function () {
            return this.get("error");
        },
        getErrors: function () {
            var error = this.getError();
            if (error) {
                return [error];
            }
            return [];
        },
        clearErrors: function (reset) {
            var _self = this;
            _self._clearError();
            if (reset && _self.getControlValue() != _self.get("value")) {
                if (BUI.isNullOrEmpty(_self.get("value"))) {
                    _self.setControlValue(_self.get("value"));
                }
            }
        },
        /**
         * 获取控制器
         */
        getInnerControl: function () {
            return this.get("control");
        },
        destructor: function () {
            var _self = this,
                errTooltip = _self.get("errTooltip");
            if (errTooltip) {
                errTooltip.destroy();
                _self.set("errTooltip", null);
            }
            var datePicker = _self.get("datePicker");
            if (datePicker) {
                datePicker.destroy();
                _self.set("datePicker", null);
            }
            var select = _self.get("select");
            if (select) {
                select.destroy();
                _self.set("select", null);
            }
        },
        /**
         * 设置内部控件的宽度
         * @param {Object} width
         */
        setInnerWidth: function (width) {
            var _self = this,
                innerControl = _self.getInnerControl(),
                siblings = innerControl.siblings(),
                appendWidth = innerControl.outerWidth() - innerControl.width();
            BUI.each(siblings,
                function (dom) {
                    appendWidth += $(dom).outerWidth();
                });
            innerControl.width(width - appendWidth);
        },
        _uiSetValue: function (v) {
            var _self = this;
            _self.setControlValue(v);
            if (_self.get("rendered")) {
                _self.validControl(v);
                _self.onChange();
            }
        },
        _uiSetName: function (v) {
            var _self = this;
            _self.get("control").attr("name", v);
        },
        _uiSetDisabled: function (v) {
            var _self = this,
                innerControl = _self.getInnerControl(),
                children = _self.get("children");
            innerControl.attr("disabled", v);
            if (_self.get("rendered")) {
                if (v) {
                    _self.clearErrors();
                }
                if (!v) {
                    _self.valid();
                }
            }
            BUI.each(children,
                function (child) {
                    child.set("disabled", v);
                });
        },
        _uiSetWidth: function (v) {
            var _self = this;
            _self.get("el").width(v);
            if (v != null && _self.get("forceFit")) {
                _self.setInnerWidth(v);
            }
        }
    },
        {
            ATTRS: {
                defaultRules: {
                    value: {}
                },
                defaultMessages: {
                    value: {}
                },
                rules: {
                    shared: false,
                    value: {}
                },
                messages: {
                    shared: false,
                    value: {}
                },
                validator: {},
                error: {},
                pauseValid: {
                    value: false
                },
                forceFit: {
                    value: false
                },
                name: {},
                showError: {
                    value: true
                },
                value: {},
                controlContainer: {},
                control: {},
                controlTpl: {
                    value: '<input class="form-control" type="text"/>'
                },
                prefixCls: {
                    value: BUI.prefix
                },
                focusable: {
                    value: true
                },
                events: {
                    value: {
                        error: false,
                        valid: false,
                        change: true,
                        validchange: true
                    }
                }
            }

        },
        {
            xclass: "form-field"
        });
    BUI.Form.Field.Base = field;
})(window.BUI, jQuery);
//BUI.Form.Field.Text
(function (BUI, $) {
    "use strict";
    var Field = BUI.Form.Field.Base;
    var textField = Field.extend({},
        {
            xclass: "form-field-text"
        });
    BUI.Form.Field.Text = textField;
})(window.BUI, jQuery);
//BUI.Form.Field.Hidden
(function (BUI, $) {
    "use strict";
    var Field = BUI.Form.Field.Base;
    var hiddenField = Field.extend({},
        {
            ATTRS: {
                controlTpl: {
                    value: '<input type="hidden"/>'
                },
                tpl: {
                    value: ""
                }
            }
        },
        {
            xclass: "form-field-hidden"
        });
    BUI.Form.Field.Hidden = hiddenField;
})(window.BUI, jQuery);
//BUI.Form.Field.Number
(function (BUI, $) {
    "use strict";
    var Field = BUI.Form.Field.Base;
    var numberField = Field.extend({
        parseValue: function (value) {
            if (value == "" || value == null) {
                return null;
            }
            if (BUI.isNumber(value)) {
                return value;
            }
            var _self = this,
                allowDecimals = _self.get("allowDecimals");
            value = value.replace(/\,/g, "");
            if (!allowDecimals) {
                return parseInt(value, 10);
            }
            return parseFloat(parseFloat(value).toFixed(_self.get("decimalPrecision")));
        },
        _uiSetMax: function (v) {
            this.addRule("max", v);
        },
        _uiSetMin: function (v) {
            this.addRule("min", v);
        }
    },
        {
            ATTRS: {
                max: {},
                min: {},
                decorateCfgFields: {
                    value: {
                        min: true,
                        max: true
                    }
                },
                defaultRules: {
                    value: {
                        number: true
                    }
                },
                allowDecimals: {
                    value: true
                },
                decimalPrecision: {
                    value: 2
                },
                step: {
                    value: 1
                }
            }
        },
        {
            xclass: "form-field-number"
        });
    BUI.Form.Field.Number = numberField;
})(window.BUI, jQuery);
//BUI.Form.Field.Plain
(function (BUI, $) {
    "use strict";
    var Field = BUI.Form.Field.Base;

    var PlainField = Field.extend({
        _uiSetValue: function (v) {
            var _self = this,
                textEl = _self.get("textEl"),
                container = _self.getControlContainer(),
                renderer = _self.get("renderer"),
                text = renderer ? renderer(v) : v,
                width = _self.get("width"),
                appendWidth = 0,
                textTpl;
            if (textEl) {
                textEl.remove();
            }
            text = text || "&nbsp;";
            textTpl = BUI.substitute(_self.get("textTpl"), {
                text: text
            });
            textEl = $(textTpl).appendTo(container);
            appendWidth = textEl.outerWidth() - textEl.width();
            textEl.width(width - appendWidth);
            _self.set("textEl", textEl);
        }
    },
        {
            ATTRS: {
                controlTpl: {
                    value: '<input type="hidden"/>'
                },
                textTpl: {
                    value: '<span class="x-form-text">{text}</span>'
                },
                renderer: {
                    value: function (value) {
                        return value;
                    }
                },
                tpl: {
                    value: ""
                }
            }
        },
        {
            xclass: "form-field-plain"
        });
    BUI.Form.Field.Plain = PlainField;
})(window.BUI, jQuery);
//BUI.Form.Field.TextArea
(function (BUI, $) {
    "use strict";
    var Field = BUI.Form.Field.Base;
    var TextAreaField = Field.extend({
        _uiSetRows: function (v) {
            var _self = this,
                innerControl = _self.getInnerControl();
            if (v) {
                innerControl.attr("rows", v);
            }
        },
        _uiSetCols: function (v) {
            var _self = this,
                innerControl = _self.getInnerControl();
            if (v) {
                innerControl.attr("cols", v);
            }
        }
    },
        {
            ATTRS: {
                controlTpl: {
                    value: '<textarea class="form-control"></textarea>'
                },
                rows: {},
                cols: {},
                decorateCfgFields: {
                    value: {
                        rows: true,
                        cols: true
                    }
                }
            }
        },
        {
            xclass: "form-field-textarea"
        });
    BUI.Form.Field.TextArea = TextAreaField;
})(window.BUI, jQuery);
//BUI.Form.Field.Date
(function (BUI, $) {
    "use strict";
    var Field = BUI.Form.Field.Base,
        DateUtil = BUI.Date,
        Calendar = BUI.Calendar;
    var dateField = Field.extend({
        renderUI: function () {
            var _self = this,
                datePicker = _self.get("datePicker");
            if ($.isPlainObject(datePicker)) {
                _self.initDatePicker(datePicker);
            }
            if (datePicker.get && datePicker.get("showTime") || datePicker.showTime) {
                _self.getInnerControl().addClass("calendar-time");
            }
        },
        initDatePicker: function (datePicker) {
            var _self = this;
            datePicker.trigger = _self.getInnerControl();
            datePicker.autoRender = true;
            datePicker = new Calendar.DatePicker(datePicker);
            _self.set("datePicker", datePicker);
            _self.set("isCreatePicker", true);
            _self.get("children").push(datePicker);

        },
        setControlValue: function (value) {
            var _self = this,
                innerControl = _self.getInnerControl();
            if (BUI.isDate(value)) {
                value = DateUtil.format(value, _self._getFormatMask());
            }
            innerControl.val(value);
        },
        _getFormatMask: function () {
            var _self = this,
                datePicker = _self.get("datePicker");
            if (datePicker.showTime || datePicker.get && datePicker.get("showTime")) {
                return "yyyy-mm-dd HH:MM:ss";
            }
            return "yyyy-mm-dd";
        },
        parseValue: function (value) {
            if (BUI.isNumber(value)) {
                return new Date(value);
            }
            return DateUtil.parse(value);
        },
        isCurrentValue: function (value) {
            return DateUtil.isEquals(value, this.get("value"));
        },
        _uiSetMax: function (v) {
            this.addRule("max", v);
            var _self = this,
                datePicker = _self.get("datePicker");
            if (datePicker) {
                if (datePicker.set) {
                    datePicker.set("maxDate", v);
                } else {
                    datePicker.maxDate = v;
                }
            }
        },
        _uiSetMin: function (v) {
            this.addRule("min", v);
            var _self = this,
                datePicker = _self.get("datePicker");
            if (datePicker) {
                if (datePicker.set) {
                    datePicker.set("minDate", v);
                } else {
                    datePicker.minDate = v;
                }
            }
        }
    },
        {
            ATTRS: {
                controlTpl: {
                    value: '<input type="text" class="form-control calendar"/>'
                },
                defaultRules: {
                    value: {
                        date: true
                    }
                },
                max: {},
                min: {},
                value: {
                    setter: function (v) {
                        if (BUI.isNumber(v)) {
                            return new Date(v);
                        }
                        return v;
                    }
                },
                datePicker: {
                    shared: false,
                    value: {}
                },
                isCreatePicker: {
                    value: true
                }
            }
        },
        {
            xclass: "form-field-date"
        });
    BUI.Form.Field.Date = dateField;
})(window.BUI, jQuery);
//BUI.Form.Field.Select
(function (BUI, $) {
    "use strict";
    var Select = BUI.Select,
        Field = BUI.Form.Field.Base;
    function resetOptions(select, options, self) {
        select.children().remove();
        var emptyText = self.get("emptyText");
        if (emptyText && self.get("showBlank")) {
            appendItem("", emptyText, select);
        }
        BUI.each(options,
            function (option) {
                appendItem(option.value, option.text, select);
            });
    }
    function appendItem(value, text, select) {
        var option = new Option(text, value),
            options = select[0].options;
        options[options.length] = option;
    }
    var selectField = Field.extend({
        renderUI: function () {
            var _self = this,
                innerControl = _self.getInnerControl(),
                select = _self.get("select");
            if (_self.get("srcNode") && innerControl.is("select")) {
                return;
            }
            if ($.isPlainObject(select)) {
                _self._initSelect(select);
            }
        },
        _initSelect: function (select) {
            var _self = this,
                items = _self.get("items");
            select.render = _self.getControlContainer();
            select.valueField = _self.getInnerControl();
            select.autoRender = true;
            select.items = _self.setItems(items);
            select = new Select.Select(select);
            _self.set("select", select);
            _self.set("isCreate", true);
            _self.get("children").push(select);
        },
        setItems: function (items) {
            var _self = this;
            if ($.isPlainObject(items)) {
                var tmp = [];
                BUI.each(items,
                    function (v, n) {
                        tmp.push({
                            value: n,
                            text: v
                        });
                    });
                items = tmp;
            }
            return items;
        },
        setControlValue: function (value) {
            var _self = this,
                select = _self.get("select");
            if (select && select.set && select.getSelectedValue() !== value) {
                select.setSelectedValue(value);
            }
        },
        getSelectedText: function () {
            var _self = this,
                select = _self.get("select"),
                innerControl = _self.getInnerControl();
            if (innerControl.is("select")) {
                var dom = innerControl[0],
                    item = dom.options[dom.selectedIndex];
                return item ? item.text : "";
            } else {
                return select.getSelectedText();
            }
        },
        setInnerWidth: function (width) {
            var _self = this,
                innerControl = _self.getInnerControl(),
                select = _self.get("select"),
                appendWidth = innerControl.outerWidth() - innerControl.width();
            innerControl.width(width - appendWidth);
            if (select && select.set) {
                select.set("width", width);
            }
        }
    },
        {
            ATTRS: {
                items: {},
                controlTpl: {
                    value: '<input type="hidden"/>'
                },
                showBlank: {
                    value: true
                },
                forceFit: {
                    value: true
                },
                emptyText: {
                    value: "请选择"
                },
                select: {
                    shared: false,
                    value: {}
                }
            }
        },
        {
            xclass: "form-field-select"
        });
    BUI.Form.Field.Select = selectField;
})(window.BUI, jQuery);
//BUI.Form.Field.Search
(function (BUI, $) {
    "use strict";
    var Field = BUI.Form.Field.Base,
        Select = BUI.Select;
    var searchField = Field.extend({
        renderUI: function () {
            var _self = this,
                innerControl = _self.getInnerControl(),
                search = _self.get("search");
            if (_self.get("srcNode") && innerControl.is("search")) {
                return;
            }
            if ($.isPlainObject(search)) {
                _self._initSearch(search);
            }
        },
        _initSearch: function (search) {
            var _self = this;
            search.render = _self.getControlContainer();
            search.valueField = _self.getInnerControl();
            search.autoRender = true;
            search = new Select.Search(search);
            _self.set("search", search);
            _self.set("isCreate", true);
            _self.get("children").push(search);
        },
        _uiSetText: function (val) {
            var _self = this;
            _self.setSearchText(val);
        },
        setControlValue: function (value) {
            var _self = this,
                search = _self.get("search");
            search.setSearchValue(value);
        },
        setSearchText: function (txt) {
            var _self = this,
                search = _self.get("search");
            search.setSearchText(txt);
        },
        setInnerWidth: function (width) {
            var _self = this,
                innerControl = _self.getInnerControl(),
                search = _self.get("search"),
                appendWidth = innerControl.outerWidth() - innerControl.width();
            innerControl.width(width - appendWidth);
            if (search && search.set) {
                search.set("width", width);
            }
        }
    }, {
            ATTRS: {
                controlTpl: {
                    value: '<input type="hidden"/>'
                },
                forceFit: {
                    value: true
                },
                search: {
                    shared: false,
                    value: {}
                },
                text: {
                    value: ""
                }
            }
        },
        {
            xclass: "form-field-search"
        });
    BUI.Form.Field.Search = searchField;
})(window.BUI, jQuery);
////BUI.Form.Field.Uploader
//(function (BUI, $) {
//    "use strict";
//})(window.BUI, jQuery);
//BUI.Form.Form
(function (BUI, $) {
    "use strict";
    var Field = BUI.Form.Field;
    var Form = BUI.Component.Controller.extend({
        getFields: function (name) {
            var _self = this,
                rst = [],
                children = _self.get("children");
            BUI.each(children,
                function (item) {
                    if (item instanceof Field.Base) {
                        if (!name || item.get("name") == name) {
                            rst.push(item);
                        }
                    } else if (item.getFields) {
                        rst = rst.concat(item.getFields(name));
                    }
                });
            return rst;
        },
        getRecord: function () {
            var _self = this,
                rst = {},
                fields = _self.getFields();
            BUI.each(fields,
                function (field) {
                    var name = field.get("name"),
                        value = _self._getFieldValue(field);
                    if (!rst[name]) {
                        rst[name] = value;
                    } else if (BUI.isArray(rst[name]) && value != null) {
                        rst[name].push(value);
                    } else if (value != null) {
                        var arr = [rst[name]];
                        arr.push(value);
                        rst[name] = arr;
                    }
                });
            return rst;
        },
        getField: function (name) {
            var _self = this,
                fields = _self.getFields(),
                rst = null;
            BUI.each(fields,
                function (field) {
                    if (field.get("name") === name) {
                        rst = field;
                        return false;
                    }
                });
            return rst;
        },
        getFieldAt: function (index) {
            return this.getFields()[index];
        },
        setFieldValue: function (name, value) {
            var _self = this,
                fields = _self.getFields(name);
            BUI.each(fields,
                function (field) {
                    _self._setFieldValue(field, value);
                });
        },
        _setFieldValue: function (field, value) {
            if (field.get("disabled")) {
                return;
            }
            if (value == null) {
                value = "";
            }
            field.clearErrors(true);
            field.set("value", value);
        },
        getFieldValue: function (name) {
            var _self = this,
                fields = _self.getFields(name),
                rst = [];
            BUI.each(fields,
                function (field) {
                    var value = _self._getFieldValue(field);
                    if (value) {
                        rst.push(value);
                    }
                });
            if (rst.length === 0) {
                return null;
            }
            if (rst.length === 1) {
                return rst[0];
            }
            return rst;
        },
        _getFieldValue: function (field) {
            return field.getControlValue() || "";
        },
        clearFields: function () {
            this.clearErrors(true);
            this.setRecord({});
        },
        setRecord: function (record) {
            var _self = this,
                fields = _self.getFields();
            BUI.each(fields,
                function (field) {
                    var name = field.get("name");
                    _self._setFieldValue(field, record[name]);
                });
        },
        updateRecord: function (record) {
            var _self = this,
                fields = _self.getFields();
            BUI.each(fields,
                function (field) {
                    var name = field.get("name");
                    if (record.hasOwnProperty(name)) {
                        _self._setFieldValue(field, record[name]);
                    }
                });
        },
        valid: function () {
            var _self = this,
                children = _self.get('children');
            BUI.each(children,
                function (item) {
                    if (!item.get('disabled')) {
                        item.valid();
                    }
                });
        },
        isValid: function () {
            var _self = this;
            return _self.isChildrenValid();
        },
        isChildrenValid: function () {
            var _self = this,
                children = _self.get("children"),
                isValid = true;
            BUI.each(children,
                function (item) {
                    if (!item.get("disabled") && !item.isValid()) {
                        isValid = false;
                        return false;
                    }
                });
            return isValid;
        },
        serializeToObject: function () {
            return BUI.FormHelper.serializeToObject(this.get("el")[0]);
        },
        getData: function (valid) {
            var _self = this;
            if (valid) {
                if (_self.isValid()) {
                    return _self.getRecord();
                }
            } else {
                return _self.getRecord();
            }
            return false;
        }
    },
        {
            ATTRS: {
                record: {
                    setter: function (v) {
                        this.setRecord(v);
                    },
                    getter: function () {
                        return this.getRecord();
                    }
                },
                tpl: {
                    value: '<div class="bui-form-fields"></div>'
                }
            }
        },
        {
            xclass: "form"
        });
    BUI.Form.Form = Form;
})(window.BUI, jQuery);
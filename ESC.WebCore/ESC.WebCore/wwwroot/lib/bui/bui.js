///<jscompress sourcefile="01common.js" />
//BUI +
(function ($) {
    "use strict";
    var win = window,
        doc = document,
        objectPrototype = Object.prototype,
        toString = objectPrototype.toString,
        BODY = "body",
        DOC_ELEMENT = "documentElement",
        SCROLL = "scroll",
        SCROLL_WIDTH = SCROLL + "Width",
        SCROLL_HEIGHT = SCROLL + "Height",
        ATTRS = "ATTRS",
        GUID_DEFAULT = "guid";
    win.BUI = win.BUI || {};
    $.extend(BUI, {
        /**
		 * 版本号
		 * @memberOf BUI
		 * @type {Number}
		 */
        version: "3.3.0",
        /**
		 * 是否为函数
		 * @param  {*} fn 对象
		 * @return {Boolean}  是否函数
		 */
        isFunction: function (fn) {
            return typeof fn === "function";
        },
        /**
		 * 是否数组
		 * @method
		 * @param  {*}  obj 是否数组
		 * @return {Boolean}  是否数组
		 */
        isArray: "isArray" in Array ? Array.isArray : function (value) {
            return toString.call(value) === "[object Array]";
        },
        /**
		 * 是否日期
		 * @param  {*}  value 对象
		 * @return {Boolean}  是否日期
		 */
        isDate: function (value) {
            return toString.call(value) === "[object Date]";
        },
        /**
		 * 是否是javascript对象
		 * @param {Object} value The value to test
		 * @return {Boolean}
		 * @method
		 */
        isObject: toString.call(null) === "[object Object]" ? function (value) {
            return value !== null && value !== undefined && toString.call(value) === "[object Object]" && value.ownerDocument === undefined;
        } : function (value) {
            return toString.call(value) === "[object Object]";
        },
        /**
		 * 是否是数字或者数字字符串
		 * @param  {String}  value 数字字符串
		 * @return {Boolean}  是否是数字或者数字字符串
		 */
        isNumeric: function (value) {
            return !isNaN(parseFloat(value)) && isFinite(value);
        },
        /**
		 * 是否字符串
		 * @param {String} value
		 * @return {Boolean} 
		 */
        isString: function (value) {
            return typeof value === "string";
        },
        /**
		 * 是否数字
		 * @param {Number} value
		 * @return {Boolean} 
		 */
        isNumber: function (value) {
            return typeof value === "number";
        },
        /**
		 * 是否bool类型
		 * @param {Boolean} value
		 * @return {Boolean} 
		 */
        isBoolean: function (value) {
            return typeof value === "boolean";
        },
        /**
		 * 是否为空
		 * @param {Object} value
		 * @return {Boolean} 
		 */
        isNullOrEmpty: function (value) {
            if (value) {
                value = value.toString().replace(/(^\s*)|(\s*$)/g, "");
                if (value == "null" || value == "") {
                    return true;
                } else {
                    return false;
                }
            } else {
                if (value === 0 || value === false) {
                    return false;
                }
                return true;
            }
        },
        /**
		 * 将s1的属性或方法复制到r的原型链上面
		 * @param {Function} 目标函数
		 * @param {[Function]} 待复制函数列表
		 */
        augment: function (r, s1) {
            if (!BUI.isFunction(r)) {
                return r;
            }
            for (var i = 1; i < arguments.length; i++) {
                BUI.mix(r.prototype, arguments[i].prototype || arguments[i]);
            }
            return r;
        },
        /**
		 * 拷贝对象
		 * @param  {Object} obj 要拷贝的对象
		 * @return {Object} 拷贝生成的对象
		 */
        cloneObject: function (obj) {
            var result = BUI.isArray(obj) ? [] : {};
            return BUI.mix(true, result, obj);
        },
        /**
		 * 抛出异常
		 * @param {String} 错误消息
		 */
        error: function (msg) {
            if (BUI.debug) {
                throw new Error(msg);
            }
        },
        /**
		 * 实现类的继承，通过父类生成子类
		 * @param {Object} 子类
		 * @param {Object} 父类
		 * @param {Object} 重写实体属性
		 * @param {Object} 重写静态属性
		 */
        extend: function (subclass, superclass, overrides, staticOverrides) {
            //如果只提供父类构造函数，则自动生成子类构造函数
            if (!BUI.isFunction(superclass)) {
                overrides = superclass;
                superclass = subclass;
                subclass = function () { };
            }
            var create;
            if (Object.create) {
                create = function (proto, c) {
                    return Object.create(proto, {
                        constructor: {
                            value: c
                        }
                    });
                }
            } else {
                create = function (proto, c) {
                    function F() { }
                    F.prototype = proto;
                    var o = new F();
                    o.constructor = c;
                    return o;
                };
            }

            //继承主要通过原型链实现，用于共享方法。
            var superObj = create(superclass.prototype, subclass);
            //合并prototype属性
            subclass.prototype = BUI.mix(superObj, subclass.prototype);
            //构建父类属性
            subclass.superclass = create(superclass.prototype, superclass);
            BUI.mix(superObj, overrides);
            BUI.mix(subclass, staticOverrides);
            return subclass;
        },
        /**
         * 获取唯一标识
         */
        guid: function () {
            var map = {};
            return function (prefix) {
                prefix = prefix || BUI.prefix + GUID_DEFAULT;
                if (!map[prefix]) {
                    map[prefix] = 1;
                } else {
                    map[prefix] += 1;
                }
                return prefix + map[prefix];
            };
        }(),
        /**
         * 添加日志
         * @param {Object} obj
         */
        log: function (obj) {
            if (BUI.debug && win.console && win.console.log) {
                win.console.log(obj);
            }
        },
        /**
         * 将多个对象的属性复制到一个新的对象
         */
        merge: function () {
            var args = $.makeArray(arguments),
                first = args[0];
            if (BUI.isBoolean(first)) {
                args.shift();
                args.unshift({});
                args.unshift(first);
            } else {
                args.unshift({});
            }
            return BUI.mix.apply(null, args);
        },
        /**
         * 封装 jQuery.extend 方法，将两个或更多对象的内容合并到第一个对象
         */
        mix: function () {
            return $.extend.apply(null, arguments);
        },
        /**
         * 合并属性  bui特定ATTR
         * @param {Object} 目标属性
         * @param {Object} 复制属性
         */
        mixAttrs: function (to, from) {
            for (var c in from) {
                //只复制本身的属性，原型链属性忽略
                if (from.hasOwnProperty(c)) {
                    to[c] = to[c] || {};
                    BUI.mixAttr(to[c], from[c]);
                }
            }
        },
        /**
         * 合并属性  bui特定ATTR  内部使用
         * @param {Object} attr
         * @param {Object} attrConfig
         */
        mixAttr: function (attr, attrConfig) {
            for (var p in attrConfig) {
                //只复制本身的属性，原型链属性忽略
                if (attrConfig.hasOwnProperty(p)) {
                    if (p == "value") {
                        if (BUI.isObject(attrConfig[p])) {
                            attr[p] = attr[p] || {};
                            BUI.mix(attr[p], attrConfig[p]);
                        } else if (BUI.isArray(attrConfig[p])) {
                            attr[p] = attr[p] || [];
                            attr[p] = attr[p].concat(attrConfig[p]);
                        } else {
                            attr[p] = attrConfig[p];
                        }
                    } else {
                        attr[p] = attrConfig[p];
                    }
                }
            }
        },
        /**
         * 添加命名空间
         * @param {Object} 名称
         * @param {Object} 父类命名空间
         */
        namespace: function (name, baseNS) {
            baseNS = baseNS || BUI;
            if (!name) {
                return baseNS;
            }
            var list = name.split("."),
                curNS = baseNS;
            for (var i = 0; i < list.length; i++) {
                var nsName = list[i];
                if (!curNS[nsName]) {
                    curNS[nsName] = {};
                }
                curNS = curNS[nsName];
            }
            return curNS;
        },
        /**
         * 前缀
         */
        prefix: "bui-",
        /**
         * 替换字符串中的字段
         * @param {Object} 模版字符串
         * @param {Object} JSON对象
         * @param {Object} 替换正则
         */
        substitute: function (str, o, regexp) {
            if (!BUI.isString(str) || !BUI.isObject(o) && !BUI.isArray(o)) {
                return str;
            }
            return str.replace(regexp || /\\?\{([^{}]+)\}/g, function (match, name) {
                if (match.charAt(0) === "\\") {
                    return match.slice(1);
                }
                return o[name] === undefined ? "" : o[name];
            });
        },
        /**
         * 将url参数转换成对象
         * @param {Object} str
         */
        unparam: function (str) {
            if (typeof str != "string" || !(str = $.trim(str))) {
                return {};
            }
            var pairs = str.split("&"),
                pairsArr, rst = {};
            for (var i = pairs.length - 1; i >= 0; i--) {
                pairsArr = pairs[i].split("=");
                rst[pairsArr[0]] = decodeURIComponent(pairsArr[1]);
            }
            return rst;
        },
        /**
         * 首字母大写
         * @param {Object} s
         */
        ucfirst: function (s) {
            s += "";
            return s.charAt(0).toUpperCase() + s.substring(1);
        },
        /**
         * 页面上的一点是否在用户的视图内
         * @param {Object} 位置对象
         */
        isInView: function (offset) {
            var left = offset.left,
                top = offset.top,
                viewWidth = BUI.viewportWidth(),
                wiewHeight = BUI.viewportHeight(),
                scrollTop = BUI.scrollTop(),
                scrollLeft = BUI.scrollLeft();
            if (left < scrollLeft || left > scrollLeft + viewWidth) {
                return false;
            }
            if (top < scrollTop || top > scrollTop + wiewHeight) {
                return false;
            }
            return true;
        },
        /**
         * 页面上的一点纵向坐标是否在用户的视图内
         * @param {Object} top
         */
        isInVerticalView: function (top) {
            var wiewHeight = BUI.viewportHeight(),
                scrollTop = BUI.scrollTop();
            if (top < scrollTop || top > scrollTop + wiewHeight) {
                return false;
            }
            return true;
        },
        /**
         * 页面上的一点横向坐标是否在用户的视图内
         * @param {Object} left
         */
        isInHorizontalView: function (left) {
            var viewWidth = BUI.viewportWidth(),
                scrollLeft = BUI.scrollLeft();
            if (left < scrollLeft || left > scrollLeft + viewWidth) {
                return false;
            }
            return true;
        },
        /**
         * 获取窗口可视范围宽度
         */
        viewportWidth: function () {
            return $(window).width();
        },
        /**
         * 获取窗口可视范围高度
         */
        viewportHeight: function () {
            return $(window).height();
        },
        /**
         * 滚动到窗口的left位置
         */
        scrollLeft: function () {
            return $(window).scrollLeft();
        },
        /**
         * 滚动到横向位置
         */
        scrollTop: function () {
            return $(window).scrollTop();
        },
        /**
         * 窗口宽度
         */
        docWidth: function () {
            return Math.max(this.viewportWidth(), doc[DOC_ELEMENT][SCROLL_WIDTH], doc[BODY][SCROLL_WIDTH]);
        },
        /**
         * 窗口高度
         */
        docHeight: function () {
            return Math.max(this.viewportHeight(), doc[DOC_ELEMENT][SCROLL_HEIGHT], doc[BODY][SCROLL_HEIGHT]);
        },
        /**
         * 遍历数组执行func参数
         * @param {Object} elements
         * @param {Object} func
         */
        each: function (elements, func) {
            if (!elements) {
                return;
            }
            $.each(elements, function (k, v) {
                return func(v, k);
            });
        },
        /**
         * 封装事件，便于使用上下文this和便于解除事件时使用
         * @param {Object} 对象
         * @param {Object} 事件名称
         */
        wrapBehavior: function (self, action) {
            return self["__bui_wrap_" + action] = function (e) {
                if (!self.get("disabled")) {
                    return self[action](e);
                }
            };
        },
        /**
         * 获取封装的事件
         * @param {Object} self
         * @param {Object} action
         */
        getWrapBehavior: function (self, action) {
            return self["__bui_wrap_" + action];
        },
        /**
         * 设置属性
         * @param {Object} 对象
         * @param {Object} 属性
         * @param {Object} 属性值
         */
        setValue: function (obj, name, value) {
            if (!obj && !name) {
                return obj;
            }
            var arr = name.split("."),
                curObj = obj,
                len = arr.length;
            for (var i = 0; i < len; i++) {
                if (!curObj || !BUI.isObject(curObj)) {
                    break;
                }
                var subName = arr[i];
                if (i === len - 1) {
                    curObj[subName] = value;
                    break;
                }
                if (!curObj[subName]) {
                    curObj[subName] = {};
                }
                curObj = curObj[subName];
            }
            return obj;
        },
        /**
         * 获取对象属性
         * @param {Object} 对象
         * @param {Object} 属性名
         */
        getValue: function (obj, name) {
            if (!obj && !name) {
                return null;
            }
            var arr = name.split("."),
                curObj = obj,
                len = arr.length,
                value = null;
            for (var i = 0; i < len; i++) {
                if (!curObj || !BUI.isObject(curObj)) {
                    break;
                }
                var subName = arr[i];
                if (i === len - 1) {
                    value = curObj[subName];
                    break;
                }
                if (!curObj[subName]) {
                    break;
                }
                curObj = curObj[subName];
            }
            return value;
        }
    });
})(jQuery);
//BUI.JSON +
(function (BUI, $) {
    "use strict";
    var JSON = window.JSON || {};
    var rx_one = /^[\],:{}\s]*$/;
    var rx_two = /\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g;
    var rx_three = /"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g;
    var rx_four = /(?:^|:|,)(?:\s*\[)+/g;
    var rx_escapable = /[\\"\u0000-\u001f\u007f-\u009f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g;
    var rx_dangerous = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g;

    function f(n) {
        return n < 10 ? "0" + n : n
    }

    function this_value() {
        return this.valueOf()
    }
    if (typeof Date.prototype.toJSON !== "function") {
        Date.prototype.toJSON = function () {
            return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z" : null
        };
        Boolean.prototype.toJSON = this_value;
        Number.prototype.toJSON = this_value;
        String.prototype.toJSON = this_value
    }
    var gap;
    var indent;
    var meta;
    var rep;

    function quote(string) {
        rx_escapable.lastIndex = 0;
        return rx_escapable.test(string) ? "\"" + string.replace(rx_escapable, function (a) {
            var c = meta[a];
            return typeof c === "string" ? c : "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4)
        }) + "\"" : "\"" + string + "\""
    }

    function str(key, holder) {
        var i;
        var k;
        var v;
        var length;
        var mind = gap;
        var partial;
        var value = holder[key];
        if (value && typeof value === "object" && typeof value.toJSON === "function") {
            value = value.toJSON(key)
        }
        if (typeof rep === "function") {
            value = rep.call(holder, key, value)
        }
        switch (typeof value) {
            case "string":
                return quote(value);
            case "number":
                return isFinite(value) ? String(value) : "null";
            case "boolean":
            case "null":
                return String(value);
            case "object":
                if (!value) {
                    return "null"
                }
                gap += indent;
                partial = [];
                if (Object.prototype.toString.apply(value) === "[object Array]") {
                    length = value.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, value) || "null"
                    }
                    v = partial.length === 0 ? "[]" : gap ? "[\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "]" : "[" + partial.join(",") + "]";
                    gap = mind;
                    return v
                }
                if (rep && typeof rep === "object") {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        if (typeof rep[i] === "string") {
                            k = rep[i];
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ": " : ":") + v)
                            }
                        }
                    }
                } else {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ": " : ":") + v)
                            }
                        }
                    }
                }
                v = partial.length === 0 ? "{}" : gap ? "{\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "}" : "{" + partial.join(",") + "}";
                gap = mind;
                return v
        }
    }
    if (typeof JSON.stringify !== "function") {
        meta = {
            "\b": "\\b",
            "\t": "\\t",
            "\n": "\\n",
            "\f": "\\f",
            "\r": "\\r",
            "\"": "\\\"",
            "\\": "\\\\"
        };
        JSON.stringify = function (value, replacer, space) {
            var i;
            gap = "";
            indent = "";
            if (typeof space === "number") {
                for (i = 0; i < space; i += 1) {
                    indent += " "
                }
            } else if (typeof space === "string") {
                indent = space
            }
            rep = replacer;
            if (replacer && typeof replacer !== "function" && (typeof replacer !== "object" || typeof replacer.length !== "number")) {
                throw new Error("JSON.stringify");
            }
            return str("", {
                "": value
            })
        }
    }
    if (typeof JSON.parse !== "function") {
        JSON.parse = function (text, reviver) {
            var j;

            function walk(holder, key) {
                var k;
                var v;
                var value = holder[key];
                if (value && typeof value === "object") {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v
                            } else {
                                delete value[k]
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value)
            }
            text = String(text);
            rx_dangerous.lastIndex = 0;
            if (rx_dangerous.test(text)) {
                text = text.replace(rx_dangerous, function (a) {
                    return "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4)
                })
            }
            if (rx_one.test(text.replace(rx_two, "@").replace(rx_three, "]").replace(rx_four, ""))) {
                j = eval("(" + text + ")");
                return (typeof reviver === "function") ? walk({
                    "": j
                }, "") : j
            }
            throw new SyntaxError("JSON.parse");
        }
    }

    BUI.JSON = JSON;
})(window.BUI, jQuery);
//BUI.Date +
(function (BUI) {
    "use strict";
    var dateRegex = /^(?:(?!0000)[0-9]{4}([-/.]+)(?:(?:0?[1-9]|1[0-2])\1(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])\1(?:29|30)|(?:0?[13578]|1[02])\1(?:31))|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-/.]?)0?2\2(?:29))(\s+([01]|([01][0-9]|2[0-3])):([0-9]|[0-5][0-9]):([0-9]|[0-5][0-9]))?$/;

    function dateParse(val, format) {
        if (val instanceof Date) {
            return val;
        }
        if (typeof format == "undefined" || format == null || format == "") {
            var checkList = new Array("y-m-d", "yyyy-mm-dd", "yyyy-mm-dd HH:MM:ss", "H:M:s");
            for (var i = 0; i < checkList.length; i++) {
                var d = dateParse(val, checkList[i]);
                if (d != null) {
                    return d;
                }
            }
            return null;
        }
        val = val + "";
        var i_val = 0;
        var i_format = 0;
        var c = "";
        var token = "";
        var x, y;
        var now = new Date();
        var year = now.getYear();
        var month = now.getMonth() + 1;
        var date = 1;
        var hh = 0;
        var mm = 0;
        var ss = 0;
        var isInteger = function (val) {
            return /^\d*$/.test(val);
        };
        var getInt = function (str, i, minlength, maxlength) {
            for (var x = maxlength; x >= minlength; x--) {
                var token = str.substring(i, i + x);
                if (token.length < minlength) {
                    return null;
                }
                if (isInteger(token)) {
                    return token;
                }
            }
            return null;
        };
        while (i_format < format.length) {
            c = format.charAt(i_format);
            token = "";
            while (format.charAt(i_format) == c && i_format < format.length) {
                token += format.charAt(i_format++);
            }
            if (token == "yyyy" || token == "yy" || token == "y") {
                if (token == "yyyy") {
                    x = 4;
                    y = 4;
                }
                if (token == "yy") {
                    x = 2;
                    y = 2;
                }
                if (token == "y") {
                    x = 2;
                    y = 4;
                }
                year = getInt(val, i_val, x, y);
                if (year == null) {
                    return null;
                }
                i_val += year.length;
                if (year.length == 2) {
                    year = year > 70 ? 1900 + (year - 0) : 2e3 + (year - 0);
                }
            } else if (token == "mm" || token == "m") {
                month = getInt(val, i_val, token.length, 2);
                if (month == null || month < 1 || month > 12) {
                    return null;
                }
                i_val += month.length;
            } else if (token == "dd" || token == "d") {
                date = getInt(val, i_val, token.length, 2);
                if (date == null || date < 1 || date > 31) {
                    return null;
                }
                i_val += date.length;
            } else if (token == "hh" || token == "h") {
                hh = getInt(val, i_val, token.length, 2);
                if (hh == null || hh < 1 || hh > 12) {
                    return null;
                }
                i_val += hh.length;
            } else if (token == "HH" || token == "H") {
                hh = getInt(val, i_val, token.length, 2);
                if (hh == null || hh < 0 || hh > 23) {
                    return null;
                }
                i_val += hh.length;
            } else if (token == "MM" || token == "M") {
                mm = getInt(val, i_val, token.length, 2);
                if (mm == null || mm < 0 || mm > 59) {
                    return null;
                }
                i_val += mm.length;
            } else if (token == "ss" || token == "s") {
                ss = getInt(val, i_val, token.length, 2);
                if (ss == null || ss < 0 || ss > 59) {
                    return null;
                }
                i_val += ss.length;
            } else {
                if (val.substring(i_val, i_val + token.length) != token) {
                    return null;
                } else {
                    i_val += token.length;
                }
            }
        }
        if (i_val != val.length) {
            return null;
        }
        if (month == 2) {
            if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0) {
                if (date > 29) {
                    return null;
                }
            } else {
                if (date > 28) {
                    return null;
                }
            }
        }
        if (month == 4 || month == 6 || month == 9 || month == 11) {
            if (date > 30) {
                return null;
            }
        }
        return new Date(year, month - 1, date, hh, mm, ss);
    }

    function DateAdd(strInterval, NumDay, dtDate) {
        var dtTmp = new Date(dtDate);
        if (isNaN(dtTmp)) {
            dtTmp = new Date();
        }
        NumDay = parseInt(NumDay, 10);
        switch (strInterval) {
            case "s":
                dtTmp = new Date(dtTmp.getTime() + 1000 * NumDay);
                break;
            case "n":
                dtTmp = new Date(dtTmp.getTime() + 60000 * NumDay);
                break;
            case "h":
                dtTmp = new Date(dtTmp.getTime() + 3600000 * NumDay);
                break;
            case "d":
                dtTmp = new Date(dtTmp.getTime() + 86400000 * NumDay);
                break;
            case "w":
                dtTmp = new Date(dtTmp.getTime() + 86400000 * 7 * NumDay);
                break;
            case "m":
                dtTmp = new Date(dtTmp.getFullYear(), dtTmp.getMonth() + NumDay, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
                break;
            case "y":
                dtTmp = new Date(dtTmp.getFullYear() + NumDay, dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
                break;
        }
        return dtTmp;
    }
    var dateFormat = function () {
        var token = /w{1}|d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
            timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
            timezoneClip = /[^-+\dA-Z]/g,
            pad = function (val, len) {
                val = String(val);
                len = len || 2;
                while (val.length < len) {
                    val = "0" + val;
                }
                return val;
            },
            masks = {
                "default": "ddd mmm dd yyyy HH:MM:ss",
                shortDate: "m/d/yy",
                longDate: "mmmm d, yyyy",
                fullDate: "dddd, mmmm d, yyyy",
                shortTime: "h:MM TT",
                longTime: "h:MM:ss TT Z",
                isoDate: "yyyy-mm-dd",
                isoTime: "HH:MM:ss",
                isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
                isoUTCDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'",
                localShortDate: "yy年mm月dd日",
                localShortDateTime: "yy年mm月dd日 hh:MM:ss TT",
                localLongDate: "yyyy年mm月dd日",
                localLongDateTime: "yyyy年mm月dd日 hh:MM:ss TT",
                localFullDate: "yyyy年mm月dd日 w",
                localFullDateTime: "yyyy年mm月dd日 w hh:MM:ss TT"
            },
            i18n = {
                dayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
                monthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
            };
        return function (date, mask, utc) {
            if (arguments.length === 1 && Object.prototype.toString.call(date) === "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }
            date = date ? new Date(date) : new Date();
            if (isNaN(date)) {
                throw SyntaxError("invalid date");
            }
            mask = String(masks[mask] || mask || masks["default"]);
            if (mask.slice(0, 4) === "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }
            var _ = utc ? "getUTC" : "get",
                d = date[_ + "Date"](),
                D = date[_ + "Day"](),
                m = date[_ + "Month"](),
                y = date[_ + "FullYear"](),
                H = date[_ + "Hours"](),
                M = date[_ + "Minutes"](),
                s = date[_ + "Seconds"](),
                L = date[_ + "Milliseconds"](),
                o = utc ? 0 : date.getTimezoneOffset(),
                flags = {
                    d: d,
                    dd: pad(d, undefined),
                    ddd: i18n.dayNames[D],
                    dddd: i18n.dayNames[D + 7],
                    w: i18n.dayNames[D + 14],
                    m: m + 1,
                    mm: pad(m + 1, undefined),
                    mmm: i18n.monthNames[m],
                    mmmm: i18n.monthNames[m + 12],
                    yy: String(y).slice(2),
                    yyyy: y,
                    h: H % 12 || 12,
                    hh: pad(H % 12 || 12, undefined),
                    H: H,
                    HH: pad(H, undefined),
                    M: M,
                    MM: pad(M, undefined),
                    s: s,
                    ss: pad(s, undefined),
                    l: pad(L, 3),
                    L: pad(L > 99 ? Math.round(L / 10) : L, undefined),
                    t: H < 12 ? "a" : "p",
                    tt: H < 12 ? "am" : "pm",
                    T: H < 12 ? "A" : "P",
                    TT: H < 12 ? "AM" : "PM",
                    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 !== 10) * d % 10]
                };
            return mask.replace(token, function ($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    }();
    var DateUtil = {
        /**
		 * 日期加法
		 * @param {String} strInterval 加法的类型，s(秒),n(分),h(时),d(天),w(周),m(月),y(年)
		 * @param {Number} Num   数量，如果为负数，则为减法
		 * @param {Date} dtDate   起始日期，默认为此时
		 */
        add: function (strInterval, Num, dtDate) {
            return DateAdd(strInterval, Num, dtDate);
        },
        /**
		 * 小时的加法
		 * @param {Number} hours 小时
		 * @param {Date} date 起始日期
		 */
        addHour: function (hours, date) {
            return DateAdd("h", hours, date);
        },
        /**
		 * 分的加法
		 * @param {Number} minutes 分
		 * @param {Date} date 起始日期
		 */
        addMinute: function (minutes, date) {
            return DateAdd("n", minutes, date);
        },
        /**
		 * 秒的加法
		 * @param {Number} seconds 秒
		 * @param {Date} date 起始日期
		 */
        addSecond: function (seconds, date) {
            return DateAdd("s", seconds, date);
        },
        /**
		 * 天的加法
		 * @param {Number} days 天数
		 * @param {Date} date 起始日期
		 */
        addDay: function (days, date) {
            return DateAdd("d", days, date);
        },
        /**
		 * 增加周
		 * @param {Number} weeks 周数
		 * @param {Date} date  起始日期
		 */
        addWeek: function (weeks, date) {
            return DateAdd("w", weeks, date);
        },
        /**
		 * 增加月
		 * @param {Number} months 月数
		 * @param {Date} date  起始日期
		 */
        addMonths: function (months, date) {
            return DateAdd("m", months, date);
        },
        /**
		 * 增加年
		 * @param {Number} years 年数
		 * @param {Date} date  起始日期
		 */
        addYear: function (years, date) {
            return DateAdd("y", years, date);
        },
        /**
		 * 日期是否相等，忽略时间
		 * @param  {Date}  d1 日期对象
		 * @param  {Date}  d2 日期对象
		 * @return {Boolean}    是否相等
		 */
        isDateEquals: function (d1, d2) {
            return d1.getFullYear() === d2.getFullYear() && d1.getMonth() === d2.getMonth() && d1.getDate() === d2.getDate();
        },
        /**
		 * 日期时间是否相等，包含时间
		 * @param  {Date}  d1 日期对象
		 * @param  {Date}  d2 日期对象
		 * @return {Boolean}    是否相等
		 */
        isEquals: function (d1, d2) {
            if (d1 == d2) {
                return true;
            }
            if (!d1 || !d2) {
                return false;
            }
            if (!d1.getTime || !d2.getTime) {
                return false;
            }
            return d1.getTime() == d2.getTime();
        },
        /**
		 * 字符串是否是有效的日期类型
		 * @param {String} str 字符串
		 * @return 字符串是否能转换成日期
		 */
        isDateString: function (str) {
            return dateRegex.test(str);
        },
        /**
		 * 将日期格式化成字符串
		 * @param  {Date} date 日期
		 * @param  {String} mask 格式化方式
		 * @param  {Date} utc  是否utc时间
		 * @return {String}      日期的字符串
		 */
        format: function (date, mask, utc) {
            return dateFormat(date, mask, utc);
        },
        /**
		 * 转换成日期
		 * @param  {String|Date} date 字符串或者日期
		 * @param  {String} dateMask  日期的格式,如:yyyy-MM-dd
		 * @return {Date}      日期对象
		 */
        parse: function (date, s) {
            if (BUI.isString(date)) {
                date = date.replace("/", "-");
            }
            return dateParse(date, s);
        },
        /**
		 * 当前天
		 * @return {Date} 当前天 00:00:00
		 */
        today: function () {
            var now = new Date();
            return new Date(now.getFullYear(), now.getMonth(), now.getDate());
        },
        /**
		 * 返回当前日期
		 * @return {Date} 日期的 00:00:00
		 */
        getDate: function (date) {
            return new Date(date.getFullYear(), date.getMonth(), date.getDate());
        }
    };
    BUI.Date = DateUtil;
})(window.BUI);
//BUI.Array +
(function (BUI) {
    "use strict";
    var ArrayUtil = {
        /**
		 * 返回数组的最后一个对象
		 * @param {Array} array 数组或者类似于数组的对象.
		 * @return {*} 数组的最后一项.
		 */
        peek: function (array) {
            return array[array.length - 1];
        },
        /**
		 * 查找记录所在的位置
         * https://developer.mozilla.org/zh-CN/docs/Web/JavaScript/Reference/Global_Objects/Array/indexOf
		 * @param  {*} value 值
		 * @param  {Array} array 数组或者类似于数组的对象
		 * @param  {Number} [fromIndex=0] 起始项，默认为0
		 * @return {Number} 位置，如果为 -1则不在数组内
		 */
        indexOf: function (value, array, fromIndex) {
            var k;
            if (array.indexOf) {
                return array.indexOf(value, fromIndex);
            }
            //判断fromIndex合法性
            var n = +fromIndex || 0;
            if (Math.abs(n) === Infinity) {
                n = 0;
            }
            if (n >= array.length) {
                return -1;
            }
            //循环遍历
            k = Math.max(n >= 0 ? n : array.length - Math.abs(n), 0);
            while (k < array.length) {
                if (k in array && array[k] === value) {
                    return k;
                }
                k++;
            }
            return -1;
        },
        /**
		 * 数组是否存在指定值
		 * @param  {*} value 值
		 * @param  {Array} array 数组或者类似于数组的对象
		 * @return {Boolean} 是否存在于数组中
		 */
        contains: function (value, array) {
            if (array.includes) {
                return array.includes(value)
            }
            return ArrayUtil.indexOf(value, array) >= 0;
        },
        /**
		 * 遍历数组或者对象
		 * @method 
		 * @param {Object|Array} element/Object 数组中的元素或者对象的值 
		 * @param {Function} func 遍历的函数 function(elememt,index){} 或者 function(value,key){}
		 */
        each: function (array, func) {
            if (array.forEach) {
                array.forEach(func)
                return array;
            }
            return BUI.each(array, func);
        },
        /**
		 * 2个数组内部的值是否相等
		 * @param  {Array} a1 数组1
		 * @param  {Array} a2 数组2
		 * @return {Boolean} 2个数组相等或者内部元素是否相等
		 */
        equals: function (a1, a2) {
            if (a1 == a2) {
                return true;
            }
            if (!a1 || !a2) {
                return false;
            }
            if (a1.length != a2.length) {
                return false;
            }
            var rst = true;
            for (var i = 0; i < a1.length; i++) {
                if (a1[i] !== a2[i]) {
                    rst = false;
                    break;
                }
            }
            return rst;
        },
        /**
		 * 过滤数组
		 * @param {Object|Array} element/Object 数组中的元素或者对象的值 
		 * @param {Function} func 遍历的函数 function(elememt,index){} 或者 function(value,key){},如果返回true则添加到结果集
		 * @return {Array} 过滤的结果集
		 */
        filter: function (array, func) {
            if (array.filter) {
                return array.filter(func);
            }
            var result = [];
            ArrayUtil.each(array, function (value, index) {
                if (func(value, index)) {
                    result.push(value);
                }
            });
            return result;
        },
        /**
		 * 转换数组数组
		 * @param {Object|Array} element/Object 数组中的元素或者对象的值 
		 * @param {Function} func 遍历的函数 function(elememt,index){} 或者 function(value,key){},将返回的结果添加到结果集
		 * @return {Array} 过滤的结果集
		 */
        map: function (array, func) {
            if (array.map) {
                return array.map(func);
            }
            var result = [];
            ArrayUtil.each(array, function (value, index) {
                result.push(func(value, index));
            });
            return result;
        },
        /**
		 * 获取第一个符合条件的数据
		 * @param  {Array} array 数组
		 * @param  {Function} func  匹配函数
		 * @return {*}  符合条件的数据
		 */
        find: function (array, func) {
            if (array.find) {
                return array.find(func);
            }
            var i = ArrayUtil.findIndex(array, func);
            return i < 0 ? null : array[i];
        },
        /**
		 * 获取第一个符合条件的数据的索引值
		 * @param  {Array} array 数组
		 * @param  {Function} func  匹配函数
		 * @return {Number} 符合条件的数据的索引值
		 */
        findIndex: function (array, func) {
            var result = -1;
            ArrayUtil.each(array, function (value, index) {
                if (func(value, index)) {
                    result = index;
                    return false;
                }
            });
            return result;
        },
        /**
		 * 数组是否为空
		 * @param  {Array}  array 数组
		 * @return {Boolean}  是否为空
		 */
        isEmpty: function (array) {
            return array.length == 0;
        },
        /**
		 * 插入数组
		 * @param  {Array} array 数组
		 * @param  {Number} index 位置
		 * @param {*} value 插入的数据
		 */
        add: function (array, value) {
            array.push(value);
        },
        /**
		 * 将数据插入数组指定的位置
		 * @param  {Array} array 数组
		 * @param {*} value 插入的数据
		 * @param  {Number} index 位置
		 */
        addAt: function (array, value, index) {
            ArrayUtil.splice(array, index, 0, value);
        },
        /**
		 * 清空数组
		 * @param  {Array} array 数组
		 * @return {Array}  清空后的数组
		 */
        empty: function (array) {
            if (!(array instanceof Array)) {
                for (var i = array.length - 1; i >= 0; i--) {
                    delete array[i];
                }
            }
            array.length = 0;
        },
        /**
		 * 移除记录
		 * @param  {Array} array 数组
		 * @param  {*} value 记录
		 * @return {Boolean}   是否移除成功
		 */
        remove: function (array, value) {
            var i = ArrayUtil.indexOf(value, array);
            var rv;
            if (rv = i >= 0) {
                ArrayUtil.removeAt(array, i);
            }
            return rv;
        },
        /**
		 * 移除指定位置的记录
		 * @param  {Array} array 数组
		 * @param  {Number} index 索引值
		 * @return {Boolean}   是否移除成功
		 */
        removeAt: function (array, index) {
            return ArrayUtil.splice(array, index, 1).length == 1;
        },
        /**
		 * 重写slice  方法返回一个从开始到结束（不包括结束）选择的数组的一部分浅拷贝到一个新数组对象。且原始数组不会被修改。
		 * @param {Object} arr  
		 * @param {Object} start  从该索引处开始提取原数组中的元素（从0开始）。
		 * @param {Object} opt_end  在该索引处结束提取原数组元素（包含start，但不包含opt_end）
		 */
        slice: function (arr, start, opt_end) {
            if (arguments.length <= 2) {
                return Array.prototype.slice.call(arr, start);
            } else {
                return Array.prototype.slice.call(arr, start, opt_end);
            }
        },
        /**
		 * 重写splice  方法通过删除现有元素和/或添加新元素来更改一个数组的内容。
		 * @param {Object} arr
		 * @param {Object} index  指定修改的开始位置
		 * @param {Object} howMany  整数，表示要移除的数组元素的个数
		 * @param {Object} var_args  要添加进数组的元素
		 */
        splice: function (arr, index, howMany, var_args) {
            return Array.prototype.splice.apply(arr, ArrayUtil.slice(arguments, 1));
        }
    };
    BUI.Array = ArrayUtil;
})(window.BUI);
//BUI.KeyCode +
(function (BUI) {
    "use strict";
    /**
	 * 键盘按键对应的数字值
	 * @class BUI.KeyCode
	 * @singleton
	 */
    var keyCode = {
        BACKSPACE: 8,
        TAB: 9,
        NUM_CENTER: 12,
        ENTER: 13,
        RETURN: 13,
        SHIFT: 16,
        CTRL: 17,
        ALT: 18,
        PAUSE: 19,
        CAPS_LOCK: 20,
        ESC: 27,
        SPACE: 32,
        PAGE_UP: 33,
        PAGE_DOWN: 34,
        END: 35,
        HOME: 36,
        LEFT: 37,
        UP: 38,
        RIGHT: 39,
        DOWN: 40,
        PRINT_SCREEN: 44,
        INSERT: 45,
        DELETE: 46,
        ZERO: 48,
        ONE: 49,
        TWO: 50,
        THREE: 51,
        FOUR: 52,
        FIVE: 53,
        SIX: 54,
        SEVEN: 55,
        EIGHT: 56,
        NINE: 57,
        A: 65,
        B: 66,
        C: 67,
        D: 68,
        E: 69,
        F: 70,
        G: 71,
        H: 72,
        I: 73,
        J: 74,
        K: 75,
        L: 76,
        M: 77,
        N: 78,
        O: 79,
        P: 80,
        Q: 81,
        R: 82,
        S: 83,
        T: 84,
        U: 85,
        V: 86,
        W: 87,
        X: 88,
        Y: 89,
        Z: 90,
        CONTEXT_MENU: 93,
        NUM_ZERO: 96,
        NUM_ONE: 97,
        NUM_TWO: 98,
        NUM_THREE: 99,
        NUM_FOUR: 100,
        NUM_FIVE: 101,
        NUM_SIX: 102,
        NUM_SEVEN: 103,
        NUM_EIGHT: 104,
        NUM_NINE: 105,
        NUM_MULTIPLY: 106,
        NUM_PLUS: 107,
        NUM_MINUS: 109,
        NUM_PERIOD: 110,
        NUM_DIVISION: 111,
        F1: 112,
        F2: 113,
        F3: 114,
        F4: 115,
        F5: 116,
        F6: 117,
        F7: 118,
        F8: 119,
        F9: 120,
        F10: 121,
        F11: 122,
        F12: 123
    };
    BUI.KeyCode = keyCode;
})(window.BUI);
//BUI.Observable +
(function (BUI, $) {
    "use strict";
    var ArrayUtil = BUI.Array;
    var Callbacks = function () {
        this._init();
    };
    BUI.augment(Callbacks, {
        /**
		 * 内部函数集合
		 */
        _functions: null,
        /**
		 * 初始化
		 */
        _init: function () {
            var _self = this;
            _self._functions = [];
        },
        /**
		 * 添加回调函数
		 * @param {Function} fn 回调函数
		 */
        add: function (fn) {
            this._functions.push(fn);
        },
        /**
		 * 移除回调函数
		 * @param  {Function} fn 回调函数
		 */
        remove: function (fn) {
            var functions = this._functions;
            index = ArrayUtil.indexOf(fn, functions);
            if (index >= 0) {
                functions.splice(index, 1);
            }
        },
        /**
		 * 清空事件
		 */
        empty: function () {
            var length = this._functions.length;
            this._functions.splice(0, length);
        },
        /**
		 * 暂停事件
		 */
        pause: function () {
            this._paused = true;
        },
        /**
		 * 唤醒事件
		 */
        resume: function () {
            this._paused = false;
        },
        /**
		 * 触发回调
		 * @param  {Object} scope 上下文
		 * @param  {Array} args  回调函数的参数
		 * @return {Boolean|undefined} 当其中有一个函数返回为false时，阻止后面的执行，并返回false
		 */
        fireWith: function (scope, args) {
            var _self = this,
                rst;
            if (_self._paused) {
                return;
            }
            BUI.each(_self._functions, function (fn) {
                rst = fn.apply(scope, args);
                if (rst === false) {
                    return false;
                }
            });
            return rst;
        }
    });

    /**
	 * 支持事件的对象，参考观察者模式
	 *  - 此类提供事件绑定
	 *  - 提供事件冒泡机制
	 * @param {Object} config 配置项键值对
	 */
    var Observable = function (config) {
        this._events = [];
        this._eventMap = {};
        this._bubblesEvents = [];
        this._bubbleTarget = null;
        this._initEvents(config);
    };
    BUI.augment(Observable, {
        //支持的事件名列表
        _events: [],
        //绑定的事件
        _eventMap: {},
        //冒泡事件集合
        _bubblesEvents: [],
        //接收冒泡事件对象
        _bubbleTarget: null,
        //获取回调集合
        _getCallbacks: function (eventType) {
            var _self = this,
                eventMap = _self._eventMap;
            return eventMap[eventType];
        },
        //初始化事件列表
        _initEvents: function (config) {
            var _self = this,
                listeners = null;
            if (!config) {
                return;
            }
            listeners = config.listeners || {};
            //handler默认click事件
            if (config.handler) {
                listeners.click = config.handler;
            }
            if (listeners) {
                for (var name in listeners) {
                    if (listeners.hasOwnProperty(name)) {
                        _self.on(name, listeners[name]);
                    }
                }
            }
        },
        //事件是否支持冒泡
        _isBubbles: function (eventType) {
            return ArrayUtil.indexOf(eventType, this._bubblesEvents) >= 0;
        },
        /**
		 * 添加冒泡的对象 默认是父类对象
		 * @protected
		 * @param {Object} target  冒泡的事件源
		 */
        addTarget: function (target) {
            this._bubbleTarget = target;
        },
        /**
		 * 添加支持的事件
		 * @param {Object} events
		 */
        addEvents: function (events) {
            var _self = this,
                existEvents = _self._events,
                eventMap = _self._eventMap;

            function addEvent(eventType) {
                if (ArrayUtil.indexOf(eventType, existEvents) === -1) {
                    eventMap[eventType] = new Callbacks();
                    existEvents.push(eventType);
                }
            }
            if (BUI.isArray(events)) {
                BUI.each(events, function (eventType) {
                    addEvent(eventType);
                });
            } else {
                addEvent(events);
            }
        },
        /**
		 * 移除所有绑定的事件
		 * @protected
		 */
        clearListeners: function () {
            var _self = this,
                eventMap = _self._eventMap;
            for (var name in eventMap) {
                if (eventMap.hasOwnProperty(name)) {
                    eventMap[name].empty();
                }
            }
        },
        /**
		 * 触发事件		
		 * @param  {String} eventType 事件类型
		 * @param  {Object} eventData 事件触发时传递的数据
		 * @return {Boolean|undefined}  如果其中一个事件处理器返回 false , 则返回 false, 否则返回最后一个事件处理器的返回值
		 */
        fire: function (eventType, eventData) {
            var _self = this,
                callbacks = _self._getCallbacks(eventType),
                args = $.makeArray(arguments),
                result;
            if (!eventData) {
                eventData = {};
                args.push(eventData);
            }
            if (!eventData.target) {
                eventData.target = _self;
            }
            if (callbacks) {
                result = callbacks.fireWith(_self, Array.prototype.slice.call(args, 1));
            }
            if (_self._isBubbles(eventType)) {
                var bubbleTarget = _self._bubbleTarget;
                if (bubbleTarget && bubbleTarget.fire) {
                    bubbleTarget.fire(eventType, eventData);
                }
            }
            return result;
        },
        /**
		 * 暂停事件的执行
		 * @param {Object} eventType
		 */
        pauseEvent: function (eventType) {
            var _self = this,
                callbacks = _self._getCallbacks(eventType);
            callbacks && callbacks.pause();
        },
        /**
		 * 唤醒事件
		 * @param {Object} eventType
		 */
        resumeEvent: function (eventType) {
            var _self = this,
                callbacks = _self._getCallbacks(eventType);
            callbacks && callbacks.resume();
        },
        /**
		 * 添加绑定事件
		 * @param {Object} 事件类型
		 * @param {Object} 回调函数
		 */
        on: function (eventType, fn) {
            var arr = eventType.split(" "),
                _self = this,
                callbacks = null;
            if (arr.length > 1) {
                BUI.each(arr, function (name) {
                    _self.on(name, fn);
                });
            } else {
                callbacks = _self._getCallbacks(eventType);
                if (callbacks) {
                    callbacks.add(fn);
                } else {
                    _self.addEvents(eventType);
                    _self.on(eventType, fn);
                }
            }
            return _self;
        },
        /**
		 * 移除绑定的事件
		 * @param  {String}   eventType 事件类型
		 * @param  {Function} fn        回调函数
		 */
        off: function (eventType, fn) {
            if (!eventType && !fn) {
                this.clearListeners();
                return this;
            }
            var _self = this,
                callbacks = _self._getCallbacks(eventType);
            if (callbacks) {
                if (fn) {
                    callbacks.remove(fn);
                } else {
                    callbacks.empty();
                }
            }
            return _self;
        },
        /**
		 * 配置事件是否允许冒泡
		 * @protected
		 * @param  {String} eventType 支持冒泡的事件
		 * @param  {Object} cfg 配置项
		 * @param {Boolean} cfg.bubbles 是否支持冒泡
		 */
        publish: function (eventType, cfg) {
            var _self = this,
                bubblesEvents = _self._bubblesEvents;
            if (cfg.bubbles) {
                if (BUI.Array.indexOf(eventType, bubblesEvents) === -1) {
                    bubblesEvents.push(eventType);
                }
            } else {
                var index = BUI.Array.indexOf(eventType, bubblesEvents);
                if (index !== -1) {
                    bubblesEvents.splice(index, 1);
                }
            }
        }
    });
    BUI.Observable = Observable;
})(window.BUI, jQuery);
//BUI.Base +
(function (BUI, $) {
    "use strict";
    var INVALID = {},
        Observable = BUI.Observable;

    //如果属性为空,自动创建
    function ensureNonEmpty(obj, name, create) {
        var ret = obj[name] || {};
        if (create) {
            obj[name] = ret;
        }
        return ret;
    }

    //返回方法函数
    function normalFn(host, method) {
        if (BUI.isString(method)) {
            return host[method];
        }
        return method;
    }

    /**
	 * 触发事件
	 * @param {Object} self
	 * @param {Object} when
	 * @param {Object} name
	 * @param {Object} prevVal
	 * @param {Object} newVal
	 */
    function __fireAttrChange(self, when, name, prevVal, newVal) {
        var attrName = name;
        return self.fire(when + BUI.ucfirst(name) + "Change", {
            attrName: attrName,
            prevVal: prevVal,
            newVal: newVal
        });
    }

    /**
	 * 设置属性 触发Change事件
	 * @param {Object} self
	 * @param {Object} name
	 * @param {Object} value
	 * @param {Object} opts
	 * @param {Object} attrs
	 */
    function setInternal(self, name, value, opts, attrs) {
        opts = opts || {};
        var ret, subVal, prevVal;
        prevVal = self.get(name);
        if (!$.isPlainObject(value) && !BUI.isArray(value) && prevVal === value) {
            return undefined;
        }
        //触发 before事件
        if (false === __fireAttrChange(self, "before", name, prevVal, value)) {
            return false;
        }

        ret = self._set(name, value, opts);
        if (ret === false) {
            return ret;
        }
        //触发 after事件
        value = self.__attrVals[name];
        __fireAttrChange(self, "after", name, prevVal, value);

        return self;
    }

    //属性初始化 父类属性覆盖子类属性
    function initClassAttrs(c) {
        if (c._attrs || c == Base) {
            return;
        }
        var superCon = c.superclass.constructor;
        if (superCon && !superCon._attrs) {
            initClassAttrs(superCon);
        }
        c._attrs = {};
        BUI.mixAttrs(c._attrs, superCon._attrs);
        BUI.mixAttrs(c._attrs, c.ATTRS);
    }

    var Base = function (config) {
        var _self = this,
            c = _self.constructor;
        this.__attrs = {};  //静态属性(ATTR)
        this.__attrVals = {};  //动态属性

        //继承Observable的属性
        Observable.apply(this, arguments);
        var con = _self.constructor;
        initClassAttrs(con);
        _self._initStaticAttrs(con._attrs);
        _self._initAttrs(config);
    };
    Base.INVALID = INVALID;

    //继承Observalble的方法
    BUI.extend(Base, Observable);
    BUI.augment(Base, {
        /**
		 * 初始化对象属性
		 * @param {Object} attrs
		 */
        _initStaticAttrs: function (attrs) {
            var _self = this,
                __attrs;
            __attrs = _self.__attrs = {};
            for (var p in attrs) {
                if (attrs.hasOwnProperty(p)) {
                    var attr = attrs[p];
                    if (attr.shared === false || attr.valueFn) {
                        __attrs[p] = {};
                        BUI.mixAttr(__attrs[p], attrs[p]);
                    } else {
                        __attrs[p] = attrs[p];
                    }
                }
            }
        },
        /**
		 * 添加属性定义
		 * @protected
		 * @param {String} name       属性名
		 * @param {Object} attrConfig 属性定义
		 * @param {Boolean} overrides 是否覆盖字段
		 */
        addAttr: function (name, attrConfig, overrides) {
            var _self = this,
                attrs = _self.__attrs,
                attr = attrs[name];
            if (!attr) {
                attr = attrs[name] = {};
            }
            for (var p in attrConfig) {
                if (attrConfig.hasOwnProperty(p)) {
                    if (p == "value") {
                        if (BUI.isObject(attrConfig[p])) {
                            attr[p] = attr[p] || {};
                            BUI.mix(attr[p], attrConfig[p]);
                        } else if (BUI.isArray(attrConfig[p])) {
                            attr[p] = attr[p] || [];
                            BUI.mix(attr[p], attrConfig[p]);
                        } else {
                            attr[p] = attrConfig[p];
                        }
                    } else {
                        attr[p] = attrConfig[p];
                    }
                }
            }
            return _self;
        },
        /**
		 * 添加属性定义
		 * @protected
		 * @param {Object} attrConfigs  An object with attribute name/configuration pairs.
		 * @param {Object} initialValues user defined initial values
		 * @param {Boolean} overrides 是否覆盖字段
		 */
        addAttrs: function (attrConfigs, initialValues, overrides) {
            var _self = this;
            if (!attrConfigs) {
                return _self;
            }
            if (typeof initialValues === "boolean") {
                overrides = initialValues;
                initialValues = null;
            }
            BUI.each(attrConfigs, function (attrConfig, name) {
                _self.addAttr(name, attrConfig, overrides);
            });
            if (initialValues) {
                _self.set(initialValues);
            }
            return _self;
        },
        /**
		 * 是否包含此属性
		 * @protected
		 * @param  {String}  name 值
		 * @return {Boolean} 是否包含
		 */
        hasAttr: function (name) {
            return name && this.__attrs.hasOwnProperty(name);
        },
        /**
		 * 获取默认的属性值
		 * @protected
		 * @return {Object} 属性值的键值对
		 */
        getAttrs: function () {
            return this.__attrs;
        },
        /**
		 * 获取属性名/属性值键值对
		 * @protected
		 * @return {Object} 属性对象
		 */
        getAttrVals: function () {
            return this.__attrVals;
        },
        /**
		 * 获取属性
		 * @param {Object} name
		 */
        get: function (name) {
            var _self = this,
                attrVals = _self.__attrVals,
                attrConfig, getter, ret;
            attrConfig = ensureNonEmpty(_self.__attrs, name);
            getter = attrConfig["getter"];
            ret = name in attrVals ? attrVals[name] : _self._getDefAttrVal(name);
            if (getter && (getter = normalFn(_self, getter))) {
                ret = getter.call(_self, ret, name);
            }
            return ret;
        },
        /**
		 * @清理所有属性值
		 */
        clearAttrVals: function () {
            this.__attrVals = {};
        },
        /**
		 * 移除属性定义
		 * @param {Object} name
		 */
        removeAttr: function (name) {
            var _self = this;
            if (_self.hasAttr(name)) {
                delete _self.__attrs[name];
                delete _self.__attrVals[name];
            }
            return _self;
        },
        /**
		 * 设置属性值，会触发before+Name+Change,和 after+Name+Change事件
		 * @param {String|Object} name  属性名
		 * @param {Object} value 值
		 * @param {Object} opts 配置项
		 */
        set: function (name, value, opts) {
            var _self = this;
            if ($.isPlainObject(name)) {
                opts = value;
                var all = Object(name),
                    attrs = [];
                for (name in all) {
                    if (all.hasOwnProperty(name)) {
                        setInternal(_self, name, all[name], opts);
                    }
                }
                return _self;
            }
            return setInternal(_self, name, value, opts);
        },
        /**
		 * 设置属性，不触发事件    
		 * @param  {String} name  属性名
		 * @param  {Object} value 属性值
		 * @return {Boolean|undefined}   如果值无效则返回false,否则返回undefined
		 */
        setInternal: function (name, value, opts) {
            return this._set(name, value, opts);
        },
        /**
        * 获取静态属性
        */
        _getDefAttrVal: function (name) {
            var _self = this,
                attrs = _self.__attrs,
                attrConfig = ensureNonEmpty(attrs, name),
                valFn = attrConfig.valueFn,
                val;
            if (valFn && (valFn = normalFn(_self, valFn))) {
                val = valFn.call(_self);
                if (val !== undefined) {
                    attrConfig.value = val;
                }
                delete attrConfig.valueFn;
                attrs[name] = attrConfig;
            }
            return attrConfig.value;
        },
        /**
		 * 设置属性 内部方法
		 * @param {Object} name
		 * @param {Object} value
		 * @param {Object} opts
		 */
        _set: function (name, value, opts) {
            var _self = this,
                setValue, attrConfig = ensureNonEmpty(_self.__attrs, name, true),
                setter = attrConfig["setter"];
            if (setter && (setter = normalFn(_self, setter))) {
                setValue = setter.call(_self, value, name);
            }
            if (setValue === INVALID) {
                return false;
            }
            if (setValue !== undefined) {
                value = setValue;
            }
            _self.__attrVals[name] = value;
            return _self;
        },
        /**
		 * 初始化实例属性
		 * @param {Object} config
		 */
        _initAttrs: function (config) {
            var _self = this;
            if (config) {
                for (var attr in config) {
                    if (config.hasOwnProperty(attr)) {
                        _self._set(attr, config[attr]);
                    }
                }
            }
        }
    });
    BUI.Base = Base;
})(window.BUI, jQuery);
//BUI.Component +
(function (BUI) {
    "use strict";
    var Component = {};

    /**
	 * 根据Xclass创建对象
	 * @method
	 * @static
	 * @param  {Object} component 控件的配置项或者控件
	 * @param  {Object} self      父类实例
	 * @return {Object} 实例对象
	 */
    function create(component, self) {
        var childConstructor, xclass;
        if (component && (xclass = component.xclass)) {
            if (self && !component.prefixCls) {
                component.prefixCls = BUI.prefix;
            }
            childConstructor = Component.Manager.getConstructorByXClass(xclass);
            if (!childConstructor) {
                BUI.error("can not find class by xclass desc : " + xclass);
            }
            component = new childConstructor(component);
        }
        return component;
    }
    Component.create = create;
    BUI.Component = Component;
})(window.BUI);
//BUI.Component.Manager +
(function (BUI, $) {
    "use strict";
    var uis = {};
    var componentInstances = {};
    var Manager = {
        __instances: componentInstances,
        /**
		 * 每实例化一个控件，就注册到管理器上
		 * @param {String} id  控件 id
		 * @param {BUI.Component.Controller} component 控件对象
		 */
        addComponent: function (id, component) {
            componentInstances[id] = component;
        },
        /**
		 * 移除注册的控件
		 * @param  {String} id 控件 id
		 */
        removeComponent: function (id) {
            delete componentInstances[id];
        },
        /**
		 * 遍历所有的控件
		 * @param  {Function} fn 遍历函数
		 */
        eachComponent: function (fn) {
            BUI.each(componentInstances, fn);
        },
        /**
		 * 根据Id获取控件
		 * @param  {String} id 编号
		 * @return {BUI.Component.UIBase}   继承 UIBase的类对象
		 */
        getComponent: function (id) {
            return componentInstances[id];
        },
        /**
		 * 获取class类名 格式:prefix-class
		 * @param {Object} cls
		 */
        getCssClassWithPrefix: function (cls) {
            var cs = $.trim(cls).split(/\s+/);
            for (var i = 0; i < cs.length; i++) {
                if (cs[i]) {
                    cs[i] = this.get("prefixCls") + cs[i];
                }
            }
            return cs.join(" ");
        },
        /**
		 * 通过构造函数获取xclass.
		 * @param {Function} constructor 控件的构造函数.
		 * @type {Function}
		 * @return {String}
		 * @method
		 */
        getXClassByConstructor: function (constructor) {
            for (var u in uis) {
                var ui = uis[u];
                if (ui.constructor == constructor) {
                    return u;
                }
            }
            return 0;
        },
        /**
		 * 通过xclass获取控件的构造函数
		 * @param {String} classNames Class names separated by space.
		 * @type {Function}
		 * @return {Function}
		 * @method
		 */
        getConstructorByXClass: function (cls) {
            var cs = cls.split(/\s+/),
                p = -1,
                t, ui = null;
            for (var i = 0; i < cs.length; i++) {
                var uic = uis[cs[i]];
                if (uic && (t = uic.priority) > p) {
                    p = t;
                    ui = uic.constructor;
                }
            }
            return ui;
        },
        /**
		 * 将 xclass 同构造函数相关联.
		 * @type {Function}
		 * @param {String} className 控件的xclass名称.
		 * @param {Function} componentConstructor 构造函数
		 * @method
		 */
        setConstructorByXClass: function (cls, uic) {
            if (BUI.isFunction(uic)) {
                uis[cls] = {
                    constructor: uic,
                    priority: 0
                };
            } else {
                uic.priority = uic.priority || 0;
                uis[cls] = uic;
            }
        }
    };
    BUI.Component.Manager = Manager;
})(window.BUI, jQuery);
//BUI.Component.UIBase +
(function (BUI, $) {
    "use strict";
    var Manager = BUI.Component.Manager,
        UI_SET = "_uiSet",
        ATTRS = "ATTRS",
        ucfirst = BUI.ucfirst,
        noop = $.noop,
        Base = BUI.Base;

    /**
	 * 模拟多继承 ATTRS
	 */
    function initHierarchy(host, config) {
        callMethodByHierarchy(host, "initializer");
    }

    function callMethodByHierarchy(host, mainMethod) {
        var c = host.constructor,
            main, t = [];
        while (c) {
            //只调用真正自己构造器原型的定义，继承原型链上的不要管
            if (c.prototype.hasOwnProperty(mainMethod) && (main = c.prototype[mainMethod])) {
                t.push(main);
            }
            c = c.superclass && c.superclass.constructor;
        }
        // 初始化函数 顺序： 父类对应函数  -> 子类对应函数
        for (var i = t.length - 1; i >= 0; i--) {
            t[i] && t[i].call(host);
        }
    }

    /**
	 * 销毁组件顺序： 子类 destructor ->  父类 destructor 
	 */
    function destroyHierarchy(host) {
        var c = host.constructor,
            extensions, d, i;
        while (c) {
            if (c.prototype.hasOwnProperty("destructor")) {
                c.prototype.destructor.apply(host);
            }
            c = c.superclass && c.superclass.constructor;
        }
    }

    function bindUI(self) { }

    /**
	 * 根据当前（初始化）状态来设置 UI
	 */
    function syncUI(self) {
        var v, f, attrs = self.getAttrs();
        for (var a in attrs) {
            if (attrs.hasOwnProperty(a)) {
                var m = UI_SET + ucfirst(a);
                if ((f = self[m]) && attrs[a].sync !== false && (v = self.get(a)) !== undefined) {
                    f.call(self, v);
                }
            }
        }
    }

    /**
	 * 构建 插件
	 */
    function constructPlugins(plugins) {
        if (!plugins) {
            return;
        }
        BUI.each(plugins, function (plugin, i) {
            if (BUI.isFunction(plugin)) {
                plugins[i] = new plugin();
            }
        });
    }

    /**
	 * 调用插件的方法
	 */
    function actionPlugins(self, plugins, action) {
        if (!plugins) {
            return;
        }
        BUI.each(plugins, function (plugin, i) {
            if (plugin[action]) {
                plugin[action](self);
            }
        });
    }

    var UIBase = function (config) {
        var _self = this,
            id;
        // 读取用户设置的属性值并设置到自身
        Base.apply(_self, arguments);
        // 按照类层次执行初始函数，主类执行 initializer 函数，扩展类执行构造器函数
        initHierarchy(_self, config);
        var plugins = _self.get('plugins');
        constructPlugins(plugins);
        var xclass = _self.get("xclass");
        if (xclass) {
            _self.__xclass = xclass;
        }
        actionPlugins(_self, plugins, 'initializer');
        // 是否自动渲染
        config && config.autoRender && _self.render();
    };
    UIBase.ATTRS = {
        /**
		 * 是否自动渲染,如果不自动渲染，需要用户调用 render()方法
		 */
        autoRender: {
            value: false
        },
        /**
		 * 事件处理函数
		 */
        listeners: {
            value: {}
        },
        /**
		 * 是否已经渲染完成
		 */
        rendered: {
            value: false
        },
        /**
		 * 插件集合
		 */
        plugins: {
            value: []
        },
        /**
		 * 获取控件的 xclass
		 */
        xclass: {
            valueFn: function () {
                return Manager.getXClassByConstructor(this.constructor);
            }
        }
    };
    BUI.extend(UIBase, Base);
    BUI.augment(UIBase, {
        /**
		 * 创建dom
		 */
        create: function () {
            var self = this;
            if (!self.get("created")) {

                self.fire("beforeCreateDom");
                callMethodByHierarchy(self, "createDom");
                self._set("created", true);
                self.fire("afterCreateDom");
                actionPlugins(self, self.get('plugins'), 'createDom');
            }
            return self;
        },
        /**
		 * 渲染
		 */
        render: function () {
            var _self = this;
            if (!_self.get("rendered")) {
                var plugins = _self.get('plugins');
                _self.create(undefined);
                _self.set('created', true);

                _self.fire("beforeRenderUI");
                callMethodByHierarchy(_self, "renderUI");
                _self.fire("afterRenderUI");
                actionPlugins(_self, plugins, 'renderUI');

                _self.fire("beforeBindUI");
                bindUI(_self);
                callMethodByHierarchy(_self, "bindUI");
                _self.fire("afterBindUI");
                _self._set("binded", true);
                actionPlugins(_self, plugins, 'bindUI');

                _self.fire("beforeSyncUI");
                syncUI(_self);
                callMethodByHierarchy(_self, "syncUI");
                _self.fire("afterSyncUI");
                actionPlugins(_self, plugins, 'syncUI');

                _self._set("rendered", true);
            }
            return _self;
        },
        createDom: noop,
        renderUI: noop,
        bindUI: noop,
        syncUI: noop,
        destroy: function () {
            var _self = this;
            //防止返回销毁
            if (_self.destroyed) {
                return _self;
            }
            _self.fire("beforeDestroy");
            actionPlugins(_self, _self.get('plugins'), 'destructor');
            destroyHierarchy(_self);
            _self.fire("afterDestroy");
            _self.off();
            _self.clearAttrVals();
            _self.destroyed = true;
            return _self;
        }
    });

    //延时处理构造函数
    function initConstuctor(c) {
        var constructors = [];
        while (c.base) {
            constructors.push(c);
            c = c.base;
        }
        for (var i = constructors.length - 1; i >= 0; i--) {
            var C = constructors[i];
            BUI.mix(C.prototype, C.px);
            BUI.mix(C, C.sx);
            C.base = null;
            C.px = null;
            C.sx = null;
        }
    }
    BUI.mix(UIBase, {
        /**
		 * 定义一个类
		 * @static
		 * @param  {Function} base   基类构造函数
		 * @param  {Object} px  原型链上的扩展
		 * @param  {Object} sx  类上的扩展
		 * @return {Function} 继承与基类的构造函数
		 */
        define: function (base, px, sx) {
            function C() {
                var c = this.constructor;
                if (c.base) {
                    initConstuctor(c);
                }
                UIBase.apply(this, arguments);
            }
            BUI.extend(C, base);
            C.base = base;
            C.px = px; //延迟复制原型链上的函数
            C.sx = sx; //延迟复制静态属性
            return C;
        },
        /**
		 * 扩展一个类，基类就是类本身
		 * @static
		 * @param  {Object} px  原型链上的扩展
		 * @param  {Object} sx  静态属性的扩展
		 * @return {Function} 继承与基类的构造函数
		 */
        extend: function extend(px, sx) {
            var args = $.makeArray(arguments),
                ret, last = args[args.length - 1];
            //将this添加的参数列表开头
            args.unshift(this);
            ret = UIBase.define.apply(UIBase, args);
            if (last.xclass) {
                var priority = last.priority || (this.priority ? this.priority + 1 : 1);
                Manager.setConstructorByXClass(last.xclass, {
                    constructor: ret,
                    priority: priority
                });
                ret.__xclass = last.xclass;
                ret.priority = priority;
                ret.toString = function () {
                    return last.xclass;
                };
            }
            ret.extend = extend;
            return ret;
        }
    });
    BUI.Component.UIBase = UIBase;
})(window.BUI, jQuery);
//BUI.Component.Controller +
(function (BUI, $) {
    "use strict";
    var UIBase = BUI.Component.UIBase,
        Manager = BUI.Component.Manager,
        wrapBehavior = BUI.wrapBehavior,
        getWrapBehavior = BUI.getWrapBehavior,
        doc = document;

    //鼠标事件是否在元素内部
    function isMouseEventWithinElement(e, elem) {
        var relatedTarget = e.relatedTarget;
        return relatedTarget && (relatedTarget === elem[0] || $.contains(elem, relatedTarget));
    }
    /**
     * 控制器的容器关系:render -> el -> contentEl -> childContainer -> children
     * */
    var Controller = UIBase.extend({
        isController: true,
        /**
		 * 初始化
		 */
        initializer: function () {
            var self = this,
                defaultChildCfg = self.get("defaultChildCfg");
            if (defaultChildCfg) {
                self.on("beforeAddChild", function (ev) {
                    var child = ev.child;
                    if ($.isPlainObject(child)) {
                        BUI.each(defaultChildCfg, function (v, k) {
                            if (child[k] == null) {
                                child[k] = v;
                            }
                        });
                    }
                });
            }
            if (!self.get("id")) {
                self.set("id", self.getNextUniqueId());
            }
            Manager.addComponent(self.get("id"), self);
        },
        /**
		 * 获取唯一标识,结果是 'xclass' + number
		 */
        getNextUniqueId: function () {
            var self = this,
                xclass = Manager.getXClassByConstructor(self.constructor);
            return BUI.guid(xclass);
        },
        /**
		 * 创建html DOM
		 */
        createDom: function () {
            var self = this,
                contentEl = self.get("contentEl"),
                el = self.get("el"),
                srcNode = self.get("srcNode");
            if (!srcNode) {
                el = $("<" + self.get("elTagName") + ">");
                if (contentEl) {
                    el.append(contentEl);
                }
            } else {
                el = $(srcNode);
            }
            self.setInternal("el", el);
            el.addClass(self.getComponentCssClassWithState());
            if (!contentEl) {
                self.setInternal("contentEl", el);
            }
        },
        /**
		 * 根据状态获取组件class
		 * @param {Object} state
		 */
        getComponentCssClassWithState: function (state) {
            var self = this,
                componentCls = self.get("xclass");
            if (state) {
                state = "-" + state;
            } else {
                state = "";
            }
            return self.getCssClassWithPrefix(componentCls.split(/\s+/).join(state + " ") + state);
        },
        /**
		 * 使用前缀获取类的名字
		 * @param classes {String} class names without prefixCls. Separated by space.
		 * @method
		 * @protected
		 * @return {String} class name with prefixCls
		 */
        getCssClassWithPrefix: Manager.getCssClassWithPrefix,
        /**
		 * 渲染组件 父组件->子组件
		 */
        renderUI: function () {
            var self = this;
            if (!self.get("srcNode")) {
                self.setTplContent();
            }
            var self = this,
                contentContainer = self.get("childContainer"),
                contentEl;
            if (contentContainer) {
                contentEl = self.get("el").find(contentContainer);
                if (contentEl.length) {
                    self.set("contentEl", contentEl);
                }
            }
            if (!self.get("srcNode")) {
                var render = self.get("render"),
                    el = self.get("el"),
                    renderBefore = self.get("elBefore");
                if (renderBefore) {
                    el.insertBefore(renderBefore, undefined);
                } else if (render) {
                    el.appendTo(render, undefined);
                } else {
                    el.appendTo(doc.body, undefined);
                }
            }
            self._initChildren();
        },
        /**
        * 获取模版内容,如果content不为空，则忽略tpl
        */
        setTplContent: function () {
            var _self = this,
                attrs = _self.getAttrVals(),
                el = _self.get("el"),
                content = _self.get("content"),
                tpl = _self.getTpl(attrs);
            if (!content && tpl) {
                el.empty();
                el.html(tpl);
            }
        },
        getTpl: function (attrs) {
            var _self = this,
                tpl = _self.get("tpl"),
                tplRender = _self.get("tplRender");
            attrs = attrs || _self.getAttrVals();
            if (tplRender) {
                return tplRender(attrs);
            }
            if (tpl) {
                return BUI.substitute(tpl, attrs);
            }
            return "";
        },
        _initChildren: function (children) {
            var self = this,
                i, children, child;
            children = children || self.get("children").concat();
            self.get("children").length = 0;
            for (i = 0; i < children.length; i++) {
                child = self.addChild(children[i]);
                child.render();
            }
        },
        /**
		 * 添加控件的子控件，索引值为 0-based
		 * @param {BUI.Component.Controller|Object} c 子控件的实例或者配置项
		 * @param {String} [c.xclass] 如果c为配置项，设置c的xclass
		 * @param {Number} [index]  0-based  如果未指定索引值，则插在控件的最后
		 */
        addChild: function (c, index) {
            var self = this,
                children = self.get("children"),
                renderBefore;
            if (index === undefined) {
                index = children.length;
            }
            self.fire("beforeAddChild", {
                child: c,
                index: index
            });
            renderBefore = children[index] && children[index].get("el") || null;
            c = self.initChild(c, renderBefore);
            children.splice(index, 0, c);
            if (self.get("rendered")) {
                c.render();
            }
            self.fire("afterAddChild", {
                child: c,
                index: index
            });
            return c;
        },
        initChild: function (c, renderBefore) {
            var self = this;
            self.create();
            var contentEl = self.getContentElement(),
                defaultCls = self.get("defaultChildClass");
            if (!c.xclass && !(c instanceof Controller)) {
                if (!c.xtype) {
                    c.xclass = defaultCls;
                } else {
                    c.xclass = defaultCls + "-" + c.xtype;
                }
            } else {
                if (c.render && !c.isController) {
                    contentEl = $(c.render);
                }
            }
            c = BUI.Component.create(c, self);
            c.setInternal("parent", self);
            c.set("render", contentEl);
            c.set("elBefore", renderBefore);
            c.create(undefined);
            return c;
        },
        /**
		 * 绑定事件
		 */
        bindUI: function () {
            var self = this,
                events = self.get("events");
            this.on("afterVisibleChange", function (e) {
                this.fire(e.newVal ? "show" : "hide");
            });
            BUI.each(events, function (v, k) {
                self.publish(k, {
                    bubbles: v
                });
            });
        },
        /**
		 * 是否包含元素
		 */
        containsElement: function (elem) {
            var _self = this,
                el = _self.get("el"),
                children = _self.get("children"),
                result = false;
            if (!_self.get("rendered")) {
                return false;
            }
            if ($.contains(el[0], elem) || el[0] === elem) {
                result = true;
            } else {
                BUI.each(children, function (item) {
                    if (item.containsElement(elem)) {
                        result = true;
                        return false;
                    }
                });
            }
            return result;
        },
        /**
		 * 是否是子节点
		 * @param {Object} elem
		 */
        isChildrenElement: function (elem) {
            var _self = this,
                children = _self.get("children"),
                rst = false;
            BUI.each(children, function (child) {
                if (child.containsElement(elem)) {
                    rst = true;
                    return false;
                }
            });
            return rst;
        },
        /**
		 * 显示
		 */
        show: function () {
            var self = this;
            self.render();
            self.set("visible", true);
            return self;
        },
        /**
		 * 隐藏
		 */
        hide: function () {
            var self = this;
            self.set("visible", false);
            return self;
        },
        /**
		 * 交替显示或者隐藏
		 */
        toggle: function () {
            this.set("visible", !this.get("visible"));
            return this;
        },
        /**
         * 是否可以获取焦点
         * 如果可以则监听 blur focus keydown keyup事件
         */
        _uiSetFocusable: function (focusable) {
            var self = this,
                t, el = self.getKeyEventTarget();
            if (focusable) {
                el.attr("tabIndex", 0)
                    .attr("hideFocus", true)
                    .on("focus", wrapBehavior(self, "handleFocus"))
                    .on("blur", wrapBehavior(self, "handleBlur"))
                    .on("keydown", wrapBehavior(self, "handleKeydown"))
                    .on("keyup", wrapBehavior(self, "handleKeyUp"));
            } else {
                el.removeAttr("tabIndex");
                if (t = getWrapBehavior(self, "handleFocus")) {
                    el.off("focus", t);
                }
                if (t = getWrapBehavior(self, "handleBlur")) {
                    el.off("blur", t);
                }
                if (t = getWrapBehavior(self, "handleKeydown")) {
                    el.off("keydown", t);
                }
                if (t = getWrapBehavior(self, "handleKeyUp")) {
                    el.off("keyup", t);
                }
            }
        },
        /**
         * 是否可以监听鼠标事件
         * 如果可以则监听 mouseenter mouseleave click dblclick事件
         */
        _uiSetHandleMouseEvents: function (handleMouseEvents) {
            var self = this,
                el = self.get("el"),
                t;
            if (handleMouseEvents) {
                el.on("mouseenter", wrapBehavior(self, "handleMouseEnter"))
                    .on("mouseleave", wrapBehavior(self, "handleMouseLeave"))
                    .on("click", wrapBehavior(self, "handleClick"))
                    .on("dblclick", wrapBehavior(self, "handleDblClick"));
            } else {
                t = getWrapBehavior(self, "handleMouseEnter") && el.off("mouseenter", t);
                t = getWrapBehavior(self, "handleMouseLeave") && el.off("mouseleave", t);
                t = getWrapBehavior(self, "handleClick") && el.off("click", t);
                t = getWrapBehavior(self, "handleDblClick") && el.off("dblclick", t);
            }
        },
        _uiSetFocused: function (v) {
            var self = this,
                el = self.get("el"),
                componentCls = self.getStatusCls("focused");
            el[v ? "addClass" : "removeClass"](componentCls);
            if (v) {
                this.getKeyEventTarget()[0].focus();
            }
        },
        /**
         * 设置元素显示隐藏
         */
        _uiSetVisible: function (isVisible) {
            var self = this,
                el = self.get("el"),
                visibleMode = self.get("visibleMode");
            if (visibleMode === "visibility") {
                el.css("visibility", isVisible ? "visible" : "hidden");
            } else {
                el.css("display", isVisible ? "" : "none");
            }
            if (visibleMode === "visibility") {
                if (isVisible) {
                    var position = self.get("cachePosition");
                    if (position) {
                        self.set("xy", position);
                    }
                } else {
                    var position = [self.get("x"), self.get("y")];
                    self.set("cachePosition", position);
                    self.set("xy", [-999, -999]);
                }
            }
        },
        /**
        * 设置元素属性
        */
        _uiSetElAttrs: function (attrs) {
            this.get("el").attr(attrs);
        },
        /**
         * 设置类名
         */
        _uiSetElCls: function (cls) {
            this.get("el").addClass(cls);
        },
        /**
        * 样式
        */
        _uiSetElStyle: function (style) {
            this.get("el").css(style);
        },
        _uiSetWidth: function (w) {
            this.get("el").width(w);
        },
        _uiSetHeight: function (h) {
            var self = this;
            self.get("el").height(h);
        },
        _uiSetHighlighted: function (v) {
            var self = this,
                componentCls = self.getStatusCls("hover"),
                el = self.get("el");
            el[v ? "addClass" : "removeClass"](componentCls);
        },
        _uiSetContent: function (c) {
            var self = this,
                el;
            if (self.get("srcNode") && !self.get("rendered")) { } else {
                el = self.get("contentEl");
                if (typeof c == "string") {
                    el.html(c);
                } else if (c) {
                    el.empty().append(c);
                }
            }
        },
        _uiSetDisabled: function (v) {
            var self = this,
                el = self.get("el");
            if (v && self.get("highlighted")) {
                self.set("highlighted", false);
            }
            if (self.get("focusable")) {
                self.getKeyEventTarget().attr("tabIndex", v ? -1 : 0);
            }
        },
        /*
         * 解除禁用
         */
        enable: function () {
            this.set("disabled", false);
            return this;
        },
        /*
       * 禁用
       */
        disable: function () {
            this.set("disabled", true);
            return this;
        },
        /*
       * 获取焦点
       */
        focus: function () {
            if (this.get("focusable")) {
                this.set("focused", true);
            }
        },
        getStatusCls: function (name) {
            var self = this,
                statusCls = self.get("statusCls"),
                cls = statusCls[name];
            if (!cls) {
                cls = self.getComponentCssClassWithState(name);
            }
            return cls;
        },
        /**
		 * 子组件将要渲染到的节点，在 render 类上覆盖对应方法      
		 */
        getContentElement: function () {
            return this.get("contentEl") || this.get("el");
        },
        /**
		 * 焦点所在元素即键盘事件处理元素，在 render 类上覆盖对应方法       
		 */
        getKeyEventTarget: function () {
            return this.get("el");
        },
        /**
		 * 将自己从父控件中移除        
		 * @param  {Boolean} destroy 是否删除DON节点
		 * @return {BUI.Component.Controller} 删除的子对象.
		 */
        remove: function (destroy) {
            var self = this,
                parent = self.get("parent");
            if (parent) {
                parent.removeChild(self, destroy);
            } else if (destroy) {
                self.destroy();
            }
            return self;
        },
        /**
		 * 移除子控件，并返回移除的控件         
		 * @param {BUI.Component.Controller} c 要移除的子控件.
		 * @param {Boolean} [destroy=false] 如果是true,
		 * 调用控件的方法 {@link BUI.Component.UIBase#destroy} .
		 * @return {BUI.Component.Controller} 移除的子控件.
		 */
        removeChild: function (c, destroy) {
            var self = this,
                children = self.get("children"),
                index = BUI.Array.indexOf(c, children);
            if (index === -1) {
                return;
            }
            self.fire("beforeRemoveChild", {
                child: c,
                destroy: destroy
            });
            if (index !== -1) {
                children.splice(index, 1);
            }
            if (destroy && c.destroy) {
                c.destroy();
            }
            self.fire("afterRemoveChild", {
                child: c,
                destroy: destroy
            });
            return c;
        },
        /**
		 * 删除当前控件的子控件
		 * @param {Boolean} [destroy] 如果设置 true,
		 * 调用子控件的 {@link BUI.Component.UIBase#destroy}方法.
		 */
        clearChildren: function (destroy) {
            var self = this,
                i, t = [].concat(self.get("children"));
            for (i = 0; i < t.length; i++) {
                self.removeChild(t[i], destroy);
            }
        },
        /**
		 * 根据索引获取子控件       
		 * @param {Number} index 0-based 索引值.
		 * @return {BUI.Component.Controller} 子控件或者null 
		 */
        getChildAt: function (index) {
            var children = this.get("children");
            return children[index] || null;
        },
        /**
		 * 根据Id获取子控件      
		 * @param  {String} id 控件编号
		 * @param  {Boolean} deep 是否继续查找在子控件中查找
		 * @return {BUI.Component.Controller} 子控件或者null 
		 */
        getChild: function (id, deep) {
            return this.getChildBy(function (item) {
                return item.get("id") === id;
            }, deep);
        },
        /**
		 * 通过匹配函数查找子控件，返回第一个匹配的对象       
		 * @param  {Function} math 查找的匹配函数
		 * @param  {Boolean} deep 是否继续查找在子控件中查找
		 * @return {BUI.Component.Controller} 子控件或者null 
		 */
        getChildBy: function (math, deep) {
            return this.getChildrenBy(math, deep)[0] || null;
        },
        getAppendHeight: function () {
            var el = this.get("el");
            return el.outerHeight() - el.height();
        },
        getAppendWidth: function () {
            var el = this.get("el");
            return el.outerWidth() - el.width();
        },
        /**
		 * 查找符合条件的子控件        
		 * @param  {Function} math 查找的匹配函数
		 * @param  {Boolean} deep 是否继续查找在子控件中查找，如果符合上面的匹配函数，则不再往下查找
		 * @return {BUI.Component.Controller[]} 子控件数组 
		 */
        getChildrenBy: function (math, deep) {
            var self = this,
                results = [];
            if (!math) {
                return results;
            }
            self.eachChild(function (child) {
                if (math(child)) {
                    results.push(child);
                } else if (deep) {
                    results = results.concat(child.getChildrenBy(math, deep));
                }
            });
            return results;
        },
        /**
		 * 遍历子元素        
		 * @param  {Function} func 迭代函数，函数原型function(child,index)
		 */
        eachChild: function (func) {
            BUI.each(this.get("children"), func);
        },
        /**
         * 鼠标指针进入（穿过）元素
         */
        handleMouseEnter: function (ev) {
            var self = this;
            this.set("highlighted", true);
            self.fire("mouseenter", {
                domTarget: ev.target,
                domEvent: ev
            });
        },
        /**
        * 鼠标指针离开元素
        */
        handleMouseLeave: function (ev) {
            var self = this;
            self.set("active", false);
            self.set("highlighted", false);
            self.fire("mouseleave", {
                domTarget: ev.target,
                domEvent: ev
            });
        },
        /**
        * 鼠标双击
        */
        handleDblClick: function (ev) {
            if (!this.isChildrenElement(ev.target)) {
                this.fire("dblclick", {
                    domTarget: ev.target,
                    domEvent: ev
                });
            }
        },
        /**
         * 鼠标单击
         */
        handleClick: function (ev) {
            var self = this,
                n, target = $(ev.target),
                el;
            el = self.getKeyEventTarget();
            if (self.get("focusable")) {
                self.setInternal("focused", true);
            }
            if (!self.isChildrenElement(ev.target)) {
                self.fire("click", {
                    domTarget: ev.target,
                    domEvent: ev
                });
            }
        },
        /**
         * 获取焦点
         */
        handleFocus: function (ev) {
            this.set("focused", !!ev);
            this.fire("focus", {
                domEvent: ev,
                domTarget: ev.target
            });
        },
        /**
         * 失去焦点
         */
        handleBlur: function (ev) {
            this.set("focused", !ev);
            this.fire("blur", {
                domEvent: ev,
                domTarget: ev.target
            });
        },
        /**
        * 按钮被按下
        */
        handleKeydown: function (ev) {
            var self = this;
            if (self.handleKeyEventInternal(ev)) {
                ev.halt();
                return true;
            }
        },
        /**
        * 按钮被松开
        */
        handleKeyUp: function (ev) {
            var self = this;
            if (!self.isChildrenElement(ev.target)) {
                self.fire("keyup", {
                    domTarget: ev.target,
                    domEvent: ev
                });
            }
        },
        /**
        * 键盘内部处理事件
        */
        handleKeyEventInternal: function (ev) {
            var self = this,
                isChildrenElement = self.isChildrenElement(ev.target);
            if (ev.which === 13) {
                if (!isChildrenElement) {
                    self.fire("click", {
                        domTarget: ev.target,
                        domEvent: ev
                    });
                }
            }
            if (!isChildrenElement) {
                self.fire("keydown", {
                    domTarget: ev.target,
                    domEvent: ev
                });
            }
            return false;
        },
        destructor: function () {
            var self = this,
                id, i, children = self.get("children");
            id = self.get("id");
            for (i = 0; i < children.length; i++) {
                children[i].destroy && children[i].destroy();
            }
            var el = this.get("el");
            if (el) {
                el.remove();
            }
            Manager.removeComponent(id);
        },
        set: function (name, value, opt) {
            var _self = this,
                attr = _self.__attrs[name],
                ucName, ev, m;
            if (BUI.isObject(name)) {
                opt = value;
                BUI.each(name, function (v, k) {
                    _self.set(k, v, opt);
                });
            }
            if (!attr) {
                _self.setInternal(name, value, opt);
                return _self;
            }
            var prevVal = Controller.superclass.get.call(this, name);
            if (!$.isPlainObject(value) && !BUI.isArray(value) && prevVal === value) {
                return _self;
            }
            ucName = BUI.ucfirst(name);
            m = "_uiSet" + ucName;
            _self.fire("before" + ucName + "Change", {
                attrName: name,
                prevVal: prevVal,
                newVal: value
            });
            _self.setInternal(name, value);
            if (_self.get("binded") && _self[m]) {
                _self[m](value, ev);
            }
            ev = {
                attrName: name,
                prevVal: prevVal,
                newVal: value
            };
            _self.fire("after" + ucName + "Change", ev);
            return _self;
        },
        get: function (name) {
            var _self = this;
            return Controller.superclass.get.call(this, name);
        }
    }, {
            ATTRS: {
                content: {},
                defaultChildCfg: {},
                elTagName: {
                    value: "div"
                },
                defaultChildClass: {},
                xtype: {},
                id: {},
                width: {},
                height: {},
                elCls: {},
                elStyle: {},
                elAttrs: {},
                elBefore: {},
                el: {
                    setter: function (v) {
                        return $(v);
                    }
                },
                tpl: {
                    sync: false
                },
                contentEl: {
                    valueFn: function () {
                        return this.get("el");
                    }
                },
                tplRender: {
                    value: null
                },
                childContainer: {},
                events: {
                    value: {
                        click: true,
                        dblclick: true,
                        mouseenter: false,
                        mouseleave: false,
                        keydown: false,
                        keyup: false,
                        focus: false,
                        blur: false,
                        show: false,
                        hide: false
                    }
                },
                render: {},
                statusCls: {
                    value: {}
                },
                visibleMode: {
                    value: "display"
                },
                visible: {
                    value: true
                },
                handleMouseEvents: {
                    value: true
                },
                focusable: {
                    value: false
                },
                activeable: {
                    value: true
                },
                focused: {},
                active: {},
                highlighted: {},
                cachePosition: {},
                children: {
                    sync: false,
                    shared: false,
                    value: []
                },
                prefixCls: {
                    value: BUI.prefix
                },
                parent: {
                    setter: function (p) {
                        this.addTarget(p);
                    }
                },
                disabled: {
                    value: false
                }
            }
        }, {
            xclass: "controller",
            priority: 0
        });
    BUI.Component.Controller = Controller;
})(window.BUI, jQuery);;
///<jscompress sourcefile="02cookie.js" />
//BUI.Cookie
(function (BUI) {
    "use strict";
    var doc = document,
        MILLISECONDS_OF_DAY = 24 * 60 * 60 * 1e3,
        encode = encodeURIComponent,
        decode = decodeURIComponent;

    function isNotEmptyString(val) {
        return typeof val === "string" && val !== "";
    }
    var Cookie = {
        /**
		 * Returns the cookie value for given name
		 * @return {String} name The name of the cookie to retrieve
		 */
        get: function (name) {
            var ret, m;
            if (isNotEmptyString(name)) {
                if (m = String(doc.cookie).match(new RegExp("(?:^| )" + name + "(?:(?:=([^;]*))|;|$)"))) {
                    ret = m[1] ? decode(m[1]) : "";
                }
            }
            return ret;
        },
        /**
		 * Set a cookie with a given name and value
		 * @param {String} name The name of the cookie to set
		 * @param {String} val The value to set for cookie
		 * @param {Number|Date} expires
		 * if Number secified how many days this cookie will expire
		 * @param {String} domain set cookie's domain
		 * @param {String} path set cookie's path
		 * @param {Boolean} secure whether this cookie can only be sent to server on https
		 */
        set: function (name, val, expires, domain, path, secure) {
            var text = String(encode(val)),
                date = expires;
            if (typeof date === "number") {
                date = new Date();
                date.setTime(date.getTime() + expires * MILLISECONDS_OF_DAY);
            }
            if (date instanceof Date) {
                text += "; expires=" + date.toUTCString();
            }
            if (isNotEmptyString(domain)) {
                text += "; domain=" + domain;
            }
            if (isNotEmptyString(path)) {
                text += "; path=" + path;
            }
            if (secure) {
                text += "; secure";
            }
            doc.cookie = name + "=" + text;
        },
        /**
		 * Remove a cookie from the machine by setting its expiration date to sometime in the past
		 * @param {String} name The name of the cookie to remove.
		 * @param {String} domain The cookie's domain
		 * @param {String} path The cookie's path
		 * @param {String} secure The cookie's secure option
		 */
        remove: function (name, domain, path, secure) {
            this.set(name, "", -1, domain, path, secure);
        }
    };
    BUI.Cookie = Cookie;
})(window.BUI);;
///<jscompress sourcefile="03data.js" />
//BUI.Data.Store +
(function (BUI) {
    "use strict";
    BUI.Data = {};
    var ASC = "ASC",
        DESC = "DESC";
    /**
	 * 删除数组指定索引元素
	 * @param {Object} index
	 * @param {Object} array
	 */
    function removeAt(index, array) {
        if (index < 0) {
            return;
        }
        var records = array,
            record = records[index];
        records.splice(index, 1);
        return record;
    }

    /**
	 * 删除数组指定元素
	 * @param {Object} record
	 * @param {Object} array
	 */
    function removeFrom(record, array) {
        var index = BUI.Array.indexOf(record, array);
        if (index >= 0) {
            removeAt(index, array);
        }
    }

    /**
	 * 是否包含指定元素
	 * @param {Object} record
	 * @param {Object} array
	 */
    function contains(record, array) {
        return BUI.Array.indexOf(record, array) !== -1;
    }


    var store = function (config) {
        store.superclass.constructor.call(this, config);
        this._init();
    };
    store.ATTRS = {
        /**
		 * 比较函数
		 * @cfg {Function} compareFunction
		 * 函数原型 function(v1,v2)，比较2个字段是否相等
		 * 如果是字符串则按照本地比较算法，否则使用 > ,== 验证
		 */
        compareFunction: {
            value: function (v1, v2) {
                if (v1 === undefined) {
                    v1 = "";
                }
                if (v2 === undefined) {
                    v2 = "";
                }
                if (BUI.isString(v1)) {
                    return v1.localeCompare(v2);
                }
                if (v1 > v2) {
                    return 1;
                } else if (v1 === v2) {
                    return 0;
                } else {
                    return -1;
                }
            }
        },
        /**
		 * 排序字段
		 * @type {String}
		 */
        sortField: {},
        /**
		 * 排序方向,'ASC'、'DESC'
		 * @type {String}
		 */
        sortDirection: {
            value: "ASC"
        },
        /**
		 * 对比2个对象是否相当，在去重、更新、删除，查找数据时使用此函数
		 * @type {Object}
		 * 
		 */
        matchFunction: {
            value: function (obj1, obj2) {
                return obj1 == obj2;
            }
        },
        /**
		 * 事件
		 */
        events: {
            value: ["load", "add", "remove", "update"]
        },
        /**
		 * 上次查询的参数
		 * @type {Object}
		 * @readOnly
		 */
        lastParams: {
            shared: false,
            value: {}
        },
        /**
		 * 参数
		 */
        params: {},
        /**
		 * 本地数据源
		 * @type {Array}
		 */
        data: {
            value: []
        },
        /**
		 * 缓存的数据，包含以下几个字段
		 * <ol>
		 * <li>rows: 数据集合</li>
		 * <li>results: 总的数据条数</li>
		 * </ol>
		 * @type {Object}
		 * @private
		 * @readOnly
		 */
        resultMap: {
            shared: false,
            value: {
                total: 0,
                rows: []
            }
        },
        /**
		 * 加载数据时，返回数据的根目录
		 */
        root: {
            value: "rows"
        },
        /**
		 * 加载数据时，返回记录的总数的字段，用于分页
		 */
        totalProperty: {
            value: "total"
        },
        /**
		 * 删除掉的纪录
		 * @readOnly
		 * @private
		 * @type {Array}
		 */
        deletedRecords: {
            shared: false,
            value: []
        },
        /**
		 * 更改的纪录集合
		 * @type {Array}
		 * @private
		 * @readOnly
		 */
        modifiedRecords: {
            shared: false,
            value: []
        },
        /**
		 * 新添加的纪录集合，只读
		 * @type {Array}
		 * @private
		 * @readOnly
		 */
        newRecords: {
            shared: false,
            value: []
        },
        /**
		 * 页码
		 */
        pageIndex: {
            value: 1
        },
        /**
		 *  每页多少条记录,默认为null,此时不分页，当指定了此值时分页
		 */
        pageSize: {
            value: 20
        },
        /**
		 * 首付自动查询
		 */
        autoLoad: {
            value: true
        },
        /**
		 * 是否ajax
		 */
        isAjax: {
            value: false
        },
        /**
		 * 匹配的字段名
		 * @type {Array}
		 */
        matchFields: {
            value: []
        }
    };
    BUI.extend(store, BUI.Base);
    BUI.augment(store, {
        isStore: true,
        /**
		 * 初始化
		 */
        _init: function () {
            var _self = this,
                autoLoad = _self.get("autoLoad");
            _self._initParams();
            if (autoLoad) {
                _self.load();
            } else {
                _self._setResult([]);
            }
        },
        /**
		 * 初始化参数
		 */
        _initParams: function () {
            var _self = this,
                lastParams = _self.get("lastParams"),
                params = _self.get("params");
            BUI.mix(lastParams, params);
        },
        /**
		 * 根据特定参数进行查询
		 * @param {Object} params
		 * @param {Object} callback
		 */
        load: function (params, callback) {
            var _self = this,
                sortField = _self.get("sortField"),
                sortDirection = _self.get("sortDirection"),
                lastParams = _self.get("lastParams");

            BUI.mix(lastParams, _self.getAppendParams(), params);
            params = BUI.cloneObject(lastParams);
            //如果ajax请求,则自定义查询,否则本地查询
            if (_self.get("isAjax")) {
                _self.onAjax(params, callback);
            } else {
                var data = _self._getMatches(params),
                    rows = [];
                _self.sortData(sortField, sortDirection, data);
                _self.onLoad(data, params);

                if (callback) {
                    if (params.pageSize) {
                        var end = parseInt(params.pageIndex) * parseInt(params.pageSize);
                        rows = data.slice(end - param.pageSize, end);
                        callback({
                            rows: rows,
                            total: data.length
                        });
                    } else {
                        callback(data, params);
                    }
                }
            }
        },
        onAjax: function (params, callback) { },
        onLoad: function (data, params) {
            var _self = this,
                root = _self.get("root"),
                pageSize = params.pageSize;
            if (pageSize) {
                _self.set("pageIndex", params.pageIndex);
            }
            _self._setResult(data);

            _self.fire("load", {
                params: params
            });
        },
        getAppendParams: function () {
            var _self = this,
                pageSize = _self.get("pageSize"),
                pageIndex = _self.get("pageIndex") || 1;
            var params = {
                pageSize: pageSize,
                pageIndex: pageIndex
            };
            return params;
        },
        /**
		 * 添加记录
		 * @param {Object} data
		 * @param {Object} noRepeat
		 * @param {Object} match
		 */
        add: function (data, noRepeat, match) {
            var _self = this,
                count = _self.getCount();
            _self.addAt(data, count, noRepeat, match);
        },
        /**
		 * 添加记录到指定索引
		 * @param {Object} data
		 * @param {Object} index
		 * @param {Object} noRepeat
		 * @param {Object} match
		 */
        addAt: function (data, index, noRepeat, match) {
            var _self = this;
            match = match || _self._getDefaultMatch();
            if (!BUI.isArray(data)) {
                data = [data];
            }
            $.each(data,
                function (pos, element) {
                    if (!noRepeat || !_self.contains(element, match)) {
                        _self._addRecord(element, pos + index);
                        _self.get("newRecords").push(element);
                        removeFrom(element, _self.get("deletedRecords"));
                        removeFrom(element, _self.get("modifiedRecords"));
                    }
                });
        },
        _getDefaultMatch: function () {
            return this.get("matchFunction");
        },
        /**
		 * 判断是否包含指定记录
		 * @param {Object} record
		 * @param {Object} match
		 */
        contains: function (record, match) {
            return this.findIndexBy(record, match) !== -1;
        },
        /**
		 * 查找数据所在的索引位置,若不存在返回-1
		 * <pre><code>
		 *  var index = store.findIndexBy(obj);
		 *
		 *  var index = store.findIndexBy(obj,function(obj1,obj2){
		 *    return obj1.id == obj2.id;
		 *  });
		 * </code></pre>
		 * @param {Object} target 指定的记录
		 * @param {Function} [match = matchFunction] @see {BUI.Data.Store#matchFunction}默认为比较2个对象是否相同
		 * @return {Number}
		 */
        findIndexBy: function (target, match) {
            var _self = this,
                position = -1,
                records = _self.getResult();
            match = match || _self._getDefaultMatch();
            if (target === null || target === undefined) {
                return -1;
            }
            $.each(records,
                function (index, record) {
                    if (match(target, record)) {
                        position = index;
                        return false;
                    }
                });
            return position;
        },
        _addRecord: function (record, index) {
            var records = this.getResult();
            if (index == undefined) {
                index = records.length;
            }
            records.splice(index, 0, record);
            this.fire("add", {
                record: record,
                index: index
            });
        },
        /**
		 * 删除记录
		 * @param {Object} data
		 * @param {Object} match
		 */
        remove: function (data, match) {
            var _self = this,
                delData = [];
            match = match || _self._getDefaultMatch();
            if (!BUI.isArray(data)) {
                data = [data];
            }
            $.each(data,
                function (index, element) {
                    var index = _self.findIndexBy(element, match),
                        record = removeAt(index, _self.getResult());
                    if (!contains(record, _self.get("newRecords")) && !contains(record, _self.get("deletedRecords"))) {
                        _self.get("deletedRecords").push(record);
                    }
                    removeFrom(record, _self.get("newRecords"));
                    removeFrom(record, _self.get("modifiedRecords"));
                    _self.fire("remove", {
                        record: record
                    });
                });
        },
        /**
		 * 更新记录
		 * @param {Object} obj
		 * @param {Object} isMatch
		 * @param {Object} match
		 */
        update: function (obj, isMatch, match) {
            var record = obj,
                _self = this,
                match = null,
                index = null;
            if (isMatch) {
                match = match || _self._getDefaultMatch();
                index = _self.findIndexBy(obj, match);
                if (index >= 0) {
                    record = _self.getResult()[index];
                }
            }
            record = BUI.mix(record, obj);
            if (!contains(record, _self.get("newRecords")) && !contains(record, _self.get("modifiedRecords"))) {
                _self.get("modifiedRecords").push(record);
            }
            _self.fire("update", {
                record: record
            });
        },
        /**
		 * 获取当前缓存的纪录
		 */
        getResult: function () {
            var _self = this,
                resultMap = _self.get("resultMap"),
                root = _self.get("root");
            return BUI.getValue(resultMap, root);
        },
        /**
		 * 查找记录，仅返回第一条
		 * <pre><code>
		 *  var record = store.find('id','123');
		 * </code></pre>
		 * @param {String} field 字段名
		 * @param {String} value 字段值
		 * @return {Object|null}
		 */
        find: function (field, value) {
            var _self = this,
                result = null,
                records = _self.getResult();
            $.each(records,
                function (index, record) {
                    if (record[field] === value) {
                        result = record;
                        return false;
                    }
                });
            return result;
        },
        /**
		 * 查找记录，返回所有符合查询条件的记录
		 * <pre><code>
		 *   var records = store.findAll('type','0');
		 * </code></pre>
		 * @param {String} field 字段名
		 * @param {String} value 字段值
		 * @return {Array}
		 */
        findAll: function (field, value) {
            var _self = this,
                result = [],
                records = _self.getResult();
            $.each(records,
                function (index, record) {
                    if (record[field] === value) {
                        result.push(record);
                    }
                });
            return result;
        },
        /**
		 * 根据索引查找记录
		 * <pre><code>
		 *  var record = store.findByIndex(1);
		 * </code></pre>
		 * @param {Number} index 索引
		 * @return {Object} 查找的记录
		 */
        findByIndex: function (index) {
            return this.getResult()[index];
        },
        /**
		 * 获取下一条记录
		 * <pre><code>
		 *  var record = store.findNextRecord(obj);
		 * </code></pre>
		 * @param {Object} record 当前记录
		 * @return {Object} 下一条记录
		 */
        findNextRecord: function (record) {
            var _self = this,
                index = _self.findIndexBy(record);
            if (index >= 0) {
                return _self.findByIndex(index + 1);
            }
            return;
        },
        /**
		 * 获取缓存的记录数
		 * <pre><code>
		 *  var count = store.getCount(); //缓存的数据数量
		 *
		 *  var totalCount = store.getTotalCount(); //数据的总数，如果有分页时，totalCount != count
		 * </code></pre>
		 * @return {Number} 记录数
		 */
        getCount: function () {
            return this.getResult().length;
        },
        /**
		 * 获取数据源的数据总数，分页时，当前仅缓存当前页数据
		 * <pre><code>
		 *  var count = store.getCount(); //缓存的数据数量
		 *
		 *  var totalCount = store.getTotalCount(); //数据的总数，如果有分页时，totalCount != count
		 * </code></pre>
		 * @return {Number} 记录的总数
		 */
        getTotalCount: function () {
            var _self = this,
                resultMap = _self.get("resultMap"),
                total = _self.get("totalProperty"),
                totalVal = BUI.getValue(resultMap, total);
            return parseInt(totalVal, 10) || 0;
        },
        /**
		 * 是否包含数据
		 * @return {Boolean} 
		 */
        hasData: function () {
            return this.getCount() !== 0;
        },
        /**
		 * 获取未保存的数据
		 */
        getDirtyData: function () {
            var _self = this;
            return {
                add: _self.get("newRecords"),
                update: _self.get("modifiedRecords"),
                remove: _self.get("deletedRecords")
            };
        },
        /**
		 * 按照指定字段和方向排序
		 * @param {Object} field
		 * @param {Object} direction
		 */
        sort: function (field, direction) {
            var _self = this;
            _self._sortData(field, direction);
        },
        setResult: function (data) {
            var _self = this,
                root = _self.get("root"),
                totalProperty = _self.get("totalProperty"),
                lastParams = _self.get("lastParams");
            if (BUI.isArray(data)) {
                _self.set("data", data);
                _self._setResult(data);
            } else {
                _self.set("data", BUI.getValue(data, root));
                _self._setResult(BUI.getValue(data, root), BUI.getValue(data, totalProperty));
            }

            var params = BUI.cloneObject(lastParams),
                pageSize = params.pageSize;

            if (pageSize) {
                _self.set("pageIndex", params.pageIndex);
            }

            _self.fire("load", {
                params: params
            });
        },
        /**
		 * 计算指定字段的和
		 * @param {Object} field
		 * @param {Object} data
		 */
        sum: function (field, data) {
            var _self = this,
                records = data || _self.getResult(),
                sum = 0;
            BUI.each(records,
                function (record) {
                    var val = record[field];
                    if (!isNaN(val)) {
                        sum += parseFloat(val);
                    }
                });
            return sum;
        },
        /**
		 * 设置记录的值 ，触发 update 事件
		 * <pre><code>
		 *  store.setValue(obj,'value','new value');
		 * </code></pre>
		 * @param {Object} obj 修改的记录
		 * @param {String} field 修改的字段名
		 * @param {Object} value 修改的值
		 */
        setValue: function (obj, field, value) {
            var record = obj,
                _self = this;
            record[field] = value;
            if (!contains(record, _self.get("newRecords")) && !contains(record, _self.get("modifiedRecords"))) {
                _self.get("modifiedRecords").push(record);
            }
            _self.fire("update", {
                record: record,
                field: field,
                value: value
            });
        },
        _sortData: function (field, direction, data) {
            var _self = this;
            data = data || _self.getResult();
            _self.sortData(field, direction, data);
        },
        /**
		 * 结果集排序
		 * @param {Object} field
		 * @param {Object} direction
		 * @param {Object} records
		 */
        sortData: function (field, direction, records) {
            var _self = this,
                records = records || _self.getSortData();
            if (BUI.isArray(field)) {
                records = field;
                field = null;
            }
            field = field || _self.get("sortField");
            direction = direction || _self.get("sortDirection");
            _self.set("sortField", field);
            _self.set("sortDirection", direction);
            if (!field || !direction) {
                return records;
            }
            records.sort(function (obj1, obj2) {
                return _self.compare(obj1, obj2, field, direction);
            });
            return records;
        },
        /**
		 * 按照指定字段进行比较
		 * @param {Object} obj1
		 * @param {Object} obj2
		 * @param {Object} field
		 * @param {Object} direction ASC DESC
		 */
        compare: function (obj1, obj2, field, direction) {
            var _self = this,
                dir;
            field = field || _self.get("sortField");
            direction = direction || _self.get("sortDirection");
            if (!field || !direction) {
                return 1;
            }
            dir = direction === ASC ? 1 : -1;
            return _self.get("compareFunction")(obj1[field], obj2[field]) * dir;
        },
        /**
		 * 获取排序数据
		 */
        getSortData: function () {
            return this.getResult();
        },
        /**
		 * 设置结果集
		 * @param {Object} rows
		 * @param {Object} totalCount
		 */
        _setResult: function (rows, totalCount) {
            var _self = this,
                resultMap = _self.get("resultMap");
            totalCount = totalCount || rows.length;
            BUI.setValue(resultMap, _self.get("root"), rows);
            BUI.setValue(resultMap, _self.get("totalProperty"), totalCount);
            _self._clearChanges();
        },
        /**
		 * 清空所有改变
		 */
        _clearChanges: function () {
            var _self = this;
            BUI.Array.empty(_self.get("newRecords"));
            BUI.Array.empty(_self.get("modifiedRecords"));
            BUI.Array.empty(_self.get("deletedRecords"));
        },
        /**
		 * 获取匹配记录
		 * @param {Object} params
		 */
        _getMatches: function (params) {
            var _self = this,
                matchFields = _self.get("matchFields"),
                matchFn,
                data = _self.get("data") || [];
            if (params && matchFields.length) {
                matchFn = _self._getMatchFn(params, matchFields);
                data = BUI.Array.filter(data, matchFn);
            }
            return data;
        },
        /**
		 * 获取匹配函数
		 * @param {Object} params
		 * @param {Object} matchFields
		 */
        _getMatchFn: function (params, matchFields) {
            var _self = this;
            return function (obj) {
                var result = true;
                BUI.each(matchFields,
                    function (field) {
                        if (params[field] != null && !(params[field] === obj[field])) {
                            result = false;
                            return false;
                        }
                    });
                return result;
            };
        }
    });
    BUI.Data.Store = store;
})(window.BUI);
//BUI.Data.Node +
(function (BUI) {
    "use strict";

    function mapNode(cfg, map) {
        var rst = {};
        if (map) {
            BUI.each(cfg,
                function (v, k) {
                    var name = map[k] || k;
                    rst[name] = v;
                });
            rst.record = cfg;
        } else {
            rst = cfg;
        }
        return rst;
    }

    /**
	 * 树形数据结构的节点类
	 * @param {Object} cfg
	 * @param {Object} map
	 */
    function Node(cfg, map) {
        var _self = this;
        cfg = mapNode(cfg, map);
        BUI.mix(this, cfg);
    }
    BUI.augment(Node, {
        /**
		 * 是否根节点
		 * @type {Boolean}
		 */
        root: false,
        /**
		 * 是否叶子节点
		 * @type {Boolean}
		 */
        leaf: null,
        /**
		 * 显示节点时显示的文本
		 * @type {Object}
		 */
        text: '',
        /**
		 * 代表节点的编号
		 * @type {String}
		 */
        id: null,
        /**
		 * 子节点是否已经加载过
		 * @type {Boolean}
		 */
        loaded: false,
        /**
		 * 从根节点到此节点的路径，id的集合如： ['0','1','12'],
		 * 便于快速定位节点
		 * @type {Array}
		 */
        path: null,
        /**
		 * 父节点
		 * @type {BUI.Data.Node}
		 */
        parent: null,
        /**
		 * 树节点的等级
		 * @type {Number}
		 */
        level: 0,
        /**
		 * 节点是否由一条记录封装而成
		 * @type {Object}
		 */
        record: null,
        /**
		 * 子节点集合
		 * @type {BUI.Data.Node[]}
		 */
        children: null,
        /**
		 * 是否是Node对象
		 * @type {Object}
		 */
        isNode: true
    });
    BUI.Data.Node = Node;
})(window.BUI);
//BUI.Data.TreeStore
(function (BUI, $) {
    "use strict";
    var Node = BUI.Data.Node,
        Proxy = BUI.Data.Proxy;

    function TreeStore(config) {
        TreeStore.superclass.constructor.call(this, config);
        this._init();
    }
    TreeStore.ATTRS = {
        /**
        * 用户判断是否ajax异步数据
        **/
        url: {
        },
        /**
		 * 首付自动查询
		 */
        autoLoad: {
            value: false
        },
        /**
		 * 上次查询的参数
		 * @type {Object}
		 * @readOnly
		 */
        lastParams: {
            shared: false,
            value: {}
        },
        /**
		 * 参数
		 */
        params: {},
        /**
		 * 比较函数
		 * @cfg {Function} compareFunction
		 * 函数原型 function(v1,v2)，比较2个字段是否相等
		 * 如果是字符串则按照本地比较算法，否则使用 > ,== 验证
		 */
        matchFunction: {
            value: function (obj1, obj2) {
                return obj1 == obj2;
            }
        },
        /**
		 * 本地数据源
		 * @type {Array}
		 */
        data: {
            value: []
        },
        /**
		 *  根节点,初始化后不要更改对象，可以更改属性值
		 */
        root: {},
        /**
		 * 数据映射，用于设置的数据跟@see {BUI.Data.Node} 不一致时，进行匹配。
		 */
        map: {},
        /**
		 * 标示父元素id的字段名称
		 * @type {String}
		 */
        pidField: {},
        /**
		 * 返回数据标示数据的字段<br/>
		 * 异步加载数据时，返回数据可以使数组或者对象
		 * - 如果返回的是对象,可以附加其他信息,那么取对象对应的字段 {nodes : [],hasError:false}
		 * - 如何获取附加信息参看 @see {BUI.Data.AbstractStore-event-beforeprocessload}
		 */
        dataProperty: {
            value: "nodes"
        },
        events: {
            value: ["load", "add", "remove", "update"]
        },
        matchFields: {
            value: []
        }
    };
    BUI.extend(TreeStore, BUI.Base);
    BUI.augment(TreeStore, {
        isStore: true,
        _init: function () {
            var _self = this;
            _self.initRoot();
            _self._initParams();
            _self._initData();
        },
        /**
		 * 初始化根节点
		 */
        initRoot: function () {
            var _self = this,
                map = _self.get("map"),
                root = _self.get("root");
            if (!root) {
                root = {};
            }
            if (!root.isNode) {
                root = new Node(root, map);
            }
            root.path = [root.id];
            root.level = 0;
            if (root.children) {
                _self.setChildren(root, root.children);
            }
            _self.set("root", root);
        },
        /**
		 * 设置子节点
		 * @param {Object} node
		 * @param {Object} children
		 */
        setChildren: function (node, children) {
            var _self = this;
            node.children = [];
            if (!children.length) {
                return;
            }
            BUI.each(children,
                function (item) {
                    _self._add(item, node);
                });
        },
        /**
		 * 添加节点
		 * @param {Object} node 
		 * @param {Object} parent 父节点,如果未指定则为根节点
		 * @param {Object} index
		 */
        add: function (node, parent, index) {
            var _self = this;
            node = _self._add(node, parent, index);
            _self.fire("add", {
                node: node,
                record: node,
                index: index
            });
            return node;
        },
        _add: function (node, parent, index) {
            parent = parent || this.get("root");
            var _self = this,
                map = _self.get("map"),
                nodes = parent.children,
                nodeChildren;
            if (!node.isNode) {
                node = new Node(node, map);
            }
            nodeChildren = node.children || [];
            if (nodeChildren.length == 0 && node.leaf == null) {
                node.leaf = true;
            }
            if (parent) {
                parent.leaf = false;
            }
            node.parent = parent;
            node.level = parent.level + 1;
            node.path = parent.path.concat(node.id);
            index = index == null ? parent.children.length : index;
            BUI.Array.addAt(nodes, node, index);
            _self.setChildren(node, nodeChildren);
            return node;
        },
        _initParams: function () {
            var _self = this,
                lastParams = _self.get("lastParams"),
                params = _self.get("params");
            BUI.mix(lastParams, params);
        },
        _initData: function () {
            var _self = this,
                pidField = _self.get("pidField"),
                root = _self.get("root");
            if (!root.children) {
                _self.loadNode(root);
            }
        },
        /**
		 * 加载节点的子节点
		 * @param  {BUI.Data.Node} node 节点
		 * @param {Boolean} forceLoad 是否强迫重新加载节点，如果设置成true，不判断是否加载过
		 */
        loadNode: function (node, forceLoad) {
            var _self = this,
                pidField = _self.get("pidField"),
                params;
            if (!forceLoad && _self.isLoaded(node)) {
                return;
            }
            params = {
                id: node.id
            };
            if (pidField) {
                params[pidField] = node.id;
            }
            _self.load(params);
        },
        /**
		 * 根据特定参数进行查询
		 * @param {Object} params
		 * @param {Object} callback
		 */
        load: function (params, callback) {
            var _self = this,
                lastParams = _self.get("lastParams"),
                url = _self.get("url");
            BUI.mix(lastParams, params);
            if (url) {
                $.ajax({
                    cache: false,
                    url: url,
                    data: params,
                    type: "GET",
                    datatype: "json",
                    success: function (data) {
                        _self.onLoad(data, params);
                        if (callback) {
                            callback(data, params);
                        }
                    },
                    error: function (err) {
                        BUI.log(err.responseText);
                    }
                });
            } else {
                var data = _self._getMatches(params);
                _self.onLoad(data, params);

                if (callback) {
                    callback(data, params);
                }
            }
        },
        onLoad: function (data, params) {
            var _self = this,
                pidField = _self.get("pidField"),
                id = params.id || params[pidField],
                dataProperty = _self.get("dataProperty"),
                node = _self.findNode(id) || _self.get("root");

            if (BUI.isArray(data)) {
                _self.setChildren(node, data);
            } else {
                _self.setChildren(node, BUI.getValue(data, dataProperty));
            }
            node.loaded = true;
            _self.fire("load", {
                node: node,
                params: params
            });
        },
        /**
		 * 删除记录
		 * @param {Object} data
		 * @param {Object} match
		 */
        remove: function (node) {
            var parent = node.parent || _self.get("root"),
                index = BUI.Array.indexOf(node, parent.children);
            BUI.Array.remove(parent.children, node);
            if (parent.children.length === 0) {
                parent.leaf = true;
            }
            this.fire("remove", {
                node: node,
                record: node,
                index: index
            });
            node.parent = null;
            return node;
        },
        /**
		 * 更新记录
		 * @param {Object} node
		 * @param {Object} field
		 * @param {Object} value
		 */
        setValue: function (node, field, value) {
            var _self = this;
            node[field] = value;
            _self.fire("update", {
                node: node,
                record: node,
                field: field,
                value: value
            });
        },
        /**
		 * 更新记录
		 * @param {Object} node
		 */
        update: function (node) {
            this.fire("update", {
                node: node,
                record: node
            });
        },
        /**
		 * 获取当前缓存的纪录
		 */
        getResult: function () {
            return this.get("root").children;
        },
        /**
		 * 设置结果
		 * @param {Object} data
		 */
        setResult: function (data) {
            var _self = this,
                root = _self.get("root");
            _self.set("data", data);
            _self.load({
                id: root.id
            });
        },
        /**
		 * 查找节点
		 * <pre><code>
		 *  var node = store.findNode('1');//从根节点开始查找节点
		 *  
		 *  var subNode = store.findNode('123',node); //从指定节点开始查找
		 * </code></pre>
		 * @param  {String} id 节点Id
		 * @param  {BUI.Data.Node} [parent] 父节点
		 * @param {Boolean} [deep = true] 是否递归查找
		 * @return {BUI.Data.Node} 节点
		 */
        findNode: function (id, parent, deep) {
            return this.findNodeBy(function (node) {
                return node.id === id;
            },
                parent, deep);
        },
        /**
		 * 根据匹配函数查找节点
		 * @param  {Function} fn  匹配函数
		 * @param  {BUI.Data.Node} [parent] 父节点
		 * @param {Boolean} [deep = true] 是否递归查找
		 * @return {BUI.Data.Node} 节点
		 */
        findNodeBy: function (fn, parent, deep) {
            var _self = this;
            deep = deep == null ? true : deep;
            if (!parent) {
                var root = _self.get("root");
                if (fn(root)) {
                    return root;
                }
                return _self.findNodeBy(fn, root);
            }
            var children = parent.children,
                rst = null;
            BUI.each(children,
                function (item) {
                    if (fn(item)) {
                        rst = item;
                    } else if (deep) {
                        rst = _self.findNodeBy(fn, item);
                    }
                    if (rst) {
                        return false;
                    }
                });
            return rst;
        },
        /**
		 * 查找节点,根据匹配函数查找
		 * <pre><code>
		 *  var nodes = store.findNodesBy(function(node){
		 *   if(node.status == '0'){
		 *     return true;
		 *   }
		 *   return false;
		 *  });
		 * </code></pre>
		 * @param  {Function} func 匹配函数
		 * @param  {BUI.Data.Node} [parent] 父元素，如果不存在，则从根节点查找
		 * @return {Array} 节点数组
		 */
        findNodesBy: function (func, parent) {
            var _self = this,
                root, rst = [];
            if (!parent) {
                parent = _self.get("root");
            }
            BUI.each(parent.children,
                function (item) {
                    if (func(item)) {
                        rst.push(item);
                    }
                    rst = rst.concat(_self.findNodesBy(func, item));
                });
            return rst;
        },
        /**
		 * 根据path查找节点
		 * @return {BUI.Data.Node} 节点
		 * @ignore
		 */
        findNodeByPath: function (path) {
            if (!path) {
                return null;
            }
            var _self = this,
                root = _self.get("root"),
                pathArr = path.split(","),
                node,
                i,
                tempId = pathArr[0];
            if (!tempId) {
                return null;
            }
            if (root.id == tempId) {
                node = root;
            } else {
                node = _self.findNode(tempId, root, false);
            }
            if (!node) {
                return;
            }
            for (i = 1; i < pathArr.length; i = i + 1) {
                var tempId = pathArr[i];
                node = _self.findNode(tempId, node, false);
                if (!node) {
                    break;
                }
            }
            return node;
        },
        /**
		 * 是否包含指定节点，如果未指定父节点，从根节点开始搜索
		 * @param {Object} node
		 * @param {Object} parent
		 */
        contains: function (node, parent) {
            var _self = this,
                findNode = _self.findNode(node.id, parent);
            return !!findNode;
        },
        /**
		 * 是否包含数据
		 */
        hasData: function () {
            return this.get("root").children && this.get("root").children.length !== 0;
        },
        /**
		 * 是否已经加载过，叶子节点或者存在字节点的节点
		 * @param {Object} node
		 */
        isLoaded: function (node) {
            var root = this.get("root");
            if (node == root && !root.children) {
                return false;
            }
            if (!this.get("pidField")) {
                return true;
            }
            return node.loaded || node.leaf || !!(node.children && node.children.length);
        },
        /**
		 * 重新加载节点
		 * @param {Object} node
		 */
        reloadNode: function (node) {
            var _self = this;
            node = node || _self.get("root");
            node.loaded = false;
            _self.loadNode(node, true);
        },
        /**
		 * 加载节点，根据path
		 * @param {Object} path
		 */
        loadPath: function (path) {
            var _self = this,
                arr = path.split(","),
                id = arr[0];
            if (_self.findNodeByPath(path)) {
                return;
            }
            _self.load({
                id: id,
                path: path
            });
        },
        /**
		 * 获取匹配记录
		 * @param {Object} params
		 */
        _getMatches: function (params) {
            var _self = this,
                matchFields = _self.get("matchFields"),
                matchFn,
                data = _self.get("data") || [];
            if (params && matchFields.length) {
                matchFn = _self._getMatchFn(params, matchFields);
                data = BUI.Array.filter(data, matchFn);
            }
            return data;
        },
        /**
		 * 获取匹配参数
		 * @param {Object} params
		 * @param {Object} matchFields
		 */
        _getMatchFn: function (params, matchFields) {
            var _self = this;
            return function (obj) {
                var result = true;
                BUI.each(matchFields,
                    function (field) {
                        if (params[field] != null && !(params[field] === obj[field])) {
                            result = false;
                            return false;
                        }
                    });
                return result;
            };
        }
    });
    BUI.Data.TreeStore = TreeStore;
})(window.BUI, jQuery);;
///<jscompress sourcefile="04mask.js" />
//BUI.Mask +
(function (BUI, $) {
    "use strict";
    var Mask = BUI.Mask = {},
        CLS_MASK = "bui-mask",
        CLS_MASK_MSG = "bui-mask-msg",
        CLS_MASK_LOADING = "bui-mask-loading";
    BUI.mix(Mask, {
        /**
        * 屏蔽指定元素
        */
        maskElement: function (element) {
            var maskedEl = $(element),
                maskDiv = maskedEl.children("." + CLS_MASK),
                tpl = null,
                msgDiv = null,
                top = null,
                left = null;
            if (!maskDiv.length) {
                maskDiv = $('<div class="' + CLS_MASK + '"></div>').appendTo(maskedEl);
                maskedEl.addClass("bui-masked");
                if (element == "body") {
                    maskDiv.css("position", "fixed");
                }
                tpl = '<div class="' + CLS_MASK_MSG + ' ' + CLS_MASK_LOADING + '"><div></div></div>';
                msgDiv = $(tpl).appendTo(maskedEl);
                try {
                    top = (maskDiv.height() - msgDiv.height()) / 2;
                    left = (maskDiv.width() - msgDiv.width()) / 2;
                    msgDiv.css({
                        left: left,
                        top: top
                    });
                } catch (ex) {
                    BUI.log("mask error occurred");
                }
            }
            return maskDiv;
        },
        /**
        *解除元素的屏蔽
        */
        unmaskElement: function (element) {
            var maskedEl = $(element),
                msgEl = maskedEl.children("." + CLS_MASK_MSG),
                maskDiv = maskedEl.children("." + CLS_MASK);
            if (msgEl) {
                msgEl.remove();
            }
            if (maskDiv) {
                maskDiv.remove();
            }
            maskedEl.removeClass("bui-masked");
        }
    });
})(window.BUI, jQuery);;
///<jscompress sourcefile="05list.js" />
//BUI.List.ChildList +
(function (BUI, $) {
    "use strict";
    BUI.List = {};
    var Component = BUI.Component,
        FIELD_PREFIX = "data-";

    /**
	 * 清空选择
	 * @param {Object} child
	 */
    function clearSelected(child) {
        if (child.selected) {
            child.selected = false;
        }
        if (child.set) {
            child.set("selected", false);
        }
    }

    /**
	 * 设置child默认配置
	 * @param {Object} self
	 * @param {Object} child
	 */
    function beforeAddChild(self, child) {
        var c = child.isController ? child.getAttrVals() : child,
            defaultTpl = self.get("childTpl"),
            defaultStatusCls = self.get("childStatusCls"),
            defaultTplRender = self.get("childTplRender");
        if (defaultTpl && !c.tpl) {
            setChildAttr(child, "tpl", defaultTpl);
        }
        if (defaultTplRender && !c.tplRender) {
            setChildAttr(child, "tplRender", defaultTplRender);
        }
        if (defaultStatusCls) {
            var statusCls = c.statusCls || child.isController ? child.get("statusCls") : {};
            BUI.each(defaultStatusCls,
                function (v, k) {
                    if (v && !statusCls[k]) {
                        statusCls[k] = v;
                    }
                });
            setChildAttr(child, "statusCls", statusCls);
        }
    }

    /**
	 * 设置属性值
	 * @param {Object} child
	 * @param {Object} name
	 * @param {Object} val
	 */
    function setChildAttr(child, name, val) {
        if (child.isController) {
            child.set(name, val);
        } else {
            child[name] = val;
        }
    }

    var childList = Component.Controller.extend({
        /**
		 *初始化列表
		 */
        initializer: function () {
            var _self = this;
            _self.on("beforeRenderUI",
                function () {
                    _self._beforeRenderUI();
                });
        },
        _beforeRenderUI: function () {
            var _self = this,
                children = _self.get("children");
            BUI.each(children,
                function (child) {
                    beforeAddChild(_self, child);
                });
        },
        /**
		 * 绑定事件
		 */
        bindUI: function () {
            var _self = this,
                selectedEvent = _self.get("selectedEvent");
            _self.on(selectedEvent,
                function (e) {
                    var child = e.target;
                    if (child.get("selectable")) {
                        if (!child.get("selected")) {
                            _self.setSelected(child);
                        } else if (_self.get("multipleSelect")) {
                            _self.clearSelected(child);
                        }
                    }
                });
            _self.on("click",
                function (e) {
                    if (e.target !== _self) {
                        _self.fire("childclick", {
                            child: e.target,
                            domTarget: e.domTarget,
                            domEvent: e
                        });
                    }
                });
            _self.on("beforeAddChild",
                function (ev) {
                    beforeAddChild(_self, ev.child);
                });
            _self.on("beforeRemoveChild",
                function (ev) {
                    var child = ev.child,
                        selected = child.get("selected");
                    if (selected) {
                        if (_self.get("multipleSelect")) {
                            _self.clearSelected(child);
                        } else {
                            _self.setSelected(null);
                        }
                    }
                    child.set("selected", false);
                });
        },
        /**
		 * 添加控件的子控件集合
		 * @param {Object} children
		 */
        addChildren: function (children) {
            var _self = this;
            BUI.each(children,
                function (child) {
                    _self.addChild(child);
                });
        },
        /**
		 * 删除控件的子控件集合
		 * @param {Object} children
		 */
        removeChildren: function (children) {
            var _self = this;
            BUI.each(children,
                function (child) {
                    var idField = _self.get("idField");
                    if (!(child instanceof BUI.Component.Controller)) {
                        child = _self.findChildByField(idField, child[idField]);
                    }
                    _self.removeChild(child, true);
                });
        },
        /**
		 *删除指定索引的子控件 
		 * @param {Object} index
		 */
        removeChildAt: function (index) {
            this.removeChild(this.getChildAt(index), true);
        },
        /**
		 *获取子控件 
		 */
        getChildren: function () {
            return this.get("children");
        },
        /**
		 *获取子控件数量 
		 */
        getChildCount: function () {
            return this.get("children").length;
        },
        /**
		 * 获取第一个子控件
		 */
        getFirstChild: function () {
            return this.getChildAt(0);
        },
        /**
		 * 获取最后一个子控件
		 */
        getLastChild: function () {
            return this.getChildAt(this.getChildCount() - 1);
        },
        /**
		 * 获取指定索引的子控件
		 * @param {Object} index
		 */
        getChildAt: function (index) {
            return this.getChildren()[index] || null;
        },
        /**
		 *通过ID获取子控件 
		 * @param {Object} id
		 */
        getChild: function (id) {
            var field = this.get("idField");
            return this.findChildByField(field, id);
        },
        /**
		 * 获取选中的项的子控件 
		 */
        getSelection: function () {
            var _self = this,
                children = _self.getChildren(),
                rst = [];
            BUI.each(children,
                function (child) {
                    if (_self.isChildSelected(child)) {
                        rst.push(child);
                    }
                });
            return rst;
        },
        /**
		 * 获取选中的第一项
		 */
        getSelected: function () {
            return this.getSelection()[0];
        },
        /**
		 * 获取选中的值集合
		 */
        getSelectionValues: function () {
            var _self = this,
                field = _self.get("idField"),
                children = _self.getSelection();
            return $.map(children,
                function (child) {
                    return _self.getValueByField(child, field);
                });
        },
        /**
		 * 获取选中的第一项的值
		 */
        getSelectedValue: function () {
            var _self = this,
                field = _self.get("idField"),
                child = _self.getSelected();
            return _self.getValueByField(child, field);
        },
        /**
		 * 获取选中的文本集合
		 */
        getSelectionText: function () {
            var _self = this,
                itechildrenms = _self.getSelection();
            return $.map(children,
                function (child) {
                    return _self.getChildText(child);
                });
        },
        /**
		 * 获取选中的第一项的文本
		 */
        getSelectedText: function () {
            var _self = this,
                child = _self.getSelected();
            return _self.getChildText(child);
        },
        /**
		 * 获取特定字段的值
		 * @param {Object} child
		 * @param {Object} field
		 */
        getValueByField: function (child, field) {
            return child && child.get(field);
        },
        /**
		 * 获取特定字段的文本
		 * @param {Object} child
		 */
        getChildText: function (child) {
            return child.get("el").text();
        },
        /**
		 * 清空选择
		 */
        clearSelection: function () {
            var _self = this,
                selection = _self.getSelection();
            BUI.each(selection,
                function (child) {
                    _self.clearSelected(child);
                });
        },
        /**
		 * 取消选中
		 * @param {Object} child
		 */
        clearSelected: function (child) {
            var _self = this;
            child = child || _self.getSelected();
            if (child) {
                _self.setChildSelected(child, false);
            }
        },
        clearControl: function () {
            this.clearChildren(true);
        },
        /**
		 * 选中特定子控件
		 * @param {Object} children
		 */
        setSelection: function (children) {
            var _self = this;
            children = BUI.isArray(children) ? children : [children];
            BUI.each(children,
                function (child) {
                    _self.setSelected(child);
                });
        },
        /**
		 * 选择特定子控件
		 * @param {Object} child
		 */
        setSelected: function (child) {
            var _self = this,
                multipleSelect = _self.get("multipleSelect");
            if (!_self.isChildselectable(child)) {
                return;
            }
            if (!multipleSelect) {
                var selectedChild = _self.getSelected();
                if (child != selectedChild) {
                    _self.clearSelected(selectedChild);
                }
            }
            _self.setChildSelected(child, true);
        },
        setChildSelected: function (child, selected) {
            var _self = this,
                isSelected;
            if (child) {
                isSelected = _self.isChildSelected(child);
                if (isSelected == selected) {
                    return;
                }
            }
            if (_self.fire("beforeselectedchange", {
                child: child,
                selected: selected
            }) !== false) {
                _self.setChildSelectedStatus(child, selected);
            }
        },
        setSelectedByField: function (field, value) {
            if (!value) {
                value = field;
                field = this.get("idField");
            }
            var _self = this,
                child = _self.findChildByField(field, value);
            _self.setSelected(child);
        },
        setSelectionByField: function (field, values) {
            if (!values) {
                values = field;
                field = this.get("idField");
            }
            var _self = this;
            BUI.each(values,
                function (value) {
                    _self.setSelectedByField(field, value);
                });
        },
        /**
		 * 设置选中状态
		 * @param {Object} child
		 * @param {Object} selected
		 */
        setChildSelectedStatus: function (child, selected) {
            var _self = this,
                chd = null;
            if (child) {
                child.set("selected", selected);
                chd = child.get("el");
            }
            _self.afterSelected(child, selected, chd);
        },
        /**
		 * 全选
		 */
        setAllSelection: function () {
            var _self = this,
                children = _self.getChildren();
            _self.setSelection(children);
        },
        /**
		 * 更新子控件
		 * @param {Object} child
		 */
        updateChild: function (child) {
            var _self = this,
                idField = _self.get("idField"),
                chd = _self.findChildByField(idField, child[idField]);
            if (chd) {
                chd.setTplContent();
            }
            return chd;
        },
        indexOfChild: function (child) {
            return BUI.Array.indexOf(child, this.getChildren());
        },
        /**
		 * 子控件是否可以选择
		 * @param {Object} child
		 */
        isChildselectable: function (child) {
            return true;
        },
        /**
		 * 子控件是否被选中
		 * @param {Object} child
		 */
        isChildSelected: function (child) {
            return child ? child.get("selected") : false;
        },
        /**
		 * 获取特定字段的child控件
		 * @param {Object} field
		 * @param {Object} value
		 * @param {Object} root
		 */
        findChildByField: function (field, value, root) {
            root = root || this;
            var _self = this,
                children = root.get("children"),
                result = null;
            BUI.each(children,
                function (child) {
                    if (child.get(field) == value) {
                        result = child;
                    } else if (child.get("children").length) {
                        result = _self.findChildByField(field, value, child);
                    }
                    if (result) {
                        return false;
                    }
                });
            return result;
        },
        afterSelected: function (child, selected, element) {
            var _self = this;
            if (selected) {
                _self.fire("childselected", {
                    child: child,
                    domTarget: element
                });
                _self.fire("selectedchange", {
                    child: child,
                    domTarget: element,
                    selected: selected
                });
            } else {
                _self.fire("childunselected", {
                    child: child,
                    domTarget: element
                });
                if (_self.get("multipleSelect")) {
                    _self.fire("selectedchange", {
                        child: child,
                        domTarget: element,
                        selected: selected
                    });
                }
            }
        }
    }, {
            ATTRS: {
                /**
                 * 主键字段
                 */
                idField: {
                    value: "id"
                },
                /**
                 * 子控件模版
                 */
                childTpl: {},
                /**
                 * 子控件渲染方法
                 */
                childTplRender: {},
                childStatusCls: {
                    value: {}
                },
                /**
                 * 是否多选
                 */
                multipleSelect: {
                    value: false
                },
                /**
                 * 选择事件
                 */
                selectedEvent: {
                    value: "click"
                },
                events: {
                    value: {
                        childclick: true,
                        selectedchange: false,
                        beforeselectedchange: false,
                        childselected: false,
                        childunselected: false
                    }
                }
            }
        }, {
            xclass: "childlist"
        });
    BUI.List.ChildList = childList;
})(window.BUI, jQuery);
//BUI.List.ListItem +
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component;
    var child = Component.Controller.extend({
        _uiSetSelected: function (v) {
            var _self = this,
                cls = _self.getStatusCls("selected"),
                el = _self.get("el");
            if (v) {
                el.addClass(cls);
            } else {
                el.removeClass(cls);
            }
        }
    }, {
            ATTRS: {
                selectable: {
                    value: true
                },
                selected: {
                    sync: false,
                    value: false
                },
                elTagName: {
                    value: "li"
                },
                tpl: {
                    value: "<span>{text}</span>"
                }
            }
        }, {
            xclass: "list-item"
        });
    BUI.List.ListChild = child;
})(window.BUI, jQuery);
//BUI.List.List +
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        ChildList = BUI.List.ChildList;
    var list = ChildList.extend({}, {
        ATTRS: {
            elTagName: {
                value: "ul"
            },
            idField: {
                value: "id"
            },
            defaultChildClass: {
                value: "list-item"
            }
        }
    }, {
            xclass: "list"
        });
    BUI.List.List = list;
})(window.BUI, jQuery);
//BUI.List.DomList
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        FIELD_PREFIX = "data-";

    function getItemStatusCls(name, self) {
        var _self = self,
            itemCls = _self.get("itemCls"),
            itemStatusCls = _self.get("itemStatusCls");
        if (itemStatusCls && itemStatusCls[name]) {
            return itemStatusCls[name];
        }
        return itemCls + "-" + name;
    }

    function parseItem(elem, self) {
        var attrs = elem.attributes,
            itemStatusFields = self.get("itemStatusFields"),
            item = {};
        BUI.each(attrs,
            function (attr) {
                var name = attr.nodeName;
                if (name.indexOf(FIELD_PREFIX) !== -1) {
                    name = name.replace(FIELD_PREFIX, "");
                    item[name] = attr.nodeValue;
                }
            });
        item.text = $(elem).text();
        BUI.each(itemStatusFields,
            function (v, k) {
                var cls = getItemStatusCls(k, self);
                if ($(elem).hasClass(cls)) {
                    item[v] = true;
                }
            });
        return item;
    }

    var domList = Component.Controller.extend({
        bindUI: function () {
            var _self = this,
                selectedEvent = _self.get("selectedEvent"),
                itemCls = _self.get("itemCls"),
                itemContainer = _self.getItemContainer();
            itemContainer.delegate("." + itemCls, "click",
                function (ev) {
                    if (_self.get("disabled")) {
                        return;
                    }
                    var itemEl = $(ev.currentTarget),
                        item = _self.getItemByElement(itemEl);
                    if (_self.isItemDisabled(item, itemEl)) {
                        return;
                    }
                    var rst = _self.fire("itemclick", {
                        item: item,
                        element: itemEl[0],
                        domTarget: ev.target,
                        domEvent: ev
                    });
                    if (rst !== false && selectedEvent == "click" && _self.isItemSelectable(item)) {
                        setItemSelectedStatus(item, itemEl);
                    }
                });
            if (selectedEvent !== "click") {
                itemContainer.delegate("." + itemCls, selectedEvent,
                    function (ev) {
                        if (_self.get("disabled")) {
                            return;
                        }
                        var itemEl = $(ev.currentTarget),
                            item = _self.getItemByElement(itemEl);
                        if (_self.isItemDisabled(item, itemEl)) {
                            return;
                        }
                        if (_self.isItemSelectable(item)) {
                            setItemSelectedStatus(item, itemEl);
                        }
                        return false;
                    });
            }
            itemContainer.delegate("." + itemCls, "dblclick",
                function (ev) {
                    if (_self.get("disabled")) {
                        return;
                    }
                    var itemEl = $(ev.currentTarget),
                        item = _self.getItemByElement(itemEl);
                    if (_self.isItemDisabled(item, itemEl)) {
                        return;
                    }
                    _self.fire("itemdblclick", {
                        item: item,
                        element: itemEl[0],
                        domTarget: ev.target
                    });
                });

            function setItemSelectedStatus(item, itemEl) {
                var multipleSelect = _self.get("multipleSelect"),
                    isSelected;
                isSelected = _self.isItemSelected(item, itemEl);
                if (!isSelected) {
                    if (!multipleSelect) {
                        _self.clearSelected();
                    }
                    _self.setItemSelected(item, true, itemEl);
                } else if (multipleSelect) {
                    _self.setItemSelected(item, false, itemEl);
                } else if (_self.get("cancelSelected")) {
                    _self.setSelected(null);
                }
            }
            _self.on("itemrendered itemupdated",
                function (ev) {
                    var item = ev.item,
                        element = ev.element;
                    _self._syncItemStatus(item, element);
                });
        },
        _uiSetItems: function (items) {
            var _self = this;
            if (_self.get("srcNode") && !_self.get("rendered")) {
                return;
            }
            this.setItems(items);
        },
        _syncItemStatus: function (item, element) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields");
            BUI.each(itemStatusFields,
                function (v, k) {
                    if (item[v] != null) {
                        var cls = _self.getItemStatusCls(k),
                            method = item[v] ? "addClass" : "removeClass";
                        if (element) {
                            $(element)[method](cls);
                        }
                    }
                });
        },
        /**
		 * 设置列表记录
		 * @param {Object} items
		 */
        setItems: function (items) {
            var _self = this;
            if (items != _self.getItems()) {
                _self.setInternal("items", items);
            }
            _self.clearControl();
            _self.fire("beforeitemsshow");
            BUI.each(items,
                function (item, index) {
                    _self.addItemToView(item, index);
                });
            _self.fire("itemsshow");
        },
        /**
		 * 添加列表
		 * @param {Object} items
		 */
        addItems: function (items) {
            var _self = this;
            BUI.each(items,
                function (item) {
                    _self.addItem(item);
                });
        },
        /**
		 * 添加单个列表
		 * @param {Object} item
		 */
        addItem: function (item) {
            return this.addItemAt(item, this.getCount());
        },
        /**
		 * 在指定位置添加选项,选项值为一个对象
		 * @param {Object} item
		 * @param {Object} index
		 */
        addItemAt: function (item, index) {
            var _self = this,
                items = _self.get("items");
            if (index === undefined) {
                index = items.length;
            }
            items.splice(index, 0, item);
            _self.addItemToView(item, index);
            return item;
        },
        /**
		 * 添加列表到html dom
		 * @param {Object} item
		 * @param {Object} index
		 */
        addItemToView: function (item, index) {
            var _self = this,
                listEl = _self.getItemContainer(),
                itemCls = _self.get("itemCls"),
                dataField = _self.get("dataField"),
                tpl = _self.getItemTpl(item, index),
                elem = $(tpl);
            if (index !== undefined) {
                var target = listEl.find("." + itemCls)[index];
                if (target) {
                    elem.insertBefore(target);
                } else {
                    elem.appendTo(listEl);
                }
            } else {
                elem.appendTo(listEl);
            }
            elem.addClass(itemCls);
            elem.data(dataField, item);
            _self.fire("itemrendered", {
                item: item,
                domTarget: $(elem)[0],
                element: elem
            });
            return elem;
        },
        /**
		 * 删除列表项
		 * @param {Object} items
		 */
        removeItems: function (items) {
            var _self = this;
            BUI.each(items,
                function (item) {
                    _self.removeItem(item);
                });
        },
        /**
		 * 删除列表
		 * @param {Object} item
		 */
        removeItem: function (item) {
            var _self = this,
                items = _self.get("items"),
                element = _self.findElement(item),
                index;
            index = BUI.Array.indexOf(item, items);
            if (index !== -1) {
                items.splice(index, 1);
            }
            $(element).remove();
            _self.fire("itemremoved", {
                item: item,
                domTarget: $(element)[0],
                element: element
            });
        },
        /**
		 * 删除制定位置列表
		 * @param {Object} index
		 */
        removeItemAt: function (index) {
            this.removeItem(this.getItemAt(index));
        },
        /**
		 * 获取所有列表
		 */
        getItems: function () {
            return this.get("items");
        },
        /**
		 * 获取列表总数
		 */
        getCount: function () {
            var items = this.getItems();
            return items ? items.length : 0;
        },
        /**
		 * 获取值，通过字段
		 * @param {Object} item
		 * @param {Object} field
		 */
        getValueByField: function (item, field) {
            return item && item[field];
        },
        /**
		 * 获取记录中的状态值，未定义则为undefined
		 * @param {Object} item
		 * @param {Object} status
		 */
        getStatusValue: function (item, status) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields"),
                field = itemStatusFields[status];
            return item[field];
        },
        /**
     * 更改状态值对应的字段
     * @protected
     * @param  {String} status 状态名
     * @return {String} 状态对应的字段
     */
        getStatusField: function (status) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields");
            return itemStatusFields[status];
        },
        /**
     * 获取DOM结构中的数据
     * @protected
     * @param {HTMLElement} element DOM 结构
     * @return {Object} 该项对应的值
     */
        getItemByElement: function (element) {
            var _self = this,
                dataField = _self.get("dataField");
            return $(element).data(dataField);
        },
        /**
		 * 获取选择列表项
		 */
        getSelected: function () {
            var _self = this,
                element = _self.getFirstElementByStatus("selected");
            return _self.getItemByElement(element) || null;
        },
        /**
		 * 获取特定状态列表项
		 * @param {Object} status
		 */
        getItemsByStatus: function (status) {
            var _self = this,
                elements = _self.getElementsByStatus(status),
                rst = [];
            BUI.each(elements,
                function (element) {
                    rst.push(_self.getItemByElement(element));
                });
            return rst;
        },
        /**
		 * 获取特定项目文本
		 * @param {Object} item
		 */
        getItemText: function (item) {
            var _self = this,
                textGetter = _self.get("textGetter");
            if (!item) {
                return "";
            }
            if (textGetter) {
                return textGetter(item);
            } else {
                return $(_self.findElement(item)).text();
            }
        },
        /**
		 * 获取第一个列表
		 */
        getFirstItem: function () {
            return this.getItemAt(0);
        },
        /**
		 * 获取最后一个列表
		 */
        getLastItem: function () {
            return this.getItemAt(this.getCount() - 1);
        },
        /**
		 * 获取特定索引项目
		 * @param {Object} index
		 */
        getItemAt: function (index) {
            return this.getItems()[index] || null;
        },
        /**
		 * 通过ID获取项目
		 * @param {Object} id
		 */
        getItem: function (id) {
            var field = this.get("idField");
            return this.findItemByField(field, id);
        },
        /**
		 * 获取选中项
		 */
        getSelection: function () {
            var _self = this,
                elements = _self.getSelectedElements(),
                rst = [];
            BUI.each(elements,
                function (elem) {
                    rst.push(_self.getItemByElement(elem));
                });
            return rst;
        },
        /**
		 * 获取选中项值
		 */
        getSelectedValue: function () {
            var _self = this,
                field = _self.get("idField"),
                item = _self.getSelected();
            return _self.getValueByField(item, field);
        },
        /**
		 * 获取选中项值
		 */
        getSelectionValues: function () {
            var _self = this,
                field = _self.get("idField"),
                items = _self.getSelection();
            return $.map(items,
                function (item) {
                    return _self.getValueByField(item, field);
                });
        },
        /**
		 * 获取选中项文本
		 */
        getSelectionText: function () {
            var _self = this,
                items = _self.getSelection();
            return $.map(items,
                function (item) {
                    return _self.getItemText(item);
                });
        },
        /**
		 * 获取选中项文本
		 */
        getSelectedText: function () {
            var _self = this,
                item = _self.getSelected();
            return _self.getItemText(item);
        },
        /**
		 * 清空状态
		 * @param {Object} item
		 * @param {Object} status
		 * @param {Object} element
		 */
        clearItemStatus: function (item, status, element) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields");
            element = element || _self.findElement(item);
            if (status) {
                _self.setItemStatus(item, status, false, element);
            } else {
                BUI.each(itemStatusFields,
                    function (v, k) {
                        _self.setItemStatus(item, k, false, element);
                    });
                if (!itemStatusFields["selected"]) {
                    _self.setItemSelected(item, false);
                }
                _self.setItemStatus(item, "hover", false);
            }
        },
        /**
		 * 清空选择
		 */
        clearSelection: function () {
            var _self = this,
                selection = _self.getSelection();
            BUI.each(selection,
                function (item) {
                    _self.clearSelected(item);
                });
        },
        /**
		 * 清空特定项选择
		 * @param {Object} item
		 */
        clearSelected: function (item) {
            var _self = this;
            item = item || _self.getSelected();
            if (item) {
                _self.setItemSelected(item, false);
            }
        },
        clearItems: function () {
            var _self = this,
                items = _self.getItems();
            items.splice(0);
            _self.clearControl();
        },
        /**
         * 清空列表项
         */
        clearControl: function () {
            this.fire("beforeitemsclear");
            var _self = this,
                listEl = _self.getItemContainer(),
                itemCls = _self.get("itemCls");
            listEl.find("." + itemCls).remove();
            this.fire("itemsclear");
        },
        setStatusValue: function (item, status, value) {
            var _self = this,
                itemStatusFields = _self.get("itemStatusFields"),
                field = itemStatusFields[status];
            if (field) {
                item[field] = value;
            }
        },
        /**
         * 全选
         */
        setAllSelection: function () {
            var _self = this,
                items = _self.getItems();
            _self.setSelection(items);
        },
        /**
         * 设置禁用
         * @param {Object} item
         * @param {Object} disabled
         */
        setItemDisabled: function (item, disabled) {
            var _self = this;
            _self.setItemStatus(item, "disabled", disabled);
        },
        /**
         * 设置列表项选中/未选中
         * @param {Object} item
         * @param {Object} selected
         */
        setItemSelected: function (item, selected) {
            var _self = this,
                isSelected;
            if (item) {
                isSelected = _self.isItemSelected(item);
                if (isSelected == selected) {
                    return;
                }
            }
            if (_self.fire("beforeselectedchange", {
                item: item,
                selected: selected
            }) !== false) {
                _self.setItemSelectedStatus(item, selected);
            }
        },
        /**
         * 根据特定字段值选中列表项
         * @param {Object} field
         * @param {Object} value
         */
        setSelectedByField: function (field, value) {
            if (!value) {
                value = field;
                field = this.get("idField");
            }
            var _self = this,
                item = _self.findItemByField(field, value);
            _self.setSelected(item);
        },
        /**
		 * 通过特定字段选中项目
		 * @param {Object} field
		 * @param {Object} values
		 */
        setSelectionByField: function (field, values) {
            if (!values) {
                values = field;
                field = this.get("idField");
            }
            var _self = this;
            BUI.each(values,
                function (value) {
                    _self.setSelectedByField(field, value);
                });
        },
        /**
		 * 选中项目
		 * @param {Object} items
		 */
        setSelection: function (items) {
            var _self = this;
            items = BUI.isArray(items) ? items : [items];
            BUI.each(items,
                function (item) {
                    _self.setSelected(item);
                });
        },
        /**
		 * 选中项目
		 * @param {Object} item
		 */
        setSelected: function (item) {
            var _self = this,
                multipleSelect = _self.get("multipleSelect");
            if (!_self.isItemSelectable(item)) {
                return;
            }
            if (!multipleSelect) {
                var selectedItem = _self.getSelected();
                if (item != selectedItem) {
                    _self.clearSelected(selectedItem);
                }
            }
            _self.setItemSelected(item, true);
        },
        /**
		 * 设置特定状态
		 * @param {Object} item
		 * @param {Object} status
		 * @param {Object} value
		 * @param {Object} element
		 */
        setItemStatus: function (item, status, value, element) {
            var _self = this;
            if (item) {
                element = element || _self.findElement(item);
            }
            if (!_self.isItemDisabled(item, element) || status === "disabled") {
                if (item) {
                    if (status === "disabled" && value) {
                        _self.clearItemStatus(item);
                    }
                    _self.setStatusValue(item, status, value);
                    _self.setItemStatusCls(status, element, value);
                    _self.fire("itemstatuschange", {
                        item: item,
                        status: status,
                        value: value,
                        element: element
                    });
                }
                if (status === "selected") {
                    _self.afterSelected(item, value, element);
                }
            }
        },
        /**
		 * 设置选中状态
		 * @param {Object} item
		 * @param {Object} selected
		 * @param {Object} element
		 */
        setItemSelectedStatus: function (item, selected, element) {
            var _self = this;
            element = element || _self.findElement(item);
            _self.setItemStatus(item, "selected", selected, element);
        },
        /**
		 * 更新项目
		 * @param {Object} item
		 */
        updateItem: function (item) {
            var _self = this,
                items = _self.getItems(),
                index = BUI.Array.indexOf(item, items),
                element = null,
                tpl;
            if (index >= 0) {
                element = _self.findElement(item);
                tpl = _self.getItemTpl(item, index);
                if (element) {
                    $(element).html(tpl);
                }
            }
            _self.fire("itemupdated", {
                item: item,
                domTarget: $(element)[0],
                element: element
            });
        },
        /**
		 * 选项是否存在某种状态
		 * @param {Object} item
		 * @param {Object} status
		 * @param {Object} elem
		 */
        hasStatus: function (item, status, elem) {
            if (!item) {
                return false;
            }
            var _self = this,
                elem = elem || _self.findElement(item);
            var cls = _self.getItemStatusCls(status);
            return $(elem).hasClass(cls);

        },
        /**
		 * 通过列表获取对应dom
		 * @param {Object} item
		 */
        findElement: function (item) {
            var _self = this,
                elements = _self.getAllElements(),
                result = null;
            if (BUI.isString(item)) {
                item = _self.getItem(item);
            }
            BUI.each(elements,
                function (element) {
                    if (_self.getItemByElement(element) == item) {
                        result = element;
                        return false;
                    }
                });
            return result;
        },
        /**
		 * 通过特定字段获取列表
		 * @param {Object} field
		 * @param {Object} value
		 */
        findItemByField: function (field, value) {
            var _self = this,
                items = _self.get("items"),
                result = null;
            BUI.each(items,
                function (item) {
                    if (item[field] != null && item[field] == value) {
                        result = item;
                        return false;
                    }
                });
            return result;
        },
        /**
         * 判断列表项是否被选中
         * @param {Object} item
         * @param {Object} element
         */
        isItemSelected: function (item, element) {
            var _self = this,
                cls = _self.getItemStatusCls("selected");
            element = element || _self.findElement(item);
            return element && $(element).hasClass(cls);
        },
        /**
		 * 列表是否禁用
		 * @param {Object} item
		 * @param {Object} element
		 */
        isItemDisabled: function (item, element) {
            return this.hasStatus(item, "disabled", element);
        },
        /**
		 * 列表是否可以选择
		 * @param {Object} item
		 */
        isItemSelectable: function (item) {
            return true;
        },
        indexOfItem: function (item) {
            return BUI.Array.indexOf(item, this.getItems());
        },
        /**
		 * 获取列表容器
		 */
        getItemContainer: function () {
            var container = this.get("itemContainer");
            if (container.length) {
                return container;
            }
            return this.get("el");
        },
        /**
		 * 获取列表项对应状态的样式
		 * @param {Object} name
		 */
        getItemStatusCls: function (name) {
            return getItemStatusCls(name, this);
        },
        /**
		 * 获取列表模版
		 * @param {Object} item
		 * @param {Object} index
		 */
        getItemTpl: function (item, index) {
            var _self = this,
                render = _self.get("itemTplRender"),
                itemTpl = _self.get("itemTpl");
            if (render) {
                return render(item, index);
            }
            return BUI.substitute(itemTpl, item);
        },
        /**
		 * 获取所有列表dom
		 */
        getAllElements: function () {
            var _self = this,
                itemCls = _self.get("itemCls"),
                el = _self.get("el");
            return el.find("." + itemCls);
        },
        /**
		 * 通过状态获取第一个dom
		 * @param {Object} name
		 */
        getFirstElementByStatus: function (name) {
            var _self = this,
                cls = _self.getItemStatusCls(name),
                el = _self.get("el");
            return el.find("." + cls)[0];
        },
        /**
		 * 通过样式查找DOM元素
		 * @param {Object} status
		 */
        getElementsByStatus: function (status) {
            var _self = this,
                cls = _self.getItemStatusCls(status),
                el = _self.get("el");
            return el.find("." + cls);
        },
        /**
		 * 通过样式查找DOM元素
		 */
        getSelectedElements: function () {
            var _self = this,
                cls = _self.getItemStatusCls("selected"),
                el = _self.get("el");
            return el.find("." + cls);
        },
        /**
		 * 设置列表项选中
		 * @param {Object} name
		 * @param {Object} elem
		 * @param {Object} value
		 */
        setItemStatusCls: function (name, elem, value) {
            var _self = this,
                cls = _self.getItemStatusCls(name),
                method = value ? "addClass" : "removeClass";
            if (elem) {
                $(elem)[method](cls);
            }
        },
        afterSelected: function (item, selected, elem) {
            var _self = this;
            if (selected) {
                _self.fire("itemselected", {
                    item: item,
                    domTarget: elem
                });
                _self.fire("selectedchange", {
                    item: item,
                    domTarget: elem,
                    selected: selected
                });
            } else {
                _self.fire("itemunselected", {
                    item: item,
                    domTarget: elem
                });
                if (_self.get("multipleSelect")) {
                    _self.fire("selectedchange", {
                        item: item,
                        domTarget: elem,
                        selected: selected
                    });
                }
            }
        }
    }, {
            ATTRS: {
                items: {
                    shared: false
                },
                /**
                 * 主键字段
                 */
                idField: {
                    value: "value"
                },
                /**
                 * 列表项的默认模板。
                 */
                itemTpl: {},
                /**
                 * 列表项渲染函数
                 */
                itemTplRender: {},
                itemStatusCls: {
                    value: {}
                },
                multipleSelect: {},
                selectedEvent: {
                    value: "click"
                },
                dataField: {
                    value: "data-item"
                },
                /**
                 *  选项所在容器，如果未设定，使用 el
                 */
                itemContainer: {},
                /**
                 * 选项状态对应的选项值
                 * 
                 *   - 此字段用于将选项记录的值跟显示的DOM状态相对应
                 *   - 例如：下面记录中 <code> checked : true </code>，可以使得此记录对应的DOM上应用对应的状态(默认为 'list-item-checked')
                 *     <pre><code>{id : '1',text : 1,checked : true}</code></pre>
                 *   - 当更改DOM的状态时，记录中对应的字段属性也会跟着变化
                 * <pre><code>
                 *   var list = new List.SimpleList({
                 *   render : '#t1',
                 *   idField : 'id', //自定义样式名称
                 *   itemStatusFields : {
                 *     checked : 'checked',
                 *     disabled : 'disabled'
                 *   },
                 *   items : [{id : '1',text : '1',checked : true},{id : '2',text : '2',disabled : true}]
                 * });
                 * list.render(); //列表渲染后，会自动带有checked,和disabled对应的样式
                 *
                 * var item = list.getItem('1');
                 * list.hasStatus(item,'checked'); //true
                 *
                 * list.setItemStatus(item,'checked',false);
                 * list.hasStatus(item,'checked');  //false
                 * item.checked;                    //false
                 * 
                 * </code></pre>
                 * ** 注意 **
                 * 此字段跟 {@link #itemStatusCls} 一起使用效果更好，可以自定义对应状态的样式
                 * @cfg {Object} itemStatusFields
                 */
                itemStatusFields: {
                    value: {}
                },
                /**
                 * 项的样式，用来获取子项
                 */
                itemCls: {},
                /**
                 * 是否允许取消选中，在多选情况下默认允许取消，单选情况下不允许取消,注意此属性只有单选情况下生效
                 */
                cancelSelected: {
                    value: false
                },
                /**
                 * 获取项的文本，默认获取显示的文本
                 */
                textGetter: {},
                events: {
                    value: {
                        itemrendered: true,
                        itemremoved: true,
                        itemupdated: true,
                        itemclick: false,
                        itemsshow: false,
                        beforeitemsshow: false,
                        itemsclear: false,
                        itemdblclick: false,
                        beforeitemsclear: false,
                        selectedchange: false,
                        beforeselectedchange: false,
                        itemselected: false,
                        itemunselected: false
                    }
                }
            }
        }, {
            xclass: "domlist"
        });
    BUI.List.DomList = domList;
})(window.BUI, jQuery);
//BUI.List.SimpleList +
(function (BUI, $) {
    "use strict";
    var UIBase = BUI.Component.UIBase,
        DomList = BUI.List.DomList,
        CLS_ITEM = BUI.prefix + "list-item";
    var simpleList = DomList.extend({
        bindUI: function () {
            var _self = this,
                itemCls = _self.get("itemCls"),
                itemContainer = _self.getItemContainer();
            itemContainer.delegate("." + itemCls, "mouseover",
                function (ev) {
                    if (_self.get("disabled")) {
                        return;
                    }
                    var element = ev.currentTarget,
                        item = _self.getItemByElement(element);
                    if (_self.isItemDisabled(ev.item, ev.currentTarget)) {
                        return;
                    }
                    _self.setItemStatus(item, "hover", true, element);

                }).delegate("." + itemCls, "mouseout",
                    function (ev) {
                        if (_self.get("disabled")) {
                            return;
                        }
                        var element = $(ev.currentTarget);
                        _self.setItemStatusCls("hover", element, false);
                    });
        }
    }, {
            ATTRS: {
                focusable: {
                    value: false
                },
                items: {
                    value: []
                },
                itemCls: {
                    value: CLS_ITEM
                },
                idField: {
                    value: "value"
                },
                listSelector: {
                    value: "ul"
                },
                itemTpl: {
                    value: '<li class="' + CLS_ITEM + '">{text}</li>'
                },
                tpl: {
                    value: '<ul></ul>'
                },
                itemContainer: {
                    valueFn: function () {
                        return this.get("el").find(this.get("listSelector"));
                    }
                }
            }
        }, {
            xclass: "simple-list",
            prority: 0
        });
    BUI.List.SimpleList = simpleList;
})(window.BUI, jQuery);
//BUI.List.Listbox +
(function (BUI, $) {
    "use strict";
    var SimpleList = BUI.List.SimpleList;
    var listbox = SimpleList.extend({}, {
        ATTRS: {
            itemTpl: {
                value: '<li><span class="bui-checkbox"></span>{text}</li>'
            },
            multipleSelect: {
                value: true
            }
        }
    }, {
            xclass: "listbox"
        });
    BUI.List.Listbox = listbox;
})(window.BUI, jQuery);;
///<jscompress sourcefile="06toolbar.js" />
//BUI.Toolbar.BarItem  +
(function (BUI, $) {
    "use strict";
    BUI.Toolbar = {};
    var PREFIX = BUI.prefix;
    var BarItem = BUI.Component.Controller.extend({
        renderUI: function () {
            var el = this.get("el");
            el.addClass(PREFIX + "inline-block");
            if (!el.attr("id")) {
                el.attr("id", this.get("id"));
            }
        },
        _uiSetSelected: function (v) {
            var _self = this,
                cls = _self.getStatusCls("selected"),
                el = _self.get("el");
            if (v) {
                el.addClass(cls);
            } else {
                el.removeClass(cls);
            }
        }
    },
        {
            ATTRS: {
                selected: {
                    sync: false,
                    value: false
                },
                elTagName: {
                    value: "li"
                },
                selectable: {
                    value: true
                },
                focusable: {
                    value: false
                },
                tpl: {
                    value: "<span>{text}</span>"
                }
            }
        },
        {
            xclass: "bar-item",
            priority: 1
        });
    var ButtonBarItem = BarItem.extend({
        _uiSetDisabled: function (value) {
            var _self = this,
                el = _self.get("el"),
                method = value ? "addClass" : "removeClass";
            el.children("a").attr("disabled", value)[method](PREFIX + "btn-disabled");
        },
        _uiSetChecked: function (value) {
            var _self = this,
                el = _self.get("el"),
                method = value ? "addClass" : "removeClass";
            el.children("a")[method](PREFIX + "btn-checked");
        },
        _uiSetText: function (v) {
            var _self = this,
                el = _self.get("el");
            el.children("a").text(v);
        },
        _uiSetBtnCls: function (v) {
            var _self = this,
                el = _self.get("el");
            el.children("a").addClass(v);
        }
    },
        {
            ATTRS: {
                checked: {
                    value: false
                },
                tpl: {
                    value: '<a href="#" class="{btnCls}">{text}</a >'
                },
                btnCls: {
                    value: "btn btn-default"
                },
                text: {
                    sync: false,
                    value: ""
                }
            }
        },
        {
            xclass: "bar-item-button",
            priority: 2
        });
    var TextBarItem = BarItem.extend({
        _uiSetText: function (text) {
            var _self = this,
                el = _self.get("el");
            el.html(text);
        }
    },
        {
            ATTRS: {
                text: {
                    value: ""
                }
            }
        },
        {
            xclass: "bar-item-text",
            priority: 2
        });
    BarItem.types = {
        button: ButtonBarItem,
        text: TextBarItem
    };
    BUI.Toolbar.BarItem = BarItem;
})(window.BUI, jQuery);
//BUI.Toolbar.Bar  +
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        UIBase = Component.UIBase,
        ChildList = BUI.List.ChildList;
    var Bar = ChildList.extend({
        renderUI: function () {
            var el = this.get("el");
            if (!el.attr("id")) {
                el.attr("id", BUI.guid("bar"));
            }
        }
    },
        {
            ATTRS: {
                elTagName: {
                    value: "ul"
                },
                idField: {
                    value: "id"
                },
                defaultChildClass: {
                    value: "bar-item"
                }
            }
        },
        {
            xclass: "toolbar",
            priority: 1
        });
    BUI.Toolbar.Bar = Bar;
})(window.BUI, jQuery);
//BUI.Toolbar.PagingBar  +
(function (BUI, $) {
    "use strict";
    var ChildList = BUI.List.ChildList,
        Component = BUI.Component;
    var PREFIX = BUI.prefix,
        ID_FIRST = "first",
        ID_PREV = "prev",
        ID_NEXT = "next",
        ID_LAST = "last",
        ID_SKIP = "skip",
        ID_REFRESH = "refresh",
        ID_TOTAL_PAGE = "totalPage",
        ID_CURRENT_PAGE = "curPage",
        ID_TOTAL_COUNT = "totalCount";
    var PagingBar = ChildList.extend({
        initializer: function () {
            var _self = this,
                children = _self.get("children"),
                store = _self.get("store");
            if (children.length < 1) {
                var items = _self._getItems();
                BUI.each(items,
                    function (item) {
                        children.push(item);
                    });
            }
            if (store && store.get("pageSize")) {
                _self.set("pageSize", store.get("pageSize"));
            }
        },
        bindUI: function () {
            var _self = this,
                store = _self.get("store");
            if (!store) {
                return;
            }
            store.on("load",
                function (e) {
                    _self.onLoad(e);
                });
            _self._bindButtonEvent();
        },
        _bindButtonEvent: function () {
            var _self = this;
            _self._bindButtonItemEvent(ID_FIRST,
                function () {
                    _self.jumpToPage(1);
                });
            _self._bindButtonItemEvent(ID_PREV,
                function () {
                    _self.jumpToPage(_self.get("curPage") - 1);
                });
            _self._bindButtonItemEvent(ID_NEXT,
                function () {
                    _self.jumpToPage(_self.get("curPage") + 1);
                });
            _self._bindButtonItemEvent(ID_LAST,
                function () {
                    _self.jumpToPage(_self.get("totalPage"));
                });
            _self._bindButtonItemEvent(ID_SKIP,
                function () {
                    handleSkip();
                });
            _self._bindButtonItemEvent(ID_REFRESH,
                function () {
                    _self.jumpToPage(_self.get("curPage"));
                });
            var curPage = _self.getChild(ID_CURRENT_PAGE);
            if (curPage) {
                curPage.get("el").on("keydown",
                    function (event) {
                        event.stopPropagation();
                        if (event.keyCode === 13) {
                            handleSkip();
                        }
                    });
            }
            function handleSkip() {
                var value = parseInt(_self._getCurrentPageValue(), 10);
                if (_self._isPageAllowRedirect(value)) {
                    _self.jumpToPage(value);
                } else {
                    _self._setCurrentPageValue(_self.get("curPage"));
                }
            }
        },
        _bindButtonItemEvent: function (id, func) {
            var _self = this,
                item = _self.getChild(id);
            if (item) {
                item.on("click", func);
            }
        },
        onLoad: function (params) {
            var _self = this,
                store = _self.get("store");
            _self._afterStoreLoad(store, params);
        },
        _afterStoreLoad: function (store, params) {
            var _self = this,
                pageSize = _self.get("pageSize"),
                pageIndex = store.get("pageIndex"),
                start = 0,
                end,
                totalCount,
                curPage,
                totalPage;
            start = (pageIndex - 1) * pageSize + 1;
            totalCount = store.getTotalCount();
            end = totalCount - start > pageSize ? start + store.getCount() - 1 : totalCount;
            totalPage = parseInt((totalCount + pageSize - 1) / pageSize, 10);
            totalPage = totalPage > 0 ? totalPage : 1;
            _self.set("start", start);
            _self.set("end", end);
            _self.set("totalCount", totalCount);
            _self.set("curPage", pageIndex);
            _self.set("totalPage", totalPage);
            _self._setAllButtonsState();
            _self._setNumberPages();
        },
        jumpToPage: function (page) {
            if (page <= 0 || page > this.get("totalPage")) {
                return;
            }
            var _self = this,
                store = _self.get("store"),
                pageSize = _self.get("pageSize");
            var result = _self.fire("beforepagechange", {
                from: _self.get("curPage"),
                to: page
            });
            if (store && result !== false) {
                store.load({
                    pageSize: pageSize,
                    pageIndex: page
                });
            }
        },
        _getItems: function () {
            var _self = this,
                items = [];
            items.push(_self._getButtonItem(ID_FIRST));
            items.push(_self._getButtonItem(ID_PREV));
            items.push(_self._getTextItem(ID_TOTAL_PAGE));
            items.push(_self._getTextItem(ID_CURRENT_PAGE));
            items.push(_self._getButtonItem(ID_SKIP));
            items.push(_self._getButtonItem(ID_NEXT));
            items.push(_self._getButtonItem(ID_LAST));
            items.push(_self._getTextItem(ID_TOTAL_COUNT));
            return items;
        },
        _getButtonItem: function (id) {
            var _self = this;
            return {
                id: id,
                xclass: "bar-item-button",
                text: _self.get(id + "Text"),
                disabled: true,
                elCls: id + "Cls"
            };
        },
        _getTextItem: function (id) {
            var _self = this;
            return {
                id: id,
                xclass: "bar-item-text",
                text: _self._getTextItemTpl(id),
                elCls: "bar-item-" + id
            };
        },
        _getTextItemTpl: function (id) {
            var _self = this,
                obj = _self.getAttrVals();
            return BUI.substitute(this.get(id + "Tpl"), obj);
        },
        _isPageAllowRedirect: function (value) {
            var _self = this;
            return value && value > 0 && value <= _self.get("totalPage") && value !== _self.get("curPage");
        },
        _setAllButtonsState: function () {
            var _self = this,
                store = _self.get("store");
            if (store) {
                _self._setButtonsState([ID_PREV, ID_NEXT, ID_FIRST, ID_LAST, ID_SKIP], true);
            }
            if (_self.get("curPage") === 1) {
                _self._setButtonsState([ID_PREV, ID_FIRST], false);
            }
            if (_self.get("curPage") === _self.get("totalPage")) {
                _self._setButtonsState([ID_NEXT, ID_LAST], false);
            }
        },
        _setButtonsState: function (buttons, enable) {
            var _self = this,
                children = _self.get("children");
            BUI.each(children,
                function (child) {
                    if (BUI.Array.indexOf(child.get("id"), buttons) !== -1) {
                        child.set("disabled", !enable);
                    }
                });
        },
        _setNumberPages: function () {
            var _self = this,
                items = _self.getChildren();
            BUI.each(items,
                function (item) {
                    if (item.__xclass === "bar-item-text") {
                        item.set("content", _self._getTextItemTpl(item.get("id")));
                    }
                });
        },
        _getCurrentPageValue: function (curItem) {
            var _self = this;
            curItem = curItem || _self.getChild(ID_CURRENT_PAGE);
            if (curItem) {
                var textEl = curItem.get("el").find("input");
                return textEl.val();
            }
        },
        _setCurrentPageValue: function (value, curItem) {
            var _self = this;
            curItem = curItem || _self.getChild(ID_CURRENT_PAGE);
            if (curItem) {
                var textEl = curItem.get("el").find("input");
                textEl.val(value);
            }
        }
    },
        {
            ATTRS: {
                firstText: {
                    value: "首 页"
                },
                prevText: {
                    value: "上一页"
                },
                nextText: {
                    value: "下一页"
                },
                lastText: {
                    value: "末 页"
                },
                skipText: {
                    value: "确 定"
                },
                refreshText: {
                    value: "刷 新"
                },
                totalPageTpl: {
                    value: "共 {totalPage} 页"
                },
                curPageTpl: {
                    value: '第 <input type="text" ' + 'autocomplete="off" class="form-control ' + PREFIX + 'pb-page" size="20" value="{curPage}" name="inputItem"> 页'
                },
                totalCountTpl: {
                    value: "共{totalCount}条记录"
                },
                curPage: {
                    value: 0
                },
                totalPage: {
                    value: 0
                },
                totalCount: {
                    value: 0
                },
                pageSize: {
                    value: 20
                },
                elCls: {
                    value: ""
                },
                defaultChildClass: {
                    value: "bar-item"
                },
                elTagName: {
                    value: "ul"
                },
                idField: {
                    value: "id"
                },
                store: {}
            }
        },
        {
            xclass: "pagingbar",
            priority: 2
        });
    BUI.Toolbar.PagingBar = PagingBar;
})(window.BUI, jQuery);
//BUI.Toolbar.NumberPagingBar   +
(function (BUI, $) {
    "use strict";
    var ChildList = BUI.List.ChildList,
        Component = BUI.Component;
    var PREFIX = BUI.prefix,
        ID_PREV = "prev",
        ID_NEXT = "next";
    var NumberPagingBar = ChildList.extend({
        initializer: function () {
            var _self = this,
                children = _self.get("children"),
                store = _self.get("store");
            if (children.length < 1) {
                var items = _self._getItems();
                BUI.each(items,
                    function (item) {
                        children.push(item);
                    });
            }
            if (store && store.get("pageSize")) {
                _self.set("pageSize", store.get("pageSize"));
            }
        },
        bindUI: function () {
            var _self = this,
                store = _self.get("store");
            if (!store) {
                return;
            }
            store.on("load",
                function (e) {
                    _self.onLoad(e);
                });
            _self._bindButtonEvent();
        },
        _bindButtonEvent: function () {
            var _self = this,
                cls = _self.get('numberButtonCls');
            _self._bindButtonItemEvent(ID_PREV,
                function () {
                    _self.jumpToPage(_self.get("curPage") - 1);
                });
            _self._bindButtonItemEvent(ID_NEXT,
                function () {
                    _self.jumpToPage(_self.get("curPage") + 1);
                });
            _self.get('el').delegate('a', 'click', function (ev) {
                ev.preventDefault();
                return false;
            });
            _self.on('click', function (ev) {
                var item = ev.target;
                if (item && item.get('el').hasClass(cls)) {
                    var page = item.get('id');
                    _self.jumpToPage(page);
                }
            });
        },
        _bindButtonItemEvent: function (id, func) {
            var _self = this,
                item = _self.getChild(id);
            if (item) {
                item.on("click", func);
            }
        },
        onLoad: function (params) {
            var _self = this,
                store = _self.get("store");
            _self._afterStoreLoad(store, params);
        },
        _afterStoreLoad: function (store, params) {
            var _self = this,
                pageSize = _self.get("pageSize"),
                pageIndex = store.get("pageIndex"),
                start = 0,
                end,
                totalCount,
                curPage,
                totalPage;
            start = (pageIndex - 1) * pageSize + 1;
            totalCount = store.getTotalCount();
            end = totalCount - start > pageSize ? start + store.getCount() - 1 : totalCount;
            totalPage = parseInt((totalCount + pageSize - 1) / pageSize, 10);
            totalPage = totalPage > 0 ? totalPage : 1;
            _self.set("start", start);
            _self.set("end", end);
            _self.set("totalCount", totalCount);
            _self.set("curPage", pageIndex);
            _self.set("totalPage", totalPage);
            _self._setAllButtonsState();
            _self._setNumberPages();
        },
        jumpToPage: function (page) {
            if (page <= 0 || page > this.get("totalPage")) {
                return;
            }
            var _self = this,
                store = _self.get("store"),
                pageSize = _self.get("pageSize");
            var result = _self.fire("beforepagechange", {
                from: _self.get("curPage"),
                to: page
            });
            if (store && result !== false) {
                store.load({
                    pageSize: pageSize,
                    pageIndex: page
                });
            }
        },
        _getItems: function () {
            var _self = this,
                items = [];
            items.push(_self._getButtonItem(ID_PREV));
            items.push(_self._getButtonItem(ID_NEXT));
            return items;
        },
        _getButtonItem: function (id) {
            var _self = this;
            return {
                id: id,
                content: '<a href="javascript:;"><span aria-hidden="true">' + _self.get(id + 'Text') + '</span></a>',
                disabled: true
            };
        },
        _setAllButtonsState: function () {
            var _self = this,
                store = _self.get("store");
            if (store) {
                _self._setButtonsState([ID_PREV, ID_NEXT], true);
            }
            if (_self.get("curPage") === 1) {
                _self._setButtonsState([ID_PREV], false);
            }
            if (_self.get("curPage") === _self.get("totalPage")) {
                _self._setButtonsState([ID_NEXT], false);
            }
        },
        _setButtonsState: function (buttons, enable) {
            var _self = this,
                children = _self.get("children");
            BUI.each(children,
                function (child) {
                    if (BUI.Array.indexOf(child.get("id"), buttons) !== -1) {
                        child.set("disabled", !enable);
                    }
                });
        },
        _setNumberPages: function () {
            var _self = this,
                curPage = _self.get('curPage'),
                totalPage = _self.get('totalPage'),
                numberItems = _self._getNumberItems(curPage, totalPage),
                curItem;

            _self._clearNumberButtons();

            BUI.each(numberItems, function (item) {
                _self._appendNumberButton(item);
            });
            curItem = _self.getChild(curPage);
            if (curItem) {
                curItem.set('selected', true);
            }
        },
        _appendNumberButton: function (cfg) {
            var _self = this,
                count = _self.getChildCount();
            var item = _self.addChild(cfg, count - 1);
        },
        _clearNumberButtons: function () {
            var _self = this,
                items = _self.getChildren(),
                count = _self.getChildCount();

            while (count > 2) {
                _self.removeChildAt(count - 2);
                count = _self.getChildCount();
            }
        },
        //获取所有页码按钮的配置项
        _getNumberItems: function (curPage, totalPage) {
            var _self = this,
                result = [],
                maxLimitCount = _self.get('maxLimitCount'),
                showRangeCount = _self.get('showRangeCount'),
                maxPage;

            function addNumberItem(from, to) {
                for (var i = from; i <= to; i++) {
                    result.push(_self._getNumberItem(i));
                }
            }

            function addEllipsis() {
                result.push(_self._getEllipsisItem());
            }

            if (totalPage < maxLimitCount) {
                maxPage = totalPage;
                addNumberItem(1, totalPage);
            } else {
                var startNum = (curPage <= maxLimitCount) ? 1 : (curPage - showRangeCount),
                    lastLimit = curPage + showRangeCount,
                    endNum = lastLimit < totalPage ? (lastLimit > maxLimitCount ? lastLimit : maxLimitCount) : totalPage;
                if (startNum > 1) {
                    addNumberItem(1, 1);
                    if (startNum > 2) {
                        addEllipsis();
                    }
                }
                maxPage = endNum;
                addNumberItem(startNum, endNum);
            }

            if (maxPage < totalPage) {
                if (maxPage < totalPage - 1) {
                    addEllipsis();
                }
                addNumberItem(totalPage, totalPage);
            }

            return result;
        },
        //获取省略号
        _getEllipsisItem: function () {
            var _self = this;
            return {
                disabled: true,
                content: _self.get('ellipsisTpl')
            };
        },
        //生成页面按钮配置项
        _getNumberItem: function (page) {
            var _self = this;
            return {
                id: page,
                elCls: "bui-button-number"
            };
        }
    },
        {
            ATTRS: {
                prevText: {
                    value: "上一页"
                },
                nextText: {
                    value: "下一页"
                },
                curPage: {
                    value: 0
                },
                totalPage: {
                    value: 0
                },
                totalCount: {
                    value: 0
                },
                pageSize: {
                    value: 20
                },
                elCls: {
                    value: "pagination"
                },
                defaultChildClass: {
                    value: "bar-item"
                },
                elTagName: {
                    value: "ul"
                },
                idField: {
                    value: "id"
                },
                childStatusCls: {
                    value: {
                        selected: 'active',
                        disabled: 'disabled'
                    }
                },
                childTpl: {
                    value: '<a href="###">{id}</a>'
                },
                maxLimitCount: {
                    value: 6
                },
                showRangeCount: {
                    value: 1
                },
                numberButtonCls: {
                    value: 'bui-button-number'
                },
                ellipsisTpl: {
                    value: '<a href="#">...</a>'
                },
                store: {}
            }
        },
        {
            xclass: "pagingnumberbar",
            priority: 2
        });
    BUI.Toolbar.NumberPagingBar = NumberPagingBar;
})(window.BUI, jQuery);
;
///<jscompress sourcefile="07overlay.js" />
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

    //得到会导致元素显示不全的祖先元素
    //position位置丁文的四种情况
    //  relative:生成相对定位的元素，相对于其正常位置进行定位。原来的位置空间还存在
    //  absolute:生成绝对定位的元素，相对于 static 定位以外的第一个父元素进行定位。原来的位置清空
    //           只要父元素存在absolute、fixed和relative任一的定位属性，就会相对其进行移位
    //  fixed:特殊的absolute定位,生成绝对定位的元素，相对于浏览器窗口进行定位。
    //  static:默认值。没有定位，元素出现在正常的流中（忽略 top, bottom, left, right 或者 z-index 声明）。
    function getOffsetParent(element) {
        var doc = element.ownerDocument,
            body = doc.body,
            parent, positionStyle = $(element).css("position"),
            skipStatic = positionStyle == "fixed" || positionStyle == "absolute";
        if (!skipStatic) {
            return element.nodeName.toLowerCase() == "html" ? null : element.parentNode;
        }
        for (parent = element.parentNode; parent && parent != body; parent = parent.parentNode) {
            positionStyle = $(parent).css("position");
            if (positionStyle != "static") {
                return parent;
            }
        }
        return null;
    }
    //获得元素的显示部分的区域
    function getVisibleRectForElement(element) {
        var visibleRect = {
            left: 0,
            right: Infinity,
            top: 0,
            bottom: Infinity
        },
            el,
            scrollX,
            scrollY,
            winSize,
            doc = element.ownerDocument,
            body = doc.body,
            documentElement = doc.documentElement;
        for (el = element; el = getOffsetParent(el);) {
            if (el.clientWidth != 0 && el != body && el != documentElement && $(el).css("overflow") != "visible") {
                var pos = $(el).offset();
                pos.left += el.clientLeft;
                pos.top += el.clientTop;
                visibleRect.top = Math.max(visibleRect.top, pos.top);
                visibleRect.right = Math.min(visibleRect.right, pos.left + el.clientWidth);
                visibleRect.bottom = Math.min(visibleRect.bottom, pos.top + el.clientHeight);
                visibleRect.left = Math.max(visibleRect.left, pos.left);
            }
        }
        scrollX = $(win).scrollLeft();
        scrollY = $(win).scrollTop();
        visibleRect.left = Math.max(visibleRect.left, scrollX);
        visibleRect.top = Math.max(visibleRect.top, scrollY);
        winSize = {
            width: BUI.viewportWidth(),
            height: BUI.viewportHeight()
        };
        visibleRect.right = Math.min(visibleRect.right, scrollX + winSize.width);
        visibleRect.bottom = Math.min(visibleRect.bottom, scrollY + winSize.height);
        return visibleRect.top >= 0 && visibleRect.left >= 0 && visibleRect.bottom > visibleRect.top && visibleRect.right > visibleRect.left ? visibleRect : null;
    }
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
                //当前节点可以被放置的显示区域
                //visibleRect = getVisibleRectForElement(el[0]),
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
                    value: '<s class="' + CLS_ARROW + '"><s class="' + CLS_ARROW + '-inner"></s></s>'
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
        CLS_PREFIX = BUI.prefix + "stdmod-",
        CLS_TITLE = "header-title",
        PREFIX = BUI.prefix,
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
        var mask = $('<div style="left: 0px; top: 0px; width: 100%; height: 100%; position: fixed;" class="' + maskCls + '"></div>').prependTo("body");
        mask.on("mousedown",
            function (e) {
                e.preventDefault();
            });
        return mask;
    }

    function createUI(self, part) {
        var el = self.get("contentEl"),
            partEl = self.get(part);
        if (!partEl) {
            partEl = $('<div class="' + CLS_PREFIX + part + '" ></div>');
            partEl.appendTo(el);
            self.setInternal(part, partEl);
        }
    }

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
            createUI(this, "header");
            createUI(this, "body");
            createUI(this, "footer");
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
                temp = '<button class="' + conf.elCls + '">' + conf.text + "</button>",
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
                el = _self.get("el");
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
                if (!btn) {
                    self.setInternal("closeBtn", btn = $(self.get("closeTpl")));
                }
                btn.appendTo(self.get("el"), undefined);
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
                header: {},
                body: {},
                footer: {},
                bodyStyle: {},
                footerStyle: {},
                headerStyle: {},
                headerContent: {
                    value: '<div class="' + CLS_TITLE + '">标题</div>'
                },
                bodyContent: {},
                footerContent: {},
                closeTpl: {
                    value: '<a tabindex="0" href=javascript:void("0") class="' + PREFIX + 'ext-close" style=""><span class="' + PREFIX + 'ext-close-x">×</span></a>'
                },
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
                    value: false
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
})(window.BUI, jQuery);;
///<jscompress sourcefile="08menu.js" />
//BUI.Menu.SideMenu
(function (BUI, $) {
    "use strict";
    BUI.Menu = {};
    var SideMenu = function (config) {
        SideMenu.superclass.constructor.call(this, config);
        this.init();
    }

    SideMenu.ATTRS = {
        renderEl: {
            value: ""
        },
        activeMenu: {
            value: ""
        },
        activeItem: {
            value: ""
        },
        events: {
            click: false
        }
    };

    BUI.extend(SideMenu, BUI.Base);

    BUI.augment(SideMenu, {
        init: function () {
            var _self = this,
                render = _self.get("render");
            _self.set("renderEl", $(render));
            _self.syncItems();
            _self.bindEvents();

        },
        syncItems: function () {
            var _self = this, renderEl = _self.get("renderEl");
            var activeMenu = renderEl.children(".active");
            _self.set("activeMenu", activeMenu);

        },
        bindEvents: function () {
            var _self = this, renderEl = _self.get("renderEl");

            renderEl.delegate("a", "click",
                function (e) {
                    var $this = $(this),
                        parentLi = $this.parent('li'),
                        checkElement = parentLi.children("ul");
                    if (checkElement.hasClass('treeview-menu') && checkElement.hasClass('menu-open')) {
                        parentLi.removeClass("active");
                        parentLi.removeClass("menu-open");
                        checkElement.removeClass("menu-open");
                    } else if (checkElement.hasClass('treeview-menu') && !checkElement.hasClass('menu-open')) {
                        var activeMenu = _self.get("activeMenu");
                        if (activeMenu) {
                            activeMenu.removeClass("active");
                            activeMenu.removeClass("menu-open");
                            var chkElement = activeMenu.children("ul");
                            chkElement.removeClass("menu-open");
                        }
                        checkElement.addClass('menu-open');
                        parentLi.addClass('active');
                        parentLi.removeClass("menu-open");
                        _self.set("activeMenu", parentLi);
                    } else {
                        if ($this.hasClass("active")) {
                            return true;
                        } else {
                            var activeItem = _self.get("activeItem");
                            if (activeItem) {
                                activeItem.removeClass("active");
                            }
                            parentLi.addClass("active");
                            _self.set("activeItem", parentLi);
                            _self.fire("click", {
                                href: $this.attr("data-href"),
                                target: $this,
                                text: $this.text()
                            });
                        }
                    }
                    if (checkElement.is('.treeview-menu')) {
                        e.preventDefault();
                    }
                });
        },
        syncFit: function () {
            var _self = this,
                renderEl = _self.get("renderEl"),
                parentEl = renderEl.parent();
            var pHeight = parentEl.height();
            renderEl.css({
                height: pHeight
            });
        }
    });
    BUI.Menu.SideMenu = SideMenu;
})(window.BUI, jQuery);;
///<jscompress sourcefile="09picker.js" />
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
})(window.BUI, jQuery);;
///<jscompress sourcefile="10select.js" />
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
})(window.BUI, jQuery);;
///<jscompress sourcefile="11calendar.js" />
//BUI.Calendar.Resource
(function (BUI) {
    "use strict";
    BUI.Calendar = {};
    var Res = {
        "zh-CN": {
            yearMonthMask: "yyyy 年 mm 月",
            months: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            weekDays: ["日", "一", "二", "三", "四", "五", "六"],
            today: "今天",
            clean: "清除",
            submit: "确定",
            cancel: "取消"
        },
        en: {
            yearMonthMask: "MMM yyyy",
            months: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"],
            weekDays: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
            today: "today",
            clean: "clean",
            submit: "submit",
            cancel: "cancel"
        },
        setLanguage: function (type) {
            if (Res[type]) {
                BUI.mix(this, Res[type]);
            }
        }
    };
    Res.setLanguage("zh-CN");
    BUI.Calendar.Resource = Res;
})(window.BUI);
//BUI.Calendar.Header
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        Component = BUI.Component,
        CLS_TEXT_YEAR = "year-text",
        CLS_TEXT_MONTH = "month-text",
        CLS_ARROW = "bui-datepicker-arrow",
        CLS_PREV = "bui-datepicker-prev",
        CLS_NEXT = "bui-datepicker-next",
        Resource = BUI.Calendar.Resource,
        DateUtil = BUI.Date;
    var header = Component.Controller.extend({
        bindUI: function () {
            var _self = this,
                el = _self.get("el");
            el.delegate("." + CLS_ARROW, "click",
                function (e) {
                    var sender = $(e.currentTarget);
                    if (sender.hasClass(CLS_NEXT)) {
                        _self.nextMonth();
                    } else if (sender.hasClass(CLS_PREV)) {
                        _self.prevMonth();
                    }
                    return false;
                });
            el.delegate(".bui-datepicker-month", "click",
                function () {
                    _self.fire("headerclick");
                });
        },
        setMonth: function (year, month) {
            var _self = this,
                curYear = _self.get("year"),
                curMonth = _self.get("month");
            if (year !== curYear || month !== curMonth) {
                _self.set("year", year);
                _self.set("month", month);
                _self.fire("monthchange", {
                    year: year,
                    month: month
                });
            }
        },
        nextMonth: function () {
            var _self = this,
                date = new Date(_self.get("year"), _self.get("month") + 1);
            _self.setMonth(date.getFullYear(), date.getMonth());
        },
        prevMonth: function () {
            var _self = this,
                date = new Date(_self.get("year"), _self.get("month") - 1);
            _self.setMonth(date.getFullYear(), date.getMonth());
        },
        _uiSetYear: function (v) {
            var _self = this;
            var month = _self.get("month");
            if (!isNaN(month)) {
                _self._setYearMonth(v, month);
            }
        },
        _uiSetMonth: function (v) {
            var _self = this;
            var year = _self.get("year");
            if (!isNaN(year)) {
                _self._setYearMonth(year, v);
            }
        },
        _setYearMonth: function (year, month) {
            var _self = this;
            var date = new Date(year, month);
            var str = DateUtil.format(date, Resource.yearMonthMask);
            if (str.indexOf("000") !== -1) {
                var months = Resource.months;
                str = str.replace("000", months[month]);
            }
            _self.get("el").find("." + PREFIX + "year-month-text").html(str);
        }
    },
        {
            ATTRS: {
                year: {
                    sync: true
                },
                month: {
                    sync: true,
                    setter: function (v) {
                        this.set("monthText", v + 1);
                    }
                },
                monthText: {},
                tpl: {
                    valueFn: function () {
                        return '<div class="' + CLS_ARROW + " " + CLS_PREV + '"><span class="iconfont icon-left"></span></div>' + '<div class="bui-datepicker-month">' + '<div class="month-text-container">' + '<span class="' + PREFIX + 'year-month-text ">' + "</span>" + "</div>" + "</div>" + '<div class="' + CLS_ARROW + " " + CLS_NEXT + '"><span class="iconfont icon-right "></span></div>';
                    }
                },
                elCls: {
                    value: "bui-datepicker-header"
                },
                events: {
                    value: {
                        monthchange: true
                    }
                }
            }
        },
        {
            xclass: "calendar-header"
        });
    BUI.Calendar.Header = header;
})(window.BUI, jQuery);
//BUI.Calendar.Panel
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        DateUtil = BUI.Date,
        CLS_DATE = "bui-datepicker-date",
        CLS_TODAY = "bui-datepicker-today",
        CLS_DISABLED = "bui-datepicker-disabled",
        CLS_ACTIVE = "bui-datepicker-active",
        DATA_DATE = "data-date",
        DATE_MASK = "isoDate",
        CLS_SELECTED = "bui-datepicker-selected",
        SHOW_WEEKS = 6,
        dateTypes = {
            deactive: "prevday",
            active: "active",
            disabled: "disabled"
        },
        resource = BUI.Calendar.Resource,
        weekDays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var panel = Component.Controller.extend({
        initializer: function () {
            var _self = this,
                now = new Date();
            if (!_self.get("year")) {
                _self.set("year", now.getFullYear());
            }
            if (!_self.get("month")) {
                _self.set("month", now.getMonth());
            }
        },
        renderUI: function () {
            this.updatePanel();
        },
        updatePanel: function () {
            var _self = this,
                el = _self.get("el"),
                bodyEl = el.find("tbody"),
                innerTem = _self._getPanelInnerTpl();
            bodyEl.empty();
            $(innerTem).appendTo(bodyEl);
        },
        _getPanelInnerTpl: function () {
            var _self = this,
                startDate = _self._getFirstDate(),
                temps = [];
            for (var i = 0; i < SHOW_WEEKS; i++) {
                var weekStart = DateUtil.addWeek(i, startDate);
                temps.push(_self._getWeekTpl(weekStart));
            }
            return temps.join("");
        },
        _getFirstDate: function (year, month) {
            var _self = this,
                monthFirstDate = _self._getMonthFirstDate(year, month),
                day = monthFirstDate.getDay();
            return DateUtil.addDay(day * -1, monthFirstDate);
        },
        _getWeekTpl: function (startDate) {
            var _self = this,
                weekTpl = _self.get("weekTpl"),
                daysTemps = [];
            for (var i = 0; i < weekDays.length; i++) {
                var date = DateUtil.addDay(i, startDate);
                daysTemps.push(_self._getDayTpl(date));
            }
            return BUI.substitute(weekTpl, {
                daysTpl: daysTemps.join("")
            });
        },
        _getDayTpl: function (date) {
            var _self = this,
                dayTpl = _self.get("dayTpl"),
                day = date.getDay(),
                todayCls = _self._isToday(date) ? CLS_TODAY : "",
                dayOfWeek = weekDays[day],
                dateNumber = date.getDate(),
                dateType = _self._isInRange(date) ? _self._isCurrentMonth(date) ? dateTypes.active : dateTypes.deactive : dateTypes.disabled;
            return BUI.substitute(dayTpl, {
                dayOfWeek: dayOfWeek,
                dateType: dateType,
                dateNumber: dateNumber,
                todayCls: todayCls,
                date: DateUtil.format(date, DATE_MASK)
            });
        },
        bindUI: function () {
            var _self = this,
                el = _self.get("el");
            el.delegate("." + CLS_DATE, "click",
                function (ev) {
                    var sender = $(ev.target).closest("." + CLS_DATE);
                    if (sender) {
                        var date = sender.attr("title");
                        if (date) {
                            date = DateUtil.parse(date);
                            if (_self._isInRange(date)) {
                                _self.set("selected", date);
                            }
                        }
                    }
                    ev.preventDefault();
                });
            el.delegate("." + CLS_DISABLED, "click",
                function (e) {
                    e.stopPropagation();
                });
        },
        setMonth: function (year, month) {
            var _self = this,
                curYear = _self.get("year"),
                curMonth = _self.get("month");
            if (year !== curYear || month !== curMonth) {
                _self.set("year", year);
                _self.set("month", month);
                _self.updatePanel();
            }
        },
        _uiSetSelected: function (date, ev) {
            var _self = this;
            if (!(ev && ev.prevVal && DateUtil.isDateEquals(date, ev.prevVal))) {
                _self.setMonth(date.getFullYear(), date.getMonth());
                _self._setSelectedDate(date);
                _self.fire("selectedchange", {
                    date: date
                });
            }
        },
        _setSelectedDate: function (date) {
            var _self = this,
                dateEl = _self._findDateElement(date);
            _self._clearSelectedDate();
            if (dateEl) {
                dateEl.addClass(CLS_SELECTED);
            }
        },
        _findDateElement: function (date) {
            var _self = this,
                dateStr = DateUtil.format(date, DATE_MASK),
                activeList = _self.get("el").find("." + CLS_DATE),
                result = null;
            if (dateStr) {
                activeList.each(function (index, item) {
                    if ($(item).attr("title") === dateStr) {
                        result = $(item);
                        return false;
                    }
                });
            }
            return result;
        },
        _clearSelectedDate: function () {
            var _self = this;
            _self.get("el").find("." + CLS_SELECTED).removeClass(CLS_SELECTED);
        },
        _getMonthFirstDate: function (year, month) {
            var _self = this,
                year = year || _self.get("year"),
                month = month || _self.get("month");
            return new Date(year, month);
        },
        _isCurrentMonth: function (date) {
            return date.getMonth() === this.get("month");
        },
        _isToday: function (date) {
            var tody = new Date();
            return tody.getFullYear() === date.getFullYear() && tody.getMonth() === date.getMonth() && tody.getDate() === date.getDate();
        },
        _isInRange: function (date) {
            var _self = this,
                maxDate = _self.get("maxDate"),
                minDate = _self.get("minDate");
            if (minDate && date < minDate) {
                return false;
            }
            if (maxDate && date > maxDate) {
                return false;
            }
            return true;
        },
        _uiSetMaxDate: function (v) {
            if (v) {
                this.updatePanel();
            }
        },
        _uiSetMinDate: function (v) {
            if (v) {
                this.updatePanel();
            }
        }
    },
        {
            ATTRS: {
                year: {

                },
                month: {

                },
                selected: {},
                focusable: {
                    value: true
                },
                dayTpl: {
                    value: '<td class="bui-datepicker-date bui-datepicker-{dateType} {todayCls} day-{dayOfWeek}" title="{date}"><a href="#" hidefocus="on" tabindex="1"><em><span>{dateNumber}</span></em></a></td>'
                },
                events: {
                    value: {
                        click: false,
                        selectedchange: true
                    }
                },
                maxDate: {
                    setter: function (val) {
                        if (val) {
                            if (BUI.isString(val)) {
                                return DateUtil.parse(val);
                            }
                            return val;
                        }
                    }
                },
                minDate: {
                    setter: function (val) {
                        if (val) {
                            if (BUI.isString(val)) {
                                return DateUtil.parse(val);
                            }
                            return val;
                        }
                    }
                },
                weekTpl: {
                    value: "<tr>{daysTpl}</tr>"
                },
                tpl: {
                    valueFn: function () {
                        return '<table style="width:100%" cellspacing="0">' + "<thead>" + "<tr>" + '<th  title="Sunday"><span>' + resource.weekDays[0] + "</span></th>" + '<th  title="Monday"><span>' + resource.weekDays[1] + "</span></th>" + '<th  title="Tuesday"><span>' + resource.weekDays[2] + "</span></th>" + '<th  title="Wednesday"><span>' + resource.weekDays[3] + "</span></th>" + '<th  title="Thursday"><span>' + resource.weekDays[4] + "</span></th>" + '<th  title="Friday"><span>' + resource.weekDays[5] + "</span></th>" + '<th  title="Saturday"><span>' + resource.weekDays[6] + "</span></th>" + "</tr>" + "</thead>" + '<tbody class="bui-datepicker-body">' + "</tbody>" + "</table>";
                    }
                }
            }
        },
        {
            xclass: "calendar-panel",
            priority: 0
        });
    BUI.Calendar.Panel = panel;
})(window.BUI, jQuery);
//BUI.Calendar.MonthPicker
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        List = BUI.List.SimpleList,
        Resource = BUI.Calendar.Resource,
        Toolbar = BUI.Toolbar,
        PREFIX = BUI.prefix,
        CLS_MONTH = "bui-monthpicker-month",
        DATA_MONTH = "data-month",
        DATA_YEAR = "data-year",
        CLS_YEAR = "bui-monthpicker-year",
        CLS_YEAR_NAV = "bui-monthpicker-yearnav",
        CLS_SELECTED = "bui-monthpicker-selected",
        CLS_ITEM = "bui-monthpicker-item";
    function getMonths() {
        return $.map(Resource.months,
            function (month, index) {
                return {
                    text: month,
                    value: index
                };
            });
    }
    var MonthPanel = List.extend({
        bindUI: function () {
            var _self = this;
            _self.get("el").delegate("a", "click",
                function (ev) {
                    ev.preventDefault();
                }).delegate("." + CLS_MONTH, "dblclick",
                    function () {
                        _self.fire("monthdblclick");
                    });
        }
    },
        {
            ATTRS: {
                itemTpl: {
                    value: '<li class="' + CLS_ITEM + ' bui-monthpicker-month"><a href="#" hidefocus="on">{text}</a></li>'
                },
                itemCls: {
                    value: CLS_ITEM
                },
                items: {
                    valueFn: function () {
                        return getMonths();
                    }
                },
                elCls: {
                    value: "bui-monthpicker-months"
                }
            }
        },
        {
            xclass: "calendar-month-panel"
        });
    var YearPanel = List.extend({
        bindUI: function () {
            var _self = this,
                el = _self.get("el");
            el.delegate("a", "click",
                function (ev) {
                    ev.preventDefault();
                });
            el.delegate("." + CLS_YEAR, "dblclick",
                function () {
                    _self.fire("yeardblclick");
                });
            el.delegate(".x-icon", "click",
                function (ev) {
                    var sender = $(ev.currentTarget);
                    if (sender.hasClass(CLS_YEAR_NAV + "-prev")) {
                        _self._prevPage();
                    } else if (sender.hasClass(CLS_YEAR_NAV + "-next")) {
                        _self._nextPage();
                    }
                });
            _self.on("itemselected",
                function (ev) {
                    if (ev.item) {
                        _self.setInternal("year", ev.item.value);
                    }
                });
        },
        _prevPage: function () {
            var _self = this,
                start = _self.get("start"),
                yearCount = _self.get("yearCount");
            _self.set("start", start - yearCount);
        },
        _nextPage: function () {
            var _self = this,
                start = _self.get("start"),
                yearCount = _self.get("yearCount");
            _self.set("start", start + yearCount);
        },
        _uiSetStart: function () {
            var _self = this;
            _self._setYearsContent();
        },
        _uiSetYear: function (v) {
            var _self = this,
                item = _self.findItemByField("value", v);
            if (item) {
                _self.setSelectedByField(v);
            } else {
                _self.set("start", v);
            }
        },
        _setYearsContent: function () {
            var _self = this,
                year = _self.get("year"),
                start = _self.get("start"),
                yearCount = _self.get("yearCount"),
                items = [];
            for (var i = start; i < start + yearCount; i++) {
                var text = i.toString();
                items.push({
                    text: text,
                    value: i
                });
            }
            _self.set("items", items);
            _self.setSelectedByField(year);
        }
    },
        {
            ATTRS: {
                items: {
                    value: []
                },
                elCls: {
                    value: "bui-monthpicker-years"
                },
                itemCls: {
                    value: CLS_ITEM
                },
                year: {},
                start: {
                    value: new Date().getFullYear()
                },
                yearCount: {
                    value: 10
                },
                itemTpl: {
                    value: '<li class="' + CLS_ITEM + " " + CLS_YEAR + '"><a href="#" hidefocus="on">{text}</a></li>'
                },
                tpl: {
                    value: '<div class="' + CLS_YEAR_NAV + '">' + '<span class="' + CLS_YEAR_NAV + '-prev x-icon"><span class="iconfont icon-left" aria-hidden="true"></span></span>' + '<span class="' + CLS_YEAR_NAV + '-next x-icon"><span class="iconfont icon-right" aria-hidden="true"></span></span>' + "</div>" + "<ul></ul>"
                }
            }
        },
        {
            xclass: "calendar-year-panel"
        });
    var monthPicker = Component.Controller.extend({
        initializer: function () {
            var _self = this,
                children = _self.get("children"),
                monthPanel = new MonthPanel(),
                yearPanel = new YearPanel(),
                footer = _self._createFooter();
            children.push(monthPanel);
            children.push(yearPanel);
            children.push(footer);
            _self.set("yearPanel", yearPanel);
            _self.set("monthPanel", monthPanel);
        },
        bindUI: function () {
            var _self = this;
            _self.get("monthPanel").on("itemselected",
                function (ev) {
                    if (ev.item) {
                        _self.setInternal("month", ev.item.value);
                    }
                }).on("monthdblclick",
                    function () {
                        _self._successCall();
                    });
            _self.get("yearPanel").on("itemselected",
                function (ev) {
                    if (ev.item) {
                        _self.setInternal("year", ev.item.value);
                    }
                }).on("yeardblclick",
                    function () {
                        _self._successCall();
                    });
        },
        _successCall: function () {
            var _self = this,
                callback = _self.get("success");
            if (callback) {
                callback.call(_self);
            }
        },
        _createFooter: function () {
            var _self = this;
            return new Toolbar.Bar({
                elCls: PREFIX + "clear bui-monthpicker-footer",
                children: [{
                    xclass: "bar-item-button",
                    text: Resource.submit,
                    btnCls: "btn btn-sm btn-primary",
                    handler: function () {
                        _self._successCall();
                    }
                },
                {
                    xclass: "bar-item-button",
                    text: Resource.cancel,
                    btnCls: "btn btn-sm btn-default last",
                    handler: function () {
                        var callback = _self.get("cancel");
                        if (callback) {
                            callback.call(_self);
                        }
                    }
                }]
            });
        },
        _uiSetYear: function (v) {
            this.get("yearPanel").set("year", v);
        },
        _uiSetMonth: function (v) {
            this.get("monthPanel").setSelectedByField(v);
        }
    },
        {
            ATTRS: {
                footer: {},
                align: {
                    value: {}
                },
                year: {},
                success: {
                    value: function () { }
                },
                cancel: {
                    value: function () { }
                },
                width: {
                    value: 180
                },
                month: {},
                yearPanel: {},
                monthPanel: {}
            }
        },
        {
            xclass: "monthpicker"
        });
    BUI.Calendar.MonthPicker = monthPicker;
})(window.BUI, jQuery);
//BUI.Calendar.Calendar
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        CLS_PICKER_TIME = "bui-datepicker-time",
        CLS_PICKER_HOUR = "bui-datepicker-hour",
        CLS_PICKER_MINUTE = "bui-datepicker-minute",
        CLS_PICKER_SECOND = "bui-datepicker-second",
        CLS_TIME_PICKER = "bui-timepicker",
        Picker = BUI.Picker.ListPicker,
        MonthPicker = BUI.Calendar.MonthPicker,
        Header = BUI.Calendar.Header,
        Panel = BUI.Calendar.Panel,
        Toolbar = BUI.Toolbar,
        Component = BUI.Component,
        DateUtil = BUI.Date,
        Resource = BUI.Calendar.Resource;
    function today() {
        var now = new Date();
        return new Date(now.getFullYear(), now.getMonth(), now.getDate());
    }
    function fixedNumber(n) {
        if (n < 10) {
            return "0" + n;
        }
        return n.toString();
    }
    function getNumberItems(end) {
        var items = [];
        for (var i = 0; i < end; i++) {
            items.push({
                text: fixedNumber(i),
                value: fixedNumber(i)
            });
        }
        return items;
    }
    function getTimeUnit(self, cls) {
        var inputEl = self.get("el").find("." + cls);
        return parseInt(inputEl.val(), 10);
    }
    function setTimeUnit(self, cls, val) {
        var inputEl = self.get("el").find("." + cls);
        if (BUI.isNumber(val)) {
            val = fixedNumber(val);
        }
        inputEl.val(val);
    }
    var calendar = Component.Controller.extend({
        initializer: function () {
            var _self = this,
                children = _self.get("children"),
                header = new Header(),
                panel = new Panel(),
                footer = _self.get("footer") || _self._createFooter();
            children.push(header);
            children.push(panel);
            children.push(footer);
            _self.set("header", header);
            _self.set("panel", panel);
            _self.set("footer", footer);
        },
        renderUI: function () {
            var _self = this,
                children = _self.get("children");
            if (_self.get("showTime")) {
                var timepicker = _self.get("timepicker") || _self._initTimePicker();
                children.push(timepicker);
                _self.set("timepicker", timepicker);
            }
        },
        bindUI: function () {
            var _self = this,
                header = _self.get("header"),
                panel = _self.get("panel");
            panel.on("selectedchange",
                function (e) {
                    var date = e.date;
                    if (!DateUtil.isDateEquals(date, _self.get("selectedDate"))) {
                        _self.set("selectedDate", date);
                    }
                });
            if (!_self.get("showTime")) {
                panel.on("click",
                    function () {
                        _self.fire("accept");
                    });
            } else {
                _self._initTimePickerEvent();
            }
            header.on("monthchange",
                function (e) {
                    _self._setYearMonth(e.year, e.month);
                });
            header.on("headerclick",
                function () {
                    var monthPicker = _self.get("monthpicker") || _self._createMonthPicker();
                    monthPicker.set("year", header.get("year"));
                    monthPicker.set("month", header.get("month"));
                    monthPicker.show();
                });
        },
        _initTimePicker: function () {
            var _self = this,
                lockTime = _self.get("lockTime"),
                _timePickerEnum = {
                    hour: CLS_PICKER_HOUR,
                    minute: CLS_PICKER_MINUTE,
                    second: CLS_PICKER_SECOND
                };
            if (lockTime) {
                for (var key in lockTime) {
                    var noCls = _timePickerEnum[key.toLowerCase()];
                    _self.set(key, lockTime[key]);
                    if (!lockTime.editable) {
                        _self.get("el").find("." + noCls).attr("disabled", "");
                    }
                }
            }
            var picker = new Picker({
                elCls: CLS_TIME_PICKER,
                children: [{
                    itemTpl: '<li><a href="#">{text}</a></li>'
                }],
                autoAlign: false,
                align: {
                    node: _self.get("el").find(".bui-calendar-footer"),
                    points: ["tl", "bl"],
                    offset: [-1, 1]
                },
                trigger: _self.get("el").find("." + CLS_PICKER_TIME)
            });
            picker.render();
            _self._initTimePickerEvent(picker);
            return picker;
        },
        _initTimePickerEvent: function (picker) {
            var _self = this,
                picker = _self.get("timepicker");
            if (!picker) {
                return;
            }
            picker.get("el").delegate("a", "click",
                function (ev) {
                    ev.preventDefault();
                });
            picker.on("triggerchange",
                function (ev) {
                    var curTrigger = ev.curTrigger;
                    if (curTrigger.hasClass(CLS_PICKER_HOUR)) {
                        picker.get("list").set("items", getNumberItems(24));
                    } else {
                        picker.get("list").set("items", getNumberItems(60));
                    }
                });
            picker.on("selectedchange",
                function (ev) {
                    var curTrigger = ev.curTrigger,
                        val = ev.value;
                    if (curTrigger.hasClass(CLS_PICKER_HOUR)) {
                        _self.setInternal("hour", val);
                    } else if (curTrigger.hasClass(CLS_PICKER_MINUTE)) {
                        _self.setInternal("minute", val);
                    } else {
                        _self.setInternal("second", val);
                    }
                });
        },
        _setYearMonth: function (year, month) {
            var _self = this,
                selectedDate = _self.get("selectedDate"),
                date = selectedDate.getDate();
            if (year !== selectedDate.getFullYear() || month !== selectedDate.getMonth()) {
                var newDate = new Date(year, month, date);
                if (newDate.getMonth() != month) {
                    newDate = DateUtil.addDay(-1, new Date(year, month + 1));
                }
                _self.set("selectedDate", newDate);
            }
        },
        _createMonthPicker: function () {
            var _self = this,
                monthpicker;
            monthpicker = new MonthPicker({
                render: _self.get("el"),
                visibleMode: "display",
                success: function () {
                    var picker = this;
                    _self._setYearMonth(picker.get("year"), picker.get("month"));
                    picker.hide();
                },
                cancel: function () {
                    this.hide();
                }
            });
            _self.set("monthpicker", monthpicker);
            _self.get("children").push(monthpicker);
            return monthpicker;
        },
        _createFooter: function () {
            var _self = this,
                showTime = this.get("showTime"),
                items = [];
            if (showTime) {
                items.push({
                    content: _self.get("timeTpl")
                });
                items.push({
                    xclass: "bar-item-button",
                    text: Resource.submit,
                    btnCls: "btn btn-sm btn-primary",
                    listeners: {
                        click: function () {
                            _self.fire("accept");
                        }
                    }
                });
            } else {
                items.push({
                    xclass: "bar-item-button",
                    text: Resource.today,
                    btnCls: "btn btn-sm btn-default",
                    id: "todayBtn",
                    listeners: {
                        click: function () {
                            var day = today();
                            _self.set("selectedDate", day);
                            _self.fire("accept");
                        }
                    }
                });
                items.push({
                    xclass: "bar-item-button",
                    text: Resource.clean,
                    btnCls: "btn btn-sm btn-default",
                    id: "clsBtn",
                    listeners: {
                        click: function () {
                            _self.fire("clear");
                        }
                    }
                });
            }
            return new Toolbar.Bar({
                elCls: PREFIX + "calendar-footer",
                children: items
            });
        },
        _updateTodayBtnAble: function () {
            var _self = this;
            if (!_self.get("showTime")) {
                var footer = _self.get("footer"),
                    panelView = _self.get("panel"),
                    now = today(),
                    btn = footer.getChild("todayBtn");
                panelView._isInRange(now) ? btn.enable() : btn.disable();
            }
        },
        _uiSetSelectedDate: function (v) {
            var _self = this,
                year = v.getFullYear(),
                month = v.getMonth();
            _self.get("header").setMonth(year, month);
            _self.get("panel").set("selected", v);
            _self.fire("datechange", {
                date: v
            });
        },
        _uiSetHour: function (v) {
            setTimeUnit(this, CLS_PICKER_HOUR, v);
        },
        _uiSetMinute: function (v) {
            setTimeUnit(this, CLS_PICKER_MINUTE, v);
        },
        _uiSetSecond: function (v) {
            setTimeUnit(this, CLS_PICKER_SECOND, v);
        },
        _uiSetMaxDate: function (v) {
            var _self = this;
            _self.get("panel").set("maxDate", v);
            _self._updateTodayBtnAble();
        },
        _uiSetMinDate: function (v) {
            var _self = this;
            _self.get("panel").set("minDate", v);
            _self._updateTodayBtnAble();
        }
    },
        {
            ATTRS: {
                header: {},
                panel: {},
                maxDate: {},
                minDate: {},
                monthPicker: {},
                timepicker: {},
                width: {
                    value: 180
                },
                events: {
                    value: {
                        click: false,
                        accept: false,
                        datechange: false,
                        monthchange: false
                    }
                },
                showTime: {
                    value: false
                },
                lockTime: {},
                timeTpl: {
                    value: '<input type="text" readonly class="form-control ' + CLS_PICKER_TIME + " " + CLS_PICKER_HOUR + '" />:<input type="text" readonly class="form-control ' + CLS_PICKER_TIME + " " + CLS_PICKER_MINUTE + '" />:<input type="text" readonly class="form-control ' + CLS_PICKER_TIME + " " + CLS_PICKER_SECOND + '" />'
                },
                selectedDate: {
                    value: today()
                },
                hour: {
                    value: new Date().getHours()
                },
                minute: {
                    value: new Date().getMinutes()
                },
                second: {
                    value: 0
                }
            }
        },
        {
            xclass: "calendar",
            priority: 0
        });
    BUI.Calendar.Calendar = calendar;
})(window.BUI, jQuery);
//BUI.Calendar.DatePicker
(function (BUI, $) {
    "use strict";
    var Picker = BUI.Picker.Picker,
        Calendar = BUI.Calendar.Calendar,
        DateUtil = BUI.Date;
    var datepicker = Picker.extend({
        initializer: function () { },
        createControl: function () {
            var _self = this,
                children = _self.get("children"),
                calendar = new Calendar({
                    render: _self.get("el"),
                    showTime: _self.get("showTime"),
                    lockTime: _self.get("lockTime"),
                    minDate: _self.get("minDate"),
                    maxDate: _self.get("maxDate"),
                    autoRender: true
                });
            calendar.on("clear",
                function () {
                    var curTrigger = _self.get("curTrigger"),
                        oldValue = curTrigger.val();
                    if (oldValue) {
                        curTrigger.val("");
                        curTrigger.trigger("change");
                    }
                });
            if (!_self.get("dateMask")) {
                if (_self.get("showTime")) {
                    _self.set("dateMask", "yyyy-mm-dd HH:MM:ss");
                } else {
                    _self.set("dateMask", "yyyy-mm-dd");
                }
            }
            children.push(calendar);
            _self.set("calendar", calendar);
            return calendar;
        },
        setSelectedValue: function (val) {
            if (!this.get("calendar")) {
                return;
            }
            var _self = this,
                calendar = this.get("calendar"),
                date = DateUtil.parse(val, _self.get("dateMask"));
            date = date || _self.get("selectedDate");
            calendar.set("selectedDate", DateUtil.getDate(date));
            if (_self.get("showTime")) {
                var lockTime = this.get("lockTime"),
                    hour = date.getHours(),
                    minute = date.getMinutes(),
                    second = date.getSeconds();
                if (lockTime) {
                    if (!val || !lockTime.editable) {
                        hour = lockTime["hour"] != null ? lockTime["hour"] : hour;
                        minute = lockTime["minute"] != null ? lockTime["minute"] : minute;
                        second = lockTime["second"] != null ? lockTime["second"] : second;
                    }
                }
                calendar.set("hour", hour);
                calendar.set("minute", minute);
                calendar.set("second", second);
            }
        },
        getSelectedValue: function () {
            if (!this.get("calendar")) {
                return null;
            }
            var _self = this,
                calendar = _self.get("calendar"),
                date = DateUtil.getDate(calendar.get("selectedDate"));
            if (_self.get("showTime")) {
                date = DateUtil.addHour(calendar.get("hour"), date);
                date = DateUtil.addMinute(calendar.get("minute"), date);
                date = DateUtil.addSecond(calendar.get("second"), date);
            }
            return date;
        },
        getSelectedText: function () {
            if (!this.get("calendar")) {
                return "";
            }
            return DateUtil.format(this.getSelectedValue(), this._getFormatType());
        },
        _getFormatType: function () {
            return this.get("dateMask");
        },
        _uiSetMaxDate: function (v) {
            if (!this.get("calendar")) {
                return null;
            }
            var _self = this;
            _self.get("calendar").set("maxDate", v);
        },
        _uiSetMinDate: function (v) {
            if (!this.get("calendar")) {
                return null;
            }
            var _self = this;
            _self.get("calendar").set("minDate", v);
        }
    },
        {
            ATTRS: {
                showTime: {
                    value: false
                },
                lockTime: {},
                maxDate: {},
                minDate: {},
                dateMask: {},
                changeEvent: {
                    value: "accept"
                },
                hideEvent: {
                    value: "accept clear"
                },
                calendar: {},
                selectedDate: {
                    value: new Date(new Date().setSeconds(0))
                }
            }
        },
        {
            xclass: "datepicker",
            priority: 0
        });
    BUI.Calendar.DatePicker = datepicker;
})(window.BUI, jQuery);;
///<jscompress sourcefile="12layout.js" />
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
        getExpandedItem: function () {
            return this.getItemBy(function (item) {
                return !item.get("collapsed");
            });
        },
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
//BUI.Layout.BuiItem
(function (BUI, $) {
    "use strict";
    var CLS_COLLAPSED = "bui-collapsed",
        REGINS = {
            NORTH: "north",
            WEST: "west",
            CENTER: "center"
        };

    var BuiItem = function (config) {
        BuiItem.superclass.constructor.call(this, config);
        this.init();
    };

    BuiItem.ATTRS = {
        region: {},
        collapsed: {
            value: false
        },
        cssProperties: {
            value: ["width", "height"]
        },
        leftRange: {
            value: 28
        },
        width: {
            value: 0
        },
        height: {
            value: 0
        },
        layout: {},
        control: {}
    };

    BUI.extend(BuiItem, BUI.Base);

    BUI.augment(BuiItem, {
        init: function () {
            var _self = this,
                el = _self._wrapControl();
            _self.set("el", el);
            _self.syncFit();
        },
        _wrapControl: function () {
            var _self = this,
                control = _self.get("control"),
                controlEl = control.get("el");
            _self.set("region", control.get("region"));
            _self.set("width", control.get("width"));
            return controlEl;
        },
        syncFit: function () {
            var _self = this,
                control = _self.get("control"),
                region = _self.get("region");

            if (region === REGINS.WEST) {
                _self._syncControlHeight(control);
                return;
            }
            if (region === REGINS.CENTER) {
                _self._syncControlWidth(control);
                _self._syncControlHeight(control);
            }
        },
        _syncControlWidth: function (control) {
            var _self = this,
                width = _self.get("width") || _self.get("container").width(),
                appendWidth = control.getAppendWidth(),
                leftWidth = _self.get("layout")._getMiddleLeft();
            control.set("width", width - appendWidth - leftWidth);
        },
        _syncControlHeight: function (control) {
            var _self = this,
                height = _self.getFitHeight(),
                appendHeight = control.getAppendHeight();
            control.set("height", height - appendHeight);
        },
        getFitHeight: function () {
            var _self = this,
                el = _self.get("container"),
                siblings,
                outerHeight = _self.get("height") || el.height();
            return outerHeight;
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
    BuiItem.REGINS = REGINS;
    BUI.Layout.BuiItem = BuiItem;
})(window.BUI, jQuery);
//BUI.Layout.BuiLayout
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        BuiItem = BUI.Layout.BuiItem,
        REGINS = BuiItem.REGINS,
        CLS_TOP = "bui-border-top",
        CLS_MIDDLE = "bui-border-middle",
        CLS_VIEW_CONTAINER = "bui-viewport-container";
    var BuiLayout = Component.Controller.extend({
        initializer: function (control) {
            var _self = this;
            _self.set("control", _self);
        },
        renderUI: function () {
            this._initWraper();
            this.initItems();
            this.reset();
        },
        bindUI: function () {
            var _self = this,
                control = _self.get("control"),
                layoutEvents = _self.get("layoutEvents").join(" ");
            control.on(layoutEvents,
                function () {
                    _self.resetLayout();
                });
            $(window).on("resize", BUI.wrapBehavior(_self, "reset"));
        },
        syncUI: function () {
            this._setMiddleDimension();
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
                cfg = _self.getItemCfg(controlChild);
            return new BuiItem(cfg);
        },
        getItemCfg: function (controlChild) {
            var _self = this;
            return {
                control: controlChild,
                layout: _self,
                container: _self.getItemContainer(controlChild.get("region"))
            };
        },
        getItems: function () {
            return this.get("items");
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
        getItemsByRegion: function (region) {
            return this.getItemsBy(function (item) {
                return item.get("region") === region;
            });
        },
        _fitMiddleControl: function () {
            var _self = this,
                items = _self.getItems();
            BUI.each(items,
                function (item) {
                    var region = item.get("region");
                    if (region == REGINS.WEST || region == REGINS.CENTER) {
                        item.syncFit();
                    }
                });
        },
        _setMiddleDimension: function () {
            var _self = this,
                middleEl = _self.get("middleEl"),
                middleHeight = _self._getMiddleHeight(),
                left = _self._getMiddleLeft(),
                items = _self.get("items"),
                center = _self.getItemsByRegion("center")[0];
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
                westItems = _self.getItemsByRegion("west"),
                leftWidth = 0;
            BUI.each(westItems,
                function (item) {
                    leftWidth += item.get("el").outerWidth();
                });
            return leftWidth;
        },
        resetLayout: function () {
            var _self = this,
                items = _self.getItems();
            BUI.each(items,
                function (item) {
                    item.syncFit();
                });
            _self._setMiddleDimension();
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
        }
    }, {
            ATTRS: {
                layoutEvents: {
                    value: ["afterWidthChange", "afterHeightChange"]
                },
                items: {},
                render: {
                    value: "body"
                }
            }
        }, {
            xclass: "bui-layout"
        });

    BUI.Layout.BuiLayout = BuiLayout;
})(window.BUI, jQuery);;
///<jscompress sourcefile="13tooltip.js" />
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
                elCls: {
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
})(window.BUI, jQuery);;
///<jscompress sourcefile="14tree.js" />
//BUI.Tree.TreeList
(function (BUI, $) {
    "use strict";
    BUI.Tree = {};

    var List = BUI.List,
        Data = BUI.Data,
        EXPAND = "expanded",
        LOADING = "loading",
        CHECKED = "checked",
        PARTIAL_CHECKED = "partial-checked",
        MAP_TYPES = {
            NONE: "none",
            ALL: "all",
            CUSTOM: "custom",
            ONLY_LEAF: "onlyLeaf"
        },
        CLS_ICON = "bui-tree-icon",
        CLS_ELBOW = "bui-tree-elbow",
        CLS_SHOW_LINE = "bui-tree-show-line",
        CLS_ICON_PREFIX = CLS_ELBOW + "-",
        CLS_ICON_WRAPER = CLS_ICON + "-wraper",
        CLS_LINE = CLS_ICON_PREFIX + "line",
        CLS_END = CLS_ICON_PREFIX + "end",
        CLS_EMPTY = CLS_ICON_PREFIX + "empty",
        CLS_EXPANDER = CLS_ICON_PREFIX + "expander",
        CLS_CHECKBOX = CLS_ICON + "-checkbox",
        CLS_RADIO = CLS_ICON + "-radio",
        CLS_EXPANDER_END = CLS_EXPANDER + "-end";

    function makeSureNode(self, node) {
        if (BUI.isString(node)) {
            node = self.findNode(node);
        }
        return node;
    }

    var TreeList = List.SimpleList.extend({
        bindUI: function () {
            var _self = this,
                el = _self.get("el"),
                multipleCheck = _self.get("multipleCheck"),
                store = _self.get("store");
            _self.on("itemclick",
                function (ev) {
                    var sender = $(ev.domTarget),
                        element = ev.element,
                        node = ev.item;
                    if (sender.hasClass(CLS_EXPANDER)) {
                        _self._toggleExpand(node, element);
                        return false;
                    } else if (sender.hasClass(CLS_CHECKBOX)) {
                        var checked = _self.isChecked(node);
                        _self.setNodeChecked(node, !checked);
                        _self.fire("checkedchangeonce", {
                            node: node,
                            checked: !checked
                        });
                    } else if (sender.hasClass(CLS_RADIO)) {
                        _self.setNodeChecked(node, true);
                    }
                });
            _self.on("itemrendered",
                function (ev) {
                    var node = ev.item,
                        element = ev.domTarget;
                    _self._resetIcons(node, element);
                    if (_self.isCheckable(node) && multipleCheck && _self.get("cascadeCheckd")) {
                        _self._resetPatialChecked(node, null, null, element);
                    }
                    if (_self._isExpanded(node, element)) {
                        _self._showChildren(node);
                    }
                });
            _self._initExpandEvent();

            if (!store) {
                return;
            }
            store.on("load",
                function (e) {
                    _self.onLoad(e);
                });
            store.on("add",
                function (e) {
                    _self.onAdd(e);
                });
            store.on("remove",
                function (e) {
                    _self.onRemove(e);
                });
            store.on("update",
                function (e) {
                    _self.onUpdate(e);
                });
        },
        onLoad: function (e) {
            var _self = this,
                store = _self.get("store"),
                root = store.get("root"),
                node;
            if (!e || e.node == root) {
                _self._initRoot();
            }
            if (e && e.node) {
                _self._loadNode(e.node);
            }
        },
        _initRoot: function () {
            var _self = this,
                store = _self.get("store"),
                root,
                showRoot = _self.get("showRoot"),
                nodes;
            if (store) {
                root = store.get("root");
                _self.setInternal("root", root);
                if (showRoot) {
                    nodes = [root];
                } else {
                    nodes = root.children;
                }
                BUI.each(nodes,
                    function (subNode) {
                        _self._initChecked(subNode, true);
                    });
                _self.clearItems();
                _self.addItems(nodes);
            }
        },
        _initChecked: function (node, deep) {
            var _self = this,
                checkType = _self.get("checkType"),
                checkedField = _self.get("checkedField"),
                multipleCheck = _self.get("multipleCheck"),
                checkableField = _self.get("checkableField"),
                cascadeCheckd = _self.get("cascadeCheckd"),
                parent;
            if (checkType === MAP_TYPES.NONE) {
                node[checkableField] = false;
                node[checkedField] = false;
                return;
            }
            if (checkType === MAP_TYPES.ONLY_LEAF) {
                if (node.leaf) {
                    node[checkableField] = true;
                } else {
                    node[checkableField] = false;
                    node[checkedField] = false;
                    if (deep) {
                        BUI.each(node.children,
                            function (subNode) {
                                _self._initChecked(subNode, deep);
                            });
                    }
                }
                return;
            }
            if (checkType === MAP_TYPES.CUSTOM) {
                if (node[checkableField] == null) {
                    node[checkableField] = node[checkedField] != null;
                }
            }
            if (checkType === MAP_TYPES.ALL) {
                node[checkableField] = true;
            }
            if (!node || !_self.isCheckable(node)) {
                return;
            }
            parent = node.parent;
            if (!_self.isChecked(node) && cascadeCheckd) {
                if (parent && _self.isChecked(parent)) {
                    if (multipleCheck || !_self._hasChildChecked(parent)) {
                        _self.setStatusValue(node, "checked", true);
                    }
                }
                if (node.children && node.children.length && _self._isAllChildrenChecked(node) || !multipleCheck && _self._hasChildChecked(node)) {
                    _self.setStatusValue(node, "checked", true);
                }
            }
            if (deep) {
                BUI.each(node.children,
                    function (subNode) {
                        _self._initChecked(subNode, deep);
                    });
            }
        },
        isCheckable: function (node) {
            return node[this.get("checkableField")];
        },
        isChecked: function (node) {
            if (!node) {
                return false;
            }
            return !!node[this.get("checkedField")];
        },
        _hasChildChecked: function (node) {
            if (!node || node.leaf) {
                return false;
            }
            var _self = this;
            return _self.getCheckedNodes(node).length != 0;
        },
        getCheckedNodes: function (parent) {
            var _self = this,
                store = _self.get("store");
            return store.findNodesBy(function (node) {
                return _self.isChecked(node);
            },
                parent);
        },
        _isAllChildrenChecked: function (node) {
            if (!node || node.leaf) {
                return false;
            }
            var _self = this,
                children = node.children,
                rst = true;
            BUI.each(children,
                function (subNode) {
                    rst = rst && _self.isChecked(subNode);
                    if (!rst) {
                        return false;
                    }
                });
            return rst;
        },
        _loadNode: function (node) {
            var _self = this;
            _self._initChecked(node, true);
            _self.expandNode(node);
            _self._updateIcons(node);
            _self.setItemStatus(node, LOADING, false);
        },
        expandNode: function (node, deep) {
            var _self = this,
                element;
            if (BUI.isString(node)) {
                node = _self.findNode(node);
            }
            if (!node) {
                return;
            }
            if (node.parent && !_self.isExpanded(node.parent)) {
                _self.expandNode(node.parent);
            }
            element = _self.findElement(node);
            _self._expandNode(node, element, deep);
        },
        findNode: function (id, parent) {
            return this.get("store").findNode(id, parent);
        },
        _expandNode: function (node, element, deep) {
            var _self = this,
                accordion = _self.get("accordion"),
                store = _self.get("store");
            if (node.leaf) {
                return;
            }
            if (!_self.hasStatus(node, EXPAND, element)) {
                if (accordion && node.parent) {
                    var slibings = node.parent.children;
                    BUI.each(slibings,
                        function (sNode) {
                            if (sNode != node) {
                                _self.collapseNode(sNode);
                            }
                        });
                }
                if (store && !store.isLoaded(node)) {
                    if (!_self._isLoading(node, element)) {
                        store.loadNode(node);
                    }
                } else if (element) {
                    _self.setItemStatus(node, EXPAND, true, element);
                    _self._showChildren(node);
                    _self.fire("expanded", {
                        node: node,
                        element: element
                    });
                }
            }
            BUI.each(node.children,
                function (subNode) {
                    if (deep || _self.isExpanded(subNode)) {
                        _self.expandNode(subNode, deep);
                    }
                });
        },
        collapseNode: function (node) {
            var _self = this,
                element;
            if (BUI.isString(node)) {
                node = _self.findNode(node);
            }
            if (!node) {
                return;
            }
            element = _self.findElement(node);
            _self._collapseNode(node, element);
        },
        _collapseNode: function (node, element, deep) {
            var _self = this;
            if (node.leaf) {
                return;
            }
            if (_self.hasStatus(node, EXPAND, element)) {
                _self.setItemStatus(node, EXPAND, false, element);
                if (deep) {
                    _self._collapseChildren(node, deep);
                    _self.removeItems(node.children);
                } else {
                    _self._hideChildrenNodes(node);
                }
                _self.fire("collapsed", {
                    node: node,
                    element: element
                });
            }
        },
        _collapseChildren: function (parentNode, deep) {
            var _self = this,
                children = parentNode.children;
            BUI.each(children,
                function (node) {
                    _self.collapseNode(node, deep);
                });
        },
        _hideChildrenNodes: function (node) {
            var _self = this,
                children = node.children,
                elements = [];
            BUI.each(children,
                function (subNode) {
                    var element = _self.findElement(subNode);
                    if (element) {
                        elements.push(element);
                        _self._hideChildrenNodes(subNode);
                    }
                });
            if (_self.get("expandAnimate")) {
                elements = $(elements);
                elements.animate({
                    height: 0
                },
                    function () {
                        _self.removeItems(children);
                    });
            } else {
                _self.removeItems(children);
            }
        },
        _updateIcons: function (node) {
            var _self = this,
                element = _self.findElement(node);
            if (element) {
                _self._resetIcons(node, element);
                if (_self._isExpanded(node, element) && !node.leaf) {
                    BUI.each(node.children,
                        function (subNode) {
                            _self._updateIcons(subNode);
                        });
                }
            }
        },
        _resetIcons: function (node, element) {
            if (!this.get("showIcons")) {
                return;
            }
            var _self = this,
                iconContainer = _self.get("iconContainer"),
                containerEl,
                iconsTpl = _self._getIconsTpl(node);
            $(element).find("." + CLS_ICON_WRAPER).remove();
            containerEl = $(element).find(iconContainer).first();
            if (iconContainer && containerEl.length) {
                $(iconsTpl).prependTo(containerEl);
            } else {
                $(element).prepend($(iconsTpl));
            }
        },
        _getIconsTpl: function (node) {
            var _self = this,
                level = node.level,
                start = _self.get("startLevel"),
                iconWraperTpl = _self.get("iconWraperTpl"),
                icons = [],
                i;
            for (i = start; i < level; i = i + 1) {
                icons.push(_self._getLevelIcon(node, i));
            }
            icons.push(_self._getExpandIcon(node));
            icons.push(_self._getCheckedIcon(node));
            icons.push(_self._getNodeTypeIcon(node));
            return BUI.substitute(iconWraperTpl, {
                icons: icons.join("")
            });
        },
        _getLevelIcon: function (node, level) {
            var _self = this,
                showLine = _self.get("showLine"),
                cls = CLS_EMPTY,
                levelNode;
            if (showLine) {
                if (node.level === level || level == null) {
                    cls = _self._isLastNode(node) ? CLS_END : CLS_ELBOW;
                } else {
                    levelNode = _self._getParentNode(node, level);
                    cls = _self._isLastNode(levelNode) ? CLS_EMPTY : CLS_LINE;
                }
            }
            return _self._getIcon(cls);
        },
        _getExpandIcon: function (node) {
            var _self = this,
                cls = CLS_EXPANDER;
            if (node.leaf) {
                return _self._getLevelIcon(node);
            }
            if (_self._isLastNode(node)) {
                cls = cls + " " + CLS_EXPANDER_END;
            }
            return _self._getIcon(cls);
        },
        _getCheckedIcon: function (node) {
            var _self = this,
                checkable = _self.isCheckable(node),
                cls;
            if (checkable) {
                cls = _self.get("multipleCheck") ? CLS_CHECKBOX : CLS_RADIO;
                return _self._getIcon(cls);
            }
            return "";
        },
        _getNodeTypeIcon: function (node) {
            var _self = this,
                cls = node.cls ? node.cls : node.leaf ? _self.get("leafCls") : _self.get("dirCls");
            return _self._getIcon(cls);
        },
        _getIcon: function (cls) {
            var _self = this,
                iconTpl = _self.get("iconTpl");
            return BUI.substitute(iconTpl, {
                cls: cls
            });
        },
        _getParentNode: function (node, level) {
            var nodeLevel = node.level,
                parent = node.parent,
                i = nodeLevel - 1;
            if (nodeLevel <= level) {
                return null;
            }
            while (i > level) {
                parent = parent.parent;
                i = i - 1;
            }
            return parent;
        },
        _isLastNode: function (node) {
            if (!node) {
                return false;
            }
            if (node == this.get("root")) {
                return true;
            }
            var _self = this,
                parent = node.parent,
                siblings = parent ? parent.children : _self.get("nodes"),
                count;
            count = siblings.length;
            return siblings[count - 1] === node;
        },
        _isExpanded: function (node, element) {
            return this.hasStatus(node, EXPAND, element);
        },
        collapseAll: function () {
            var _self = this,
                elements = _self.getAllElements();
            BUI.each(elements,
                function (element) {
                    var item = _self.getItemByElement(element);
                    if (item) {
                        _self._collapseNode(item, element, true);
                    }
                });
        },
        expandAll: function () {
            var _self = this,
                elements = _self.getAllElements();
            BUI.each(elements,
                function (element) {
                    var item = _self.getItemByElement(element);
                    _self._expandNode(item, element, true);
                });
        },
        expandPath: function (path, async, startIndex) {
            if (!path) {
                return;
            }
            startIndex = startIndex || 0;
            var _self = this,
                store = _self.get("store"),
                preNode,
                node,
                i,
                id,
                arr = path.split(",");
            preNode = _self.findNode(arr[startIndex]);
            for (i = startIndex + 1; i < arr.length; i++) {
                id = arr[i];
                node = _self.findNode(id, preNode);
                if (preNode && node) {
                    _self.expandNode(preNode);
                    preNode = node;
                } else if (preNode && async) {
                    store.load({
                        id: preNode.id
                    },
                        function () {
                            node = _self.findNode(id, preNode);
                            if (node) {
                                _self.expandPath(path, async, i);
                            }
                        });
                    break;
                }
            }
        },
        getCheckedLeaf: function (parent) {
            var _self = this,
                store = _self.get("store");
            return store.findNodesBy(function (node) {
                return node.leaf && _self.isChecked(node);
            },
                parent);
        },
        isItemSelectable: function (item) {
            var _self = this,
                dirSelectable = _self.get("dirSelectable"),
                node = item;
            if (node && !dirSelectable && !node.leaf) {
                return false;
            }
            return true;
        },
        isExpanded: function (node) {
            if (!node || node.leaf) {
                return false;
            }
            var _self = this,
                element;
            if (_self._isRoot(node) && !_self.get("showRoot")) {
                return true;
            }
            if (BUI.isString(node)) {
                item = _self.getItem(node);
            }
            element = _self.findElement(node);
            return this._isExpanded(node, element);
        },
        toggleExpand: function (node) {
            var _self = this,
                element;
            if (BUI.isString(node)) {
                item = _self.getItem(node);
            }
            element = _self.findElement(node);
            _self._toggleExpand(node, element);
        },
        setNodeChecked: function (node, checked, deep) {
            deep = deep == null ? true : deep;
            if (!node) {
                return;
            }
            var _self = this,
                parent, multipleCheck = _self.get("multipleCheck"),
                cascadeCheckd = _self.get("cascadeCheckd"),
                element;
            node = makeSureNode(this, node);
            if (!node) {
                return;
            }
            parent = node.parent;
            if (!_self.isCheckable(node)) {
                return;
            }
            if (_self.isChecked(node) !== checked || _self.hasStatus(node, "checked") !== checked) {
                element = _self.findElement(node);
                if (cascadeCheckd) {
                    if (element) {
                        _self.setItemStatus(node, CHECKED, checked, element);
                        if (multipleCheck) {
                            _self._resetPatialChecked(node, checked, checked, element);
                        } else {
                            if (checked && parent && _self.isChecked(parent) != checked) {
                                _self.setNodeChecked(parent, checked, false);
                            }
                        }
                    } else if (!_self.isItemDisabled(node)) {
                        _self.setStatusValue(node, CHECKED, checked);
                    }
                    if (parent) {
                        if (_self.isChecked(parent) != checked) {
                            _self._resetParentChecked(parent);
                        } else if (multipleCheck) {
                            _self._resetPatialChecked(parent, null, null, null, true);
                        }
                    }
                } else if (!_self.isItemDisabled(node)) {
                    if (element) {
                        _self.setItemStatus(node, CHECKED, checked, element);
                    } else {
                        _self.setStatusValue(node, CHECKED, checked);
                    }
                }
                if (checked && !multipleCheck && (_self.isChecked(parent) || parent == _self.get("root") || !cascadeCheckd)) {
                    var nodes = parent.children;
                    BUI.each(nodes,
                        function (slibNode) {
                            if (slibNode !== node && _self.isChecked(slibNode)) {
                                _self.setNodeChecked(slibNode, false);
                            }
                        });
                }
                _self.fire("checkedchange", {
                    node: node,
                    element: element,
                    checked: checked
                });
            }
            if (!node.leaf && deep && cascadeCheckd) {
                BUI.each(node.children,
                    function (subNode, index) {
                        if (multipleCheck || !checked || !multipleCheck && index == 0) {
                            _self.setNodeChecked(subNode, checked, deep);
                        }
                    });
            }
        },
        setChecked: function (node) {
            this.setNodeChecked(node, true);
        },
        clearAllChecked: function () {
            var _self = this,
                nodes = _self.getCheckedNodes();
            BUI.each(nodes,
                function (node) {
                    _self.setNodeChecked(node, false);
                });
        },
        _resetPatialChecked: function (node, checked, hasChecked, element, upper) {
            if (!node || node.leaf) {
                return true;
            }
            var _self = this,
                hasChecked;
            checked = checked == null ? _self.isChecked(node) : checked;
            if (checked) {
                _self.setItemStatus(node, PARTIAL_CHECKED, false, element);
                return;
            }
            hasChecked = hasChecked == null ? _self._hasChildChecked(node) : hasChecked;
            _self.setItemStatus(node, PARTIAL_CHECKED, hasChecked, element);
            if (upper && node.parent) {
                _self._resetPatialChecked(node.parent, false, hasChecked ? hasChecked : null, null, upper);
            }
        },
        _resetParentChecked: function (parentNode) {
            if (!this.isCheckable(parentNode)) {
                return;
            }
            var _self = this,
                multipleCheck = _self.get("multipleCheck"),
                allChecked = multipleCheck ? _self._isAllChildrenChecked(parentNode) : _self._hasChildChecked(parentNode);
            _self.setStatusValue(parentNode, "checked", allChecked);
            _self.setNodeChecked(parentNode, allChecked, false);
            multipleCheck && _self._resetPatialChecked(parentNode, allChecked, null, null, true);
        },
        _initExpandEvent: function () {
            var _self = this,
                el = _self.get("el"),
                expandEvent = _self.get("expandEvent"),
                collapseEvent = _self.get("collapseEvent");
            function createCallback(methodName) {
                return function (ev) {
                    var sender = $(ev.domTarget),
                        element = ev.element,
                        node = ev.item;
                    if (!sender.hasClass(CLS_EXPANDER)) {
                        _self[methodName](node, element);
                    }
                };
            }
            if (expandEvent == collapseEvent) {
                _self.on(expandEvent, createCallback("_toggleExpand"));
            } else {
                if (expandEvent) {
                    _self.on(expandEvent, createCallback("_expandNode"));
                }
                if (collapseEvent) {
                    _self.on(collapseEvent, createCallback("_collapseNode"));
                }
            }
        },
        _isForceChecked: function (node) {
            var _self = this,
                multipleCheck = _self.get("multipleCheck");
            return multipleCheck ? _self._isAllChildrenChecked() : _isForceChecked();
        },
        _isRoot: function (node) {
            var _self = this,
                store = _self.get("store");
            if (store && store.get("root") == node) {
                return true;
            }
            return false;
        },
        _setLoadStatus: function (node, element, loading) {
            var _self = this;
            _self.setItemStatus(node, LOADING, loading, element);
        },
        _addNode: function (node, index) {
            var _self = this,
                parent = node.parent,
                scount, prevNode, nextNode, cIndex;
            _self._initChecked(node, true);
            if (parent) {
                if (_self.isExpanded(parent)) {
                    scount = parent.children.length;
                    cIndex = _self._getInsetIndex(node);
                    _self.addItemAt(node, cIndex);
                    if (index == scount - 1 && index > 0) {
                        prevNode = parent.children[index - 1];
                        _self._updateIcons(prevNode);
                    }
                }
                _self._updateIcons(parent);
            } else {
                cIndex = _self._getInsetIndex(node);
                _self.addItemAt(node, cIndex);
                prevNode = _self.get("nodes")[index - 1];
                _self._updateIcons(prevNode);
            }
        },
        _getInsetIndex: function (node) {
            var _self = this,
                nextNode, rst = null;
            nextNode = _self._getNextItem(node);
            if (nextNode) {
                return _self.indexOfItem(nextNode);
            }
            return _self.getItemCount();
        },
        _getNextItem: function (item) {
            var _self = this,
                parent = item.parent,
                slibings, cIndex, rst = null;
            if (!parent) {
                return null;
            }
            slibings = parent.children;
            cIndex = BUI.Array.indexOf(item, slibings);
            rst = slibings[cIndex + 1];
            return rst || _self._getNextItem(parent);
        },
        onAdd: function (e) {
            var _self = this,
                node = e.node,
                index = e.index;
            _self._addNode(node, index);
        },
        _updateNode: function (node) {
            var _self = this;
            _self.updateItem(node);
            _self._updateIcons(node);
        },
        onUpdate: function (e) {
            var _self = this,
                node = e.node;
            _self._updateNode(node);
        },
        _removeNode: function (node, index) {
            var _self = this,
                parent = node.parent,
                scount, prevNode;
            _self.collapseNode(node);
            if (!parent) {
                return;
            }
            _self.removeItem(node);
            if (_self.isExpanded(parent)) {
                scount = parent.children.length;
                if (scount == index && index !== 0) {
                    prevNode = parent.children[index - 1];
                    _self._updateIcons(prevNode);
                }
            }
            _self._updateIcons(parent);
            _self._resetParentChecked(parent);
        },
        onRemove: function (e) {
            var _self = this,
                node = e.node,
                index = e.index;
            _self._removeNode(node, index);
        },
        _showChildren: function (node) {
            if (!node || !node.children) {
                return;
            }
            var _self = this,
                index = _self.indexOfItem(node),
                length = node.children.length,
                subNode,
                i = length - 1,
                elements = [];
            for (i = length - 1; i >= 0; i--) {
                subNode = node.children[i];
                if (!_self.getItem(subNode)) {
                    if (_self.get("expandAnimate")) {
                        el = _self._addNodeAt(subNode, index + 1);
                        el.hide();
                        el.slideDown();
                    } else {
                        _self.addItemAt(subNode, index + 1);
                    }
                }
            }
        },
        _addNodeAt: function (item, index) {
            var _self = this,
                items = _self.get("items");
            if (index === undefined) {
                index = items.length;
            }
            items.splice(index, 0, item);
            return _self.addItemToView(item, index);
        },
        _isLoading: function (node, element) {
            var _self = this;
            return _self.hasStatus(node, LOADING, element);
        },
        _toggleExpand: function (node, element) {
            var _self = this;
            if (_self._isExpanded(node, element)) {
                _self._collapseNode(node, element);
            } else {
                _self._expandNode(node, element);
            }
        },
        _uiSetShowRoot: function (v) {
            var _self = this,
                start = this.get("showRoot") ? 0 : 1;
            _self.set("startLevel", start);
        },
        _uiSetShowLine: function (v) {
            var _self = this,
                el = _self.get("el");
            if (v) {
                el.addClass(CLS_SHOW_LINE);
            } else {
                el.removeClass(CLS_SHOW_LINE);
            }
        },
        getSelection: function () {
            var _self = this,
                field = _self.getStatusField("selected"),
                store;
            if (field) {
                store = _self.get("store");
                return store.findNodesBy(function (node) {
                    return node[field];
                });
            }
            return List.SimpleList.prototype.getSelection.call(this);
        },
        getSelected: function () {
            var _self = this,
                field = _self.getStatusField("selected"),
                store;
            if (field) {
                store = _self.get("store");
                return store.findNodeBy(function (node) {
                    return node[field];
                });
            }
            return List.SimpleList.prototype.getSelected.call(this);
        }
    },
        {
            ATTRS: {
                store: {
                    value: {}
                },
                root: {},
                iconContainer: {},
                iconWraperTpl: {
                    value: '<span class="' + CLS_ICON_WRAPER + '">{icons}</span>'
                },
                showLine: {
                    value: false
                },
                showIcons: {
                    value: true
                },
                iconTpl: {
                    value: '<span class="bui-tree-icon {cls}"></span>'
                },
                leafCls: {
                    value: CLS_ICON_PREFIX + "leaf"
                },
                dirCls: {
                    value: CLS_ICON_PREFIX + "dir"
                },
                checkType: {
                    share: false,
                    value: "custom"
                },
                cascadeCheckd: {
                    value: true
                },
                accordion: {
                    value: false
                },
                multipleCheck: {
                    value: true
                },
                checkedField: {
                    valueFn: function () {
                        return this.getStatusField("checked");
                    }
                },
                checkableField: {
                    value: "checkable"
                },
                itemStatusFields: {
                    value: {
                        expanded: "expanded",
                        disabled: "disabled",
                        checked: "checked"
                    }
                },
                dirSelectable: {
                    value: true
                },
                showRoot: {
                    value: false
                },
                events: {
                    value: {
                        expanded: false,
                        collapsed: false,
                        checkedchange: false,
                        checkedchangeonce: false
                    }
                },
                expandEvent: {
                    value: "itemdblclick"
                },
                expandAnimate: {
                    value: false
                },
                collapseEvent: {
                    value: "itemdblclick"
                },
                startLevel: {
                    value: 1
                },
                itemCls: {
                    value: BUI.prefix + "tree-item"
                },
                itemTpl: {
                    value: "<li>{text}</li>"
                },
                idField: {
                    value: "id"
                },
                items: {
                    sync: false
                }
            }
        },
        {
            xclass: "tree-list"
        });
    BUI.Tree.TreeList = TreeList;
})(window.BUI, jQuery);;
///<jscompress sourcefile="15tab.js" />
//BUI.Tab.TabPanel
(function (BUI, $) {
    "use strict";
    BUI.Tab = {};
    var List = BUI.List,
        Component = BUI.Component;
    var tabPanel = List.ChildList.extend({
        renderUI: function () {
            var _self = this,
                children = _self.get("children"),
                panelContainer = _self._initPanelContainer(),
                panels = panelContainer.children();
            BUI.each(children,
                function (item, index) {
                    var panel = panels[index];
                    _self._initPanelItem(item, panel);
                });
        },
        _initPanelContainer: function () {
            var _self = this,
                panelContainer = _self.get("panelContainer");
            if (panelContainer && BUI.isString(panelContainer)) {
                if (panelContainer.indexOf("#") == 0) {
                    panelContainer = $(panelContainer);
                } else {
                    panelContainer = _self.get("el").find(panelContainer);
                }
                _self.setInternal("panelContainer", panelContainer);
            }
            return panelContainer;
        },
        _initPanelItem: function (item, panel) {
            var _self = this;
            if (item.set) {
                if (!item.get("panel")) {
                    panel = panel || _self._getPanel(item.getAttrVals());
                    item.set("panel", panel);
                }
            } else {
                if (!item.panel) {
                    panel = panel || _self._getPanel(item);
                    item.panel = panel;
                }
            }
        },
        _getPanel: function (item) {
            var _self = this,
                panelContainer = _self.get("panelContainer"),
                panelTpl = BUI.substitute(_self.get("panelTpl"), item);
            return $(panelTpl).appendTo(panelContainer);
        }
    },
        {
            ATTRS: {
                elTagName: {
                    value: "div"
                },
                childContainer: {
                    value: "ul"
                },
                tpl: {
                    value: '<div class="bui-tab"><ul class="bui-tab-title"></ul><div class="bui-tab-content"></div></div>'
                },
                panelTpl: {
                    value: '<div></div>'
                },
                panelContainer: {
                    value: ".bui-tab-content"
                },
                defaultChildClass: {
                    value: "tab-panel-item"
                }
            }
        },
        {
            xclass: "tab-panel"
        });
    BUI.Tab.TabPanel = tabPanel;
})(window.BUI, jQuery);
//BUI.Tab.TabPanelItem
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        Close = Component.UIBase.Close,
        CLS_TITLE = "bui-tab-item-text";
    var item = Component.Controller.extend({
        renderUI: function () {
            this._resetPanelVisible();
        },
        bindUI: function () {
            var _self = this,
                el = _self.get("el"),
                eventName = _self._getVisibleEvent();
            _self.on(eventName,
                function (ev) {
                    _self._setPanelVisible(ev.newVal);
                });
        },
        _uiSetTitle: function (v) {
            var _self = this,
                el = _self.get("el"),
                titleEl = el.find("." + CLS_TITLE);
            titleEl.text(v);
        },
        _resetPanelVisible: function () {
            var _self = this,
                status = _self.get("panelVisibleStatus"),
                visible = _self.get(status);
            _self._setPanelVisible(visible);
        },
        _getVisibleEvent: function () {
            var _self = this,
                status = _self.get("panelVisibleStatus");
            return "after" + BUI.ucfirst(status) + "Change";
        },
        _setPanelVisible: function (visible) {
            var _self = this,
                panel = _self.get("panel"),
                method = visible ? "show" : "hide";
            if (panel) {
                $(panel)[method]();
            }
        },
        _uiSetSelected: function (v) {
            var _self = this,
                cls = _self.getStatusCls("selected"),
                el = _self.get("el");
            if (v) {
                el.addClass(cls);
            } else {
                el.removeClass(cls);
            }
        },
        _setPanelContent: function (panel, content) {
            $(panel).html(content);
        },
        _uiSetPanelContent: function (v) {
            var _self = this,
                panel = _self.get("panel");
            _self._setPanelContent(panel, v);
        },
        _uiSetPanel: function (v) {
            var _self = this,
                content = _self.get("panelContent");
            if (content) {
                _self._setPanelContent(v, content);
            }
            _self._resetPanelVisible();
        }
    },
        {
            ATTRS: {
                selected: {
                    sync: true,
                    value: false
                },
                title: {
                    sync: false
                },
                elTagName: {
                    value: "li"
                },
                tpl: {
                    value: '<span class="' + CLS_TITLE + '">{title}</span>'
                },
                selectable: {
                    value: true
                },
                panel: {},
                panelContent: {},
                panelVisibleStatus: {
                    value: "selected"
                }
            }
        },
        {
            xclass: "tab-panel-item"
        });
    BUI.Tab.TabPanelItem = item;
})(window.BUI, jQuery);
//BUI.Tab.NavTabItem
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        CLS_ITEM_CLOSE = "bui-tab-close",
        CLS_NAV_ACTIVED = "bui-tab-nav-item-selected",
        CLS_CONTENT = "tab-content";
    var navTabItem = Component.Controller.extend({
        createDom: function () {
            var _self = this,
                parent = _self.get("parent");
            if (parent) {
                _self.set("tabContentContainer", parent.getTabContentContainer());
            }
        },
        renderUI: function () {
            var _self = this,
                contentContainer = _self.get("tabContentContainer"),
                contentTpl = _self.get("tabContentTpl");
            if (contentContainer) {
                var tabContentEl = $(contentTpl).appendTo(contentContainer);
                _self.set("tabContentEl", tabContentEl);
            }
        },
        bindUI: function () {
            var _self = this,
                el = _self.get("el");
            el.on("click",
                function (ev) {
                    var sender = $(ev.target);
                    if (sender.hasClass(CLS_ITEM_CLOSE)) {
                        if (_self.fire("closing") !== false) {
                            _self.close();
                        }
                    } else {
                        _self.fire("itemclick");
                    }
                });
        },
        destructor: function () {
            var _self = this,
                tabContentEl = _self.get("tabContentEl");
            if (tabContentEl) {
                tabContentEl.remove();
            }
        },
        setTabContentVisible: function (v) {
            var _self = this,
                tabContentEl = _self.get("tabContentEl");
            if (tabContentEl) {
                if (v) {
                    tabContentEl.show();
                } else {
                    tabContentEl.hide();
                }
            }
        },
        close: function () {
            this.fire("closed");
        },
        _uiSetHref: function (href) {
            var _self = this, tabContentEl = _self.get("tabContentEl");
            href = href || _self.get("href");
            if (tabContentEl) {
                $("iframe", tabContentEl).attr("src", href);
            }
        },
        _uiSetSelected: function (v) {
            var _self = this,
                parent = _self.get("parent"),
                el = _self.get("el");
            _self.setTabContentVisible(v);
            if (v) {
                el.addClass(CLS_NAV_ACTIVED);
            } else {
                el.removeClass(CLS_NAV_ACTIVED);
            }
        },
        _uiSetCloseable: function (v) {
            var _self = this,
                el = _self.get("el"),
                closeEl = el.find("." + CLS_ITEM_CLOSE);
            if (v) {
                closeEl.show();
            } else {
                closeEl.hide();
            }
        }
    },
        {
            ATTRS: {
                elTagName: {
                    value: "li"
                },
                selected: {
                    value: false
                },
                closeable: {
                    value: true
                },
                href: {
                    value: ""
                },
                events: {
                    value: {
                        click: true,
                        closing: true,
                        closed: true,
                        itemclick: true
                    }
                },
                tabContentContainer: {},
                tabContentTpl: {
                    value: '<div class="' + CLS_CONTENT + '" style="display:none;"><iframe src="" width="100%" height="100%" frameborder="0"></iframe></div>'
                },
                visible: {
                    value: true
                },
                title: {
                    value: ""
                },
                tpl: {
                    value: '<span>{title}<i class="iconfont icon-close bui-tab-close"></i></span>'
                }
            }
        },
        {
            xclass: "nav-tab-item",
            priority: 0
        });
    BUI.Tab.NavTabItem = navTabItem;
})(window.BUI, jQuery);
//BUI.Tab.NavTab
(function (BUI, $) {
    "use strict";
    var Component = BUI.Component,
        CLS_NAV_LIST = "bui-tab-title";
    var navTab = Component.Controller.extend({
        addTab: function (config, reload) {
            var _self = this,
                id = config.id || BUI.guid("tab-item"),
                item = _self.getItemById(id);
            if (item) {
                _self._setItemSelected(item);
            } else {
                config = BUI.mix({
                    id: id,
                    visible: true,
                    seleted: true,
                    xclass: "nav-tab-item"
                },
                    config);
                _self.addChild(config);
                item = _self.getItemById(id);
                _self._setItemSelected(item);
            }
            return item;
        },
        getTabContentContainer: function () {
            return this.get("el").find(".bui-tab-content");
        },
        getContentElement: function () {
            var _self = this,
                listEl = _self.get("listEl");
            if (!listEl) {
                var el = _self.get("el");
                listEl = el.find("." + CLS_NAV_LIST);
                _self.setInternal("listEl", listEl);
            }

            return listEl;
        },
        bindUI: function () {
            var _self = this;
            _self.on("itemclick",
                function (ev) {
                    var item = ev.target;
                    _self._setItemSelected(item);
                });
            _self.on("closed",
                function (ev) {
                    var item = ev.target;
                    _self._closeItem(item);
                });
        },
        setSelected: function (id) {
            var _self = this,
                item = _self.getItemById(id);
            _self._setItemSelected(item);
        },
        getSelectedItem: function () {
            var _self = this,
                children = _self.get("children"),
                result = null;
            BUI.each(children,
                function (item) {
                    if (item.get("selected")) {
                        result = item;
                        return false;
                    }
                });
            return result;
        },
        getItemById: function (id) {
            var _self = this,
                children = _self.get("children"),
                result = null;
            BUI.each(children,
                function (item) {
                    if (item.get("id") === id) {
                        result = item;
                        return false;
                    }
                });
            return result;
        },
        _closeItem: function (item) {
            var _self = this,
                index = _self._getIndex(item),
                selectedItem = _self.getSelectedItem(),
                preItem = _self.get("preItem") || _self._getItemByIndex(index - 1),
                nextItem = _self._getItemByIndex(index + 1);

            _self.removeChild(item, true);
            if (selectedItem === item) {
                if (preItem) {
                    _self._setItemSelected(preItem);
                } else {
                    _self._setItemSelected(nextItem);
                }
            }
            return false;
        },
        closeAll: function () {
            var _self = this,
                children = _self.get("children");
            BUI.each(children,
                function (item) {
                    if (item.get("closeable")) {
                        item.close();
                    }
                });
        },
        closeOther: function (curItem) {
            var _self = this,
                children = _self.get("children");
            BUI.each(children,
                function (item) {
                    if (curItem !== item) {
                        item.close();
                    }
                });
        },
        _getItemByIndex: function (index) {
            var _self = this,
                children = _self.get("children");
            return children[index];
        },
        _getIndex: function (item) {
            var _self = this,
                children = _self.get("children");
            return BUI.Array.indexOf(item, children);
        },
        _uiSetHeight: function (v) {
            var _self = this,
                el = _self.get("el"),
                barEl = el.find(".bui-tab-title"),
                containerEl = _self.getTabContentContainer();
            if (v) {
                containerEl.height(v - barEl.height());
            }
            el.height(v);
        },
        _setItemSelected: function (item) {
            var _self = this,
                preSelectedItem = _self.getSelectedItem();
            if (item === preSelectedItem) {
                return;
            }
            if (preSelectedItem) {
                preSelectedItem.set("selected", false);
            }
            _self.set("preItem", preSelectedItem);
            if (item) {
                if (!item.get("selected")) {
                    item.set("selected", true);
                }
                _self.fire("activeChange", {
                    item: item
                });
                _self.fire("selectedchange", {
                    item: item
                });
            }
        },
        syncFit: function () {
            var _self = this,
                render = _self.get("render"),
                pHeight = $(render).height();
            _self.set("height", pHeight - 5);
        }
    },
        {
            ATTRS: {
                defaultChildClass: {
                    value: "nav-tab-item"
                },
                tpl: {
                    value: '<div class="bui-tab"><ul class="bui-tab-title"></ul><div class="bui-tab-content"></div></div>'
                },
                events: {
                    value: {
                        itemclick: false,
                        selectedchange: false
                    }
                }
            }
        },
        {
            xclass: "nav-tab",
            priority: 0
        });
    BUI.Tab.NavTab = navTab;
})(window.BUI, jQuery);;
///<jscompress sourcefile="16slider.js" />
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
})(window.BUI, jQuery);;
///<jscompress sourcefile="17form.js" />
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
                focusable: {
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
                events: {
                    value: {
                        error: false,
                        valid: false,
                        click: false,
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
})(window.BUI, jQuery);;
///<jscompress sourcefile="19grid.js" />
//BUI.Grid.SimpleGrid
(function (BUI, $) {
    "use strict";
    BUI.Grid = {};
    var List = BUI.List,
        Component = BUI.Component,
        UIBase = Component.UIBase,
        PREFIX = BUI.prefix,
        CLS_GRID = PREFIX + "grid",
        CLS_GRID_BORDER = PREFIX + "grid-border",
        CLS_GRID_ROW = CLS_GRID + "-row";
    var simpleGrid = BUI.List.SimpleList.extend({
        renderUI: function () {
            this.setColumns();
        },
        bindUI: function () {
            var _self = this,
                itemCls = _self.get("itemCls"),
                hoverCls = itemCls + "-hover",
                el = _self.get("el");
            el.delegate("." + itemCls, "mouseover",
                function (ev) {
                    var sender = $(ev.currentTarget);
                    sender.addClass(hoverCls);
                }).delegate("." + itemCls, "mouseout",
                    function (ev) {
                        var sender = $(ev.currentTarget);
                        sender.removeClass(hoverCls);
                    });
        },
        showData: function (data) {
            var _self = this;
            _self.clearData();
            BUI.each(data,
                function (record, index) {
                    _self._createRow(record, index);
                });
        },
        clearData: function () {
            var _self = this,
                tbodyEl = _self.get("itemContainer");
            tbodyEl.empty();
        },
        _uiSetColumns: function (columns) {
            var _self = this;
            _self.clearData();
            _self.setColumns(columns);
        },
        setColumns: function (columns) {
            var _self = this,
                headerRowEl = _self.get("headerRowEl");
            columns = columns || _self.get("columns");
            headerRowEl.empty();
            BUI.each(columns,
                function (column) {
                    _self._createColumn(column, headerRowEl);
                });
        },
        _createColumn: function (column, parent) {
            var _self = this,
                columnTpl = BUI.substitute(_self.get("columnTpl"), column);
            $(columnTpl).appendTo(parent);
        },
        getItemTpl: function (record, index) {
            var _self = this,
                columns = _self.get("columns"),
                rowTpl = _self.get("rowTpl"),
                cellsTpl = [],
                rowEl;
            BUI.each(columns,
                function (column) {
                    var dataIndex = column["dataIndex"];
                    cellsTpl.push(_self._getCellTpl(column, dataIndex, record));
                });
            rowTpl = BUI.substitute(rowTpl, {
                cellsTpl: cellsTpl.join("")
            });
            return rowTpl;
        },
        _getCellTpl: function (column, dataIndex, record) {
            var _self = this,
                renderer = column.renderer,
                text = renderer ? renderer(record[dataIndex], record) : record[dataIndex],
                cellTpl = _self.get("cellTpl");
            return BUI.substitute(cellTpl, {
                elCls: column.elCls,
                text: text
            });
        },
        _uiSetInnerBorder: function (v) {
            var _self = this, el = _self.get("el");
            if (v) {
                el.addClass(CLS_GRID_BORDER);
            } else {
                el.removeClass(CLS_GRID_BORDER);
            }
        },
        _uiSetTableCls: function (v) {
            var _self = this,
                tableEl = _self.get("el").find("table");
            tableEl.attr("class", v);
        }
    },
        {
            ATTRS: {
                itemCls: {
                    value: CLS_GRID_ROW
                },
                tableCls: {
                    value: CLS_GRID + "-table"
                },
                columns: {
                    sync: false,
                    value: []
                },
                innerBorder: {
                    value: true
                },
                headerRowEl: {
                    valueFn: function () {
                        var _self = this,
                            thead = _self.get("el").find("thead");
                        return thead.children("tr");
                    }
                },
                itemContainer: {
                    valueFn: function () {
                        return this.get("el").find("tbody");
                    }
                },
                tpl: {
                    value: '<table cellspacing="0" class="{tableCls}" cellpadding="0"><thead><tr></tr></thead><tbody></tbody></table>'
                },
                rowTpl: {
                    value: '<tr class="' + CLS_GRID_ROW + '">{cellsTpl}</tr>'
                },
                cellTpl: {
                    value: '<td class="' + CLS_GRID + '-cell {elCls}"><div class="' + CLS_GRID + '-cell-inner">{text}</div></td>'
                },
                columnTpl: {
                    value: '<th class="' + CLS_GRID + '-hd {elCls}" width="{width}"><div class="' + CLS_GRID + '-hd-inner">{title}</div></th>'
                },
                events: {
                    value: {}
                }
            }
        },
        {
            xclass: "simple-grid"
        });
    BUI.Grid.SimpleGrid = simpleGrid;
})(window.BUI, jQuery);
//BUI.Grid.Column
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        CLS_HD_TITLE = PREFIX + "grid-hd-title",
        SORT_PREFIX = "sort-",
        SORT_ASC = "ASC",
        SORT_DESC = "DESC";
    var column = BUI.Component.Controller.extend({
        setTplContent: function (attrs) {
            var _self = this,
                sortTpl = _self.get("sortTpl"),
                el = _self.get("el"),
                titleEl;
            var content = _self.get("content"),
                tpl = _self.getTpl(attrs);
            if (!content && tpl) {
                el.empty();
                el.html(tpl);
            }
            titleEl = el.find("." + CLS_HD_TITLE);
            $(sortTpl).insertAfter(titleEl);
        },
        _toggleSortState: function () {
            var _self = this,
                sortState = _self.get("sortState"),
                v = sortState ? sortState === SORT_ASC ? SORT_DESC : SORT_ASC : SORT_ASC;
            _self.set("sortState", v);
        },
        _uiSetWidth: function (v) {
            if (v) {
                this.get("el").css("width", v + "px");
                this.set("originWidth", v);
            }
        },
        _uiSetTitle: function (title) {
            if (!this.get("rendered")) {
                return;
            }
            this.setTplContent();
        },
        _uiSetDraggable: function (v) {
            if (!this.get("rendered")) {
                return;
            }
            this.setTplContent();
        },
        _uiSetSortable: function (v) {
            if (!this.get("rendered")) {
                return;
            }
            this.setTplContent();
        },
        _uiSetTpl: function (v) {
            if (!this.get("rendered")) {
                return;
            }
            this.setTplContent();
        },
        _uiSetSortState: function (v) {
            var _self = this,
                el = _self.get("el"),
                method = v ? "addClass" : "removeClass",
                ascCls = SORT_PREFIX + "asc",
                desCls = SORT_PREFIX + "desc";
            el.removeClass(ascCls + " " + desCls);
            if (v === "ASC") {
                el.addClass(ascCls);
            } else if (v === "DESC") {
                el.addClass(desCls);
            }
        }
    },
        {
            ATTRS: {
                elTagName: {
                    value: "th"
                },
                sortTpl: {
                    getter: function () {
                        var _self = this,
                            sortable = _self.get("sortable");
                        if (sortable) {
                            return '<span class="' + PREFIX + 'grid-sort-icon">&nbsp;</span>';
                        }
                        return "";
                    }
                },
                dataIndex: {
                    value: ""
                },
                draggable: {
                    sync: false,
                    value: true
                },
                editor: {},
                focusable: {
                    value: false
                },
                fixed: {
                    value: false
                },
                id: {},
                visible: {},
                renderer: {},
                resizable: {
                    value: true
                },
                sortable: {
                    sync: false,
                    value: true
                },
                sortState: {
                    value: null
                },
                title: {
                    sync: false,
                    value: "&#160;"
                },
                width: {
                    value: 180
                },
                tpl: {
                    sync: false,
                    value: '<div class="' + PREFIX + 'grid-hd-inner">' + '<span class="' + CLS_HD_TITLE + '">{title}</span>' + "</div>"
                },
                events: {
                    value: {
                        afterWidthChange: true,
                        afterSortStateChange: true,
                        afterVisibleChange: true,
                        click: true,
                        resize: true,
                        move: true
                    }
                }
            }
        },
        {
            xclass: "grid-hd",
            priority: 1
        });
    column.Empty = column.extend({},
        {
            ATTRS: {
                type: {
                    value: "empty"
                },
                sortable: {
                    value: false
                },
                width: {
                    value: null
                },
                tpl: {
                    value: '<div class="' + PREFIX + 'grid-hd-inner"></div>'
                }
            }
        },
        {
            xclass: "grid-hd-empty",
            priority: 1
        });
    BUI.Grid.Column = column;
})(window.BUI, jQuery);
//BUI.Grid.Header
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        Column = BUI.Grid.Column,
        Controller = BUI.Component.Controller,
        CLS_SCROLL_WITH = 17;
    var header = Controller.extend({
        initializer: function () {
            var _self = this,
                children = _self.get("children"),
                columns = _self.get("columns"),
                emptyColumn;
            $.each(columns,
                function (index, item) {
                    var columnControl = _self._createColumn(item);
                    children[index] = columnControl;
                    columns[index] = columnControl;
                });
            emptyColumn = _self._createEmptyColumn();
            children.push(emptyColumn);
            _self.set("emptyColumn", emptyColumn);
        },
        getContentElement: function () {
            return this.get("el").find("tr");
        },
        addColumn: function (c, index) {
            var _self = this,
                insertIndex = index,
                columns = _self.get("columns");
            c = _self._createColumn(c);
            if (index === undefined) {
                index = columns.length;
                insertIndex = _self.get("children").length - 1;
            }
            columns.splice(index, 0, c);
            _self.addChild(c, insertIndex);
            _self.fire("add", {
                column: c,
                index: index
            });
            return c;
        },
        removeColumn: function (c) {
            var _self = this,
                columns = _self.get("columns"),
                index;
            c = BUI.isNumber(c) ? columns[c] : c;
            index = BUI.Array.indexOf(c, columns);
            columns.splice(index, 1);
            _self.fire("remove", {
                column: c,
                index: index
            });
            return _self.removeChild(c, true);
        },
        bindUI: function () {
            var _self = this;
            _self._bindColumnsEvent();
        },
        getColumns: function () {
            return this.get("columns");
        },
        getColumnsWidth: function () {
            var _self = this,
                columns = _self.getColumns(),
                totalWidth = 0;
            $.each(columns,
                function (index, column) {
                    if (column.get("visible")) {
                        totalWidth += column.get("el").outerWidth();
                    }
                });
            return totalWidth;
        },
        getColumnOriginWidth: function () {
            var _self = this,
                columns = _self.getColumns(),
                totalWidth = 0;
            $.each(columns,
                function (index, column) {
                    if (column.get("visible")) {
                        var width = column.get("originWidth") || column.get("width");
                        totalWidth += width;
                    }
                });
            return totalWidth;
        },
        getColumnByIndex: function (index) {
            var _self = this,
                columns = _self.getColumns(),
                result = columns[index];
            return result;
        },
        getColumn: function (func) {
            var _self = this,
                columns = _self.getColumns(),
                result = null;
            $.each(columns,
                function (index, column) {
                    if (func(column)) {
                        result = column;
                        return false;
                    }
                });
            return result;
        },
        getColumnById: function (id) {
            var _self = this;
            return _self.getColumn(function (column) {
                return column.get("id") === id;
            });
        },
        getColumnIndex: function (column) {
            var _self = this,
                columns = _self.getColumns();
            return BUI.Array.indexOf(column, columns);
        },
        scrollTo: function (obj) {
            var _self = this,
                el = _self.get("el");
            if (obj.top !== undefined) {
                el.scrollTop(obj.top);
            }
            if (obj.left !== undefined) {
                el.scrollLeft(obj.left);
            }
        },
        _bindColumnsEvent: function () {
            var _self = this;
            _self.on("afterWidthChange",
                function (e) {
                    var sender = e.target;
                    if (sender !== _self) {
                        _self.setTableWidth();
                    }
                });
            _self.on("afterVisibleChange",
                function (e) {
                    var sender = e.target;
                    if (sender !== _self) {
                        _self.setTableWidth();
                    }
                });
            _self.on("afterSortStateChange",
                function (e) {
                    var sender = e.target,
                        columns = _self.getColumns(),
                        val = e.newVal;
                    if (val) {
                        $.each(columns,
                            function (index, column) {
                                if (column !== sender) {
                                    column.set("sortState", "");
                                }
                            });
                    }
                });
            _self.on("add",
                function () {
                    _self.setTableWidth();
                });
            _self.on("remove",
                function () {
                    _self.setTableWidth();
                });
        },
        _createColumn: function (cfg) {
            if (cfg instanceof Column) {
                return cfg;
            }
            if (!cfg.id) {
                cfg.id = BUI.guid("col");
            }
            return new Column(cfg);
        },
        _createEmptyColumn: function () {
            return new Column.Empty();
        },
        setTableWidth: function () {
            var _self = this,
                width = _self.get("width"),
                totalWidth = 0,
                emptyColumn = null;
            if (width == "auto") {
                return;
            }
            totalWidth = _self.getColumnsWidth();
            emptyColumn = _self.get("emptyColumn");
            if (width < totalWidth - 3) {
                emptyColumn.get("el").width(CLS_SCROLL_WITH);
            } else {
                emptyColumn.get("el").width("auto");
            }

        },
        _uiSetWidth: function (w) {
            var _self = this;
            _self.get("el").width(w);
            _self.setTableWidth();
        }
    },
        {
            ATTRS: {
                columns: {
                    value: []
                },
                emptyColumn: {},
                focusable: {
                    value: false
                },
                tpl: {
                    value: '<table cellspacing="0" class="' + PREFIX + 'grid-table" cellpadding="0"><thead><tr></tr></thead></table>'
                },
                events: {
                    value: {
                        add: false,
                        remove: false
                    }
                }
            }
        },
        {
            xclass: "grid-header",
            priority: 1
        });
    BUI.Grid.Header = header;
})(window.BUI, jQuery);
//BUI.Grid.Grid
(function (BUI, $) {
    "use strict";
    var Mask = BUI.Mask,
        Component = BUI.Component,
        toolbar = BUI.Toolbar,
        List = BUI.List,
        Header = BUI.Grid.Header,
        Column = BUI.Grid.Column;
    function isPercent(str) {
        if (BUI.isString(str)) {
            return str.indexOf("%") !== -1;
        }
        return false;
    }
    var PREFIX = BUI.prefix,
        CLS_GRID_HEADER_CONTAINER = PREFIX + "grid-header-container",
        CLS_GRID_BODY = PREFIX + "grid-body",
        CLS_GRID_WITH = PREFIX + "grid-width",
        CLS_GRID_HEIGHT = PREFIX + "grid-height",
        CLS_GRID_BORDER = PREFIX + "grid-border",
        CLS_GRID_TBAR = PREFIX + "grid-tbar",
        CLS_GRID_BBAR = PREFIX + "grid-bbar",
        CLS_BUTTON_BAR = PREFIX + "grid-btn-bar",
        CLS_GRID_ROW = PREFIX + "grid-row",
        CLS_GRID_CELL = PREFIX + "grid-cell",
        CLS_GRID_CELL_INNER = PREFIX + "grid-cell-inner",
        CLS_TD_PREFIX = "grid-td-",
        CLS_CELL_EMPTY = PREFIX + "grid-cell-empty",
        CLS_SCROLL_WITH = "17",
        CLS_HIDE = PREFIX + "hidden",
        ATTR_COLUMN_FIELD = "data-column-field",
        WIDTH_BORDER = 2;
    function getInnerWidth(width) {
        var _self = this;
        if (BUI.isNumber(width)) {
            width -= WIDTH_BORDER;
        }
        return width;
    }
    var grid = List.SimpleList.extend({
        createDom: function () {
            var _self = this,
                render = _self.get("render"),
                outerWidth = $(render).width(),
                width = _self.get("width");
            if (!width && outerWidth) {
                var appendWidth = _self.getAppendWidth();
                _self.set("width", outerWidth - appendWidth);
            } else if (width) {
                _self.set("width", width);
                _self.get("el").addClass(CLS_GRID_WITH);
            }
            if (_self.get("height")) {
                _self.get("el").addClass(CLS_GRID_HEIGHT);
            }
            if (_self.get("innerBorder")) {
                _self.get("el").addClass(CLS_GRID_BORDER);
            }
        },
        renderUI: function () {
            var _self = this,
                el = _self.get("el"),
                bodyEl = el.find("." + CLS_GRID_BODY);
            _self.set("bodyEl", bodyEl);
            _self._setTableTpl();
            _self._initHeader();
            _self._initBars();
            _self.resetHeaderRow();
        },
        _setTableTpl: function (tpl) {
            var _self = this,
                bodyEl = _self.get("bodyEl");
            tpl = tpl || _self.get("tableTpl");
            $(tpl).appendTo(bodyEl);
            var tableEl = bodyEl.find("table"),
                tbodyEl = bodyEl.find("tbody");
            _self.set("tableEl", tableEl);
            _self.set("tbodyEl", tbodyEl);
            _self.set("itemContainer", tbodyEl);
            _self._setTableCls(_self.get("tableCls"));
        },
        _uiSetTableCls: function (v) {
            this._setTableCls(v);
        },
        _setTableCls: function (cls) {
            var _self = this,
                tableEl = _self.get("tableEl");
            tableEl.attr("class", cls);
        },
        resetHeaderRow: function () {
            if (!this.get("useHeaderRow")) {
                return;
            }
            var _self = this,
                headerRowEl = _self.get("headerRowEl"),
                tbodyEl = _self.get("tbodyEl");
            if (headerRowEl) {
                headerRowEl.remove();
            }
            headerRowEl = _self._createHeaderRow();
            headerRowEl.prependTo(tbodyEl);
            _self.set("headerRowEl", headerRowEl);
        },
        _createHeaderRow: function () {
            var _self = this,
                columns = _self._getColumns(),
                tbodyEl = _self.get("tbodyEl"),
                rowTpl = _self.get("headerRowTpl"),
                rowEl,
                cellsTpl = [];
            $.each(columns,
                function (index, column) {
                    cellsTpl.push(_self._getHeaderCellTpl(column));
                });
            if (_self.get("useEmptyCell")) {
                cellsTpl.push(_self._getEmptyCellTpl());
            }
            rowTpl = BUI.substitute(rowTpl, {
                cellsTpl: cellsTpl.join("")
            });
            rowEl = $(rowTpl).appendTo(tbodyEl);
            return rowEl;
        },
        _getColumns: function () {
            return this.get("columns");
        },
        _getHeaderCellTpl: function (column) {
            var _self = this,
                headerCellTpl = _self.get("headerCellTpl"),
                width = column.get("width");
            if (BUI.isNumeric(width)) {
                width = width + "px";
            }
            return BUI.substitute(headerCellTpl, {
                id: column.get("id"),
                width: width,
                hideCls: !column.get("visible") ? CLS_HIDE : ""
            });
        },
        _getEmptyCellTpl: function () {
            return '<td class="' + CLS_GRID_CELL + " " + CLS_CELL_EMPTY + '">&nbsp;</td>';
        },
        getItemTpl: function (record, index) {
            var _self = this,
                columns = _self._getColumns(),
                tbodyEl = _self.get("tbodyEl"),
                rowTpl = _self.get("rowTpl"),
                cellsTpl = [],
                rowEl;
            BUI.each(columns,
                function (column) {
                    var dataIndex = column.get("dataIndex");
                    cellsTpl.push(_self._getCellTpl(column, dataIndex, record, index));
                });
            if (_self.get("useEmptyCell")) {
                cellsTpl.push(_self._getEmptyCellTpl());
            }
            rowTpl = BUI.substitute(rowTpl, {
                cellsTpl: cellsTpl.join("")
            });
            return rowTpl;
        },
        _getCellTpl: function (column, dataIndex, record, index) {
            var _self = this,
                cellText = _self._getCellText(column, record, index) || column.get("cellTpl"),
                cellTpl = _self.get("cellTpl");
            return BUI.substitute(cellTpl, {
                elCls: column.get("elCls"),
                id: column.get("id"),
                dataIndex: dataIndex,
                cellText: cellText,
                hideCls: !column.get("visible") ? CLS_HIDE : ""
            });
        },
        _getCellText: function (column, record, index) {
            try {
                var _self = this,
                    dataIndex = column.get("dataIndex"),
                    renderer = column.get("renderer"),
                    text = renderer ? renderer(record[dataIndex], record, index) : record[dataIndex];
                return text == null ? "" : text;
            } catch (ex) {
                throw "column:" + column.get("title") + " fomat error!";
            }
        },
        bindUI: function () {
            var _self = this, autoFit = _self.get("autoFit"), store = _self.get('store');
            _self._bindHeaderEvent();
            _self._bindBodyEvent();
            _self._bindItemsEvent();
            if (autoFit) {
                _self._bindAutoFit();
            }
            if (!store) {
                return;
            }
            store.on("load",
                function (e) {
                    _self.onLoad(e);
                });
            store.on("add",
                function (e) {
                    _self.onAdd(e);
                });
            store.on("remove",
                function (e) {
                    _self.onRemove(e);
                });
            store.on("update",
                function (e) {
                    _self.onUpdate(e);
                });
        },
        addColumn: function (column, index) {
            var _self = this,
                header = _self.get("header");
            if (header) {
                column = header.addColumn(column, index);
            } else {
                column = new Column(column);
                _self.get("columns").splice(index, 0, column);
            }
            return column;
        },
        clearData: function () {
            this.clearItems();
        },
        getRecords: function () {
            return this.getItems();
        },
        findColumn: function (id) {
            var _self = this,
                header = _self.get("header");
            if (BUI.isNumber(id)) {
                return header.getColumnByIndex(id);
            } else {
                return header.getColumnById(id);
            }
        },
        findColumnByField: function (field) {
            var _self = this,
                header = _self.get("header");
            return header.getColumn(function (column) {
                return column.get("dataIndex") === field;
            });
        },
        findCell: function (id, record) {
            var _self = this,
                rowEl = null;
            if (record instanceof $) {
                rowEl = record;
            } else {
                rowEl = _self.findRow(record);
            }
            if (rowEl) {
                var cls = CLS_TD_PREFIX + id;
                return rowEl.find("." + cls);
            }
            return null;
        },
        findRow: function (record) {
            var _self = this;
            return $(_self.findElement(record));
        },
        removeColumn: function (column) {
            var _self = this;
            _self.get("header").removeColumn(column);
        },
        showData: function (data) {
            var _self = this;
            _self.set("items", data);
        },
        resetColumns: function () {
            var _self = this,
                store = _self.get("store");
            _self.resetHeaderRow();
            if (store) {
                _self.onLoad();
            }
        },
        _bindScrollEvent: function () {
            var _self = this,
                el = _self.get("el"),
                bodyEl = el.find("." + CLS_GRID_BODY),
                header = _self.get("header");
            bodyEl.on("scroll",
                function () {
                    var left = bodyEl.scrollLeft(),
                        top = bodyEl.scrollTop();
                    header.scrollTo({
                        left: left,
                        top: top
                    });
                    _self.fire("scroll", {
                        scrollLeft: left,
                        scrollTop: top,
                        bodyWidth: bodyEl.width(),
                        bodyHeight: bodyEl.height()
                    });
                });
        },
        _bindHeaderEvent: function () {
            var _self = this,
                header = _self.get("header"),
                store = _self.get("store");
            header.on("afterWidthChange",
                function (e) {
                    var sender = e.target;
                    if (sender !== header) {
                        _self.resetColumnsWidth(sender);
                    }
                });
            header.on("afterSortStateChange",
                function (e) {
                    var column = e.target,
                        val = e.newVal;
                    if (val && store) {
                        store.sort(column.get("dataIndex"), column.get("sortState"));
                    }
                });
            header.on("afterVisibleChange",
                function (e) {
                    var sender = e.target;
                    if (sender !== header) {
                        _self.setColumnVisible(sender);
                        _self.fire("columnvisiblechange", {
                            column: sender
                        });
                    }
                });
            header.on("click",
                function (e) {
                    var sender = e.target;
                    if (sender !== header) {
                        _self.fire("columnclick", {
                            column: sender,
                            domTarget: e.domTarget
                        });
                    }
                });
            header.on("add",
                function (e) {
                    if (_self.get("rendered")) {
                        _self.fire("columnadd", {
                            column: e.column,
                            index: e.index
                        });
                        _self.resetColumns();
                    }
                });
            header.on("remove",
                function (e) {
                    if (_self.get("rendered")) {
                        _self.resetColumns();
                        _self.fire("columnremoved", {
                            column: e.column,
                            index: e.index
                        });
                    }
                });
        },
        _bindBodyEvent: function () {
            var _self = this;
            _self._bindScrollEvent();
        },
        _bindItemsEvent: function () {
            var _self = this,
                store = _self.get("store");
            _self.on("itemsshow",
                function () {
                    _self.fire("aftershow");
                });
            _self.on("itemsclear",
                function () {
                    _self.fire("clear");
                });
            _self.on("itemclick",
                function (ev) {
                    var target = ev.domTarget,
                        record = ev.item,
                        cell = $(target).closest("." + CLS_GRID_CELL),
                        rowEl = $(target).closest("." + CLS_GRID_ROW),
                        rst;
                    if (cell.length) {
                        rst = _self.fire("cellclick", {
                            record: record,
                            row: rowEl[0],
                            cell: cell[0],
                            field: cell.attr(ATTR_COLUMN_FIELD),
                            domTarget: target,
                            domEvent: ev.domEvent
                        });
                    }
                    if (rst === false) {
                        return rst;
                    }
                    return _self.fire("rowclick", {
                        record: record,
                        row: rowEl[0],
                        domTarget: target
                    });
                });
            _self.on("itemunselected",
                function (ev) {
                    _self.fire("rowunselected", getEventObj(ev));
                });
            _self.on("itemselected",
                function (ev) {
                    _self.fire("rowselected", getEventObj(ev));
                });
            _self.on("itemrendered",
                function (ev) {
                    _self.fire("rowcreated", getEventObj(ev));
                });
            _self.on("itemremoved",
                function (ev) {
                    _self.fire("rowremoved", getEventObj(ev));
                });
            _self.on("itemupdated",
                function (ev) {
                    _self.fire("rowupdated", getEventObj(ev));
                });
            function getEventObj(ev) {
                return {
                    record: ev.item,
                    row: ev.domTarget,
                    domTarget: ev.domTarget
                };
            }
        },
        _bindAutoFit: function () {
            var _self = this,
                handler;

            $(window).on("resize",
                function () {
                    function autoFit() {
                        clearTimeout(handler);
                        handler = setTimeout(function () {
                            _self._autoFit(grid);
                        }, 400);
                        _self.set("handler", handler);
                    }
                    autoFit();
                });

            //立即执行一次
            handler = setTimeout(function () {
                _self._autoFit(grid);
                clearTimeout(handler);
            }, 400);        
        },
        _getInnerWidth: function (width) {
            width = width || this.get("width");
            return getInnerWidth(width);
        },
        _initHeader: function () {
            var _self = this,
                header = _self.get("header"),
                container = _self.get("el").find("." + CLS_GRID_HEADER_CONTAINER);
            if (!header) {
                header = new Header({
                    columns: _self.get("columns"),
                    tableCls: _self.get("tableCls"),
                    width: _self._getInnerWidth(),
                    render: container,
                    parent: _self
                }).render();
                _self.set("header", header);
            }
        },
        _initBars: function () {
            var _self = this,
                bbar = _self.get("bbar"),
                tbar = _self.get("tbar");
            _self._initBar(bbar, CLS_GRID_BBAR, "bbar");
            _self._initBar(tbar, CLS_GRID_TBAR, "tbar");
        },
        _initBar: function (bar, cls, name) {
            var _self = this,
                store = null,
                pagingBarCfg = null;
            if (bar) {
                if (!bar.xclass && !(bar instanceof Component.Controller)) {
                    bar.xclass = "toolbar";
                    bar.elCls = CLS_BUTTON_BAR;

                    if (bar.pagingBar) {
                        store = _self.get("store");
                        bar = {
                            xclass: "pagingbar",
                            store: store,
                            pageSize: store.pageSize
                        };
                        if (bar.pagingBar !== true) {
                            pagingBarCfg = BUI.merge(pagingBarCfg, bar.pagingBar);
                        }
                    }
                }
                if (bar.xclass) {
                    var barContainer = _self.get("el").find("." + cls);
                    barContainer.show();
                    bar.render = barContainer;
                    bar.elTagName = "div";
                    bar.autoRender = true;
                    bar = _self.addChild(bar);
                }
                _self.set(name, bar);
            }
            return bar;
        },
        _uiSetWidth: function (w) {
            var _self = this;
            _self.get("el").width(w);
            _self.setBodyWidth(_self._getInnerWidth(w));
            _self.get("el").addClass(CLS_GRID_WITH);
            if (_self.get("rendered")) {
                if (!isPercent(w)) {
                    _self.get("header").set("width", _self._getInnerWidth(w));
                } else {
                    _self.get("header").set("width", "100%");
                }
            }
            _self.setTableWidth();
        },
        _uiSetHeight: function (h, obj) {
            var _self = this,
                header = _self.get("header");
            _self.get("el").height(h);
            _self.get("el").addClass(CLS_GRID_HEIGHT);
            _self.setBodyHeight(h);
            if (_self.get("rendered")) {
                header.setTableWidth();
            }
        },
        resetColumnsWidth: function (column, width) {
            var _self = this,
                headerRowEl = _self.get("headerRowEl"),
                cell = _self.findCell(column.get("id"), headerRowEl);
            width = width || column.get("width");
            if (cell) {
                cell.width(width);
            }
            _self.setTableWidth();
        },
        setTableWidth: function (columnsWidth) {
            if (!columnsWidth && isPercent(this.get("width"))) {
                this.get("tableEl").width("100%");
                return;
            }
            var _self = this,
                width = _self._getInnerWidth(),
                height = _self.get("height"),
                tableEl = _self.get("tableEl"),
                headerRowEl = _self.get("headerRowEl");
            //if (!isPercent(columnsWidth)) {
            //    columnsWidth = columnsWidth || _self._getColumnsWidth();
            //    if (!width) {
            //        return;
            //    }
            //    if (width >= columnsWidth) {
            //        columnsWidth = width;
            //        if (height) {
            //            columnsWidth = width - CLS_SCROLL_WITH;
            //        }
            //    }
            //}
            tableEl.width(columnsWidth);
        },
        setBodyWidth: function (width) {
            var _self = this,
                bodyEl = _self.get("bodyEl");
            width = width || _self._getInnerWidth();
            bodyEl.width(width);
        },
        setBodyHeight: function (height) {
            var _self = this,
                bodyEl = _self.get("bodyEl"),
                bodyHeight = height,
                siblings = bodyEl.siblings();
            BUI.each(siblings,
                function (item) {
                    if ($(item).css("display") !== "none") {
                        bodyHeight -= $(item).outerHeight();
                    }
                });
            bodyEl.height(bodyHeight);
        },
        setColumnVisible: function (column) {
            var _self = this,
                hide = !column.get("visible"),
                colId = column.get("id"),
                tbodyEl = _self.get("tbodyEl"),
                cells = $("." + CLS_TD_PREFIX + colId, tbodyEl);
            if (hide) {
                cells.hide();
            } else {
                cells.show();
            }
        },
        updateItem: function (record) {
            var _self = this,
                items = _self.getItems(),
                index = BUI.Array.indexOf(record, items),
                columns = _self._getColumns(),
                element = null,
                tpl;
            if (index >= 0) {
                element = _self.findElement(record);
                BUI.each(columns,
                    function (column) {
                        var cellEl = _self.findCell(column.get("id"), $(element)),
                            textTpl = _self._getCellText(column, record, index);
                        var tpl = '<div class="' + CLS_GRID_CELL_INNER + '">' + textTpl + '</div>';
                        cellEl.html(tpl);
                    });
                return element;
            }
        },
        showEmptyText: function () {
            var _self = this,
                bodyEl = _self.get("bodyEl"),
                emptyDataTpl = _self.get("emptyDataTpl"),
                emptyEl = _self.get("emptyEl");
            if (emptyEl) {
                emptyEl.remove();
            }
            var emptyEl = $(emptyDataTpl).appendTo(bodyEl);
            _self.set("emptyEl", emptyEl);
        },
        clearEmptyText: function () {
            var _self = this,
                emptyEl = _self.get("emptyEl");
            if (emptyEl) {
                emptyEl.remove();
            }
        },
        _getColumnsWidth: function () {
            var _self = this,
                columns = _self.get("columns"),
                totalWidth = 0;
            BUI.each(columns,
                function (column) {
                    if (column.get("visible")) {
                        totalWidth += column.get("el").outerWidth();
                    }
                });
            return totalWidth;
        },
        _uiSetInnerBorder: function (v) {
            var _self = this,
                el = _self.get("el");
            if (v) {
                el.addClass(CLS_GRID_BORDER);
            } else {
                el.removeClass(CLS_GRID_BORDER);
            }
        },
        _autoFit: function () {
            var _self = this,
                render = _self.get('render'),
                appendWidth = 2,
                appendheight = 20,
                parentHeight = 0,
                parentWidth=0,
                parent = $(render).parent(),
                siblings = $(render).siblings();
            if (parent.is("body")) {
                parentHeight = BUI.viewportHeight();
                parentWidth = BUI.viewportWidth();
            } else {
                parentHeight = parent.height();
                parentWidth = parent.width();
            }
            BUI.each(siblings,
                function (elem) {
                    if (elem.nodeName == 'SCRIPT' || elem.nodeName == 'script') {
                        return;
                    }
                    var node = $(elem);
                    if (node.css("position") !== "absolute") {
                        appendheight += node.outerHeight();
                    }
                });
            _self.set('width', parentWidth - appendWidth);
            _self.set('height', parentHeight - appendheight);
        },
        onAdd: function (e) {
            var _self = this,
                store = _self.get('store'),
                item = e.record;
            if (_self.getCount() == 0) { //初始为空时，列表跟Store不同步
                _self.setItems(store.getResult());
            } else {
                _self.addItemToView(item, e.index);
            }

        },
        onRemove: function (e) {
            var _self = this,
                item = e.record;
            _self.removeItem(item);
        },
        onUpdate: function (e) {
            this.updateItem(e.record);
        },
        onLocalSort: function (e) {
            if (this.get('frontSortable')) {
                this.sort(e.field, e.direction);
            } else {
                this.onLoad(e);
            }
        },
        onLoad: function () {
            var _self = this,
                store = _self.get("store"),
                items = store.getResult();
            _self.set('items', items);
            if (_self.get("emptyDataTpl")) {
                if (store && store.getCount() == 0) {
                    _self.showEmptyText();
                } else {
                    _self.clearEmptyText();
                }
            }
        }
    },
        {
            ATTRS: {
                store: {
                    value: {}
                },
                header: {},
                bbar: {},
                itemCls: {
                    value: CLS_GRID_ROW
                },
                columns: {
                    value: []
                },
                items: {},
                emptyDataTpl: {},
                headerRowTpl: {
                    value: '<tr class="' + PREFIX + 'grid-header-row">{cellsTpl}</tr>'
                },
                headerCellTpl: {
                    value: '<td class="{hideCls} ' + CLS_TD_PREFIX + '{id}" style="width: {width}; height: 0;"></td>'
                },
                rowTpl: {
                    value: '<tr class="' + CLS_GRID_ROW + ' {oddCls}">{cellsTpl}</tr>'
                },
                cellTpl: {
                    value: '<td  class="{elCls} {hideCls} ' + CLS_GRID_CELL + " " + CLS_TD_PREFIX + '{id}" data-column-id="{id}" data-column-field = "{dataIndex}" >' + '<div class="' + CLS_GRID_CELL_INNER + '" >{cellText}</div>' + "</td>"
                },
                events: {
                    value: {
                        aftershow: false,
                        clear: false,
                        cellclick: false,
                        columnclick: false,
                        rowclick: false,
                        rowcreated: false,
                        rowremoved: false,
                        rowselected: false,
                        rowunselected: false,
                        scroll: false
                    }
                },
                tbar: {},
                tableCls: {
                    sync: false,
                    value: PREFIX + "grid-table"
                },
                tableTpl: {
                    value: '<table cellspacing="0" cellpadding="0" ><tbody></tbody></table>'
                },
                tpl: {
                    value: '<div class="' + CLS_GRID_TBAR + '" style="display:none"></div><div class="' + CLS_GRID_HEADER_CONTAINER + '"></div><div class="' + CLS_GRID_BODY + '"></div><div style="display:none" class="' + CLS_GRID_BBAR + '"></div>'
                },
                innerBorder: {
                    sync: false,
                    value: true
                },
                useEmptyCell: {
                    value: true
                },
                useHeaderRow: {
                    value: true
                },
                autoFit: {
                    value: true
                }
            }
        },
        {
            xclass: "grid"
        });
    BUI.Grid.Grid = grid;
})(window.BUI, jQuery);
//BUI.Grid.Format
(function (BUI, $) {
    "use strict";
    function formatTimeUnit(v) {
        if (v < 10) {
            return "0" + v;
        }
        return v;
    }
    var Format = {
        dateRenderer: function (d) {
            if (!d) {
                return "";
            }
            if (BUI.isString(d)) {
                return d;
            }
            var date = null;
            try {
                date = new Date(d);
            } catch (e) {
                return "";
            }
            if (!date || !date.getFullYear) {
                return "";
            }
            return date.getFullYear() + "-" + formatTimeUnit(date.getMonth() + 1) + "-" + formatTimeUnit(date.getDate());
        },
        datetimeRenderer: function (d) {
            if (!d) {
                return "";
            }
            if (BUI.isString(d)) {
                return d;
            }
            var date = null;
            try {
                date = new Date(d);
            } catch (e) {
                return "";
            }
            if (!date || !date.getFullYear) {
                return "";
            }
            return date.getFullYear() + "-" + formatTimeUnit(date.getMonth() + 1) + "-" + formatTimeUnit(date.getDate()) + " " + formatTimeUnit(date.getHours()) + ":" + formatTimeUnit(date.getMinutes()) + ":" + formatTimeUnit(date.getSeconds());
        },
        cutTextRenderer: function (length) {
            return function (value) {
                value = value || "";
                if (value.toString().length > length) {
                    return value.toString().substring(0, length) + "...";
                }
                return value;
            };
        },
        enumRenderer: function (enumObj) {
            return function (value) {
                return enumObj[value] || "";
            };
        },
        multipleItemsRenderer: function (enumObj) {
            var enumFun = Format.enumRenderer(enumObj);
            return function (values) {
                var result = [];
                if (!values) {
                    return "";
                }
                if (!BUI.isArray(values)) {
                    values = values.toString().split(",");
                }
                $.each(values,
                    function (index, value) {
                        result.push(enumFun(value));
                    });
                return result.join(",");
            };
        },
        moneyCentRenderer: function (v) {
            if (BUI.isString(v)) {
                v = parseFloat(v);
            }
            if ($.isNumberic(v)) {
                return (v * .01).toFixed(2);
            }
            return v;
        }
    };
    BUI.Grid.Format = Format;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.Selection
//plugins : [Grid.Plugins.CheckSelection]
(function (BUI, $) {
    "use strict";
    BUI.Grid.Plugins = {};
    var PREFIX = BUI.prefix,
        CLS_CHECKBOX = PREFIX + "grid-checkBox",
        CLS_CHECK_ICON = "bui-grid-checkbox";
    function checkSelection(config) {
        checkSelection.superclass.constructor.call(this, config);
    }
    BUI.extend(checkSelection, BUI.Base);
    checkSelection.ATTRS = {
        width: {
            value: 40
        },
        column: {},
        cellInner: {
            value: '<div class="' + CLS_CHECKBOX + '-container"><span class="' + CLS_CHECK_ICON + '"></span></div>'
        }
    };
    BUI.augment(checkSelection, {
        createDom: function (grid) {
            var _self = this;
            var cfg = {
                title: "",
                width: _self.get("width"),
                fixed: true,
                resizable: false,
                sortable: false,
                tpl: '<div class="' + PREFIX + 'grid-hd-inner">' + _self.get("cellInner") + "",
                cellTpl: _self.get("cellInner")
            },
                checkColumn = grid.addColumn(cfg, 0);
            grid.set("multipleSelect", true);
            _self.set("column", checkColumn);
        },
        bindUI: function (grid) {
            var _self = this,
                col = _self.get("column"),
                colEl = col.get("el"),
                checkBox = colEl.find("." + CLS_CHECK_ICON);
            checkBox.on("click",
                function () {
                    var checked = colEl.hasClass("checked");
                    if (!checked) {
                        grid.setAllSelection();
                        colEl.addClass("checked");
                    } else {
                        grid.clearSelection();
                        colEl.removeClass("checked");
                    }
                });
            grid.on("rowunselected",
                function (e) {
                    colEl.removeClass("checked");
                });
            grid.on("clear",
                function () {
                    colEl.removeClass("checked");
                });
        }
    });
    BUI.Grid.Plugins.CheckSelection = checkSelection;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.RowNumber
//plugins : [Grid.Plugins.RowNumber]
(function (BUI, $) {
    "use strict";
    var CLS_NUMBER = "bui-grid-rownumber";
    function RowNumber(config) {
        RowNumber.superclass.constructor.call(this, config);
    }
    BUI.extend(RowNumber, BUI.Base);
    RowNumber.ATTRS = {
        width: {
            value: 40
        },
        column: {}
    };
    BUI.augment(RowNumber, {
        createDom: function (grid) {
            var _self = this;
            var cfg = {
                title: "",
                width: _self.get("width"),
                fixed: true,
                resizable: false,
                sortable: false,
                renderer: function (value, obj, index) {
                    return index + 1;
                },
                elCls: CLS_NUMBER
            },
                column = grid.addColumn(cfg, 0);
            _self.set("column", column);
        }
    });
    BUI.Grid.Plugins.RowNumber = RowNumber;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.ColumnGroup  暂时无用
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        CLS_HD_TITLE = PREFIX + "grid-hd-title",
        CLS_GROUP = PREFIX + "grid-column-group",
        CLS_GROUP_HEADER = PREFIX + "grid-group-header",
        CLS_DOUBLE = PREFIX + "grid-db-hd";
    var Group = function (cfg) {
        Group.superclass.constructor.call(this, cfg);
    };
    Group.ATTRS = {
        groups: {
            value: []
        },
        columnTpl: {
            value: '<th class="bui-grid-hd center" colspan="{colspan}"><div class="' + PREFIX + 'grid-hd-inner">' + '<span class="' + CLS_HD_TITLE + '">{title}</span>' + "</div></th>"
        }
    };
    BUI.extend(Group, BUI.Base);
    BUI.augment(Group, {
        renderUI: function (grid) {
            var _self = this,
                groups = _self.get("groups"),
                header = grid.get("header"),
                headerEl = header.get("el"),
                columns = header.get("children"),
                wraperEl = $('<tr class="' + CLS_GROUP + '"></tr>').prependTo(headerEl.find("thead"));
            headerEl.addClass(CLS_GROUP_HEADER);
            BUI.each(groups,
                function (group) {
                    var tpl = _self._getGroupTpl(group),
                        gEl = $(tpl).appendTo(wraperEl);
                    group.el = gEl;
                    for (var i = group.from; i <= group.to; i++) {
                        var column = columns[i];
                        if (column) {
                            column.set("group", group);
                        }
                    }
                });
            var afterEl;
            for (var i = columns.length - 1; i >= 0; i--) {
                var column = columns[i],
                    group = column.get("group");
                if (group) {
                    afterEl = group.el;
                } else {
                    var cEl = column.get("el");
                    cEl.addClass(CLS_DOUBLE);
                    cEl.attr("rowspan", 2);
                    if (afterEl) {
                        cEl.insertBefore(afterEl);
                    } else {
                        cEl.appendTo(wraperEl);
                    }
                    afterEl = cEl;
                }
            }
            if (groups[0].from !== 0) {
                var firstCol = columns[groups[0].from];
                if (firstCol) {
                    firstCol.get("el").css("border-left-width", 1);
                }
            }
        },
        _getGroupTpl: function (group) {
            var _self = this,
                columnTpl = _self.get("columnTpl"),
                colspan = group.to - group.from + 1;
            return BUI.substitute(columnTpl, {
                colspan: colspan,
                title: group.title
            });
        }
    });
    BUI.Grid.Plugins.ColumnGroup = Group;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.Cascade
//var cascade = new Grid.Plugins.Cascade({
//    renderer: function (record) {
//        return '<div style="padding: 10px 20px;"><h2>详情信息</h2><p>' + record.detail + '</p></div>';
//    }
//});
(function (BUI, $) {
    "use strict";
    var PREFIX = BUI.prefix,
        CLS_GRID_CASCADE = "",
        DATA_RECORD = "data-record",
        CLS_CASCADE = PREFIX + "grid-cascade",
        CLS_CASCADE_EXPAND = CLS_CASCADE + "-expand",
        CLS_CASCADE_ROW = CLS_CASCADE + "-row",
        CLS_CASCADE_CELL = CLS_CASCADE + "-cell",
        CLS_CASCADE_ROW_COLLAPSE = CLS_CASCADE + "-collapse";
    var cascade = function (config) {
        cascade.superclass.constructor.call(this, config);
    };
    BUI.extend(cascade, BUI.Base);
    cascade.ATTRS = {
        width: {
            value: 40
        },
        cellInner: {
            value: '<span class="' + CLS_CASCADE + '"><i class="' + CLS_CASCADE + '-icon"></i></span>'
        },
        rowTpl: {
            value: '<tr class="' + CLS_CASCADE_ROW + '"><td class="' + CLS_CASCADE_CELL + '"></td></tr>'
        },
        renderer: {},
        events: ["expand", "collapse", "removed"]
    };
    BUI.augment(cascade, {
        initializer: function (grid) {
            var _self = this;
            var cfg = {
                title: "",
                elCls: "center",
                width: _self.get("width"),
                resizable: false,
                fixed: true,
                sortable: false,
                cellTpl: _self.get("cellInner")
            },
                expandColumn = grid.addColumn(cfg, 0);
            //grid.set("innerBorder", false);
            _self.set("grid", grid);
        },
        bindUI: function (grid) {
            var _self = this;
            grid.on("cellclick",
                function (ev) {
                    var sender = $(ev.domTarget),
                        cascadeEl = sender.closest("." + CLS_CASCADE);
                    if (cascadeEl.length) {
                        if (!cascadeEl.hasClass(CLS_CASCADE_EXPAND)) {
                            _self._onExpand(ev.record, ev.row, cascadeEl);
                        } else {
                            _self._onCollapse(ev.record, ev.row, cascadeEl);
                        }
                    }
                });
            grid.on("columnvisiblechange",
                function () {
                    _self._resetColspan();
                });
            grid.on("rowremoved",
                function (ev) {
                    _self.remove(ev.record);
                });
            grid.on("clear",
                function () {
                    _self.removeAll();
                });
        },
        expandAll: function () {
            var _self = this,
                grid = _self.get("grid"),
                records = grid.getRecords();
            $.each(records,
                function (index, record) {
                    _self.expand(record);
                });
        },
        expand: function (record) {
            var _self = this,
                grid = _self.get("grid");
            var row = grid.findRow(record);
            if (row) {
                _self._onExpand(record, row);
            }
        },
        collapse: function (record) {
            var _self = this,
                grid = _self.get("grid");
            var row = grid.findRow(record);
            if (row) {
                _self._onCollapse(record, row);
            }
        },
        removeAll: function () {
            var _self = this,
                rows = _self._getAllCascadeRows();
            rows.each(function (index, row) {
                _self._removeCascadeRow(row);
            });
        },
        remove: function (record) {
            var _self = this,
                cascadeRow = _self._findCascadeRow(record);
            if (cascadeRow) {
                _self._removeCascadeRow(cascadeRow);
            }
        },
        collapseAll: function () {
            var _self = this,
                grid = _self.get("grid"),
                records = grid.getRecords();
            $.each(records,
                function (index, record) {
                    _self.collapse(record);
                });
        },
        _getRowRecord: function (cascadeRow) {
            return $(cascadeRow).data(DATA_RECORD);
        },
        _removeCascadeRow: function (row) {
            this.fire("removed", {
                record: $(row).data(DATA_RECORD),
                row: row
            });
            $(row).remove();
        },
        _findCascadeRow: function (record) {
            var _self = this,
                rows = _self._getAllCascadeRows(),
                result = null;
            $.each(rows,
                function (index, row) {
                    if (_self._getRowRecord(row) === record) {
                        result = row;
                        return false;
                    }
                });
            return result;
        },
        _getAllCascadeRows: function () {
            var _self = this,
                grid = _self.get("grid");
            return grid.get("el").find("." + CLS_CASCADE_ROW);
        },
        _getCascadeRow: function (gridRow) {
            var nextRow = $(gridRow).next();
            if (nextRow.hasClass(CLS_CASCADE_ROW)) {
                return nextRow;
            }
            return null;
        },
        _getRowContent: function (record) {
            var _self = this,
                renderer = _self.get("renderer"),
                content = renderer ? renderer(record) : "";
            return content;
        },
        _createCascadeRow: function (record, gridRow) {
            var _self = this,
                rowTpl = _self.get("rowTpl"),
                content = _self._getRowContent(record),
                rowEl = $(rowTpl).insertAfter(gridRow);
            rowEl.find("." + CLS_CASCADE_CELL).append($(content));
            rowEl.data(DATA_RECORD, record);
            return rowEl;
        },
        _onExpand: function (record, row, cascadeEl) {
            var _self = this,
                cascadeRow = _self._getCascadeRow(row),
                colspan = _self._getColumnCount(row);
            cascadeEl = cascadeEl || $(row).find("." + CLS_CASCADE);
            cascadeEl.addClass(CLS_CASCADE_EXPAND);
            if (!cascadeRow || !cascadeRow.length) {
                cascadeRow = _self._createCascadeRow(record, row);
            }
            $(cascadeRow).removeClass(CLS_CASCADE_ROW_COLLAPSE);
            _self._setColSpan(cascadeRow, row);
            _self.fire("expand", {
                record: record,
                row: cascadeRow[0]
            });
        },
        _onCollapse: function (record, row, cascadeEl) {
            var _self = this,
                cascadeRow = _self._getCascadeRow(row);
            cascadeEl = cascadeEl || $(row).find("." + CLS_CASCADE);
            cascadeEl.removeClass(CLS_CASCADE_EXPAND);
            if (cascadeRow && cascadeRow.length) {
                $(cascadeRow).addClass(CLS_CASCADE_ROW_COLLAPSE);
                _self.fire("collapse", {
                    record: record,
                    row: cascadeRow[0]
                });
            }
        },
        _getColumnCount: function (row) {
            return $(row).children().filter(function () {
                return $(this).css("display") !== "none";
            }).length;
        },
        _setColSpan: function (cascadeRow, gridRow) {
            gridRow = gridRow || $(cascadeRow).prev();
            var _self = this,
                colspan = _self._getColumnCount(gridRow);
            $(cascadeRow).find("." + CLS_CASCADE_CELL).attr("colspan", colspan);
        },
        _resetColspan: function () {
            var _self = this,
                cascadeRows = _self._getAllCascadeRows();
            $.each(cascadeRows,
                function (index, cascadeRow) {
                    _self._setColSpan(cascadeRow);
                });
        },
        destructor: function () {
            var _self = this;
            _self.removeAll();
            _self.off();
            _self.clearAttrVals();
        }
    });
    BUI.Grid.Plugins.Cascade = cascade;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.Editing
(function (BUI, $) {
    "use strict";
    var CLS_CELL_INNER = BUI.prefix + "grid-cell-inner";
    function Editing(config) {
        Editing.superclass.constructor.call(this, config);
    }
    BUI.extend(Editing, BUI.Base);
    Editing.ATTRS = {
        showError: {
            value: true
        },
        record: {},
        curEditor: {},
        fields: {
            shared: false,
            value: []
        },
        triggerSelected: {
            value: true
        }
    };
    BUI.augment(Editing, {
        initializer: function (grid) {
            var _self = this;
            _self.set("grid", grid);
        },
        renderUI: function () {
            var _self = this,
                grid = _self.get("grid");
            _self.initEditors();
            _self._initGridEvent(grid);
        },
        initEditors: function () {
            var _self = this,
                grid = _self.get("grid"),
                fields = [],
                columns = grid.get("columns");
            BUI.each(columns,
                function (column) {
                    var field = _self.getFieldConfig(column);
                    if (field) {
                        field.name = column.get("dataIndex");
                        field.colId = column.get("id");
                        field.width = column.get("width");
                        field.displayfield = column.get("displayfield");
                        field.forceFit = true;
                        if (field.validator) {
                            field.validator = _self.wrapValidator(field.validator);
                        }
                        fields.push(field);
                    }
                });
            _self.set("fields", fields);
        },
        acceptEidtor: function (editor) {
            var _self = this,
                grid = _self.get("grid"),
                store = grid.get("store");
            var record = _self.get("record");
            _self.updateRecord(store, record, editor);
            editor.destroy && editor.destroy();
            _self.set("curEditor", null);
        },
        _getCurEditor: function () {
            return this.get("curEditor");
        },
        _initGridEvent: function (grid) { },
        getFieldConfig: function (column) {
            return column.get("editor");
        },
        wrapValidator: function (validator) {
            var _self = this;
            return function (value) {
                var record = _self.get("record");
                return validator(value, record);
            };
        },
        onColumnVisibleChange: function (column) { },
        getEditorConstructor: function (c) {
            var _self = this,
                defaultCls = "form-field";
            if (!c.xclass) {
                if (!c.xtype) {
                    c.xclass = defaultCls;
                } else {
                    c.xclass = defaultCls + "-" + c.xtype;
                }
            }
            return BUI.Component.create(c, _self);
        },
        getEditor: function (options) { },
        getEditValue: function (options) { },
        showEditor: function (editor, options) { },
        beforeShowEditor: function (editor, options) { },
        isValid: function () {
            var _self = this, curEditor = _self.get("curEditor");
            if (curEditor) {
                var value = curEditor.getControlValue();
                var errMsg = curEditor.getValidError(value);
                var name = curEditor.get("name");
                if (BUI.isNullOrEmpty(errMsg)) {
                    return true
                } else {
                    _self.showError(errMsg, curEditor);
                    return false;
                }
            } else {
                return true;
            }
        },
        showError: function (errMsg, curEditor) {
            BUI.Message.Alert(errMsg, function () {
                curEditor.focus();
            }, 'error');
        },
        updateRecord: function (store, record, editor) { },
        cancelRecord: function (store, record) { },
        cancel: function () {
            var _self = this,
                curEditor = _self._getCurEditor();
            if (curEditor) {
                var record = _self.get("record");
                _self.cancelRecord(record);
                curEditor.destroy && curEditor.destroy();
                _self.set("curEditor", null);
            }
        },
        destructor: function () {
            var _self = this,
                editors = _self.get("editors");
            BUI.each(editors,
                function (editor) {
                    editor.destroy && editor.destroy();
                });
            _self.off();
            _self.clearAttrVals();
        }
    });
    BUI.Grid.Plugins.Editing = Editing;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.CellEditing
//var editing = new Grid.Plugins.CellEditing({
//    triggerSelected: false //触发编辑的时候不选中行
//});
(function (BUI, $) {
    "use strict";
    var Editing = BUI.Grid.Plugins.Editing,
        CLS_BODY = BUI.prefix + "grid-body",
        CLS_CELL = BUI.prefix + "grid-cell";
    var CellEditing = function (config) {
        CellEditing.superclass.constructor.call(this, config);
    };
    BUI.extend(CellEditing, Editing);
    BUI.augment(CellEditing, {
        getEditor: function (ev) {
            if (!ev.field) {
                return null;
            }
            var _self = this,
                fields = _self.get("fields"),
                editor = null;
            BUI.each(fields,
                function (cfg) {
                    if (cfg.name === ev.field) {
                        editor = _self.getEditorConstructor(cfg);
                        return false;
                    }
                });
            return editor;
        },
        resetWidth: function (editor, width) {
            editor.set("width", width);
        },
        _initGridEvent: function (grid) {
            var _self = this;
            grid.on("cellclick",
                function (ev) {
                    var editor = null,
                        domTarget = ev.domTarget,
                        curEditor = _self._getCurEditor();
                    if (curEditor) {
                        if (curEditor.get("render")[0] == ev.cell) {
                            return false;
                        } else {
                            if (curEditor.isValid()) {
                                _self.acceptEidtor(curEditor);
                            } else {
                                return false;
                            }
                        }
                    }
                    editor = _self.getEditor(ev);
                    if (editor) {
                        _self.showEditor(editor, ev);
                        if (!_self.get("triggerSelected")) {
                            return false;
                        }
                    }
                });
        },
        showEditor: function (editor, options) {
            var _self = this,
                value = _self.getEditValue(options);
            _self.beforeShowEditor(editor, options);
            $(options.cell).html("");
            _self.set("record", options.record);
            _self.fire("beforeeditorshow", {
                editor: editor,
                record: options.record
            });
            editor.set("render", $(options.cell));
            editor.set("value", value);
            editor.render();
            _self.set("curEditor", editor);
            _self.fire("editorshow", {
                editor: editor,
                record: options.record
            });
        },
        updateRecord: function (store, record, editor) {
            var _self = this,
                value = editor.getControlValue(),
                fieldName = editor.get("name"),
                preValue = record[fieldName];
            value = BUI.isDate(value) ? value.getTime() : value;
            if (preValue !== value) {
                store.setValue(record, fieldName, value);
            } else {
                var grid = _self.get("grid");
                grid.updateItem(record);
            }
        },
        cancelRecord: function (record) {
            var _self = this;
            var grid = _self.get("grid");
            grid.updateItem(record);
        },
        getEditValue: function (options) {
            if (options.record && options.field) {
                var value = options.record[options.field];
                return value == null ? "" : value;
            }
            return "";
        }
    });
    BUI.Grid.Plugins.CellEditing = CellEditing;
})(window.BUI, jQuery);
//BUI.Grid.Plugins.RowEditing
//var editing = new Grid.Plugins.RowEditing({
//    triggerSelected: false //触发编辑的时候不选中行
//});
(function (BUI, $) {
    "use strict";
    var Editing = BUI.Grid.Plugins.Editing, CLS_ROW = BUI.prefix + "grid-row";
    var RowEditing = function (config) {
        RowEditing.superclass.constructor.call(this, config);
    };
    BUI.extend(RowEditing, Editing);
    BUI.augment(RowEditing, {
        updateRecord: function (store, record, editor) {
            var _self = this, rst = {};
            BUI.each(editor.children, function (c, k) {
                var name = c.get("name"),
                    value = c.getControlValue() || "";
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
            BUI.each(rst, function (v, k) {
                if (BUI.isDate(v)) {
                    rst[k] = v.getTime();
                }
            });
            BUI.mix(record, rst);
            store.update(record);
        },
        _initGridEvent: function (grid) {
            var _self = this;
            grid.on("rowclick",
                function (ev) {
                    var editor = null,
                        domTarget = ev.domTarget,
                        curEditor = _self._getCurEditor();
                    if (curEditor) {
                        if (curEditor.row == ev.row) {
                            return false;
                        } else {
                            if (_self.isValid()) {
                                _self.acceptEidtor(curEditor);
                            } else {
                                return false;
                            }
                        }
                    }
                    editor = _self.getEditor(ev);
                    if (editor) {
                        _self.showEditor(editor, ev);
                        if (!_self.get("triggerSelected")) {
                            return false;
                        }
                    }
                });
        },
        showEditor: function (editor, options) {
            var _self = this,
                value = _self.getEditValue(options);
            _self.beforeShowEditor(editor, options);

            _self.set("record", options.record);
            _self.fire("beforeeditorshow", {
                editor: editor,
                record: options.record
            });
            for (var i = 0; i < editor.children.length; i++) {
                editor.children[i].get("render").html("");
                editor.children[i].render();
            }

            _self.set("curEditor", editor);
            _self.fire("editorshow", {
                editor: editor,
                record: options.record
            });
        },
        cancelRecord: function (record) {
            var _self = this;
            var grid = _self.get("grid");
            grid.updateItem(record);
        },
        getEditor: function (ev) {
            var _self = this,
                fields = _self.get("fields"),
                children = [];
            var tds = $(ev.row).children();
            for (var i = 0; i < fields.length; i++) {
                for (var j = 0; j < tds.length; j++) {
                    var td = $(tds[j]);
                    if (fields[i].name == td.attr("data-column-field")) {
                        fields[i].render = td;
                        fields[i].value = ev.record[fields[i].name];
                        if (!BUI.isNullOrEmpty(fields[i].displayfield)) {
                            fields[i].text = ev.record[fields[i].displayfield];
                        }
                        var editor = _self.getEditorConstructor(fields[i]);
                        children.push(editor);
                        break;
                    }
                }
            }
            var rowEditor = {
                row: ev.row,
                children: children,
                destroy: function () {
                    BUI.each(this.children, function (c, k) {
                        c.destroy && c.destroy();
                    });
                }
            };
            return rowEditor;
        },
        getEditValue: function (options) {
            return options.record;
        },
        isValid: function () {
            var _self = this, curEditor = _self.get("curEditor");
            if (curEditor) {
                var children = curEditor.children,
                    isValid = true;
                BUI.each(children,
                    function (item) {
                        if (!item.get("disabled") && !item.isValid()) {
                            isValid = false;
                            return false;
                        }
                    });
                return isValid;
            } else {
                return true;
            }
        }
    });
    BUI.Grid.Plugins.RowEditing = RowEditing;
})(window.BUI, jQuery);;

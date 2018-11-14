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
})(window.BUI, jQuery);
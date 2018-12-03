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
})(window.BUI);
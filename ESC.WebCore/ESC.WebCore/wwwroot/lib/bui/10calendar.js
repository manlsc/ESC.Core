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
                    value: '<td class="bui-datepicker-date bui-datepicker-{dateType} {todayCls} day-{dayOfWeek}" title="{date}"><a href="###" hidefocus="on" tabindex="1"><em><span>{dateNumber}</span></em></a></td>'
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
                        return '<table style="width:100%" cellspacing="0">\
                                <thead>\
                                <tr>\
                                    <th title="Sunday"><span>' + resource.weekDays[0] + '</span></th>\
                                    <th title="Monday"><span>' + resource.weekDays[1] + '</span></th>\
                                    <th title="Tuesday"><span>' + resource.weekDays[2] + '</span></th>\
                                    <th title="Wednesday"><span>' + resource.weekDays[3] + '</span></th>\
                                    <th title="Thursday"><span>' + resource.weekDays[4] + '</span></th>\
                                    <th title="Friday"><span>' + resource.weekDays[5] + '</span></th>\
                                    <th title="Saturday"><span>' + resource.weekDays[6] + '</span></th>\
                                </tr>\
                               </thead>\
                               <tbody class="bui-datepicker-body"></tbody>\
                         </table>';
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
        List = BUI.List.ListGroup,
        Nav = BUI.List.Nav,
        Resource = BUI.Calendar.Resource,
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
                    value: '<li class="' + CLS_ITEM + ' bui-monthpicker-month"><a href="###" hidefocus="on">{text}</a></li>'
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
                itemContainer: {
                    valueFn: function () {
                        return this.get("el").find("ul");
                    }
                },
                elTagName: {
                    value: "div"
                },
                year: {},
                start: {
                    value: new Date().getFullYear()
                },
                yearCount: {
                    value: 10
                },
                itemTpl: {
                    value: '<li class="' + CLS_ITEM + " " + CLS_YEAR + '"><a href="###" hidefocus="on">{text}</a></li>'
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
            var _self = this,
                items = [];
            items.push({
                text: Resource.submit,
                elCls: "btn btn-sm btn-default",
                id: "todayBtn",
                listeners: {
                    click: function () {
                        _self._successCall();
                    }
                }
            });
            items.push({
                text: Resource.cancel,
                elCls: "btn btn-sm btn-default",
                id: "clsBtn",
                listeners: {
                    click: function () {
                        var callback = _self.get("cancel");
                        if (callback) {
                            callback.call(_self);
                        }
                    }
                }
            });

            return new Nav({
                elCls: PREFIX + "calendar-footer",
                children: items
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
        Picker = BUI.Picker.ListPicker,
        MonthPicker = BUI.Calendar.MonthPicker,
        Header = BUI.Calendar.Header,
        Panel = BUI.Calendar.Panel,
        Nav = BUI.List.Nav,
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
                footer = _self.get("footer") || _self._createFooter()
            children.push(header);
            children.push(panel);
            children.push(footer);
            _self.set("header", header);
            _self.set("panel", panel);
            _self.set("footer", footer);
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
            panel.on("click",
                function () {
                    _self.fire("accept");
                });
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
                items = [];
            items.push({
                text: Resource.today,
                elCls: "btn btn-sm btn-default",
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
                text: Resource.clean,
                elCls: "btn btn-sm btn-default",
                id: "clsBtn",
                listeners: {
                    click: function () {
                        _self.fire("clear");
                    }
                }
            });

            return new Nav({
                elCls: PREFIX + "calendar-footer",
                children: items
            });
            var _self = this,               
                items = [];          
                items.push({
                    text: Resource.today,
                    elCls: "btn btn-sm btn-default",
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
                    text: Resource.clean,
                    elCls: "btn btn-sm btn-default",
                    id: "clsBtn",
                    listeners: {
                        click: function () {
                            _self.fire("clear");
                        }
                    }
                });

            return new Nav({
                elCls: PREFIX + "calendar-footer",
                children: items
            });
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
                selectedDate: {
                    value: today()
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
                _self.set("dateMask", "yyyy-mm-dd");
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
        },
        getSelectedValue: function () {
            if (!this.get("calendar")) {
                return null;
            }
            var _self = this,
                calendar = _self.get("calendar"),
                date = DateUtil.getDate(calendar.get("selectedDate"));          
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
})(window.BUI, jQuery);
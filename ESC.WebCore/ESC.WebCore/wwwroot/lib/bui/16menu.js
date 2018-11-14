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
            value:""
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
})(window.BUI, jQuery);
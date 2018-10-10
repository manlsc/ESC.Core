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
})(window.BUI, jQuery);
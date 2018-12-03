//BUI.Mask +
(function (BUI, $) {
    "use strict";
    var Mask = BUI.Mask = {},
    CLS_MASK = "bui-mask",
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
                if (element == "body") {
                    maskDiv.css("position", "fixed");
                }
                tpl = '<div class="' + CLS_MASK_LOADING + '">\
                            <div class="sk-three-bounce">\
                                <div class="sk-child sk-bounce1"></div>\
                                <div class="sk-child sk-bounce2"></div>\
                                <div class="sk-child sk-bounce3"></div>\
                            </div>\
                        </div> ';
                msgDiv = $(tpl).appendTo(maskDiv);
            }
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
            return maskDiv;
        },
        /**
        *解除元素的屏蔽
        */
        unmaskElement: function (element) {
            var maskedEl = $(element),
            maskDiv = maskedEl.children("." + CLS_MASK);          
            if (maskDiv) {
                maskDiv.remove();
            }
        }
    });
})(window.BUI, jQuery);
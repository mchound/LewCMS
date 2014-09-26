(function (document, window, undefined) {

    var

    self = this,

    pitta = function (selector, context) {
        return new pitta.fn.init(selector, context);
    };

    pitta.fn = pitta.prototype = {

        event: function (eventName, eventHandler) {
            
            var

            _self = this,

            _event = function (context, eventName, eventHandler) {

                if (window.addEventListener) {
                    context.addEventListener(eventName, eventHandler, false);
                } else {
                    context.attachEvent('on' + eventName, eventHandler, false);
                }

            }

            if (this.length > 1) {

                for (var i = 0; i < this.length; i++) {
                    _event(this[i], eventName, eventHandler);
                    return this;
                }

            }

            _event(this[0], eventName, eventHandler);
            return this;

        }

    };

    var init = pitta.fn.init = function (selector, context) {

        if (!selector)
            return this;

        this.context = context || document;
        this.selector = selector;

        if (typeof selector === 'string') {

            var
            elements = this.context.querySelectorAll(selector);

            this.length = 0;

            for (var i = 0; i < elements.length; i++) {
                this[i] = elements[i];
                this.length++;
            }

        } else if (selector.nodeType) {

            this.context = this[0] = selector;
            this.length = 1;
            return this;

        } else if (typeof selector !== undefined) {
            this[0] = selector;
            this.length = 1;
        }

        return this;

    };

    init.prototype = pitta.fn;

    window.pitta = window.p = pitta;


})(document, window);
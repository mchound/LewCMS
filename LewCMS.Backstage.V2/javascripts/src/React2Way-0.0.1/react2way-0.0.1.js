; (function () {

    'use strict';

    var

    r2w = {

        observable: function (initial) {
            var
            last = initial,
            internalArray = initial;

            function observable() {

                if (arguments.length === 0) {
                    return last;
                } if (arguments.length >= 1) {
                    if (last !== arguments[0]) {
                        var _last = last;
                        last = arguments[0];
                        for (var i = 0; i < observable.subscribers.length; i++) {
                            observable.subscribers[i].call(this, _last, arguments[0], !!arguments[1]);
                        }
                    }
                    return this;
                }
            };

            observable.subscribers = [];

            observable.subscribe = function (callback) {
                if (!!callback) {
                    this.subscribers.push(callback);
                }
            };

            function observableArray() {
                return internalArray;
            };

            observableArray.subscribers = [];

            observableArray.subscribe = function (callback) {
                if (!!callback) {
                    this.subscribers.push(callback);
                }
            };

            observableArray.push = function () {
                if (arguments.length === 0) return;
                Array.prototype.push.apply(internalArray, arguments);
                for (var i = 0; i < observableArray.subscribers.length; i++) {
                    observableArray.subscribers[i].call(this);
                }
            };

            if (Object.prototype.toString.call(initial) === '[object Array]') return observableArray;
            else return observable;
        },

        viewModel: function (properties) {

            function viewModel() {
                for (var prop in properties) {
                    this[prop] = properties[prop];
                }
                this.subscribeToProperties();
                return this;
            };

            viewModel.prototype.subscribers = [];

            viewModel.prototype.notifySubscribers = function (prop) {
                for (var i = 0; i < this.subscribers.length; i++) {
                    this.subscribers[i].call(this, prop);
                }
            };

            viewModel.prototype.onModelChange = function (callback) {
                if (!!callback) this.subscribers.push(callback);
            };

            viewModel.prototype.toState = function (prop) {

                var
                _prop = prop || this,
                type = Object.prototype.toString.call(_prop);

                if (type === '[object Object]') {

                    var
                    obj = {},
                    member;

                    for (var p in _prop) {
                        member = this.toState(_prop[p]);

                        if (member) obj[p] = member;
                    }

                    return obj;

                }
                else if (type === '[object Function]' && !!_prop.subscribe) {
                    return _prop();
                }
                else if (type === '[object Function]') {
                    return;
                }
                else {
                    return _prop;
                }

            };

            viewModel.prototype.fromState = function (state, model) {

                var
                _model = model || this,
                type = Object.prototype.toString.call(_model);

                if (type === '[object Object]') {

                    var obj = {};

                    for (var p in state) {
                        obj[p] = this.fromState(state[p], _model[p]);
                    }

                }
                else if (type === '[object Function]' && !!_model.subscribe) {
                    _model(state);
                }
                else {
                    _model = state;
                }
            };

            viewModel.prototype.subscribeToProperties = function (prop) {

                var
                _prop = prop || this,
                type = Object.prototype.toString.call(_prop);

                if (type === '[object Object]') {

                    for (var p in _prop) {
                        this.subscribeToProperties(_prop[p]);
                    }

                    return;

                }
                else if (type === '[object Array]') {
                    for (var i = 0; i < _prop.length; i++) {
                        this.subscribeToProperties(_prop[i]);
                    }
                    return;
                }
                else if (type === '[object Function]' && !!_prop.subscribe) {
                    _prop.subscribe(function () {
                        this.notifySubscribers.call(this, _prop);
                    }.bind(this));
                    return;
                }
            };

            return new viewModel();

        }

    };

    window.React2Way = r2w;

})(document, window);
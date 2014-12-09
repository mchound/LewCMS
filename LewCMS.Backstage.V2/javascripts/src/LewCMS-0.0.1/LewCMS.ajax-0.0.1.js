; (function (doc, win, lewcms, undef) {

    'use strict';

    var

    request = function (url, method, data, callback, error) {
        var json = JSON.stringify(data);
        var xmlhttp;
        // compatible with IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                if (xmlhttp.responseText)
                    callback(JSON.parse(xmlhttp.responseText));
                else
                    callback({});
            }
            else if (xmlhttp.readyState == 4 && xmlhttp.status != 200) {
                if (error) {
                    error.call(this, {
                        xmlhttp: xmlhttp,
                        status: xmlhttp.status,
                        statusText: xmlhttp.statusText,
                        responseText: xmlhttp.responseText
                    });
                }
            }
        }
        xmlhttp.open(method, url, true);
        xmlhttp.setRequestHeader('Content-type', 'application/json');
        xmlhttp.setRequestHeader('Accept', 'application/json');

        if (data)
            xmlhttp.send(json);
        else
            xmlhttp.send();
    },

    status = {
        OK: 'OK',
        ERROR: 'ERROR',
        SERVER_ERROR: 'SERVER_ERROR'
    },

    createUrlWithParameters = function(url, parameters){

        var _url = url;

        if (parameters) {
            _url += '?';

            for (var key in parameters) {
                var val = parameters[key] === undef || parameters[key] === null ? '' : parameters[key];
                _url += key + '=' + val + '&';
            }

            _url = _url.substring(0, _url.length - 1);
        }

        return _url;
    },

    successResponse = function (response) {
        if (response.success) {
            return {
                status: status.OK,
                data: response.data,
            };
        }

        return {
            status: status.SERVER_ERROR,
            errorMessages: response.errorMessages,
        };
        
    },

    errorResponse = function (error) {
        return {
            status: status.ERROR,
            error: error
        }
    },

    onRequestOk = function (callback) {

    },

    onRequestError = function (callback) {

    },

    get = function (url, data, callback) {
        var _url = createUrlWithParameters(url, data);
        request(_url, 'GET', null,
            function (response) { callback.call(this, successResponse(response)) },
            function (response) { callback.call(this, errorResponse(response)) });
    },

    post = function (url, data, callback) {
        request(_url, 'POST', data,
            function (data) { callback.call(successResponse(data)) },
            function (error) { callback.call(errorResponse(error)) });
    },

    put = function (url, data, callback) {

    },

    remove = function (url, data, callback) {

    };

    lewcms.ajax = {

        get: get,
        post: post,
        put: put,
        remove: remove 

    };

    lewCMS.ajax.status = status;
    
})(document, window, lewCMS);
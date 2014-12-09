; (function (doc, win, lewcms, ajax, undef) {

    'use strict';

    var

    routes = {

        pageTypes: lewcms.settings.apiUrl + 'pageTypes',
        pageTree: lewcms.settings.apiUrl + 'pageTree'

    },

    createResponse = function(response, callback, onErrorValue){

        if (response.status === ajax.status.ERROR) {
            // TODO: Do error handling
            callback.call(this, onErrorValue);
            return;
        }
        else if (response.status === ajax.status.SERVER_ERROR) {
            // TODO: Do error handling
            callback.call(this, onErrorValue);
            return;
        }

        callback.call(this, response.data);

    },

    getPageTypes = function (callback, onErrorValue) {

        ajax.get(routes.pageTypes, null, function (response) {
            createResponse(response, callback, onErrorValue);
        });

    },

    getPageTree = function (callback, onErrorValue, depth, rootId) {
        ajax.get(routes.pageTree, {depth: depth, rootId: rootId}, function(response){
            createResponse(response, callback, onErrorValue);
        });
    };

    lewcms.contentService = {

        get: {
            pageTypes: getPageTypes,
            pageTree: getPageTree
        }

    };
    
})(document, window, lewCMS, lewCMS.ajax);
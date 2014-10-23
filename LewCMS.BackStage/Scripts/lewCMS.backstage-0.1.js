; (function (document, window, $, undefined) {
	
    var

	self = this,

    // #region Defaults

    _defaults = {

        apiUrl: '/LewCMS-api/',
        editUIUrl: '/LewCMS/content/edit/',
        clientScriptsContainerSelector: '#client-scripts',
        culture: 'en'

    },

    // #endregion

    // #region Private

	_private = {

	    localization: {

	        currentCulture: _defaults.culture

	    },

	    ajax: {

	        createUrlWithParameters: function(url, parameters){

	            var _url = url;

	            if (parameters) {
	                _url += '?';

	                for (var key in parameters) {
	                    _url += key + '=' + parameters[key] + '&';
	                }

	                _url = _url.substring(0, _url.length - 1);
	            }

	            return _url;
	        },

	        apiBase: function (url, type, data, callback) {

	            $.ajax({
	                url: _defaults.apiUrl + url,
	                data: data,
	                type: type,
	                success: function (response) {

	                    if (response.success) {
	                        callback.call(this, true, response.data);
	                    } else {
	                        callback.call(this, false, { errorMessages: response.errorMessages });
	                    }

	                },
	                error: function (jqXHR, textStatus, errorThrown) {
	                    callback.call(this, false, {errorMessages: [errorThrown]});
	                }
	            });

	        },

	        editUIBase: function(url, type, data, callback){
	            
	            $.ajax({
	                url: _defaults.editUIUrl + url,
	                data: data,
	                type: type,
	                success: function (response) {
	                    callback.call(this, true, response);
	                },
	                error: function (jqXHR, textStatus, errorThrown) {
	                    callback.call(this, false, { errorMessages: [errorThrown] });
	                }
	            });

	        },

	        get: function (url, callback, parameters) {

	            var _url = _private.ajax.createUrlWithParameters(url, parameters);

	            _private.ajax.apiBase(_url, 'GET', null, callback);
	        },

	        post: function (url, data, callback) {
	            _private.ajax.apiBase(url, 'POST', data, callback);
	        },

	        remove: function (url, callback, parameters) {

	            var _url = _private.ajax.createUrlWithParameters(url, parameters);

	            _private.ajax.apiBase(_url, 'DELETE', null, callback);
	        },

	        html: function (url, callback, parameters) {

	            var _url = _private.ajax.createUrlWithParameters(url, parameters);

	            _private.ajax.apiBase(_url, 'GET', null, callback);
	            //_private.ajax.editUIBase(_url, 'GET', null, callback);

	        }

	    },



	},

    // #endregion

    // #region Store

	store = {

	    pages: {

	        get: {

	            contentTypes: function (callback) {

	                _private.ajax.get('pageTypes', callback);
	            },

	            children: function (parentId, callback) {
	                _private.ajax.get('children', callback, { parentId: parentId });
	            },

	            pageTree: function (callback, depth, rootId) {
	                _private.ajax.get('page-tree', callback, { depth: depth ? depth : '', rootId: rootId ? rootId : ''});
	            }

	        },

	        create: function (pageTypeId, name, parentId, callback) {
	            _private.ajax.post('create/page', { contentTypeId: pageTypeId, name: name, parentId: parentId }, callback);
	        },

	        remove: function (id, callback) {
	            _private.ajax.remove('delete/page', callback, { id: id });
	        },

	        edit: function (id, callback) {
	            //_private.ajax.html('page', callback, { id: id });
	            _private.ajax.html('edit/page', callback, { id: id });
	        }

	    }

	},

    // #endregion

    // #region Events

    events = {

        global: {

            subscribeTo: {

                htmlClick: function (subscriber, callback) {
                    eventSubscriptions.add('htmlClick', subscriber, callback);
                }

            },

            unsubscribeTo: {
                
                htmlClick: function (subscriber) {
                    eventSubscriptions.remove('htmlClick', subscriber);
                }

            }

        },

        subscribeTo: {

            changeMainView: function (subscriber, callback, condition) {
                eventSubscriptions.add('changeMainView', subscriber, callback, condition);
            },

            createPage: function (subscriber, callback, condition) {
                eventSubscriptions.add('createPage', subscriber, callback, condition);
            },

            pageCreated: function (subscriber, callback, condition) {
                eventSubscriptions.add('pageCreated', subscriber, callback, condition);
            },

            generalError: function (subscriber, callback, condition) {
                eventSubscriptions.add('generalError', subscriber, callback, condition);
            },

            showConfirmModal: function (subscriber, callback, condition) {
                eventSubscriptions.add('showConfirmModal', subscriber, callback, condition);
            },

            pageDeleted: function (subscriber, callback, condition) {
                eventSubscriptions.add('pageDeleted', subscriber, callback, condition);
            },

            editPage: function (subscriber, callback, condition) {
                eventSubscriptions.add('editPage', subscriber, callback, condition);
            }
        },

        unsubscribeTo: {

            changeMainView: function (subscriber) {
                eventSubscriptions.remove('changeMainView', subscriber);
            },

            createPage: function (subscriber) {
                eventSubscriptions.remove('createPage', subscriber);
            },

            pageCreated: function (subscriber) {
                eventSubscriptions.remove('pageCreated', subscriber);
            },

            generalError: function (subscriber) {
                eventSubscriptions.remove('generalError', subscriber);
            },

            showConfirmModal: function (subscriber) {
                eventSubscriptions.remove('showConfirmModal', subscriber);
            },

            pageDeleted: function (subscriber) {
                eventSubscriptions.remove('pageDeleted', subscriber);
            },

            editPage: function (subscriber) {
                eventSubscriptions.remove('editPage', subscriber);
            }
        },

        trigger: {

            changeMainView: function () {
                eventSubscriptions.trigger('changeMainView', arguments);
            },

            createPage: function () {
                eventSubscriptions.trigger('createPage', arguments);
            },

            pageCreated: function () {
                eventSubscriptions.trigger('pageCreated', arguments);
            },

            generalError: function () {
                eventSubscriptions.trigger('generalError', arguments);
            },

            showConfirmModal: function () {
                eventSubscriptions.trigger('showConfirmModal', arguments);
            },

            pageDeleted: function () {
                eventSubscriptions.trigger('pageDeleted', arguments);
            },

            editPage: function () {
                eventSubscriptions.trigger('editPage', arguments);
            }

        }

    },

    eventSubscriptions = {

        add: function (eventName, subscriber, callback, condition) {
            if (!eventSubscriptions.subscriptions[eventName]) {
                eventSubscriptions.subscriptions[eventName] = [];
            }
            eventSubscriptions.subscriptions[eventName].push({ subscriber: subscriber, callback: callback, condition: condition});
        },

        remove: function (eventName, subscriber) {
            var subscription = eventSubscriptions.subscriptions[eventName];
            if (subscription && subscription.length > 0) {
                for (var i = 0; i < subscription.length; i++) {
                    if (subscription[i].subscriber === subscriber)
                        break;
                }
                eventSubscriptions.subscriptions[eventName].splice(i, 1);
            }
        },

        trigger: function (eventName) {
            var subscription = eventSubscriptions.subscriptions[eventName];
            var args = arguments.length > 1 ? arguments[1] : null;
            if (subscription && subscription.length > 0) {
                for (var i = 0; i < subscription.length; i++) {
                    if (!!subscription[i].condition && subscription[i].condition.apply(this, args)) {
                        subscription[i].callback.apply(this, args);
                    } else if (!subscription[i].condition) {
                        subscription[i].callback.apply(this, args);
                    }
                    
                }
            }
        },

        subscriptions: {}

    },

    globalEvents = (function (document) {

        document.querySelector('html').addEventListener('click', function (e) {

            if (!e.target.hasAttribute('data-prevent-html-click')) {
                eventSubscriptions.trigger('htmlClick', e);
            }

        });

    })(document),

    // #endregion

    // #region Localization

    localization = {

        currentCulture: function () {
            return _private.localization.currentCulture;
        },

        setCulture: function (culture) {
            _private.localization.currentCulture = culture;
        },

        translate: function (key, section, culture) {
            var _culture = culture || localization.currentCulture();

            if (section) {
                return localization.translations.sections[section][_culture][key];
            }

            return localization.translations.keys[_culture][key];

        },

        translations: {

            sections: {

                createPage: {

                    en: {
                        createTitle: 'Create Page',
                        createDesc: 'Choose the page template you want to use',
                        nameTitle: 'Name Page',
                        nameDesc: 'Choose the name you want to give your page'
                    },

                    se: {
                        createTitle: 'Skapa Sida',
                        createDesc: 'Välj den sidmall du vill använda',
                        nameTitle: 'Namnge Sida',
                        nameDesc: 'Välj det namn du vill ge din sida'
                    }

                },

                createSection: {

                    en: {
                        createTitle: 'Create Section',
                        createDesc: 'Choose the section template you want to use',
                        nameTitle: 'Name Section',
                        nameDesc: 'Choose the name you want to give your section'
                    },

                    se: {
                        createTitle: 'Skapa Sektion',
                        createDesc: 'Välj den sektionsmall du vill använda',
                        nameTitle: 'Namnge Sektion',
                        nameDesc: 'Välj det namn du vill ge din sektion'
                    }

                }

            },

            keys: {

            }

        }

    };

    // #endregion

    clientScripts = {

        addScripts: function (scriptSources, callback) {

            var
            container = $(_defaults.clientScriptsContainerSelector).empty(),
            script,
            loadCounter = 0;

            if (scriptSources.length === 0)
                callback.call(container[0]);

            for (var i = 0; i < scriptSources.length; i++) {
                script = document.createElement('script');
                script.setAttribute('type', 'text/javascript');
                script.setAttribute('src', scriptSources[i]);
                container[0].appendChild(script);

                (function (script) {

                    script.onload = function () {
                        if (++loadCounter >= scriptSources.length) {
                            callback.call(container[0]);
                        }
                    }

                })(script);
            }

        }

    }

	window.lewCMS = {};
	window.lewCMS.store = store;
	window.lewCMS.events = events;
	window.lewCMS.localization = {};
	window.lewCMS.localization.currentCulture = localization.currentCulture;
	window.lewCMS.localization.setCulture = localization.setCulture;
	window.lewCMS.localization.translate = localization.translate;
	window.lewCMS.clientScripts = clientScripts;

})(document, window, jQuery);
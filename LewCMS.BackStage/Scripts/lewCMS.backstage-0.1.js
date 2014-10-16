; (function (document, window, $, undefined) {
	
    var

	self = this,

    // #region Defaults

    _defaults = {

        apiUrl: '/LewCMS-api/',
        culture: 'en'

    },

    // #endregion

    // #region Private

	_private = {

	    localization: {

	        currentCulture: _defaults.culture

	    },

	    ajax: {

	        base: function (url, type, data, callback) {

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
	                    callback.call(this, false, { jqXHR: jqXHR, textStatus: textStatus, errorThrown: errorThrown });
	                }
	            });

	        },

	        get: function (url, callback, parameters) {

	            var _url = url;

	            if (parameters) {
	                _url += '?';

	                for (var key in parameters) {
	                    _url += key + '=' + parameters[key] + '&';
	                }

	                _url = _url.substring(0, _url.length - 1);
	            }

	            _private.ajax.base(_url, 'GET', null, callback);
	        },

	        post: function (url, data, callback) {
	            _private.ajax.base(url, 'POST', data, callback);
	        }

	    }

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

            changeMainView: function (subscriber, callback) {
                eventSubscriptions.add('changeMainView', subscriber, callback);
            },

            createPage: function (subscriber, callback) {
                eventSubscriptions.add('createPage', subscriber, callback);
            },

            pageCreated: function (subscriber, callback) {
                eventSubscriptions.add('pageCreated', subscriber, callback);
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
            }
        },

        trigger: {

            changeMainView: function (viewName, parameters) {
                eventSubscriptions.trigger('changeMainView', [viewName].concat(parameters));
            },

            createPage: function () {
                eventSubscriptions.trigger('createPage', arguments);
            },

            pageCreated: function () {
                eventSubscriptions.trigger('pageCreated', arguments);
            }

        }

    },

    eventSubscriptions = {

        add: function (eventName, subscriber, callback) {
            if (!eventSubscriptions.subscriptions[eventName]) {
                eventSubscriptions.subscriptions[eventName] = [];
            }
            eventSubscriptions.subscriptions[eventName].push({ subscriber: subscriber, callback: callback });
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

        trigger: function (eventName, parameters) {
            var subscription = eventSubscriptions.subscriptions[eventName];
            if (subscription && subscription.length > 0) {
                for (var i = 0; i < subscription.length; i++) {
                    subscription[i].callback.apply(this, parameters);
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

	window.lewCMS = {};
	window.lewCMS.store = store;
	window.lewCMS.events = events;
	window.lewCMS.localization = {};
	window.lewCMS.localization.currentCulture = localization.currentCulture;
	window.lewCMS.localization.setCulture = localization.setCulture;
	window.lewCMS.localization.translate = localization.translate;

})(document, window, jQuery);
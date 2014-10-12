; (function (document, window, $, undefined) {
	
	var

	self = this,

    _defaults = {

        apiUrl: '/LewCMS-api/'

    },

	_private = {

		ajax: {

			base: function(url, type, data, callback){

				$.ajax({
					url: _defaults.apiUrl + url,
					data: data,
					type: type,
					success: function (response) {

					    if (response.success) {
					        callback.call(this, true, response.data);
					    } else {
					        callback.call(this, false, {errorMessage: response.errorMessage});
					    }
						
					},
					error: function (jqXHR, textStatus, errorThrown ) {
						callback.call(this, false, {jqXHR: jqXHR, textStatus: textStatus, errorThrown: errorThrown});
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

	lewCMS = {

	    pages: {

	        get: {

	            pageTypes: function (callback) {

	                _private.ajax.get('pageTypes', function (success, data) {
	                    callback.call(this, { success: success, data: data });
	                });
	            },

	            children: function (parentId, callback) {
	                _private.ajax.get('children', callback, { parentId: parentId });
	            }

	        },

	        create: {

	            page: function (pageTypeId, name, parentId, callback) {
	                _private.ajax.post('createPage', { pageTypeId: pageTypeId, name: name, parentId: parentId }, callback);
	            }

	        }

	    }

	};

	window.lewCMS = lewCMS;

})(document, window, jQuery);
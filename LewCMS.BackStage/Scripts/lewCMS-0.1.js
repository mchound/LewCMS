; (function (document, window, $, undefined) {
	
	var

	self = this,

	_private = {

		ajax: {

			base: function(url, type, data, callback){

				$.ajax({
					url: '/LewCMS-api/' + url,
					data: data,
					type: type,
					success: function (response) {
						callback.call(this, true, response);
					},
					error: function (jqXHR, textStatus, errorThrown ) {
						callback.call(this, false, {jqXHR: jqXHR, textStatus: textStatus, errorThrown: errorThrown});
					}
				});

			},

			get: function (url, callback) {
				_private.ajax.base(url, 'GET', null, callback);
			},

			post: function (url, data, callback) {
				_private.ajax.base(url, 'POST', data, callback);
			}

		}

	},

	lewCMS = {

		getPageTypes: function () {

			_private.ajax.get('pageTypes', function (success, data) {
				console.log('success: ' + success);
				console.log(data);
			});

		}

	};

	window.lewCMS = lewCMS;

})(document, window, jQuery);
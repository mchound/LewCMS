/** @jsx React.DOM */

var ItemMenu = React.createClass({

	onNewPageClick: function(e){

		e.stopPropagation();
		lewCMS.events.trigger.createPage(this.props.page.id);

		if(!!this.props.itemMenuClicked){
			this.props.itemMenuClicked.call(this);
		}
	},

	onDeleteClick: function(e){
		
		e.stopPropagation();
		lewCMS.events.trigger.showConfirmModal('Trash Page', 'Are you sure you want to move the page to trash?', this.onDeleteConfirm);

	},

	onDeleteConfirm: function(){
		
		lewCMS.store.pages.remove(this.props.page.id, function(success, response){

			if(success){
				lewCMS.events.trigger.pageDeleted(response);
			} else {
				lewCMS.events.trigger.generalError({isSelfDestroying: false, errorMessage: response.errorMessages[0]});
			}

			
		}.bind(this));

	},

    render: function () {

        return (
            <ul data-lm-itemmenu>
                <li><button data-prevent-html-click data-lm-button="link" onClick={this.onNewPageClick}>New Page</button></li>
                <li><button data-prevent-html-click data-lm-button="link">Cut</button></li>
                <li><button data-prevent-html-click data-lm-button="link">Copy</button></li>
                <li><button data-prevent-html-click data-lm-button="link">Paste</button></li>
                <li><button data-prevent-html-click data-lm-button="link" onClick={this.onDeleteClick}>Trash</button></li>
            </ul>
        );


    }


});
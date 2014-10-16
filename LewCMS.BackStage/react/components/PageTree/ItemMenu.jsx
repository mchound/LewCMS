/** @jsx React.DOM */

var ItemMenu = React.createClass({

	onNewPageClick: function(){
		lewCMS.events.trigger.createPage(this.props.page.id);

		if(!!this.props.itemMenuClicked){
			this.props.itemMenuClicked.call(this);
		}
	},

	onDeleteClick: function(){
		console.log('Delete');
	},

    render: function () {

        return (
            <ul data-lm-itemmenu>
                <li><button data-prevent-html-click data-lm-button="link" onClick={this.onNewPageClick}>New Page</button></li>
                <li><button data-prevent-html-click data-lm-button="link">Cut</button></li>
                <li><button data-prevent-html-click data-lm-button="link">Copy</button></li>
                <li><button data-prevent-html-click data-lm-button="link">Paste</button></li>
                <li><button data-prevent-html-click data-lm-button="link" onClick={this.onDeleteClick}>Delete</button></li>
            </ul>
        );


    }


});
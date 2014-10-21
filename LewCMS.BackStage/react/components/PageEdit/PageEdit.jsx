/** @jsx React.DOM */

var PageEdit = React.createClass({
	
	getInitialState: function(){
		return {html: ''};
	},

	componentDidMount: function(){
		
		lewCMS.store.pages.edit(
		
		this.props.id, 
		
		function(success, response){
			
			if(success){
			
				this.setState({html: response});

			} else {
				
				lewCMS.events.trigger.generalError({isSelfDestroying: false, errorMessage: response.errorMessages[0]});

			}

		}.bind(this));

	},

	render: function(){
		
		return (
			<div dangerouslySetInnerHTML={{__html: this.state.html}} data-lm-propertylist>
				
			</div>
		);

	}


});
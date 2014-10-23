/** @jsx React.DOM */

var PageEdit = React.createClass({
	
	getInitialState: function(){
		return {html: '', clientScripts: [], propertyNames: []};
	},

	componentDidMount: function(){
		
		lewCMS.store.pages.edit(
		
		this.props.id, 
		
		function(success, response){
			
			if(success){
				
				this.setState({html: response.html, clientScripts: response.clientScripts || [], propertyNames: response.propertyNames || []});

			} else {
				
				lewCMS.events.trigger.generalError({isSelfDestroying: false, errorMessage: response.errorMessages[0]});

			}

		}.bind(this));

	},

	componentDidUpdate: function(){
	
		lewCMS.clientScripts.addScripts(this.state.clientScripts, function(){
			for(var i = 0; i < this.state.propertyNames.length; i++){
				PropertyString(this.state.propertyNames[i], this.props.id);
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
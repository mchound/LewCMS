/** @jsx React.DOM */

var ErrorSummary = React.createClass({

	getInitialState: function(){
		return {errors: [], showErrors: false};
	},

	componentWillMount: function(){
	
		lewCMS.events.subscribeTo.generalError(
		
		'error-summary',

		function(error){

			var 
			errors = this.state.errors,
			index = errors.length,
			_error = error;

			_error.key = (new Date().getTime()) + error.errorMessage;

			errors.push(error);
			this.setState({errors: errors, showErrors: true});

			setTimeout(function(){
				this.setState({showErrors: false});
			}.bind(this), 5000);

			if(error.isSelfDestroying){
			
				setTimeout(function(){
				
					errors = this.state.errors;
					errors.splice(index, 1);
					this.setState({errors: errors});
				
				}.bind(this), 5000);

			}

		}.bind(this)

		);

	},

	onErrorCountClick: function(){
		
		this.setState({showErrors: !this.state.showErrors});
	
	},

	onErrorDelete: function(key){
		
		for(var i = 0; i < this.state.errors.length; i++){
			if(this.state.errors[i].key == key){
				break;
			}
		}

		if(i < this.state.errors.length){
			var errors = this.state.errors;
			errors.splice(i, 1);
			this.setState({errors: errors});
		}

	},

	render: function(){
	
		var

		hasErrors = this.state.errors.length > 0;

		icon = hasErrors ? <i className="error icon-alert"></i> : <i className="success icon-check"></i>,

		errors = this.state.errors.map(function(error, index){
			return <li key={error.key}><i className="icon-right-open"></i>{error.errorMessage}<i className="delete icon-cancel" onClick={this.onErrorDelete.bind(this, error.key)}></i></li>
		}.bind(this));

		return (
			<div>
				<div className="error-count-wrapper error" data-error-count={this.state.errors.length} onClick={this.onErrorCountClick}>
					{icon}
					<span className="error-count">{this.state.errors.length}</span>
				</div>
				<div className={(!this.state.showErrors || this.state.errors.length === 0)? 'errors remove' : 'errors'}>
					<ul>
						{errors}
					</ul>
				</div>
			</div>
		);
	}

});

React.renderComponent(
  ErrorSummary(),
  document.getElementById('error-summary')
);
/** @jsx React.DOM */

var Form = React.createClass({

	getInitialState: function(){
	
		return {valid: false, showSummary: false, errorMessages: [], submitTried: false};

	},

	onSubmit: function(e){
		
		e.preventDefault();
		var 
		valid = true,
		form = this.getDOMNode(),
		errorMessages = [];

		for(var i = 0; i < form.children.length; i++){
			
			var attr = form.children[i].getAttribute('data-validation');
			valid = valid & !attr;

			if(!!attr){
				errorMessages = errorMessages.concat(attr.split(','));
			}
		}

		if(this.props.onSubmit){
			this.props.onSubmit.call(this, e, valid, errorMessages);
		}

		this.setState({valid: valid, showSummary: this.props.showSummary && !valid, errorMessages: errorMessages, submitTried: true});
	},

	render: function(){

		var

		errors = this.state.errorMessages.map(function(error){
		
			return (
				<li>{error}</li>
			);
		
		}),

		summary = !this.state.showSummary ? '' : (<ul>{errors}</ul>);
		
		
		return (

			<form method={this.props.method} action={this.props.action} noValidate="novalidate" onSubmit={this.onSubmit} data-lm-form data-submit-tried={this.state.submitTried}>
				{summary}
				{this.props.children}
			</form>
		
		);
	}
});

var TextBox = React.createClass({
	
	mixins: [ValidationMixin],

	getInitialState: function(){
		return {valid: false, hasErrorClass: false, errorMessages: []};
	},

	onChange: function(e){
		this.validate(e);
	},

	val: function(){
		return this.refs.input.getDOMNode().value;
	},

	render: function(){

		var
		label = !!this.props.label ? <label>{this.props.label}</label> : '',
		error = !!this.props.showErrorMessage && this.state.errorMessages.length > 0 ? <span className="error-message">{this.state.errorMessages[0]}</span> : '';

		return (
			<div data-lm-textbox="" className={this.state.hasErrorClass ? (this.props.errorClass || 'error') : ''}>
				{label}
				<input 
					ref="input"
					type={!!this.props.type ? this.props.type : "text"}
					placeholder={this.props.placeholder}
					defaultValue={this.props.value}
					onChange={this.onChange}
					data-validation={this.state.errorMessages.join()} />
					{error}
			</div>
		);
	}
});
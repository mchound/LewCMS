/** @jsx React.DOM */

var ConfirmModal = React.createClass({

	getInitialState: function(){
	
		return {show: false, header: '', body: '', onConfirm: null, onCancel: null};

	},

	componentWillMount: function(){
	
		lewCMS.events.subscribeTo.showConfirmModal(
		
		'confirm-modal',

		function(header, body, onConfirmCallback, onCancelCallback){
		
			this.setState({show: true, header: header, body: body, onConfirm: onConfirmCallback, onCancel: onCancelCallback});
			this.toggleBackdrop(true);

		}.bind(this)

		);

	},

	onCancelClick: function(){
		
		if(this.state.onCancel){
			this.state.onCancel.call(this);
		}

		this.setState({show: false, header: '', body: '', onCancel: null, onConfirm: null});
		this.toggleBackdrop(false);
	},

	onConfirmClick: function(){
		
		if(this.state.onConfirm){
			this.state.onConfirm.call(this);
		}

		this.setState({show: false, header: '', body: '', onCancel: null, onConfirm: null});
		this.toggleBackdrop(false);

	},

	toggleBackdrop: function(show){
		if(show){
			document.querySelector('#modal-backdrop').classList.remove('remove');
			document.querySelector('#modal').classList.remove('remove');
		}
		else{
			document.querySelector('#modal-backdrop').classList.add('remove');
			document.querySelector('#modal').classList.add('remove');
		}
	},
	
	render: function(){
		
		var

		component = null;

		if(this.state.show){
			component = (<div className="modal-dialog">
							<div className="content">
								<div className="header">
									<h2>{this.state.header}</h2>
								</div>
								<div className="body">
									<span>{this.state.body}</span>
								</div>
								<div className="footer">
									<button onClick={this.onCancelClick} data-lm-button="icon clr-gray"><i className="icon-cancel"></i>Cancel</button>
									<button onClick={this.onConfirmClick} data-lm-button="icon clr-green"><i className="icon-check"></i>Confirm</button>
								</div>
							</div>
						</div>);
		}

		return(
			
			<div>
				{component}
			</div>

		);
	
	}


});

React.renderComponent(
  ConfirmModal(),
  document.getElementById('modal')
);
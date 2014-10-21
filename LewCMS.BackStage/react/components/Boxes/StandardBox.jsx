/** @jsx React.DOM */

var StandardBox = React.createClass({


	render: function(){
		
		var
		header = !!this.props.header ? <h2>{this.props.header}</h2> : null;

		return (
		
			<div data-lm-box={this.props.styleAttr}>

				{header}
				{this.props.children}

			</div>
		
		);

	}

})
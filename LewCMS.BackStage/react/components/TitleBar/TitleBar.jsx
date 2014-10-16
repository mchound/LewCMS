/** @jsx React.DOM */

var TitleBar = React.createClass({
	
	render: function(){
		
		return(
			<div data-lm-titlebar="title-bar">
				<h1>{this.props.title}</h1>
				<p>{this.props.desc}</p>
			</div>
		);
	
	}

});
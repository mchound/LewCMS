var LeftPane = React.createClass({displayName: 'LeftPane',
	
	render: function() {
		
		return (
			React.createElement("div", null, 
				React.createElement(PageTree, {model: new React2Way.viewModel({pageTree: this.props.pageTree})})
			)
        );
    }
});
var LeftPane = React.createClass({
	
	render: function() {
		
		return (
			<div>
				<PageTree model={new React2Way.viewModel({pageTree: this.props.pageTree})} />
			</div>
        );
    }
});
/** @jsx React.DOM */

var PageTree = React.createClass({
	
	render: function() {
		
		var pages = this.props.pages.map(function(page){
		
			return (
				<PageTreeListItem page={page} />
			);
		
		});

        return (
            <ul key={this.props.pages[0].id}>
                {pages}
            </ul>
        );
    }
});
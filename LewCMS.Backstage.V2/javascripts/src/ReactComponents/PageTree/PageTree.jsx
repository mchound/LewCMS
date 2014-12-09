var PageTree = React.createClass({
	
	mixins: [R2wMixin],
		
	render: function() {
		
		var pages = this.state.pageTree.map(function(pageTreeItem){
			return <li>{pageTreeItem.name}</li>
		});

		return (
            <ul>
                {pages}
            </ul>
        );
    }
});
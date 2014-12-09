var PageTree = React.createClass({displayName: 'PageTree',
	
	mixins: [R2wMixin],
		
	render: function() {
		
		var pages = this.state.pageTree.map(function(pageTreeItem){
			return React.createElement("li", null, pageTreeItem.name)
		});

		return (
            React.createElement("ul", null, 
                pages
            )
        );
    }
});
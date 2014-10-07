/** @jsx React.DOM */

var PageTreeComponent = React.createClass({
	
	getInitialState: function(){
		return {pages: []};
	},

	componentWillMount: function(){
	
		$.ajax({
		  url: this.props.url,
		  dataType: 'json',
		  success: function(response) {
			this.setState({pages: response});
		  }.bind(this),
		  error: function(xhr, status, err) {
			console.error(this.props.url, status, err.toString());
		  }.bind(this)
		});

	},

    render: function(){
		
		return(
			<PageTree pages={this.state.pages} />
		);
	
	}

});

var PageTree = React.createClass({

    render: function() {

        var pages = this.props.pages.map(function (page) {
            
            return (
                    <PageTreeListItem page={page} />
            );
        });

        return (
            <ul>
                {pages}
            </ul>
        );
    }
});

var PageTreeListItem = React.createClass({
 
    render: function() {

        var children = "";
        if (this.props.page.children && this.props.page.children.length > 0) {
            children = <PageTree pages={this.props.page.children} />
            }

        return (
            <li key={this.props.page.title}>    
                <span>{this.props.page.title}</span>
                {children}
            </li>
        );
}

});

var pages = {pages: [
            {
                title: 'One',
                children: [ {title: 'One_1'}, {title: 'One_2'}]
            },
            {
                title: 'Two',
                children: [ ]
            },
            {
                title: 'Three'
            }
]};

React.renderComponent(
  PageTreeComponent({url: '/LewCMS-api/page-tree'}),
  document.getElementById('page-tree')
);
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
	
	getInitialState: function(){
		return {isExpanded: false, isExpanding: false};
	},

	onExpandClick: function(e){
		
		this.setState({isExpanding: true});

		if(this.state.isExpanded){
			this.setState({isExpanded: false, isExpanding: false});
		}
		else {
			setTimeout(function(){
				this.setState({isExpanded: true, isExpanding: false});
			}.bind(this), 2000);
		}
		

	},

    render: function() {

        var 
		children = '',
		liClassName = '',
		parentIcon = '',
		iconClassName = this.state.isExpanded ? 'icon-minus' : 'icon-plus';

		iconClassName = this.state.isExpanding ? 'icon-plus-circled animate-spin' : iconClassName;

        if (this.props.page.children && this.props.page.children.length > 0) {
            children = <PageTree pages={this.props.page.children} />;
			parentIcon = <i className={iconClassName} onClick={this.onExpandClick}></i>;
			liClassName = 'parent' + (this.state.isExpanded ? ' expanded' : '');
        }

        return (
            <li key={this.props.page.title} className={liClassName}>
                <a href="#">{parentIcon}{this.props.page.title}</a>
                {children}
            </li>
        );
}

});

React.renderComponent(
  PageTreeComponent({url: '/LewCMS-api/page-tree'}),
  document.getElementById('page-tree')
);
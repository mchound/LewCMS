/** @jsx React.DOM */

var MainView = React.createClass({
	
	getInitialState: function(){
		return {view: 'initial'};
	},

	componentWillMount: function(){
	
		lewCMS.events.subscribeTo.createPage('mainView', function (parentId) {
			this.component = <CreateContent storeSection="pages" parentId={parentId} langSection="createPage" />
			this.setState({view: 'createPage'});
		}.bind(this));

		lewCMS.events.subscribeTo.pageCreated('mainView', function(page){
			this.component = '';
			this.setState({view: 'dashboard'});
		}.bind(this));

	},

	component: '',

    render: function(){
		
		return(
			<div className="mainView">
				{this.component}
			</div>
		);
	
	}

});

React.renderComponent(
  MainView(),
  document.getElementById('main-view')
);
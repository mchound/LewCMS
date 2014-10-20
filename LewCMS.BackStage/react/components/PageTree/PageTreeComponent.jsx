/** @jsx React.DOM */

var PageTreeComponent = React.createClass({
	
	getInitialState: function(){
		return {pages: [], serverOK: false};
	},

	componentWillMount: function(){
	
		lewCMS.store.pages.get.pageTree(function(success, response){
			
			if(success && response){
				var pageTree = response;
				pageTree.isExpanded = true;
				this.setState({pages: [pageTree], serverOK: true});

			} else if(success && !response){
				
				this.setState({pages: [], serverOK: true});

			} else {
				this.setState({pages: null, serverOK: false});
				lewCMS.events.trigger.generalError({isSelfDestroying: false, errorMessage: 'Error trying to fetch Page Tree from server'});
			}
			
		}.bind(this), 1);

		// Page with this page as parent has been deleted
		lewCMS.events.subscribeTo.pageDeleted(
		
			'pageTree', 
			
			function(pageInfo){
			
				this.setState({pages: []});

			}.bind(this),
			
			function(pageInfo){
				return pageInfo.parentId == null;
			}.bind(this)
				
		);
	},

	pageCreated: function(page){
	
		var 
		pages = this.state.pages || [],
		parentId = undefined;

		if(!page.parentId){
			
			pages.push(page);

		} else {
		
			var parent = this.findPageByIdRecursive(pages[0], page.parentId);
			parent.children.push(page);
			parent.hasChildren = true;
			parent.isExpanded = true;
		
		}

		this.setState({pages: pages});
		this.forceUpdate();
	},

	findPageByIdRecursive: function(page, id){
		
		if(page.id == id){
			return page;
		}

		for(var i = 0; i < page.children.length; i++){
			if(page.children[i].id == id){
				
				return page.children[i];

			} else if (page.children[i].children && page.children[i].children.length > 0){
				
				return this.findPageByIdRecursive(page.children[i], id);

			}
			
		}

		return;

	},

	onRootClick: function(){
		lewCMS.events.trigger.createPage(null);
	},

    render: function(){
		
		var rootNode = '';

		if(this.state.pages && this.state.pages.length == 0 && this.state.serverOK){
			rootNode = <button onClick={this.onRootClick} data-lm-button="link" className="btn-first-page" style={{display: 'block', margin: '0 40px', 'white-space': 'nowrap'}}>Create your first page<i className="icon-right-open-mini"></i></button>
		} else if(this.state.pages && this.state.serverOK){
			rootNode = <PageTree pages={this.state.pages} />
		}

		return(
			<div>
				{rootNode}
			</div>
		);
	
	}

});

React.renderComponent(
  PageTreeComponent({url: '/LewCMS-api/page-tree'}),
  document.getElementById('page-tree')
);
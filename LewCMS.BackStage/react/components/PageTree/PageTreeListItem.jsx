/** @jsx React.DOM */

var PageTreeListItem = React.createClass({
	
	getInitialState: function(){
		return {showMenu: false, page: this.props.page, isExpanded: false, isExpanding: false};
	},

	componentWillMount: function(){

		lewCMS.events.global.subscribeTo.htmlClick('pageTree', function(){
			if(this.state.showMenu){
				this.setState({showMenu: false});
			}
		}.bind(this));

	},

	componentDidMount: function(){
		this.setState({isExpanded: this.props.page.isExpanded});
	},

	onMenuClick: function(page, e){
		this.setState({showMenu: !this.state.showMenu});
	},

	onItemMenuClick: function(){
		this.setState({showMenu: false});
	},

	onParentIconClick: function(){
		
		if(this.state.isExpanded){
		
			this.setState({isExpanded: false});
			return;

		}

		this.setState({isExpanding: true});

		lewCMS.store.pages.get.pageTree(function(success, response){
			
			if(success){
			
				this.setState({page: response, isExpanded: true, isExpanding: false});

			} else {
			
				this.setState({isExpanding: false});

			}

		}.bind(this), 1, this.props.page.id);
	
	},

	render: function() {
		
		var createLiClassName = function(){
			var classNames = [];
			if(this.state.page.isStartPage){ classNames.push('start-page');}
			if(this.state.page.hasChildren){classNames.push('parent');}
			if(this.state.isExpanded){classNames.push('expanded');}
			return classNames.join(' ');
		}.bind(this);

		var parentIconClassName = function(){
			var classNames = ['icon-parent'];
			if(this.state.isExpanded){ classNames.push('icon-down-open-mini');}
			if(!this.state.isExpanded && !this.state.isExpanding ){ classNames.push('icon-right-open-mini');}
			if(this.state.isExpanding){ classNames.push('icon-spin6 animate-spin');}
			return classNames.join(' ');
		}.bind(this);

        var 
		children = null,
		startPageIcon = this.state.page.isStartPage ? <i className="icon-startpage icon-home"></i> : null,
		menu = this.state.showMenu ? <ItemMenu page={this.state.page} itemMenuClicked={this.onItemMenuClick} /> : null,
		parentIconClass = parentIconClassName(),
		parentIcon = this.state.page.hasChildren ? <i className={parentIconClass} onClick={this.onParentIconClick}></i> : null;
		
        if (this.state.page.hasChildren) {
            children = <PageTree pages={this.state.page.children} />;
        }

        return (
            <li key={this.state.page.name} className={createLiClassName()}>
                <a href="#">
					{startPageIcon}
					{this.state.page.name}
					<i className="icon-menu icon-dot-3" onClick={this.onMenuClick.bind(this, this.state.page)} data-prevent-html-click></i>
					{parentIcon}
					{menu}
				</a>
                {children}
            </li>
        );
}

});
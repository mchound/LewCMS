/** @jsx React.DOM */

var ContentTypeList = React.createClass({
	
	getInitialState: function(){
		return {contentTypesByCategory: []};
	},

	componentWillMount: function(){
		
		lewCMS.store[this.props.storeSection].get.contentTypes(function(success, data){
			if(success){
				this.setState({contentTypesByCategory: data});
			}
		}.bind(this));

	},

    render: function(){
		
		var categories = this.state.contentTypesByCategory.map(function(category, index){
			return (
				<ContentTypeCategory item={category} key={index} onContentTypeClick={this.props.onContentTypeClick} />
			);
		}.bind(this));

		return(
			<div data-lew-style="content-type-list">
				<ul className="category-list">
					{categories}
				</ul>
			</div>
		);
	
	}

});
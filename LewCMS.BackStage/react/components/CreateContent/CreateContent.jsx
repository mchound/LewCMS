
/** @jsx React.DOM */

var CreateContent = React.createClass({
	
	getInitialState: function(){
		return {
			contentTypeId: null, 
			title: lewCMS.localization.translate('createTitle', this.props.langSection), 
			desc: lewCMS.localization.translate('createDesc', this.props.langSection)
		};
	},

	onContentTypeClick: function(contentTypeId){
		this.setState({
			contentTypeId: contentTypeId,
			title: lewCMS.localization.translate('nameTitle', this.props.langSection), 
			desc: lewCMS.localization.translate('nameDesc', this.props.langSection)
		});
	},

	onNameFormSubmit: function(contentName){
	
		if(!!contentName && contentName != ''){
			lewCMS.store[this.props.storeSection].create(this.state.contentTypeId, contentName, this.props.parentId, function(success, response){
				
				if(success){
					lewCMS.events.trigger.pageCreated(response);
				} else {
					
				}

			});
			
		}

	},

	onNameFormCancel: function(){
	
		lewCMS.events.trigger.changeMainView('dashboard2');

	},

	render: function(){
		
		var
		component = '';

		if(this.state.contentTypeId){
			component = <ContentNameForm onNameFormSubmit={this.onNameFormSubmit} onCancel={this.onNameFormCancel} />
		} else {
			component = <ContentTypeList storeSection={this.props.storeSection} onContentTypeClick={this.onContentTypeClick} />;
		}

		return(
			<div data-lm-createcontent="">

				<div data-lm-titlebar="title-bar">
					<h1>{this.state.title}</h1>
					<p>{this.state.desc}</p>
				</div>
				{component}
			</div>
		);
	
	}

});
/** @jsx React.DOM */
var ContentNameForm = React.createClass({

	onSubmit: function(e, valid, errorMessages){
		
		if(valid && this.props.onNameFormSubmit){
			this.props.onNameFormSubmit.call(this, this.refs.textbox.val())
		}
	},

    render: function () {
		
		return(
            <StandardBox styleAttr="auto-width">

                <Form showSummary={false} onSubmit={this.onSubmit}>
					<TextBox 
						label="Page Name"
                        type="text"
                        value="New Page"
						showErrorMessage={true}
                        valProperties={
							{
								required: {
									errorMessage: 'This field is required'
								}
							}
						} 
					/>

					<div data-lm-actionrow="2 gutter-top">
						<button className="action" type="submit" data-lm-button="clr-green icon-check">Save</button>
						<button className="action" type="button" data-lm-button="clr-gray icon-cancel" onClick={this.props.onCancel}>Cancel</button>
					</div>

		        </Form>

            </StandardBox>
        );
    }

})
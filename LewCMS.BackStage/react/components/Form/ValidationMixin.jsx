/** @jsx React.DOM */

var ValidationMixin = {
    
    validationRules: {
        
            required: function (el) {
                return !!el.value && el.value !== '';
            },

            minLength: function (el, prop) {
                return el.value.length >= prop.limit;
            },

            maxLength: function (el, prop) {
                return el.value.length <= prop.limit;
            }
        
    },

    validate: function (changeEvent) {

        var

		node = changeEvent.target,
		val = node.value,
		valid = true,
		errorMessages = [],
		propIsValid;

        for (var prop in this.props.valProperties) {

            propIsValid = this.validationRules[prop](node, this.props.valProperties[prop]);
            valid = valid & propIsValid;
            if (!propIsValid && !!this.props.valProperties[prop].errorMessage) {
                errorMessages.push(this.props.valProperties[prop].errorMessage);
            }

        }

        this.setState({ valid: valid, hasErrorClass: !valid, errorMessages: errorMessages });
        return valid;
    }

};
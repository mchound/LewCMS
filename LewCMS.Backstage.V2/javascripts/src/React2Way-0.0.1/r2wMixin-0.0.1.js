var R2wMixin = {

    getInitialState: function () {
        if (!!this.props.model) return this.props.model.toState();
        return {};
    },

    componentWillMount: function () {
        if (!!this.props.model) this.props.model.onModelChange(function () {
            this.setState(this.props.model.toState());
        }.bind(this));
    },

    componentWillUpdate: function (nextProps, nextState) {
        if (!this.props.model) return;
        this.props.model.fromState(nextState);
    }

};
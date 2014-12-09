; (function (doc, win, contentService, React, r2w, undef) {

    'use strict';

    var

//#region Private properties

    leftPane = null,

    centerPane = null,

//#endregion

//#region Model

    model = {

        pageTypes: r2w.observable([]),

        pageTree: r2w.observable([])

    },

//#endregion

//#region Private methods

    init = function () {

        getElements();
        renderComponents();
        //getPageTypes();
        getPageTree();

    },

    getElements = function () {

        leftPane = doc.querySelector('#left-pane');
        centerPane = doc.querySelector('#center-pane');

    },

    renderComponents = function () {
        React.render(
          React.createElement(LeftPane, { pageTree: model.pageTree }),
          leftPane
        );
    },

    getPageTypes = function () {

        contentService.get.pageTypes(function (pageTypes) {

            pageTypes.forEach(function (pageType) {

                model.pageTypes.push(pageType);

            });

        }, []);

    },

    getPageTree = function () {

        contentService.get.pageTree(function (pageTree) {

            model.pageTree.push(pageTree);

        }, []);

    };

    //#endregion

    window.addEventListener('load', init);

})(document, window, lewCMS.contentService, React, React2Way);
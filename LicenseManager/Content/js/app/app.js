"use strict";

var $;
window.jQuery = $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;

// LM Namespace (hold-over from non CommonJS method)
var LM = {
    Backbone: {},
    Catalog: {},
    Collection: {
        Asset: {}
    },
    Debug: {
        Collections: {},
        Models: {},
        Views: {}
    },
    Components: {},
    Interactive: {},
    Model: {
        Asset: {}
    },
    Module: {
        Asset: {}
    },
    Pages: {},
    Routes: {},
    Utilities: {},
    UI: {
        Asset: {
            Edit: {},
            Navigator: {}
        },
        Catalog: {},
        Common: {}
    },
    Vent: _.extend({}, Backbone.Events),
    Views: {}
};

window.LM = LM;

// App Dependencies
var jquery_browser = require('jquery.browser'); // so that aui works
var Router = require('./router');

// Helpers
var ApplicationHelpers = require('./helpers/hbs_helpers');

var _rootUrl, _applicationPath, _currentUserId;

var Application = {

    /**
     * 
     */
    initialize: function (rootUrl, applicationPath, currentUser) {
        this.mapProperties();

        var parsedUserId;

        if (currentUser === undefined || currentUser === null) {
            parsedUserId = 0;
        } else {
            parsedUserId = parseInt(currentUser.Id, 10);
        }

        _rootUrl = this.buildRootUrl(rootUrl);
        _applicationPath = applicationPath;
        _currentUserId = parsedUserId;

        // register all the handlebars helpers
        ApplicationHelpers.initialize();
        this.initRouter();
    },

    /**
     * 
     */
    buildRootUrl: function (context) {
        if (context === "/") {
            return context;
        } else {
            if (context.charAt(context.length - 1) === "/") {
                return context;
            } else {
                return context + "/";
            }
        }
    },

    /**
     * 
     */
    mapProperties: function () {
        Object.defineProperty(LM, 'RootUrl', {
            get: function () {
                return _rootUrl;
            },
            set: function (value) {
                _rootUrl = value;
            }
        });

        Object.defineProperty(LM, 'ApplicationPath', {
            get: function () {
                return _applicationPath;
            },
            set: function (value) {
                _applicationPath = value;
            }
        });

        Object.defineProperty(LM, 'CurrentUserId', {
            get: function () {
                return _currentUserId;
            },
            set: function (value) {
                _currentUserId = value;
            }
        });
    },

    /**
     * News up a fresh router.
     */
    initRouter: function () {
        LM.Router = new Router();
        Backbone.history.start({ pushState: true, root: LM.ApplicationPath });
    }
};

Application.initialize(window.rootUrl, window.applicationPath, window.currentUser);

module.exports = Application;

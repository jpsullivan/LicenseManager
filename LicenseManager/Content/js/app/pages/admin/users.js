"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
var SubRoute = require('backbone.subroute');
Backbone.$ = $;

var BaseView = require('../../view');

// UI Components
var UserList = require('../../ui/admin/user/user-list');

/**
 *
 */
var UsersPage = BaseView.extend({
    templateName: null,
    subviews: {},
    events: {},
    options: {},

    presenter: function () { },

    initialize: function () {
        this.userList();
    },

    postRenderTemplate: function () {},

    /**
     * Add event handlers for the user list.
     */
    userList: function () {
        var view = new UserList();
    }
});

UsersPage.Router = Backbone.SubRoute.extend({
    routes: {
        "": "userList"
    },

    userList: function () {
        return new UsersPage().userList();
    }
});

module.exports = UsersPage;

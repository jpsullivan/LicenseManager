"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
var SubRoute = require('backbone.subroute');
Backbone.$ = $;

var BaseView = require('../../view');

// UI Components
var CustomerList = require('../../ui/admin/customer/customer-list');

/**
 *
 */
var CustomersPage = BaseView.extend({
    templateName: null,
    subviews: {},
    events: {},
    options: {},

    presenter: function () { },

    initialize: function () {
        this.customerList();
    },

    postRenderTemplate: function () { },

    /**
     * Add event handlers for the customer list.
     */
    customerList: function () {
        var view = new CustomerList();
    }
});

CustomersPage.Router = Backbone.SubRoute.extend({
    routes: {
        "": "userList"
    },

    userList: function () {
        return new CustomersPage().customerList();
    }
});

module.exports = CustomersPage;

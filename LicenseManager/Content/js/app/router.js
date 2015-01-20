"use strict";

var $ = require('jquery');
var Backbone = require('backbone');
Backbone.$ = $;

// Pages
var AdminCustomerPage = require('./pages/admin/customers');
var AdminUserPage = require('./pages/admin/users');

var Router = Backbone.Router.extend({
    routes: {
        "admin/customers": "adminCustomers",
        "admin/users": "adminUsers"
    },

    adminCustomers: function() {
        this.renderPage(function () {
            return new AdminCustomerPage();
        });
    },

    adminUsers: function () {
        this.renderPage(function () {
            return new AdminUserPage();
        });
    },

    renderPage: function (pageConstructor) {
        var existingView = {};
        if (LM.Page) {
            existingView = LM.Page;
            LM.Page.unbind && LM.Page.unbind(); // old page might mutate global events $(document).keypress, so unbind before creating
        }

        var page = pageConstructor();

        // remove the old view after the new one renders to prevent any paint flashes
        existingView.remove && existingView.remove();
    }
});

module.exports = Router;
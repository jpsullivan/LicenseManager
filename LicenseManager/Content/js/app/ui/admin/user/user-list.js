"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;

// Data Layer
var UsersCollection = require('../../../collections/users');

// UI Components
var BaseView = require('../../../view');
var DeleteUserDialog = require('./delete-dialog');

/**
 *
 */
var UserList = BaseView.extend({
    el: "table#list_users tbody",

    options: {},

    events: {
        "click .delete-user": "deleteUserClickHandler"
    },

    presenter: function () {
        return _.extend(this.defaultPresenter(), {

        });
    },

    initialize: function (options) {
        _.bindAll(this, "deleteUser");

        this.users = new UsersCollection();
        if (window.bootstrapData) {
            this.users.reset(window.bootstrapData.users);
        }
    },

    postRenderTemplate: function () { },

    /**
     * Takes the username from the delete button and sends it
     * to the delete user modal.
     */
    deleteUserClickHandler: function (e) {
        var name = $(e.currentTarget).data('for');
        var user = this.users.findWhere({ FullName: name });
        if (user) {
            this.showDeleteCustomerModal(user);
        }
    },

    /**
     * Displays the modal window confirming whether or not the 
     * provided user should be deleted.
     * @param {UserModel} user The user model to be deleted.
     */
    showDeleteCustomerModal: function (user) {
        var dialog = new DeleteUserDialog({ user: user, okFunc: this.deleteUser });
        $('#dialog_container').html(dialog.render().el);
        dialog.showModal();
    },

    /**
     * Deletes the provided user.
     * @param {UserModel} user The user model to be deleted.
     */
    deleteUser: function (user) {
        // remove the user row from the DOM manually since we aren't data binding
        this.$el('*[data-for="' + user.getFullName() + '"]').parents('tr').remove();
        user.destroy();
    }
});

module.exports = UserList;
"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;

var BaseView = require('../../../view');

/**
 *
 */
var DeleteUserDialog = BaseView.extend({
    templateName: "admin/user/delete-dialog",

    options: {
        user: null,
        okFunc: null
    },

    events: {},

    presenter: function () {
        return _.extend(this.defaultPresenter(), {
            displayName: "jaysche"
        });
    },

    initialize: function (options) {
        // so we can use options throughout each function
        this.options = options || {};

        // required sinced AJS overrides 'this'
        _.bindAll(this, 'submitModal', 'closeModal');
    },

    postRenderTemplate: function () {
        var self = this;
        _.defer(function () {
            self.modalAJS = AJS.dialog2("#delete_user_dialog");

            // handle event bindings here since AJS apparently overrides them...
            AJS.$('#dialog-close-button').on('click', self.closeModal);
            AJS.$('#dialog-create').on('click', self.submitModal);
        });
    },

    showModal: function () {
        var self = this;
        _.defer(function () {
            self.modalAJS.show();
        });
    },

    closeModal: function () {
        this.modalAJS.hide();
        //AJS.dialog2("#asset_var_selection_modal").hide();
        this.remove();
        this.modalAJS.remove();
        //AJS.dialog2("#asset_var_selection_modal").remove();
    },

    submitModal: function (e) {
        var el = $(e.currentTarget);
        el.attr('aria-disabled', "true");

        this.options.okFunc(this.options.user);
        this.closeModal();
    }
});

module.exports = DeleteUserDialog;

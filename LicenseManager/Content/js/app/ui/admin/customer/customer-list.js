"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;

// Data Layer
var CustomersCollection = require('../../../collections/customers');

// UI Components
var BaseView = require('../../../view');
var DeleteCustomerDialog = require('./delete-dialog');

/**
 *
 */
var CustomerList = BaseView.extend({
    el: "table#list_customers tbody",

    options: {},

    events: {
        "click .delete-customer": "deleteCustomerClickHandler"
    },

    presenter: function () {
        return _.extend(this.defaultPresenter(), {

        });
    },

    initialize: function (options) {
        _.bindAll(this, "deleteCustomer");

        this.customers = new CustomersCollection();
        if (window.bootstrapData) {
            this.customers.reset(window.bootstrapData.customers);
        }
    },

    postRenderTemplate: function () { },

    /**
     * Takes the customer name from the delete button and sends it
     * to the delete customer modal.
     */
    deleteCustomerClickHandler: function (e) {
        var name = $(e.currentTarget).data('for');
        var customer = this.customers.findWhere({ Name: name });
        if (customer) {
            this.showDeleteCustomerModal(customer);
        }
    },

    /**
     * Displays the modal window confirming whether or not the 
     * provided customer should be deleted.
     * @param {CustomerModel} customer The customer model to be deleted.
     */
    showDeleteCustomerModal: function (customer) {
        var dialog = new DeleteCustomerDialog({ customer: customer, okFunc: this.deleteCustomer });
        $('#dialog_container').html(dialog.render().el);
        dialog.showModal();
    },

    /**
     * Deletes the provided customer.
     * @param {CustomerModel} customer The customer model to be deleted.
     */
    deleteCustomer: function (customer) {
        customer.destroy();
    }
});

module.exports = CustomerList;
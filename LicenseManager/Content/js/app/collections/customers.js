"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
var Brace = require('backbone-brace');
Backbone.$ = $;

var CustomerModel = require('../models/customer');

/**
 * 
 */
var CustomersCollection = Brace.Collection.extend({
    model: CustomerModel,

    url: function () {
        return LM.RootUrl + "api/customers";
    }
});

module.exports = CustomersCollection;
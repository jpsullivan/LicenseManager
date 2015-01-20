"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
var Brace = require('backbone-brace');
Backbone.$ = $;

/**
 * 
 */
var CustomerModel = Brace.Model.extend({
    idAttribute: "Id",

    namedAttributes: [
        "Id",
        "Name",
        "BillingContact",
        "BillingContactEmail",
        "TechnicalContact",
        "TechnicalContactEmail",
        "IsHosted",
        "CreatedUtc"
    ]
});

module.exports = CustomerModel;

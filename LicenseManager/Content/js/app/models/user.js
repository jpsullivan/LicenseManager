"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
var Brace = require('backbone-brace');
Backbone.$ = $;

/**
 * 
 */
var UserModel = Brace.Model.extend({
    idAttribute: "Id",

//    url: function () {
//        return LM.RootUrl + "api/user";
//    },

    namedAttributes: [
        "Id",
        "Username",
        "EmailAddress",
        "FullName"
    ]
});

module.exports = UserModel;

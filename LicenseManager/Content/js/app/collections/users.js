"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
var Brace = require('backbone-brace');
Backbone.$ = $;

var UserModel = require('../models/user');

/**
 * 
 */
var UserCollection = Brace.Collection.extend({
    model: UserModel,

    url: function () {
        return LM.RootUrl + "api/users";
    }
});

module.exports = UserCollection;
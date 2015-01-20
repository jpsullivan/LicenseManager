"use strict";

var $ = require('jquery');
var _ = require('underscore');
var Backbone = require('backbone');
Backbone.$ = $;

// UI Components
var BaseView = require('../../../view');

/**
 *
 */
var SubmitSpinner = BaseView.extend({
    events: {
        "click .check-list-item input": "applyChanges",
        keydown: "_onKeydown"
    },

    options: {
        buttonElement: null,
        position: "before"
    },

    initialize: function (options) {
        this.button = $(options.buttonElement);
        this.spinner = $('<div class="submit-spinner invisible" />');

        if (options.position === "before") {
            this.spinner.insertBefore(this.button);
        } else {
            this.spinner.insertAfter(this.button);
        }
    },

    /**
     * Display the spinner.
     */
    show: function() {
        this.spinner.removeClass('invisible');
        this.spinner.spin();
    },

    /**
     * Hides the spinner.
     */
    hide: function() {
        this.spinner.addClass('invisible');
        this.spinner.spinStop();
    },

    dispose: function() {
        this.spinner.remove();
        this.remove();
    }
});

module.exports = SubmitSpinner;
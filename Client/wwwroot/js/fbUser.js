﻿window.fbUser = function () {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}
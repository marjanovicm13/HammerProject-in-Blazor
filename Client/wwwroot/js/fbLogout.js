window.location.fbLogout = function () {
    window.location.FB.logout(function (response) {
        console.log("in fb logout")
        console.log(response);
    });
};

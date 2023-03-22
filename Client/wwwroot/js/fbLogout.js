window.fbLogout = function () {
    window.FB.logout(function (response) {
        console.log("in fb logout")
        console.log(response);
    });
};

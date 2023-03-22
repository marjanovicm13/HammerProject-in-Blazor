fbLogout = function () {
    FB.logout(function (response) {
        console.log("in fb logout")
        console.log(response);
    });
};

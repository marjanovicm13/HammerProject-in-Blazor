window.fbAsyncInit = function () {
    window.FB.init({
        appId: '3156761404633872',
        cookie: true,
        xfbml: true,
        version: 'v15.0'
    });
    console.log("initialized");
    window.FB.AppEvents.logPageView();
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
    console.log("initialized");
}(document, 'script', 'facebook-jssdk'));
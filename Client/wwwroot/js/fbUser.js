window.fbUser = function () {
    window.FB.getLoginStatus(function (response) {
        console.log(response);
       // statusChangeCallback(response);

        DotNet.invokeMethodAsync('HammerProject.Client', 'FbUserProcessCallback', response).then(() => {

        });
    });
}
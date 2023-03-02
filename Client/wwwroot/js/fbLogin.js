window.fbLogin = function () {
    window.FB.login(function (response) {
        if (response.status === 'connected') {
            //console.log(response)
        } else {
            console.log("Not logged in.");
        }



        DotNet.invokeMethodAsync('HammerProject.Client', 'FbLoginProcessCallback', response).then(() => {

        });


    }, { scope: 'public_profile, email' });
};


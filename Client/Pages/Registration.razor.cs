using HammerProject.Client.HttpRepository;
using Microsoft.AspNetCore.Components;

namespace HammerProject.Client.Pages
{
    public partial class Registration
    {
        private UserForRegistrationDto _userForRegistrationDto = new UserForRegistrationDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string> RegistrationErrors { get; set; }

        public async Task Register()
        {
            ShowRegistrationErrors = false;
            var result = await AuthenticationService.RegisterUser(_userForRegistrationDto);
            if (!result.IsSuccessfulRegistration)
            {
                RegistrationErrors = result.Errors;
                ShowRegistrationErrors = true;
            }
            else
            {
                var userToLogin = new UserForAuthenticationDto { UserName = _userForRegistrationDto.UserName, Password = _userForRegistrationDto.Password };
                await AuthenticationService.Login(userToLogin);
                NavigationManager.NavigateTo("/");
            }
        }
    }
}

using HammerProject.Shared.Entitites.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HammerProject.Client.HttpRepository
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
        Task<AuthResponseDto> LoginFacebook(FacebookUserInfo userInfo);
        Task Logout();
        Task<string> RefreshToken();
    }
}

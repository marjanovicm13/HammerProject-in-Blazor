using Blazored.LocalStorage;
using HammerProject.Client.AuthProviders;
using HammerProject.Shared.Entitites.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HammerProject.Client.HttpRepository
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IJSRuntime jSRuntime; 

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, IJSRuntime jS)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            jSRuntime = jS;
        }   

        public async Task<RegistrationResponseDto> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            
            var content = JsonSerializer.Serialize(userForRegistration);
            var bodyContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            var registrationResult = await _httpClient.PostAsync("Account/Registration", bodyContent);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();

            if (!registrationResult.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RegistrationResponseDto>(registrationContent, _options);
                return result;
            }

            return new RegistrationResponseDto { IsSuccessfulRegistration = true };
        }
        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
        {
            var content = JsonSerializer.Serialize(userForAuthentication);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var authResult = await _httpClient.PostAsync("Account/Login", bodyContent);
            var authContent = await authResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, _options);
            if (!authResult.IsSuccessStatusCode)
                return result;
            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(userForAuthentication.UserName);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            return new AuthResponseDto { IsAuthSuccessful = true };
        }

        public async Task<AuthResponseDto> LoginFacebook(FacebookUserInfo userInfo)
        {
            var content = JsonSerializer.Serialize(userInfo);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var authResult = await _httpClient.PostAsync("Account/FacebookLogin", bodyContent);
            
            if (!authResult.IsSuccessStatusCode)
            {
               await _httpClient.PostAsync("Account/FacebookRegistration", bodyContent);
               authResult = await _httpClient.PostAsync("Account/FacebookLogin", bodyContent);
            }
            var authContent = await authResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, _options);

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(userInfo.Email.Substring(0, userInfo.Email.IndexOf('@')));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            
            return new AuthResponseDto { IsAuthSuccessful = true };
        }
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            await jSRuntime.InvokeAsync<object>("fbLogout");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var tokenDto = JsonSerializer.Serialize(new RefreshTokenDto { Token = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");

            var refreshResult = await _httpClient.PostAsync("Token/refresh", bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(refreshContent, _options);

            if (!refreshResult.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong during the refresh token action.");

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

            return result.Token;
        }
    }
}

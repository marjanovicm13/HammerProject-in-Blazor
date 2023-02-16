using Blazored.LocalStorage;
using HammerProject;
using HammerProject.Client;
using HammerProject.Client.AuthProviders;
using HammerProject.Client.HttpRepository;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Add auth services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped<HttpInterceptorService>();
builder.Services.AddSingleton<EmployeeStateContainerService>();

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://www.facebook.com/";
    options.ProviderOptions.ClientId = "3156761404633872";
    options.ProviderOptions.ResponseType = "token";
    options.ProviderOptions.DefaultScopes.Add("email");
});




await builder.Build().RunAsync();

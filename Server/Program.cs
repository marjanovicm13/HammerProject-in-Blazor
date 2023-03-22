using HammerProject.Server;
using HammerProject.Server.TokenHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    builder.WithOrigins("https://localhost:7253", "https://localhost:44391", "http://localhost:5197", "https://web.facebook.com/x/oauth/logout?access_token=EAAs3Dsop8xABALdJI2oNvtSKdy8ie4HOBc6VutYOi3bfna9RHq9CYXUnUXXFzRBvN1iLdRNYjMlM8lb1V70Hmk95CMRun3bM2xlssBtppH77TGr4ZAezzLv9iZABnc0Jk0wK5A59cNBZBDZCSxpHn9tZBiNdPpVuRVNZAEb45D1Xe707dFf5hx6xmRJFK1lbEZD&_rdc=1&_rdr")
             .AllowAnyHeader()
            .AllowAnyMethod()
   );
       
});

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//My code
//Add database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HammerProjectContext>(options => options.UseMySQL(connectionString));

//Asp.net core identity authentication
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<HammerProjectContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
});

//JWT configuration
var jwtSettings = builder.Configuration.GetSection("JWTSettings"); 
var facebookSettings = builder.Configuration.GetSection("Authentication:Facebook");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
    };
}).AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = facebookSettings["AppId"];
    facebookOptions.AppSecret = facebookSettings["AppSecret"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

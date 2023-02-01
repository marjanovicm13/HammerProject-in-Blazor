using HammerProject.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

//Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    builder.WithOrigins("https://localhost:7253", "https://localhost:44391", "http://localhost:5197")
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

//Identity server
builder.Services.AddDefaultIdentity<Login>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<HammerProjectContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<Login, HammerProjectContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

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
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

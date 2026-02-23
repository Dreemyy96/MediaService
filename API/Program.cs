using System.Text;
using Common.Models.ConfigModels;
using Identity.Persistence;
using IdentityCore.Models;
using Media.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Services.IdentityService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettingModel>(builder.Configuration.GetSection("JwtSetting"));

var audience = builder.Configuration["JwtSetting:Audience"];
var issuer = builder.Configuration["JwtSetting:Issuer"];
var secretKey = builder.Configuration["JwtSetting:SecretKey"];
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey
    };
});

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddTransient<IIdentityService, IdentityService>();

var identityConnection = builder.Configuration.GetConnectionString("IdentityDb");
var mediaConnection = builder.Configuration.GetConnectionString("MediaDb");
builder.Services.AddDbContext<IdentityContext>(options =>
{
    
});
builder.Services.AddDbContext<MediaContext>(options =>
{
    
});


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
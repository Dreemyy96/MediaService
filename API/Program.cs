using Common.Models.ConfigModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettingModel>(builder.Configuration.GetSection("JwtSetting"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using server.Extensions;
using server.Controllers;



var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
builder.Services.AddControllers();


builder.Services
    .AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration)
    .ConfigureAppSettings(builder.Configuration)
    .AddIdentityHandlerAndStores()
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration)//Authentication and Authorization here
    .AddCORS();
    ;
//builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));//here an instance of the appsetting class will be created and containing properties will be assigned with the value with the corresponding key value pair from appsetting section 




var app = builder.Build();

app.ConfigureSwaggerExplorer()
    .ConfigureCORS()
    .AddIdentityAuthMiddlewares();


app.MapGroup("/Account")
    .MapIdentityApi<AppUser>();
app.MapGroup("/Account").MapIdentityUserEndPoints();
app.MapGroup("/Account").MapAuthorizationDemoEndpoints();
app.MapAccountEndPoints();
app.UseHttpsRedirection();


app.Run();



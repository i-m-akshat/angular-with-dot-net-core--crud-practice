using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using server.Extensions;



var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
builder.Services.AddControllers();


builder.Services
    .AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration)
    .AddIdentityHandlerAndStores()
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration)
    .AddCORS()
    ;



#region Config.Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });
    options.AddPolicy("Common", policy =>
    {
        policy.RequireRole("");
    });
});
#endregion

var app = builder.Build();

app.ConfigureSwaggerExplorer()
    .ConfigureCORS()
    .AddIdentityAuthMiddlewares();


app.MapGroup("/Account")
    .MapIdentityApi<AppUser>();
app.MapPost("/Account/signup", async (
    UserManager<AppUser> userManager, [FromBody] UserRegistrationModel registrationModel
    ) =>
{
    AppUser user = new AppUser()
    {
        Email = registrationModel.Email,
        FullName = registrationModel.FullName,
        UserName = registrationModel.Email,
    };
    var res = await userManager.CreateAsync(user, registrationModel.Password);

    if (res.Succeeded)
    {
        return Results.Ok(res);
    }
    else
    {
        return Results.BadRequest(res);
    }
});

app.MapPost("/Account/signin", async (UserManager<AppUser> _userService, [FromBody] UserLoginModel loginModel) =>
{
    var res = await _userService.FindByEmailAsync(loginModel.email);
    if (res != null && await _userService.CheckPasswordAsync(res, loginModel.password))
    {
        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSetting:JWTSecretKey"]!));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]//claim array
            {
                new Claim("UserID",res.Id.ToString())//a particular claim with type and value
            }),//payload
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials
            (
                signInKey,
                SecurityAlgorithms.HmacSha256Signature
            )//header and sign 
        };
        var tokenHandler = new JwtSecurityTokenHandler();//token handler 
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);//creating token/
        var token = tokenHandler.WriteToken(securityToken);//serializing token
        return Results.Ok(new { token });
    }
    else
    {
        return Results.BadRequest(new { message = "username or password is incorrect" });
    }

});
app.UseHttpsRedirection();


app.Run();



public class UserRegistrationModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }

}

public class UserLoginModel
{
    public string email { get; set; }
    public string password { get; set; }
}
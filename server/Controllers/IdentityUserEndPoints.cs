using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

public static class IdentityUserEndPoints
{

    public class UserRegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int? LibraryID { get; set; }

    }

    public class UserLoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public static IEndpointRouteBuilder MapIdentityUserEndPoints(this IEndpointRouteBuilder app)//IEndpointRouteBuilder is a derived interface of WebApplication class and parent can activate the child class methods 

    {
        //app.MapPost("/Account/signup", CreateUser);
        app.MapPost("/signup", CreateUser);//since we are grouping the route in program.cs thats we dont need the account part here in the url
        app.MapPost("/signin", SignInUser);
        /* app.MapPost("/Account/signin", SignInUser);*///here not using paranthesis making it a delegate kind of thing MapPost will think this ia delegate handler and will hit this method 
                                                        //By not using parentheses, you're passing the SignInUser method as a method group conversion to app.MapPost.
                                                        //MapPost is expecting a delegate—a reference to a method that it can call later. The C# compiler automatically converts the SignInUser method name into this delegate. This means MapPost now knows which method to execute when an HTTP POST request is sent to the /Account/signin endpoint. The framework then handles the details of passing the correct parameters to your SignInUser method.
        return app;
    }
    [AllowAnonymous]
    public static async Task<IResult> CreateUser(UserManager<AppUser> userManager, [FromBody] UserRegistrationModel registrationModel
    )
    {
        AppUser user = new AppUser()
        {
            Email = registrationModel.Email,
            FullName = registrationModel.FullName,
            UserName = registrationModel.Email,
            Gender = registrationModel.Gender,
            DOB = DateOnly.FromDateTime(DateTime.Now.AddYears(-registrationModel.Age)),
            LibraryID= registrationModel.LibraryID,
        };
        var res = await userManager.CreateAsync(user, registrationModel.Password);
        await userManager.AddToRoleAsync(user,registrationModel.Role);

        if (res.Succeeded)
        {
            return Results.Ok(res);
        }
        else
        {
            return Results.BadRequest(res);
        }
    }
    [AllowAnonymous]
    public static async Task<IResult> SignInUser(UserManager<AppUser> _userService, [FromBody] UserLoginModel loginModel,IOptions<AppSetting> appSettings)
    {
        var res = await _userService.FindByEmailAsync(loginModel.email);
        if (res != null && await _userService.CheckPasswordAsync(res, loginModel.password))
        {
            /*
             1. get the signInKey means get JWT Secret key
             2. create an instance of SecurityTokenDescriptor and pass the values into its properties like Subject which will contain an instance of claim identity and inside that instance wwe will be passing claims array,
             3. Provide the expiry time
             4. SigningCredentials 
             5. Create an instance of JWT Security token handlers 
             6. here we will generate the token using instance of token handler class  using method CreateToken() and passing the token descriptor
             7.  
             */

            var roles =await _userService.GetRolesAsync(res);

            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecretKey));
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserID",res.Id.ToString()),
                new Claim("Gender",res.Gender.ToString()),
                new Claim("Age",(DateTime.Now.Year-res.DOB.Year).ToString()),
                new Claim(ClaimTypes.Role,roles.First())
            });
            if (res.LibraryID != null)
            {
                claims.AddClaim(new Claim("LibraryID", res.LibraryID.ToString()!));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,//payload
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
    }

}
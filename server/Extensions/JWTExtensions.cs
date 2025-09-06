using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace server.Extensions
{
    public static class JWTExtensions
    {
        public static IServiceCollection AddIdentityAuth(this IServiceCollection _services,IConfiguration configuration)
        {
           _services.AddAuthentication(options =>
            {
                //schemes=> registered authentication handlers and their configuration options are called scheme
                //these authentication handlers can be of any types like negotiate, cookie, or jwt bearer - in negotiate please follow the concept related to kereberos and NTLM
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//default scheme
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//scheme to how to handle unauthenticated request
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;//default scheme 
            }).AddJwtBearer(config =>
            {
                config.SaveToken = false;//where the token should be stored post successfull authentication
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSetting:JWTSecretKey"]!)),
                    ValidateActor = false,
                    ValidateIssuer = false
                };
            });
            return _services;
        }

        public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}

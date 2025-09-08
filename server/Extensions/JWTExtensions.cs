using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
                config.SaveToken = false;//whether the token should be stored post successfull authentication
                config.TokenValidationParameters = new TokenValidationParameters //token validation parameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSetting:JWTSecretKey"]!)),
                    ValidateIssuer = false,
                    ValidateAudience=false,
                };
            });

            _services.AddAuthorization(options =>
            {
               
                options.FallbackPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
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

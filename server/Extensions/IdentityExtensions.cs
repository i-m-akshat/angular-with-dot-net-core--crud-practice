using Microsoft.AspNetCore.Identity;
using server.Models;

namespace server.Extensions
{
    public static class IdentityExtensions
    {
       
        public static IServiceCollection AddIdentityHandlerAndStores(this IServiceCollection _services)//Iservicecollections extension method
        {
            _services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();
            return _services;
        }
        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection _services)//Iservicecollections extension method
        {

            _services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            });
            return _services;
        }

        
    }
}

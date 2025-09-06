using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace server.Extensions
{
    public static class EFCoreExtensions
    {
     
        public static IServiceCollection InjectDbContext(this IServiceCollection _services,IConfiguration configuration)
        {
            string conString = configuration.GetConnectionString("DevDB");
            _services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conString));
            return _services;
        }
    }
}

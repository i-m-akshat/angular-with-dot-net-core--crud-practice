using server.Models;

namespace server.Extensions
{
    public static class AppSettingsExtension
    {
        public static IServiceCollection ConfigureAppSettings(this IServiceCollection _services,IConfiguration _config)
        {
            _services.Configure<AppSetting>(_config.GetSection("AppSetting"));
            return _services;
        }
    }
}

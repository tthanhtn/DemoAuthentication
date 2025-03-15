using Demo.Business.Entity;

namespace Demo.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static AppSettings GetAppSettings(this IConfiguration configuration)
        {
            return configuration.GetSection("AppSettings").Get<AppSettings>();
        }
    }


}

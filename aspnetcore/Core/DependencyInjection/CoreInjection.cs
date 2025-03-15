using Demo.API.Interface;
using Demo.API.Services;

namespace Demo.Core.DependencyInjection
{
    public static class CoreInjection
    {
        public static void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICookiesAccount, CookiesAccount>();
            services.AddScoped<IJwtAccount, JwtAccount>();
        }
    }
}

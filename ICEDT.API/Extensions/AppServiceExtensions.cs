using ICEDT.API.Services.Implementation;
using ICEDT.API.Services.Interfaces;

namespace ICEDT.API.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILevelService, LevelService>();
            return services;
        }
    }
}
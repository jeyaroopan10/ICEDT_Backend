

using ICEDT.API.Repositories.Implementation;
using ICEDT.API.Repositories.Interfaces;

namespace ICEDT.API.Extensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILevelRepository, LevelRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();
            return services;
        }
    }
}

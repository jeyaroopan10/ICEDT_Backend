

using ICEDT.API.Repositories.Interfaces;

namespace ICEDT.API.Extensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILevelRepository, LevelRepository>();
            return services;
        }
    }
}

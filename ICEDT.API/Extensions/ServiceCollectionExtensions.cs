using ICEDT.API.Extensions;

namespace ICEDT.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddIdentityServices();
            services.ConfigureJwtAuthentication(configuration);
            services.ConfigureAuthorizationPolicies();
            services.AddSwaggerAndMvc(configuration);
            services.AddAppServices();
            return services;
        }


    }
}

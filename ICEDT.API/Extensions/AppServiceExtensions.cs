using Amazon.S3;
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
            services.AddScoped<IActivityService, ActivityService>();

            // AWS S3 client and MediaService registration
            services.AddAWSService<IAmazonS3>(); // Uses AWS SDK's credential provider chain
            services.AddScoped<IMediaService, MediaService>();

            return services;
        }
    }
}
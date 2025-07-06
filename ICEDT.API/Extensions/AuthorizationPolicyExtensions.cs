namespace ICEDT.API.Extensions
{
    public static class AuthorizationPolicyExtensions
    {
        public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("AddActivity", policy => policy.RequireClaim("Permission", "AddActivity"));

            });

            return services;
        }
    }
}

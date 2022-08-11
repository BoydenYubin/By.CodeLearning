using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Admin.WebAPI.Configuration
{
    public static class CorsConfig
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Default",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            return services;
        }

        public static void UseDefaultCors(this IApplicationBuilder app)
        {
            app.UseCors("Default");
        }

    }
}

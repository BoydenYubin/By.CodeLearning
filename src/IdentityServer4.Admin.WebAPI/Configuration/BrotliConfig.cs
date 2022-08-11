using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Admin.WebAPI.Configuration
{
    public static class BrotliConfig
    {
        public static void AddBrotliCompression(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.EnableForHttps = true;
            });
        }
    }
}

using ByProject.AutoUpgrade;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoUpgrade(this IServiceCollection services, Action<UpgradeOptions> config)
        {
            services.AddScoped<IAutoUpgradeProvider, AutoUpgradeProvider>();
            services.Configure(config);
        }
        public static void AddAutoUpgrade(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAutoUpgradeProvider, AutoUpgradeProvider>();
            services.Configure<UpgradeOptions>(config);
        }
    }
}
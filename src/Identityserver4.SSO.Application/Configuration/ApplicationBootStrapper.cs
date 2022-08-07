using ByLearning.SSO.Application.CloudServices.Email;
using ByLearning.SSO.Application.Interfaces;
using ByLearning.SSO.Application.Services;
using ByLearning.SSO.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ByLearning.SSO.Application.Configuration
{
    internal static class ApplicationBootStrapper
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IUserManageAppService, UserManagerAppService>();
            services.AddScoped<IRoleManagerAppService, RoleManagerAppService>();
            services.AddScoped<IEmailAppService, EmailAppService>();
            services.AddScoped<IEventStoreAppService, EventStoreAppService>();

            services.AddTransient<IEmailService, GeneralSmtpService>();
            services.AddTransient<IGlobalConfigurationSettingsService, GlobalConfigurationAppService>();
            services.AddTransient<IGlobalConfigurationAppService, GlobalConfigurationAppService>();

            return services;
        }
    }
}

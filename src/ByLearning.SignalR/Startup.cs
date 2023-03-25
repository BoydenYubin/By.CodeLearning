using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByLerning.SignalR
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddScoped<ITestInjection, TestInjection>();
            services.AddConnections();
            services.AddControllers();
            services.AddAuthentication(options => 
            {
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCertificate(options => 
            {
                options.ValidateCertificateUse = true;
            });
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapHub<ChartHub>("/charthub");
                endpoints.MapHub<StronglyTypedChatHub>("/intimechathub");
                //endpoints.MapHub<StreamHub>("/streamhub");
            });
        }
    }
}

using ByLearning.SSO.Application.Configuration;
using ByLearning.SSO.AspNetIdentity.Configuration;
using ByLearning.SSO.AspNetIdentity.Models.Identity;
using ByLearning.SSO.EntityFramework.Repository.Configuration;
using IdentityServer4.Services;
using IdentityServer4.SSO.AspNetIdentity;
using IdentityServer4.SSO.Database;
using IdentityServer4.SSO.Database.Context;
using IdentityServer4.SSO.WebUI.Configuration;
using IdentityServer4.SSO.WebUI.Util;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.SSO.WebUI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                //Microsoft.AspNetCore.Mvc.Razor;
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();
            services.AddRazorPages();
            // config the cookiePolicy
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.Secure = CookieSecurePolicy.SameAsRequest;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();

            // Add localization
            //services.AddMvcLocalization();

            // Dbcontext config
            // IdentityServer4 Config--Add DbContext
            services.ConfigureProviderForContext<SSOContext>(DetectDatabase);

            // ASP.NET Identity Configuration
            services
                .AddIdentity<UserIdentity, RoleIdentity>(AccountOptions.NistAccountOptions)
                .AddClaimsPrincipalFactory<ApplicationClaimsIdentityFactory>()   //---------------- added by JPP project author
                .AddEntityFrameworkStores<SSOContext>()                          // necessary => IUserStore  IRoleStore
                .AddDefaultTokenProviders();

            // Improve Identity password security
            //services.UpgradePasswordSecurity().UseArgon2<UserIdentity>();       //---------------- added by JPP project author

            // IdentityServer4 Configuration
            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                // add nuget package IdentityServer4.AspNetIdentity
                .AddAspNetIdentity<UserIdentity>()
                .ConfigureContext(DetectDatabase, _env)
                // find in configuration
                .AddProfileService<SSOProfileService>()   //---------------- added by JPP project author
                // Configure key material. By default it supports load balance scenarios and have a key managemente close to Key Management from original IdentityServer4
                // Unless you really know what are you doing, change it.
                .SetupKeyMaterial();                      //---------------- added by JPP project author

            // SSO Configuration
            services
                .ConfigureSSO<AspNetUser>()
                .AddSSOContext<SSOContext>()
                // If your ASP.NET Identity has additional fields, you can remove this line and implement IIdentityFactory<TUser> and IRoleFactory<TUser>
                // theses interfaces will able you to intercept Register / Update Flows from User and Roles
                .AddDefaultAspNetIdentityServices();   // IUserService, UserService<UserIdentity, RoleIdentity, string
                                                       // IRoleService, RoleService<RoleIdentity, string>>();
                                                       // IIdentityFactory<UserIdentity>, IdentityFactory>();
                                                       // IRoleFactory<RoleIdentity>, IdentityFactory>();

            // Configure Federation gateway (external logins), such as Facebook, Google etc
            //services.AddFederationGateway(Configuration);

            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));

            // .NET Native DI Abstraction
            RegisterServices(services);
        }

        /// <summary>
        /// it's just a tuple. Returns 2 parameters.
        /// Trying to improve readability at ConfigureServices
        /// </summary>
        private (DatabaseType, string) DetectDatabase => (
            Configuration.GetValue<DatabaseType>("ApplicationSettings:DatabaseType"),
            Configuration.GetConnectionString("SSOConnection"));

        private void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            services.AddScoped<IEventSink, IdentityServerEventStore>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Only use HTTPS redirect in Production Ambients
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction() && !env.IsBehindReverseProxy(Configuration))
            {
                app.UseHttpsRedirection();
                //NWebsec.AspNetCore.Middlewar
                //---------------- added by JPP project author
                app.UseHsts(options => options.MaxAge(days: 365));
            }

            app.UseSerilogRequestLogging();
            // ---------------- added by JPP project author
            app.UseSecurityHeaders(env, Configuration);
            app.UseStaticFiles();

            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                // Microsoft.AspNetCore.HttpOverrides
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(fordwardedHeaderOptions);

            app.UseIdentityServer();

            //app.UseLocalization();         

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

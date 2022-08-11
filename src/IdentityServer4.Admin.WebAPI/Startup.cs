using Hellang.Middleware.ProblemDetails;
using IdentityServer4.Admin.WebAPI.Configuration;
using IdentityServer4.Admin.WebAPI.Interfaces;
using IdentityServer4.SSO.Database.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace IdentityServer4.Admin.WebAPI
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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options => { options.RespectBrowserAcceptHeader = true; })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.AllowInputFormatterExceptionMessages = true;
                });
            // 可以确定下AddProblemDetails 中间件作用
            services.AddProblemDetails(options => options.IncludeExceptionDetails = (context, exception) => Environment.IsDevelopment());
            // Response compression
            services.AddBrotliCompression();
            // SSO configuration
            ConfigureApi(services);
            // Key material for API.
            // Data protection to persiste keys in database. 
            // It's necessary for Load balance scenarios
            services.AddDataProtection().SetApplicationName("sso").PersistKeysToDbContext<SSOContext>();
            // Cors request
            services.ConfigureCors();

            // Configure policies
            services.AddPolicies();

            // configure auth Server
            services.ConfigureOAuth2Server(Configuration);

            // configure openapi
            // services.AddSwagger(Configuration);

            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));

            // For Recaptcha service
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            // .NET Native DI Abstraction
            RegisterServices(services);
        }

        public virtual void ConfigureApi(IServiceCollection services)
        {
            services.ConfigureSSOApi(Configuration);
            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));
        }

        private void RegisterServices(IServiceCollection services)
        {
            //services.AddScoped<IUserService, MyUserService<UserIdentity, RoleIdentity, string>>();
            services.AddScoped<IReCaptchaService, ReCaptchaService>();
            // Adding dependencies from another layers (isolated from Presentation)
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseDefaultCors();
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseProblemDetails();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSO Api Management");
            //    c.OAuthClientId("Swagger");
            //    c.OAuthClientSecret("swagger");
            //    c.OAuthAppName("SSO Management Api");
            //    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

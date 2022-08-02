using ByLearning.IdentityServer4.HybridMVC.Extendsions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ByLearning.IdentityServer4.HybridMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthorization();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies") //使用Cookie作为验证用户的首选方式
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "http://localhost:5000"; //授权服务器地址
                options.RequireHttpsMetadata = false; //暂时不用https
                options.ClientId = "ByIdentity";
                options.ClientSecret = "6KGqzUx6nfZZp0a4NH2xenWSJQWAT8la";
                options.ResponseType = "code id_token"; //代表
                options.Scope.Add("hybrid_scope1"); //添加授权资源
                options.SaveTokens = true; //表示把获取的Token存到Cookie中
                options.GetClaimsFromUserInfoEndpoint = true;
                
            });
            services.ConfigureNonBreakingSameSiteCookies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

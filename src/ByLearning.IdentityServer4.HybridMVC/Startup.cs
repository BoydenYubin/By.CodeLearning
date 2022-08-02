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
            .AddCookie("Cookies") //ʹ��Cookie��Ϊ��֤�û�����ѡ��ʽ
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "http://localhost:5000"; //��Ȩ��������ַ
                options.RequireHttpsMetadata = false; //��ʱ����https
                options.ClientId = "ByIdentity";
                options.ClientSecret = "6KGqzUx6nfZZp0a4NH2xenWSJQWAT8la";
                options.ResponseType = "code id_token"; //����
                options.Scope.Add("hybrid_scope1"); //�����Ȩ��Դ
                options.SaveTokens = true; //��ʾ�ѻ�ȡ��Token�浽Cookie��
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

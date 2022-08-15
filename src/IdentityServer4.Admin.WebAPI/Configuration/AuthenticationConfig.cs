using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;

namespace IdentityServer4.Admin.WebAPI.Configuration
{
    public static class AuthenticationConfig
    {
        public static void ConfigureOAuth2Server(this IServiceCollection services, IConfiguration configuration)
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(o =>
                    {
                        o.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                        o.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                        //o.DefaultChallengeScheme = "oidc";
                    })
                    //.AddOpenIdConnect("oidc", options =>
                    //{
                    //    options.Authority = "https://localhost:5000"; //授权服务器地址
                    //    options.RequireHttpsMetadata = false; //暂时不用https
                    //    options.ClientId = "IS4-Admin";
                    //    //options.ClientSecret = "6KGqzUx6nfZZp0a4NH2xenWSJQWAT8la";
                    //    options.ResponseType = "code id_token"; //代表
                    //    //options.Scope.Add("hybrid_scope1"); //添加授权资源
                    //    options.SaveTokens = true; //表示把获取的Token存到Cookie中
                    //    options.GetClaimsFromUserInfoEndpoint = true;
                    //});
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = configuration.GetValue<string>("ApplicationSettings:Authority");
                        options.RequireHttpsMetadata = false;
                        options.ApiSecret = "Q&tGrEQMypEk.XxPU:%bWDZMdpZeJiyMwpLv4F7d**w9x:7KuJ#fy,E8KPHpKz++";
                        options.ApiName = "jp_api";
                        options.RoleClaimType = JwtClaimTypes.Role;
                        options.NameClaimType = JwtClaimTypes.Name;
                    });
        }
    }
}

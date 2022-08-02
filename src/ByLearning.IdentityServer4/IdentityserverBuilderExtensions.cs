using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ByLearning.IdentityServer4
{
    public static class IdentityserverBuilderExtensions
    {
        public static IIdentityServerBuilder AddInitDevelopInfo(this IIdentityServerBuilder builder)
        {
            builder.AddTestUsers(GetTestUsers().ToList());
            builder.AddInMemoryClients(GetClients());
            builder.AddInMemoryApiScopes(GetApiScope());
            builder.AddInMemoryApiResources(GetApiResource());
            builder.AddInMemoryIdentityResources(IdentityResources);
            return builder;
        }

        private static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        /// <summary>
        /// Authorization Server保护了哪些 API Scope（作用域）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScope()
        {
            return new[]
            {
                new ApiScope("hybrid_scope1")
            };
        }
        /// <summary>
        /// Authorization Server保护了哪些 API Resourc
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ApiResource> GetApiResource()
        {
            return new[]
           { 
               new ApiResource("ApiScope1", "ApiScope1_Display") 
               {
                   Scopes={ "hybrid_scope1" },
                   UserClaims={JwtClaimTypes.Role}, //添加Cliam 角色类型
                   ApiSecrets={new Secret("apipwd".Sha256())}
               }
           };
        }

        /// <summary>
        /// 哪些客户端 Client（应用） 可以使用这个 Authorization Server
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Client> GetClients()
        {
            return new[]
            {
               new Client()
               {
                     ClientId="ByIdentity", ///客户端的标识，要是惟一的
                     ClientName = "Hybird Auth",
                     ClientSecrets = {new Secret("6KGqzUx6nfZZp0a4NH2xenWSJQWAT8la".Sha256())}, ////客户端密码，进行了加密
                     RedirectUris = {"http://localhost:5002/signin-oidc" },
                     PostLogoutRedirectUris = {"http://localhost:5002/signout-callback-oidc"},
                     AllowedGrantTypes = GrantTypes.Hybrid, ////授权方式，这里采用的是客户端认证模式，只要ClientId，以及ClientSecrets
                    ///正确即可访问对应的AllowedScopes里面的api资源
                     AllowedScopes = {
                       "hybrid_scope1",
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,
                     }, //定义这个客户端可以访问的APi资源数组，上面只有一个api
                     //允许将token通过浏览器传递
                     AllowAccessTokensViaBrowser = true,
                     // 是否需要同意授权 （默认是false）
                     RequireConsent = true,
                     RequirePkce = false
               }
            };
        }

        /// <summary>
        /// 哪些User可以被这个AuthorizationServer识别并授权
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<TestUser> GetTestUsers()
        {
            return new[]
            {
                new TestUser
                {
                   SubjectId="001",
                   Username="boyden001",
                   Password="123456",
                   Claims = {
                        new Claim(JwtClaimTypes.Name, "boyden yol"),
                        new Claim(JwtClaimTypes.GivenName, "boyden"),
                        new Claim(JwtClaimTypes.FamilyName, "yol"),
                        new Claim(JwtClaimTypes.Email, "boyden@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
                   }
                }
            };
        }
    }
}

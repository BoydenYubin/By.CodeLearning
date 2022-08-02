using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace ByLearning.IdentityServer4
{
    public static class IdentityserverBuilderExtensions
    {
        public static IIdentityServerBuilder AddInitDevelopInfo(this IIdentityServerBuilder builder)
        {
            builder.AddTestUsers(GetTestUsers().ToList());
            builder.AddInMemoryClients(GetClients());
            builder.AddInMemoryApiScopes(GetApiScopes());
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
        private static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[] { new ApiScope("ApiScope1", "ApiScope2") };
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
                     ClientSecrets=new []{new Secret( "6KGqzUx6nfZZp0a4NH2xenWSJQWAT8la".Sha256())}, ////客户端密码，进行了加密
                     AllowedGrantTypes= GrantTypes.ClientCredentials, ////授权方式，这里采用的是客户端认证模式，只要ClientId，以及ClientSecrets
                    ///正确即可访问对应的AllowedScopes里面的api资源
                     AllowedScopes=new[]{"ApiScope1" }, //定义这个客户端可以访问的APi资源数组，上面只有一个api
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
                   Password="123456"
                }
            };
        }
    }
}

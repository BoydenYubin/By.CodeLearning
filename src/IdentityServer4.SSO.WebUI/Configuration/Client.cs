using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer4.SSO.WebUI.Configuration
{
    public static class Clients
    {

        public static IEnumerable<Client> GetAdminClient(IConfiguration configuration)
        {

            return new List<Client>
            {
                /*
                 * ID4 Admin Client
                 */
                new Client
                {

                    ClientId = "IS4-Admin",
                    ClientName = "IS4-Admin",
                    ClientUri = configuration["ApplicationSettings:IS4AdminUi"],
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = true,
                    RequireClientSecret = false,
                    //may need to change
                    RequirePkce = false,
                    AllowPlainTextPkce = false,
                    RedirectUris = new[] {
                        $"{configuration["ApplicationSettings:IS4AdminUi"]}/login-callback",
                        $"{configuration["ApplicationSettings:IS4AdminUi"]}/silent-refresh.html"
                    },
                    AllowedCorsOrigins = { configuration.GetValue<string>("ApplicationSettings:IS4AdminUi")},
                    PostLogoutRedirectUris = {$"{configuration["ApplicationSettings:IS4AdminUi"]}",},
                    LogoUri = "https://jpproject.blob.core.windows.net/images/jplogo.png",
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "jp_api.is4"
                    }
                },

                /*
                 * User Management Client - OpenID Connect implicit flow client
                 */
                new Client {
                    ClientId = "UserManagementUI",
                    ClientName = "User Management UI",
                    ClientUri = configuration["ApplicationSettings:UserManagementURL"],
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = true,
                    RequirePkce = true,
                    AllowPlainTextPkce = false,
                    RequireClientSecret = false,
                    RedirectUris =new[] {
                        $"{configuration["ApplicationSettings:UserManagementURL"]}/login-callback",
                        $"{configuration["ApplicationSettings:UserManagementURL"]}/silent-refresh.html"
                    },
                    AllowedCorsOrigins = { configuration["ApplicationSettings:UserManagementURL"] },
                    PostLogoutRedirectUris =  { $"{configuration["ApplicationSettings:UserManagementURL"]}" },
                    LogoUri = "https://jpproject.blob.core.windows.net/images/usermanagement.jpg",
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "jp_api.user",
                    }
                }

            };

        }
    }
}

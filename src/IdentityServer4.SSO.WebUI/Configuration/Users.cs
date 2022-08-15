using Microsoft.Extensions.Configuration;

namespace IdentityServer4.SSO.WebUI.Configuration
{
    public static class Users
    {
        public static string GetUser(IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApplicationSettings:DefaultUser") ?? "boydenyol";
        }
        public static string GetPassword(IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApplicationSettings:DefaultPass") ?? "Pa$$word123";
        }
        public static string GetEmail(IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApplicationSettings:DefaultEmail") ?? "boydenyol@gmail.com";
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4.SSO.WebUI.Util
{
    public static class Extensions
    {
        public static void AddIfDontExist(this List<Claim> claims, Claim newClaim)
        {
            if (claims.Any(c => c.Type == newClaim.Type))
                return;

            claims.Add(newClaim);
        }

        public static void Merge(this List<Claim> claims, IEnumerable<Claim> newClaim)
        {
            foreach (var claim in newClaim)
            {
                if (claims.Any(c => c.Type == claim.Type))
                    continue;

                claims.Add(claim);
            }
        }
        public static void Merge(this List<string> items, IEnumerable<string> content)
        {
            foreach (var claim in content)
            {
                if (items.Contains(claim))
                    continue;

                items.Add(claim);
            }
        }

        public static bool IsBehindReverseProxy(this IWebHostEnvironment host, IConfiguration configuration)
        {
            var config = configuration["ASPNETCORE_REVERSEPROXY"];
            return !string.IsNullOrEmpty(config) && config.Equals("true");
        }
    }
}

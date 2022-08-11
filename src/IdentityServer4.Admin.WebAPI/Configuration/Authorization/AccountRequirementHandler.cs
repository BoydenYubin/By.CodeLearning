using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Configuration
{
    public class AccountRequirementHandler : AuthorizationHandler<AccountRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAcceSSOr;

        public AccountRequirementHandler(IHttpContextAccessor httpContextAcceSSOr)
        {
            _httpContextAcceSSOr = httpContextAcceSSOr ?? throw new ArgumentNullException(nameof(httpContextAcceSSOr));
        }
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AccountRequirement requirement)
        {

            var httpMethod = _httpContextAcceSSOr.HttpContext.Request.Method;

            if (HttpMethods.IsGet(httpMethod) || HttpMethods.IsHead(httpMethod))
            {
                if (context.User.IsAuthenticated())
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            else
            {
                if (context.User.HasClaim("is4-rights", "manager") ||
                    context.User.IsInRole("Administrator"))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
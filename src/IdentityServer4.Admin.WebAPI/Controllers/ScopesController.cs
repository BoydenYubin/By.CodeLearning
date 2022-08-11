using ByLearning.Admin.Application.Interfaces;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Controllers
{
    [Route("[controller]"), Authorize(Policy = "Default")]
    public class ScopesController : ApiController
    {
        private readonly IScopesAppService _scopesAppService;

        public ScopesController(
            INotificationHandler<DomainNotification> notifications,
            IEventBus bus,
            IScopesAppService scopesAppService
            ) : base(notifications, bus)
        {
            _scopesAppService = scopesAppService;
        }

        [HttpGet, Route("{scope}")]
        public async Task<ActionResult<IEnumerable<string>>> Search(string scope)
        {
            var clients = await _scopesAppService.GetScopes(scope);
            return ResponseGet(clients);
        }
    }
}
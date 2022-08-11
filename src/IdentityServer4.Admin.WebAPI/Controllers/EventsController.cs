using ByLearning.Admin.Application.Interfaces;
using ByLearning.DDD.Domain.Core.ViewModels;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Notifications;
using ByLearning.SSO.Application.EventSourcedNormalizers;
using ByLearning.SSO.Application.Interfaces;
using ByLearning.SSO.Application.ViewModels.EventsViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Controllers
{

    [Route("events"), Authorize(Policy = "Default")]
    public class EventsController : ApiController
    {
        private readonly IEventStoreAppService _eventStoreAppService;
        private readonly IIdentityServerEventStore _identityServerEventStore;
        private readonly ISystemUser _user;

        public EventsController(
            INotificationHandler<DomainNotification> notifications,
            IEventBus bus,
            IEventStoreAppService eventStoreAppService,
            IIdentityServerEventStore identityServerEventStore,
            ISystemUser user) : base(notifications, bus)
        {
            _eventStoreAppService = eventStoreAppService;
            _identityServerEventStore = identityServerEventStore;
            _user = user;
        }

        /// <summary>
        /// Get a list of events by some aggregate
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("")]
        public ActionResult<ListOf<EventHistoryData>> ShowLogs([FromQuery] SearchEventByAggregate query)
        {
            var clients = _eventStoreAppService.GetEvents(query);

            if (!User.IsInRole("Administrator") && !User.HasClaim(c => c.Type == "is4-manager") && query.Aggregate != _user.Username)
            {
                foreach (var client in clients.Collection)
                {
                    client.MarkAsSensitiveData();
                }
            }

            return ResponseGet(clients);
        }

        /// <summary>
        /// Get a list aggregates
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("aggregates")]
        public async Task<ActionResult<IEnumerable<EventSelector>>> ListAggreggates()
        {
            var is4Aggregates = await _identityServerEventStore.ListAggregates();
            var SSOAggregates = await _eventStoreAppService.ListAggregates();

            return ResponseGet(is4Aggregates.Concat(SSOAggregates));
        }


    }
}

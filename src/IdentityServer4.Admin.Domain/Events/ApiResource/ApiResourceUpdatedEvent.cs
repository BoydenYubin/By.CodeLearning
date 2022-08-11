using System;
using ByLearning.Admin.Domain.Commands.ApiResource;
using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.ApiResource
{
    public class ApiResourceUpdatedEvent : Event
    {
        public IdentityServer4.Models.ApiResource ApiResource { get; }

        public ApiResourceUpdatedEvent(IdentityServer4.Models.ApiResource api)
        {
            ApiResource = api;
            AggregateId = api.Name;
        }
    }
}
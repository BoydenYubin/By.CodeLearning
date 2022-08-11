using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.ApiResource
{
    public class ApiResourceRegisteredEvent : Event
    {
        public IdentityServer4.Models.ApiResource Resource { get; }

        public ApiResourceRegisteredEvent(IdentityServer4.Models.ApiResource name)
        {
            Resource = name;
            AggregateId = name.Name;
        }
    }
}
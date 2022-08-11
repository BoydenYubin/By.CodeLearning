using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.IdentityResource
{
    public class IdentityResourceRemovedEvent : Event
    {
        public IdentityResourceRemovedEvent(string name)
            : base(EventTypes.Success)
        {
            AggregateId = name;
        }
    }
}
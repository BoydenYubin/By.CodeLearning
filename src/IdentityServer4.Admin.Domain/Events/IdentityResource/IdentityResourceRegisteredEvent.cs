using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.IdentityResource
{
    public class IdentityResourceRegisteredEvent : Event
    {
        public IdentityResourceRegisteredEvent(string name)
            : base(EventTypes.Success)
        {
            AggregateId = name;
        }
    }
}
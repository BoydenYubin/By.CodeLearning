using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.Role
{
    public class RoleRemovedEvent : Event
    {
        public RoleRemovedEvent(string name)
            : base(EventTypes.Success)
        {
            AggregateId = name;
        }
    }
}

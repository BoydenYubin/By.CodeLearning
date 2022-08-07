using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.Role
{
    public class RoleSavedEvent : Event
    {
        public RoleSavedEvent(string name)
            : base(EventTypes.Success)
        {
            AggregateId = name;
        }
    }
}
using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.Role
{
    public class RoleUpdatedEvent : Event
    {
        public string Name { get; }
        public string OldName { get; }

        public RoleUpdatedEvent(string name, string oldName)
            : base(EventTypes.Success)
        {
            AggregateId = name;
            Name = name;
            OldName = oldName;
        }
    }
}

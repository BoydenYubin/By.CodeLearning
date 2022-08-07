using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.UserManagement
{
    public class PasswordCreatedEvent : Event
    {

        public PasswordCreatedEvent(string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
        }
    }
}
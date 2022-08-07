using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.UserManagement
{
    public class AccountRemovedEvent : Event
    {
        public AccountRemovedEvent(string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
        }
    }
}
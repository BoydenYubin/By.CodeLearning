using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.User
{
    public class AdminChangedPasswordEvent : Event
    {
        public AdminChangedPasswordEvent(string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
        }
    }
}
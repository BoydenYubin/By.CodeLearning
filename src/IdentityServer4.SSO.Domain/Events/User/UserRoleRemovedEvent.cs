using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.User
{
    public class UserRoleRemovedEvent : Event
    {
        public string Username { get; }
        public string Role { get; }

        public UserRoleRemovedEvent(string username, string role)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Username = username;
            Role = role;
        }
    }
}
using System;
using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.User
{
    public class UserRoleSavedEvent : Event
    {
        public string Username { get; }
        public string Role { get; }

        public UserRoleSavedEvent(string username, string role)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Username = username;
            Role = role;
        }
    }
}
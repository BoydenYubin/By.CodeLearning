using System;
using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.UserManagement
{
    public class PasswordChangedEvent : Event
    {

        public PasswordChangedEvent(string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
        }
    }
}
﻿using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.User
{
    public class UserRegisteredEvent : Event
    {
        public string Username { get; }
        public string UserEmail { get; }

        public UserRegisteredEvent(string username, string userEmail)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Username = username;
            UserEmail = userEmail;
        }
    }
}
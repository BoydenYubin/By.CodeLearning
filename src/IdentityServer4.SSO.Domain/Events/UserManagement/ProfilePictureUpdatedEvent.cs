using System;
using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.UserManagement
{
    public class ProfilePictureUpdatedEvent : Event
    {
        public string Picture { get; }

        public ProfilePictureUpdatedEvent(string username, string picture)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Picture = picture;
        }
    }
}
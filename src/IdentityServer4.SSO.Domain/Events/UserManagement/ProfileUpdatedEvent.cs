using System;
using ByLearning.Domain.Core.Events;
using ByLearning.SSO.Domain.Commands.UserManagement;

namespace ByLearning.SSO.Domain.Events.UserManagement
{
    public class ProfileUpdatedEvent : Event
    {
        public UpdateProfileCommand Request { get; }

        public ProfileUpdatedEvent(string username, UpdateProfileCommand request)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Request = request;
        }
    }
}

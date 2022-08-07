using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.Email
{
    public class EmailSavedEvent : Event
    {
        public Models.Email Email { get; }

        public EmailSavedEvent(Models.Email email)
            : base(EventTypes.Success)
        {
            Email = email;
            AggregateId = email.Type.ToString();
        }

    }
}
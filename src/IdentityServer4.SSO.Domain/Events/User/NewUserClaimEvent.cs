using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.User
{
    public class NewUserClaimEvent : Event
    {
        public string Username { get; }
        public string Type { get; }
        public string Value { get; }

        public NewUserClaimEvent(string username, string type, string value)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Username = username;
            Type = type;
            Value = value;
        }
    }
}
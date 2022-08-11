using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.Client
{
    public class NewClientClaimEvent : Event
    {
        public string Type { get; }
        public string Value { get; }

        public NewClientClaimEvent(string clientId, string type, string value)
            : base(EventTypes.Success)
        {
            AggregateId = clientId;
            Type = type;
            Value = value;
        }
    }
}
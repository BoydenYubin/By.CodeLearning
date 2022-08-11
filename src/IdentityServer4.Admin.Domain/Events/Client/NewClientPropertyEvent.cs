using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.Client
{
    public class NewClientPropertyEvent : Event
    {
        public string Key { get; }
        public string Value { get; }

        public NewClientPropertyEvent(string clientId, string key, string value)
        {
            AggregateId = clientId;
            Key = key;
            Value = value;
        }
    }

}
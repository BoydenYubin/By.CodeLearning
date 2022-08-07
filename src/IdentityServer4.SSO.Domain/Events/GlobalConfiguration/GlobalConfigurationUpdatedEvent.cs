using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.GlobalConfiguration
{
    public class GlobalConfigurationUpdatedEvent : Event
    {
        public string Key { get; }
        public string Value { get; }
        public bool IsPublic { get; }
        public bool Sensitive { get; }

        public GlobalConfigurationUpdatedEvent(string key, string value, in bool isPublic, in bool sensitive)
        {
            AggregateId = key;
            Key = key;
            Value = value;
            IsPublic = isPublic;
            Sensitive = sensitive;
        }
    }
}
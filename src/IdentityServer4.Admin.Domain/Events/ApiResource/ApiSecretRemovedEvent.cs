using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.ApiResource
{
    public class ApiSecretRemovedEvent : Event
    {
        public string Type { get; }

        public ApiSecretRemovedEvent(string type, string resourceName)
        {
            Type = type;
            AggregateId = resourceName;
        }
    }
}
using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.ApiResource
{

    public class ApiSecretSavedEvent : Event
    {
        public string Type { get; }

        public ApiSecretSavedEvent(string type, string resourceName)
        {
            Type = type;
            AggregateId = resourceName;
        }
    }
}

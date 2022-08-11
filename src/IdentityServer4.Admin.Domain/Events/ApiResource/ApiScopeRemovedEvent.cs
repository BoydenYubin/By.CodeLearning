using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.ApiResource
{
    public class ApiScopeRemovedEvent : Event
    {
        public string Name { get; }

        public ApiScopeRemovedEvent(string name, string resourceName)
        {
            AggregateId = resourceName;
            Name = name;
        }
    }
}
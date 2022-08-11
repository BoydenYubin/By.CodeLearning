using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.ApiResource
{
    public class ApiResourceRemovedEvent : Event
    {
        public ApiResourceRemovedEvent(string name)
        {
            AggregateId = name;
        }
    }
}
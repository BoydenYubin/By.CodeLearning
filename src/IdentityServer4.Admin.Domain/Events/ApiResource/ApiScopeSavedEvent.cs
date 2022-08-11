using ByLearning.Admin.Domain.Commands.ApiResource;
using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.ApiResource
{
    public class ApiScopeSavedEvent : Event
    {
        public SaveApiScopeCommand Scope { get; }

        public ApiScopeSavedEvent(string resourceName, SaveApiScopeCommand scope)
        {
            Scope = scope;
            AggregateId = resourceName;
        }
    }
}
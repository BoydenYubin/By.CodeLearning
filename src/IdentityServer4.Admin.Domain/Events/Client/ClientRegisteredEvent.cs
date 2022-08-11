using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.Client
{
    public class ClientRemovedEvent : Event
    {
        public ClientRemovedEvent(string clientId)
            : base(EventTypes.Success)
        {
            AggregateId = clientId;
        }
    }
}
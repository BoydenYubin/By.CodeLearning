using ByLearning.Admin.Domain.Commands.Clients;
using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.Client
{
    public class ClientUpdatedEvent : Event
    {
        public UpdateClientCommand Request { get; }

        public ClientUpdatedEvent(UpdateClientCommand request)
            : base(EventTypes.Success)
        {
            Request = request;
            AggregateId = request.Client.ClientId;
        }
    }
}
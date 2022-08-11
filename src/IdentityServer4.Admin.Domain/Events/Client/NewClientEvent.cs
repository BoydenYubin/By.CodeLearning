using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.Client
{
    public class NewClientEvent : Event
    {
        public IdentityServer4.Models.Client Client { get; }

        public NewClientEvent(IdentityServer4.Models.Client client)
            : base(EventTypes.Success)
        {
            Client = client;
            AggregateId = client.ClientId;
        }
    }
}
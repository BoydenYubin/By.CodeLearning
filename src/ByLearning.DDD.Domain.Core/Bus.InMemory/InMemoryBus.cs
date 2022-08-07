using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Commands;
using ByLearning.Domain.Core.Events;
using ByLearning.Domain.Core.Notifications;
using MediatR;
using System.Threading.Tasks;

namespace ByLearning.Domain.Core.Bus
{
    public sealed class InMemoryBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly IEventStoreService _eventStore;
        public InMemoryBus(IMediator mediator, IEventStoreService eventStore)
        {
            _mediator = mediator;
            _eventStore = eventStore;
        }

        public Task<bool> SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send<bool>(command);
        }
        public async Task Publish<T>(T @event) where T : Event
        {
            if (!@event.MessageType.Equals(nameof(DomainNotification)))
                await _eventStore.Save(@event);

            await _mediator.Publish(@event);
        }
    }
}

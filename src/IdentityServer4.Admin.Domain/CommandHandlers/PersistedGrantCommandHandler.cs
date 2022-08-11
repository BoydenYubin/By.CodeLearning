using ByLearning.Admin.Domain.Commands.PersistedGrant;
using ByLearning.Admin.Domain.Events.PersistedGrant;
using ByLearning.Admin.Domain.Interfaces;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Commands;
using ByLearning.Domain.Core.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ByLearning.Domain.Core.Interfaces;

namespace ByLearning.Admin.Domain.CommandHandlers
{
    public class PersistedGrantCommandHandler : CommandHandler,
        IRequestHandler<RemovePersistedGrantCommand, bool>
    {
        private readonly IPersistedGrantRepository _persistedGrantRepository;

        public PersistedGrantCommandHandler(
            IUnitOfWork uow,
            IEventBus bus,
            INotificationHandler<DomainNotification> notifications,
            IPersistedGrantRepository persistedGrantRepository) : base(uow, bus, notifications)
        {
            _persistedGrantRepository = persistedGrantRepository;
        }


        public async Task<bool> Handle(RemovePersistedGrantCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false; ;
            }

            var grant = await _persistedGrantRepository.GetGrant(request.Key);
            if (grant == null)
                return false;

            _persistedGrantRepository.Remove(grant);

            if (await Commit())
            {
                await Bus.Publish(new PersistedGrantRemovedEvent(request.Key, grant.ClientId, grant.Type, grant.SubjectId));
                return true;
            }
            return false;
        }

    }
}
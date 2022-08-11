using ByLearning.Admin.Domain.Commands.IdentityResource;
using ByLearning.Admin.Domain.Events.IdentityResource;
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
    public class IdentityResourceCommandHandler : CommandHandler,
        IRequestHandler<RegisterIdentityResourceCommand, bool>,
        IRequestHandler<UpdateIdentityResourceCommand, bool>,
        IRequestHandler<RemoveIdentityResourceCommand, bool>
    {
        private readonly IIdentityResourceRepository _identityResourceRepository;

        public IdentityResourceCommandHandler(
            IUnitOfWork uow,
            IEventBus bus,
            INotificationHandler<DomainNotification> notifications,
            IIdentityResourceRepository identityResourceRepository) : base(uow, bus, notifications)
        {
            _identityResourceRepository = identityResourceRepository;
        }


        public async Task<bool> Handle(RegisterIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _identityResourceRepository.GetByName(request.Resource.Name);
            if (savedClient != null)
            {
                await Bus.Publish(new DomainNotification("Identity Resource", "Resource already exists"));
                return false;
            }

            var irs = request.Resource;

            _identityResourceRepository.Add(irs);

            if (await Commit())
            {
                await Bus.Publish(new IdentityResourceRegisteredEvent(irs.Name));
                return true;
            }
            return false;

        }

        public async Task<bool> Handle(UpdateIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _identityResourceRepository.GetByName(request.OldIdentityResourceName);
            if (savedClient == null)
            {
                await Bus.Publish(new DomainNotification("Identity Resource", "Resource not found"));
                return false;
            }

            await _identityResourceRepository.UpdateWithChildrens(request.OldIdentityResourceName, request.Resource);
            if (await Commit())
            {
                await Bus.Publish(new IdentityResourceUpdatedEvent(request.Resource));
                return true;
            }
            return false;
        }


        public async Task<bool> Handle(RemoveIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var identityResource = await _identityResourceRepository.GetByName(request.Resource.Name);
            if (identityResource == null)
            {
                await Bus.Publish(new DomainNotification("Identity Resource", "Resource not found"));
                return false;
            }

            _identityResourceRepository.Remove(identityResource);

            if (await Commit())
            {
                await Bus.Publish(new IdentityResourceRemovedEvent(request.Resource.Name));
                return true;
            }
            return false;
        }
    }
}
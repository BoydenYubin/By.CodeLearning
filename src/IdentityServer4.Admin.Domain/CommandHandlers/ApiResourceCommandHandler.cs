using ByLearning.Admin.Domain.Commands.ApiResource;
using ByLearning.Admin.Domain.Events.ApiResource;
using ByLearning.Admin.Domain.Interfaces;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Commands;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Notifications;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearning.Admin.Domain.CommandHandlers
{
    public class ApiResourceCommandHandler : CommandHandler,
        IRequestHandler<RegisterApiResourceCommand, bool>,
        IRequestHandler<UpdateApiResourceCommand, bool>,
        IRequestHandler<RemoveApiResourceCommand, bool>,
        IRequestHandler<RemoveApiSecretCommand, bool>,
        IRequestHandler<SaveApiSecretCommand, bool>,
        IRequestHandler<RemoveApiScopeCommand, bool>,
        IRequestHandler<SaveApiScopeCommand, bool>
    {
        private readonly IApiResourceRepository _apiRepository;

        public ApiResourceCommandHandler(
            IUnitOfWork uow,
            IEventBus bus,
            INotificationHandler<DomainNotification> notifications,
            IApiResourceRepository apiRepository) : base(uow, bus, notifications)
        {
            _apiRepository = apiRepository;
        }


        public async Task<bool> Handle(RegisterApiResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.Resource.Name);
            if (savedClient != null)
            {
                await Bus.Publish(new DomainNotification("Api", "Resource already exists"));
                return false;
            }

            _apiRepository.Add(request.ToModel());
            if (await Commit())
            {
                await Bus.Publish(new ApiResourceRegisteredEvent(request.Resource));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(UpdateApiResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.OldResourceName);
            if (savedClient == null)
            {
                await Bus.Publish(new DomainNotification("Api", "Resource not found"));
                return false;
            }

            await _apiRepository.UpdateWithChildrens(request.OldResourceName, request.Resource);

            if (await Commit())
            {
                await Bus.Publish(new ApiResourceUpdatedEvent(request.Resource));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveApiResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.Resource.Name);
            if (savedClient == null)
            {
                await Bus.Publish(new DomainNotification("Api", "Resource not found"));
                return false;
            }

            _apiRepository.Remove(savedClient);

            if (await Commit())
            {
                await Bus.Publish(new ApiResourceRemovedEvent(request.Resource.Name));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveApiSecretCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.ResourceName);
            if (savedClient == null)
            {
                await Bus.Publish(new DomainNotification("Api", "Api not found"));
                return false;
            }

            if (!savedClient.ApiSecrets.Any(f => f.Type == request.Type && f.Value == request.Value))
            {
                await Bus.Publish(new DomainNotification("Api Secret", "Invalid secret"));
                return false;
            }

            _apiRepository.RemoveSecret(request.ResourceName, request.ToModel());

            if (await Commit())
            {
                await Bus.Publish(new ApiSecretRemovedEvent(request.Type, request.ResourceName));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SaveApiSecretCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedApi = await _apiRepository.GetResource(request.ResourceName);
            if (savedApi == null)
            {
                await Bus.Publish(new DomainNotification("Api", "Api not found"));
                return false;
            }


            _apiRepository.AddSecret(request.ResourceName, request.ToModel());

            if (await Commit())
            {
                await Bus.Publish(new ApiSecretSavedEvent(request.Type, request.ResourceName));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveApiScopeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.ResourceName);
            if (savedClient == null)
            {
                await Bus.Publish(new DomainNotification("Api", "Api not found"));
                return false;
            }

            if (savedClient.Scopes.Any(f => f.Name != request.Name))
            {
                await Bus.Publish(new DomainNotification("Api Scope", "Invalid scope"));
                return false;
            }

            _apiRepository.RemoveScope(request.ResourceName, request.Name);

            if (await Commit())
            {
                await Bus.Publish(new ApiScopeRemovedEvent(request.Name, request.ResourceName));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SaveApiScopeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedApi = await _apiRepository.GetResource(request.ResourceName);
            if (savedApi == null)
            {
                await Bus.Publish(new DomainNotification("Api", "Api not found"));
                return false;
            }

            _apiRepository.AddScope(request.ResourceName, request.ToModel());

            if (await Commit())
            {
                await Bus.Publish(new ApiScopeSavedEvent(request.ResourceName, request));
                return true;
            }
            return false;
        }
    }
}
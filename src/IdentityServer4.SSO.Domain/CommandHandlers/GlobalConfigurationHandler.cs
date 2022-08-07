using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Commands;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Notifications;
using ByLearning.SSO.Domain.Commands.GlobalConfiguration;
using ByLearning.SSO.Domain.Events.GlobalConfiguration;
using ByLearning.SSO.Domain.Interfaces;
using ByLearning.SSO.Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearning.SSO.Domain.CommandHandlers
{
    public class GlobalConfigurationHandler :
        CommandHandler,
        IRequestHandler<ManageConfigurationCommand, bool>
    {
        private readonly IGlobalConfigurationSettingsRepository _globalConfigurationSettingsRepository;

        public GlobalConfigurationHandler(
            IUnitOfWork uow,
            IEventBus bus,
            INotificationHandler<DomainNotification> notifications,
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository) : base(uow, bus, notifications)
        {
            _globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;
        }


        public async Task<bool> Handle(ManageConfigurationCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var setting = await _globalConfigurationSettingsRepository.FindByKey(request.Key);
            if (setting is null)
                return await CreateConfiguration(request);

            return await UpdateConfiguration(setting, request);
        }

        public async Task<bool> UpdateConfiguration(
            GlobalConfigurationSettings setting,
            ManageConfigurationCommand request)
        {
            setting.Update(request.Value, request.IsPublic, request.Sensitive);
            _globalConfigurationSettingsRepository.Update(setting);

            if (await Commit())
            {
                await Bus.Publish(new GlobalConfigurationUpdatedEvent(request.Key, request.Sensitive ? "Sensitive information" : request.Value, request.IsPublic, request.Sensitive));
                return true;
            }

            return false;
        }

        private async Task<bool> CreateConfiguration(ManageConfigurationCommand request)
        {
            // Businness logic here
            _globalConfigurationSettingsRepository.Add(request.ToEntity());

            if (await Commit())
            {
                await Bus.Publish(new GlobalConfigurationCreatedEvent(request.Key,
                    request.Sensitive ? "Sensitive information" : request.Value, request.IsPublic, request.Sensitive));
                return true;
            }

            return false;
        }
    }
}

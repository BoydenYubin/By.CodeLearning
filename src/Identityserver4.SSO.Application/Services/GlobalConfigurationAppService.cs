using AutoMapper;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.SSO.Application.AutoMapper;
using ByLearning.SSO.Application.Interfaces;
using ByLearning.SSO.Application.ViewModels;
using ByLearning.SSO.Domain.Commands.GlobalConfiguration;
using ByLearning.SSO.Domain.Interfaces;
using ByLearning.SSO.Domain.ViewModels.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.SSO.Application.Services
{
    public class GlobalConfigurationAppService : IGlobalConfigurationAppService
    {
        public IEventBus Bus { get; }
        private readonly IMapper _mapper;
        private readonly IGlobalConfigurationSettingsRepository _globalConfigurationSettingsRepository;

        private readonly ISystemUser _systemUser;

        public GlobalConfigurationAppService(
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository,
            ISystemUser systemUser,
            IEventBus bus)
        {
            Bus = bus;
            _mapper = GlobalConfigurationMapping.Mapper;
            _globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;

            _systemUser = systemUser;
        }

        public async Task<PrivateSettings> GetPrivateSettings()
        {
            var settings = await _globalConfigurationSettingsRepository.All();
            var privateSettings = new PrivateSettings(new Settings(settings));

            return privateSettings;
        }

        public async Task<PublicSettings> GetPublicSettings()
        {
            var settings = await _globalConfigurationSettingsRepository.All();

            var publicSettings = new PublicSettings(new Settings(settings));

            return publicSettings;
        }

        public async Task<bool> UpdateSettings(IEnumerable<ConfigurationViewModel> configs)
        {
            var success = true;
            foreach (var configurationViewModel in configs)
            {
                success = await Bus.SendCommand(_mapper.Map<ManageConfigurationCommand>(configurationViewModel));
                if (!success)
                    break;
            }
            return success;
        }

        public async Task<IEnumerable<ConfigurationViewModel>> ListSettings()
        {
            var settings = _mapper.Map<IEnumerable<ConfigurationViewModel>>(await _globalConfigurationSettingsRepository.All());
            if (!_systemUser.IsInRole("Administrator"))
            {
                foreach (var configurationViewModel in settings)
                {
                    configurationViewModel.UpdateSensitive();
                }
            }
            return settings;
        }
    }
}

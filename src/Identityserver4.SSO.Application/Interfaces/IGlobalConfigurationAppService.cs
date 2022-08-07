using ByLearning.SSO.Application.ViewModels;
using ByLearning.SSO.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.SSO.Application.Interfaces
{
    public interface IGlobalConfigurationAppService : IGlobalConfigurationSettingsService
    {
        Task<bool> UpdateSettings(IEnumerable<ConfigurationViewModel> configs);
        Task<IEnumerable<ConfigurationViewModel>> ListSettings();
    }
}
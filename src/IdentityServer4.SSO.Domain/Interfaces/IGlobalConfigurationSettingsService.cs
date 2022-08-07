using System.Threading.Tasks;
using ByLearning.SSO.Domain.ViewModels.Settings;

namespace ByLearning.SSO.Domain.Interfaces
{
    public interface IGlobalConfigurationSettingsService
    {
        Task<PrivateSettings> GetPrivateSettings();
        Task<PublicSettings> GetPublicSettings();
    }
}

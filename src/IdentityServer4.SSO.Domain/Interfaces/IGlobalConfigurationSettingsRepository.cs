using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Domain.Interfaces
{
    public interface IGlobalConfigurationSettingsRepository : IRepository<GlobalConfigurationSettings>
    {
        Task<GlobalConfigurationSettings> FindByKey(string key);
        Task<List<GlobalConfigurationSettings>> All();
    }
}

using System.Collections.Generic;
using ByLearning.EntityFrameworkCore.Interfaces;
using ByLearning.EntityFrameworkCore.Repository;
using ByLearning.SSO.Domain.Interfaces;
using ByLearning.SSO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ByLearning.SSO.EntityFramework.Repository.Repository
{
    public class GlobalConfigurationSettingsRepository : Repository<GlobalConfigurationSettings>, IGlobalConfigurationSettingsRepository
    {
        public GlobalConfigurationSettingsRepository(IEntityFrameworkStore context) : base(context)
        {
        }

        public Task<GlobalConfigurationSettings> FindByKey(string key)
        {
            return DbSet.FirstOrDefaultAsync(f => f.Key == key);
        }

        public Task<List<GlobalConfigurationSettings>> All()
        {
            return DbSet.ToListAsync();
        }
    }
}

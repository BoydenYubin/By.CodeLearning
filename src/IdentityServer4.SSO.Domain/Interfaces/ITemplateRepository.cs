using System.Collections.Generic;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.SSO.Domain.Models;
using System.Threading.Tasks;

namespace ByLearning.SSO.Domain.Interfaces
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Task<bool> Exist(string name);
        Task<Template> GetByName(string name);
        Task<List<Template>> All();
    }
}

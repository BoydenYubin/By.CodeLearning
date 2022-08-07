using System.Collections.Generic;
using System.Threading.Tasks;
using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Domain.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRoles();
        Task<bool> Remove(string name);
        Task<Role> Details(string name);
        Task<bool> Save(string name);
        Task<bool> Update(string name, string oldName);
    }
}
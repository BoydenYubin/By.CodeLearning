using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByLearning.SSO.Application.ViewModels.RoleViewModels;

namespace ByLearning.SSO.Application.Interfaces
{
    public interface IRoleManagerAppService : IDisposable
    {
        Task<IEnumerable<RoleViewModel>> GetAllRoles();
        Task Remove(RemoveRoleViewModel model);
        Task<RoleViewModel> GetDetails(string name);
        Task Save(SaveRoleViewModel model);
        Task Update(string id, UpdateRoleViewModel model);
        Task RemoveUserFromRole(RemoveUserFromRoleViewModel model);
    }
}

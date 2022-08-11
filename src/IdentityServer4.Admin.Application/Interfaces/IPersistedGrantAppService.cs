using ByLearning.Admin.Application.ViewModels;
using ByLearning.DDD.Domain.Core.ViewModels;
using System;
using System.Threading.Tasks;

namespace ByLearning.Admin.Application.Interfaces
{
    public interface IPersistedGrantAppService : IDisposable
    {
        Task<ListOf<PersistedGrantViewModel>> GetPersistedGrants(IPersistedGrantCustomSearch search);
        Task Remove(RemovePersistedGrantViewModel model);
    }
}
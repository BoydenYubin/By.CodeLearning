using System;
using System.Threading.Tasks;

namespace ByLearning.Domain.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}

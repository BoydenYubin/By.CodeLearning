using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ByLearning.EntityFrameworkCore.Interfaces
{
    public interface IEntityFrameworkStore : IDisposable
    {
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}

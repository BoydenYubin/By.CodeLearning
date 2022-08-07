using ByLearning.Domain.Core.Interfaces;
using ByLearning.EntityFrameworkCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace ByLearning.EntityFrameworkCore.Repository
{
    /// <summary>
    /// Generic repository for <see cref="T:ByLearning.EntityFrameworkCore.Interfaces.IEntityFrameworkStore" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly IEntityFrameworkStore Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(IEntityFrameworkStore context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }

        public virtual void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public virtual void Remove<T>(T id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public void Remove(TEntity obj)
        {
            DbSet.Remove(obj);
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

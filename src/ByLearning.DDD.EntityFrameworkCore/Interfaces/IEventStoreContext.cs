using ByLearning.Domain.Core.Events;
using Microsoft.EntityFrameworkCore;

namespace ByLearning.EntityFrameworkCore.Interfaces
{
    public interface IEventStoreContext : IEntityFrameworkStore
    {
        public DbSet<StoredEvent> StoredEvent { get; set; }
        public DbSet<EventDetails> StoredEventDetails { get; set; }
    }
}
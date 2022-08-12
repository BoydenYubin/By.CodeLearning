using ByLearning.DDD.Domain.Core.ViewModels;
using ByLearning.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByLearning.Domain.Core.Interfaces
{
    public interface IEventStoreRepository : IDisposable
    {
        Task Store(StoredEvent theEvent);
        IQueryable<StoredEvent> All();
        Task<List<StoredEvent>> GetEvents(string username, PagingViewModel paging);
        Task<int> Count(string username, string search);
    }
}
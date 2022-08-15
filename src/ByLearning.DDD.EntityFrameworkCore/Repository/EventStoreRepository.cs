using ByLearning.DDD.Domain.Core.ViewModels;
using ByLearning.Domain.Core.Events;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Util;
using ByLearning.EntityFrameworkCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ByLearning.EntityFrameworkCore.Repository
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IEventStoreContext _context;

        public EventStoreRepository(IEventStoreContext context)
        {
            _context = context;
        }

        public IQueryable<StoredEvent> All()
        {
            return _context.StoredEvent.Include(s => s.Details);
        }

        public async Task<List<StoredEvent>> GetEvents(string username, PagingViewModel paging)
        {
            List<StoredEvent> events = null;
            if (paging.Search.IsPresent())
                events = await _context.StoredEvent
                                    .Include(s => s.Details)
                                    .Where(EventFind(username, paging.Search))
                                    .OrderByDescending(o => o.Timestamp)
                                    .Skip(paging.Offset)
                                    .Take(paging.Limit).ToListAsync();
            else
                events = await _context.StoredEvent
                                    .Include(s => s.Details)
                                    .Where(w => w.User == username)
                                    .Skip(paging.Offset)
                                    .OrderByDescending(o => o.Timestamp)
                                    .Take(paging.Limit).ToListAsync();

            return events;
        }

        private static Expression<Func<StoredEvent, bool>> EventFind(string username, string search)
        {
            return w => (w.Message.Contains(search) ||
                        w.MessageType.Contains(search) ||
                        w.AggregateId.Contains(search)) &&
                        w.User == username;
        }

        public Task<int> Count(string username, string search)
        {
            return search.IsPresent() ? _context.StoredEvent.Where(EventFind(username, search)).CountAsync() : _context.StoredEvent.Where(w => w.User == username).CountAsync();
        }

        public async Task Store(StoredEvent theEvent)
        {
            await _context.StoredEvent.AddAsync(theEvent);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

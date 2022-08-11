using ByLearning.DDD.Domain.Core.ViewModels;
using ByLearning.Domain.Core.ViewModels;
using ByLearning.SSO.Application.EventSourcedNormalizers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.SSO.Application.Interfaces
{
    public interface IEventStoreAppService
    {
        ListOf<EventHistoryData> GetEvents(ICustomEventQueryable query);
        Task<IEnumerable<EventSelector>> ListAggregates();
    }
}
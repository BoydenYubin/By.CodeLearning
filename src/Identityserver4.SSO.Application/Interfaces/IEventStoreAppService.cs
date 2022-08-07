using ByLearning.SSO.Application.EventSourcedNormalizers;
using Identityserver4.SSO.Application.ViewModels.Common;
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
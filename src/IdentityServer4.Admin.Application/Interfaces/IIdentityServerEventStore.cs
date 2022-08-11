using ByLearning.DDD.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.Admin.Application.Interfaces
{
    public interface IIdentityServerEventStore
    {
        Task<IEnumerable<EventSelector>> ListAggregates();
    }
}
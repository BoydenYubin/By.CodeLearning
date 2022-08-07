using System.Threading.Tasks;

namespace ByLearning.Domain.Core.Events
{
    public interface IEventStoreService
    {
        Task Save<T>(T @event) where T : Event;
    }
}

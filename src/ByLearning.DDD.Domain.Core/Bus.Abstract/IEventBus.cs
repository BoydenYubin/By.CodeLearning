using ByLearning.Domain.Core.Commands;
using ByLearning.Domain.Core.Events;
using System.Threading.Tasks;

namespace ByLearning.Domain.Core.Bus.Abstract
{
    public interface IEventBus
    {
        Task<bool> SendCommand<T>(T command) where T : Command;
        Task Publish<T>(T @event) where T : Event;
    }
}

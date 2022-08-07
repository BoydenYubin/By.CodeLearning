using ByLearning.Domain.Core.Events;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Util;
using ServiceStack;
using System.Threading.Tasks;

namespace ByLearning.EntityFrameworkCore.EventSourcing
{
    public class SqlEventStore : IEventStoreService
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ISystemUser _systemUser;

        public SqlEventStore(IEventStoreRepository eventStoreRepository, ISystemUser systemUser)
        {
            _eventStoreRepository = eventStoreRepository;
            _systemUser = systemUser;
        }

        public Task Save<T>(T theEvent) where T : Event
        {
            var serializedData = theEvent.ToJson();

            if (theEvent.Message.IsMissing())
                theEvent.Message = theEvent.MessageType.AddSpacesToSentence().Replace("Event", string.Empty).Trim();

            var storedEvent = new StoredEvent(
               theEvent.MessageType,
               theEvent.EventType,
               theEvent.Message,
               _systemUser.GetLocalIpAddress(),
               _systemUser.GetRemoteIpAddress(),
               serializedData)
                .SetUser(_systemUser.Username).SetAggregate(theEvent.AggregateId);

            return _eventStoreRepository.Store(storedEvent);
        }
    }
}
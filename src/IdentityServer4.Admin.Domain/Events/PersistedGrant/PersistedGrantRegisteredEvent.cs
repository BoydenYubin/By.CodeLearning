using ByLearning.Domain.Core.Events;

namespace ByLearning.Admin.Domain.Events.PersistedGrant
{
    public class PersistedGrantRemovedEvent : Event
    {
        public string Key { get; }
        public string GrantType { get; }
        public string SubjectId { get; }

        public PersistedGrantRemovedEvent(string key, string clientId, string grantType, string subjectId)
        {
            Key = key;
            GrantType = grantType;
            SubjectId = subjectId;
            AggregateId = clientId;
        }
    }
}
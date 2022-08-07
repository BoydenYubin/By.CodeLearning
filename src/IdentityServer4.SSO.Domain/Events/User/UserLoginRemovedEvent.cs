using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.User
{
    public class UserLoginRemovedEvent : Event
    {
        public string Username { get; }
        public string LoginProvider { get; }
        public string ProviderKey { get; }

        public UserLoginRemovedEvent(string username, string loginProvider, string providerKey)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Username = username;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }
    }
}
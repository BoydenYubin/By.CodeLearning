using System.Collections.Generic;
using System.Security.Claims;
using ByLearning.Domain.Core.Events;

namespace ByLearning.SSO.Domain.Events.User
{
    public class ClaimsSyncronizedEvent : Event
    {
        public IEnumerable<Claim> Claims { get; }

        public ClaimsSyncronizedEvent(string username, IEnumerable<Claim> claims)
            : base(EventTypes.Success)
        {
            Claims = claims;
            AggregateId = username;
        }
    }
}
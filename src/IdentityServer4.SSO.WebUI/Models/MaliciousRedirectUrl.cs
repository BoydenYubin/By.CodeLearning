using IdentityServer4.Events;

namespace IdentityServer4.SSO.WebUI.Models
{
    public class MaliciousRedirectUrlEvent : Event
    {
        public string Url { get; }

        public const int MaliciousUriFailure = 1022;
        public MaliciousRedirectUrlEvent(string url, string username) : base(EventCategories.Authentication,
        "Malicious Url",
        EventTypes.Failure,
        MaliciousUriFailure)
        {
            Url = url;
            Name = username;
        }
    }
}

using ByLearning.Domain.Core.Commands;
using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Domain.Commands.Email
{
    public abstract class EmailCommand : Command
    {
        public string Subject { get; protected set; }
        public string Description { get; protected set; }
        public EmailType Type { get; protected set; }
        public Sender Sender { get; protected set; }
        public BlindCarbonCopy Bcc { get; protected set; }
        public string Username { get; protected set; }
        public string Content { get; protected set; }

    }
}
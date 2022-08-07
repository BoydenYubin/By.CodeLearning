using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Domain.ViewModels.User
{
    public class EmailMessage
    {
        public string Email { get; }
        public BlindCarbonCopy Bcc { get; }
        public string Subject { get; }
        public string Content { get; }
        public Sender Sender { get; }

        public EmailMessage(string email, BlindCarbonCopy bcc, string subject, string content, Sender sender)
        {
            Email = email;
            Bcc = bcc;
            Subject = subject;
            Content = content;
            Sender = sender;
        }
    }
}

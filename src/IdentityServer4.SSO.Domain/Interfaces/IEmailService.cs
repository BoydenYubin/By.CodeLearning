using ByLearning.SSO.Domain.ViewModels.User;
using System.Threading.Tasks;

namespace ByLearning.SSO.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}

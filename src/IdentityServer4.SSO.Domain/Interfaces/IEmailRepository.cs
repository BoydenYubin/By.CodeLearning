using System.Threading.Tasks;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Domain.Interfaces
{
    public interface IEmailRepository : IRepository<Email>
    {
        Task<Email> GetByType(EmailType type);
    }
}
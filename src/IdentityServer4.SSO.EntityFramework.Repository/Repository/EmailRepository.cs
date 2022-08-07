using ByLearning.EntityFrameworkCore.Interfaces;
using ByLearning.EntityFrameworkCore.Repository;
using ByLearning.SSO.Domain.Interfaces;
using ByLearning.SSO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ByLearning.SSO.EntityFramework.Repository.Repository
{
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        public EmailRepository(IEntityFrameworkStore context) : base(context)
        {
        }

        public Task<Email> GetByType(EmailType type)
        {
            return DbSet.FirstOrDefaultAsync(f => f.Type == type);
        }
    }
}
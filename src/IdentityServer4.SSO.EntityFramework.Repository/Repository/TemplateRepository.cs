using ByLearning.EntityFrameworkCore.Interfaces;
using ByLearning.EntityFrameworkCore.Repository;
using ByLearning.SSO.Domain.Interfaces;
using ByLearning.SSO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.SSO.EntityFramework.Repository.Repository
{
    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(IEntityFrameworkStore context) : base(context)
        {
        }

        public Task<bool> Exist(string name)
        {
            return DbSet.AnyAsync(w => w.Name.ToUpper() == name.ToUpper());
        }

        public Task<Template> GetByName(string name)
        {
            return DbSet.FirstOrDefaultAsync(s => s.Name == name);
        }

        public Task<List<Template>> All()
        {
            return DbSet.ToListAsync();
        }
    }
}

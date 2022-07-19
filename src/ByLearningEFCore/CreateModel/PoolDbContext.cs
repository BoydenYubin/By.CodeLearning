using Microsoft.EntityFrameworkCore;

namespace ByLearningEFCore.CreateModel
{
    public class PoolDbContext : DbContext
    {
        public PoolDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}

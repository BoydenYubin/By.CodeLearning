using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;

namespace ByLearningEFCore
{
    public class BaseCar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
    }

    public class TruckCar : BaseCar
    {
        public int Height { get; set; }
    }

    public class InheriteContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConfig.GetConnectionString());
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TruckCar>();
        }
    }
}

using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using System;

namespace ByLearningEFCore
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class PersonContext : DbContext
    {
        private string ConnectionString = GetConfig.GetConnectionString();
        public DbSet<Person> Persons { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }
    }
}

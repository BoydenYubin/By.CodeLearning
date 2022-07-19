using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ByLearningEFCore
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Students> Students { get; set; }
        public Teacher()
        {
            Students = new List<Students>();
        }
    }

    public class Students
    {
        public Guid Id { get; set; }
        public int Name { get; set; }
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }

    public class TeacherContext : DbContext
    {
        private string ConnectionString = GetConfig.GetConnectionString();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var b1 = modelBuilder.Entity<Students>();
            b1.HasKey(t => t.Id);
            b1.HasOne(s => s.Teacher)
                .WithMany(t => t.Students)
                .HasForeignKey(s => s.TeacherId);
        }
    }
}

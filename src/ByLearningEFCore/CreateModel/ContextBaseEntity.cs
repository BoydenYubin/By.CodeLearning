using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ByLearningEFCore
{
    public class BlogEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public ICollection<PostEntity> Posts { get; set; }
    }

    public class PostEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public BlogEntity Blog { get; set; }
        public int BlogId { get; set; }
    }

    public class BlogConfiguration : IEntityTypeConfiguration<BlogEntity>
    {
        public void Configure(EntityTypeBuilder<BlogEntity> builder)
        {
            builder.ToTable("blogs");
            builder.HasKey(b => b.Id);
            builder.Property(p => p.Name).HasMaxLength(500).IsRequired();
            builder.Property(p => p.CreatedTime).HasColumnType("DATETIME").HasDefaultValueSql("NOW()");
            builder.Property(p => p.ModifiedTime).HasColumnType("DATETIME").HasDefaultValueSql("NOW()");

            builder.HasMany(p => p.Posts).WithOne(b => b.Blog).HasForeignKey(b => b.BlogId);
        }
    }

    public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.ToTable("posts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(500).IsRequired();
            builder.Property(p => p.CreatedTime).HasColumnType("DATETIME").HasDefaultValueSql("NOW()");
            builder.Property(p => p.ModifiedTime).HasColumnType("DATETIME").HasDefaultValueSql("NOW()");
        }
    }

    public class BlogContext : DbContext
    {
        public DbSet<BlogEntity> Blogs { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var looger = LoggerFactory.Create(config =>
            {
                config.AddDebug();
            });
            optionsBuilder.UseLoggerFactory(looger);
            optionsBuilder.UseMySql(GetConfig.GetConnectionString());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogEntity>().Property<DateTime>("LastUpdated");
            modelBuilder.ApplyConfiguration(new BlogConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

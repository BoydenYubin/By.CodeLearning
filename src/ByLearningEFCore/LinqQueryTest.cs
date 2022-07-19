using ByLearningEFCore.CreateModel;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ByLearningEFCore
{
    public class LinqQueryTest
    {
        [Fact]
        public void SeedDataM1()
        {
            BlogContext context = new BlogContext();
            for (int i = 0; i < 100; i++)
            {
                var blog = new BlogEntity()
                {
                    Name = $"Blog_{i}",
                    Posts = new List<PostEntity>()
                };
                for (int j = 0; j < 20; j++)
                {
                    blog.Posts.Add(new PostEntity()
                    {
                        Name = $"Blog_{i}_Post{j}"
                    });
                }
                context.Blogs.Add(blog);
            }
            context.SaveChanges();
        }
        /// <summary>
        /// 预先加载
        /// </summary>
        [Fact]
        public void IncludeTest()
        {
            BlogContext context = new BlogContext();
            var blogs = context.Blogs.Where(b => b.Id < 50).AsNoTracking();
            var result = context.Blogs.FirstOrDefault().Posts == null;
            result.ShouldBeTrue();
            var inBlogs = context.Blogs.Where(b => b.Id < 50).Include(b => b.Posts).AsNoTracking();
            result = inBlogs.FirstOrDefault().Posts == null;
            result.ShouldBeFalse();
        }
        /// <summary>
        /// 显式加载
        /// </summary>
        [Fact]
        public void ExplictLoadTest()
        {
            BlogContext context = new BlogContext();
            var blog = context.Blogs.Find(1);
            blog.Posts.ShouldBeNull();
            context.Entry(blog).Collection(b => b.Posts).Load();
            blog.Posts.ShouldNotBeNull();
        }
        [Fact]
        public void SeedDataM2()
        {
            OneToOneContext context = new OneToOneContext();
            for (int i = 0; i < 100; i++)
            {
                var owner = new Ownner()
                {
                    Name = $"Owner{i}"
                };
                context.Ownners.Add(owner);
            }
            for (int i = 0; i < 200; i++)
            {
                var proper = new Properties()
                {
                    OwnerID = new Random().Next(1, 100),
                    Name = $"Properties{i}"
                };
                context.Properties.Add(proper);
            }
            context.SaveChanges();
        }
        /// <summary>
        /// innder join
        /// </summary>
        [Fact]
        public void InnerJoinTest()
        {
            OneToOneContext context = new OneToOneContext();

            var result = from o in context.Ownners.Where(o => o.ID < 15)
                         join p in context.Properties
                         on o.ID equals p.OwnerID
                         select new { o, p };

            result.Count().ShouldBe(4);
            /* 使用时再调用sql语句
             SELECT `o`.`ID`, `o`.`Name`, `p`.`PropertiesID`, `p`.`Name`, `p`.`OwnerID`
             FROM `Ownners` AS `o`
             INNER JOIN `Properties` AS `p` ON `o`.`ID` = `p`.`OwnerID`
             WHERE `o`.`ID` < 15
             */
        }
        /// <summary>
        /// 如果键选择器是匿名类型，则 EF Core 会生成一个联接条件，以比较组件是否相等
        /// </summary>
        [Fact]
        public void InnerJoinWithAnonymousKeyTest()
        {
            OneToOneContext context = new OneToOneContext();

            var result = from o in context.Ownners
                         join p in context.Properties
                         on new { id = o.ID, name = o.Name } equals new { id = p.OwnerID, name = "Owner30" }
                         select new { o, p };
            /*
             INNER JOIN `Properties` AS `p` ON (`o`.`ID` = `p`.`OwnerID`) AND (`o`.`Name` = 'Owner30')
             */
            var veritify = result.Select(res => res.o.Name).Contains("Owner30");
            veritify.ShouldBeTrue();
        }
        /// <summary>
        /// group by语句
        /// </summary>
        [Fact]
        public void GroupJoinTest()
        {
            OneToOneContext context = new OneToOneContext();
            var result = from p in context.Properties
                         group p by p.OwnerID into grouping
                         select new { grouping.Key, count = grouping.Count() };
            result.ShouldNotBeNull();
            /*
             SELECT `p`.`OwnerID` AS `Key`, COUNT(*) AS `count`
             FROM `Properties` AS `p`
             GROUP BY `p`.`OwnerID
             */
        }
        /// <summary>
        /// Left join
        /// </summary>
        [Fact]
        public void LeftJoinTest()
        {
            OneToOneContext context = new OneToOneContext();

            var result = from o in context.Ownners.Where(o => o.ID < 15)
                         join p in context.Properties
                         on o.ID equals p.OwnerID into grouping
                         from p in grouping.DefaultIfEmpty()
                         select new { o, p };
            var list = result.ToList();
            /*
               SELECT `o`.`ID`, `o`.`Name`, `p`.`PropertiesID`, `p`.`Name`, `p`.`OwnerID`
               FROM `Ownners` AS `o`
               LEFT JOIN `Properties` AS `p` ON `o`.`ID` = `p`.`OwnerID`
               WHERE `o`.`ID` < 15
             */
        }
        [Fact]
        public void SubQueryTest()
        {
            OneToOneContext context = new OneToOneContext();
            var result = from sub in
                             (from o in context.Ownners.Where(o => o.ID < 10)
                              select new { o.ID,o.Name})
                         join  p in context.Properties
                         on sub.ID equals p.OwnerID
                         select p;
            /* 生成的语句
             SELECT `p`.`PropertiesID`, `p`.`Name`, `p`.`OwnerID`
             FROM `Ownners` AS `o`
             CROSS JOIN `Properties` AS `p`
             WHERE (`o`.`ID` < 10) AND (`p`.`OwnerID` = `o`.`ID`)
             ORDER BY `p`.`OwnerID`
               * /
            /* 预实现的语句
             select p.propertiesid, p.name,p.ownerid from 
               (select o.id, o.name from ownners o
                where o.id < 10) as sub
            left join properties p
            on p.ownerid = sub.id
            order by p.ownerid
             */
            //结果是一致的
            result.ToArray();
        }
        /// <summary>
        /// 原生sql查询
        /// </summary>
        [Fact]
        public void RawSqlQueryTest()
        {
            OneToOneContext context = new OneToOneContext();
            //Way 1
            var ownners = context.Ownners.FromSqlRaw("select * from ownners").ToList();

            //Way 2
            int id = 10;
            string sql = $"SELECT * From Ownners WHERE Ownners.ID < {id}";

            var con = context.Database.GetDbConnection();
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sql;
            var reader = cmd.ExecuteReader();
            var list = new List<Ownner>();
            while (reader.Read())
            {
                var owner = new Ownner()
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
                list.Add(owner);
            }
            list.Count.ShouldBe(9);
        }
    }
}

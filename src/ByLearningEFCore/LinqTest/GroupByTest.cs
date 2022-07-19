using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shouldly;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace ByLearningEFCore.LinqTest
{
    public class Person
    {
        public string Name { set; get; }
        public int Age { set; get; }
        public string Gender { set; get; }
    }

    public class NewPerson
    {
        public string Name { set; get; }
        public int Age { set; get; }
        public string Gender { set; get; }
    }

    public class NewPersonContext : DbContext
    {
        public DbSet<NewPerson> NewPerson { get; set; }
        public NewPersonContext()
        {
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NewPerson>().HasNoKey();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConfig.GetConnectionString());
            var looger = LoggerFactory.Create(config =>
            {
                config.AddDebug();
            });
            optionsBuilder.UseLoggerFactory(looger);
            //以下语句将会记录参数值
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
    public class GroupByTest
    {
        private List<Person> personList;
        public GroupByTest()
        {
            personList = new List<Person>()
            {
                new Person
                {
                    Name = "P1", Age = 18, Gender = "Male"
                },
                new Person
                {
                    Name = "P2", Age = 19, Gender = "Male",
                },
                new Person
                {
                    Name = "P2", Age = 17,Gender = "Female",
                }
            };
        }
        [Fact]
        public void GroupBySingleProperityTest()
        {
            //public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            //官方释义：根据指定的键选择器函数对序列中的元素进行分组。
            var result = personList.GroupBy(p => p.Gender).Select(p => new { gender = p.Key, count = p.Average(o => o.Age) });
            result.Where(a => a.gender == "Male").Count().ShouldBe(2);
        }
        [Fact]
        public void GroupByIEqualityComparerTest()
        {
            //public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, EqualityComparer<TKey> comparer)
            //根据指定的键选择器函数对序列中的元素进行分组，并使用指定的比较器对键进行比较。
            //这里的比较器是PersonEqualityComparer
            var result = personList.GroupBy(p => p, new PersonEqualityComparer());
        }
        private class PersonEqualityComparer : EqualityComparer<Person>
        {
            public override bool Equals([AllowNull] Person x, [AllowNull] Person y)
            {
                if (x.Name == y.Name) return true;
                else return false;
            }

            public override int GetHashCode([DisallowNull] Person obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        private class NewPersonEqualityComparer : EqualityComparer<NewPerson>
        {
            public override bool Equals([AllowNull] NewPerson x, [AllowNull] NewPerson y)
            {
                if (x.Name == y.Name) return true;
                else return false;
            }

            public override int GetHashCode([DisallowNull] NewPerson obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        [Fact]
        public void GroupByAddElementBTest()
        {
            //public static IEnumerable<IGrouping<TKey, TElement>>
            //GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source,
            //                                 Func<TSource, TKey> keySelector,
            //                                 Func<TSource, TElement> elementSelector,
            //                                 IEqualityComparer<TKey> comparer)
            var result = personList.GroupBy(p => p, p => p.Name, new PersonEqualityComparer());
            //IGrouping<Person,string>
        }
        [Fact]
        public void GroupByAddElementATest()
        {
            //public static IEnumerable<IGrouping<TKey, TElement>>
            //GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source,
            //                                 Func<TSource, TKey> keySelector,
            //                                 Func<TSource, TElement> elementSelector)
            var result = personList.GroupBy(p => p.Gender, p => p.Name);
        }

        [Fact]
        public void SeedDataForContext()
        {
            NewPersonContext context = new NewPersonContext();
            //context.AddRange(personList);
            foreach (var people in personList)
            {
                context.Database.ExecuteSqlInterpolated($"insert into newperson (Name,Age,Gender) values ({people.Name}, {people.Age}, {people.Gender})");
            }    
            //context.People.AddRange(personList);
            var result = context.SaveChanges();
        }

        [Fact]
        public void GroupBySingleProperityWithContextTest()
        {
            NewPersonContext person = new NewPersonContext();
            var result = person.NewPerson.GroupBy(p => p.Gender).Select(p => new { gender = p.Key, count = p.Average(o => o.Age) });
            /*
             SELECT `n`.`Gender` AS `gender`, AVG(CAST(`n`.`Age` AS double)) AS `count`
             FROM `NewPerson` AS `n`
             GROUP BY `n`.`Gender`
             */
        }
        [Fact]
        public void GroupByIEqualityComparerWithContextTest()
        {
            NewPersonContext context = new NewPersonContext();
            //带比较器无法转换为sql语句
            //需要使用ToList()方法在客户端调用后续Group方法
            var result = context.NewPerson.ToList().GroupBy(p => p, new NewPersonEqualityComparer());
        }

        [Fact]
        public void GroupByAddElementAWithContextTest()
        {
            //此方法也无法翻译为sql语句
            //需要先使用ToList，在客户端执行
            NewPersonContext context = new NewPersonContext();
            var result = context.NewPerson.ToList().GroupBy(p => p.Gender, p => p.Name);
        }
    }
}

using ByLearningEFCore.CreateModel;
using ByLearningEFCore.TestClass;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace ByLearningEFCore
{
    public class ModelCreateTest
    {
        [Fact]
        public void SimpleUseContext()
        {
            PersonContext dbContext = new PersonContext();
            try
            {
                dbContext.Add(new Person() { Id = Guid.NewGuid(), Name = "boyden", Age = 28 });
                var res = dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        [Fact]
        public void BackingFieldTest()
        {
            BackingFieldsContext context = new BackingFieldsContext();
            context.BackingFields.Add(new BackingFieldsClass(url: "www.baidu.com"));
            context.SaveChanges();
        }

        [Fact]
        public void BackingFieldQueryTest()
        {
            BackingFieldsContext context = new BackingFieldsContext();
            //查询
            var results = context.BackingFields;
            results.Count().ShouldBe(1);
        }

        [Fact]
        public void ShadowPropertyTest()
        {
            //ShadowProperty主要是增加暴漏外部类时的属性，可单独设置，可单独查询
            ShadowPropertyContext context = new ShadowPropertyContext();
            var shadow = new ShadowPropertyClass() { Name = "rose" };
            context.Add(shadow);
            context.Entry(shadow).Property<DateTime>("createtime").CurrentValue = new DateTime(1985, 12, 23);
            context.SaveChanges();
        }

        [Fact]
        public void ShadowPropertyQueryTest()
        {
            ShadowPropertyContext context = new ShadowPropertyContext();
            var result = context.Shadows.Where(s => EF.Property<DateTime>(s, "createtime") == new DateTime(1985, 12, 23));         
        }
    }
}

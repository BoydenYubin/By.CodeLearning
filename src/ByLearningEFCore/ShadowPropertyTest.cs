using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ByLearningEFCore
{
    public class ShadowPropertyTest
    {
        [Fact]
        public void AddDataTest()
        {
            using (var context = new BlogContext())
            {
                var blog = new BlogEntity()
                {
                    Name = "bylearing EFcore Second",
                    Posts = new List<PostEntity>()
                };
                var post = new PostEntity()
                {
                    Name = "post stuff second"
                };
                blog.Posts.Add(post);
                context.Add(blog);
                context.SaveChanges();
            }
        }

        [Fact]
        public void ShadowPropertyOnTest()
        {
            using (var context = new BlogContext())
            {
                var myblog = context.Find<BlogEntity>(2);
                context.Entry(myblog).Property("LastUpdated").CurrentValue = DateTime.Now;
                context.SaveChanges();
            }
        }

        [Fact]
        public void ShadowPropertyOnSecongTest()
        {
            using (var context = new BlogContext())
            {
                var blogs = context.Blogs.OrderBy(blog => EF.Property<DateTime>(blog, "LastUpdated"));
                var res = blogs.Select(b => b.Id);
                res.ShouldContain(2);
                res.ShouldContain(1);
            }
        }
    }
}

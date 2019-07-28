using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServiceLayer;

namespace Tests
{
    [TestFixture]
    public class SearchServiceTests
    {
        [TestCase("coffee", 3)]
        [TestCase("teA", 1)]
        [TestCase("waTer", 0)]
        public void ReturnFilteredResultsCorrespondingToQuery(string query, int expected)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var ctx = new ApplicationDbContext(options))
            {
                ctx.Forums.Add(new Forum
                {
                    Id = 19
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = 23556,
                    Title = "First Post",
                    Content = "Coffee"
                });
                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = -236,
                    Title = "Coffee",
                    Content = "Some Content"
                });
                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = 2356,
                    Title = "Tea",
                    Content = "Coffee"
                });

                ctx.SaveChanges();
            }

            using (var ctx = new ApplicationDbContext(options))
            {
                var postService = new PostService(ctx);

                var result = postService.GetFilteredPosts(query);

                var postCount = result.Count();

                Assert.AreEqual(expected, postCount);
            }
        }
    }
}

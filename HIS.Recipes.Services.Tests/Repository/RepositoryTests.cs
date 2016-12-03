using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Implementation.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HIS.Recipes.Services.Tests.Repository
{
    public class RepositoryTests
    {
        /*[Fact]
        public void Add_writes_to_database()
        {
            var options = new DbContextOptionsBuilder<RecipeDBContext>()
                .UseInMemoryDatabase("Add_writes_to_database")
                .Options;

            // Run the test against one instance of the context
            using (var context = new RecipeDBContext(options))
            {
                var service = new RecipeDbRepository.DbImageRepository(context);
                service.Add("http://sample.com");
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new BloggingContext(options))
            {
                Assert.AreEqual(1, context.Blogs.Count());
                Assert.AreEqual("http://sample.com", context.Blogs.Single().Url);
            }
        }*/
    }
}

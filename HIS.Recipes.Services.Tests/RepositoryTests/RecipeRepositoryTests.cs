using System;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Implementation.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HIS.Recipes.Services.Tests.RepositoryTests
{
    public class RecipeRepositoryTests:IDisposable
    {
        #region CONST
        #endregion

        #region FIELDS
        bool _disposed;
        #endregion

        #region CTOR
        public RecipeRepositoryTests()
        {
            
        }
        ~RecipeRepositoryTests()
        {
            Dispose(false);
        }
        #endregion

        #region METHODS

        #region Tests

        [Fact]
        public async Task SearchRecipe()
        {

            // Use a separate instance of the context to verify correct data was saved to database
            await this.Intialize();
            var respoitory = new RecipeDbRepository.RecipeRepository(this.Context);
            var input = this.TestData.Recipes.First();
            var output = await respoitory.FindAsync(input.Id);

            Assert.NotNull(output);
            Assert.Equal(input.Id, output.Id);
        }

        /*
         [Fact]
        public void Add_writes_to_database()
        {
            var options = new DbContextOptionsBuilder<RecipeDBContext>()
                .UseInMemoryDatabase("read_from_database")
                .Options;

            // Run the test against one instance of the context
            using (var context = new RecipeDBContext(options))
            {
                var imageRepository = new RecipeDbRepository.RecipeRepository(context);
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new RecipeDBContext(options))
            {
                Assert.Equal(1, context.Blogs.Count());
                Assert.Equal("http://sample.com", context.Blogs.Single().Url);
            }
        }
         */

        #endregion

        #region Helper

        private async Task Intialize()
        {
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                            .UseInMemoryDatabase("read_from_database")
                            .Options;
            this.Context = new RecipeDbContext(options);

            this.TestData = new RepositoryTestData();
            await this.Context.Database.EnsureCreatedAsync();
            await this.TestData.WriteTestDataToDataBaseAsync(this.Context);
        }

        /// <summary>
        /// Releases an returns unnessessary system resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only

                this.Context.Database.EnsureDeleted();
                this.Context.Dispose();
            }

            // release any unmanaged objects
            this.Context = null;

            _disposed = true;
        }


        #endregion

        #endregion

        #region PROPERTIES

        private RecipeDbContext Context { get; set; }

        private RepositoryTestData TestData { get; set; }
        #endregion

    }
}

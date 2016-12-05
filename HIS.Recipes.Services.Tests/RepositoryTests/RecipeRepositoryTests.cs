using System;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HIS.Recipes.Services.Tests.RepositoryTests
{
    public class RecipeRepositoryTests : IDisposable
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
            var repository = new RecipeDbRepository.RecipeRepository(this.Context);
            var input = this.TestData.Recipes.First();
            var output = await repository.FindAsync(input.Id);

            Assert.NotNull(output);
            Assert.Equal(input.Id, output.Id);
        }


        [Fact]
        public async Task CreateNewRecipe()
        {
            await Intialize();
            var repository = new RecipeDbRepository.RecipeRepository(this.Context);

            var input = new Recipe()
            {
                Calories = 1,
                CookedCounter = 1,
                Creator = "Tester",
                Name = "New Recipe",
                NumberOfServings = 1
            };
            var output = await repository.AddAsync(input);
            Assert.NotNull(output);
            Assert.NotEqual(0, output.Id);

            Assert.Equal(this.TestData.Recipes.Count +1 , this.Context.Recipes.Count());
            var result = await this.Context.Recipes.FindAsync(output.Id);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteRecipe()
        {
            await Intialize();
            var repository = new RecipeDbRepository.RecipeRepository(this.Context);

            var input = this.TestData.Recipes.First();
            var output = await repository.RemoveAsync(input);
            Assert.True(output);

            var result = await this.Context.Recipes.FindAsync(input.Id);
            Assert.Null(result);
            Assert.Equal(this.TestData.Recipes.Count -1, this.Context.Recipes.Count());
        }


        [Fact]
        public async Task UpdateRecipe()
        {
            const int newCalorien = 2;
            const string newCreator = "New User";

            await Intialize();
            var repository = new RecipeDbRepository.RecipeRepository(this.Context);

            var input = this.TestData.Recipes.First();
            var dbInput = await repository.FindAsync(input.Id);
            Assert.NotNull(dbInput);

            dbInput.Calories = newCalorien;
            dbInput.Creator = newCreator;
            await repository.UpdateAsync(dbInput);
            
            dbInput = await repository.FindAsync(input.Id);
            Assert.NotNull(dbInput);

            Assert.Equal(dbInput.Creator, newCreator);
            Assert.Equal(dbInput.Calories, newCalorien);
        }


        #endregion

        #region Helper

        private async Task Intialize()
        {
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                            .UseInMemoryDatabase("read_from_database")
                            .Options;
            this.Context = new RecipeDbContext(options);

            this.TestData = new RepositoryTestData();
            await this.Context.Database.EnsureDeletedAsync();
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

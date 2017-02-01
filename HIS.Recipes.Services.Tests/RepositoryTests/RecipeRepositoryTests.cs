using System;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Test;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Models;
using HIS.Recipes.Services.Tests.Helper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HIS.Recipes.Services.Tests.RepositoryTests
{
    public class RecipeRepositoryTests : TestBase
    {
        [Fact]
        public async Task SearchRecipe()
        {
            await this.InitializeAsync();
            var repository = new RecipeDbRepository.RecipeRepository(this.DbContext, new MockLoggerFactory<object>());
            var input = this.TestData.Recipes.First();
            var output = await repository.FindAsync(input.Id);

            Assert.NotNull(output);
            Assert.Equal(input.Id, output.Id);
        }

        [Fact]
        public async Task CreateNewRecipe()
        {
            await InitializeAsync();
            var repository = new RecipeDbRepository.RecipeRepository(this.DbContext, new MockLoggerFactory<object>());

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

            Assert.Equal(this.TestData.Recipes.Count +1 , this.DbContext.Recipes.Count());
            var result = await this.DbContext.Recipes.FindAsync(output.Id);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteRecipe()
        {
            await InitializeAsync();
            var repository = new RecipeDbRepository.RecipeRepository(this.DbContext, new MockLoggerFactory<object>());
            
            var input = this.TestData.Recipes.First();
            var output = await repository.RemoveAsync(input);
            Assert.True(output);

            var result = await this.DbContext.Recipes.AsNoTracking().FirstOrDefaultAsync(x=>x.Id.Equals(input.Id));
            Assert.Null(result);
            Assert.Equal(this.TestData.Recipes.Count -1, this.DbContext.Recipes.Count());
        }

        [Fact]
        public async Task UpdateRecipe()
        {
            const int newCalorien = 2;
            const string newCreator = "New User";

            await InitializeAsync();
            var repository = new RecipeDbRepository.RecipeRepository(this.DbContext, new MockLoggerFactory<object>());

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
    }
}

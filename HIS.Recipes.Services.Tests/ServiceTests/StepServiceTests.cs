using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Tests.Helper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HIS.Recipes.Services.Tests.ServiceTests
{
    public class StepServiceTests:TestBase
    {
        [Fact]
        public async Task GetSteps()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var recipe = this.DbContext.Recipes.Include(x => x.Steps).AsNoTracking().First(x => x.Steps.Any());
                var steps = service.GetStepsForRecipe(recipe.Id);

                Assert.NotNull(steps);
                Assert.NotEmpty(steps);

                var firstDbStep = recipe.Steps.First();
                var firstStep = await steps.FirstAsync();

                Assert.Equal(firstDbStep.Id, firstStep.Id);
                Assert.Equal(firstDbStep.Order, firstStep.Order);
                Assert.Equal(firstDbStep.RecipeId, firstStep.RecipeId);
                Assert.Equal(firstDbStep.Description, firstStep.Description);
            }
        }

        [Fact]
        public async Task AddStep()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var recipe = await this.DbContext.Recipes.Include(x => x.Steps).AsNoTracking().FirstAsync();
                var newStep = new StepCreateViewModel()
                {
                    Description = "new Step"
                };

                var result = await service.AddAsync(recipe.Id, newStep);
                Assert.NotEqual(0, result.Id);

                var recipeAfterAdding = await this.DbContext.Recipes.Include(x => x.Steps).AsNoTracking().FirstAsync(x=>x.Id.Equals(recipe.Id));
                Assert.Equal(recipe.Steps.Count + 1, recipeAfterAdding.Steps.Count);

                var newCreatedStep = recipeAfterAdding.Steps.OrderBy(x=>x.Order).Last();
                Assert.Equal(newStep.Description, newCreatedStep.Description);
            }
        }
        [Fact]
        public async Task UpdateStep()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var recipe = await this.DbContext.Recipes.Include(x => x.Steps).AsNoTracking().FirstAsync(x=>x.Steps.Any());
                var firstStep = recipe.Steps.First();
                var newStep = new StepViewModel
                {
                    Id = firstStep.Id,
                    RecipeId = firstStep.RecipeId,
                    Order = firstStep.Order,
                    Description = "New Description"
                };

                await service.UpdateAsync(newStep.Id, newStep);

                var stepAfterChange = await DbContext.RecipeSteps.FirstAsync(x => x.Id.Equals(newStep.Id));
                Assert.Equal(newStep.Description, stepAfterChange.Description);
            }
        }

        [Fact]
        public async Task RemoveStep()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var step = await DbContext.RecipeSteps.AsNoTracking().FirstAsync();
                await service.RemoveAsync(step.Id);

                var stepAfterDelete = await DbContext.RecipeSteps.FirstOrDefaultAsync(x => x.Id.Equals(step.Id));
                Assert.Null(stepAfterDelete);
            }
        }

        [Fact]
        public async Task UpdateAllSteps()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var recipe = await this.DbContext.Recipes.Include(x => x.Steps).AsNoTracking().FirstAsync(x => x.Steps.Any());

                var newSteps = new List<string>() {"Step 1", "Step 2"};
                await service.UpdateAllStepsAsync(recipe.Id, newSteps);

                var recipeAfterUpdate = await this.DbContext.Recipes.Include(x => x.Steps).AsNoTracking().FirstAsync(x => x.Steps.Any());
                Assert.Equal(newSteps.Count, recipeAfterUpdate.Steps.Count);
                var firstStep = recipeAfterUpdate.Steps.OrderBy(x => x.Order).First();
                Assert.Equal(newSteps.First(), firstStep.Description);
                Assert.Equal(1, firstStep.Order);
            }
        }

        private IRecipeStepService GetService()
        {
            var rep = new RecipeDbRepository.StepRepository(this.DbContext);
            var recipeRep = new RecipeDbRepository.RecipeRepository(this.DbContext);
            var mockFactory = new MockLoggerFactory<IngrediantService>();
            IMapper mapper = new Mapper(new MapperConfiguration(m => m.AddProfile<AutoMapperServiceProfile>()));
            return new RecipeStepService(rep, recipeRep, mapper, mockFactory);
        }

    }
}

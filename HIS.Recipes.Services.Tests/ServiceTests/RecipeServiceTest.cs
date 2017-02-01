using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Helpers.Test;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;
using HIS.Recipes.Services.Tests.Helper;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace HIS.Recipes.Services.Tests.ServiceTests
{
    public class RecipeServiceTest:TestBase
    {
        [Fact]
        public async Task GetRecipes()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var output = await service.GetRecipes().ToListAsync();
                Assert.NotNull(output);
                Assert.NotEmpty(output);
                Assert.Equal(this.DbContext.Recipes.Count(), output.Count);

                var firstResult = output.First();
                var firstDbResult = await this.DbContext
                                                .Recipes
                                                .Include(x=>x.Images)
                                                .Include(x=>x.Tags)
                                                    .ThenInclude(x=>x.RecipeTag)
                                                .FirstAsync();

                Assert.Equal(firstDbResult.Id, firstResult.Id);
                Assert.Equal(firstDbResult.Creator, firstResult.Creator);
                Assert.Equal(firstDbResult.LastTimeCooked, firstResult.LastTimeCooked);
                Assert.Equal(firstDbResult.Name, firstResult.Name);
                Assert.Equal(firstDbResult.Images.FirstOrDefault()?.Url, firstResult.ImageUrl);
                Assert.Equal(firstDbResult.Tags.Count, firstResult.Tags.Count);
                Assert.Equal(firstDbResult.Tags.FirstOrDefault()?.RecipeTag.Name, firstResult.Tags.FirstOrDefault());

                Assert.Null(firstResult.Url);
            }
        }

        [Fact]
        public async Task SearchForRecipesByTag()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var tagWith = this.DbContext.RecipeTags.Include(x => x.Recipes).First(x => x.Recipes.Any());

                var searchModel = new RecipeSearchViewModel()
                {
                    Tags = new List<string>() { tagWith.Name }
                };


                var output = await (await service.SearchForRecipes(searchModel)).ToListAsync();
                Assert.NotNull(output);
                Assert.NotEmpty(output);
                Assert.Equal(tagWith.Recipes.Count, output.Count);
                
            }
        }

        [Fact]
        public async Task SearchForRecipesByIngrediant()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var ingrediantWithRecipes = this.DbContext.Ingrediants.Include(x => x.Recipes).First(x => x.Recipes.Any());

                var searchModel = new RecipeSearchViewModel()
                {
                    Ingrediants = new List<string>() { ingrediantWithRecipes.Name }
                };


                var output = await (await service.SearchForRecipes(searchModel)).ToListAsync();
                Assert.NotNull(output);
                Assert.NotEmpty(output);
                Assert.Equal(ingrediantWithRecipes.Recipes.Count, output.Count);

            }
        }


        [Fact]
        public async Task SearchForRecipesByName()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var firstRecipe = await this.DbContext.Recipes.FirstAsync();

                var searchModel = new RecipeSearchViewModel()
                {
                    Name = firstRecipe.Name
                };


                var output = await (await service.SearchForRecipes(searchModel)).ToListAsync();
                Assert.NotNull(output);
                Assert.NotEmpty(output);
                Assert.Equal(1, output.Count);

            }
        }


        [Fact]
        public async Task GetRecipe()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var recipeId = (await DbContext.Recipes.FirstAsync()).Id;
                var recipe = await service.GetRecipeAsync(recipeId);
                var dbRecipe = await DbContext.Recipes
                                                .Include(x=>x.Tags)
                                                    .ThenInclude(x=>x.RecipeTag)
                                                .Include(x => x.Images)
                                                .Include(x=>x.Steps)
                                                .Include(x=>x.Ingrediants)
                                                    .ThenInclude(x=>x.Ingrediant)
                                                .Include(x=>x.Source)
                                                    .ThenInclude(x=>x.Source)
                                                .FirstAsync();

                Assert.Equal(dbRecipe.Id, recipe.Id);
                Assert.Equal(dbRecipe.Name, recipe.Name);
                Assert.Equal(dbRecipe.CookedCounter, recipe.CookedCounter);
                Assert.Equal(dbRecipe.Calories, recipe.Calories);
                Assert.Equal(dbRecipe.NumberOfServings, recipe.NumberOfServings);
                Assert.Equal(dbRecipe.LastTimeCooked, recipe.LastTimeCooked);
                
                Assert.Equal(dbRecipe.Source.Source is RecipeUrlSource ? SourceType.WebSource : SourceType.Cookbook, recipe.Source.Type);
                Assert.Equal(dbRecipe.Source.Page, recipe.Source.Page);
                Assert.Equal(dbRecipe.Source.Source.Name, recipe.Source.Name);
                Assert.Equal(dbRecipe.Source.SourceId, recipe.Source.Id);

                Assert.Equal(dbRecipe.Ingrediants.Count, recipe.Ingrediants.Count());
                var ingrediant = recipe.Ingrediants.First();
                var dbIngrediant = dbRecipe.Ingrediants.First();
                Assert.Equal(dbIngrediant.IngrediantId, ingrediant.Id);
                Assert.Equal(dbIngrediant.Amount, ingrediant.Amount);
                Assert.Equal(dbIngrediant.CookingUnit, ingrediant.Unit);
                Assert.Equal(dbIngrediant.Ingrediant.Name, ingrediant.Name);

                Assert.Equal(recipe.Steps.Count(), dbRecipe.Steps.Count);
                var firstStep = recipe.Steps.First();
                var firstDbStep = dbRecipe.Steps.First();
                Assert.Equal(firstDbStep.Id, firstStep.Id);
                Assert.Equal(firstDbStep.Order, firstStep.Order);
                Assert.Equal(firstDbStep.RecipeId, firstStep.RecipeId);
                Assert.Equal(firstDbStep.Description, firstStep.Description);

                var image = recipe.Images.First();
                var dbImage = dbRecipe.Images.First();
                Assert.Equal(dbImage.Id, image.Id);
                Assert.Equal(dbImage.Filename, image.Name);
                
                Assert.Equal(dbRecipe.Tags.Count, recipe.Tags.Count());
                var firstTag = recipe.Tags.First();
                var firstDbTag = dbRecipe.Tags.First();
                Assert.Equal(firstDbTag.RecipeTagId, firstTag.Id);
                Assert.Equal(firstDbTag.RecipeTag.Name, firstTag.Name);
            }
        }

        [Fact]
        public async Task AddRecipe()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var newRecipe = new RecipeCreationViewModel()
                {
                    Name = "New Recipes",
                    Calories = 123,
                    NumberOfServings = 1,
                    Creator = "Tester"
                };

                var result = await service.AddAsync(newRecipe);
                var dbRecipe = await DbContext.Recipes.FirstAsync(x => x.Id.Equals(result.Id));
                Assert.Equal(DbContext.Recipes.Count(), this.TestData.Recipes.Count + 1);
            }
        }

        [Fact]
        public async Task RemoveRecipe()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var firstRecipe = await DbContext.Recipes.FirstAsync();
                await service.RemoveAsync(firstRecipe.Id);

                Assert.Equal(DbContext.Recipes.Count(), this.TestData.Recipes.Count - 1);
                var result = await DbContext.Recipes.FirstOrDefaultAsync(x => x.Id.Equals(firstRecipe.Id));
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateRecipe()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var recipe = await DbContext.Recipes.FirstAsync();
                var changedRecipe = new RecipeUpdateViewModel()
                {
                    Id = recipe.Id,
                    Name = "New Name",
                    Calories = 234,
                    NumberOfServings = 2
                };
                await service.UpdateAsync(recipe.Id, changedRecipe);

                var newRecipe = await DbContext.Recipes.FirstAsync(x => x.Id.Equals(recipe.Id));

                Assert.Equal(changedRecipe.Name, newRecipe.Name);
                Assert.Equal(changedRecipe.Calories, newRecipe.Calories);
                Assert.Equal(changedRecipe.NumberOfServings, newRecipe.NumberOfServings);
            }
        }

        [Fact]
        public async Task CookNow()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var recipe = await DbContext.Recipes.AsNoTracking().FirstAsync();
                await service.CookNowAsync(recipe.Id);
                var newRecipe = await DbContext.Recipes.AsNoTracking().FirstAsync(x=>x.Id.Equals(recipe.Id));

                Assert.Equal(recipe.CookedCounter + 1, newRecipe.CookedCounter);
                Assert.True(Math.Abs((DateTime.UtcNow - newRecipe.LastTimeCooked).TotalSeconds) < 5);
            }
        }

        private IRecipeService GetService()
        {
            var rep = new RecipeDbRepository.RecipeRepository(this.DbContext, new MockLoggerFactory<object>());
            var mockFactory = new MockLoggerFactory<IngrediantService>();
            IMapper mapper = new Mapper(new MapperConfiguration(m => m.AddProfile<AutoMapperServiceProfile>()));
            return new RecipeService(rep, mapper, mockFactory);
        }

    }
}

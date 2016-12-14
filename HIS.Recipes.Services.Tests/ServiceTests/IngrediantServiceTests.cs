using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HIS.Helpers.Extensions;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Tests.Helper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HIS.Recipes.Services.Tests.ServiceTests
{
    public class IngrediantServiceTests:TestBase
    {
        [Fact]
        public async Task AddIngrediant()
        {
            await InitializeAsync();
            var newIngrediant = "New Ingrediant";

            using (IIngrediantService service = GetService())
            {
                var output = await service.AddAsync(newIngrediant);
                
                Assert.NotNull(output);
                Assert.NotEqual(0, output.Id);
                Assert.Equal(this.TestData.Ingrediants.Count + 1, this.DbContext.Ingrediants.Count());
            }
        }

        [Fact]
        public async Task GetIngrediant()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var output = await service.GetIngrediantList().ToListAsync();
                Assert.NotNull(output);
                Assert.NotEmpty(output);
                Assert.Equal(this.DbContext.Ingrediants.Count(), output.Count);

                var firstIngrediant = await this.DbContext.Ingrediants.Include(x=>x.Recipes).FirstAsync();
                var firstResult = output.First();

                Assert.Equal(firstIngrediant.Id, firstResult.Id);
                Assert.Equal(firstIngrediant.Recipes.Count, firstResult.NumberOfRecipes);
                Assert.Equal(firstIngrediant.Name, firstResult.Name);
                Assert.Null(firstResult.Url);
            }
        }

        [Fact]
        public async Task GetRecipeIngrediants()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var recipe = await DbContext.Recipes
                                              .Include(x=>x.Ingrediants)
                                                .ThenInclude(x=>x.Ingrediant)
                                              .FirstAsync();

                var ingrediantList = await service.GetIngrediantsForRecipe(recipe.Id).ToListAsync();
                Assert.NotNull(ingrediantList);
                Assert.NotEmpty(ingrediantList);

                Assert.Equal(recipe.Ingrediants.Count, ingrediantList.Count);

                var firstResult = ingrediantList.First();
                var firstIngrediant = recipe.Ingrediants.First();

                Assert.Equal(firstIngrediant.IngrediantId, firstResult.Id);
                Assert.Equal(firstIngrediant.Amount, firstResult.Amount);
                Assert.Equal(firstIngrediant.CookingUnit, firstResult.Unit);
                Assert.Equal(firstIngrediant.Ingrediant.Name, firstResult.Name);
                Assert.Null(firstResult.Url);
            }
        }

        [Fact]
        public async Task AddIngrediantToRecipe()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var recipe = await DbContext
                                    .Recipes
                                    .AsNoTracking()
                                    .Include(x=>x.Ingrediants)
                                    .FirstAsync();

                var newIngrediant = await DbContext
                                            .Ingrediants
                                            .AsNoTracking()
                                            .Include(x => x.Recipes)
                                            .FirstAsync(x=>x.Recipes
                                                                .All(y=>y.RecipeId != recipe.Id));

                var alterModel = new AlterIngrediantViewModel()
                {
                    RecipeId = recipe.Id,
                    IngrediantId = newIngrediant.Id,
                    Amount = 1,
                    Unit = CookingUnit.Gramm
                };
                await service.AddOrUpdateIngrediantToRecipeAsync(alterModel);

                var newRecipe = await DbContext
                                .Recipes
                                .AsNoTracking()
                                .Include(x => x.Ingrediants)
                                .FirstAsync(x=>x.Id.Equals(recipe.Id));

                Assert.Equal(recipe.Ingrediants.Count + 1, newRecipe.Ingrediants.Count);
                Assert.False(recipe.Ingrediants.Any(x=>x.IngrediantId.Equals(newIngrediant.Id)));
                Assert.True(newRecipe.Ingrediants.Any(x=>x.IngrediantId.Equals(newIngrediant.Id)));
            }
        }

        [Fact]
        public async Task RemoveIngrediantFromRecipe()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var recipe = await DbContext
                                    .Recipes
                                    .AsNoTracking()
                                    .Include(x => x.Ingrediants)
                                    .FirstAsync();

                var oldIngrediant = recipe.Ingrediants.First();
                
                await service.RemoveIngrediantFromRecipeAsync(recipe.Id, oldIngrediant.IngrediantId);

                var newRecipe = await DbContext
                                .Recipes
                                .AsNoTracking()
                                .Include(x => x.Ingrediants)
                                .FirstAsync(x => x.Id.Equals(recipe.Id));

                Assert.Equal(recipe.Ingrediants.Count - 1, newRecipe.Ingrediants.Count);
                Assert.True(recipe.Ingrediants.Any(x => x.IngrediantId.Equals(oldIngrediant.IngrediantId)));
                Assert.False(newRecipe.Ingrediants.Any(x => x.IngrediantId.Equals(oldIngrediant.IngrediantId)));
            }

        }

        [Fact]
        public async Task RemoveIngrediant()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var ingrediant = await DbContext.Ingrediants.Include(x=>x.Recipes).FirstAsync(x=>x.Recipes.Any());
                var containingRecipe = ingrediant.Recipes.First().RecipeId;

                await service.RemoveAsync(ingrediant.Id);
                var deletedIngrediant = await DbContext.Ingrediants.FirstOrDefaultAsync(x => x.Id.Equals(ingrediant.Id));
                Assert.Null(deletedIngrediant);
                var recipe = await DbContext.Recipes.Include(x=>x.Ingrediants).FirstOrDefaultAsync(x => x.Id.Equals(containingRecipe));
                Assert.NotNull(recipe);
                var recipeIngrediant = recipe.Ingrediants.FirstOrDefault(x => x.IngrediantId.Equals(ingrediant.Id));
                Assert.Null(recipeIngrediant);
            }
        }

        [Fact]
        public async Task UpdateIngrediant()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var ingrediant = await DbContext.Ingrediants.AsNoTracking().FirstAsync();
                var alteredIngrediantModel = new NamedViewModel()
                {
                    Id = ingrediant.Id,
                    Name= "New Name"
                };

                await service.UpdateAsync(ingrediant.Id, alteredIngrediantModel);
                var alteredIngrediant = await DbContext.Ingrediants.FirstAsync(x => x.Id.Equals(ingrediant.Id));

                Assert.Equal(ingrediant.Id, alteredIngrediant.Id);
                Assert.NotEqual(ingrediant.Name, alteredIngrediant.Name);
                Assert.Equal(alteredIngrediantModel.Name, alteredIngrediant.Name);
            }
        }

        private IIngrediantService GetService()
        {
            var rep = new RecipeDbRepository.IngrediantRepository(this.DbContext);
            var mockFactory = new MockLoggerFactory<IngrediantService>();
            IMapper mapper = new Mapper(new MapperConfiguration(m => m.AddProfile<AutoMapperServiceProfile>()));
            return new IngrediantService(rep, mapper, mockFactory);
        }
    }
}

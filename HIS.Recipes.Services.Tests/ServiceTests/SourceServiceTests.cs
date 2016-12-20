using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;
using HIS.Recipes.Services.Tests.Helper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HIS.Recipes.Services.Tests.ServiceTests
{
    public class SourceServiceTests:TestBase
    {
        [Fact]
        public async Task GetCookbooks()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var dbCookbooks = DbContext.RecipeCookbookSources.AsNoTracking();
                var cookbooks = service.GetCookbooks(); 
                Assert.Equal(dbCookbooks.Count(), cookbooks.Count());

                var firstDbEntry = await dbCookbooks.Include(x=>x.RecipeSourceRecipes).FirstAsync();
                var firstEntry = await cookbooks.FirstAsync();
                
                Assert.Equal(firstDbEntry.Id, firstEntry.Id);
                Assert.Equal(firstDbEntry.ISBN, firstEntry.ISBN);
                Assert.Equal(firstDbEntry.Name, firstEntry.Name);
                Assert.Equal(firstDbEntry.PublishingCompany, firstEntry.PublishingCompany);
                Assert.Equal(firstDbEntry.RecipeSourceRecipes.Count, firstEntry.Recipes.Count());
            }
        }

        [Fact]
        public async Task GetSources()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var dbSources = await DbContext.RecipeBaseSources.Include(x=>x.RecipeSourceRecipes).ThenInclude(x=>x.Recipe).ToListAsync();
                var sources = await service.GetSources().ToListAsync();

                var websource = sources.First(x => x.Type == SourceType.WebSource);
                var dbwebSource = dbSources.First(x => x.Id.Equals(websource.Id));
                Assert.Equal(dbwebSource.Id, websource.Id);
                Assert.Equal(dbwebSource.RecipeSourceRecipes.Count, websource.CountRecipes);
                Assert.Equal(dbwebSource.Name, websource.Name);

                var cbsource = sources.First(x => x.Type == SourceType.Cookbook);
                var dbcbSource = dbSources.First(x => x.Id.Equals(cbsource.Id));
                Assert.Equal(dbcbSource.Id, cbsource.Id);
                Assert.Equal(dbcbSource.RecipeSourceRecipes.Count, cbsource.CountRecipes);
                Assert.Equal(dbcbSource.Name, cbsource.Name);

            }
        }

        [Fact]
        public async Task AddCookbook()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var newCookbook = new CookbookSourceCreationViewModel()
                {
                    Name = "My Cookbook",
                    PublishingCompany = "ABC Corp",
                    ISBN = "ISBN 978-3-86680-192-9"
                };

                var result = await service.AddCookbookAsync(newCookbook);
                Assert.NotNull(result);

                Assert.Equal(TestData.Sources.Count + 1, DbContext.RecipeBaseSources.Count());
            }
        }

        [Fact]
        public async Task AddWebSource()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var recipe = await DbContext.Recipes.AsNoTracking().FirstAsync();
                var newWebSource = new WebSourceCreationViewModel() {Name = "chefkoch.de", SourceUrl = "http://www.websource.com"};
                var result = await service.AddWebSourceAsync(recipe.Id, newWebSource);

                Assert.NotNull(result);
                Assert.Equal(TestData.Sources.Count + 1, DbContext.RecipeBaseSources.Count());
                var recipeAfterChange = await this.DbContext.Recipes.Include(x => x.Source).ThenInclude(x=>x.Source).SingleAsync(x=>x.Id.Equals(recipe.Id));
                Assert.NotNull(recipeAfterChange);
                Assert.NotNull(recipeAfterChange.Source);
                var recipeSource = recipeAfterChange.Source.Source as RecipeUrlSource;
                Assert.NotNull(recipeSource);

                Assert.Equal(recipeSource.Id, result.Id);
                Assert.Equal(recipeSource.Url, result.SourceUrl);
            }
        }

        [Fact]
        public async Task UpdateWebSource()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var recipe = await DbContext.Recipes.AsNoTracking().Include(x => x.Source).ThenInclude(x => x.Source).FirstAsync(x => x.Source.Source is RecipeCookbookSource);
                
                var source = await DbContext.RecipeUrlSources.FirstAsync();

                var sourceModel = new WebSourceViewModel()
                {
                    Id = source.Id,
                    Name = source.Name + "1" ,
                    SourceUrl = source.Url
                };

                await service.UpdateWebSourceAsync(recipe.Id, source.Id, sourceModel);

                var recipeAfterChange = await DbContext.Recipes.AsNoTracking().Include(x=>x.Source).ThenInclude(x=>x.Source).SingleAsync(x => x.Id.Equals(recipe.Id));
                Assert.True(recipeAfterChange.Source.Source is RecipeUrlSource);
                Assert.Equal(sourceModel.Name, recipeAfterChange.Source.Source.Name);
            }
        }

        [Fact]
        public async Task UpdateCookbookSource()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var cookbook = await service.GetCookbooks().FirstAsync();
                cookbook.Name = "New Cookbook";
                cookbook.PublishingCompany = "blabla";
                await service.UpdateCookbookAsync(cookbook.Id,cookbook);

                var dbcookbook = await DbContext.RecipeCookbookSources.SingleAsync(x => x.Id.Equals(cookbook.Id));
                Assert.Equal(cookbook.Name, dbcookbook.Name);
                Assert.Equal(cookbook.PublishingCompany, dbcookbook.PublishingCompany);
            }
        }

        [Fact]
        public async Task DeleteSource()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var source = await DbContext.RecipeBaseSources.FirstAsync();
                await service.RemoveSourceAsync(source.Id);

                Assert.Equal(this.TestData.Sources.Count -1, DbContext.RecipeBaseSources.Count());
                var sourceAfterDelete = await DbContext.RecipeBaseSources.FirstOrDefaultAsync(x=>x.Id.Equals(source.Id));
                Assert.Null(sourceAfterDelete);
            }
        }

        [Fact]
        public async Task UpdateAddRecipeToCookbook()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                var cookbook = await service.GetCookbooks().FirstAsync();
                var recipe = await DbContext.Recipes.Include(x => x.Source).ThenInclude(x => x.Source).AsNoTracking().FirstAsync(x => x.Source.Source is RecipeUrlSource);

                await service.UpdateRecipeOnCookbookAsync(recipe.Id, cookbook.Id, 1);

                var recipeAfteUpdate = await DbContext.Recipes.Include(x => x.Source).ThenInclude(x => x.Source).AsNoTracking().FirstAsync(x => x.Id.Equals(recipe.Id));
                Assert.True(recipeAfteUpdate.Source.Source is RecipeCookbookSource);
                Assert.Equal(1, recipeAfteUpdate.Source.Page);
            }
        }

        private ISourceService GetService()
        {
            var baseSourceRep = new RecipeDbRepository.BaseSourceRepository(this.DbContext);
            var webSourceRep = new RecipeDbRepository.WebSourceRepository(this.DbContext);
            var cookbookSourceRep = new RecipeDbRepository.CookbookSourceRepository(this.DbContext);
            var sourceRep = new RecipeSourceRepository(webSourceRep, baseSourceRep, cookbookSourceRep);

            var reciperep = new RecipeDbRepository.RecipeRepository(this.DbContext);
            var mockFactory = new MockLoggerFactory<IngrediantService>();
            IMapper mapper = new Mapper(new MapperConfiguration(m => m.AddProfile<AutoMapperServiceProfile>()));
            return new SourceService(sourceRep, reciperep, mapper, mockFactory);
        }

    }
}

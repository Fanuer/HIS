using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Services;
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
                Assert.Null(websource.Url);

                var cbsource = sources.First(x => x.Type == SourceType.Cookbook);
                var dbcbSource = dbSources.First(x => x.Id.Equals(cbsource.Id));
                Assert.Equal(dbcbSource.Id, cbsource.Id);
                Assert.Equal(dbcbSource.RecipeSourceRecipes.Count, cbsource.CountRecipes);
                Assert.Equal(dbcbSource.Name, cbsource.Name);
                Assert.Null(cbsource.Url);

            }
        }

        [Fact]
        public async Task peng2()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {
                
            }
        }

        [Fact]
        public async Task peng3()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {

            }
        }

        [Fact]
        public async Task peng4()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {

            }
        }

        [Fact]
        public async Task peng5()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {

            }
        }

        [Fact]
        public async Task peng6()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {

            }
        }

        [Fact]
        public async Task peng7()
        {
            await InitializeAsync();
            using (var service = this.GetService())
            {

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

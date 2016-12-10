using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Services;
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
                var queryable = await this.DbContext.Recipes.ToListAsync();

                var output = await service.GetRecipes().ToListAsync();
                Assert.NotNull(output);
                Assert.NotEmpty(output);
                Assert.Equal(this.DbContext.Recipes.Count(), output.Count);
            }
        }

        private IRecipeService GetService()
        {
            var rep = new RecipeDbRepository.RecipeRepository(this.DbContext);
            var mockFactory = new MockLoggerFactory<IngrediantService>();
            IMapper mapper = new Mapper(new MapperConfiguration(m => m.AddProfile<AutoMapperServiceProfile>()));
            return new RecipeService(rep, mapper, mockFactory);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HIS.Helpers.Extensions;
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
                Assert.Equal(this.DbContext.TestDataGenerator.Ingrediants.Count + 1, this.DbContext.Ingrediants.Count());
            }
        }

        [Fact]
        public async Task GetIngrediant()
        {
            await InitializeAsync();

            using (var service = GetService())
            {
                var queryable = await this.DbContext.Ingrediants.ToListAsync();

                var output = await service.GetIngrediantList().ToListAsync();
                Assert.NotNull(output);
                Assert.NotEmpty(output);
                Assert.Equal(this.DbContext.Ingrediants.Count(), output.Count);
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

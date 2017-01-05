using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Helpers.Test;
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
    public class TagServiceTests:TestBase
    {
        [Fact]
        public async Task GetTags()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var tags = await service.GetTags().ToListAsync();
                Assert.NotNull(tags);
                Assert.NotEmpty(tags);

                var firstTag = tags.First();
                var firstDbTag = await DbContext.RecipeTags.Include(x => x.Recipes).FirstAsync(x => x.Id.Equals(firstTag.Id));

                Assert.Equal(firstDbTag.Id, firstTag.Id);
                Assert.Equal(firstDbTag.Name, firstTag.Name);
                Assert.Null(firstTag.Url);
            }
        }

        [Fact]
        public async Task UpdateTag()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var newName = "updated Tag Name";
                var availableTag = await DbContext.RecipeTags.AsNoTracking().FirstAsync();

                await service.UpdateAsync(availableTag.Id, new NamedViewModel() {Id = availableTag.Id, Name = newName});
                var tag = await DbContext.RecipeTags.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(availableTag.Id));

                Assert.Equal(DbContext.RecipeTags.Count(), this.TestData.Tags.Count);
                Assert.NotEqual(availableTag.Name, tag.Name);
                Assert.Equal(newName, tag.Name);
            }
        }


        [Fact]
        public async Task AddTag()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var newTag = await service.AddAsync("New Tag");
                Assert.NotNull(newTag);
                Assert.Equal(DbContext.RecipeTags.Count(), this.TestData.Tags.Count + 1);
            }
        }


        [Fact]
        public async Task RemoveTag()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var availableTag = await DbContext.RecipeTags.AsNoTracking().FirstAsync();
                await service.RemoveAsync(availableTag.Id);

                var tag = await DbContext.RecipeTags.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(availableTag.Id));
                Assert.Equal(DbContext.RecipeTags.Count(), this.TestData.Tags.Count - 1);
                Assert.Null(tag);
            }
        }

        [Fact]
        public async Task AddTagToRecipe()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var tag = await this.DbContext.RecipeTags.Include(x => x.Recipes).SingleAsync(x=>!x.Recipes.Any());
                var recipe = await this.DbContext.Recipes.AsNoTracking().Include(x => x.Tags).FirstAsync(x => x.Tags.All(y => !y.RecipeTagId.Equals(tag.Id)));
                await service.AddTagToRecipeAsync(recipe.Id, tag.Id);

                var recipeAfterChange = await this.DbContext.Recipes.AsNoTracking().Include(x => x.Tags).FirstOrDefaultAsync(x => x.Tags.Any(y => y.RecipeTagId.Equals(tag.Id)));
                Assert.NotNull(recipeAfterChange);

                var tagAfterChange = await this.DbContext.RecipeTags.Include(x => x.Recipes).SingleAsync(x =>x.Id.Equals(tag.Id));
                Assert.True(tagAfterChange.Recipes.Any(x=>x.RecipeId.Equals(recipeAfterChange.Id)));
            }
        }

        [Fact]
        public async Task RemoveTagFromRecipe()
        {
            await InitializeAsync();
            using (var service = GetService())
            {
                var tag = await this.DbContext.RecipeTags.Include(x => x.Recipes).FirstAsync(x => x.Recipes.Any());
                var recipe = await this.DbContext.Recipes.AsNoTracking().FirstAsync(x => x.Id.Equals(tag.Recipes.First().RecipeId));
                await service.RemoveTagFromRecipeAsync(recipe.Id, tag.Id);

                var recipeAfterChange = await this.DbContext.Recipes.AsNoTracking().Include(x => x.Tags).SingleAsync(x=>x.Id.Equals(recipe.Id));
                Assert.True(recipeAfterChange.Tags.All(x=>!x.RecipeTagId.Equals(tag.Id)));

                var tagAfterChange = await this.DbContext.RecipeTags.AsNoTracking().Include(x => x.Recipes).SingleAsync(x => x.Id.Equals(tag.Id));
                Assert.True(tagAfterChange.Recipes.All(x => !x.RecipeId.Equals(recipe.Id)));
            }
        }

        private ITagService GetService()
        {
            var rep = new RecipeDbRepository.TagsRepository(this.DbContext);
            var recipeRep = new RecipeDbRepository.RecipeRepository(this.DbContext);
            var mockFactory = new MockLoggerFactory<IngrediantService>();
            IMapper mapper = new Mapper(new MapperConfiguration(m => m.AddProfile<AutoMapperServiceProfile>()));
            return new TagService(rep, recipeRep, mapper, mockFactory);
        }
    }
}

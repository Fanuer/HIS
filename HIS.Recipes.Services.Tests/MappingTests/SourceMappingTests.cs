using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Models;
using Xunit;

namespace HIS.Recipes.Services.Tests.MappingTests
{
    public class SourceMappingTests
    {
        [Fact]
        public void Convert_WebSourceCreationViewModel_To_RecipeUrlSource()
        {
            Initalize();

            var input = new WebSourceCreationViewModel()
            {
                Name = "My Source",
                SourceUrl = "http://www.test.de/source"
            };

            var output = Mapper.Map<RecipeUrlSource>(input);

            Assert.IsType<RecipeUrlSource>(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.SourceUrl, output.Url);

            Assert.Equal(Guid.Empty, output.Id);
            Assert.NotNull(output.RecipeSourceRecipes);
            Assert.Empty(output.RecipeSourceRecipes);
        }

        [Fact]
        public void Convert_RecipeUrlSource_To_WebSourceViewModel()
        {
            Initalize();

            var input = new RecipeUrlSource()
            {
                Id = Guid.NewGuid(),
                Name = "TestDomain",
                Url = "http://www.test.de/source"
            };

            var output = Mapper.Map<WebSourceViewModel>(input);

            Assert.IsType<WebSourceViewModel>(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Url, output.SourceUrl);
            Assert.Equal(input.Id, output.Id);

            Assert.Null(output.Url);
        }

        [Fact]
        public void Convert_WebSourceViewModel_To_RecipeUrlSource()
        {
            Initalize();

            var input = new WebSourceViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "TestDomain",
                SourceUrl = "http://www.test.de/source"
            };

            var output = Mapper.Map<RecipeUrlSource>(input);

            Assert.IsType<RecipeUrlSource>(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.SourceUrl, output.Url);
            Assert.Equal(input.Id, output.Id);
            
            Assert.NotNull(output.RecipeSourceRecipes);
            Assert.Empty(output.RecipeSourceRecipes);
        }

        [Fact]
        public void Convert_CookbookSourceCreationViewModel_To_RecipeCookbookSource()
        {
            Initalize();

            var input = new CookbookSourceCreationViewModel()
            {
                Name = "Cookbook 1",
                PublishingCompany = "ABC Corp",
                ISBN = "1234"
            };

            var output = Mapper.Map<RecipeCookbookSource>(input);

            Assert.IsType<RecipeCookbookSource>(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.ISBN, output.ISBN);
            Assert.Equal(input.PublishingCompany, output.PublishingCompany);
            Assert.Equal(Guid.Empty, output.Id);

            Assert.NotNull(output.RecipeSourceRecipes);
            Assert.Empty(output.RecipeSourceRecipes);
        }

        [Fact]
        public void Convert_CookbookSourceViewModel_To_RecipeCookbookSource()
        {
            Initalize();

            var input = new CookbookSourceViewModel
            {
                Name = "Cookbook 1",
                PublishingCompany = "ABC Corp",
                ISBN = "1234",
                Id = Guid.NewGuid(),
                Url = "http://www.test.de",
                Recipes = new List<RecipeShortInfoViewModel>()
                {
                    new RecipeShortInfoViewModel() {Id = Guid.NewGuid(), Name = "Recipe 1", Page = 1, Type = SourceType.Cookbook, Url = "http://www.recipeUrl.de"},
                    new RecipeShortInfoViewModel() {Id = Guid.NewGuid(), Name = "Recipe 2", Page = 2, Type = SourceType.Cookbook, Url = "http://www.recipeUrl2.de"},
                }
            };

            var output = Mapper.Map<RecipeCookbookSource>(input);

            Assert.IsType<RecipeCookbookSource>(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.ISBN, output.ISBN);
            Assert.Equal(input.PublishingCompany, output.PublishingCompany);
            Assert.Equal(input.Id, output.Id);

            Assert.NotNull(output.RecipeSourceRecipes);
            Assert.Empty(output.RecipeSourceRecipes);
        }

        [Fact]
        public void Convert_RecipeSourceRecipe_To_RecipeSourceShortInfoViewModel()
        {
            Initalize();

            var source = new RecipeUrlSource() {Name = "WebSource", Id = Guid.NewGuid(), Url = "http://www.websource.de"};
            var recipe = new Recipe() {Name = "recipe 1", Id = Guid.NewGuid()};

            var input = new RecipeSourceRecipe()
            {
                Recipe = recipe,
                RecipeId = recipe.Id,
                Source = source,
                SourceId = source.Id,
                Page = 0
            };

            source.RecipeSourceRecipes.Add(input);
            recipe.SourceId = source.Id;
            recipe.Source = input;

            var output = Mapper.Map<RecipeShortInfoViewModel>(input);

            Assert.IsType<RecipeShortInfoViewModel>(output);
            Assert.Equal(input.Recipe.Name, output.Name);
            Assert.Equal(input.Page, output.Page);
            Assert.Equal(input.Source.GetSourceType(), output.Type);
            Assert.Equal(input.RecipeId, output.Id);

            Assert.Null(output.Url);
        }

        [Fact]
        public void Convert_RecipeCookbookSource_To_CookbookSourceViewModel()
        {
            Initalize();

            var source = new RecipeCookbookSource() { Name = "WebSource", Id = Guid.NewGuid(), ISBN = "1234", PublishingCompany = "ABC Corp."};
            var recipe = new Recipe() { Name = "recipe 1", Id = Guid.NewGuid() };

            var input = new RecipeSourceRecipe()
            {
                Recipe = recipe,
                RecipeId = recipe.Id,
                Source = source,
                SourceId = source.Id,
                Page = 0
            };

            source.RecipeSourceRecipes.Add(input);
            recipe.SourceId = source.Id;
            recipe.Source = input;

            var output = Mapper.Map<CookbookSourceViewModel>(source);

            Assert.IsType<CookbookSourceViewModel>(output);
            Assert.Equal(source.Name, output.Name);
            Assert.Equal(source.Id, output.Id);

            var outputRecipe = output.Recipes.FirstOrDefault();
            Assert.NotNull(outputRecipe);
            Assert.Equal(input.Page, outputRecipe.Page);
            Assert.Equal(source.GetSourceType(), outputRecipe.Type);
            Assert.Equal(recipe.Name, outputRecipe.Name);
            Assert.Equal(recipe.Id, outputRecipe.Id);

            Assert.Null(outputRecipe.Url);
            Assert.Null(output.Url);
        }

        [Fact]
        public void Convert_RecipeBaseSource_To_SourceListEntryViewModel()
        {
            Initalize();

            var source = new RecipeUrlSource() { Name = "WebSource", Id = Guid.NewGuid(), Url = "http://www.websource.de" };
            var recipe = new Recipe() { Name = "recipe 1", Id = Guid.NewGuid() };

            var input = new RecipeSourceRecipe()
            {
                Recipe = recipe,
                RecipeId = recipe.Id,
                Source = source,
                SourceId = source.Id,
                Page = 0
            };

            source.RecipeSourceRecipes.Add(input);
            recipe.SourceId = source.Id;
            recipe.Source = input;

            var output = Mapper.Map<SourceListEntryViewModel>(source);

            Assert.IsType<SourceListEntryViewModel>(output);
            Assert.Equal(input.Source.Name, output.Name);
            Assert.Equal(source.RecipeSourceRecipes.Count, output.CountRecipes);
            Assert.Equal(input.Source.GetSourceType(), output.Type);
            Assert.Equal(input.SourceId, output.Id);

            Assert.Null(output.Url);
        }


        private SourceTestData Initalize()
        {
            Mapper.Initialize(m => m.AddProfile<AutoMapperServiceProfile>());
            return new SourceTestData();
        }

        private class SourceTestData
        {
            public SourceTestData()
            {
                
            }
        }
    }
}

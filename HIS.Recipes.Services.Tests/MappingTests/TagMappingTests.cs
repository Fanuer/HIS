using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Models;
using Xunit;

namespace HIS.Recipes.Services.Tests.MappingTests
{
    public class TagMappingTests
    {
        [Fact]
        public void Convert_String_To_RecipeTag()
        {
            Initialize();
            var input = "New Tag";
            var output = Mapper.Map<RecipeTag>(input);

            Assert.IsType<RecipeTag>(output);
            Assert.Equal(input, output.Name);
            Assert.Equal(Guid.Empty, output.Id);

            Assert.NotNull(output.Recipes);
            Assert.Empty(output.Recipes);
        }

        [Fact]
        public void Convert_RecipeTag_To_NamedViewModel()
        {
            Initialize();
            var input = new RecipeTag()
            {
                Id = Guid.NewGuid(),
                Name = "MyTag"
            };

            var recipe = new Recipe() {Name ="Recipe 1", Id = Guid.NewGuid()};

            var recipeTag = new RecipeRecipeTag()
            {
                Recipe = recipe,
                RecipeId = recipe.Id,
                RecipeTag = input,
                RecipeTagId = input.Id
            };
            input.Recipes.Add(recipeTag);
            recipe.Tags.Add(recipeTag);


            var output = Mapper.Map<NamedViewModel>(input);

            Assert.IsType<NamedViewModel>(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Id, output.Id);
            Assert.Null(output.Url);
        }

        [Fact]
        public void Convert_NamedViewModel_To_RecipeTag()
        {
            Initialize();
            var input = new NamedViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "MyTag",
                Url = "http://www.service.de/tags"
            };
            
            var output = Mapper.Map<RecipeTag>(input);

            Assert.IsType<RecipeTag>(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Id, output.Id);
            Assert.NotNull(output.Recipes);
            Assert.Empty(output.Recipes);
        }

        private void Initialize()
        {
            Mapper.Initialize(m => m.AddProfile<AutoMapperServiceProfile>());
        }
    }
}

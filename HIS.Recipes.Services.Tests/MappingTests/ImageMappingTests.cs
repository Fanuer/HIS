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
    public class ImageMappingTests
    {
        [Fact]
        public void Convert_RecipeImage_To_RecipeImageViewModel()
        {
            Initialize();

            var recipe = new Recipe() {Name= "MyRecipe", Id = 1};

            var input = new RecipeImage()
            {
                Recipe = recipe,
                Id = 1,
                RecipeId = recipe.Id,
                Url = "http://www.service/images/1",
                Filename = "MyImage.jpg"
            };

            var output = Mapper.Map<RecipeImageViewModel>(input);
            Assert.Equal(input.Id, output.Id);
            Assert.Equal(input.Filename, output.Filename);
            Assert.Equal(input.Url, output.Url);
        }

        private void Initialize()
        {
            Mapper.Initialize(m => m.AddProfile<AutoMapperServiceProfile>());

        }
    }
}

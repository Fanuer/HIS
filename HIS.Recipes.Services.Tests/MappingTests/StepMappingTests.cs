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
    public class StepMappingTests
    {
        [Fact]
        public void Convert_StepCreateViewModel_To_RecipeStep()
        {
            Initialize();

            var input = new StepCreateViewModel()
            {
                RecipeId = 1,
                Order = 0,
                Description = "Test Description"
            };

            var output = Mapper.Map<RecipeStep>(input);
            Assert.Equal(0, output.Id);
            Assert.Equal(input.Description, output.Description);
            Assert.Equal(input.Order, output.Order);

            Assert.Equal(input.RecipeId, output.RecipeId);
            Assert.Null(output.Recipe);
        }

        [Fact]
        public void Convert_RecipeStep_To_StepViewModel()
        {
            Initialize();

            var input = new RecipeStep()
            {
                RecipeId = 1,
                Order = 0,
                Description = "Test Description",
                Id = 1
            };

            var output = Mapper.Map<StepViewModel>(input);
            Assert.Equal(input.Id, output.Id);
            Assert.Equal(input.Description, output.Description);
            Assert.Equal(input.Order, output.Order);
            Assert.Equal(input.RecipeId, output.RecipeId);
            Assert.Null(output.Url);
        }

        [Fact]
        public void Convert_StepViewModel_To_RecipeStep()
        {
            Initialize();

            var input = new StepViewModel()
            {
                RecipeId = 1,
                Order = 0,
                Description = "Test Description",
                Id = 1
            };

            var output = Mapper.Map<RecipeStep>(input);
            Assert.Equal(input.Id, output.Id);
            Assert.Equal(input.Description, output.Description);
            Assert.Equal(input.Order, output.Order);
            Assert.Equal(input.RecipeId, output.RecipeId);
            Assert.Null(output.Recipe);
        }

        private void Initialize()
        {
            Mapper.Initialize(m => m.AddProfile<AutoMapperServiceProfile>());
        }
    }
}

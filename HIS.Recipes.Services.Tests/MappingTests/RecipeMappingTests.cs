using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Models;
using NuGet.Packaging;
using Xunit;

namespace HIS.Recipes.Services.Tests.MappingTests
{
    public class RecipeMappingTests
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS

        [Fact]
        public void Convert_RecipeCreationViewModel_To_Recipe()
        {
            var testData = Initialize();
            var input = testData.CreationModel;
            var output = Mapper.Map<Recipe>(input);

            Assert.IsType<Recipe>(output);

            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Creator, output.Creator);
            Assert.Equal(input.Calories, output.Calories);
            Assert.Equal(input.NumberOfServings, output.NumberOfServings);

            Assert.Equal(0, output.Id);
            Assert.Equal(0, output.CookedCounter);
            Assert.Equal(new DateTime(), output.LastTimeCooked);
            Assert.Equal(0, output.SourceId);
            
            Assert.NotNull(output.Ingrediants);
            Assert.Empty(output.Ingrediants);

            Assert.NotNull(output.Images);
            Assert.Empty(output.Images);

            Assert.NotNull(output.Steps);
            Assert.Empty(output.Steps);

            Assert.NotNull(output.Tags);
            Assert.Empty(output.Tags);

            Assert.NotNull(output.Source);
            Assert.Equal(0, output.SourceId);
        }

        [Fact]
        public void Convert_Recipe_To_RecipeUpdateModel()
        {
            var testData = Initialize();
            var input = testData.Recipe;
            var output = Mapper.Map<RecipeUpdateViewModel>(input);

            Assert.IsType<RecipeUpdateViewModel>(output);

            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Creator, output.Creator);
            Assert.Equal(input.Calories, output.Calories);
            Assert.Equal(input.NumberOfServings, output.NumberOfServings);
            Assert.Equal(input.Id, output.Id);
            Assert.Null(output.Url);
        }

        [Fact]
        public void Convert_RecipeUpdate_To_Recipe()
        {
            var testData = Initialize();
            var input = testData.UpdateModel;
            var output = Mapper.Map<Recipe>(input);

            Assert.IsType<Recipe>(output);

            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Creator, output.Creator);
            Assert.Equal(input.Calories, output.Calories);
            Assert.Equal(input.NumberOfServings, output.NumberOfServings);
            Assert.Equal(input.Id, output.Id);

            Assert.Equal(0, output.CookedCounter);
            Assert.Equal(new DateTime(), output.LastTimeCooked);
            Assert.Equal(0, output.SourceId);

            Assert.NotNull(output.Ingrediants);
            Assert.Empty(output.Ingrediants);

            Assert.NotNull(output.Images);
            Assert.Empty(output.Images);

            Assert.NotNull(output.Steps);
            Assert.Empty(output.Steps);

            Assert.NotNull(output.Tags);
            Assert.Empty(output.Tags);

            Assert.NotNull(output.Source);
            Assert.Equal(0, output.SourceId);
        }

        [Fact]
        public void Convert_Recipe_To_ShortRecipeViewModel()
        {
            var testData = Initialize();
            var input = testData.Recipe;
            var output = Mapper.Map<ShortRecipeViewModel>(input);

            Assert.IsType<ShortRecipeViewModel>(output);

            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Creator, output.Creator);
            Assert.Equal(input.LastTimeCooked, output.LastTimeCooked);
            Assert.Equal(input.Id, output.Id);

            Assert.Null(output.Url);

            Assert.Equal(testData.Recipe.Images.First().Url, output.ImageUrl);
            Assert.NotEmpty(output.Tags);
            Assert.Equal(testData.Recipe.Tags.Count, output.Tags.Count());
            Assert.Equal(testData.Recipe.Tags.First().RecipeTag.Name, output.Tags.First());
        }

        [Fact]
        public void Convert_Recipe_To_FullRecipeViewModel()
        {
            var testData = Initialize();
            var input = testData.Recipe;
            var output = Mapper.Map<FullRecipeViewModel>(input);

            Assert.IsType<FullRecipeViewModel>(output);

            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Creator, output.Creator);
            Assert.Equal(input.LastTimeCooked, output.LastTimeCooked);
            Assert.Equal(input.Id, output.Id);
            Assert.Equal(input.CookedCounter, output.CookedCounter);
            Assert.Equal(input.Calories, output.Calories);
            Assert.Equal(input.NumberOfServings, output.NumberOfServings);
            Assert.Null(output.Url);

            Assert.NotEmpty(output.Tags);
            Assert.Equal(input.Tags.Count, output.Tags.Count());
            var firstOutputTag = output.Tags.First();
            var firstInputTag = input.Tags.First();
            Assert.Equal(firstInputTag.RecipeTag.Name, firstOutputTag.Name);
            Assert.Equal(firstInputTag.RecipeTagId, firstOutputTag.Id);
            Assert.Null(firstOutputTag.Url);

            Assert.NotEmpty(output.Ingrediants);
            Assert.Equal(input.Ingrediants.Count, output.Ingrediants.Count());
            var firstOutputIngrediant = output.Ingrediants.First();
            var firstInputIngrediant = input.Ingrediants.First();
            Assert.Equal(firstInputIngrediant.Amount, firstOutputIngrediant.Amount);
            Assert.Equal(firstInputIngrediant.CookingUnit, firstOutputIngrediant.Unit);
            Assert.Equal(firstInputIngrediant.IngrediantId, firstOutputIngrediant.Id);
            Assert.Equal(firstInputIngrediant.Ingrediant.Name, firstOutputIngrediant.Name);

            Assert.NotEmpty(output.Images);
            Assert.Equal(input.Images.Count, output.Images.Count());
            var firstOutputImage = output.Images.First();
            var firstInputImage = input.Images.First();
            Assert.Equal(firstInputImage.Filename, firstOutputImage.Name);
            Assert.Equal(firstInputImage.Id, firstOutputImage.Id);
            Assert.Equal(firstInputImage.Url, firstOutputImage.Url);

            Assert.NotEmpty(output.Steps);
            Assert.Equal(input.Steps.Count, output.Steps.Count());
            var firstOutputStep = output.Steps.First();
            var firstInputStep = input.Steps.First();
            Assert.Equal(firstInputStep.Description, firstOutputStep.Description);
            Assert.Equal(firstInputStep.Order, firstOutputStep.Order);
            Assert.Equal(firstInputStep.RecipeId, firstOutputStep.RecipeId);
            Assert.Equal(firstInputStep.Id, firstOutputStep.Id);
            Assert.Null(firstOutputStep.Url);

            Assert.NotNull(output.Type);
            Assert.Equal(input.Source.Source.GetSourceType(), output.Type.Type);
            Assert.Equal(input.Source.Page, output.Type.Page);
            Assert.Equal(input.Source.SourceId, output.Type.Id);
            Assert.Equal(input.Source.Source.Name, output.Type.Name);

        }

        private RecipeTestData Initialize()
        {
            Mapper.Initialize(m => m.AddProfile<AutoMapperServiceProfile>());
            return new RecipeTestData();
        }
        #endregion

        #region PROPERTIES
        #endregion

        #region NESTED

        private class RecipeTestData
        {
            #region CONST
            #endregion

            #region FIELDS
            #endregion

            #region CTOR

            public RecipeTestData()
            {
                #region Init Recipe

                // ------------------- Generel ----------------------
                Recipe = new Recipe()
                {
                    Name = "My Recipe",
                    Id = 1,
                    Calories = 1,
                    CookedCounter = 2,
                    Creator = "Tester",
                    LastTimeCooked = new DateTime(),
                    NumberOfServings = 3

                };

                // ------------------- Images ----------------------
                var image = new RecipeImage()
                {
                    Recipe = this.Recipe,
                    Id = 1,
                    RecipeId = Recipe.Id,
                    Url = "http://imageUrl.de",
                    Filename = "MyImage.jpg"
                };
                Recipe.Images.Add(image);

                // ------------------- Steps ----------------------

                var steps = new List<RecipeStep>()
                {
                    new RecipeStep() { Recipe = Recipe, Id = 1, RecipeId = Recipe.Id, Order = 0, Description = "Step 1"},
                    new RecipeStep() { Recipe = Recipe, Id = 2, RecipeId = Recipe.Id, Order = 1, Description = "Step 2"}
                };
                Recipe.Steps.AddRange(steps);

                // ------------------- Tags ----------------------

                var tags = new List<RecipeTag>()
                {
                    new RecipeTag() { Id = 1, Name = "Tag 1"},
                    new RecipeTag() { Id = 2, Name = "Tag 2"},
                };

                var recipeTags = new List<RecipeRecipeTag>()
                {
                    new RecipeRecipeTag() {Recipe = Recipe, RecipeId = Recipe.Id, RecipeTag = tags.First(), RecipeTagId = tags.First().Id},
                    new RecipeRecipeTag() {Recipe = Recipe, RecipeId = Recipe.Id, RecipeTag = tags.Last(), RecipeTagId = tags.Last().Id},
                };

                tags.First().Recipes.Add(recipeTags.First());
                tags.Last().Recipes.Add(recipeTags.Last());
                Recipe.Tags.AddRange(recipeTags);

                // ------------------- Source ----------------------

                var source = new RecipeUrlSource()
                {
                    Id = 1,
                    Name = "WebSource",
                    Url = "http://www.websource.de"
                };
                var recipeSource = new RecipeSourceRecipe()
                {
                    Page = 0,
                    Recipe = Recipe,
                    RecipeId = Recipe.Id,
                    Source = source,
                    SourceId = source.Id
                };
                source.RecipeSourceRecipes.Add(recipeSource);
                Recipe.Source = recipeSource;
                Recipe.SourceId = source.Id;

                // ------------------- Ingrediant ----------------------

                var ingrediant = new Ingrediant()
                {
                    Id = 1,
                    Name = "Ingrediant 1"
                };

                var recipeIngrediant = new RecipeIngrediant()
                {
                    Recipe = Recipe,
                    RecipeId = Recipe.Id,
                    Ingrediant = ingrediant,
                    IngrediantId = ingrediant.Id,
                    Amount = 1,
                    CookingUnit = CookingUnit.Gramm
                };
                ingrediant.Recipes.Add(recipeIngrediant);
                Recipe.Ingrediants.Add(recipeIngrediant);

                #endregion
                UpdateModel = new RecipeUpdateViewModel()
                {
                    Id = 1,
                    Name = "Old Recipe",
                    Url = "http://www.webservice.de/recipes/1",
                    Calories = Recipe.Calories,
                    NumberOfServings = Recipe.NumberOfServings,
                    Creator = Recipe.Creator
                };
                CreationModel = new RecipeCreationViewModel()
                {
                    Name = UpdateModel.Name,
                    Calories = UpdateModel.Calories,
                    NumberOfServings = UpdateModel.NumberOfServings,
                    Creator = UpdateModel.Creator
                };

            }
            #endregion

            #region METHODS
            #endregion

            #region PROPERTIES

            public Recipe Recipe { get; set; }
            public RecipeUpdateViewModel UpdateModel { get; set; }
            public RecipeCreationViewModel CreationModel { get; set; }
            #endregion

        }

        #endregion
    }
}

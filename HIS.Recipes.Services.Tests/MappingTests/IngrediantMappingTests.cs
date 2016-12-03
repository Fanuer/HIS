﻿using System;
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
    public class IngrediantMappingTests
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS
        [Fact]
        public void Test_Ingrediant2IngrediantStatisticViewModel()
        {
            var testdata = Initialize();

            var input = testdata.Ingrediants.First();
            var result = Mapper.Map<IngrediantStatisticViewModel>(input);

            Assert.IsType<IngrediantStatisticViewModel>(result);
            Assert.Equal(input.RecipeIngrediants.Count, result.NumberOfRecipes);
            Assert.Equal(input.Name, result.Name);
            Assert.Equal(input.Id, result.Id);
        }

        [Fact]
        public void Test_RecipeIngrediant2IngrediantViewModel()
        {
            var testdata = Initialize();

            var input = testdata.Ingrediants.First().RecipeIngrediants.First();
            var result = Mapper.Map<IngrediantViewModel>(input);

            Assert.IsType<IngrediantViewModel>(result);
            Assert.Equal(input.Amount, result.Amount);
            Assert.Equal(input.CookingUnit, result.Unit);
            Assert.Equal(input.IngrediantId, result.Id);
            Assert.Equal(input.Ingrediant.Name, result.Name);
            Assert.Null(result.Url);
        }

        [Fact]
        public void Test_String2NamedViewModel()
        {
            Initialize();

            var input = "New Ingrediant";
            var result = Mapper.Map<NamedViewModel>(input);

            Assert.IsType<NamedViewModel>(result);
            Assert.Equal(input, result.Name);
            Assert.Equal(Guid.Empty, result.Id);
            Assert.Null(result.Url);
        }

        [Fact]
        public void Test_NamedViewModel2Ingrediant()
        {
            Initialize();

            var input = new NamedViewModel() {Id = Guid.NewGuid(), Name = "New Ingrediant", Url = "http://www.test.de"};
            var result = Mapper.Map<Ingrediant>(input);

            Assert.IsType<Ingrediant>(result);
            Assert.Equal(input.Id, result.Id);
            Assert.Equal(input.Name, result.Name);
            Assert.NotNull(result.RecipeIngrediants);
            Assert.Equal(0, result.RecipeIngrediants.Count);
        }

        [Fact]
        public void Test_Ingrediant2NamedViewModel()
        {
            var testdata = Initialize();

            var input = testdata.Ingrediants.First();
            var result = Mapper.Map<NamedViewModel>(input);

            Assert.IsType<NamedViewModel>(result);
            Assert.Equal(input.Id, result.Id);
            Assert.Equal(input.Name, result.Name);
            Assert.Null(result.Url);
        }
        private IngrediantTestData Initialize()
        {
            Mapper.Initialize(m => m.AddProfile<AutoMapperServiceProfile>());
            return new IngrediantTestData();
        }

        #endregion

        #region PROPERTIES
        

        #endregion

        #region Nested

        private class IngrediantTestData
        {

            #region CONST
            #endregion

            #region FIELDS
            #endregion

            #region CTOR

            public IngrediantTestData()
            {
                Recipes = new List<Recipe>()
            {
                new Recipe() {Name = "Test Recipe 1", Id = Guid.NewGuid(), Calories = 1, CookedCounter = 0, LastTimeCooked = new DateTime(), NumberOfServings = 1, SourceId = Guid.NewGuid(), Creator = "Tester"},
                new Recipe() {Name = "Test Recipe 2", Id = Guid.NewGuid(), Calories = 1, CookedCounter = 0, LastTimeCooked = new DateTime(), NumberOfServings = 1, SourceId = Guid.NewGuid(), Creator = "Tester"},
                new Recipe() {Name = "Test Recipe 3", Id = Guid.NewGuid(), Calories = 1, CookedCounter = 0, LastTimeCooked = new DateTime(), NumberOfServings = 1, SourceId = Guid.NewGuid(), Creator = "Tester"}
            };

                Ingrediants = new List<Ingrediant>()
            {
                new Ingrediant() { Id = Guid.NewGuid(), Name = "Test Ingrediant 1"},
                new Ingrediant() { Id = Guid.NewGuid(), Name = "Test Ingrediant 2"},
                new Ingrediant() { Id = Guid.NewGuid(), Name = "Test Ingrediant 3"},
            };

                var firstRecipe = Recipes.First();
                var recipe1Ingrediant1 = new RecipeIngrediant() { Amount = 1, CookingUnit = CookingUnit.Gramm, Recipe = firstRecipe, RecipeId = firstRecipe.Id, Ingrediant = Ingrediants.First(), IngrediantId = Ingrediants.First().Id };
                var recipe1Ingrediant2 = new RecipeIngrediant() { Amount = 1, CookingUnit = CookingUnit.Gramm, Recipe = firstRecipe, RecipeId = firstRecipe.Id, Ingrediant = Ingrediants[1], IngrediantId = Ingrediants[1].Id };
                var recipe1Ingrediant3 = new RecipeIngrediant() { Amount = 1, CookingUnit = CookingUnit.Gramm, Recipe = firstRecipe, RecipeId = firstRecipe.Id, Ingrediant = Ingrediants.Last(), IngrediantId = Ingrediants.Last().Id };

                firstRecipe.Ingrediants.Add(recipe1Ingrediant1);
                firstRecipe.Ingrediants.Add(recipe1Ingrediant2);
                firstRecipe.Ingrediants.Add(recipe1Ingrediant3);

                Ingrediants[0].RecipeIngrediants.Add(recipe1Ingrediant1);
                Ingrediants[1].RecipeIngrediants.Add(recipe1Ingrediant2);
                Ingrediants[2].RecipeIngrediants.Add(recipe1Ingrediant3);
            }
            #endregion

            #region METHODS
            #endregion

            #region PROPERTIES
            public List<Ingrediant> Ingrediants { get; set; }
            private List<Recipe> Recipes { get; set; }
            #endregion

        }


        #endregion

    }
}
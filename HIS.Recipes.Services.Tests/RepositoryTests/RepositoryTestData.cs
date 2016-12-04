using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Models;
using Microsoft.Extensions.Options;

namespace HIS.Recipes.Services.Tests.RepositoryTests
{
    internal class RepositoryTestData
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public RepositoryTestData()
        {
            this.Recipes = new List<Recipe>();
            this.Sources = new List<RecipeBaseSource>();
            this.Steps = new List<RecipeStep>();
            this.Images = new List<RecipeImage>();
            this.Ingrediants = new List<Ingrediant>();
            this.Tags = new List<RecipeTag>();
            this.InitDb();
        }
        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        #endregion

        private void InitDb()
        {
            //-------------------- Recipes ---------------------------------------

            var recipe1 = new Recipe()
            {
                Id = 0,
                Name = "Recipe 1",
                Calories = 1,
                CookedCounter = 0,
                Creator = "Tester",
                LastTimeCooked = new DateTime(),
                NumberOfServings = 1
            };

            var recipe2 = new Recipe()
            {
                Id = 0,
                Name = "Recipe 1",
                Calories = 1,
                CookedCounter = 0,
                Creator = "Tester",
                LastTimeCooked = new DateTime(),
                NumberOfServings = 1
            };

            this.Recipes.Add(recipe1);
            this.Recipes.Add(recipe2);

            //-------------------- Sources ---------------------------------------

            var cookbookSource = new RecipeCookbookSource()
            {
                Id = 0,
                ISBN = "ISBN 978-3-86680-192-9",
                Name = "Cookbook 1",
                PublishingCompany = "ABC Corp"
            };

            var webSource = new RecipeUrlSource()
            {
                Id = 0,
                Name = "Chefkoch.de",
                Url = "http://www.chefkoch.de/recipe/1"
            };
            this.Sources.Add(cookbookSource);
            this.Sources.Add(webSource);

            var cookbookRecipe1Connection = new RecipeSourceRecipe()
            {
                Page = 4,
                Recipe = recipe1,
                RecipeId = recipe1.Id,
                Source = cookbookSource,
                SourceId = cookbookSource.Id
            };
            cookbookSource.RecipeSourceRecipes.Add(cookbookRecipe1Connection);
            recipe1.Source = cookbookRecipe1Connection;
            recipe1.SourceId = cookbookSource.Id;

            var webSourceRecipe2Connection= new RecipeSourceRecipe()
            {
                Recipe = recipe2,
                RecipeId = recipe2.Id,
                Source = webSource,
                SourceId = webSource.Id,
                Page = 0
            };
            webSource.RecipeSourceRecipes.Add(webSourceRecipe2Connection);
            recipe2.Source = webSourceRecipe2Connection;
            recipe2.SourceId = webSourceRecipe2Connection.SourceId;

            //-------------------- Steps ---------------------------------------

            var recipe1Step1 = new RecipeStep() {Id = 0, Recipe = recipe1,RecipeId = recipe1.Id, Order = 0, Description = "Desc 1"};
            var recipe1Step2 = new RecipeStep() {Id = 0, Recipe = recipe1,RecipeId = recipe1.Id, Order = 1, Description = "Desc 2"};
            var recipe2Step1 = new RecipeStep() {Id = 0, Recipe = recipe2,RecipeId = recipe2.Id, Order = 0, Description = "Desc 1"};
            var recipe2Step2 = new RecipeStep() {Id = 0, Recipe = recipe2,RecipeId = recipe2.Id, Order = 1, Description = "Desc 2"};
            var recipe2Step3 = new RecipeStep() {Id = 0, Recipe = recipe2,RecipeId = recipe2.Id, Order = 2, Description = "Desc 3"};

            this.Steps.Add(recipe1Step1);
            this.Steps.Add(recipe1Step2);
            this.Steps.Add(recipe2Step1);
            this.Steps.Add(recipe2Step2);
            this.Steps.Add(recipe2Step3);

            recipe1.Steps.Add(recipe1Step1);
            recipe1.Steps.Add(recipe1Step2);

            recipe2.Steps.Add(recipe2Step1);
            recipe2.Steps.Add(recipe2Step2);
            recipe2.Steps.Add(recipe2Step3);
            
            //-------------------- Ingrediants ---------------------------------------

            var ingrediant1 = new Ingrediant() {Id = 0, Name = "Ingrediant 1"};
            var ingrediant2 = new Ingrediant() {Id = 0, Name = "Ingrediant 2"};
            var ingrediant3 = new Ingrediant() {Id = 0, Name = "Ingrediant 3"};
            var ingrediant4 = new Ingrediant() {Id = 0, Name = "Ingrediant 4"};
            var ingrediant5 = new Ingrediant() {Id = 0, Name = "Ingrediant 5"};

            this.Ingrediants.Add(ingrediant1);
            this.Ingrediants.Add(ingrediant2);
            this.Ingrediants.Add(ingrediant3);
            this.Ingrediants.Add(ingrediant4);
            this.Ingrediants.Add(ingrediant5);

            var recipe1Ingrediant1Con = new RecipeIngrediant() {Recipe = recipe1, RecipeId = recipe1.Id, Ingrediant = ingrediant1, IngrediantId = ingrediant1.Id, Amount = 1, CookingUnit = CookingUnit.Gramm};
            var recipe1Ingrediant2Con = new RecipeIngrediant() {Recipe = recipe1, RecipeId = recipe1.Id, Ingrediant = ingrediant2, IngrediantId = ingrediant2.Id, Amount = 2, CookingUnit = CookingUnit.Kilogramm};

            recipe1.Ingrediants.Add(recipe1Ingrediant1Con);
            recipe1.Ingrediants.Add(recipe1Ingrediant2Con);
            ingrediant1.RecipeIngrediants.Add(recipe1Ingrediant1Con);
            ingrediant2.RecipeIngrediants.Add(recipe1Ingrediant2Con);

            var recipe2Ingrediant1Con = new RecipeIngrediant() { Recipe = recipe2, RecipeId = recipe2.Id, Ingrediant = ingrediant1, IngrediantId = ingrediant1.Id, Amount = 1, CookingUnit = CookingUnit.Bunch };
            var recipe2Ingrediant4Con = new RecipeIngrediant() { Recipe = recipe2, RecipeId = recipe2.Id, Ingrediant = ingrediant4, IngrediantId = ingrediant4.Id, Amount = 4, CookingUnit = CookingUnit.Liter };
            var recipe2Ingrediant5Con = new RecipeIngrediant() { Recipe = recipe2, RecipeId = recipe2.Id, Ingrediant = ingrediant5, IngrediantId = ingrediant5.Id, Amount = 5, CookingUnit = CookingUnit.Msp };

            recipe2.Ingrediants.Add(recipe2Ingrediant1Con);
            recipe2.Ingrediants.Add(recipe2Ingrediant4Con);
            recipe2.Ingrediants.Add(recipe2Ingrediant5Con);
            ingrediant1.RecipeIngrediants.Add(recipe2Ingrediant1Con);
            ingrediant4.RecipeIngrediants.Add(recipe2Ingrediant4Con);
            ingrediant5.RecipeIngrediants.Add(recipe2Ingrediant5Con);

            //-------------------- Images ---------------------------------------

            var image1 = new RecipeImage(){Id = 0, Recipe = recipe1, RecipeId = recipe1.Id, Filename = "File1.jpg", Url = "http://www.service.de/images/1"};
            var image2 = new RecipeImage(){Id = 0, Recipe = recipe1, RecipeId = recipe1.Id, Filename = "File2.jpg", Url = "http://www.service.de/images/2"};
            var image3 = new RecipeImage(){Id = 0, Recipe = recipe2, RecipeId = recipe2.Id, Filename = "File3.jpg", Url = "http://www.service.de/images/3"};

            this.Images.Add(image1);
            this.Images.Add(image2);
            this.Images.Add(image3);

            recipe1.Images.Add(image1);
            recipe1.Images.Add(image2);
            recipe2.Images.Add(image3);

            //-------------------- Tags ---------------------------------------

            var tag1 = new RecipeTag() {Id = 0, Name = "Tag 1"};
            var tag2 = new RecipeTag() {Id = 0, Name = "Tag 2"};
            var tag3 = new RecipeTag() {Id = 0, Name = "Tag 3"};
            var tag4 = new RecipeTag() {Id = 0, Name = "Tag 4"};

            this.Tags.Add(tag1);
            this.Tags.Add(tag2);
            this.Tags.Add(tag3);
            this.Tags.Add(tag4);

            var recipe1Tag1Con = new RecipeRecipeTag(recipe1, tag1);
            var recipe1Tag2Con = new RecipeRecipeTag(recipe1, tag2);
            var recipe2Tag1Con = new RecipeRecipeTag(recipe2, tag1);
            var recipe2Tag3Con = new RecipeRecipeTag(recipe2, tag3);

            tag1.Recipes.Add(recipe1Tag1Con);
            tag1.Recipes.Add(recipe2Tag1Con);
            tag2.Recipes.Add(recipe1Tag2Con);
            tag3.Recipes.Add(recipe2Tag3Con);

            recipe1.Tags.Add(recipe1Tag1Con);
            recipe1.Tags.Add(recipe1Tag2Con);
            recipe1.Tags.Add(recipe2Tag1Con);
            recipe1.Tags.Add(recipe2Tag3Con);
        }

        public async Task WriteTestDataToDataBaseAsync(RecipeDbContext context)
        {
            foreach (var recipe in Recipes)
            {
                await context.Recipes.AddAsync(recipe);
            }
            foreach (var ingrediant in Ingrediants.Where(x=>x.RecipeIngrediants.Count == 0))
            {
                await context.Ingrediants.AddAsync(ingrediant);
            }

            foreach (var tags in Tags.Where(x => x.Recipes.Count == 0))
            {
                await context.RecipeTags.AddAsync(tags);
            }
            await context.SaveChangesAsync();
        }

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<RecipeBaseSource> Sources{ get; set; }
        public ICollection<RecipeStep> Steps { get; set; }
        public ICollection<Ingrediant> Ingrediants { get; set; }
        public ICollection<RecipeImage> Images { get; set; }
        public ICollection<RecipeTag> Tags { get; set; }
    }
}

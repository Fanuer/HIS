using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace HIS.Recipes.Services.DB
{
    internal class RecipeDbContext : DbContext
    {

        #region CONST

        #endregion

        #region FIELDS

        #endregion

        #region CTOR

        public RecipeDbContext(DbContextOptions options) : base(options)
        {
            this.TestDataGenerator = new MyTestDataGenerator(this);
        }

        public RecipeDbContext()
        {
            this.TestDataGenerator = new MyTestDataGenerator(this);
        }

        #endregion

        #region METHODS

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngrediant>().HasKey(x => new { x.RecipeId, x.IngrediantId });
            modelBuilder.Entity<RecipeSourceRecipe>().HasKey(x => new { x.RecipeId, x.SourceId });
            modelBuilder.Entity<RecipeRecipeTag>().HasKey(x => new { x.RecipeId, x.RecipeTagId });

            modelBuilder.Entity<RecipeIngrediant>()
                .HasOne(x => x.Recipe)
                .WithMany(x => x.Ingrediants)
                .HasForeignKey(x => x.RecipeId);

            modelBuilder.Entity<RecipeIngrediant>()
                .HasOne(x => x.Ingrediant)
                .WithMany(x => x.Recipes)
                .HasForeignKey(x => x.IngrediantId);

            modelBuilder.Entity<RecipeRecipeTag>()
                .HasOne(x => x.Recipe)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.RecipeId);

            modelBuilder.Entity<RecipeRecipeTag>()
                .HasOne(x => x.RecipeTag)
                .WithMany(x => x.Recipes)
                .HasForeignKey(x => x.RecipeTagId);

            modelBuilder.Entity<RecipeSourceRecipe>()
                .HasOne(x => x.Recipe)
                .WithOne(x => x.Source);

            modelBuilder.Entity<RecipeSourceRecipe>()
                .HasOne(x => x.Source)
                .WithMany(x => x.RecipeSourceRecipes)
                .HasForeignKey(x => x.SourceId);

            modelBuilder.Entity<Recipe>().HasMany(x => x.Images).WithOne(x => x.Recipe);
            modelBuilder.Entity<Recipe>().HasMany(x => x.Steps).WithOne(x => x.Recipe);

            modelBuilder.Entity<RecipeTag>().HasAlternateKey(x => x.Name);
            modelBuilder.Entity<Ingrediant>().HasAlternateKey(x => x.Name);
            modelBuilder.Entity<RecipeSourceRecipe>().HasAlternateKey(x => new { x.RecipeId, x.SourceId, x.Page });
            modelBuilder.Entity<RecipeStep>().HasAlternateKey(x => new { x.RecipeId, x.Order });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use InMemory-Database for testing
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;");
            }
        }

        public override void Dispose()
        {
            if (this.TestDataGenerator.TestDataCreated)
            {
                this.TestDataGenerator.RemoveTestData();
            }
            base.Dispose();
        }

        #endregion

        #region PROPERTIES

        internal DbSet<Ingrediant> Ingrediants { get; set; }
        internal DbSet<Recipe> Recipes { get; set; }
        internal DbSet<RecipeBaseSource> RecipeBaseSources { get; set; }
        internal DbSet<RecipeCookbookSource> RecipeCookbookSources { get; set; }
        internal DbSet<RecipeUrlSource> RecipeUrlSources { get; set; }
        internal DbSet<RecipeImage> RecipeImages { get; set; }
        internal DbSet<RecipeIngrediant> RecipeIngrediants { get; set; }
        internal DbSet<RecipeRecipeTag> RecipeRecipeTags { get; set; }
        internal DbSet<RecipeSourceRecipe> RecipeSourceRecipes { get; set; }
        internal DbSet<RecipeStep> RecipeSteps { get; set; }
        internal DbSet<RecipeTag> RecipeTags { get; set; }


        internal MyTestDataGenerator TestDataGenerator { get; set; }

        #endregion

        #region NESTED

        internal class MyTestDataGenerator
        {

            #region CONST

            #endregion

            #region FIELDS

            #endregion

            #region CTOR

            public MyTestDataGenerator(RecipeDbContext context)
            {
                this.Context = context;
                Recipes = new List<Recipe>();
                Sources = new List<RecipeBaseSource>();
                Steps = new List<RecipeStep>();
                Ingrediants = new List<Ingrediant>();
                Images = new List<RecipeImage>();
                Tags = new List<RecipeTag>();

                InitLocalTestData();
            }


            #endregion

            #region METHODS

            private void InitLocalTestData()
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
                    Name = "Recipe 2",
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

                //-------------------- Steps ---------------------------------------

                var recipe1Step1 = new RecipeStep() { Id = 0, Recipe = recipe1, RecipeId = recipe1.Id, Order = 0, Description = "Desc 1" };
                var recipe1Step2 = new RecipeStep() { Id = 0, Recipe = recipe1, RecipeId = recipe1.Id, Order = 1, Description = "Desc 2" };
                var recipe2Step1 = new RecipeStep() { Id = 0, Recipe = recipe2, RecipeId = recipe2.Id, Order = 0, Description = "Desc 1" };
                var recipe2Step2 = new RecipeStep() { Id = 0, Recipe = recipe2, RecipeId = recipe2.Id, Order = 1, Description = "Desc 2" };
                var recipe2Step3 = new RecipeStep() { Id = 0, Recipe = recipe2, RecipeId = recipe2.Id, Order = 2, Description = "Desc 3" };

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

                var ingrediant1 = new Ingrediant() { Id = 0, Name = "Ingrediant 1" };
                var ingrediant2 = new Ingrediant() { Id = 0, Name = "Ingrediant 2" };
                var ingrediant3 = new Ingrediant() { Id = 0, Name = "Ingrediant 3" };
                var ingrediant4 = new Ingrediant() { Id = 0, Name = "Ingrediant 4" };
                var ingrediant5 = new Ingrediant() { Id = 0, Name = "Ingrediant 5" };

                this.Ingrediants.Add(ingrediant1);
                this.Ingrediants.Add(ingrediant2);
                this.Ingrediants.Add(ingrediant3);
                this.Ingrediants.Add(ingrediant4);
                this.Ingrediants.Add(ingrediant5);

                //-------------------- Images ---------------------------------------

                var image1 = new RecipeImage() { Id = 0, Recipe = recipe1, RecipeId = recipe1.Id, Filename = "File1.jpg", Url = "http://www.service.de/images/1" };
                var image2 = new RecipeImage() { Id = 0, Recipe = recipe1, RecipeId = recipe1.Id, Filename = "File2.jpg", Url = "http://www.service.de/images/2" };
                var image3 = new RecipeImage() { Id = 0, Recipe = recipe2, RecipeId = recipe2.Id, Filename = "File3.jpg", Url = "http://www.service.de/images/3" };

                this.Images.Add(image1);
                this.Images.Add(image2);
                this.Images.Add(image3);

                recipe1.Images.Add(image1);
                recipe1.Images.Add(image2);
                recipe2.Images.Add(image3);

                //-------------------- Tags ---------------------------------------

                var tag1 = new RecipeTag() { Id = 0, Name = "Tag 1" };
                var tag2 = new RecipeTag() { Id = 0, Name = "Tag 2" };
                var tag3 = new RecipeTag() { Id = 0, Name = "Tag 3" };
                var tag4 = new RecipeTag() { Id = 0, Name = "Tag 4" };

                this.Tags.Add(tag1);
                this.Tags.Add(tag2);
                this.Tags.Add(tag3);
                this.Tags.Add(tag4);
            }

            private async Task SetManyToManyRelationshipsAsync()
            {
                #region Recipes <-> Sources

                var recipe1 = this.Context.Recipes.First();

                var cookbookSource = this.Sources.First(x => x is RecipeCookbookSource);

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

                var recipe2 = this.Context.Recipes.Last();
                var webSource = this.Sources.First(x => x is RecipeUrlSource);

                var webSourceRecipe2Connection = new RecipeSourceRecipe()
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

                await this.Context.SaveChangesAsync();

                #endregion

                #region Recipes <-> Ingrediants

                var ingrediant1 = Ingrediants.ElementAt(0);
                var ingrediant2 = Ingrediants.ElementAt(1);
                var ingrediant3 = Ingrediants.ElementAt(2);
                var ingrediant4 = Ingrediants.ElementAt(3);
                var ingrediant5 = Ingrediants.ElementAt(4);

                recipe1.Ingrediants.Add(new RecipeIngrediant() { RecipeId = recipe1.Id, IngrediantId = ingrediant1.Id, Amount = 1, CookingUnit = CookingUnit.Gramm });
                recipe1.Ingrediants.Add(new RecipeIngrediant() { RecipeId = recipe1.Id, IngrediantId = ingrediant2.Id, Amount = 2, CookingUnit = CookingUnit.Kilogramm });

                recipe2.Ingrediants.Add(new RecipeIngrediant() { RecipeId = recipe2.Id, IngrediantId = ingrediant1.Id, Amount = 1, CookingUnit = CookingUnit.Bunch });
                recipe2.Ingrediants.Add(new RecipeIngrediant() { RecipeId = recipe2.Id, IngrediantId = ingrediant4.Id, Amount = 4, CookingUnit = CookingUnit.Liter });
                recipe2.Ingrediants.Add(new RecipeIngrediant() { RecipeId = recipe2.Id, IngrediantId = ingrediant5.Id, Amount = 5, CookingUnit = CookingUnit.Msp });
                await this.Context.SaveChangesAsync();

                #endregion

                #region Recipes <-> Tags

                var tag1 = this.Tags.ElementAt(0);
                var tag2 = this.Tags.ElementAt(1);
                var tag3 = this.Tags.ElementAt(2);

                recipe1.Tags.Add(new RecipeRecipeTag() {RecipeId = recipe1.Id, RecipeTagId = tag1.Id});
                recipe1.Tags.Add(new RecipeRecipeTag() { RecipeId = recipe1.Id, RecipeTagId = tag2.Id });
                recipe2.Tags.Add(new RecipeRecipeTag() { RecipeId = recipe2.Id, RecipeTagId = tag1.Id });
                recipe2.Tags.Add(new RecipeRecipeTag() { RecipeId = recipe2.Id, RecipeTagId = tag3.Id });
                await this.Context.SaveChangesAsync();


                #endregion

            }

            public async Task CreateTestDataAsync(bool removeAllData = false)
            {
                await Context.Database.EnsureCreatedAsync();

                var removeTransaction = await Context.Database.BeginTransactionAsync();
                try
                {
                    if (removeAllData)
                    {
                        this.Context.Recipes.RemoveRange(this.Context.Recipes);
                        this.Context.RecipeBaseSources.RemoveRange(this.Context.RecipeBaseSources);
                        this.Context.RecipeSteps.RemoveRange(this.Context.RecipeSteps);
                        this.Context.RecipeImages.RemoveRange(this.Context.RecipeImages);
                        this.Context.Ingrediants.RemoveRange(this.Context.Ingrediants);
                        this.Context.RecipeTags.RemoveRange(this.Context.RecipeTags);
                        await this.Context.SaveChangesAsync();
                    }
                    else
                    {
                        await RemoveTestDataAsync();
                    }
                    removeTransaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    removeTransaction.Rollback();
                }

                var createTransaction = await this.Context.Database.BeginTransactionAsync();
                try
                {
                    await this.Context.Recipes.AddRangeAsync(Recipes);
                    await this.Context.RecipeBaseSources.AddRangeAsync(Sources);
                    await this.Context.RecipeSteps.AddRangeAsync(Steps);
                    await this.Context.RecipeImages.AddRangeAsync(Images);
                    await this.Context.RecipeTags.AddRangeAsync(Tags);
                    await this.Context.Ingrediants.AddRangeAsync(Ingrediants);
                    await this.Context.SaveChangesAsync();

                    createTransaction.Commit();

                    this.Recipes = await Context.Recipes.AsNoTracking().ToListAsync();
                    this.Sources = await Context.RecipeBaseSources.AsNoTracking().ToListAsync();
                    this.Steps = await Context.RecipeSteps.AsNoTracking().ToListAsync();
                    this.Ingrediants = await Context.Ingrediants.AsNoTracking().ToListAsync();
                    this.Images = await Context.RecipeImages.AsNoTracking().ToListAsync();
                    this.Tags = await Context.RecipeTags.AsNoTracking().ToListAsync();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    createTransaction.Rollback();
                }

                var relationshipTransaction = await this.Context.Database.BeginTransactionAsync();
                try
                {
                    await SetManyToManyRelationshipsAsync();
                    await this.Context.SaveChangesAsync();
                    relationshipTransaction.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    relationshipTransaction.Rollback();
                }


                this.TestDataCreated = true;
            }

            public async Task RemoveTestDataAsync()
            {
                try
                {
                    this.Context.Recipes.RemoveRange(Recipes);
                    await this.Context.SaveChangesAsync();
                }
                catch (Exception) { }
                try
                {
                    this.Context.RecipeBaseSources.RemoveRange(Sources);
                    await this.Context.SaveChangesAsync();
                }
                catch (Exception) { }
                try
                {
                    this.Context.RecipeSteps.RemoveRange(Steps);
                    await this.Context.SaveChangesAsync();
                }
                catch (Exception) { }
                try
                {
                    this.Context.Ingrediants.RemoveRange(Ingrediants);
                    await this.Context.SaveChangesAsync();
                }
                catch (Exception) { }
                try
                {
                    this.Context.RecipeImages.RemoveRange(Images);
                    await this.Context.SaveChangesAsync();
                }
                catch (Exception) { }
                try
                {
                    this.Context.RecipeTags.RemoveRange(Tags);
                    await this.Context.SaveChangesAsync();
                }
                catch (Exception) { }
                this.TestDataCreated = false;
            }



            public void RemoveTestData()
            {
                try
                {
                    this.Context.Recipes.RemoveRange(Recipes);
                    this.Context.SaveChanges();
                }
                catch (Exception) { }
                try
                {
                    this.Context.RecipeBaseSources.RemoveRange(Sources);
                    this.Context.SaveChanges();
                }
                catch (Exception) { }
                try
                {
                    this.Context.RecipeSteps.RemoveRange(Steps);
                    this.Context.SaveChanges();
                }
                catch (Exception) { }
                try
                {
                    this.Context.Ingrediants.RemoveRange(Ingrediants);
                    this.Context.SaveChanges();
                }
                catch (Exception) { }
                try
                {
                    this.Context.RecipeImages.RemoveRange(Images);
                    this.Context.SaveChanges();
                }
                catch (Exception) { }
                try
                {
                    this.Context.RecipeTags.RemoveRange(Tags);
                    this.Context.SaveChanges();
                }
                catch (Exception) { }

                this.TestDataCreated = false;
            }
            #endregion

            #region PROPERTIES
            public ICollection<Recipe> Recipes { get; private set; }
            public ICollection<RecipeBaseSource> Sources { get; private set; }
            public ICollection<RecipeStep> Steps { get; private set; }
            public ICollection<Ingrediant> Ingrediants { get; private set; }
            public ICollection<RecipeImage> Images { get; private set; }
            public ICollection<RecipeTag> Tags { get; private set; }
            public RecipeDbContext Context { get; private set; }

            public bool TestDataCreated { get; private set; }

            #endregion

        }

        #endregion
    }
}

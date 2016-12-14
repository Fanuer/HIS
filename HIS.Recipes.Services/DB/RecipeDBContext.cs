using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
        }

        public RecipeDbContext()
        {
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

            modelBuilder.Entity<RecipeTag>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Ingrediant>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<RecipeSourceRecipe>().HasIndex(x => new { x.RecipeId, x.SourceId, x.Page }).IsUnique();
            modelBuilder.Entity<RecipeStep>().HasIndex(x => new { x.RecipeId, x.Order }).IsUnique();

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

        #endregion
    }
}

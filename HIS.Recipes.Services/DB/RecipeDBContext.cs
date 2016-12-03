using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace HIS.Recipes.Services.DB
{
    internal class RecipeDBContext:DbContext
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public RecipeDBContext(DbContextOptions options) : base(options)
        {
            
        }

        public RecipeDBContext()
        {
        }
        #endregion

        #region METHODS

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngrediant>().HasKey(x => new {x.RecipeId, x.IngrediantId});
            modelBuilder.Entity<RecipeSourceRecipe>().HasKey(x => new {x.RecipeId, x.SourceId});
            modelBuilder.Entity<RecipeRecipeTag>().HasKey(x => new {x.RecipeId, x.RecipeTagId});

            modelBuilder.Entity<Recipe>().HasMany(x => x.Images).WithOne(x=>x.Recipe);
            modelBuilder.Entity<Recipe>().HasMany(x => x.Steps).WithOne(x=>x.Recipe);

            modelBuilder.Entity<RecipeTag>().HasAlternateKey(x => x.Name);
            modelBuilder.Entity<Ingrediant>().HasAlternateKey(x => x.Name);
            modelBuilder.Entity<RecipeSourceRecipe>().HasAlternateKey(x => new { x.RecipeId, x.SourceId, x.Page});
            modelBuilder.Entity<RecipeStep>().HasAlternateKey(x => new { x.RecipeId, x.Order});

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
        internal DbSet<RecipeImage> RecipeImages { get; set; }
        internal DbSet<RecipeIngrediant> RecipeIngrediants { get; set; }
        internal DbSet<RecipeSourceRecipe> RecipeSourceRecipes { get; set; }
        internal DbSet<RecipeStep> RecipeSteps { get; set; }
        internal DbSet<RecipeTag> RecipeTags { get; set; }
        internal DbSet<RecipeUrlSource> RecipeUrlSources { get; set; }

        #endregion
    }
}

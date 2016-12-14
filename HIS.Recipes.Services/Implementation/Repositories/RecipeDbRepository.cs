using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.MSSql;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace HIS.Recipes.Services.Implementation.Repositories
{
    internal class RecipeDbRepository: IRecipeDBRepository
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public RecipeDbRepository(
            IDbImageRepository images, 
            IIngrediantRepository ingrediants, 
            IRecipeRepository recipes,
            ISourceRepository<RecipeUrlSource> urlSources,
            ISourceRepository<RecipeCookbookSource> cookbookSources,
            IStepRepository steps,
            ITagsRepository tags
            )
        {
            this.Images = images;
            this.Ingrediants = ingrediants;
            this.Recipes = recipes;
            this.UrlSources = urlSources;
            this.CookBookSources = cookbookSources;
            this.Steps = steps;
            this.Tags = tags;
        }


        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        public IDbImageRepository Images { get; }
        public IIngrediantRepository Ingrediants { get; }
        public IRecipeRepository Recipes { get; }

        public ISourceRepository<RecipeCookbookSource> CookBookSources { get; }

        public ISourceRepository<RecipeUrlSource> UrlSources { get; }
        public IStepRepository Steps { get; }
        public ITagsRepository Tags { get; }

        #endregion

        #region Nested Classes

        internal class DbImageRepository : GenericDbRepository<RecipeImage, int>, IDbImageRepository
        {
            public DbImageRepository(RecipeDbContext context): base(context){}
        }
        internal class IngrediantRepository : GenericDbRepository<Ingrediant, int>, IIngrediantRepository
        {
            public IngrediantRepository(RecipeDbContext context) : base(context) { }
        }
        internal class RecipeRepository : GenericDbRepository<Recipe, int>, IRecipeRepository
        {
            public RecipeRepository(RecipeDbContext context) : base(context) { }
        }
        internal class StepRepository : GenericDbRepository<RecipeStep, int>, IStepRepository
        {
            public StepRepository(RecipeDbContext context) : base(context) { }
            public async Task UpdateAllAsync(int recipeId, ICollection<string> entries)
            {
                if (entries == null) { throw new ArgumentNullException(nameof(entries));}
                var recipe = await DbContext.Set<Recipe>().Include(x=>x.Steps).FirstOrDefaultAsync();
                if (recipe == null) { throw new DataObjectNotFoundException($"No recipe with the given id '{recipeId}'found"); }

                if (entries.Any()) 
                {
                    recipe.Steps.Clear();
                    for (int i = 0; i < entries.Count(); i++)
                    {
                        var entry = entries.ElementAt(i);
                        recipe.Steps.Add(new RecipeStep() {Order = i+1, Description = entry, RecipeId = recipeId});
                    }
                    await SaveChangesAsync();
                }
            }
        }
        internal class CookbookSourceRepository : GenericDbRepository<RecipeCookbookSource, int>, ICookbookSourceRepository
        {
            public CookbookSourceRepository(RecipeDbContext context) : base(context) { }
        }
        internal class WebSourceRepository : GenericDbRepository<RecipeUrlSource, int>, IWebSourceRepository
        {
            public WebSourceRepository(RecipeDbContext context) : base(context) { }
        }
        internal class BaseSourceRepository : GenericDbRepository<RecipeBaseSource, int>, IBaseSourceRepository
        {
            public BaseSourceRepository(RecipeDbContext context) : base(context) { }
        }


        internal class TagsRepository : GenericDbRepository<RecipeTag, int>, ITagsRepository
        {
            public TagsRepository(RecipeDbContext context) : base(context) { }
        }

        #endregion
    }
}

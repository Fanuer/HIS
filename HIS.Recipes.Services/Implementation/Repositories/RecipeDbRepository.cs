using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.MSSql;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Models;

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

        internal class DbImageRepository : GenericDbRepository<RecipeImage, Guid>, IDbImageRepository
        {
            public DbImageRepository(RecipeDBContext context): base(context){}
        }
        internal class IngrediantRepository : GenericDbRepository<Ingrediant, Guid>, IIngrediantRepository
        {
            public IngrediantRepository(RecipeDBContext context) : base(context) { }
        }
        internal class RecipeRepository : GenericDbRepository<Recipe, Guid>, IRecipeRepository
        {
            public RecipeRepository(RecipeDBContext context) : base(context) { }
        }
        internal class StepRepository : GenericDbRepository<RecipeStep, Guid>, IStepRepository
        {
            public StepRepository(RecipeDBContext context) : base(context) { }
        }
        internal class CookbookSourceRepository : GenericDbRepository<RecipeCookbookSource, Guid>, ICookbookSourceRepository
        {
            public CookbookSourceRepository(RecipeDBContext context) : base(context) { }
        }
        internal class WebSourceRepository : GenericDbRepository<RecipeUrlSource, Guid>, IWebSourceRepository
        {
            public WebSourceRepository(RecipeDBContext context) : base(context) { }
        }
        internal class BaseSourceRepository : GenericDbRepository<RecipeBaseSource, Guid>, IBaseSourceRepository
        {
            public BaseSourceRepository(RecipeDBContext context) : base(context) { }
        }


        internal class TagsRepository : GenericDbRepository<RecipeTag, Guid>, ITagsRepository
        {
            public TagsRepository(RecipeDBContext context) : base(context) { }
        }

        #endregion
    }
}

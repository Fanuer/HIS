using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HIS.Data.Base.Interfaces.Models;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HIS.Helpers.Extensions.FuzzyString;
using Microsoft.EntityFrameworkCore.Query.Internal;
using HIS.Helpers.Extensions;

namespace HIS.Recipes.Services.Implementation.Services
{
    internal class RecipeService : BaseService<IRecipeRepository, Recipe, RecipeUpdateViewModel, RecipeCreationViewModel>, IRecipeService
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public RecipeService(IRecipeRepository rep, IMapper mapper, ILoggerFactory loggerfactory)
            : base(rep, mapper, loggerfactory.CreateLogger<RecipeService>(), "recipe")
        {
        }
        #endregion

        #region METHODS
        public async Task<IQueryable<ShortRecipeViewModel>> SearchForRecipes(RecipeSearchViewModel searchModel)
        {
            IQueryable<ShortRecipeViewModel> result = null;
            try
            {
                var recipes = this.Repository
                                    .GetAll()
                                    .Include(x => x.Images)
                                    .Include(x => x.Tags)
                                        .ThenInclude(x => x.RecipeTag)
                                    .Include(x => x.Ingrediants)
                                        .ThenInclude(x => x.Ingrediant);

                var searchresult = await this.SearchForRecipes(recipes, searchModel);

                result = searchresult
                            .OrderByDescending(x => x.CookedCounter)
                                .ThenByDescending(x => x.LastTimeCooked)
                            .ProjectTo<ShortRecipeViewModel>(this.Mapper.ConfigurationProvider);

                this.Logger.LogDebug(new EventId(), $"Returned all recipes");
            }
            catch (Exception e)
            {
                this.Logger.LogError(new EventId(), e, $"Error on receiving all recipes");
                throw new Exception($"Error on receiving all recipes");
            }
            return result;
        }

        public IQueryable<ShortRecipeViewModel> GetRecipes()
        {
            IQueryable<ShortRecipeViewModel> result = null;
            try
            {
                result = this.Repository
                                    .GetAll()
                                    .Include(x => x.Images)
                                    .Include(x => x.Tags)
                                        .ThenInclude(x => x.RecipeTag)
                                    .OrderByDescending(x => x.CookedCounter)
                                        .ThenByDescending(x => x.LastTimeCooked)
                                    .ProjectTo<ShortRecipeViewModel>(this.Mapper.ConfigurationProvider);

                this.Logger.LogDebug(new EventId(), $"Returned all recipes");
            }
            catch (Exception e)
            {
                this.Logger.LogError(new EventId(), e, $"Error on receiving all recipes");
                throw new Exception($"Error on receiving all recipes");
            }
            return result;
        }


        private async Task<IQueryable<Recipe>> SearchForRecipes(IQueryable<Recipe> recipes, RecipeSearchViewModel searchModel)
        {
            if (recipes == null) { throw new ArgumentNullException(nameof(recipes)); }

            if (searchModel == null) { return recipes; }

            if (!String.IsNullOrWhiteSpace(searchModel.Name))
            {
                recipes = recipes.Where(x => x != null && x.Name != null && x.Name.ToLower().Contains(searchModel.Name.ToLower()));
            }
            if (searchModel.Tags != null && searchModel.Tags.Any())
            {
                var hits = await GetRecipesForTagSearchQuery(recipes, searchModel);
                recipes = recipes.Where(x => x.Tags.Any(tag => hits.Contains(tag.RecipeTagId)));
            }
            if (searchModel.Ingrediants != null && searchModel.Ingrediants.Any())
            {
                var hits = await GetRecipesForIngrediantsSearchQuery(recipes, searchModel);
                recipes = recipes.Where(x => x.Ingrediants.Any(ingrediant => hits.Contains(ingrediant.IngrediantId)));
            }
            return recipes;
        }

        private async Task<IEnumerable<int>> GetRecipesForTagSearchQuery(IQueryable<Recipe> recipes, RecipeSearchViewModel model)
        {
            Dictionary<string, int> allEntries = null;
            IList<int> hits = new List<int>();
            foreach (var searchModelTag in model.Tags)
            {
                // check if tag are found within database
                var currentHit = await recipes.Where(x => x.Tags.Any(recipeTag => recipeTag.RecipeTag.Name.Contains(searchModelTag, true))).Select(x=>x.Id).FirstOrDefaultAsync();
                if (currentHit != default(int))
                {
                    hits.Add(currentHit);
                    continue;
                }

                // if no hits, lookup for fuzzy search cache for search query
                currentHit = await Repository.GetCachedFuzzyResultAsync(nameof(RecipeTag), searchModelTag);
                if (currentHit != default(int))
                {
                    hits.Add(currentHit);
                    continue;
                }

                // If no hits available, load all elements and process a fuzzy search on them 
                // TODO: Change to 'real' caching (e.g. Lucene)
                if (allEntries == null)
                {
                    allEntries = recipes.SelectMany(x => x.Tags).ToDictionary(x => x.RecipeTag.Name, x => x.RecipeTagId);
                }

                var hit = allEntries.Keys.FirstOrDefault(x => x.ApproximatelyEquals(searchModelTag, FuzzyStringComparisonTolerance.Normal, FuzzyStringComparisonOptions.UseLevenshteinDistance));
                // if hit is found, save to cache for reuse
                if (hit != null)
                {
                    var newEntry = new FuzzyEntry() { Id = allEntries[hit], SearchQuery = searchModelTag, Type = nameof(RecipeTag) };
                    await Repository.SaveFuzzyEntryAsync(newEntry);
                    hits.Add(allEntries[hit]);
                }
            }
            return hits.Distinct();
        }

        private async Task<IEnumerable<int>> GetRecipesForIngrediantsSearchQuery(IQueryable<Recipe> recipes, RecipeSearchViewModel model)
        {
            Dictionary<string, int> allEntries = null;
            IList<int> hits = new List<int>();
            foreach (var searchIngrediant in model.Ingrediants)
            {
                // check if tag are found within database
                var currentHit = await recipes.Where(x => x.Ingrediants.Any(ingrediant => ingrediant.Ingrediant.Name.Contains(searchIngrediant, true))).Select(x => x.Id).FirstOrDefaultAsync();
                if (currentHit != default(int))
                {
                    hits.Add(currentHit);
                    continue;
                }

                // if no hits, lookup for fuzzy search cache for search query
                currentHit = await Repository.GetCachedFuzzyResultAsync(nameof(RecipeTag), searchIngrediant);
                if (currentHit != default(int))
                {
                    hits.Add(currentHit);
                    continue;
                }

                // If no hits available, load all elements and process a fuzzy search on them 
                // TODO: Change to 'real' caching (e.g. Lucene)
                if (allEntries == null)
                {
                    allEntries = recipes.SelectMany(x => x.Ingrediants).ToDictionary(x => x.Ingrediant.Name, x => x.IngrediantId);
                }

                var hit = allEntries.Keys.FirstOrDefault(x => x.ApproximatelyEquals(searchIngrediant, FuzzyStringComparisonTolerance.Normal, FuzzyStringComparisonOptions.UseLevenshteinDistance));
                // if hit is found, save to cache for reuse
                if (hit != null)
                {
                    var newEntry = new FuzzyEntry() { Id = allEntries[hit], SearchQuery = searchIngrediant, Type = nameof(RecipeTag) };
                    await Repository.SaveFuzzyEntryAsync(newEntry);
                    hits.Add(allEntries[hit]);
                }
            }
            return hits.Distinct();
        }


        public async Task<FullRecipeViewModel> GetRecipeAsync(int recipeId)
        {
            FullRecipeViewModel result = null;
            try
            {
                result = await this.Repository
                                   .GetAll()
                                   .Include(x => x.Images)
                                   .Include(x => x.Tags)
                                        .ThenInclude(x => x.RecipeTag)
                                   .Include(x => x.Steps)
                                   .Include(x => x.Ingrediants)
                                        .ThenInclude(x => x.Ingrediant)
                                   .Include(x => x.Source)
                                        .ThenInclude(x => x.Source)
                                   .ProjectTo<FullRecipeViewModel>(this.Mapper.ConfigurationProvider)
                                   .SingleOrDefaultAsync(x => x.Id.Equals(recipeId));

                if (result == null)
                {
                    throw new DataObjectNotFoundException("No Recipe with the given id found");
                }

                result.Steps = result.Steps.OrderBy(x => x.Order);
                result.Images = result.Images.OrderBy(x => x.Id);
                result.Tags = result.Tags.OrderBy(x => x.Name);


                this.Logger.LogDebug(new EventId(), $"Returned recipe '{result.Name} ({result.Id})'");
            }
            catch (Exception e)
            {
                this.Logger.LogError(new EventId(), e, $"Error on receiving recipe '{recipeId}'");
                throw new Exception($"Error on receiving recipe '{recipeId}'");
            }
            return result;
        }

        public async Task CookNowAsync(int recipeId)
        {
            var recipe = await this.Repository.FindAsync(recipeId);
            if (recipe == null)
            {
                throw new DataObjectNotFoundException($"No recipe with the given no '{recipeId}' found");
            }
            recipe.CookedCounter++;
            recipe.LastTimeCooked = DateTime.UtcNow;
            await this.Repository.SaveChangesAsync();
        }
        #endregion

        #region PROPERTIES
        #endregion
    }
}

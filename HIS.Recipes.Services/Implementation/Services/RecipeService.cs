using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HIS.Recipes.Services.Implementation.Services
{
    internal class RecipeService:BaseService<IRecipeRepository, Recipe, RecipeUpdateViewModel, RecipeCreationViewModel>, IRecipeService
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
        public IQueryable<ShortRecipeViewModel> GetRecipes()
        {
            IQueryable<ShortRecipeViewModel> result = null;
            try
            {
                result = this.Repository
                                .GetAll()
                                .Include(x=>x.Images)
                                .Include(x=>x.Tags)
                                    .ThenInclude(x=>x.RecipeTag)
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
                                   .Include(x=>x.Steps)
                                   .Include(x=>x.Ingrediants)
                                        .ThenInclude(x=>x.Ingrediant)
                                   .Include(x=>x.Source)
                                        .ThenInclude(x=>x.Source)
                                   .ProjectTo<FullRecipeViewModel>(this.Mapper.ConfigurationProvider)
                                   .SingleOrDefaultAsync();

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
            if(recipe == null)
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

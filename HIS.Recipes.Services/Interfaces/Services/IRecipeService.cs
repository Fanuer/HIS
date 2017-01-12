using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.Services.Interfaces.Services
{
    public interface IRecipeService: IDisposable
    {
        /// <summary>
        /// Retunrs a collection of recipes
        /// </summary>
        /// <returns></returns>
        IQueryable<ShortRecipeViewModel> GetRecipes(RecipeSearchViewModel searchModel = null);
        /// <summary>
        /// Returns all data of a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task<FullRecipeViewModel> GetRecipeAsync(int recipeId);

        /// <summary>
        /// Marks a recipe as recently cooked
        /// </summary>
        /// <param name="recipeId">recipe id</param>
        /// <returns></returns>
        Task CookNowAsync(int recipeId);
        /// <summary>
        /// Updates a database entity with the given data from a view model
        /// </summary>
        /// <param name="id">Database id</param>
        /// <param name="model">New Data</param>
        /// <returns></returns>
        Task UpdateAsync(int id, RecipeUpdateViewModel model);
        /// <summary>
        /// Creates a new entity in the Database
        /// </summary>
        /// <param name="creationModel">entity data</param>
        /// <returns></returns>
        Task<RecipeUpdateViewModel> AddAsync(RecipeCreationViewModel creationModel);
        /// <summary>
        /// Deletes an entity from the Database
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        Task RemoveAsync(int id);
    }
}

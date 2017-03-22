using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.WebApi.Client.Services.Interfaces
{
    public interface IIngrediantsService : IDisposable
    {
        /// <summary>
        /// Get all Ingrediants from the recipe api
        /// </summary>
        /// <param name="searchterm">term to filter the ingrediants</param>
        /// <param name="page">0-based Page for Pagination</param>
        /// <param name="entriesPerPage">Entries per Page. Used for Pagination</param>
        /// <returns></returns>
        Task<ListViewModel<IngrediantStatisticViewModel>> GetAllIngrediantsAsync(string searchterm = "", int page = 0, int entriesPerPage = 10);

        /// <summary>
        /// Creates a new Ingrediants
        /// </summary>
        /// <param name="ingrediantName">Name of the Ingrediant</param>
        /// <returns></returns>
        Task<IngrediantViewModel> AddIngrediantsAsync(string ingrediantName);

        /// <summary>
        /// Removes an Ingrediants
        /// </summary>
        /// <param name="ingrediantId">ID of an ingrediant</param>
        /// <returns></returns>
        Task DeleteIngrediantAsync(int ingrediantId);

        /// <summary>
        /// Updates an Ingrediant
        /// </summary>
        /// <param name="ingrediantId">Id of the ingrediant to update</param>
        /// <param name="updateModel">new ingrediant data</param>
        /// <returns></returns>
        Task UpdateIngrediantAsync(int ingrediantId, NamedViewModel updateModel);

        /// <summary>
        /// Returns all Ingrediants of a Recipe
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <returns></returns>
        Task<IEnumerable<IngrediantViewModel>> GetIngrediantsForRecipe(int recipeId);

        /// <summary>
        /// Adds or updates an ingrediant of a recipe. If the ingrediant with the given id is not available within the recipe it will be addded. 
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <param name="model">new or updated data</param>
        /// <returns></returns>
        Task AddOrUpdateIngrediantOfRecipe(int recipeId, AlterIngrediantViewModel model);

        /// <summary>
        /// Removes an ingrediant from a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <param name="ingrediantId">id of an ingrediant</param>
        /// <returns></returns>
        Task RemoveIngrediantFromRecipeAsync(int recipeId, int ingrediantId);
    }
}